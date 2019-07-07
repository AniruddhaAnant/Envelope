using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model;
using FileHelper;
namespace AppTestor
{
    class Program
    {
        static void Main(string[] args)
        {
            //IEnvelopeData handler = new SQLiteHandler();
            //var olduser = handler.GetUser();

            //var folder = handler.GetFolders();

            //File file = new File("testFile", folder[0].FolderId, "C:\\temp", "txt");
            //handler.InsertFile(file);
            //handler.GetFiles(folder[0]);

            //User newUser = new User("QWERTY", "123");

            //handler.UpdateUser(olduser, newUser);

            FileManager fm = new FileManager("admin@123","C:\\temp");
            fm.ExportFile("C:\\temp\\Ll3MofAvu0Wd0XwJKC6IfA.envp", "C:\\temp\\text.txt");
        }
    }
}
