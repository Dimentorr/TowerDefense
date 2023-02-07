using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EndGame : MonoBehaviour
{
    public GameObject EndGameCanvas;
    public GameObject GameCanvas;
    public GameObject PauseCanvas;
    public bool End = false;
    public Text EndText;
    public string LastMassage;

    public Sprite ActiveStar;
    public GameObject FirstStar;
    public GameObject SecondStar;
    public GameObject ThirdStar;

    private void Update()
    {
        if (End)
        {
            GameCanvas.SetActive(false);
            PauseCanvas.SetActive(false);
            EndGameCanvas.SetActive(true);

            EndText.text = LastMassage;
            SetStars();
            Time.timeScale = 0;
        }
    }

    public void SetStars()
    {
        var CountLive = Convert.ToInt32(gameObject.GetComponent<CreateWorld>().CountHP.text);
        var NowProgress = 0;
        if (CountLive != 0)
        {
            //включаем 1 звезду
            FirstStar.GetComponent<SpriteRenderer>().sprite = ActiveStar;
            NowProgress += 1;

            if (CountLive >= 5) 
            {
                //включаем 2 звезду
                SecondStar.GetComponent<SpriteRenderer>().sprite = ActiveStar;
                NowProgress += 1;

                if (CountLive == 10)
                {
                    //включаем 3 звезду
                    ThirdStar.GetComponent<SpriteRenderer>().sprite = ActiveStar;
                    NowProgress += 1;
                }
            }
        }
        UpdateProgress(1, NowProgress);
        NowProgress = 0;
    }

    public void UpdateProgress(int Idlevel = 1, int Stars = 3)
    {
        var link_on_local_data_base = gameObject.GetComponent<Connection>();
        var link_on_server_data_base = gameObject.GetComponent<Connection_on_MySQL>();

        var NamePlayer = link_on_local_data_base.Select_User().Split(',')[1];
        try
        {
            link_on_local_data_base.UpdateTable_Progress(Idlevel, Stars);
            var userId = link_on_server_data_base.Reader(link_on_server_data_base.SqlSelect("users", $"name = '{ NamePlayer }'"))[0];
            link_on_server_data_base.SqlUpdate("progressPlayers", new List<string>() {"playerId", "levelId","stars" }, new List<string>() { userId, Idlevel.ToString(), Stars.ToString() },
                $"levelId = {Idlevel} AND playerId = (SELECT id FROM users Where name = '{NamePlayer}')");
        }
        catch(Exception err)
        {
            Debug.Log(err.ToString());
            int max_id = 0;
            link_on_local_data_base.InsertData("progress", new List<string>() { 1.ToString(), Idlevel.ToString(), Stars.ToString() });
            try
            {
                List<string> ids = new List<string>();
                foreach (var i in link_on_server_data_base.Reader(link_on_server_data_base.SqlSelect("progressPlayers")))
                {
                    ids.Add(i[0].ToString());
                }
                max_id = Convert.ToInt32(ids.Count);
                if (max_id == 0)
                {
                    max_id = 1;
                }
            }
            catch (Exception erro)
            {
                Debug.Log(erro.ToString());
            }
            var userId = link_on_server_data_base.Reader(link_on_server_data_base.SqlSelect("users", $"name = '{ NamePlayer }'"))[0];
            link_on_server_data_base.SqlInsert("progressPlayers", new List<string>() { max_id.ToString(), userId, Idlevel.ToString(), Stars.ToString() } );
        }
    }
}
