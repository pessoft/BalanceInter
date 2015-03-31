using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public class EntranceEventArgs :EventArgs
    {

        public EntranceEventArgs(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        public string UserName
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }
    }
}
