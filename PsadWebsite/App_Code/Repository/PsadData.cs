using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;

namespace PsadWebsite.App_Code.Repository
{
    public class PsadData
    {
      

        public int ImportCSVFiles()
        {
            string _measurePathHECPsad = System.Web.Hosting.HostingEnvironment.MapPath(_newCsvPath);
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

            ImportOrganisations(OrganisationFileList);
            ImportPsads(PsadFileList);
            ImportOperators(OperatorFileList);
            ImportPatients(PatientFileList);
            ImportMeasurements(MeasurementFileList);


            return 0;
        }

       
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


        private bool ImportOrganisations(List<string> filelist)
        {
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
            //string[] fieldArray;
            //string[] valueArray;

            // Iterate through filelist
            foreach (string filestr in filelist)
            {
                // Just transfer to datatable where table columns are called that same as in the sql tables
                DataTable measurement = GetMeasurement(filestr);

                InsertRecord(measurement, _measurementsTable);

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
            }

            // Clear archive bit
            return false;
        }

        // Creates a datatable of csv measurment file, the table conains however many columns are in the file and 1 row of vaules
        private DataTable GetMeasurement(string csvFilePath)
        {
            DataTable csvData = new DataTable();
            //int count = 0;
            int lines = 4; // We only look at the 4 first lines of measurement csv file for the data needed for the measurement table
            //MaxCount += start;

            List<string> sqlColumn;

            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csvFilePath, System.Text.Encoding.Default))
                {
                    csvReader.SetDelimiters(new string[] { _delimeter });
                    csvReader.HasFieldsEnclosedInQuotes = true;

                    for (int i = 1; i <= lines; i++)
                    {
                        string[] colFields = csvReader.ReadFields();


//!!                        // This should actually check if there is a column with this name in the sql table
//!!                        // Perhaps get name of all sql table column names and compare against to see if it exsits, if it does'nt disregard data


                        if ((i % 2) == 0) // if even number it is row values
                        {
                            for (int j = 0; j < colFields.Length; j++)      
                            {
                                if (colFields[i] == string.Empty)
                                {
                                    colFields[i] = null;
                                }
                            }

                            csvData.Rows.Add(colFields);
                        }
                        else // if odd it is column names
                        {
                            foreach (string column in colFields)
                            {
                                DataColumn dataColumn = new DataColumn(column);
                                dataColumn.AllowDBNull = true;
                                csvData.Columns.Add(dataColumn);
                            }
                        }
                    }

                    //while (count++ < start) //Skip line
                    //    csvReader.ReadFields();



                    //while (!csvReader.EndOfData && count++ < MaxCount)
                    //{
                    //    string[] fieldData = csvReader.ReadFields();


                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Converting csv file to datatable: " + ex.ToString());
            }
            return csvData;
        }

        public List<string> GetSqlColumnNames(string table)
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
                            for (int i = 1; i <= reader.FieldCount; i++)
                            {
                                //if (!reader.IsDBNull(i))
                                columnNames.Add(reader["COLUMN_NAME"].ToString());  // No data is present, perhaps sql statement is lacking "scheme" for table. Test other sql statement
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return columnNames;
        }


        private void InsertRecord(DataTable dataTable, string sqlTable)
        {
            string sqlColumns = "";
            string sqlValues = "";
            string sqlStatement = "";

            int count = 0;
            int columnAmount = dataTable.Columns.Count;
            int rowAmount = dataTable.Rows.Count;

            

            foreach (DataColumn column in dataTable.Columns)
            {
                sqlColumns += column.ColumnName;
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
                    string val = row[column].ToString();
                    if (val == "" || val == string.Empty)
                    {
                        sqlValues += "NULL";
                    }
                    else
                    {
                        sqlValues += val;
                    }
                   
                    count++;

                    if (count < columnAmount)
                    {
                        sqlValues += ",";
                    }
                }
            }

            sqlStatement = string.Format("INSERT INTO {0} ({1}) VALUES({2})", sqlTable, sqlColumns, sqlValues);

            ExecuteSqlStatement(sqlStatement);
        }

        private static int ExecuteSqlStatement(string sqlStatement)
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
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        private string[] ConvertToList(params string[] args)
        {
            return args.ToArray();
        }

        private CSVClass csvIO = new CSVClass();
        private static string _connectionString = ConfigurationManager.ConnectionStrings["PsadData"].ConnectionString; //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\MSSQLSERVER\HECPsadDB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string _newCsvPath = "~/Data/";
        private static string _delimeter = ";";
        private static string _measurementsTable = "Measurements";
    }
}