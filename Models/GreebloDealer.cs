using DatabaseVariables;

namespace backend.Models;

public class GreebloDealer(DatabaseCredentials credentials) : GenericDealer(credentials) {

    public async Task<float[]> GetStatus(string schedule) {
        Greeblo greeblo = new() {
            Health = 5,
            Cacao = 75
        };
        
        greeblo.Schedule = await TranslateSchedule(schedule);

        for (int i = 0; i < greeblo.Schedule.Length; i++) {
            if (greeblo.Schedule[i] == null) {
                Console.WriteLine($"{i}: null");
            } else {
                Console.WriteLine($"{i}: {greeblo.Schedule[i].Company}");
            }
        }

        return [];

    }

    private async Task<ChocolateBar[]> TranslateSchedule(string scheduleStr) {
        ChocolateBar[] result = new ChocolateBar[21];
        ChocolateBarDealer cacaoDealer = new(credentials);

        List<int> schedule = scheduleStr.Split(',').Select(int.Parse).ToList();

        for (int i = 0; i < schedule.Count; i++) {
            if (schedule[i] > 0) { // not >= because table IDs start at 1
                result[i] = await cacaoDealer.Get(schedule[i]);
            }
        }

        return result;
    }
}