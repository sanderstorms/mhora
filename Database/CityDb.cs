using System.Data.SQLite;
using System.IO;

namespace Mhora.Database
{
    internal class CityDb
    {
        private static CityDb _Instance;
        private const string DatabaseFile = "databaseFile.db";
        private const string DatabaseSource = "data source=" + DatabaseFile;
        private const string CreateTableQuery = @"CREATE TABLE IF NOT EXISTS [MyTable] (
                                               [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                               [Key] NVARCHAR(2048)  NULL,
                                               [Value] VARCHAR(2048)  NULL
                                               )";

        public static CityDb Instance
        {
            get
            {
                return (_Instance ??= new CityDb());
            }
        }

        private string DbName
        {
            get
            {
                return (Path.Combine(mhora.WorkingDir, DatabaseFile));
            }
        }

        private CityDb()
        {
            if (!File.Exists(DbName))
            {
                SQLiteConnection.CreateFile(DbName);
            }

            // Connect to the database 
            using (var connection = new SQLiteConnection(DatabaseSource))
            {
                // Create a database command
                using (var command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    // Create the table
                    command.CommandText = CreateTableQuery;
                    command.ExecuteNonQuery();

                    // Insert entries in database table
                    command.CommandText = "INSERT INTO MyTable (Key,Value) VALUES ('key one','value one')";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO MyTable (Key,Value) VALUES ('key two','value two')";
                    command.ExecuteNonQuery();

                    // Select and display database entries
                    command.CommandText = "Select * FROM MyTable";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mhora.Log.Debug(reader["Key"] + " : " + reader["Value"]);
                        }
                    }

                    connection.Close(); // Close the connection to the database
                }
            }
        }
    }

}
