using Npgsql;
using DatabaseVariables;
using System.Reflection;

namespace backend.Models;

public abstract class GenericDealer(DatabaseCredentials credentials) {

    protected async Task<NpgsqlDataReader> RetrieveData(string selectQuery) {
        if (credentials == null) {
            throw new Exception("No database credentials found.");
        } else {
            await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(FormatConnectionString(credentials));
            await using NpgsqlCommand cmd = dataSource.CreateCommand(selectQuery);
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            return reader;
        }
    }

    protected string FormatConnectionString(DatabaseCredentials credentials) {
        string connString = "";
        Type type = typeof(DatabaseCredentials);
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties) {
            connString += $"{property.Name}={property.GetValue(credentials, null)};";
        }
        return connString;
    }
}