static class Menu
{
 
    static public void Start()
    {
        Console.WriteLine("Enter 1 for Inloggen");
        Console.WriteLine("Enter 2 for Registreren");
        Console.WriteLine("Enter 3 for Beschikbare vluchten bekijken");
 
        string input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
            Start();
        }
        else if (input == "2")
        {
            UserRegistration.Start();
            Start();
        }
        else if (input == "3")
        {
            FlightOverview.Start();
            Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
 
    }
}