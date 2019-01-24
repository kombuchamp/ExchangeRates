using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WpfExchangeRates.FinanceData
{
    public class Currency
    {
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string ID { get; set; }

        public Dictionary<DateTime, decimal> Dynamics { get; set; } = null;

        public Dictionary<DateTime, decimal> LoadDynamics(IDynamicsLoader loader, DateTime initialDate, DateTime terminalDate)
        {
            if (Dynamics != null)
                return Dynamics;
            else
                Dynamics = loader.LoadDynamics(this, initialDate, terminalDate);

            return Dynamics;
        }
    }
}
