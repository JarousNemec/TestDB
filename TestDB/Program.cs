using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
namespace TestDB
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Program p = new Program();
            p.Connect();
            p.WriteToDB();
            p.DeleteDB();
            /* List<Actor> list = p.TestQuery();
             foreach (var a in list)
             {
                 Console.WriteLine("idcko "+a.first_name+" je: " + a.actor_id );
             }
             */
        }

        private void DeleteDB()
        {
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand(
                        "DROP SCHEMA 'exampleapp'",
                        connection);
                cmd.ExecuteNonQuery();
                Console.WriteLine("successfully delete db");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void WriteToDB()
        {
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand(
                        "INSERT INTO examples (ID, FirstNumber, SecondNumber, Operator, TrueOrFalse,Other) VALUES ('2005-10-26 17:28:03', 9, 8, '/', '0','1')",
                        connection);
                cmd.ExecuteNonQuery();
                Console.WriteLine("successfully wrote to db");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        MySqlConnection connection;

        private void Connect()
        {
            try
            {
                var MySQLConnectionString = ConfigurationSettings.AppSettings["MySQLConnectionString"]; 
                connection = new MySqlConnection(MySQLConnectionString);
                connection.Open();

                if (connection.State == ConnectionState.Open)
                {
                    Console.WriteLine("connected to schema");
                }
                else
                {
                    Console.WriteLine("error");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("problem with connect to database or schema > " + ex.Message);
                try
                {
                    connection =
                        new MySqlConnection("datasource=localhost;port=3306;username=root;password=Heslo123456*");
                    connection.Open();

                    if (connection.State == ConnectionState.Open)
                    {
                        Console.WriteLine("connected to db");

                        MySqlCommand cmd1 = new MySqlCommand("CREATE SCHEMA `exampleapp`;", connection);
                        MySqlCommand cmd2 = new MySqlCommand(
                            "CREATE TABLE `exampleapp`.`examappsexamples` (`ID` DATETIME NOT NULL,`FirstNumber` INT NOT NULL,`SecondNumber` INT NOT NULL,`Operator` VARCHAR(45) NOT NULL,`TrueOrFalse` TINYINT NOT NULL,`Other` VARCHAR(45) NULL DEFAULT NULL);",
                            connection);
                        MySqlCommand cmd3 =
                            new MySqlCommand(
                                "CREATE TABLE `exampleapp`.`examappssingpost` (`ID` DATETIME NOT NULL,`ExamplesIdentificator` VARCHAR(45) NOT NULL,`Users` VARCHAR(45) NOT NULL);",
                                connection);

                        try
                        {
                            cmd1.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("create new schema problem > " + e.Message);
                        }

                        try
                        {
                            cmd2.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("create new exampletable problem > " + e.Message);
                        }

                        try
                        {
                            cmd3.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("create new maininfotable problem > " + e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }

                    Console.WriteLine("successfully connected to db");
                }
                catch (Exception e)
                {
                    Console.WriteLine("problem with connect to database > " + e.Message);
                }
            }
        }

        private List<Actor> TestQuery()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM sakila.actor order by actor_id desc", connection);
            var reader = cmd.ExecuteReader();


            int ordId = reader.GetOrdinal("actor_id");
            int firstname = reader.GetOrdinal("first_name");
            int lastname = reader.GetOrdinal("last_name");
            int updatetime = reader.GetOrdinal("last_update");
            List<Actor> list = new List<Actor>();
            while (reader.Read())
            {
                Actor a = new Actor();
                a.actor_id = reader.GetInt32(ordId);
                a.first_name = reader.GetString(firstname);
                a.last_name = reader.GetString(lastname);
                a.last_update = reader.GetDateTime(updatetime);
                list.Add(a);
            }

            int i = 0;
            return list;
        }
    }
}