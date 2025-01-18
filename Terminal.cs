namespace S10270525_PRG2Assignment
{
    class Terminal
    {
        private string terminalName;
        private Dictionary<string, Airline> airlines = new Dictionary<string, Airline>(); // Maps airline code to airline
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>(); // Maps flight number to flight
        private Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>(); // Maps gate name to boarding gate
        private Dictionary<string, double> gateFees = new Dictionary<string, double>(); // Maps gate name to gate fees

        public string TerminalName { get; set; }

        public Terminal()
        {
            airlines = new Dictionary<string, Airline>();
            flights = new Dictionary<string, Flight>();
            boardingGates = new Dictionary<string, BoardingGate>();
            gateFees = new Dictionary<string, double>();
        }

        public Terminal(string tn)
        {
            TerminalName = tn;
        }

        public bool AddAirline(Airline airline)
        {
            if (airlines.ContainsKey(airline.Code))
            {
                return false; // Airline with the same code already exists
            }
            airlines.Add(airline.Code, airline);
            return true;
        }

        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            if (boardingGates.ContainsKey(boardingGate.GateName))
            {
                return false; // Boarding gate with the same name already exists
            }
            boardingGates.Add(boardingGate.GateName, boardingGate);
            return true;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (Airline airline in airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline;
                }
            }
            return null;
        }
        public void PrintAirlineFees()
        {
            foreach (Airline airline in airlines.Values)
            {
                Console.WriteLine(airline.CalculateFees());
            }
        }
        public override string ToString()
        {
            return $" {TerminalName} ";
        }
    }
}
