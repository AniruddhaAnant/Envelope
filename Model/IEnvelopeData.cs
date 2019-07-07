using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IEnvelopeData
    {
        void InsertUser(User user);

        void UpdateUser(User oldUser, User newUser);
        User GetUser();

        void InsertFolder(Folder folder);
        List<Folder> GetFolders();

        void InsertFile(File file);
        List<File> GetFiles(Folder folder);
    }
}
