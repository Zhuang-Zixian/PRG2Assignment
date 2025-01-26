//==================================
// Student Number : S10270525J
// Student Number : S10268018C
// Student Name   : Zhuang Zixian
// Student Name   : Tan Zi En
//==================================

namespace S10270525_PRG2Assignment
{
    class NORMFlight : Flight
    {
        public NORMFlight() { }
        public NORMFlight(string fn, string o, string d, DateTime et, string s) : base(fn, o, d, et, s) { }
        public override double CalculateFees()
        {
            if (Destination == "SIN")
            {
                return 500 + 300;
            }
            else
            {
                return 800 + 300;
            }

        }
    }
}
