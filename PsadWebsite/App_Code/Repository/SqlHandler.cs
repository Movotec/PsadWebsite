﻿using System;
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
        private static string connectionString = ConfigurationManager.ConnectionStrings["PsadData"].ConnectionString;


        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }

            set
            {
                connectionString = value;
            }
        }

        public static void InsertBulkCopy(DataTable dataTable)
        {
            try
            {
                using (SqlBulkCopy bulk = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.KeepNulls))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
    }
}