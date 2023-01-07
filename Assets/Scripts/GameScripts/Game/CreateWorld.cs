using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateWorld : MonoBehaviour
{
    public Camera Cam; // для перемещения камеры в центр поля, после построения поля

    public float Edge;//сторона размещаемой клетки

    public int SizeWorld;

    public List<string> NamesCorners = new List<string>(); // список имём углов дороги
    public List<string> NamesTowersPlace = new List<string>(); // список имём точек для башен

    private int index = 1; // индекс положительности/отрицательности направления куда строится дорога
    public GameObject Grass; // префаб клетки травы
    public Sprite ForTower; // спрайт клетки для размещения башни
    public Sprite Road; // спрайт клетки дороги

    public GameObject Circle; // круг выбора башен для покупки

    public GameObject Enemy;

    public Text CountHP;
    public Text CountGold;

    public GameObject PlaceSpawn;

    public void Start()
    {
        SpawnPlace();
        BuildOtherPlace(NamesTowersPlace, ForTower, true); // расстановка мест для башен
        BuildOtherPlace(FindAllRoad(NamesCorners), Road); // расстановка клеток дороги
    }

    public void Update()
    {
        if (PlaceSpawn)
        {
            PlaceSpawn.GetComponent<Spawner>().CountKills = gameObject.GetComponent<CountKills>().CountKillsNow;
        }
    }

    public void SpawnPlace()
    {
        for (int i = 0; i < SizeWorld; i++)
        {
            for (int j = 0; j < SizeWorld; j++)
            {
                GameObject tmp = Instantiate(Grass);
                tmp.name = $"{i};{j}";
                tmp.transform.position = new Vector3(i * Edge, j * Edge, 0);
                if (SizeWorld / 2 == i && SizeWorld / 2 == j)
                {
                    Cam.transform.position = new Vector3(tmp.transform.position.x, tmp.transform.position.y, Cam.transform.position.z);
                }
            }
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

    public void BuildOtherPlace(List<string> OtherPlaces, Sprite Spr, bool T = false) // флаг Т - если true значит ставим места для башен, иначе дороги (так как для клеток под башни нужна дополнительная настройка)
    {
        for (int i = 0; i < OtherPlaces.Count; i++)
        {
            GameObject tmp = GameObject.Find(OtherPlaces[i]);
            tmp.GetComponent<SpriteRenderer>().sprite = Spr;
            if (T)
            {
                // донастройка клетки
                tmp.AddComponent<CountClicks>();
                tmp.tag = "choice_tower";
                tmp.AddComponent<BoxCollider2D>();
                // добавление круга выбора башен для покупки
                GameObject CircleTmp = Instantiate(Circle);
                CircleTmp.name = "Circle";
                CircleTmp.transform.position = tmp.transform.position;
                CircleTmp.transform.parent = tmp.transform;
                CircleTmp.SetActive(false);
            }
            else
            {
                if (i == 0)
                {
                    PlaceSpawn = tmp;
                    tmp.AddComponent<Spawner>();
                    tmp.GetComponent<Spawner>().controller = gameObject;
                    tmp.GetComponent<Spawner>().PlaceSpawnEnemy = tmp;
                    tmp.GetComponent<Spawner>().NCorners = NamesCorners;
                    tmp.GetComponent<Spawner>().Enemy = Enemy;
                    tmp.GetComponent<Spawner>().CountGold = CountGold;
                }
                if (i + 1 == OtherPlaces.Count)
                {
                    tmp.tag = "end_road";
                    tmp.AddComponent<GetDamageFromEnemy>();
                    tmp.GetComponent<GetDamageFromEnemy>().CountHP = CountHP;
                    tmp.GetComponent<GetDamageFromEnemy>().controller = gameObject;
                    tmp.AddComponent<BoxCollider2D>();
                    tmp.GetComponent<BoxCollider2D>().isTrigger = true;
                }
            }
        }

    }
}

