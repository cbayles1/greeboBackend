using Npgsql;
using DatabaseVariables;

namespace backend.Models;

public class ChocolateBarDealer(DatabaseCredentials credentials) : GenericDealer(credentials) {
    private const string allButCacao = "id, company, specific_origin, recency, review_year, company_location, rating, bean_type, broad_origin";

    public async Task<IEnumerable<ChocolateBar>> GetAll() { // get all except cacao_percent, which user should not see
        NpgsqlDataReader reader = await RetrieveData($"SELECT {allButCacao} FROM chocolate_bars;");
        List<ChocolateBar> bars = [];
        while (reader.Read()) {
            bars.Add(GetBarFromFullRow(reader));
        }
        return bars;
    }

    public async Task<ChocolateBar> Get(int id) {
        NpgsqlDataReader reader = await RetrieveData($"SELECT {allButCacao} FROM chocolate_bars WHERE id = {id};");
        reader.Read();
        return GetBarFromFullRow(reader);
    }

    private ChocolateBar GetBarFromFullRow(NpgsqlDataReader reader) {
        ChocolateBar bar = new() {
            Id = (int)reader.GetValue(0),
            Company = (string)reader.GetValue(1),
            SpecificOrigin = (string)reader.GetValue(2),
            Recency = (int)reader.GetValue(3),
            ReviewYear = (int)reader.GetValue(4),
            //CacaoPercent = (int)reader.GetValue(5),
            CompanyLocation = (string)reader.GetValue(5),
            Rating = (float)reader.GetValue(6),
            BeanType = (string)reader.GetValue(7),
            BroadOrigin = (string)reader.GetValue(8)
        };
        if (bar == null) {
            throw new Exception("Response not understood.");
        }
        return bar;
    }

    public async Task<int> GetCacaoPercent(int id) {
        try {
            NpgsqlDataReader reader = await RetrieveData($"SELECT cacao_percent FROM chocolate_bars WHERE id = {id};");
            reader.Read();
            return (int)reader.GetValue(0);
        } catch {
            throw new Exception($"Something went wrong getting cacao amount of entry {id}.");
        }
    }
}