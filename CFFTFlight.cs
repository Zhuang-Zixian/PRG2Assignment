//==================================
// Student Number : S10270525J
// Student Number : S10268018C
// Student Name   : Zhuang Zixian
// Student Name   : Tan Zi En
//==================================

namespace S10270525_PRG2Assignment
{
    class CFFTFlight : Flight
    {
        public CFFTFlight() { }
        public CFFTFlight(string fn, string o, string d, DateTime et, string s, Airline airline) : base(fn, o, d, et, s, airline) { }

        private double RequestFee()
        {
            return 150;
        }

        public override double CalculateFees()
        {
            if (Destination == "SIN")
            {
                return 500 + 300 + RequestFee();
            }
            else
            {
                return 800 + 300 + RequestFee();
            }

        }
    }
}