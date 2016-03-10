using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace PsadWebsite.App_Code
{
    public class SearchHandler
    {
        public static string globalConnectionString;
        string connectionString;
        SqlCommand command;
        SqlConnection connection;

        SearchHandler(string connectionString, string parameter)
        {
            command = new SqlCommand();
            command.Connection = connection;
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString);
        }

        public DataSet Query(string parameter)
        {
            //command
            return null;
        }
        

        //take parameter for sql query and return a data type (dataset/datarow/datatable)
        /*public static void Query(string parameter)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EIPsad"].ConnectionString);
            cmd.Connection = conn;
            cmd.CommandText = "SELECT id FROM test_table";

            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            //RepeaterTest.DataSource = ds;
            //RepeaterTest.DataBind();
        }
        */
    }
}