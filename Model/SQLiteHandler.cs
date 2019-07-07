using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHelper;
namespace Model
{
    public class SQLiteHandler : IEnvelopeData
    {
        private SQLiteManager m_manager;
        public SQLiteHandler()
        {
            m_manager = new SQLiteManager("envelope");
            if (m_manager.isNewDatabase())
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            const string userTable = "CREATE TABLE users(" +
                                        "user_id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                        "username VARCHAR(100) UNIQUE NOT NULL," +
                                        "password VARCHAR(20) NOT NULL," +
                                        "key VARCHAR(200)," +
                                        "uid VARCHAR(100)" +
                                    ")";

            const string folderTable = "CREATE TABLE folders(" +
                                            "folder_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                            "FolderName VARCHAR(50) NOT NULL, " +
                                            "user_id INT NOT NULL," +
                                            "timestamp DATETIME DEFAULT CURRENT_TIMESTAMP," +
                                            "FOREIGN KEY(user_id) REFERENCES users(user_id)" +
                                       ")";

            const string fileTable = "CREATE TABLE files(" +
                                      "file_id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                      "FileName VARCHAR(100) NOT NULL," +
                                      "Path VARCHAR(500) NOT NULL," +
                                      "folder_id INT NOT NULL," +
                                      "extension VARCHAR(10) NOT NULL," +
                                      "timestamp DATETIME DEFAULT CURRENT_TIMESTAMP," +
                                      "FOREIGN KEY(folder_id) REFERENCES folders(folder_id))";
            //CREATE TABLES 
            m_manager.ExecuteNonQuery(userTable);
            m_manager.ExecuteNonQuery(folderTable);
            m_manager.ExecuteNonQuery(fileTable);

            //ADD DEFAULT USER TO THE SYSTEM
            User user = new User("EnvelopeUser", "admin@123");
            InsertUser(user);
            user = GetUser();

            //ADD ROOT FOLDER ENTRY IN DB.folders
            Folder folder = new Folder("root", user);
            InsertFolder(folder);
        }
        public List<File> GetFiles(Folder folder)
         {
            string sql = "SELECT * FROM files WHERE folder_id =" + folder.FolderId + " ";
            var file = m_manager.ExecuteSelectQuery(sql);

            List<File> files = new List<File>();

            for (int i = 0; i < file.Rows.Count; i++)
            {
                File currentfile = new File(file.Rows[i].ItemArray);
                files.Add(currentfile);
            }
            return files;
        }

        public List<Folder> GetFolders()
        {
            const string sql = "SELECT * FROM folders";
            var folder = m_manager.ExecuteSelectQuery(sql);

            List<Folder> folders = new List<Folder>();
            User user = GetUser();
            for (int i = 0; i < folder.Rows.Count; i++)
            {
                Folder currentFolder = new Folder(folder.Rows[i].ItemArray);              
                folders.Add(currentFolder);
            }
            return folders;
        }

        public User GetUser()
        {
            const string sql = "SELECT * FROM users";
            var user = m_manager.ExecuteSelectQuery(sql);

            User newUser = new User(user.Rows[0].ItemArray);            
            return newUser;
        }

        public void InsertFile(File file)
        {
            string sql = "INSERT INTO files(FileName, Path, folder_id, extension)" +
                "VALUES('" + file.FileName + "','" + file.FilePath + "'," + file.FolderId + ",'" + file.FileExtension + "')";
            m_manager.ExecuteNonQuery(sql);
        }

        public void InsertFolder(Folder folder)
        {
            string sql = "INSERT INTO folders(FolderName,user_id)" +
                        "VALUES('" + folder.FolderName + "'," + folder.UserId + ")";
            m_manager.ExecuteNonQuery(sql);
        }

        public void InsertUser(User user)
        {
            string sql = "INSERT INTO users(username,password,key,uid)" +
                         "VALUES('" + user.UserName + "', '" + user.Password + "','" + user.Key + "','" + user.Uid + "')";
            m_manager.ExecuteNonQuery(sql);
        }

        public void UpdateUser(User oldUser, User newUser)
        {
            string sql = "UPDATE users SET username= '" + newUser.UserName + "', password= '"+newUser.Password+"' " +
                "WHERE username= '" + oldUser.UserName + "' AND password= '" + oldUser.Password + "'";
            m_manager.ExecuteNonQuery(sql);
        }
    }
}
