//==================================
// Student Number : S10270525J
// Student Number : S10268018C
// Student Name   : Zhuang Zixian
// Student Name   : Tan Zi En
//==================================

namespace S10270525_PRG2Assignment
{
    // Abstract class representing a flight
    abstract class Flight
    {
        // Private Attributes
        private string flightNumber;
        private string origin;
        private string destination;
        private DateTime expectedTime;
        private string status;

        // Properties of the Flight class
        public string FlightNumber { get; set; } // Unique identifier for the flight
        public string Origin { get; set; } // Origin airport code
        public string Destination { get; set; } // Destination airport code
        public DateTime ExpectedTime { get; set; } // Expected departure or arrival time
        public string Status { get; set; } // Current status of the flight (e.g., On Time, Delayed)

        // Default constructor
        public Flight() { }

        // Parameterized constructor to initialize a flight with specific details
        public Flight(string fn, string o, string d, DateTime et, string s)
        {
            FlightNumber = fn;
            Origin = o;
            Destination = d;
            ExpectedTime = et;
            Status = s;
        }


        public virtual double CalculateFees()
        {
            return 0;
        }

        // Override of the ToString method to provide a string representation of the flight
        public override string ToString()
        {
            return $"Flight Number: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, Expected Time: {ExpectedTime}, Status: {Status}";
        }
    }
}