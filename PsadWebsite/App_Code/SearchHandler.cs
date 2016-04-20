using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using PsadWebsite.App_Code.Repository;

namespace PsadWebsite.App_Code
{
    // Get patients based on operator
    // Get measurement data from specific patient
    // Get patients measurement data from specific age range

    // plain query should search through all names, so organisations names, patients, operators


    public class SearchHandler
    {
        private static string q = "q"; // for basic queries
        private static string g = "g"; // for specific groups

        public static string Q { get { return q; } }

        public static string QueryFormatBase(string queryId)
        {
            return "?" + queryId + "=";
        }

        public static string QueryFormatCont(string queryId)
        {
            return "&" + queryId + "=";
        }

        public static string QueryString(string url, string query)
        {
            return url + QueryFormatBase(Q) + query;
        }

        public static string QueryString(string url, string query, string grouping)
        {
            return url + QueryFormatBase(Q) + query + QueryFormatCont(g) + grouping;
        }

        public static DataTable GetPeople(string name)
        {
            SqlParameter para = new SqlParameter("@name", SqlDbType.NVarChar, 50);
            para.Value = name;
            return SqlHandler.QueryDataTable("GetPeopleByName");
        }


        public static DataTable GetPatient(Guid patientId)
        {
            return null;
        }

        public static DataTable GetPatient(string name)
        {
            return null;
        }

        public static DataTable GetPatientsBasedOnOpereator(Guid operatorId)
        {
            SqlParameter para = new SqlParameter("@operatorId", SqlDbType.UniqueIdentifier);
            para.Value = operatorId;
            return SqlHandler.QueryDataTable("GetPatientsBasedOnOperatorById", para);

        }

        public static DataTable GetPatientsBasedOnOpereator(string operatorName)
        {
            SqlParameter para = new SqlParameter("@operatorName", SqlDbType.NVarChar, 50);
            para.Value = operatorName;
            return SqlHandler.QueryDataTable("GetPatientsBasedOnOperatorByName", para);

        }


        //public static string globalConnectionString;
        //string connectionString;
        //SqlCommand command;
        //SqlConnection connection;

        //SearchHandler(string connectionString, string parameter)
        //{
        //    command = new SqlCommand();
        //    connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString);
        //    command.Connection = connection;
        //}

        //public DataSet Query(string parameter)
        //{
        //    //command
        //    return null;
        //}


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