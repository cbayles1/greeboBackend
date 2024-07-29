using Npgsql;
using DatabaseVariables;

namespace backend.Models;

public class UserDealer(DatabaseCredentials dbCredentials) : GenericDealer(dbCredentials) {
    public async Task<User> Get(int id) {
        NpgsqlDataReader reader = await RetrieveData($"SELECT * FROM users WHERE id = {id};");
        reader.Read();
        User user = new() {
            Id = (int)reader.GetValue(0),
            GreebloHealth = (int)reader.GetValue(1),
            GreebloCacao = (int)reader.GetValue(2),
            PickedBar = (int)reader.GetValue(3),
            Day = (int)reader.GetValue(4),
            Time = (int)reader.GetValue(5),
            BarIds = (int[])reader.GetValue(6)
        };
        if (user == null) {
            throw new Exception("Response not understood.");
        }
        return user;
    }
}