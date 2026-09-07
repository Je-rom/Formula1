namespace GitFormula_1.Constants
{
    public static class ScoringConstants
    {
        public const int MaxRating = 99;

        //pace
        public const int PaceFloor = 40;
        public const double PaceMultiplier = 8.0;

        //offense
        public const int OffenseFloor = 35;
        public const double OffenseMultiplier = 10.0;

        //defense
        public const int DefenseFloor = 35;
        public const double DefenseMultiplier = 9.0;

        //racecraft
        public const int RacecraftFloor = 35;
        public const double RacecraftMultiplier = 9.0;

        //experience
        public const double ExperienceYearsMultiplier = 6.0;
        public const double ExperienceContributionsMultiplier = 5.0;

        //mentality
        public const int MentalityFloor = 30;
        public const double MentalityStreakMultiplier = 0.3;
        public const double MentalitySocialMultiplier = 6.0;

        //overall weights (should sum to 1.0)
        public const double PaceWeight = 0.18;
        public const double OffenseWeight = 0.20;
        public const double DefenseWeight = 0.14;
        public const double RacecraftWeight = 0.16;
        public const double ExperienceWeight = 0.14;
        public const double MentalityWeight = 0.18;

        public const int CurrentAlgorithmVersion = 1;
    }
}