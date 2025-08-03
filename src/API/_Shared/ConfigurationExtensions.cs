using Npgsql;

namespace API._Shared;

internal static class ConfigurationExtensions
{
    internal static NpgsqlConnectionStringBuilder GetDbConnectionString(this IConfiguration configuration)
    {
        var databaseCfgSection = configuration.GetSection("Database");
        return new NpgsqlConnectionStringBuilder
        {
            Host = databaseCfgSection.GetValue<string>("Host"),
            Port = databaseCfgSection.GetValue("Port", 5432),
            Database = databaseCfgSection.GetValue<string>("Database"),
            Username = databaseCfgSection.GetValue<string>("Username"),
            Password = databaseCfgSection.GetValue<string>("Password")
        };
    }
}