using DatabaseVariables;

namespace backend.Models;

public class GreebloDealer(DatabaseCredentials credentials) : GenericDealer(credentials) {

    public async Task<float[]> GetStatus(int[] schedule) {
        Greeblo greeblo = new() {
            Health = 5,
            Cacao = 75,
            Schedule = await TranslateSchedule(schedule)
        };
        Console.WriteLine(greeblo.Schedule[2].CacaoPercent);
        return [];

    }

    private async Task<ChocolateBar[]> TranslateSchedule(int[] schedule) {
        ChocolateBar[] result = new ChocolateBar[21];
        ChocolateBarDealer cacaoDealer = new(credentials);
        for (int i = 0; i < schedule.Length; i++) {
            result[i] = await cacaoDealer.Get(schedule[i]);
        }
        return result;
    }
}