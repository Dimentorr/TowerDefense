using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class Connection : MonoBehaviour
{
    public SQLiteConnection db;

    public void Start()
    {
        db = new SQLiteConnection(Application.streamingAssetsPath + "/base/DBForGame.db", SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        try
        {
            var for_check_DB = db.Table<CheckBase>().Where(_ => _.Check == 1).FirstOrDefault().ToString() == "1";
        }
        catch (SQLiteException)
        {
            db.CreateTable<CheckBase>();
            db.CreateTable<Levels>();
            db.CreateTable<UpgradesTower>();
            InsertData();
        }
    }

    public void InsertData()
    {
        db.InsertAll(new[]
        {
            new Levels
            {
                Condition = 0,
                Stars = 0
            },

            new Levels
            {
                Condition = 0,
                Stars = 0
            },

            new Levels
            {
                Condition = 0,
                Stars = 0
            }
        });

        db.InsertAll(new[]
        {
            new UpgradesTower
            {
                TypeTower = "Default",
                Tire = 0
            },
            new UpgradesTower
            {
                TypeTower = "Fast",
                Tire = 0
            }
        });
        db.InsertAll(new[]
        {
            new CheckBase
            {
                Check = 1
            }
        });
    }

    public string Select_UpgradesTower(int ID)
    {
        string answer = db.Table<UpgradesTower>().Where(_ => _.Id == ID).FirstOrDefault().ToString();
        return answer;
    }

    public string Select_Levels(int ID)
    {
        string answer = db.Table<Levels>().Where(_ => _.Id == ID).FirstOrDefault().ToString();
        return answer;
    }

    public void UpdateTable_UpgradesTower(int ID, int tire)
    {
        var for_update = db.Table<UpgradesTower>().Where(_ => _.Id == ID).FirstOrDefault();
        for_update.Tire = tire;
        db.Update(for_update);
    }

    public void UpdateTable_Levels(int ID, int condition, int stars)
    {
        var for_update = db.Table<Levels>().Where(_ => _.Id == ID).FirstOrDefault();
        for_update.Condition = condition;
        for_update.Stars = stars;
        db.Update(for_update);
    }
}

public class Levels
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Condition { get; set; }
    public int Stars { get; set; }
    public override string ToString()
    {
        return string.Format("{0},{1},{2}", Id, Condition, Stars);
    }
}

public class UpgradesTower
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string TypeTower { get; set; }
    public int Tire { get; set; }
    public override string ToString()
    {
        return string.Format("{0},{1},{2}", Id, TypeTower, Tire);
    }
}

public class CheckBase
{
    public int Check { get; set; }
    public override string ToString()
    {
        return string.Format("{0}", Check);
    }
}
