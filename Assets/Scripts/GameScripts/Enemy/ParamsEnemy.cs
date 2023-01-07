using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamsEnemy : MonoBehaviour
{
    public int HP;
    public int Damage;
    public float SpeedMove;
    public Text CountGold;
    public int Gold = 25;

    private void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            CountGold.text = $"{int.Parse(CountGold.text) + Gold}";
        }
    }
}
