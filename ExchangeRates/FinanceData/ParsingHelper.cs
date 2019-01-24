using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;

namespace WpfExchangeRates.FinanceData
{
    class ParsingHelper
    {
        // Only way I found to treat decimal sepatator independantly from user settings
        public static decimal ParseDecimal(string arg)
        {
            decimal.TryParse(
                        arg.Replace(',', '.'),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out decimal value);
            return value;
        }
    }
}
