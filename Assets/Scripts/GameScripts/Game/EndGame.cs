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

            if (CountLive != 0) 
            {
                //включаем 2 звезду
                SecondStar.GetComponent<SpriteRenderer>().sprite = ActiveStar;
                NowProgress += 1;

                if (CountLive != 0)
                {
                    //включаем 3 звезду
                    ThirdStar.GetComponent<SpriteRenderer>().sprite = ActiveStar;
                    NowProgress += 1;
                }
            }
        }
        UpdateProgress(NowProgress);
        NowProgress = 0;
    }

    public void UpdateProgress(int Stars)
    {
        var link_on_local_data_base = gameObject.GetComponent<Connection>();
        var link_on_server_data_base = gameObject.GetComponent<Connection_on_MySQL>();
    }
}
