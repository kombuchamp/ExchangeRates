﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExchangeRates.FinanceData
{
    public interface IDynamicsLoader
    {
        Dictionary<DateTime, decimal> LoadDynamics(Currency currency, DateTime initialDate, DateTime terminalDate);
    }
}
