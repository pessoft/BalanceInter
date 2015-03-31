using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalanceInterconnect
{
    public class RegisterApp :IRegister
    {
        Microsoft.Win32.RegistryKey _key;
        string nameApp;
        static IRegister register;

        private RegisterApp()
        {
            _key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
            nameApp = "BalanceInter";
        }

        public static IRegister GetInstance()
        {
            if (register == null)
                register = new RegisterApp();

            return register;
        }
        public void Add()
        {
            _key.SetValue(nameApp, System.Reflection.Assembly.GetExecutingAssembly().Location);
            _key.Flush();
        }

        public void Remove()
        {
            if (_key.GetValue(nameApp) != null)
                _key.DeleteValue(nameApp);
            _key.Flush(); 
        }

        public bool IsRegister()
        {
            bool result = false;

            if (_key.GetValue(nameApp) != null)
                result = true;
            return result;
        }
        ~RegisterApp()
        {
            _key.Close();
        }
    }
}
