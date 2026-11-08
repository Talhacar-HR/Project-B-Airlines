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

            string sql = $"SELECT ID, VliegtuigID, VertrekLocatie, AankomstLocatie, VertreksTijd, Vertraging, VluchtNummer, Prijs FROM {Table} ORDER BY ID";
            return connection.Query<VluchtModel>(sql).AsList();
        }
    }

    // Haalt 1 vlucht op, of null als die niet bestaat.
    public VluchtModel? GetById(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, VliegtuigID, VertrekLocatie, AankomstLocatie, VertreksTijd, Vertraging, VluchtNummer, Prijs FROM {Table} WHERE ID = @ID";
            return connection.QueryFirstOrDefault<VluchtModel>(sql, new { ID = id });
        }
    }

    public void Add(VluchtModel flight)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"INSERT INTO {Table} (VliegtuigID, VertrekLocatie, AankomstLocatie, VertreksTijd, Vertraging, VluchtNummer, Prijs) VALUES (@VliegtuigID, @VertrekLocatie, @AankomstLocatie, @VertreksTijd, @Vertraging, @VluchtNummer, @Prijs)";
            connection.Execute(sql, flight);
        }
    }

    public bool Update(VluchtModel flight)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"UPDATE {Table} SET VliegtuigID = @VliegtuigID, VertrekLocatie = @VertrekLocatie, AankomstLocatie = @AankomstLocatie, VertreksTijd = @VertreksTijd, Vertraging = @Vertraging, VluchtNummer = @VluchtNummer, Prijs = @Prijs WHERE ID = @ID";
            return connection.Execute(sql, flight) > 0;
        }
    }
}
