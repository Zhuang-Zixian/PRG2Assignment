using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270525_PRG2Assignment
{
    class DDJBFlight : Flight
    {
        public DDJBFlight() { }
        public DDJBFlight(string fn, string o, string d, DateTime et, string s) : base(fn, o, d, et, s) { }
        private double RequestFee()
        {
            return 300;
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
