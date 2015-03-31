using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public interface Ilogin
    {
        double Connect();
        string GetPassword();
        string GetUserName();
    }
}
