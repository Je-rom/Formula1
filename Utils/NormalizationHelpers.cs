namespace GitFormula_1.Utils
{
    public static class NormalizationHelpers
    {
        public static int LogScale(double floor, double multiplier, double rawValue)
        {
            var score = floor + Math.Log2(rawValue + 1) * multiplier;
            return Clamp(score);
        }

        public static int Clamp(double score)
        {
            var clamped = Math.Min(Constants.ScoringConstants.MaxRating, Math.Max(0, score));
            return (int)Math.Round(clamped);
        }
    }
}