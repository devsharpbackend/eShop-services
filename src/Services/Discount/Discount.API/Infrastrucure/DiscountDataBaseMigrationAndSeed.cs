namespace eShop.Services.Discount.Api.Infrastructure;

public class DiscountDataBaseMigrationAndSeed
{
    public void ExecuteMigrations(IConfiguration configuration)
    {
        
        // Check Exists DataBase
       
        var Serverconnection = new NpgsqlConnection(configuration.GetValue<string>("ConnectionString").Replace("servicesdiscountdb", "postgres"));
      
        bool dbExists = false;
        Serverconnection.Open();
        string cmdText = "SELECT 1 FROM pg_database WHERE datname='servicesdiscountdb'";
        using (NpgsqlCommand cmd = new NpgsqlCommand(cmdText, Serverconnection))
        {
            dbExists = cmd.ExecuteScalar() != null;
        }
        if (dbExists) return;

       
         cmdText = "Create DataBase servicesdiscountdb";
         using (NpgsqlCommand cmd = new NpgsqlCommand(cmdText, Serverconnection))
         {
             cmd.ExecuteNonQuery();
         }
        
        Serverconnection.Close();

        using var connection = new NpgsqlConnection(configuration.GetValue<string>("ConnectionString"));

        connection.Open();

        using var command = new NpgsqlCommand
        {
            Connection = connection
        };
      

        command.CommandText = "DROP TABLE IF EXISTS CatalogDiscount";
        command.ExecuteNonQuery();

        command.CommandText = @" CREATE TABLE CatalogDiscount(Id SERIAL PRIMARY KEY, 
                                                                Name Text,            
                                                                CatalogId INT NOT NULL,
                                                                Description TEXT,
                                                                Amount INT,IsActive boolean)";
        command.ExecuteNonQuery();


        command.CommandText = "INSERT INTO CatalogDiscount(Name,CatalogId, Description, Amount,IsActive) VALUES('copun 1',2, '.NET Black & White Mug', 150,true);";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO CatalogDiscount(Name,CatalogId, Description, Amount,IsActive) VALUES('copun 2',5, '.Roslyn Red Sheet', 100,true);";
        command.ExecuteNonQuery();
    }
}

