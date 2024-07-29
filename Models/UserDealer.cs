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

    public async Task<int[]> GetBarIds(int userId) {
        NpgsqlDataReader reader = await RetrieveData($"SELECT bar_ids FROM users WHERE id = {userId};");
        reader.Read();
        int[] barIds = (int[])reader.GetValue(0);
        if (barIds == null) {
            throw new Exception("Response not understood.");
        }
        return barIds;
    }

    // public async Task<ChocolateBar[]> GetBars(int userId) {
    //     NpgsqlDataReader reader = await RetrieveData($"SELECT bar_ids FROM users WHERE id = {userId};");
    //     reader.Read();
    //     int[] barIds = (int[])reader.GetValue(0);
    //     if (barIds == null) {
    //         throw new Exception("Response not understood.");
    //     }

    //     ChocolateBarDealer chocolateDealer = new ChocolateBarDealer(dbCredentials);
    //     ChocolateBar[] chocolateBars = [];
    //     for (int i = 0; i < barIds.Length; i++) {
    //         chocolateBars[i] = await chocolateDealer.Get(barIds[i]);
    //     }

    //     return chocolateBars;
    // }

    
}