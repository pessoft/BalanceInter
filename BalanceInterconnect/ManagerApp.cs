using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace BalanceInterconnect
{
    public class ManagerApp :IManager
    {
        public event EventHandler<ChangeBalanceEventArgs> ChangedBalance;
        public event EventHandler<MinimumBalanceEventArgs> MinimumBalance;
        IRegister register;
        IBalance balance;
        Ilogin login;
        string pathKey ="remember.ibk";
        string username, password;
        public ManagerApp()
        {
            Init();
        }

        private void Init()
        {
            if (File.Exists(pathKey))
            {
                string[] userData = File.ReadAllText(pathKey).Split(';');
                if (userData.Length == 2)
                {
                    username = userData[0];
                    password = userData[1];
                    if (!String.IsNullOrEmpty(username) || !String.IsNullOrEmpty(password))
                        Remember = true;
                }
            }
            else
                Remember = false;

            register = RegisterApp.GetInstance();

        }

        public bool Remember
        { get; set; }


        public void SetUserData(string user, string pass)
        {
            username = user;
            password = pass;
            login = new Login(user, pass);
            balance = new Balance(login.Connect);
            balance.ChangedBalance += BalanceChangedBalance;
            balance.MinimumBalance += BalanceMinimumBalance;

            balance.CurrentBalance();
        }

        private void BalanceMinimumBalance(object sender, MinimumBalanceEventArgs e)
        {
            OnMinimumBalance(e);
        }

        private void BalanceChangedBalance(object sender, ChangeBalanceEventArgs e)
        {
            OnChangedBalance(e);
        }

        public void UpdateBalance()
        {
            balance.CurrentBalance();
        }

        public string GetUserName()
        {
            return username;
        }

        public string GetPassword()
        {
            return password;
        }

        public void AddRegister()
        {
            register.Add();
        }

        public void RemoveRegister()
        {
            register.Remove();
        }

        public void RememberUserData()
        {
            string data = GetUserName() + ";" + GetPassword();
            try
            {
                File.WriteAllText(pathKey, data);
            }
            catch (IOException err)
            { }
        }

        public void RememberUserData(string username, string password)
        {
            string data = username + ";" + password;
            try
            {
                File.WriteAllText(pathKey, data);
            }
            catch (IOException err)
            { }
        }
        public void RemoveUserData()
        {
            try
            {
                if (File.Exists(pathKey))
                    File.Delete(pathKey);
            }
            catch (IOException err)
            { }
        }

        protected void OnChangedBalance(ChangeBalanceEventArgs e)
        {
            if (ChangedBalance != null)
                ChangedBalance(this, e);
        }

        protected void OnMinimumBalance(MinimumBalanceEventArgs e)
        {
            if (MinimumBalance != null)
                MinimumBalance(this, e);
        }
    }
}
