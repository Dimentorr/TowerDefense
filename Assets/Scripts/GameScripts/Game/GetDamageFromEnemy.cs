using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetDamageFromEnemy : MonoBehaviour
{
    public GameObject controller;
    public Text CountHP;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            controller.GetComponent<CountKills>().CountKillsNow += 1;
            if (controller.GetComponent<CountKills>().CountKillsNow % controller.GetComponent<CreateWorld>().PlaceSpawn.GetComponent<Spawner>().MaxMobs == 0)
            {
                controller.GetComponent<CreateWorld>().PlaceSpawn.GetComponent<Spawner>().isSpawn = true;
                controller.GetComponent<CreateWorld>().PlaceSpawn.GetComponent<Spawner>().Start();
            }
            CountHP.text = $"{int.Parse(CountHP.text) - other.gameObject.GetComponent<ParamsEnemy>().Damage}";
            Destroy(other.gameObject);
            if (int.Parse(CountHP.text) <= 0)
            {
                controller.GetComponent<EndGame>().End = true;
                controller.GetComponent<EndGame>().LastMassage = "You lose!";
            }
        }
    }
}
