public class FlightModel
{
    public Int64 FlightNumber { get; set; }

    public string Origin { get; set; }

    public string Destination { get; set; }

    public string DepartureDate { get; set; }

    public string DepartureTime { get; set; }
    public FlightModel()
    {
    }

    public FlightModel(Int64 flightNumber, string origin, string destination, string departureDate, string departureTime)
    {
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        DepartureDate = departureDate;
        DepartureTime = departureTime;
    }
}
