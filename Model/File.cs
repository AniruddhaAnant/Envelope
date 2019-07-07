using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class File
    {
        private string m_fileName;
        private int m_filesId;
        private string m_fileExtension;
        private DateTime m_timestamp;
        private string m_filePath;
        private int m_folderId;

        public string FileName { get => m_fileName; private set => m_fileName = value; }
        public int FilesId { get => m_filesId; private set => m_filesId = value; }
        public string FileExtension { get => m_fileExtension; private set => m_fileExtension = value; }
        public DateTime Timestamp { get => m_timestamp; private set => m_timestamp = value; }
        public string FilePath { get => m_filePath; private set => m_filePath = value; }
        public int FolderId { get => m_folderId; private set => m_folderId = value; }

        public File(string fileName, int folderId, string filePath, string fileExtension)
        {
            m_fileName = fileName;
            m_folderId = folderId;
            m_filePath = filePath;
            m_fileExtension = fileExtension;
        }

        internal File(object[] itemArray)
        {
            m_filesId = Convert.ToInt32(itemArray[0]);
            m_fileName = itemArray[1].ToString();
            m_filePath = itemArray[2].ToString();
            m_folderId = Convert.ToInt32(itemArray[3]);
            m_fileExtension = itemArray[4].ToString();
            m_timestamp = DateTime.Parse(itemArray[5].ToString());
        }
    }
}
