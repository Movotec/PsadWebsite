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

    // plain query should search through all names, so organisations names, patients, 

    public class SearchHandler
    {
        #region Stored Procedures
        const string paByName = "GetPatientsByName";
        const string opByName = "GetOperatorsByName";
        const string orgByName = "GetOrganisationsByName";
        const string paByNameGender = "GetPatientsByNameWhereGender";
        #endregion

        private static string query = "query"; // for basic queries
        private static string group = "group"; // for specific groups
        private static string gender = "gender";
        private static char delimiter = ';'; // the delimiter for the keysAndValues parameter for construction a querystring

        public static string Query { get { return query; } }

        public static string Group
        {
            get
            {
                return group;
            }
        }

        public static char Delimiter
        {
            get
            {
                return delimiter;
            }
        }

        public static string Gender
        {
            get
            {
                return gender;
            }
        }

        private static string QueryFormatBase(string key, string value)
        {
            return string.Format("?{0}={1}", key, value); 
        }

        private static string QueryFormatCont(string key, string value)
        {
            return string.Format("&{0}={1}", key, value);
        }

        public static string QueryString(string url, string query)
        {
            return url + QueryFormatBase(Query, query);
        }

        /// <summary>
        /// Generate a url query string from the, the query and various keys and values sepereated by a ';' delimiter
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="keyAndValues">Keys and values of other query string parameters seperated by a ';' delimiter</param>
        /// <returns></returns>
        public static string QueryString(string url, string query, params string[] keyAndValues)
        {
            string queryString = url + QueryFormatBase(Query, query);
            int length = keyAndValues.Length;
            for (int i = 0; i < length; i++)
            {
                string[] keyValueSplit = keyAndValues[i].Split(delimiter);

                queryString += QueryFormatCont(keyValueSplit[0], keyValueSplit[1]);
            }

            return queryString;
        }

        public static DataTable FindPeople(string query, EData group)
        {
            string storedProcedure = null;
            switch (group)
            {
                case EData.Patients: storedProcedure = paByName; break;
                case EData.Operators: storedProcedure = opByName; break;
                case EData.Organisations: storedProcedure = orgByName; break;
            }

            SqlParameter para = new SqlParameter("@name", SqlDbType.NVarChar, 50);
            para.Value = query;
            return SqlHandler.QueryDataTable(storedProcedure, para);
        }

        public static DataTable FindPeople(string query, EData group, EGender gender)
        {
            string storedProcedure = null;
            switch (group)
            {
                case EData.Patients: storedProcedure = paByNameGender; break;
                case EData.Operators: storedProcedure = opByName; break;
            }

            SqlParameter para = new SqlParameter("@name", SqlDbType.NVarChar, 50);
            para.Value = query;

            SqlParameter para1 = new SqlParameter("@gender", SqlDbType.NVarChar, 50);
            switch (gender)
            {
                case EGender.Male: para1.Value = "male"; break;
                case EGender.Female: para1.Value = "female"; break;
            }

            return SqlHandler.QueryDataTable(storedProcedure, para, para1);
        }

        //public static DataTable GetPatient(Guid patientId)
        //{
        //    return null;
        //}

        //public static DataTable GetPatient(string name)
        //{
        //    return null;
        //}

        //public static DataTable GetPatientsBasedOnOpereator(Guid operatorId)
        //{
        //    SqlParameter para = new SqlParameter("@operatorId", SqlDbType.UniqueIdentifier);
        //    para.Value = operatorId;
        //    return SqlHandler.QueryDataTable("GetPatientsBasedOnOperatorById", para);

        //}

        //public static DataTable GetPatientsBasedOnOpereator(string operatorName)
        //{
        //    SqlParameter para = new SqlParameter("@operatorName", SqlDbType.NVarChar, 50);
        //    para.Value = operatorName;
        //    return SqlHandler.QueryDataTable("GetPatientsBasedOnOperatorByName", para);

        //}


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