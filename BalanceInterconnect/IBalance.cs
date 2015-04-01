using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public interface IBalance
    {
        double CurrentBalance();
        
        event EventHandler<ChangeBalanceEventArgs> ChangedBalance;
        event EventHandler<MinimumBalanceEventArgs> MinimumBalance;
    }
}
