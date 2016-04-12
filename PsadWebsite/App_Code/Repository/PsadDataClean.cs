using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PsadWebsite.App_Code.Repository
{
    public class PsadDataClean
    {
        // Get all files from directory

        // Get table structure from sql database - DataTable

            // Get column structure from csv file - List<string>

        // Fill new DataTables with data from files - DataRow

        // Store that data from tables in database - Insert Sql

        // Move files to storage

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["PsadData"].ConnectionString; //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\MSSQLSERVER\HECPsadDB.mdf;Integrated Security=True;Connect Timeout=30";
        public static string OrganisationTableName = "Organisations";
        public static string OperatorTableName = "Operators";
        public static string PatientTableName = "Patients";
        public static string MeasurentTableName = "Measurements";
        public static string Dilimiter = ";";
        public static string NewCsvPath = "~/Data/";

        private List<string> organisationFileList = new List<string>();
        private List<string> operatorFileList = new List<string>();
        private List<string> patientFileList = new List<string>();
        private List<string> measurementFileList = new List<string>();
        //private List<string> psadFileList = new List<string>();

        private DataTable organisationTable;
        private DataTable operatorTable;
        private DataTable patientTable;
        private DataTable measurementTable;

        //private List<string> organisationCsvColumns; // "id, org, name" 

        public PsadDataClean() // Runs all code, perhaps just make this whole class a static class
        {
            //DataTable data = GetSchema("GetOrganisationsTop0");//"GetOrganisationsTop1");
            //GetDataTableFromSchema();

            //GetProviderFactoryClasses();

            //organisationTable.TableName = OrganisationTableName;
            //GetSchema(organisationTable);

            ProcessCsvFiles();


        }


        public void ProcessCsvFiles()
        {
            // Get all files from directory
            ImportCSVFiles(NewCsvPath);

            // Get table structure from sql database - DataTable
            organisationTable = GetEmptyDatabaseTable(OrganisationTableName);
            operatorTable = GetEmptyDatabaseTable(OperatorTableName);
            patientTable = GetEmptyDatabaseTable(PatientTableName);
            measurementTable = GetEmptyDatabaseTable(MeasurentTableName);

            if (organisationTable.IsInitialized)
            {
                // Takes all the organisation csv files and transfers them to the organisation data table
                CsvFilesToDataTable(organisationFileList, organisationTable, OrganisationTableName);

                //InsertIntoDatabase(organisationTable);
                //IntsertIntoDbByAdapter(organisationTable);

                // Inserts all data from the various DataTables into corresponding database table
                InsertBulkCopy(organisationTable);
            }

            //if (measurementTable.IsInitialized)
            //    CsvFilesToDataTable(measurementFileList, measurementTable, "Somefolder", 1, 2, 1, 2, 1, 2);

        }

        /// <summary>
        /// Imports all csv file paths from a directory into coresponding fields.
        /// It's a relative directory so no need for full directory path
        /// </summary>
        /// <param name="relativeDirectory">The directory with csv files</param>
        private void ImportCSVFiles(string relativeDirectory)
        {
            string directory = HostingEnvironment.MapPath(relativeDirectory);
            // Iterate through all new CSV files, only in the data directory
            foreach (string filestr in Directory.GetFiles(directory, "*.csv", System.IO.SearchOption.TopDirectoryOnly))
            {
                if ((File.GetAttributes(filestr) & FileAttributes.Archive) == FileAttributes.Archive)
                {
                    if (filestr.Contains("Organisations_"))
                        organisationFileList.Add(filestr);

                    else if (filestr.Contains("Measurement_"))
                        measurementFileList.Add(filestr);

                    else if (filestr.Contains("Operators_"))
                        operatorFileList.Add(filestr);

                    else if (filestr.Contains("Patients_"))
                        patientFileList.Add(filestr);

                    //else if (filestr.Contains("PSADs_"))
                    //    psadFileList.Add(filestr);
                }
            }           
        }

        /// <summary>
        /// Gets an empty DataTable structered as a database table
        /// </summary>
        /// <param name="storedProcedure">The stored procedure to get table structure, does not need to return rows since it's just the column structure and datatypes that are needed</param>
        /// <returns>A DataTable with the structure of a database table</returns>
        public DataTable GetEmptyDatabaseTable(string tableName)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(GetTableProcedure(tableName), connection))
                    {
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            connection.Open();
                            DataTable dataTable = new DataTable(tableName);
                            adap.Fill(dataTable);
                            connection.Close();
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Goes through a list of csv files and adds them to a DataTable.
        /// Once the file is hadled it is move to a storage folder.
        /// </summary>
        /// <param name="filePathList">A List of file paths</param>
        /// <param name="dataTable">The DataTable that will contain the data from the csv files</param>
        /// <param name="storageFolder">The relative directory where processed files with be stored</param>
        /// <returns></returns>
        private bool CsvFilesToDataTable(List<string> filePathList, DataTable dataTable, string storageFolder)
        {
            foreach (string filestr in filePathList) // foreach file
            {
                try
                {
                    TextFieldParser csvReader = new TextFieldParser(filestr, System.Text.Encoding.Default); // Make a filedparser with ; delimiter for csv files
                    csvReader.SetDelimiters(new string[] { Dilimiter }); // This could possibly be turned indto a parameter
                    csvReader.HasFieldsEnclosedInQuotes = true;

                    // Transfer that field data from csv file to datatable parameter, Note the csv file structure is 1 row with columns and the rest of the rows are datarows
                    bool success = AddRowsToDataTable(csvReader, dataTable);

                    if (success)
                    {
                        MoveFileToStorage(filestr, storageFolder);
                    }

                    //DataTable table = CsvToDatatable("Test", csvReader, compare);

                    //Dictionary<string, string> keysAndValues = GetCsvKeysAndValues(csvReader, _measurementLines);

                    //int rows = InsertRecord(table, databaseTable);

                    //if (rows > 0) // if the insert was successefull //(rows > 0)
                    //{
                    //    // Possibly bring TextFieldParser out and pass it as a parameter instead of filestr.
                    //    //DataTable measurementData = GetMeasurementData(csvReader, _measurementLines); // Get the measurement data after the measurement has been inserted into database

                    //    //float accEverage = PsadCalculation.Accelration1Average(measurementData);

                    //    //Move file to Measurements folder, this is where all raw measurement datas will be stored
                    //    string relativePath = NewCsvPath + storageFolder + Path.GetFileName(filestr);
                    //    string destinaition = HostingEnvironment.MapPath(relativePath);

                    //    File.Move(filestr, destinaition);
                    //}


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                // Just transfer to datatable where table columns are called that same as in the sql tables

                //Old-B1
            }

            // Clear archive bit
            return false;
        }

        /// <summary>
        /// Goes through a list of csv files and adds them to a DataTable.
        /// Once the file is hadled it is move to a storage folder.
        /// 
        /// The structure of the csv files must be:
        /// Multiple lines of columns names that translate to 1 row
        /// Multiple lines of values that translate to 1 row
        /// The structure of which line in the csv file is a column or values is:
        /// Skip = 0;
        /// Column = 1;
        /// Value = 2;
        /// </summary>
        /// <param name="filePathList">A List of file paths</param>
        /// <param name="dataTable">The DataTable that will contain the data from the csv files</param>
        /// <param name="storageFolder">The relative directory where processed files with be stored</param>
        /// <param name="columnValueStructure">Pass integers for where columns and values lines are located, Skip a line with 0, add Column line with 1, add Value line with 2</param>
        /// <returns></returns>
        private bool CsvFilesToDataTable(List<string> filePathList, DataTable dataTable, string storageFolder, params int[] columnValueStructure)
        {
            foreach (string filestr in filePathList) // foreach file
            {
                try
                {
                    TextFieldParser csvReader = new TextFieldParser(filestr, System.Text.Encoding.Default); // Make a filedparser with ; delimiter for csv files
                    csvReader.SetDelimiters(new string[] { Dilimiter }); // This could possibly be turned indto a parameter
                    csvReader.HasFieldsEnclosedInQuotes = true;

                    // Transfer that field data from csv file to datatable parameter, Note the csv file structure is 1 row with columns and the rest of the rows are datarows
                    bool success = AddMultiLineColumnValueRowToDataTable(csvReader, dataTable, columnValueStructure);

                    if (success)
                    {
                        MoveFileToStorage(filestr, storageFolder);
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }
            return false;
        }
      
        /// <summary>
        /// Add rows from a csv file to a DataTable.
        /// Will only add values where the csv files and DataTables column names are both present.
        /// The structure of the csv file must be:
        /// 1 row of Column names
        /// 1 to many row of Values
        /// </summary>
        /// <param name="csvReader">A TextFieldsParser of the csv file</param>
        /// <param name="dataTable">The DataTable you wish to add the rows to</param>
        private bool AddRowsToDataTable(TextFieldParser csvReader, DataTable dataTable)
        {
            List<string> columns = new List<string>();
            List<int> removedColumns = new List<int>();

            try
            {
                using (csvReader)
                {
                    while (!csvReader.EndOfData) // Iterate through csv file
                    {
                        List<string> fields;
                        // Advaces line

                        if (csvReader.LineNumber == 1) // First row
                        {
                            fields = new List<string>(csvReader.ReadFields());

                            columns = GetFieldsAsColumns(fields, dataTable, removedColumns);
                        }
                        else // All other rows
                        {
                            fields = new List<string>(csvReader.ReadFields());

                            GetFieldsAsValuesToRow(fields, dataTable, columns, removedColumns);
                        }
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Adds rows from a csv file to a DataTable.
        /// Will only add values where the csv files and DataTables column names are both present.
        /// The structure of the csv file must be:
        /// Multiple lines of columns names that translate to 1 row
        /// Multiple lines of values that translate to 1 row
        /// The structure of which line in the csv file is a column or values is:
        /// Skip = 0;
        /// Column = 1;
        /// Value = 2;
        /// </summary>
        /// <param name="csvReader">A TextFieldsParser of the csv file</param>
        /// <param name="dataTable">The DataTable you wish to add the rows to</param>
        /// <param name="columnValueStructure">Pass integers for where columns and values lines are located, Skip a line with 0, add Column line with 1, add Value line with 2</param>
        private bool AddMultiLineColumnValueRowToDataTable(TextFieldParser csvReader, DataTable datatable, params int[] columnValueStructure)
        {
            List<string> columns = new List<string>();
            List<int> removedColumns = new List<int>();
            List<string> values = new List<string>();

            int totalRows = columnValueStructure.Length;

            // Get 3 lines of columns

            // Define all columns

            // Get 3 lines of values

            // then read values

            try
            {
                using (csvReader)
                {

                    for (int i = 0; i < totalRows; i++)
                    {
                        List<string> fields = new List<string>(csvReader.ReadFields());

                        if (columnValueStructure[i] == 1)
                        {
                            columns.AddRange(fields);
                        }
                        else if (columnValueStructure[i] == 2)
                        {
                            values.AddRange(fields);
                        }
                    }

                    columns = GetFieldsAsColumns(columns, datatable, removedColumns);
                    GetFieldsAsValuesToRow(values, datatable, columns, removedColumns);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Gets a list of fields containing column names from a csv file line.
        /// The list is compared against a DataTable structure removing any columns not in the DataTable.
        /// The indexs of removed columns are stored in a list of integers obejct that has been passed as a parameter.
        /// </summary>
        /// <param name="csvReader"></param>
        /// <param name="dataTable"></param>
        /// <param name="removedColumns"></param>
        /// <returns>List of valid column names</returns>
        private List<string> GetFieldsAsColumns(List<string> fields, DataTable dataTable, List<int> removedColumns)
        {
            List<string> columns = new List<string>(fields);
            List<string> columnCopy = new List<string>(columns);
            int length = columns.Count;

            for (int i = 0; i < length; i++) // Field in row
            {
                string value = columnCopy[i];

                if (!dataTable.Columns.Contains(value)) // If column does not exist in DataTable
                {
                    columns.Remove(value); // Remove it
                    removedColumns.Add(i); // Add to exlcusions list
                }
            }

            return columns;
        }

        /// <summary>
        /// Takes a list of fields interperts them as values and adds them to a Datatable as a DataRow
        /// </summary>
        /// <param name="fields">The list of fields that are interperted as values</param>
        /// <param name="dataTable">The DataTble to add the DataRow</param>
        /// <param name="columns">The column structure of the values</param>
        /// <param name="removedColumns">The removed columns which are not a part of the DataTable</param>
        private void GetFieldsAsValuesToRow(List<string> fields, DataTable dataTable, List<string> columns, List<int> removedColumns)
        {
            DataRow row = dataTable.NewRow();
            int length = removedColumns.Count;
            List<string> values = new List<string>(fields);

            for (int i = 0; i < length; i++) // Removes exluded columns
            {
                values.RemoveAt(removedColumns[i] - i);
            }

            int newLength = values.Count;
            for (int i = 0; i < newLength; i++)
            {
                string column = columns[i]; // Gets column name
                string value = values[i];
                if (value != string.Empty && value != null)
                    row[column] = value;
            }

            dataTable.Rows.Add(row);
        }

        #region DataBaseHandling

        /// <summary>
        /// Inserts data from DataTable
        /// </summary>
        /// <param name="dataTable">The DataTable with new data to be inserted into an sql database table</param>
        private void InsertBulkCopy(DataTable dataTable)
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

        private void IntsertIntoDbByAdapter(DataTable dataTable)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlDataAdapter adap = new SqlDataAdapter())
                    {
                        connection.Open();
                        adap.Update(dataTable);
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private int InsertIntoDatabase(DataTable dataTable)
        {
            string storedProcedure = "InsertDataTableInto" + dataTable.TableName;
            int rows = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // I'm trying to pass the DataTable with DataRows full of data into it's Corresponding Table. But it seems that the DataTypes have to be the same (I think the DataTables are set to string)
                        //Either i do this by looping through it in SqlDataTableTemplate() or i do something smarter
                        // I'm trying to figure out how to copy the structure from the various Tables into the DataTable as cleanly and 1to1 as possible
                        // Something that has promise is DbDataAdapter.FillSchema(); but it's quite mischivious
                        SqlParameter table = new SqlParameter("@dataTable", dataTable);
                        cmd.Parameters.Add(table);

                        conn.Open();
                        rows = cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return rows;
        }


        /// <summary>
        /// Creates a DataTable based on an Sql Database Tables structure
        /// </summary>
        /// <param name="tableName">The name of the database table</param>
        /// <returns></returns>
        private DataTable SqlDataTableTemplate(string tableName)
        {
            DataTable table = new DataTable(tableName);

            //string sqlStatement = string.Format(@"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", tableName);

            //columnNames = new List<string>();
            //dataTypes = new List<SqlDbType>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetTableSchemaInfo", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(tableName, SqlDbType.NVarChar);

                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                table.Columns.Add(reader["COLUMN_NAME"].ToString());


                                //dataTypes.Add(reader["DATA_TYPE"]);


                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return table;
        }

        private DataTable GetSchemaSomehow(DataTable dataTable)
        {

            string sqlStatement = "GetTableSchemaInfo";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, connection))
                    {
                        SqlDataAdapter adap = new SqlDataAdapter(cmd);

                         return adap.FillSchema(dataTable, SchemaType.Mapped);

                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            //// You can specify the Catalog, Schema, Table Name, Table Type to get 
            //// the specified table(s).
            //// You can use four restrictions for Table, so you should create a 4 members array.
            //string[] restrictions = new string[4];
            //// For the array, 0 - member represents Catalog; 1 - member represents Schema;
            //// 2-member represents Table Name; 3-member represents Table Type. 
            //// Now we specify the Table Name of the table what we want to get schema information.
            //restrictions[2] = table;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString))
            //    {
            //        connection.Open();
            //        DataTable dataTable = connection.GetSchema("Tables", restrictions);
            //        connection.Close();
            //        return dataTable;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}


        }


        private DataTable GetDataTableFromSchema()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //using (SqlCommand cmd = new SqlCommand(sqlStatement, connection))
                    //{
                    //}
                    connection.Open();
                    DataTable table = connection.GetSchema("Tables");
                    connection.Close();

                    return table;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static DataTable GetProviderFactoryClasses()
        {
            // Retrieve the installed providers and factories.
            DataTable table = DbProviderFactories.GetFactoryClasses();

            // Display each row and column value.
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    Console.WriteLine(row[column]);
                }
            }
            return table;
        }
        #endregion DataBaseHandling

        /// <summary>
        /// String format for the GetEmptyDatabaseTable() stored procedure
        /// </summary>
        /// <param name="tableName">The tablename for the storedprocedure</param>
        /// <returns>A string of the format "Get{0}Top0" where {0} = tableName </returns>
        private string GetTableProcedure(string tableName)
        {
            return string.Format("Get{0}Top0",tableName);
        }

        //private string GetStoragePath(string relativeFolder)
        //{
        //    return NewCsvPath + relativeFolder;
        //}


        /// <summary>
        /// Moves a proccessed file to a storage folder
        /// </summary>
        /// <param name="file">The full file path</param>
        /// <param name="storageFolder">The relative folder name where the file will be stored, must be a child of this directory</param>
        private void MoveFileToStorage(string file, string storageFolder)
        {
            string relativePath = NewCsvPath + storageFolder + "\\" + Path.GetFileName(file);
            string destination = HostingEnvironment.MapPath(relativePath);

            File.Move(file, destination);
        }
    }
}