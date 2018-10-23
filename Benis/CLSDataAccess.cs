using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Benis
{
    public class CLSDataAccess
    {
        #region Global Members
        string connectionString;
        DataSet dataSet;
        public DialogResult dialogResult = DialogResult.None;
        #endregion

        #region Constructors
        public CLSDataAccess()
        {
            InitAccessConnection();
            //while (!ConnectionIsOK()) ;
        }
        public CLSDataAccess(string connectionStr)
        {
            connectionString = connectionStr;
            //InitConnection();
        }
        #endregion

        #region Methodes

        #region Access Methodes
        public string GetAccessDBField(string TableName, string RequestedCol, string CriteriaCol, string CriteriaVal)
        {
            CLSDataAccess da = new CLSDataAccess();
            DataTable dt = da.GetAccessDataSetByQuery("select " + RequestedCol + " from " + TableName + " where " + CriteriaCol + "='" + CriteriaVal + "'").Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return null;
        }
        public string GetAccessDBField(string TableName, string RequestedCol, string CriteriaCol1, string CriteriaVal1, string CriteriaCol2, string CriteriaVal2)
        {
            CLSDataAccess da = new CLSDataAccess();
            DataTable dt = da.GetAccessDataSetByQuery("select " + RequestedCol + " from " + TableName + " where " +
                CriteriaCol1 + "='" + CriteriaVal1 + "' AND " + CriteriaCol2 + "='" + CriteriaVal2 + "'").Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return null;
        }
        public string GetAccessDBField(string TableName, string RequestedCol, string CriteriaCol, int CriteriaVal)
        {
            CLSDataAccess da = new CLSDataAccess();
            DataTable dt = da.GetAccessDataSetByQuery("select " + RequestedCol + " from " + TableName + " where " + CriteriaCol + "=" + CriteriaVal.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return null;
        }
        public string GetAccessDBField(string TableName, string RequestedCol, string CriteriaCol1, int CriteriaVal1, string CriteriaCol2, int CriteriaVal2)
        {
            CLSDataAccess da = new CLSDataAccess();
            DataTable dt = da.GetAccessDataSetByQuery("select " + RequestedCol + " from " + TableName + " where " +
                CriteriaCol1 + "=" + CriteriaVal1.ToString() + " AND " + CriteriaCol2 + "=" + CriteriaVal2.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return null;
        }
        public string GetAccessDBField(string TableName, string RequestedCol, string CriteriaCol1, string CriteriaVal1, string CriteriaCol2, int CriteriaVal2)
        {
            CLSDataAccess da = new CLSDataAccess();
            DataTable dt = da.GetAccessDataSetByQuery("select " + RequestedCol + " from " + TableName + " where " +
                CriteriaCol1 + "='" + CriteriaVal1 + "' AND " + CriteriaCol2 + "=" + CriteriaVal2.ToString() ).Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return null;
        }
        public DataSet GetAccessDataSetByQuery(string query)
        {
            dataSet = new DataSet();
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand command = new OleDbCommand(query, connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            connection.Open();
            adapter.Fill(dataSet);
            connection.Close();
            return (dataSet);
        }
        public bool ExecuteAccess(string query)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand command = new OleDbCommand(query, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                return (false);
            }
            return (true);
        }
        public object ExecuteAccessReturnScopeIdentity(string query)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            query += ";SELECT SCOPE_IDENTITY();";
            OleDbCommand command = new OleDbCommand(query, connection);
            object obj = new object();
            try
            {
                connection.Open();
                obj = command.ExecuteScalar();
                connection.Close();
            }
            catch
            {
                return (null);
            }
            return (obj);
        }
        public void InitAccessConnection()
        {

            connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\Benis.mdb";
        }
        #endregion

        #region SQL Methods
        public bool ConnectionIsOK()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                connection.Close();
                return (true);
            }
            catch
            {
                MessageBox.Show("ارتباط با سرور برقرار نمی باشد. لطفاً پس از رفع ایراد ، دکمه تأیید را انتخاب نمایید.");
                return false;
            }
        }
        public DataSet GetSQLDataSetByQuery(string query)
        {
            //while (!ConnectionIsOK()) ;
            dataSet = new DataSet();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            try
            {
                connection.Open();
                adapter.Fill(dataSet);
                connection.Close();
                return (dataSet);
            }
            catch
            {
                return null;
            }
        }
        public bool ExecuteSQLNonQuery(string query)
        {
            //while (!ConnectionIsOK()) ;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                return (false);
            }
            return (true);
        }
        public object ExecuteSQLReturnScopeIdentity(string query)
        {
            //while (!ConnectionIsOK()) ;
            SqlConnection connection = new SqlConnection(connectionString);
            query += ";SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            object obj = new object();
            try
            {
                connection.Open();
                obj = command.ExecuteScalar();
                connection.Close();
            }
            catch
            {
                return (null);
            }
            return (obj);
        }
        public void InitSQLConnection()
        {
            connectionString = File.ReadAllText(Application.StartupPath + @"\cs.txt");
            //connectionString = @"Data Source=DOUBLEUP-XP;Initial Catalog=Bodybuilding;Integrated Security=True";
        }
        #endregion

        #endregion

        #region Properties
        public string ConnectionString
        {
            set
            {
                connectionString = value;
            }
            get
            {
                return (connectionString);
            }
        }
        public DataSet CurrentDataSet
        {
            set
            {
                dataSet = value;
            }
            get
            {
                return (dataSet);
            }
        }
        #endregion

    }
}
