using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public class Balance :IBalance
    {
        public event EventHandler<ChangeBalanceEventArgs> ChangedBalance;
        public event EventHandler<MinimumBalanceEventArgs> MinimumBalance;

        bool _minimumMsg;
        Func<double> GetBalance;
        double _currentBalance, _preBalance;
        
        public Balance(Func<double> GetBalance)
        {
            this.GetBalance = GetBalance;
            _currentBalance = 0;
            _preBalance = 0;
            _minimumMsg = false;
        }
       
        public double CurrentBalance()
        {
            _preBalance = _currentBalance;

            _currentBalance = GetBalance();

            if (_preBalance != _currentBalance)
                OnChangedBalance(new ChangeBalanceEventArgs(_currentBalance));
            if (_currentBalance <= 25 && !_minimumMsg)
            {
                _minimumMsg = true;
                OnMinimumBalance(new MinimumBalanceEventArgs(_currentBalance));
            }
            if (_currentBalance > 25)
                _minimumMsg = false;
            return _currentBalance;
        }

        protected void OnChangedBalance(ChangeBalanceEventArgs e)
        {
            if (ChangedBalance != null)
                ChangedBalance(this, e);
        }

        protected void OnMinimumBalance(MinimumBalanceEventArgs e)
        {
            if (MinimumBalance != null)
                MinimumBalance(this,e);
        }
    }
}
