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

    private int CompareByDeparture(FlightModel flight1, FlightModel flight2)
    {
        return GetDepartureMoment(flight1).CompareTo(GetDepartureMoment(flight2));
    }
}
