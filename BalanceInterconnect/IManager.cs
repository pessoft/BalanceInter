using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public interface IManager
    {
        event EventHandler<ChangeBalanceEventArgs> ChangedBalance;
        event EventHandler<MinimumBalanceEventArgs> MinimumBalance;
        bool Remember
        { get; set;}
        void SetUserData(string user, string pass);
        void UpdateBalance();
        string GetUserName();
        string GetPassword();
        void AddRegister();
        void RemoveRegister();
        void RemoveUserData();
    }
}
