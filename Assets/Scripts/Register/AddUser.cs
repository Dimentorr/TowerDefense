using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Linq;

public class AddUser : MonoBehaviour
{
    public Text user_name;
    public Text password;
    public Text repeat_password;

    public void Start()
    {
        RegUser();
    }
    public void RegUser()
    {
        var object_data_base = gameObject.GetComponent<Connection_on_MySQL>();
        if (user_name.text != "" && password.text != "" && repeat_password.text != "")
        {
            if (password.text == repeat_password.text)
            {
                List<string> ids = new List<string>();
                foreach (var i in object_data_base.Reader(object_data_base.SqlSelect("users")))
                {
                    ids.Add(i[0].ToString());
                }
                int max_id = Convert.ToInt32(ids[ids.Count() - 1]);

                object_data_base.SqlInsert("users", new List<string>() { $"{Convert.ToInt32(max_id) + 1}", user_name.text, password.text});
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
}
