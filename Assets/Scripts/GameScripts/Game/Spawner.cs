using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Text CountGold;
    public GameObject Enemy;
    public bool isSpawn = true;
    public float Interval = 2f;//время между мобам
    public int MaxMobs = 10;
    public int NowCountMobs = -1;
    public List<string> NCorners = new List<string>();
    public int Wave = 2; // колличество волн
    public int NowWave = 1; // текущая волна
    public int CountKills;
    public GameObject controller;


    public GameObject PlaceSpawnEnemy;
    public void Start()
    {
        StartCoroutine(WaitSpawn(Interval, Enemy));
    }

    public IEnumerator WaitSpawn(float waitTime, GameObject enemy)
    {
        while (isSpawn && NowWave <= Wave)
        {
            NowCountMobs++;
            if (NowCountMobs > 0)
            {
                if (NowCountMobs >= MaxMobs)
                {
                    NowWave += 1;
                    NowCountMobs = 0;
                    isSpawn = false;
                }
                GameObject tmp = Instantiate(enemy);
                tmp.GetComponent<ParamsEnemy>().CountGold = CountGold;
                tmp.transform.position = PlaceSpawnEnemy.transform.position;
                tmp.GetComponent<ParamsEnemy>().CountGold = CountGold;
                tmp.GetComponent<Move>().NCorners = NCorners;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
