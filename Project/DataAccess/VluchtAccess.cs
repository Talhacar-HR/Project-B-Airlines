using Dapper;
using Microsoft.Data.Sqlite;

public class VluchtAccess
{
    private string Table = "Vlucht";

    public List<VluchtModel> GetAll()
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, VliegtuigID, VertrekLocatie, AankomstLocatie, VertreksTijd, Vertraging, VluchtNummer FROM {Table} ORDER BY ID";
            return connection.Query<VluchtModel>(sql).AsList();
        }
    }

    public void Add(VluchtModel flight)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"INSERT INTO {Table} (VliegtuigID, VertrekLocatie, AankomstLocatie, VertreksTijd, Vertraging, VluchtNummer) VALUES (@VliegtuigID, @VertrekLocatie, @AankomstLocatie, @VertreksTijd, @Vertraging, @VluchtNummer)";
            connection.Execute(sql, flight);
        }
    }

    public bool Update(VluchtModel flight)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"UPDATE {Table} SET VliegtuigID = @VliegtuigID, VertrekLocatie = @VertrekLocatie, AankomstLocatie = @AankomstLocatie, VertreksTijd = @VertreksTijd, Vertraging = @Vertraging, VluchtNummer = @VluchtNummer WHERE ID = @ID";
            return connection.Execute(sql, flight) > 0;
        }
    }
}
