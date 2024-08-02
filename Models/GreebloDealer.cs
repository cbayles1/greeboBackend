using DatabaseVariables;

namespace backend.Models;

public class GreebloDealer(DatabaseCredentials credentials) : GenericDealer(credentials) {

    public async Task<double[]> GetStatus(int[] schedule) {
        Greeblo greeblo = new() {
            Health = 75, // default values
            Cacao = 0.75, // ..
            Schedule = await TranslateSchedule(schedule)
        };

        const int DAYS = 7;
        const int DAILY_MEALS = 3;
        const int STARVATION_AMT = 1;
        const int CACAO_BAD_AMT = 5;
        const int CACAO_GOOD_AMT = 5;
        const double CACAO_FLOOR = 0.1;
        const double CACAO_CEILING = 0.1;

        for (int i = 0; i < DAYS; i++) {
            for (int j = 0; j < DAILY_MEALS; j++) {
                ChocolateBar bar = greeblo.Schedule[i * DAILY_MEALS + j];

                if (bar == null) {
                    greeblo.Health = Math.Max(greeblo.Health - STARVATION_AMT, 0); // IF NO MEAL EATEN, LOWER HEALTH
                    greeblo.Health = Math.Min(greeblo.Health - STARVATION_AMT, 100); // MIN AND MAX PREVENT OUT OF BOUNDS
                } else {
                    greeblo.Cacao *= bar.CacaoPercent;            
                    greeblo.Cacao = Math.Max(bar.CacaoPercent, 0.0); // UPDATE CACAO VALUE AFTER NEW BAR
                    greeblo.Cacao = Math.Min(bar.CacaoPercent, 100.0);
                }
            }
            if (greeblo.Cacao > CACAO_FLOOR && greeblo.Cacao < CACAO_CEILING) {
                greeblo.Health = Math.Max(greeblo.Health + CACAO_GOOD_AMT, 0); // IF IN A GOOD CACAO RANGE, RAISE HEALTH
                greeblo.Health = Math.Min(greeblo.Health + CACAO_GOOD_AMT, 100);
            } else {
                greeblo.Health = Math.Max(greeblo.Health - CACAO_BAD_AMT, 0); // IF NOT, LOWER HEALTH
                greeblo.Health = Math.Min(greeblo.Health - CACAO_BAD_AMT, 100);
            }
        }

        return [greeblo.Health, greeblo.Cacao];
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