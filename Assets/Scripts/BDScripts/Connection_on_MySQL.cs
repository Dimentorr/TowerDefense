using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MySqlConnector;

using System;
using System.Diagnostics;

public class Connection_on_MySQL : MonoBehaviour
{   
    public MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
    {
    Server = "localhost",
    UserID = "root",
    Password = "",
    Database = "test_for_game",
    };

    // Start is called before the first frame update
    public void ReaderForTests(List<object[]> message)
    {
        foreach (var i in message)
        {
            string res = "";
            foreach (var j in i)
            {
                res = res + "; " + j;
            }
            UnityEngine.Debug.Log(res);
        }
    }

    public List<object[]> SqlSelect(MySqlConnectionStringBuilder builder, string elements, string table_base, string condition=null)
    {
        string StrCommand = "";
        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();

        List<object[]> result = new List<object[]>();

        if (condition != null)
        {
            StrCommand = $"SELECT {elements} FROM {table_base} Where {condition}";
        }
        else
        {
            StrCommand = $"SELECT {elements} FROM {table_base}";
        }

        // создание запроса в бд
        using var command = new MySqlCommand(StrCommand, connection);

        // выполнение команды и чтение(запись) полученных данных
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

    public void SqlUpdate(MySqlConnectionStringBuilder builder, string table_base, List<string> names_columns, List<string> values, string condition)
    {
        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();
        if (names_columns.Count == values.Count)
        {
            // создание текста запроса
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

            // создание запроса в бд
            using var command = new MySqlCommand(StrCommand, connection);

            // выполнение команды
            using var reader = command.ExecuteReader();
        }
        connection.Close();
    }

    public void SqlInsert(MySqlConnectionStringBuilder builder, string table_base, List<string> values)
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
        UnityEngine.Debug.Log(StrCommand);
        // создание запроса в бд
        using var command = new MySqlCommand(StrCommand, connection);

        // выполнение команды и чтение(запись) полученных данных
        using var reader = command.ExecuteReader();

        connection.Close();
    }

    public void SqlDelete(MySqlConnectionStringBuilder builder, string table_base, string condition)
    {
        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();

        string StrCommand = $"Delete From {table_base} Where {condition}";

        UnityEngine.Debug.Log(StrCommand);
        // создание запроса в бд
        using var command = new MySqlCommand(StrCommand, connection);

        // выполнение команды и чтение(запись) полученных данных
        using var reader = command.ExecuteReader();

        connection.Close();
    }
}
