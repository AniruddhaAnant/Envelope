using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Helpers;

namespace ViewModels
{
    public class LoginVM: ViewModelBase
    {
        private string m_userName= "EnvelopeUser";
        private string m_password= "admin@123";
        private Command m_loginCommand;
        private bool m_isLoggedIn = false;

        public bool IsLoggedIn
        {
            get { return m_isLoggedIn; }
            set
            {
                m_isLoggedIn = value;
                RaiseEvent("IsLoggedIn");
            }
        }

        public string UserName
        {
            get { return m_userName; }
            set { m_userName = value; }
        }

        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        public Command LoginCommand
        {
            get
            {
                if (m_loginCommand == null)
                {
                    m_loginCommand = new Command(Login, CanLogin);

                }
                return m_loginCommand;
            }
        }

        private bool CanLogin(object arg)
        {
            return m_userName != "" && m_password != "";
        }

        private void Login(object obj)
        {
            SQLiteHandler handler = new SQLiteHandler();
            User user = handler.GetUser();
            if(user.UserName == UserName && user.Password == Password)
            {
                IsLoggedIn = true;
            }
        }
    }
}
