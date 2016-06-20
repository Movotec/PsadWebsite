using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PsadWebsite.App_Code.Repository
{
    public static class SqlHandler
    {
        //private static string ConnectionString; // = ConfigurationManager.ConnectionStrings["PsadData"].ConnectionString;
        private static string configConnectionString;// = "PsadData";


        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConfigConnectionString].ConnectionString;
            }
        }

        public static string ConfigConnectionString
        {
            get
            {
                return configConnectionString;
            }

            set
            {
                configConnectionString = value;
            }
        }

        public static void InsertBulkCopy(DataTable dataTable)
        {
            try
            {
                using (SqlBulkCopy bulk = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.KeepNulls))
                {
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        string column = col.ColumnName;
                        bulk.ColumnMappings.Add(column, column);
                    }

                    bulk.BulkCopyTimeout = 600;
                    bulk.DestinationTableName = dataTable.TableName;
                    // To insert there must only be new data, if it finds any rows that contain an already excisiting RecId it will throw an exception
                    bulk.WriteToServer(dataTable);
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void ExecuteNonQuery(string commandtext, bool storedProcedure = false)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandtext, conn))
                {
                    if (storedProcedure)
                        cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            
        }

        public static DataTable QueryDataTable(string commmandtext)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commmandtext, conn))
                {
                    using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();

                        try
                        {
                            //cmd.Parameters.AddRange(parameters);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            adap.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {


                        }
                        finally
                        {
                            conn.Close();
                        }

                        return dataTable;
                    }

                }
            }
        }

        public static DataTable QueryDataTable(string commmandtext, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commmandtext, conn))
                {
                    using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();

                        try
                        {
                            cmd.Parameters.AddRange(parameters);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            adap.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {


                        }
                        finally
                        {
                            conn.Close();
                        }

                        return dataTable;
                    }

                }
            }
        }
    }
}