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
        public Dictionary<string, Flight> Flights { get; set; } // Flights operated by the airline

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
            flights.Add(flight.FlightNumber, flight);
            flight.Airline = this; // setting the airline to reference this in the flight class
            return true;
        }

        // Method to calculate the total fees for all flights in the airline
        public double CalculateFees()
        {
            double totalFees = 0;
            int flightCount = flights.Count;

            // Calculate base fees for each flight
            foreach (Flight flight in flights.Values)
            {
                totalFees += flight.CalculateFees();
            }

            // Apply 3% discount if the airline has more than 5 flights
            if (flightCount > 5)
            {
                totalFees *= 0.97; // 3% off the total bill
            }

            // Apply other discounts
            foreach (Flight flight in flights.Values)
            {
                // Apply discount for flights before 11am or after 9pm
                if (flight.ExpectedTime.TimeOfDay < new TimeSpan(11, 0, 0) || flight.ExpectedTime.TimeOfDay > new TimeSpan(21, 0, 0))
                {
                    totalFees -= 110;
                }

                // Apply discount for flights with the Origin of Dubai (DXB), Bangkok (BKK) or Tokyo (NRT)
                if (flight.Origin == "DXB" || flight.Origin == "BKK" || flight.Origin == "NRT")
                {
                    totalFees -= 25;
                }

                // Apply discount for flights not indicating any Special Request Codes
                if (flight is CFFTFlight || flight is LWTTFlight || flight is DDJBFlight)
                {
                    totalFees -= 50;
                }
            }

            // Apply discount for every 3 flights
            totalFees -= (int)(flightCount / 3) * 350;

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