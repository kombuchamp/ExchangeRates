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

namespace ExchangeRatesConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string currDate = DateTime.Now.ToString(@"dd'/'mm'/'yyy");
            Console.WriteLine(currDate);
            
            

            Console.ReadKey();
        }

        private static void ClearConsole(Uri uri, string s)
        {
            Console.Clear();
            Console.WriteLine("Getting data from\n" + uri.ToString() + "...\n");
            Console.WriteLine($"{s.Length} symbols read into string.\n\n\n");
            Console.WriteLine("Attempting to parse XML...");
        }


        private static void SomeXmlFetchingThing()
        {
            // Some output arrangements
            Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight * 3);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Beep();


            var _CBR = new CBR();

            var uri = _CBR.GetUri();


            var webClient = new WebClient
            {
                Proxy = null
            };
            Stream data = webClient.OpenRead(uri);
            string s = new StreamReader(data, Encoding.GetEncoding("Windows-1251")).ReadToEnd();

            ClearConsole(uri, s);


            //Способ чтения xml напрямую из стрима
            Console.WriteLine("Первый способ");
            using (var xmlReader = XmlReader.Create(new StringReader(s)))
            {
                while (xmlReader.Read())
                {
                    Console.Write(">");
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            Console.WriteLine("Element :: " + xmlReader.Name);
                            break;
                        case XmlNodeType.Text:
                            Console.WriteLine("Text :: " + xmlReader.Value);
                            break;
                        case XmlNodeType.XmlDeclaration:
                            Console.WriteLine("XmlDecl ::");
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            Console.WriteLine("ProcInst ::" + xmlReader.Name + " " + xmlReader.Value);
                            break;
                        case XmlNodeType.Comment:
                            Console.WriteLine("//" + xmlReader.Value);
                            break;
                        case XmlNodeType.EndElement:
                            Console.WriteLine("End element :: ");
                            break;
                    }
                }
            }

            ClearConsole(uri, s);
            Console.WriteLine("Второй способ");
            using (var xmlReader = XmlReader.Create(new StringReader(s)))
            {
                while (xmlReader.ReadToFollowing("Valute"))
                {
                    xmlReader.MoveToFirstAttribute();
                    Console.WriteLine(xmlReader.Name + "=" + xmlReader.Value);


                    xmlReader.ReadToFollowing("Nominal");
                    Console.Write("\t" + xmlReader.Value);

                    xmlReader.ReadToFollowing("Name");
                    Console.WriteLine(" " + xmlReader.Value);

                    //xmlReader.ReadToFollowing("Value");
                    //Console.WriteLine(" " + xmlReader.ReadElementContentAsString() + ">>>" + xmlReader.NodeType);
                }
            }




            // Чтение xml с помощью типа XmlReader
            // https://stackoverflow.com/questions/642293/how-do-i-read-and-parse-an-xml-file-in-c
            ClearConsole(uri, s);
            Console.WriteLine("Третий способ, XmlDocument");
            using (var xmlReader = XmlReader.Create(new StringReader(s)))
            {
                var doc = new XmlDocument();
                doc.Load(xmlReader);

                Console.WriteLine("Number of attributes: " + doc.DocumentElement.Attributes.Count);
                Console.WriteLine("BaseURI: " + doc.DocumentElement.BaseURI);
                Console.WriteLine("DocumentElement:" + doc.DocumentElement);
                Console.WriteLine("Name: " + doc.DocumentElement.Name);
                Console.WriteLine("Value: " + doc.DocumentElement.Value);
                Console.WriteLine("Date attribute: " + doc.DocumentElement.Attributes["Date"]?.Value);

                Console.WriteLine("\n\nIterating through child nodes...");
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    Console.WriteLine(node.InnerText);
                }
            }


            // Linq to XML. тип XDocument
            ClearConsole(uri, s);

            using (var xmlReader = XmlReader.Create(new StringReader(s)))
            {
                var xDoc = XDocument.Load(xmlReader);

                // linq запрос
                var query = from c in xDoc.Root.Descendants("Valute")
                            where double.Parse(c.Element("Nominal").Value) == 1
                            select c.Element("Name").Value + " = "
                                                + c.Element("Value").Value + "руб";

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

            }

            Decimal.TryParse("12.2", NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedCurrencyValue);
            Console.WriteLine($"{parsedCurrencyValue}");
        }
    }


    class CBR
    {
        public Uri GetUri()
        {
            var builder = new UriBuilder
            {
                Scheme = "http",
                Host = "www.cbr.ru",
                Path = "scripts/XML_daily.asp",
                Query = "date_req=02/03/2002"
            };

            return builder.Uri;
        }


        public void ReadXMLDirectlyFromStream(string s)
        {

        }

    }
}
