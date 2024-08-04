using DatabaseVariables;

namespace backend.Models;

public class GreebloDealer(DatabaseCredentials credentials) : GenericDealer(credentials) {

    public async Task<double> GetStatus(int[] schedule) {
        Greeblo greeblo = new() {
            Health = 75, // default values
            Cacao = 25, // ..
            Schedule = await TranslateSchedule(schedule)
        };

        const int DAYS = 7;
        const int DAILY_MEALS = 3;
        const int STARVATION_AMT = 1;
        const int CACAO_BAD_AMT = 25;
        const int CACAO_GOOD_AMT = 15;
        const double CACAO_FLOOR = 47;
        const double CACAO_CEILING = 63;

        for (int i = 0; i < DAYS; i++) {
            int dailyCacaoSum = 0;
            Console.WriteLine($"Day {i}");
            for (int j = 0; j < DAILY_MEALS; j++) {
                ChocolateBar bar = greeblo.Schedule[i * DAILY_MEALS + j];
                
                if (bar == null) {
                    greeblo.Health = Math.Clamp(greeblo.Health - STARVATION_AMT, 0, 100); // IF NO MEAL EATEN, LOWER HEALTH (within bounds)
                    if (greeblo.Health <= 0) break;
                } else {
                    Console.WriteLine($"Before: {greeblo.Cacao} | Bar: {bar.CacaoPercent}");
                    dailyCacaoSum += bar.CacaoPercent;
                    greeblo.Cacao = Math.Clamp((greeblo.Cacao + bar.CacaoPercent) / 2.0, 0, 100); // UPDATE CACAO VALUE AFTER NEW BAR BY AVERAGING THE TWO (within bounds)
                }
            }
            if (greeblo.Health <= 0) break;

            greeblo.Cacao = dailyCacaoSum / DAILY_MEALS; // daily consumption, has to be between CACAO_FLOOR and CACAO_CEILING
            Console.WriteLine($"Daily Cacao: {greeblo.Cacao}, Health: {greeblo.Health}\n---------------");

            if (greeblo.Cacao >= CACAO_FLOOR && greeblo.Cacao <= CACAO_CEILING) {
                greeblo.Health = Math.Clamp(greeblo.Health + CACAO_GOOD_AMT, 0, 100); // IF IN A GOOD CACAO RANGE, RAISE HEALTH (within bounds)
            } else {
                greeblo.Health = Math.Clamp(greeblo.Health - CACAO_BAD_AMT, 0, 100); // IF NOT, LOWER HEALTH (within bounds)
            }
            if (greeblo.Health <= 0) break;
        }

        return greeblo.Health;
    }

    private async Task<ChocolateBar[]> TranslateSchedule(int[] schedule) {
        ChocolateBar[] result = new ChocolateBar[21];
        ChocolateBarDealer cacaoDealer = new(credentials);

        for (int i = 0; i < schedule.Length; i++) {
            if (schedule[i] > 0) { // not >= because table IDs start at 1
                result[i] = await cacaoDealer.Get(schedule[i]);
            }
        }

        return result;
    }
}