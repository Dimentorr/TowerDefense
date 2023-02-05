using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Linq;

public class RegLog : MonoBehaviour
{
    public Text user_name_reg;
    public Text password_reg;
    public Text repeat_password_reg;

    public Text user_name_log;
    public Text password_log;
    public void LogIn()
    {
        var object_data_base = gameObject.GetComponent<Connection_on_MySQL>();
        var object_data_base_local = gameObject.GetComponent<Connection>();
        try
        {
            var data_check = (object_data_base_local.Select_User()).Split(',');
            if (data_check[1] == user_name_log.text && data_check[2] == password_log.text)
            {
                CheckTableLevels();
                SceneManager.LoadScene(1);
            }
            else
            {
                var data_user = object_data_base.Reader(object_data_base.SqlSelect("users", $"name = '{user_name_log.text}'"))[0].Split(',');

                if (data_user[1] == user_name_log.text && data_user[2] == password_log.text)
                {
                    object_data_base_local.DeleteTable_User();
                    foreach (var i in data_user)
                    {
                        Debug.Log(i);
                    }
                    List<string> new_user_data = new List<string>() { data_user[1], data_user[2], data_user[3], data_user[4] };

                    object_data_base_local.InsertData("user", new_user_data);

                    // проверка на заполненность таблицы уровней
                    CheckTableLevels();
                    // проверка на заполненность таблицы прогресса игрока
                    CheckTableProgress();
                    SceneManager.LoadScene(1);
                }
            }
        }
        catch
        {
            var data_user = object_data_base.Reader(object_data_base.SqlSelect("users", $"name = '{user_name_log.text}'"))[0].Split(',');

            if (data_user[1] == user_name_log.text && data_user[2] == password_log.text)
            {
                object_data_base_local.DeleteTable_User();

                List<string> new_user_data = new List<string>() { data_user[1], data_user[2], data_user[3], data_user[4] };

                object_data_base_local.InsertData("user", new_user_data);

                // проверка на заполненность таблицы уровней
                CheckTableLevels();
                // проверка на заполненность таблицы прогресса игрока
                CheckTableProgress();
                SceneManager.LoadScene(1);
            }
        }
    }

    public void RegUser()
    {
        var object_data_base = gameObject.GetComponent<Connection_on_MySQL>();
        var object_data_base_local = gameObject.GetComponent<Connection>();
        if (user_name_reg.text != "" && password_reg.text != "" && repeat_password_reg.text != "")
        {
            if (password_reg.text == repeat_password_reg.text)
            {
                List<string> ids = new List<string>();
                foreach (var i in object_data_base.Reader(object_data_base.SqlSelect("users")))
                {
                    ids.Add(i[0].ToString());
                }
                int max_id = Convert.ToInt32(ids[ids.Count() - 1]);

                object_data_base.SqlInsert("users", new List<string>() { $"{Convert.ToInt32(max_id) + 1}", user_name_reg.text, password_reg.text, "player", "0"});
                object_data_base_local.DeleteTable_User();
                List<string> new_user_data = new List<string>() { user_name_reg.text, password_reg.text, "player", "0" };
                object_data_base_local.InsertData("user", new_user_data);

                // проверка на заполненность таблицы уровней
                CheckTableLevels();
                SceneManager.LoadScene(1);
            }
            else
            {
                Debug.Log("Password not match!");
            }
        }
        else
        {
            Debug.Log("Fill all fields!");
        }
    }

    public void CheckTableLevels()
    {
        var object_data_base = gameObject.GetComponent<Connection_on_MySQL>();
        var object_data_base_local = gameObject.GetComponent<Connection>();
        try
        {
            var check = object_data_base_local.db.Table<levels>().FirstOrDefault().ToString();

        }
        catch (NullReferenceException)
        {
            var ALL_DATA_LEVELS = object_data_base.Reader(object_data_base.SqlSelect("levels"));

            for (int i = 0; i < ALL_DATA_LEVELS.Count; i++)
            {
                var row = ALL_DATA_LEVELS[i].Split(',');

                List<string> level_data = new List<string>() { row[0], row[1], row[2] };

                object_data_base_local.InsertData("levels", level_data);
                level_data.Clear();
            }
        }
    }

    public void CheckTableProgress()
    {
        var object_data_base = gameObject.GetComponent<Connection_on_MySQL>();
        var object_data_base_local = gameObject.GetComponent<Connection>();
        try
        {
            var check = object_data_base_local.db.Table<progress>().FirstOrDefault().ToString();

        }
        catch (NullReferenceException)
        {
            try
            {
                var ALL_DATA_LEVELS = object_data_base.Reader(object_data_base.SqlSelect("progressPlayers"));

                for (int i = 0; i < ALL_DATA_LEVELS.Count; i++)
                {
                    var row = ALL_DATA_LEVELS[i].Split(',');

                    List<string> progress_data = new List<string>() { row[0], row[3] };

                    object_data_base_local.InsertData("progress", progress_data);
                    progress_data.Clear();
                }
            }
            catch
            {
                Debug.Log("No data about progress this player");
            }
        }
    }
}
