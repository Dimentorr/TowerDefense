using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controll_clicks : MonoBehaviour
{
    public Sprite Place_for_tower;
    public Sprite Place_for_tower_with_him;
    public GameObject Default_tower;
    // текущее нажатие и предыдущее (если таковое было или прин€о считать, что оно было)
    private GameObject NowCell;
    private GameObject LostCell;

    public Text CountGold;

    public GameObject controller;

    [System.Obsolete]
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (FindCell("default"))
            {
                LostCell.transform.GetChild(0).gameObject.SetActive(false);
                LostCell.GetComponentInChildren<CountClicks>().Click_Clack = 0;

                if (int.Parse(CountGold.text) >= 100)
                {

                    LostCell.GetComponent<SpriteRenderer>().sprite = Place_for_tower_with_him;
                    CountGold.text = $"{int.Parse(CountGold.text) - 100}";
                    GameObject tower = Instantiate(Default_tower);
                    tower.GetComponent<SpawnBullets>().controller = controller;
                    tower.transform.position = LostCell.transform.position;
                    LostCell.tag = "Untagged";
                }

                LostCell = null;
            }
            else if (FindCell("Circle"))
            {
                LostCell.GetComponentInChildren<CountClicks>().Click_Clack = 0;
                LostCell.transform.GetChild(0).gameObject.SetActive(false);
                LostCell = null;
            }
            else if (FindCell("choice_tower"))
            {
                NowCell = FindCell("choice_tower");
                // проверка на кол-во нажатий на выбранную клетку
                if (NowCell.GetComponentInChildren<CountClicks>().Click_Clack % 2 == 0)
                {
                    if (LostCell && LostCell != NowCell)
                    {
                        LostCell.GetComponentInChildren<CountClicks>().Click_Clack = 0;
                        LostCell.transform.GetChild(0).gameObject.SetActive(false);
                        LostCell = NowCell;
                        LostCell.GetComponentInChildren<CountClicks>().Click_Clack = 1;
                        LostCell.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        LostCell = NowCell;
                        LostCell.GetComponentInChildren<CountClicks>().Click_Clack = 1;
                        LostCell.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (LostCell)
                {
                    LostCell.GetComponentInChildren<CountClicks>().Click_Clack = 0;
                    LostCell.transform.GetChild(0).gameObject.SetActive(false);
                    LostCell = null;
                }
            }
        }
    }

    public GameObject FindCell(string find_tag)
    {
        // нахождение списка коллайдеров, в месте клика Ћ ћ
        var wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        wp.z = 0;
        Vector2 click = new Vector2(wp.x, wp.y);
        var tmp = Physics2D.CircleCastAll(click, 0.1f, Vector2.zero);
        List<RaycastHit2D> hit = new List<RaycastHit2D>();
        for (int i = 0; i <= tmp.Length - 1; i++)
        {
            if (tmp[i].transform.tag == find_tag)
            {
                return tmp[i].transform.gameObject;
            }
        }
        return null;
    }
}
