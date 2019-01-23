using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class CBR
    {
        QueryLanguage _queryLanguage;
        List<Currency> _currencyCollection = new List<Currency>();

        public CBR(QueryLanguage queryLanguage)
        {
            _queryLanguage = queryLanguage;
            LoadData();
        }

        private void LoadData()
        {
            Uri queryURI;
            var webClient = new WebClient { Proxy = null };

            var uriBuilder = new UriBuilder
            {
                Scheme = "http",
                Host = "www.cbr.ru",
                Path = _queryLanguage == QueryLanguage.RUS 
                ? "scripts/XML_daily.asp" 
                : "scripts/XML_daily.asp",
                Query = $"date_req={DateTime.Now.ToString(@"dd'/'MM'/'yyy")}"
            };

            queryURI = uriBuilder.Uri;

            using (var data = webClient.OpenRead(queryURI))
            using (var xmlReader = XmlReader.Create(data))
            {
                var xDoc = XDocument.Load(xmlReader);

                var valutes = xDoc.Descendants("Valute");
                    //from item in xDoc.Descendants("Valute")
                    //        select item;

                foreach (var valute in valutes)
                {
                    decimal value = ParsingHelper.ParseDecimal(valute.Element("Value").Value);

                    var currency = new Currency
                    {
                        CharCode = valute.Element("CharCode").Value,
                        ID = valute.Attribute("ID").Value,
                        Nominal = (int)valute.Element("Nominal"),
                        Name = valute.Element("Name").Value,
                        Value = value
                    };

                    _currencyCollection.Add(currency);

                }
            }
        }

        

        public Currency this[int index] => _currencyCollection[index];

        public Currency this[string charCode]
        {
            get
            {
                var query = _currencyCollection.Where(x => x.CharCode == charCode);
                    //from item in _currencyCollection
                    //        where item.CharCode == charCode
                    //        select item;

                // Returns null if charCode is invalid. Consider throwing exception?
                return query.FirstOrDefault();
            }
        }

        
    }

}
