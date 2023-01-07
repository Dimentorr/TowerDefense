using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public GameObject EndGameCanvas;
    public GameObject GameCanvas;
    public GameObject PauseCanvas;
    public bool End = false;
    public Text EndText;
    public string LastMassage;

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
}
