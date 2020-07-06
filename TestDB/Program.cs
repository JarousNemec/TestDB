using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
namespace TestDB
{
    internal class Program
    {
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string DB_SCHEMA_NAME = "exampleapp";
        
        public static void Main(string[] args)
        {
            log.Info("Hi, I am your logger.");
            Program p = new Program();
        }

        public Program()
        {
            MySqlConnection connection = openConnectionToDB();
            if (connection == null)
            {
                Console.WriteLine("nic");
                return;
            }

            if (!IsDBschemaValid(connection))
            {
                createSchema(connection);
            }
            forceDatabaseName(DB_SCHEMA_NAME, connection);
            WriteToDB(connection);
            DeleteDB(connection);
        }

        private void forceDatabaseName(string dbSchemaName, MySqlConnection connection)
        {
            try
            {
                MySqlCommand cmd1 = new MySqlCommand("use `"+DB_SCHEMA_NAME+"`;", connection);
                cmd1.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Force Schema problem > " + e.Message);
                return;
            }
        }

        private void createSchema(MySqlConnection connection)
        {
            try
            {
                MySqlCommand cmd1 = new MySqlCommand("CREATE SCHEMA `"+DB_SCHEMA_NAME+"`;", connection);
                cmd1.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("create new schema problem > " + e.Message);
                return;
            }

            try
            {
                MySqlCommand cmd2 = new MySqlCommand(
                    "CREATE TABLE `exampleapp`.`examappsexamples` (`ID` DATETIME NOT NULL,`FirstNumber` INT NOT NULL,`SecondNumber` INT NOT NULL,`Operator` VARCHAR(45) NOT NULL,`TrueOrFalse` TINYINT NOT NULL,`Other` VARCHAR(45) NULL DEFAULT NULL);",
                    connection);
                cmd2.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("create new exampletable problem > " + e.Message);
                return;
            }

            try
            {
                MySqlCommand cmd3 =
                    new MySqlCommand(
                        "CREATE TABLE `exampleapp`.`examappssingpost` (`ID` DATETIME NOT NULL,`ExamplesIdentificator` VARCHAR(45) NOT NULL,`Users` VARCHAR(45) NOT NULL);",
                        connection);
                cmd3.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("create new maininfotable problem > " + e.Message);
                return;
            }
        }


        private void DeleteDB(MySqlConnection connection)
        {
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand(
                        "DROP SCHEMA exampleapp; ",
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

        private void WriteToDB(MySqlConnection connection)
        {
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand(
                        "INSERT INTO examappsexamples (ID, FirstNumber, SecondNumber, Operator, TrueOrFalse,Other) VALUES ('2005-10-26 17:28:03', 9, 8, '/', '0','1')",
                        connection);
                cmd.ExecuteNonQuery();
                Console.WriteLine("successfully wrote to db");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private bool IsDBschemaValid(MySqlConnection connection)
        {
            MySqlCommand cmd =
                new MySqlCommand("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'exampleapp'",
                    connection);

            var reader = cmd.ExecuteReader();
            int SCHEMA_NAME = reader.GetOrdinal("SCHEMA_NAME");

            bool hasMoreLines = reader.Read();
            reader.Close();
            
/*            if (hasMoreLines)
            {
                return true;
            }
            else
            {
                return false;
            }
*/
            return hasMoreLines;
        }


        private MySqlConnection openConnectionToDB()
        {
            try
            {
                var MySQLConnectionString = ConfigurationSettings.AppSettings["MySQLConnectionString"];
                MySqlConnection connection = new MySqlConnection(MySQLConnectionString);
                connection.Open();

                if (connection.State == ConnectionState.Open)
                {
                    Console.WriteLine("connected to schema");
                    return connection;
                }
                else
                {
                    Console.WriteLine("error");
                }
            }

            catch (Exception ex)
            {
                throw ex;
                /*
                               
            
            */
            }

            return null;
        }

/*private List<Actor> TestQuery()
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
}*/
    }
}