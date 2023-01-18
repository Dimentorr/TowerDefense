using System.Collections.Generic;
using UnityEngine;

using MySqlConnector;

using System;

public class Connection_on_MySQL : MonoBehaviour
{
    // openserver
    /*public MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
    {
        Server = "localhost",
        Database = "test_for_game",
        UserID = "root",
        Password = ""
    };*/

    // spaceweb
    public MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
    {
        Server = "FVH1.spaceweb.ru",
        Database = "toweradmin",
        UserID = "toweradmin",
        Password = "LifeAfterDeath999"
    };

    // Start is called before the first frame update
    public List<string> Reader(List<object[]> message)
    {
        List<string> result = new List<string>();
        foreach (var i in message)
        {
            string res = "";
            foreach (var j in i)
            {
                if (res != "")
                {
                    res = res + "; " + j;
                }
                else
                {
                    res = j.ToString();
                }
            }
            result.Add(res);
        }
        return result;
    }

    public List<object[]> SqlSelect(string table_base, string condition=null)
    {
        string StrCommand = "";
        using var connection = new MySqlConnection(builder.ConnectionString);
        Debug.Log(connection);
        try
        {
            connection.Open();
        }
        catch (MySqlException e)
        {
            Debug.Log(e.ToString());
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }

        List<object[]> result = new List<object[]>();

        if (condition != null)
        {
            StrCommand = $"SELECT * FROM {table_base} Where {condition}";
        }
        else
        {
            StrCommand = $"SELECT * FROM {table_base}";
        }

        // �������� ������� � ��
        using var command = new MySqlCommand(StrCommand, connection);

        // ���������� ������� � ������(������) ���������� ������
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            if (table_base == "users")
            {
                result.Add(new object[] { reader.GetInt32("id"), reader.GetString("name"), reader.GetString("password") });
            }
        }

        connection.Close();

        return result;
    }

    public void SqlUpdate(string table_base, List<string> names_columns, List<string> values, string condition)
    {
        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();
        if (names_columns.Count == values.Count)
        {
            // �������� ������ �������
            string StrCommand = $"Update {table_base} Set ";
            for (int i = 0; i < names_columns.Count; i++)
            {
                StrCommand += names_columns[i] + " = ";
                try
                {
                    StrCommand += Convert.ToInt32(values[i]);
                }
                catch
                {
                    StrCommand += "'"+values[i]+"'";
                }

                if (i + 1 < names_columns.Count)
                {
                    StrCommand += ", ";
                }
            }
            StrCommand += $" Where {condition}";

            UnityEngine.Debug.Log(StrCommand);

            // �������� ������� � ��
            using var command = new MySqlCommand(StrCommand, connection);

            // ���������� �������
            using var reader = command.ExecuteReader();
        }
        connection.Close();
    }

    public void SqlInsert(string table_base, List<string> values)
    {
        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();

        string StrCommand = $"INSERT INTO {table_base} VALUES (";
        for (int i = 0; i < values.Count; i++)
        {
            try
            {
                StrCommand += Convert.ToInt32(values[i]);
            }
            catch
            {
                StrCommand += "'" + values[i] + "'";
            }

            if (i + 1 < values.Count)
            {
                StrCommand += ", ";
            }
            else
            {
                StrCommand += ")";
            }
        }
        // �������� ������� � ��
        using var command = new MySqlCommand(StrCommand, connection);

        // ���������� ������� � ������(������) ���������� ������
        using var reader = command.ExecuteReader();

        connection.Close();
    }

    public void SqlDelete(string table_base, string condition)
    {
        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();

        string StrCommand = $"Delete From {table_base} Where {condition}";

        UnityEngine.Debug.Log(StrCommand);
        // �������� ������� � ��
        using var command = new MySqlCommand(StrCommand, connection);

        // ���������� ������� � ������(������) ���������� ������
        using var reader = command.ExecuteReader();

        connection.Close();
    }
}
