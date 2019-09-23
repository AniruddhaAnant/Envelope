using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Folder
    {
        private string m_folderName;
        private DateTime m_timestamp;
        private int m_folderId;
        private int m_userId;
        public string FolderName { get => m_folderName; internal set => m_folderName = value; }
        public DateTime Timestamp { get => m_timestamp; internal set => m_timestamp = value; }
        public int FolderId { get => m_folderId; internal set => m_folderId = value; }
        public int UserId { get => m_userId; internal set => m_userId = value; }

        public bool IsRootFolder { get { return m_folderName == "root"; } }

        public Folder(string folderName, User user)
        {
            m_folderName = folderName;
            UserId = user.User_id;
        }

        internal Folder(object[] itemArrey)
        {
            m_folderId = Convert.ToInt32(itemArrey[0]);
            m_folderName = itemArrey[1].ToString();
            m_userId = Convert.ToInt32(itemArrey[2]);
            m_timestamp = DateTime.Parse(itemArrey[3].ToString());
        }
    }
}
