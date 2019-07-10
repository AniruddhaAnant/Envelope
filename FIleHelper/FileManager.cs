using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class FileManager
    {
        private string m_key;
        private string m_secretPath;
        private string TempDir = Path.GetTempPath();
        public FileManager(string key, string secretPath)
        {
            m_key = key;
            m_secretPath = secretPath;
            if (!Directory.Exists(secretPath))
                Directory.CreateDirectory(secretPath);
        }

        public string ImportFile(string filePath)
        {
            Crypt crypt = new Crypt();
            var updatedFilePath = TempDir + Path.GetFileName(filePath);
            File.Copy(filePath,updatedFilePath,true);
            crypt.FileEncrypt(updatedFilePath,m_key);
            var newPath = Path.Combine(m_secretPath, CreateGUID()+".envp");
            File.Move(updatedFilePath+".aes", newPath);
            File.Delete(updatedFilePath);
            return newPath;
        }

        public string ExportFile(string encryptedFilePath, string decryptedFilePath)
        {
            Crypt crypt = new Crypt();
            crypt.FileDecrypt(encryptedFilePath,decryptedFilePath,m_key);
            return decryptedFilePath;
        }

        private string CreateGUID()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString.Replace("\\", "-").Replace("/","-") ;
        }
    }
}
