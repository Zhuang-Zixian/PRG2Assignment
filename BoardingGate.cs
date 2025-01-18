using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270525_PRG2Assignment
{
    class BoardingGate
    {
        private string gateName;
        private bool supportsCFFT;
        private bool supportsDDJB;
        private bool supportsLWTT;
        private Flight flight;

        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate(){}

        public BoardingGate(string gn, bool cfft, bool ddjb, bool lwtt, Flight f)
        {
            GateName = gn;
            SupportsCFFT = cfft;
            SupportsDDJB = ddjb;
            SupportsLWTT = lwtt;
            Flight = f;
        }

        public double CalculateFees()
        {
            return Flight.CalculateFees();
        }

        public override string ToString()
        {
            return $"Gate Name: {GateName}, Supports CFFT: {SupportsCFFT}, Supports DDJB: {SupportsDDJB}, Supports LWTT: {SupportsLWTT}, Flight: {Flight}";
        }
    }
}
