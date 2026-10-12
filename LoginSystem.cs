using System;
using Microsoft.Data.Sqlite;

public class LoginSystem
{
    public void Start()
    {
        Console.Clear();
        Console.WriteLine("--- Rotterdam Airlines Login ---");
        
        Console.Write("Enter your email address: ");
        string email = Console.ReadLine();

        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        string connectionString = "Data Source=RotterdamAirlines.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            // We zoeken naar het wachtwoord en de voornaam op basis van het ingevulde e-mailadres
            command.CommandText = "SELECT Password, FirstName FROM Accounts WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);

            using (var reader = command.ExecuteReader())
            {
                // Als reader.Read() true is, dan staat het e-mailadres in de database
                if (reader.Read())
                {
                    string dbPassword = reader.GetString(0);
                    string firstName = reader.GetString(1);

                    if (password == dbPassword)
                    {
                        Console.WriteLine($"\nWelcome to Rotterdam Airlines, {firstName}! You have successfully logged in.");
                    }
                    else
                    {
                        Console.WriteLine("\nError: The password entered is incorrect.");
                    }
                }
                else
                {
                    Console.WriteLine("\nError: The provided email address was not found in our system.");
                }
            }
        }
    }
}