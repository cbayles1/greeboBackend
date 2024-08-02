using DatabaseVariables;

namespace backend.Models;

public class GreebloDealer(DatabaseCredentials credentials) : GenericDealer(credentials) {

    public async Task<float[]> GetStatus(string schedule) {
        ChocolateBarDealer cacaoDealer = new(credentials);
        Greeblo greeblo = new() {
            Health = 5,
            Cacao = 75,
            Schedule = await cacaoDealer.GetMultiple(schedule)
        };
        for (int i = 0; i < greeblo.Schedule.Length; i++) {
            if (greeblo.Schedule[i] == null) {
                Console.WriteLine($"{i}: null");
            } else {
                Console.WriteLine($"{i}: {greeblo.Schedule[i].Id}");
            }
        }
        return [];
    }

    // private async Task<ChocolateBar[]> TranslateSchedule(int[] schedule) {
    //     ChocolateBar[] result = new ChocolateBar[21];
    //     ChocolateBarDealer cacaoDealer = new(credentials);
    //     for (int i = 0; i < schedule.Length; i++) {
    //         result[i] = await cacaoDealer.Get(schedule[i]);
    //     }
    //     return result;
    // }
}