namespace Core.Constants
{
    public static class ValidationConstants
    {
        // User
        public const int MinAge = 1;
        public const int MaxAge = 100;
        public const double MinHeightCm = 30;
        public const double MaxHeightCm = 300;
        public const double MinWeightKg = 1;
        public const double MaxWeightKg = 1000;
        public const int MaxFullNameLength = 100;

        // Meal
        public const int MinCalories = 1;
        public const int MaxCalories = 10000;
        public const int MaxDescriptionLength = 255;
    }
}
