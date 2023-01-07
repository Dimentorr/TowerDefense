using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject GameUI;
    private bool paused;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SetPause();
        }
    }

    public void SetPause()
    {
        if (!paused)
        {
            PauseUI.SetActive(true);
            GameUI.SetActive(false);
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            PauseUI.SetActive(false);
            GameUI.SetActive(true);
            Time.timeScale = 1;
            paused = false;
        }
    }
}
