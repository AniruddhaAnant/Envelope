using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelper;

namespace Model
{
    public class FileHandler
    {
        private static string APPLICATION_FILES_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private FileManager manager;
        public FileHandler(string key,string uid)
        {
            manager = new FileManager(key,Path.Combine(APPLICATION_FILES_PATH,"Envelope\\"+uid));
        }

        public string ImportFile(string filepath)
        {
            return manager.ImportFile(filepath);
        }

        public void ExportFile(string encfilepath,string decfilepath)
        {
            manager.ExportFile(encfilepath, decfilepath);
        }
    }
}
