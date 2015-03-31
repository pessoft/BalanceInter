using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public class ChangeBalanceEventArgs :EventArgs
    {
        public ChangeBalanceEventArgs(double balance)
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
