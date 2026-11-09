using Microsoft.Data.Sqlite;
using Dapper;

public class PassagierAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/ApplicatieDB.db");
}

