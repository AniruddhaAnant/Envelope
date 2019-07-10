using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        private int m_user_id;
        private string m_userName;
        private string m_password;
        private string m_key;
        private string m_uid;

        public string UserName { get => m_userName; internal set => m_userName = value; }
        public string Password { get => m_password; internal set => m_password = value; }
        public string Key { get => m_key; internal set => m_key = value; }
        public string Uid { get => m_uid; internal set => m_uid = value; }
        public int User_id { get => m_user_id; internal set => m_user_id = value; }

        public User(string userName, string password)
        {
            m_userName = userName;
            m_password = password;
            m_key = CreateGUID();
            m_uid = CreateGUID();
        }

        internal User(object[] itemArray)
        {
            m_user_id = Convert.ToInt32(itemArray[0]);
            m_userName = itemArray[1].ToString();
            m_password = itemArray[2].ToString();
            m_key = itemArray[3].ToString();
            m_uid = itemArray[4].ToString();
            
        }

        private string CreateGUID()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString.Replace("\\", "-").Replace("/", "-");
        }
    }
}
