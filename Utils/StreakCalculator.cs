using GitFormula_1.Providers.GitHubModels;

namespace GitFormula_1.Utils
{
    public static class StreakCalculator
    {
        public static (int longestStreak, int currentStreak) Calculate(List<DailyContribution> calendar)
        {
            if (calendar == null || calendar.Count == 0)
                return (0, 0);

            var sorted = calendar.OrderBy(d => d.Date).ToList();

            int longestStreak = 0;
            int runningStreak = 0;

            foreach (var day in sorted)
            {
                if (day.Count > 0)
                {
                    runningStreak++;
                    if (runningStreak > longestStreak)
                        longestStreak = runningStreak;
                }
                else
                {
                    runningStreak = 0;
                }
            }

            int currentStreak = 0;
            for (int i = sorted.Count - 1; i >= 0; i--)
            {
                if (sorted[i].Count > 0)
                {
                    currentStreak++;
                }
                else
                {
                    break;
                }
            }

            return (longestStreak, currentStreak);
        }
    }
}