using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public class ChangedRememberEventArgs :EventArgs
    {
        public ChangedRememberEventArgs(bool remember)
        {
            Remember = remember;
        }

        public bool Remember
        {
            get;
            private set;
        }
    }
}
