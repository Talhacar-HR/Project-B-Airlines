public class UserModel
{
    public int Id { get; set; }
 
    public string Email { get; set; }
 
    public string Password { get; set; }
 
    // Lege constructor zodat Dapper de gebruiker uit de database kan inlezen
    public UserModel()
    {
    }
 
    public UserModel(int id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = password;
    }
}