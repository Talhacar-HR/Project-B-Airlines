public class FlightLogic
{
    private FlightAccess _access = new();

    public List<FlightModel> GetAvailableFlights()
    {
        List<FlightModel> allFlights = _access.GetAllFlights();
        List<FlightModel> availableFlights = new List<FlightModel>();

        foreach (FlightModel flight in allFlights)
        {
            if (GetDepartureMoment(flight) > DateTime.Now)
            {
                availableFlights.Add(flight);
            }
        }

        availableFlights.Sort(CompareByDeparture);

        return availableFlights;
    }

    private DateTime GetDepartureMoment(FlightModel flight)
    {
        return DateTime.Parse(flight.DepartureDate + " " + flight.DepartureTime);
    }

    private int CompareByPrice(FlightModel flight1, FlightModel flight2)
    {
        return flight1.Price.CompareTo(flight2.Price);
        // Icompareable is niet nodig, omdat price al een Compare methode heeft.
        // we vergelijken namelijk de prijzen niet de objecten zelf.
    }

    private int CompareByDeparture(FlightModel flight1, FlightModel flight2)
    {
        return GetDepartureMoment(flight1).CompareTo(GetDepartureMoment(flight2));
    }

    public List<FlightModel> FlightFilter(string departureDate, string origin, string destination)
    {
        List<FlightModel> availableFlights = GetAvailableFlights();
        List<FlightModel> filteredFlights = [];

        foreach(FlightModel flight in availableFlights)
        {
           if (flight.DepartureDate == departureDate && flight.Origin == origin 
           && flight.Destination == destination)
            {
                filteredFlights.Add(flight);
            }
        }
        return filteredFlights;
    }

    public List<FlightModel> SortFlights(List<FlightModel> flights, string sortby)
    {
        if (sortby == "price")
        {
            flights.Sort(CompareByPrice);
        }
        else if (sortby == "departure")
        {
            flights.Sort(CompareByDeparture);
        }
        return flights;
    }
}
