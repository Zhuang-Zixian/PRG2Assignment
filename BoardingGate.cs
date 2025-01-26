//==================================
// Student Number : S10270525J
// Student Number : S10268018C
// Student Name   : Zhuang Zixian
// Student Name   : Tan Zi En
//==================================

namespace S10270525_PRG2Assignment
{
    class BoardingGate
    {
        // Private Attributes
        private string gateName;
        private bool supportsCFFT;
        private bool supportsDDJB;
        private bool supportsLWTT;
        private Flight flight;

        // Public Attributes
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        // Default Constructor
        public BoardingGate() { }

        // Parameterised Constructor
        public BoardingGate(string gn, bool cfft, bool ddjb, bool lwtt, Flight f)
        {
            GateName = gn;
            SupportsCFFT = cfft;
            SupportsDDJB = ddjb;
            SupportsLWTT = lwtt;
            Flight = f;
        }

        // CalculateFees() Method
        public double CalculateFees()
        {
            return Flight.CalculateFees();
        }

        // ToString Method
        public override string ToString()
        {
            return $"Gate Name: {GateName}, Supports CFFT: {SupportsCFFT}, Supports DDJB: {SupportsDDJB}, Supports LWTT: {SupportsLWTT}, Flight: {Flight}";
        }
    }
}
