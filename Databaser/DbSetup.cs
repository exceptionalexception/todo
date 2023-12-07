using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Databaser
{
    public static class DbSetup
    {
        private static ConfigurationManager? _config;
        private static string? _serverConnectionString;
        private static string? _dbConnectionString;

        public static void SetupDb(ConfigurationManager? config = null)
        {
            if (config != null)
            {
                _config = config;
            }

            if (IsDbReady())
            {
                // dont attempt to create DB or schema if they already exist.
                if (IsSchemaReady()) return;

                try
                {
                    using SqlConnection dbConnection = new(_dbConnectionString);
                    dbConnection.Open();
                    CreateTodoTable();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An unexpected error occured while creating the todo schema.{e}");
                    throw;
                }
            }
            else
            {
                CreateDb();
                
                // now db exists, we can recall the db setup to create the todo schema.
                SetupDb();
            }
        }

        private static void CreateDb()
        {
            using SqlConnection connection = new(_serverConnectionString);
            connection.Open();

            var filePath = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, 
                "DatabaseScripts", "CreateTodoAppDb.sql"
            );

            var script = string.Empty;
            //var filePath = "./CreateTodoAppDb.sql";
            if (File.Exists(filePath))
            {
                script = File.ReadAllText(filePath);
            }
            else throw new Exception($"{filePath} file not found.");

            try
            {
                connection.Execute(script);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An unexpected error occured while creating the todo database.{e}");
                throw;
            }

        }

        private static void CreateTodoTable()
        {
            try
            {
                using SqlConnection dBconnection = new(_dbConnectionString);
                dBconnection.Open();

                var filePath = Path.Combine(
                    Directory.GetParent(Directory.GetCurrentDirectory())!.FullName,
                    "DatabaseScripts", "CreateTodoTable.sql"
                );
                var script = string.Empty;
                if (File.Exists(filePath))
                {
                    script = File.ReadAllText(filePath);
                }
                else throw new Exception($"{filePath} file not found.");

                dBconnection.Execute(script);
                Console.WriteLine("Todo table created successfully.");
                Seed();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An unexpected error occured while creating the todo schema.{e}");
                throw;
            }
        }

        private static void Seed()
        {
            using SqlConnection dBconnection = new(_dbConnectionString);
            dBconnection.Open();

            var filePath = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory())!.FullName,
                "DatabaseScripts", "Todo_seed.sql"
            );
            var script = string.Empty;
            if (File.Exists(filePath))
            {
                script = File.ReadAllText(filePath);
            }
            else throw new Exception($"{filePath} file not found.");

            dBconnection.Execute(script);
            Console.WriteLine("Todo table seeded successfully.");
        }

        private static bool IsDbReady()
        {
            var serverConnectionString = _config!.GetConnectionString("ServerConnection");
            if (string.IsNullOrEmpty(serverConnectionString))
                throw new Exception($"No connection string was found in appsettings.dev-db.json file.");

            _serverConnectionString = serverConnectionString;

            using SqlConnection serverConnection = new(serverConnectionString);

            try
            {
                serverConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    @$"Failed to open server connection. 
                    Please make sure local database name in ServerConnection 
                    (in appsettings.dev-db.json file) is correct and local DB exists and 
                    SQL service is running.{e}"
                );
                throw;
            }

            var databaseExists = serverConnection
                .ExecuteScalar<int?>("SELECT DB_ID('TodoAppDb')") != null;

            return databaseExists;
        }

        private static bool IsSchemaReady()
        {
            var dbConnectionString = _config!.GetConnectionString("DbConnection");
            if (string.IsNullOrEmpty(dbConnectionString))
                throw new Exception($"No connection string was found in appsettings.dev-db.json file.");

            _dbConnectionString = dbConnectionString;

            using SqlConnection dBconnection = new(dbConnectionString);
            dBconnection.Open();
            var tableExists = dBconnection.ExecuteScalar<bool>(
                @"IF EXISTS (
                    SELECT * 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'Todos'
                ) 
                SELECT 1 
                ELSE SELECT 0"
            );
            return tableExists;
        }
    }
}
