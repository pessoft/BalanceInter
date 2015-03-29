using System;

namespace BalanceInterconnect
{
    public class LoginException :Exception
    {
        public LoginException() :base("Неверные имя пользователя или пароль")
        { }
    }
}
