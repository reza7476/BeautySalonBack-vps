using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

public static class DataMigration
{
    private const string AppSettingPath = "appsettings.json";

    public static void Main(string[] args)
    {
        var baseDir = Directory.GetCurrentDirectory();
        var connectionString = GetConnectionString(baseDir);
        var settings = new MigrationSettings
        {
            ConnectionString = connectionString
        };
        EnsureDatabaseExistPostgres(connectionString);
        var runner = CreateRunner(connectionString);
        runner.MigrateUp();
        //runner.MigrateDown(); 
    }

    private static void EnsureDatabaseExist(string connectionString)
    {
        var dbName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

        var masterConnectionString = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master"
        }.ConnectionString;

        var query = $"IF DB_ID(N'{dbName}') IS NULL CREATE DATABASE [{dbName}]";

        using var connection = new SqlConnection(masterConnectionString);
        using var command = new SqlCommand(query, connection);
        connection.Open();
        command.ExecuteNonQuery();
    }


    private static void EnsureDatabaseExistPostgres(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);//parse connection string
        var dbName = builder.Database;//get db name

        builder.Database = "postgres";// admin db name in postgres
        var adminConnectionString = builder.ConnectionString;//change connection string to admin db in postgres

        //check exist db by type script
        var checkDbQuery = @"
        SELECT 1
        FROM pg_database
        WHERE datname = @dbName;
        ";

        using var connection = new NpgsqlConnection(adminConnectionString);//create new connection string
        connection.Open();

        using var checkCommand = new NpgsqlCommand(checkDbQuery, connection);//protected from sql injection
        checkCommand.Parameters.AddWithValue("dbName", dbName);

        var exists = checkCommand.ExecuteScalar() != null; //execute query


        //create database if dose not exist
        if (!exists)
        {
            var createDbCommand = $"CREATE DATABASE \"{dbName}\";";
            using var createCommand = new NpgsqlCommand(createDbCommand, connection);
            createCommand.ExecuteNonQuery();
        }

    }

    private static IMigrationRunner CreateRunner(string connectionString)
    {
        var services = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                //.AddSqlServer()
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DataMigration).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider();

        return services.GetRequiredService<IMigrationRunner>();
    }

    public static string GetConnectionString(string baseDir)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(baseDir)
            .AddJsonFile(AppSettingPath, optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config["migrationConfig:connectionString"]
                               ?? throw new InvalidOperationException("Connection string not found in configuration.");

        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

        if (!string.IsNullOrEmpty(dbUser))
            connectionString = connectionString.Replace("${DB_USER}", dbUser);

        if (!string.IsNullOrEmpty(dbPass))
            connectionString = connectionString.Replace("${DB_PASS}", dbPass);

        return connectionString;
    }
}

public class MigrationSettings
{
    public string ConnectionString { get; set; } = default!;
}