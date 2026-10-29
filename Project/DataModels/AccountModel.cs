public class AccountModel
{
    public int Id { get; set; }
    
    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }

    public int IsAdmin { get; set; }

    public AccountModel()
    {
    }

    public AccountModel(int id, string email, string password, string fullname, int isAdmin = 0)
    {
        Id = id;
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        IsAdmin = isAdmin;
    }
}