using System;
using xNet.Net;
using xNet.Text;

namespace BalanceInterconnect
{
    public class Login :Ilogin
    {
        private string _username, _password;
        private double balance;

        public Login(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public double Connect()
        {

            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
                throw new LoginException();

            balance = 0;
            String content;
            using (var request = new HttpRequest())
            {
                request.UserAgent = HttpHelper.ChromeUserAgent();
                request.Cookies = new CookieDictionary();

                request.AddField("username", _username);
                request.AddField("password", _password);
                try
                {
                    content = request.Post("http://stat.interkonekt.ru").ToString();
                }
                catch (HttpException err)
                {
                    throw;
                }
                if (content.IndexOf("class=\"errors\"") != -1)
                    throw new LoginException();
                if (content.IndexOf("Ошибка авторизации. Не верная пара логин-пароль") != -1)
                    throw new LoginException();
                String strBalanse = content.Substring("success\">", "руб.</span>");

                strBalanse = strBalanse.Trim().Replace('.', ',');

                Double.TryParse(strBalanse, out balance);
                
            }
            return balance;
        }

        public string GetUserName()
        {
            return _username;
        }

        public string GetPassword()
        {
            return _password;
        }
    }
}
