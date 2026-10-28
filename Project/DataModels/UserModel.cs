public class UserModel
{
    public Int64 Id { get; set; }
 
    public string Email { get; set; }
 
    public string Password { get; set; }
 
    // Lege constructor zodat Dapper de gebruiker uit de database kan inlezen
    public UserModel()
    {
    }
 
    public UserModel(Int64 id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = password;
    }
}