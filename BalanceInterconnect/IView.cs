using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public interface IView
    {
        void MessageBalloon(string title, string text);
        void MessageService(string text, string caption);
        void Hide();
        void Show();

        string UserName
        {
            get;
            set;
        }

        string Password
        {
            get;
            set;
        }

        bool Remember
        {
            get;
        }

        void StartTimer();
        void SetNotifyText(string text);
        event EventHandler<EntranceEventArgs> Entrance;
        event EventHandler<ChangedWindowStartEventArgs> ChangetWindowStart;
        event EventHandler<ChangedRememberEventArgs> ChangedRemember;
        event EventHandler UpdateBalance;
        event EventHandler LoadWindow;
    }
}
