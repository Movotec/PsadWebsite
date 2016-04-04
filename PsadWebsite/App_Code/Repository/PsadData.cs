using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Diagnostics;


// To do:
//      Maken even more modular by getting data types from sql tables
//
//      Transfer Organisation
//          Insert new if not created
//          Verify if already creadted?
//
//      Transfer Operator ++
//      Transer Patient ++
//      Transfer PSADs???
//
// Done:
//      Transfer measurement data or calculate generic/average data and transfer that
//          Will move the original file instead for now and use that as raw data for generating new generric average data
//          Perhaps later the raw data will be stored in sql tables
//

// Ideas for optimizing:
//      Use sql parameters to prevent sql injections
//          This would require reading data types. maybe not neccassary with datatable class?
//
//      Checking if there already is an entry with measurement guid, and then not running the insert statement that fails
//
//      Make better verification information for the comparison of the sql columns and csv columns
//
//      



namespace PsadWebsite.App_Code.Repository
{
    public class PsadData
    {

        public PsadData()
        {
            _measurementColumns = GetSqlColumnNames(_measurementsTable);
        }

        public int ImportCSVFiles()
        {
            string _measurePathHECPsad = HostingEnvironment.MapPath(_newCsvPath);
            List<string> OrganisationFileList = new List<string>();
            List<string> PsadFileList = new List<string>();
            List<string> OperatorFileList = new List<string>();
            List<string> PatientFileList = new List<string>();
            List<string> MeasurementFileList = new List<string>();

            // Iterate through all new CSV files, only in the data directory
            foreach (string filestr in Directory.GetFiles(_measurePathHECPsad, "*.csv", System.IO.SearchOption.TopDirectoryOnly))
            {
                if ((File.GetAttributes(filestr) & FileAttributes.Archive) == FileAttributes.Archive)
                {
                    if (filestr.Contains("Organisations_"))
                        OrganisationFileList.Add(filestr);

                    else if (filestr.Contains("Measurement_"))
                        MeasurementFileList.Add(filestr);

                    else if (filestr.Contains("Operators_"))
                        OperatorFileList.Add(filestr);

                    else if (filestr.Contains("Patients_"))
                        PatientFileList.Add(filestr);

                    else if (filestr.Contains("PSADs_"))
                        PsadFileList.Add(filestr);
                }
            }

            // If archive bit set add file to appropriate list (Measurements, Patients, Operators, Psads, Organisation List


            ImportSingleRowFiles(OrganisationFileList, "Organisation", "Organisation/");

            //ImportOrganisations(OrganisationFileList);
            //ImportPsads(PsadFileList);
            //ImportOperators(OperatorFileList);
            //ImportPatients(PatientFileList);
            //ImportMeasurements(MeasurementFileList);

            return 0;
        }
        /// <summary>
        /// Imports single row files, as in a csv file with column identifiers and 1 values for each column
        /// </summary>
        /// <param name="filelist">A list of the files filepaths</param>
        /// <param name="numberOfLines">The number of lines that the key value pairs are stored on in csv file</param>
        /// <param name="databaseTable">The name of the mssql database table to store the data from the files</param>
        /// <param name="storageFolder">The folder placement of where the processed files will be stored</param>
        /// <returns></returns>
        private bool ImportSingleRowFiles(List<string> filePathList, string databaseTable, string storageFolder)
        {
            foreach (string filestr in filePathList)
            {
                try
                {
                    TextFieldParser csvReader = new TextFieldParser(filestr, System.Text.Encoding.Default);
                    csvReader.SetDelimiters(new string[] { _delimeter });
                    csvReader.HasFieldsEnclosedInQuotes = true;

                    List<string> compare = GetSqlColumnNames(databaseTable);

                    DataTable table = CsvToDatatable("Test", csvReader, compare);

                    //Dictionary<string, string> keysAndValues = GetCsvKeysAndValues(csvReader, _measurementLines);

                    int rows = InsertRecord(table, databaseTable);

                    if (rows > 0) // if the insert was successefull //(rows > 0)
                    {
                        // Possibly bring TextFieldParser out and pass it as a parameter instead of filestr.
                        //DataTable measurementData = GetMeasurementData(csvReader, _measurementLines); // Get the measurement data after the measurement has been inserted into database

                        //float accEverage = PsadCalculation.Accelration1Average(measurementData);

                        //Move file to Measurements folder, this is where all raw measurement datas will be stored
                        string relativePath = _newCsvPath + storageFolder + Path.GetFileName(filestr);
                        string destinaition = HostingEnvironment.MapPath(relativePath);

                        File.Move(filestr, destinaition);
                    }


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

        #region redundant soon
        private bool ImportOrganisations(List<string> filelist)
        {
            foreach (string filestr in filelist)
            {
                TextFieldParser csvReader = new TextFieldParser(filestr, System.Text.Encoding.Default);
                csvReader.SetDelimiters(new string[] { _delimeter });
                csvReader.HasFieldsEnclosedInQuotes = true;

                Dictionary<string, string> organisations = GetCsvKeysAndValues(csvReader, 2); // 2 lines in organisation files
            }

            return false;
        }

        private bool ImportPsads(List<string> filelist)
        {
            return false;
        }

        private bool ImportOperators(List<string> filelist)
        {
            return false;
        }

        private bool ImportPatients(List<string> filelist)
        {
            return false;
        }

        private bool ImportMeasurements(List<string> filelist)
        {
            foreach (string filestr in filelist)
            {
                try
                {
                    TextFieldParser csvReader = new TextFieldParser(filestr, System.Text.Encoding.Default);
                    csvReader.SetDelimiters(new string[] { _delimeter });
                    csvReader.HasFieldsEnclosedInQuotes = true;

                    Dictionary<string, string> measurement = GetMeasurement(csvReader, _measurementLines);

                    int rows = InsertRecord(measurement, _measurementsTable);

                    if (rows > 0) //(rows > 0)
                    {
                        // Possibly bring TextFieldParser out and pass it as a parameter instead of filestr.
                        //DataTable measurementData = GetMeasurementData(csvReader, _measurementLines); // Get the measurement data after the measurement has been inserted into database

                        //float accEverage = PsadCalculation.Accelration1Average(measurementData);

                        //Move file to Measurements folder, this is where all raw measurement datas will be stored
                        string relativePath = _newCsvPath + _measurementsFolder + Path.GetFileName(filestr);
                        string destinaition = HostingEnvironment.MapPath(relativePath);

                        File.Move(filestr, destinaition);
                    }


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
        #endregion redundant soon

        //Measurements read as 1 col 1 row 1 col 1 row 1 col 1 row
        //MeausrementsData reads 1 col X row
        // Other stuff reads as 1 col X row (typically 1 row)

        // Textfieldpaser is moved through, go through meausements, then you are at measurement data and go throught that

        /// <summary>
        /// Create a DataTable based of a single row of columns and 1 to many rows of data from a Csv file
        /// Is compared against a list of columns from an sql database
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="csvReader"></param>
        /// <param name="columnCompare"></param>
        /// <returns>A DataTable containing data from a csv file</returns>
        public DataTable CsvToDatatable(string tableName, TextFieldParser csvReader, List<string> columnCompare)
        {
            DataTable table = new DataTable(tableName);
            int shortenedWhitespace = 0;

            try
            {
                using (csvReader)
                {
                    while (!csvReader.EndOfData)
                    {

                        if (csvReader.LineNumber <= 1) //first row
                        {
                            string[] fields = csvReader.ReadFields();
                            fields = shortenDelimetedLine(fields);
                            int length = fields.Length;
                            shortenedWhitespace = length;

                            for (int i = 0; i < length; i++)
                            {
                                if (columnCompare.Contains(fields[i])) // check against column compare
                                    table.Columns.Add(fields[i]); //  and add to table columns
                            }
                        }
                        else
                        {
                            string[] fields = csvReader.ReadFields();
                            fields = shortenToColumns(fields, shortenedWhitespace);
                            int length = fields.Length;

                            table.Rows.Add(fields);
                        }
                    }

                    return table;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
/////////
//        private DataTable GetCsvData(string tableName, List<string> columnCompare, TextFieldParser csvReader, int[] columns, int[] fields)
//        {
//            DataTable table = new DataTable(tableName);
//            List<string[]> allRows = new List<string[]>();
//            int columnCount;


//            try
//            {
//                using (csvReader)
//                {
//                    while (!csvReader.EndOfData)
//                    {
//                        string[] fld = csvReader.ReadFields();
//                        allRows.Add(fld); // add alls filds to row of fields
//                    }

//                    int colLength = columns.Length;

//                    for (int i = 0; i < colLength; i++) // For each row of columns
//                    {
//                        int rowIndex = columns[i]; // get row index

//                        string[] row = allRows[rowIndex]; // get the row
//                        row = shortenDelimetedLine(row); // remove excess whitespace ?? Possible alternative in TextFieldParser
//                        columnCount = row.Length; // remember new row length for later

//                        int rowLength = row.Length; // get the row length

//                        for (int j = 0; j < rowLength; j++) // for column in row
//                        {
//                            if (columnCompare.Contains(row[j])) // check against column compare
//                                table.Columns.Add(row[j]); //  and add to table columns

//                        }
//                    }

//                    int fieldLength = fields.Length;

//                    for (int i = 0; i < fieldLength; i++)
//                    {
//                        int rowIndex = fields[i];

//                        string[] row = allRows

//                    }                   
//                }
//            }
//            catch (Exception ex)
//            {


//            }
//        }
////////

        // Creates a Dictionary of csv measurment file with colum names as keys and values as values
        private Dictionary<string,string> GetMeasurement(TextFieldParser csvReader, int lines)
        {
            Dictionary<string, string> measurementTable = new Dictionary<string, string>();
            List<string> excess = new List<string>();

            try
            {
                using (csvReader)
                {
                    for (int i = 0; i < lines; i = i + 2)
                    {
                        string[] columns = csvReader.ReadFields();

                        string[] fields = csvReader.ReadFields();

                        int length = 0;

                        columns = shortenDelimetedLine(columns);
                        fields = shortenToColumns(fields, columns.Length);

                        if (columns.Length == fields.Length)
                            length = columns.Length;

                        for (int j = 0; j < length; j++)
                        {
                            if (columns[j] == string.Empty || columns[j] == null)
                                break; // Breaks loop if it hits empty column

                            if (_measurementColumns.Contains(columns[j])) // Only adds if column exists in sql database
                                measurementTable.Add(columns[j], fields[j]);
                            else
                                excess.Add(columns[j]);
                        }
                    }


                    // Old-A1
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Converting csv file to datatable: " + ex.ToString());
            }
            return measurementTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvReader"></param>
        /// <param name="numberOfLines"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetCsvKeysAndValues(TextFieldParser csvReader, int numberOfLines)
        {
            Dictionary<string, string> keysAndValues = new Dictionary<string, string>();
            List<string> excess = new List<string>();

            try
            {
                using (csvReader)
                {
                    for (int i = 0; i < numberOfLines; i = i + 2)
                    {
                        string[] columns = csvReader.ReadFields();

                        string[] fields = csvReader.ReadFields();

                        int length = 0;

                        columns = shortenDelimetedLine(columns);
                        fields = shortenToColumns(fields, columns.Length);

                        if (columns.Length == fields.Length)
                            length = columns.Length;

                        for (int j = 0; j < length; j++)
                        {
                            if (columns[j] == string.Empty || columns[j] == null)
                                break; // Breaks loop if it hits empty column

                            if (_measurementColumns.Contains(columns[j])) // Only adds if column exists in sql database
                                keysAndValues.Add(columns[j], fields[j]);
                            else
                                excess.Add(columns[j]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Converting csv file to datatable: " + ex.ToString());
            }
            return keysAndValues;
        }

        private DataTable GetMeasurementData(TextFieldParser csvReader , int startingLineIndex)
        {
            DataTable dt = new DataTable();

            int count = 0;

            int columns = 0;

            while (!csvReader.EndOfData)
            {
                string[] fields = csvReader.ReadFields();

                if (count == startingLineIndex) // Adds the columns, no sql data validation/comparison
                {
                    fields = shortenDelimetedLine(fields);
                    columns = fields.Length;

                    for (int i = 0; i < fields.Length; i++)
                    {
                        dt.Columns.Add(fields[i]);
                    }
                }
                else if (count > startingLineIndex) // Adds rows of data
                {
                    fields = shortenToColumns(fields, columns);
                    dt.Rows.Add(fields);
                }

                count++;
            }

            return dt;
        }

        private DataTable BasedOnSqlTable(List<string> columnNames)
        {
            DataTable dt = new DataTable();

            return dt;
        }

        /// <summary>
        /// Get sql column names from a table
        /// </summary>
        /// <param name="table">The name of the sql table</param>
        /// <returns>A list of the column names as strings</returns>
        private List<string> GetSqlColumnNames(string table)
        {
            string sqlStatement = string.Format(@"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", table);

            List<string> columnNames = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                columnNames.Add(reader["COLUMN_NAME"].ToString());  // No data is present, perhaps sql statement is lacking "scheme" for table. Test other sql statement
                            }
                            //if (!reader.IsDBNull(i))

                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return columnNames;
        }

        //public cause testing
        public void GetSqlColumnInfo(string table, out List<string> columnNames, out List<SqlDbType> dataTypes)
        {
            string sqlStatement = string.Format(@"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", table);

            columnNames = new List<string>();
            dataTypes = new List<SqlDbType>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                columnNames.Add(reader["COLUMN_NAME"].ToString());
                                //dataTypes.Add(reader["DATA_TYPE"]);

                                //Type what = reader.ge;

                            }
                            //if (!reader.IsDBNull(i))

                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            //return columnNames;
        }

        private int InsertRecord(DataTable dataTable, string sqlTable)
        {
            string sqlColumns = "";
            string sqlValues = "";
            string sqlStatement = "";

            int count = 0;
            int columnAmount = dataTable.Columns.Count;
            int rowAmount = dataTable.Rows.Count;

            foreach (DataColumn column in dataTable.Columns)
            {
                sqlColumns += "[" + column.ColumnName + "]";
                count++;

                if (count < columnAmount)
                {
                    sqlColumns += ",";
                }
            }

            count = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    count++;
                    string val = row[column].ToString();
                    if (val == "" || val == string.Empty)
                    {
                        sqlValues += "'NULL'";
                    }
                    else
                    {
                        float a = 0;
                        int b = 0;

                        if (float.TryParse(val, out a) || int.TryParse(val, out b))
                        {
                            sqlValues += val.Replace(',', '.');
                        }
                        //else if ()
                        //{
                        //    sqlValues += b;
                        //}
                        else
                        {
                            sqlValues += "'" + val + "'";
                        }
                    }

                    if (count < columnAmount)
                    {
                        sqlValues += ",";
                    }
                }
            }

            // This should be redone with parameters to combat sql injections
            sqlStatement = string.Format("INSERT INTO {0} ({1}) VALUES({2})", sqlTable, sqlColumns, sqlValues); 

            return ExecuteNonQuery(sqlStatement);
        }

        private int InsertRecord(Dictionary<string,string> dict, string sqlTable)
        {
            string sqlColumns = "";
            string sqlValues = "";
            string sqlStatement = "";

            int count = 0;
            int columnAmount = dict.Keys.Count;
            // int rowAmount = dataTable;

            foreach (KeyValuePair<string,string> column in dict)
            {
                sqlColumns += "[" + column.Key + "]";
                count++;

                if (count < columnAmount)
                {
                    sqlColumns += ",";
                }

                string val = column.Value;

                if (val == "" || val == string.Empty)
                {
                    sqlValues += "NULL";
                }
                else
                {
                    float a = 0;
                    int b = 0;

                    if (float.TryParse(val, out a) || int.TryParse(val, out b))
                    {
                        sqlValues += val.Replace(',', '.');
                    }
                    //else if ()
                    //{
                    //    sqlValues += b;
                    //}
                    else
                    {
                        sqlValues += "'" + val + "'";
                    }
                }

                if (count < columnAmount)
                {
                    sqlValues += ",";
                }
            }
            // This should be redone with parameters to combat sql injections
            sqlStatement = string.Format("INSERT INTO {0} ({1}) VALUES({2})", sqlTable, sqlColumns, sqlValues);

            return ExecuteNonQuery(sqlStatement);
        }

        private static int ExecuteNonQuery(string sqlStatement)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, connection))
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        connection.Close();
                        return rows;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                // possible add a check if the row with measurmentGuid already exists don't try to insert
                return 0;
            }
        }

        private string[] ConvertToList(params string[] args)
        {
            return args.ToArray();
        }

        private string[] shortenDelimetedLine(string[] line)
        {
            int length = line.Length;
            List<string> list = new List<string>();

            for (int i = 0; i < length; i++)
            {
                if (line[i].Equals(string.Empty) == false)
                    list.Add(line[i]);
            }

            return list.ToArray();
        }

        private string[] shortenToColumns(string[] line, int columns)
        {
            string[] row = new string[columns];

            for (int i = 0; i < columns; i++)
            {
                row[i] = line[i];
            }

            return row;
        }

        private CSVClass csvIO = new CSVClass();
        private static string _connectionString = ConfigurationManager.ConnectionStrings["PsadData"].ConnectionString; //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\MSSQLSERVER\HECPsadDB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string _newCsvPath = "~/Data/";
        private static string _delimeter = ";";
        private static string _measurementsTable = "Measurements";
        private static string _measurementsFolder = "Measurements/";
        private static int _measurementLines = 6;
        private List<string> _measurementColumns;
    }

    #region OldCode

    //private DataTable GetMeasurement(string file)
    //{
    //    //DataRepository datarepository = new DataRepository();
    //    //PsadMeasureInfo psadBasics = new PsadMeasureInfo();

    //    DataTable datatable = new DataTable();

    //    datatable = csvIO.GetDataTableFromCSVFile(file, 0, 2);
    //    //datarepository.CopyDataRowToObject(datatable.Rows[0], psadBasics, typeof(PsadMeasureInfo));

    //    //PsadMeasureResult mr = new PsadMeasureResult();
    //    datatable = csvIO.GetDataTableFromCSVFile(file, 2, 2);
    //    //if (datatable.Rows.Count > 0)
    //    //    datarepository.CopyDataRowToObject(datatable.Rows[0], mr, typeof(PsadMeasureResult));

    //    //MeasureInfoAndResult measureInfoAndResult = new MeasureInfoAndResult();
    //    //measureInfoAndResult.info = psadBasics;
    //    //measureInfoAndResult.result = mr;

    //    //return measureInfoAndResult;

    //    return datatable;
    //}

 // Old-A1

    //foreach (string column in sqlColumns) // removes all the columns that do not exist in the database table
    //{
    //    if (measurementTable.ContainsKey(column) == false)
    //    {
    //        measurementTable.Remove(column);
    //    }
    //}


    //for (int i = 1; i <= lines; i++)
    //{
    //    string[] colFields = csvReader.ReadFields();


    //    //!!                        // This should actually check if there is a column with this name in the sql table
    //    //!!                        // Perhaps get name of all sql table column names and compare against to see if it exsits, if it does'nt disregard data

    //    // breaks cause starts adding more columns when a row is already added and therfore breaks it
    //    if ((i % 2) == 0) // if even number it is row values
    //    {
    //        for (int j = 0; j < colFields.Length; j++)
    //        {
    //            if (colFields[i] == string.Empty)
    //            {
    //                colFields[i] = null;
    //            }
    //            fields.Add(colFields[j]);
    //        }

    //        //csvData.Rows.Add(colFields);
    //    }
    //    else // if odd it is column names
    //    {

    //        foreach (string column in colFields)
    //        {
    //            DataColumn dataColumn = new DataColumn(column);
    //            dataColumn.AllowDBNull = true;
    //            csvData.Columns.Add(dataColumn); // add it

    //            //if (csvData.Columns.Contains(dataColumn.ColumnName) == false) // if the datatable does not already contain the column name
    //            //{
    //            //}

    //        }
    //    }
    //}

    //csvData.Rows.Add(fields);


    //foreach (string column in sqlColumns) // removes all the columns that do not exist in the database table
    //{
    //    if (csvData.Columns.Contains(column) == false)
    //    {
    //        csvData.Columns.Remove(column);
    //    }
    //}




    //while (count++ < start) //Skip line
    //    csvReader.ReadFields();



    //while (!csvReader.EndOfData && count++ < MaxCount)
    //{
    //    string[] fieldData = csvReader.ReadFields();


    //}

// Old-A1       End

// Old-B1
    // Take a datatable and insert it into the corresponing sql table
    //InsertRecord(measurement);

    //// Right now it transfers to data table and then to a MeasureInfoAndResult object
    //DateTime dt = new DateTime(mir.result.MeasureDateTime.Ticks);
    //if (mir.result.MeasureDateTime.Ticks < 1)
    //    mir.result.MeasureDateTime = DateTime.Now;

    //// This can be reduced to iterate through keys and values (columns and row values) from a datatable
    //// Do calculations (TUE, Stenberg Johan ...)
    //fieldArray = ConvertToList("[OrganisationGuid]", "[PatientGuid]", "[OperatorGuid]", "[PsadGuid]", "[MeasureGuid]", "[ExternFileName]", "[MeasureMode]", "[Limb]", "[Orientation]", "[MeasureDateTime]", "[Comments]", "[StartAngle]", "[MinAngle]", "[MaxAngle]", "[MaxAngularVelocity]", "[MinAngularVelocity]", "[Rom]", "[MaxAcceleration]", "[MinForce]", "[MaxForce]", "[Stiffness1]", "[Stiffness2]", "[Stiffness3]", "[Stiffness4]", "[Stiffness5]");
    //valueArray = ConvertToList("'" + mir.info.OrganisationGuid.ToString() + "'", "'" + mir.info.PatientGuid.ToString() + "'", "'" + mir.info.OperatorGuid.ToString() + "'", "'" + mir.info.PsadGuid.ToString() + "'", "'" + mir.result.MeasureGuid.ToString() + "'", "'" + filestr + "'", "'" + mir.info.MeasureMode.ToString() + "'", "'" + mir.info.Limb.ToString() + "'", "'" + mir.info.Orientation.ToString() + "'", "'" + mir.result.MeasureDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'", "'" + mir.result.Comments.ToString() + "'", mir.result.StartAngle.ToString().Replace(',', '.'), mir.result.MinAngle.ToString().Replace(',', '.'), mir.result.MaxAngle.ToString().Replace(',', '.'), mir.result.MaxAngularVelocity.ToString().Replace(',', '.'), mir.result.MinAngularVelocity.ToString().Replace(',', '.'), mir.result.Rom.ToString().Replace(',', '.'), mir.result.MaxAcceleration.ToString().Replace(',', '.'), mir.result.MinForce.ToString().Replace(',', '.'), mir.result.MaxForce.ToString().Replace(',', '.'), mir.result.Stiffness1.ToString().Replace(',', '.'), mir.result.Stiffness2.ToString().Replace(',', '.'), mir.result.Stiffness3.ToString().Replace(',', '.'), mir.result.Stiffness4.ToString().Replace(',', '.'), mir.result.Stiffness5.ToString().Replace(',', '.'));
    //if (GetTableCount("MeasurementTable", "MeasureGuid", mir.result.MeasureGuid.ToString()) > 0)
    //    UpdateRecord("MeasurementTable", fieldArray, valueArray, "MeasureGuid", mir.result.MeasureGuid.ToString());
    //else
    //    InsertRecord("MeasurementTable", fieldArray, valueArray);

// Old-B1       End



    #endregion OldCode

}

