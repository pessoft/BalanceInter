using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xNet.Net;
using System.Threading;

namespace BalanceInterconnect
{
    public class Presenter
    {
        IView _view;
        IManager _manager;
        string user, pass;
        public Presenter(IView view, IManager manager)
        {
            _view = view;
            _manager = manager;

            _view.ChangedRemember += _viewChangedRemember;
            _view.ChangetWindowStart += _viewChangetWindowStart;
            _view.Entrance += _viewEntrance;
            _view.UpdateBalance += _viewUpdateBalance;
            _view.LoadWindow += _viewLoadWindow;

            _manager.ChangedBalance += _managerChangedBalance;
            _manager.MinimumBalance += _managerMinimumBalance;
        }

        #region События из IManager
        private void _managerMinimumBalance(object sender, MinimumBalanceEventArgs e)
        {
            _view.MessageService(string.Format("Остаток на счету {0}", e.Balance), "Пополни баланс");
        }

        private void _managerChangedBalance(object sender, ChangeBalanceEventArgs e)
        {
            _view.MessageBalloon("Текущий баланс", e.Balance.ToString());
        }
        #endregion


        #region События из IView
        private void _viewLoadWindow(object sender, EventArgs e)
        {
            if (_manager.Remember)
            {
                _view.UserName = _manager.GetUserName();
                _view.Password = _manager.GetPassword();
            }
        }

        private void _viewUpdateBalance(object sender, EventArgs e)
        {
            try
            {
                _manager.UpdateBalance();
            }
            catch (HttpException err)
            {
             //   _view.MessageBalloon("Ошибка подключения", "Вход не выполнен");
            }
            catch (NullReferenceException err)
            {
               // _view.MessageBalloon("Ошибка подключения", "Вход не выполнен");
            }
        }

        private void _viewEntrance(object sender, EntranceEventArgs e)
        {
            _view.Hide();
            _manager.Remember = _view.Remember;
            UserDateRemOrAdd();

            user = e.UserName;
            pass = e.Password;

            new Thread(Conncect).Start();
            
        }

        private void _viewChangetWindowStart(object sender, ChangedWindowStartEventArgs e)
        {
            if (e.WinStart)
                _manager.AddRegister();
            else
                _manager.RemoveRegister();
        }

        private void _viewChangedRemember(object sender, ChangedRememberEventArgs e)
        {
            _manager.Remember = e.Remember;

            UserDateRemOrAdd();
        }
        #endregion

        private void Conncect()
        {
            bool notConnect = true;
            do
            {
                try
                {
                    notConnect = false;
                    _manager.SetUserData(user, pass);
                    _view.StartTimer();
                    return;
                }
                catch (LoginException err)
                {
                    _view.Show();
                    _view.MessageService(err.Message, "Ошибка авторизации");
                }
                catch (HttpException err)
                {
                    notConnect = true;
                    _view.SetNotifyText("Ошибка подключения. Вход не выполнен");
                }
                catch (Exception err)
                {
                    _view.Show();
                    _view.MessageService(err.Message, "Неизвестная ошибка");
                }
                Thread.Sleep(1000);
            }
            while (notConnect);
        }

        private void UserDateRemOrAdd()
        {
            if (!_manager.Remember)
                _manager.RemoveUserData();
            else
                _manager.RemoveUserData();
        }
    }
}
