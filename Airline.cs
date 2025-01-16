namespace S10270525_PRG2Assignment
{
    // Class representing an airline
    class Airline
    {
        // Private Attributes 
        private string name;
        private string code;
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>(); // Maps flight number to flight 

        // Properties of the Airline class
        public string Name { get; set; } // Name of the airline
        public string Code { get; set; } // Code of the airline

        // Default constructor
        public Airline()
        {
            flights = new Dictionary<string, Flight>();
        }

        // Parameterized constructor to initialize an airline with specific details
        public Airline(string n, string c)
        {
            Name = n;
            Code = c;
        }

        // Method to add a flight to the airline
        public bool AddFlight(Flight flight)
        {
            if (flights.ContainsKey(flight.FlightNumber))
            {
                return false; // Flight with the same flight number already exists
            }
            flights[flight.FlightNumber] = flight;
            return true;
        }

        // Method to calculate the total fees for all flights in the airline
        public double CalculateFees()
        {
            double totalFees = 0;
            foreach (Flight flight in flights.Values)
            {
                totalFees += flight.CalculateFees();
            }
            return totalFees;
        }

        // Method to remove a flight from the airline
        public bool RemoveFlights(Flight flight)
        {
            return flights.Remove(flight.FlightNumber);
        }

        // ToString():string method 
        public override string ToString()
        {
            return $" {Name} {Code} ";
        }
    }
}