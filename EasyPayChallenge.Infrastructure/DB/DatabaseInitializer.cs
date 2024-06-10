using Dapper;
using Microsoft.Data.SqlClient;

namespace EasyPayChallenge.Infrastructure.DB
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var createTableScript = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
                BEGIN
                    CREATE TABLE Products (
                        Id INT PRIMARY KEY IDENTITY,
                        Name NVARCHAR(100) NOT NULL,
                        Brand NVARCHAR(100),
                        PriceAmount DECIMAL(18, 2) NOT NULL,
                        PriceCurrency NVARCHAR(3) NOT NULL
                    );
                END";

                connection.Execute(createTableScript);
            }
        }
    }
}
