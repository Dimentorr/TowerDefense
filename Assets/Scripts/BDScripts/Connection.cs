using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SQLite4Unity3d;

public class Connection : MonoBehaviour
{
    public SQLiteConnection db = new SQLiteConnection(Application.streamingAssetsPath + "/base/DBForGame.db", SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

    public void CheckDB()
    {
        try
        {
            var for_check_DB = db.Table<user>().FirstOrDefault().ToString();
        }
        catch (SQLiteException)
        {
            db.CreateTable<user>();
            db.CreateTable<levels>();
            db.CreateTable<progress>();
        }
    }

    public void InsertData(string table, List<string> data)
    {
        if (table == "user")
        {
            db.InsertAll(new[]
            {
                new user
                {
                    id = 1,
                    name = data[0],
                    password = data[1],
                    role = data[2],
                    isBaned = Convert.ToInt32(data[3])
                }
            });
        }
        else if (table == "levels")
        {
            db.InsertAll(new[]
           {
                new levels
                {
                    id = Convert.ToInt32(data[0]),
                    corners = data[1],
                    towerPlace = data[2]
                }
            });
        }

    }

    public string Select_User(int ID=1)
    {
        try
        {
            string answer = db.Table<user>().Where(_ => _.id == ID).FirstOrDefault().ToString();
            return answer;
        }
        catch
        {
            return null;
        }
    }

    public string Select_Progress(int ID)
    {
        try
        {
            string answer = db.Table<progress>().Where(_ => _.levelId == ID).FirstOrDefault().ToString();
        return answer;
        }
        catch
        {
            return null;
        }
    }
    public string Select_Levels(int ID)
    {
        try
        {
            string answer = db.Table<levels>().Where(_ => _.id == ID).FirstOrDefault().ToString();
            return answer;
        }
        catch
        {
            return null;
        }
    }

    public void UpdateTable_Progress(int ID, int levelId, int stars)
    {
        var for_update = db.Table<progress>().Where(_ => _.levelId == ID).FirstOrDefault();
        for_update.levelId = levelId;
        for_update.stars = stars;
        db.Update(for_update);
    }

    public void UpdateTable_User(int ID, string password, int isBaned=0, string role="player")
    {
        var for_update = db.Table<user>().Where(_ => _.id == ID).FirstOrDefault();
        for_update.password = password;
        for_update.isBaned = isBaned;
        for_update.role = role;
        db.Update(for_update);
    }
    public void DeleteTable_User()
    {
        db.DeleteAll<user>();
    }
}

public class levels
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string corners { get; set; }
    public string towerPlace { get; set; }
    public override string ToString()
    {
        return string.Format("{0},{1},{2}", id, corners, towerPlace);
    }
}

public class user
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string name { get; set; }
    public string password { get; set; }
    public string role { get; set; }
    public int isBaned { get; set; }

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4}", id, name, password, isBaned, role);
    }
}

public class progress
{
    public int levelId { get; set; }
    public int stars { get; set; }
    public override string ToString()
    {
        return string.Format("{0},{1}", levelId, stars);
    }
}
