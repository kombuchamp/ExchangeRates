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

        public Dictionary<DateTime, decimal> LoadDynamics(IDynamicsLoader loader ,DateTime initialDate, DateTime terminalDate)
        {
            if (Dynamics != null)
                return Dynamics;
            else
                Dynamics = new Dictionary<DateTime, decimal>();

            Uri queryURI;
            var webClient = new WebClient { Proxy = null };

            var uriBuilder = new UriBuilder
            {
                Scheme = "http",
                Host = "www.cbr.ru",
                Path = "scripts/XML_dynamic.asp",
                Query = "date_req1=" + initialDate.ToString(@"dd'/'MM'/'yyy") +
                        "&date_req2=" + terminalDate.ToString(@"dd'/'MM'/'yyy") +
                        "&VAL_NM_RQ=" + this.ID
            };

            queryURI = uriBuilder.Uri;


            using (Stream data = webClient.OpenRead(queryURI))
            using (var xmlReader = XmlReader.Create(data))
            {
                var xDoc = XDocument.Load(xmlReader);


                var query = from item in xDoc.Descendants("Record")
                            select new
                            {
                                date = item.Attribute("Date").Value,
                                value = item.Element("Value").Value,
                            };

                foreach (var item in query)
                {
                    decimal value = ParsingHelper.ParseDecimal(item.value);
                    DateTime date = DateTime.Parse(item.date);

                    Dynamics.Add(date, value);
                }
            }

            return Dynamics;
        }
    }
}
