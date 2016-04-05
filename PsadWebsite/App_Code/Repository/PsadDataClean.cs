using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            ImportCSVFiles(NewCsvPath);

            organisationTable = SqlDataTableTemplate(OrganisationTableName);
            operatorTable = SqlDataTableTemplate(OperatorTableName);
            patientTable = SqlDataTableTemplate(PatientTableName);
            measurementTable = SqlDataTableTemplate(MeasurentTableName);

            if (organisationTable.IsInitialized)
                CsvFilesToDataTable(organisationFileList, organisationTable, "SomeFolder");

            if (measurementTable.IsInitialized)
                CsvFilesToDataTable(measurementFileList, measurementTable, "Somefolder", 1, 2, 1, 2, 1, 2);

            

        }

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

        // If archive bit set add file to appropriate list (Measurements, Patients, Operators, Psads, Organisation List

        //ImportSingleRowFiles(OrganisationFileList, "Organisation", "Organisation/");

        //ImportOrganisations(OrganisationFileList);
        //ImportPsads(PsadFileList);
        //ImportOperators(OperatorFileList);
        //ImportPatients(PatientFileList);
        //ImportMeasurements(MeasurementFileList);


        private DataTable SqlDataTableTemplate(string tableName)
        {
            DataTable table = new DataTable();

            string sqlStatement = string.Format(@"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", tableName);

            //columnNames = new List<string>();
            //dataTypes = new List<SqlDbType>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, connection))
                    {
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

        private void AddRowsToDataTable(TextFieldParser csvReader, DataTable dataTable)
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
            }
            catch (Exception ex)
            {
            }

        }

                                                                                            // string "column" "value" / 1, 2
        private void AddMultiLineColumnValueRowToDataTable(TextFieldParser csvReader, DataTable datatable, params int[] columnValueStructure) // [1] {2} | [3] {4,5}
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
            }
            catch (Exception ex)
            {

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
        /// 
        /// </summary>
        /// <param name="csvReader"></param>
        /// <param name="dataTable"></param>
        /// <param name="columns"></param>
        /// <param name="removedColumns"></param>
        private void GetFieldsAsValuesToRow(List<string> fields, DataTable dataTable, List<string> columns, List<int> removedColumns)
        {
            DataRow row = dataTable.NewRow();
            int length = removedColumns.Count;
            List<string> values = new List<string>(fields);

            for (int i = 0; i < length; i++) // Removes exluded columns
            {
                values.RemoveAt(removedColumns[i]);
            }

            int newLength = values.Count;
            for (int i = 0; i < newLength; i++)
            {
                string column = columns[i]; // Gets column name
                row[column] = values[i];
            }

            dataTable.Rows.Add(row);
        }

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
                    AddRowsToDataTable(csvReader, dataTable);

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
                    AddMultiLineColumnValueRowToDataTable(csvReader, dataTable, columnValueStructure);


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
  
            }
            return false;
        }
    }
}