using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public GameObject ThisEnemy;
    private List<GameObject> AllRoad = new List<GameObject>(); // все клетки дороги
    private List<string> SAllRoad = new List<string>();// имена всех клеток дороги
    public List<string> NCorners = new List<string>();

    public Rigidbody2D rb;

    private int index = 1; // для перелистывания углов

    public void Start()
    {
        SAllRoad = FindAllRoad(NCorners);

        foreach (string i in SAllRoad)
        {
            AllRoad.Add(GameObject.Find(i));
        }
    }

    public void Update()
    {
        MoveOnRoad(AllRoad);
    }

    private void MoveOnRoad(List<GameObject> roads)
    {
        ThisEnemy.transform.position = Vector3.MoveTowards(ThisEnemy.transform.position, roads[index].transform.position,
                    Time.deltaTime * ThisEnemy.GetComponent<ParamsEnemy>().SpeedMove);
        if (ThisEnemy.transform.position == roads[index].transform.position)
        {
            index++;
        }
    }

    public List<string> FindAllRoad(List<string> corners)
    {
        List<string> roads = new List<string>();
        string PlaceNow = corners[0];

        for (int i = 0; i < corners.Count; i++)
        {
            if (corners[i].Split(';')[0] != PlaceNow.Split(';')[0])
            {
                int tmp_different = int.Parse(corners[i].Split(';')[0]) - int.Parse(PlaceNow.Split(';')[0]);
                if (tmp_different < 0)
                {
                    tmp_different *= -1;
                    index *= -1;
                }
                for (int k = 0; k < tmp_different; k++)
                {
                    PlaceNow = ((int.Parse(PlaceNow.Split(';')[0])) + 1 * index) + ";" + int.Parse(PlaceNow.Split(';')[1]);
                    roads.Add(PlaceNow);
                }
            }
            else if (corners[i].Split(';')[1] != PlaceNow.Split(';')[1])
            {
                int index = 1; // индекс положительности/отрицательности направления куда строится дорога
                int tmp_different = int.Parse(corners[i].Split(';')[1]) - int.Parse(PlaceNow.Split(';')[1]);
                if (tmp_different < 0)
                {
                    tmp_different *= -1;
                    index *= -1;
                }
                for (int k = 0; k < tmp_different; k++)
                {
                    PlaceNow = int.Parse(PlaceNow.Split(';')[0]) + ";" + ((int.Parse(PlaceNow.Split(';')[1])) + 1 * index);
                    roads.Add(PlaceNow);
                }
            }
        }
        return roads;
    }
}
