using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public class ChangedWindowStartEventArgs :EventArgs
    {
        public ChangedWindowStartEventArgs(bool winStart)
        {
            WinStart = winStart;
        }

        public bool WinStart
        {
            get;
            private set;
        }
    }
}
