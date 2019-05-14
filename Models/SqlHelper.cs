using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Automation.API.Models
{
    public class SqlHelper
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public bool TrustedConnection { get; set; }
        public string UserId { get; set; }
        public string PassWord { get; set; }

        public SqlHelper(string server, string db, bool tc = true)
        {
            Server = server;
            Database = db;
            TrustedConnection = tc;
        }

        public SqlHelper(string server, string db, string userid, string pw)
        {
            Server = server;
            Database = db;
            UserId = UserId;
            PassWord = pw;
        }

        public bool GetConnection()
        {
            try
            {
                Connection();
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }


        private SqlConnection Connection()
        {
            if (Server == null || Database == null)
            {
                throw new NullReferenceException(nameof(SqlHelper));
            }
            SqlConnection conn = new SqlConnection();
            if (TrustedConnection)
            {
                conn = new SqlConnection(@"Data Source=" + Server + "; Initial Catalog=" + Database + ";Trusted_connection=yes");
            }
            else
            {
                conn = new SqlConnection(@"Data Source=" + Server + "; Initial Catalog=" + Database + ";User Id=" + UserId + ";Password=" + PassWord);
            }
            conn.Open();
            return conn;
        }

        public IEnumerable<List<Object>> ExecuteReader(string command)
        {
            SqlConnection conn = Connection();
            SqlCommand cm = new SqlCommand(command, conn);
            using (var reader = cm.ExecuteReader())
            {
                var indices = Enumerable.Range(0, reader.FieldCount).ToList();
                foreach (IDataRecord record in reader as IEnumerable)
                    yield return indices.Select(i => record[i]).ToList();
            }

        }
        public string ExecuteWriter(string command)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = new SqlCommand(command, conn);
                adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (System.Exception)
            {

                throw new Exception(nameof(command));
            }
            return "insert successful";
        }

        public string ExecuteUpdate(string command)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.UpdateCommand = new SqlCommand(command, conn);
                adapter.UpdateCommand.ExecuteNonQuery();

            }
            catch (System.Exception)
            {

                throw new Exception(nameof(command));
            }
            return "update successful";
        }
        public string ExecuteDelete(string command)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.DeleteCommand = new SqlCommand(command, conn);
                adapter.DeleteCommand.ExecuteNonQuery();

            }
            catch (System.Exception)
            {

                throw new Exception(nameof(command));
            }
            return "delete successful";
        }
    }
}