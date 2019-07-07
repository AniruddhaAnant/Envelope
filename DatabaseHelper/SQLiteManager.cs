using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace DatabaseHelper
{
    public class SQLiteManager
    {
        private SQLiteConnection m_dbConnection = null;
        private bool newDatabase = false;
        public SQLiteManager(string dbname)
        {
            string dbFile = dbname + ".db";
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
                newDatabase = true;
            }            
            m_dbConnection = new SQLiteConnection("Data Source="+dbFile+";Version=3;");
        }

        public bool isNewDatabase()
        {
            return newDatabase;
        }
        private void OpenConnection()
        {
            if (m_dbConnection == null)
            {
                throw new Exception("Not connected to a database");
            }
            m_dbConnection.Open();            
        }

        private void CloseConnection()
        {
            if (m_dbConnection != null)
                m_dbConnection.Close();
        }

        public void ExecuteNonQuery(string sql)
        {
            OpenConnection();
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public DataTable ExecuteSelectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                OpenConnection();  //Initiate connection to the db
                cmd = m_dbConnection.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }
            CloseConnection();
            return dt;
        }
    }
}
