using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBullets : MonoBehaviour
{
    public GameObject controller;
    public float interval_shoot;
    private float interval;

    public GameObject bull;
    public GameObject tower;
    void Start()
    {
        interval = interval_shoot;
    }

    public void Update()
    {
        if (tower.GetComponent<FindEnemies>().enemy.Count != 0)
        {
            Timer();
        }
    }

    public void Timer()
    {
        interval -= Time.deltaTime;
        if (interval <= 0)
        {
            GameObject tmp = Instantiate(bull);
            tmp.transform.position = tower.transform.position;
            tmp.gameObject.GetComponent<MoveBullet>().target = tower.GetComponent<FindEnemies>().enemy[0];
            tmp.GetComponent<Damage>().controller = controller;
            interval = interval_shoot;
        }
    }
}
