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
        private static string ENVELOPE_TEMP_PATH = Path.Combine(APPLICATION_FILES_PATH, "Envelope\\Temp\\");
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

        public void DeleteFile(string filepath)
        {
            if(System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
        }

        public string DecryptToTemp(string encfilepath,string filenameWithExtension)
        {
            if (!System.IO.Directory.Exists(ENVELOPE_TEMP_PATH))
            {
                System.IO.Directory.CreateDirectory(ENVELOPE_TEMP_PATH);
            }
            string tempExportPath = Path.Combine(ENVELOPE_TEMP_PATH, filenameWithExtension);
            ExportFile(encfilepath,tempExportPath);
            return tempExportPath;
        }

        ~FileHandler()
        {
            if(System.IO.Directory.Exists(ENVELOPE_TEMP_PATH))
            {
                System.IO.Directory.Delete(ENVELOPE_TEMP_PATH, true);
            }
        }
    }
}
