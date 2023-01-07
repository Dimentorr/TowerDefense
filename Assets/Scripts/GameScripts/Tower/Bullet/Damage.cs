using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damage : MonoBehaviour
{
    private int damage = 10;
    public GameObject controller;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<ParamsEnemy>().HP -= damage;
            if (other.gameObject.GetComponent<ParamsEnemy>().HP <= 0)
            {
                controller.GetComponent<CountKills>().CountKillsNow += 1;
                Destroy(other);
                if (controller.GetComponent<CountKills>().CountKillsNow % controller.GetComponent<CreateWorld>().PlaceSpawn.GetComponent<Spawner>().MaxMobs == 0)
                {
                    var Spawner = controller.GetComponent<CreateWorld>().PlaceSpawn.GetComponent<Spawner>();
                    Spawner.isSpawn = true;
                    controller.GetComponent<CreateWorld>().PlaceSpawn.GetComponent<Spawner>().Start();
                    int NWave = Spawner.NowWave;
                    int Wave = Spawner.Wave;
                    if (NWave - 1 >= Wave) // так как нас интерисует кол-во уже случившихся волн, а не текущий номер волны
                    {
                        controller.GetComponent<EndGame>().End = true;
                        controller.GetComponent<EndGame>().LastMassage = "You won!";
                    }
                }
            }
        }
    }
}
