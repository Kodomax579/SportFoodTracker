using SQLite;

namespace SportFoodTracker.Context.Sportplan
{
    public static class SportsPlanDbConstans
    {
        public const string DatabaseFilename = "Sportplan.db3";

        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
