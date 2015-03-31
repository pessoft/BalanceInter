using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public class MinimumBalanceEventArgs :EventArgs
    {
        public MinimumBalanceEventArgs(double balance)
        {
            Balance = balance;
        }

        public double Balance
        {
            get;
            private set;
        }
    }
}
