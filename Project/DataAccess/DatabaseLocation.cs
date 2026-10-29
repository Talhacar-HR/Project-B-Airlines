static class DatabaseLocation
{
    public static string ConnectionString { get; } = $"Data Source={GetDatabasePath()}";

    private static string GetDatabasePath()
    {
        DirectoryInfo? directory = new(AppContext.BaseDirectory);
        while (directory != null)
        {
            string projectFile = Path.Combine(directory.FullName, "Project.csproj");
            if (File.Exists(projectFile))
            {
                return Path.Combine(directory.FullName, "DataSources", "ApplicatieDB.db");
            }

            directory = directory.Parent;
        }

        return Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");
    }
}
