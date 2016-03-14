using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Data;


namespace HECPsadWebsite.Repository
{
    public class HECPsadData
    {
        public int ImportCSVFiles()
        {
            string _measurePathHECPsad = System.Web.Hosting.HostingEnvironment.MapPath("~/HECPsad/");
            List<string> OrganisationFileList = new List<string>();
            List<string> PsadFileList = new List<string>();
            List<string> OperatorFileList = new List<string>();
            List<string> PatientFileList = new List<string>();
            List<string> MeasurementFileList = new List<string>();

            // Iterate through all CSV files
            foreach (string filestr in Directory.GetFiles(_measurePathHECPsad, "*.csv", SearchOption.AllDirectories))
            {
                if ((File.GetAttributes(filestr) & FileAttributes.Archive) == FileAttributes.Archive)
                {
                    if (filestr.Contains("Organisations_"))
                        OrganisationFileList.Add(filestr);

                    if (filestr.Contains("Measurement_"))
                        MeasurementFileList.Add(filestr);

                    if (filestr.Contains("Operators_"))
                        OperatorFileList.Add(filestr);

                    if (filestr.Contains("Patients_"))
                        PatientFileList.Add(filestr);

                    if (filestr.Contains("PSADs_"))
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

        private  bool ImportOrganisations(List<string> filelist)
        {
            return false;
        }

        private  bool ImportPsads(List<string> filelist)
        {
            return false;
        }

        private  bool ImportOperators(List<string> filelist)
        {
            return false;
        }

        private  bool ImportPatients(List<string> filelist)
        {
            return false;
        }

        private  bool ImportMeasurements(List<string> filelist)
        {
            string[] fieldArray;
            string[] valueArray;

            // Iterate through filelist
            foreach (string filestr in filelist)
            { 
                MeasureInfoAndResult mir =  GetMeasureInfoAndResult(filestr);
                DateTime dt = new DateTime(mir.result.MeasureDateTime.Ticks);
                if (mir.result.MeasureDateTime.Ticks < 1)
                    mir.result.MeasureDateTime = DateTime.Now;

                // Do calculations (TUE, Stenberg Johan ...)
                fieldArray = ConvertToList("[OrganisationGuid]","[PatientGuid]","[OperatorGuid]","[PsadGuid]","[MeasureGuid]","[ExternFileName]","[MeasureMode]","[Limb]","[Orientation]","[MeasureDateTime]","[Comments]","[StartAngle]","[MinAngle]","[MaxAngle]","[MaxAngularVelocity]","[MinAngularVelocity]","[Rom]","[MaxAcceleration]","[MinForce]","[MaxForce]","[Stiffness1]","[Stiffness2]","[Stiffness3]","[Stiffness4]","[Stiffness5]");
                valueArray = ConvertToList("'" + mir.info.OrganisationGuid.ToString() + "'","'" + mir.info.PatientGuid.ToString() + "'","'" + mir.info.OperatorGuid.ToString() + "'","'" + mir.info.PsadGuid.ToString() + "'","'" + mir.result.MeasureGuid.ToString() + "'","'" + filestr + "'","'" + mir.info.MeasureMode.ToString() + "'","'" + mir.info.Limb.ToString() + "'","'" + mir.info.Orientation.ToString() + "'","'" + mir.result.MeasureDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'","'" + mir.result.Comments.ToString() + "'", mir.result.StartAngle.ToString().Replace(',', '.') , mir.result.MinAngle.ToString().Replace(',', '.') , mir.result.MaxAngle.ToString().Replace(',', '.') , mir.result.MaxAngularVelocity.ToString().Replace(',', '.') , mir.result.MinAngularVelocity.ToString().Replace(',', '.') , mir.result.Rom.ToString().Replace(',', '.') , mir.result.MaxAcceleration.ToString().Replace(',', '.') , mir.result.MinForce.ToString().Replace(',', '.') , mir.result.MaxForce.ToString().Replace(',', '.') , mir.result.Stiffness1.ToString().Replace(',', '.') , mir.result.Stiffness2.ToString().Replace(',', '.') , mir.result.Stiffness3.ToString().Replace(',', '.') , mir.result.Stiffness4.ToString().Replace(',', '.') , mir.result.Stiffness5.ToString().Replace(',', '.'));
                if(GetTableCount("MeasurementTable", "MeasureGuid", mir.result.MeasureGuid.ToString()) > 0)
                    UpdateRecord("MeasurementTable", fieldArray, valueArray, "MeasureGuid", mir.result.MeasureGuid.ToString());
                else
                    InsertRecord("MeasurementTable", fieldArray, valueArray);
            }

            // Clear archive bit
            return false;
        }

        private bool UpdateRecord(string table, string[] fields, string[] values, string field, string key)
        {
            string updateStr = null;

            if (fields.Length != values.Length)
                return false;

            for (int x = 0; x < fields.Length; x++)
            {
                updateStr += fields[x] + "=" + values[x];
                if (x < fields.Length - 1)
                {
                    updateStr += ",";
                }
            }

            string insertStr = string.Format("UPDATE {0} SET {1} where {2} like '{3}'", table, updateStr, field, key);
            ExecuteSqlStatement(insertStr);
            return true;
        }

        private bool InsertRecord(string table, string[] fields, string[] values)
        {
            string fieldStr=null;
            string valueStr = null;
            if (fields.Length != values.Length)
                return false;

            for(int x=0;x<fields.Length;x++)
            {
                fieldStr += fields[x];
                valueStr += values[x];
                if (x < fields.Length - 1)
                {
                    fieldStr += ",";
                    valueStr += ",";
                }
            }

            string insertStr = string.Format("INSERT INTO {0} ({1}) VALUES({2})", table, fieldStr, valueStr);
            ExecuteSqlStatement(insertStr);
            return true;
        }

        public string[] ConvertToList(params string[] args)
        {
            return args.ToArray();
        }

        public  MeasureInfoAndResult GetMeasureInfoAndResult(string file)
        {
            Repository.DataRepository datarepository = new DataRepository();
            PsadMeasureInfo psadBasics = new PsadMeasureInfo();

            DataTable datatable = new DataTable();

            datatable = csvIO.GetDataTableFromCSVFile(file, 0, 2);
            datarepository.CopyDataRowToObject(datatable.Rows[0], psadBasics, typeof(PsadMeasureInfo));

            PsadMeasureResult mr = new PsadMeasureResult();
            datatable = csvIO.GetDataTableFromCSVFile(file, 2, 2);
            if(datatable.Rows.Count>0)
                datarepository.CopyDataRowToObject(datatable.Rows[0], mr, typeof(PsadMeasureResult));

            MeasureInfoAndResult measureInfoAndResult = new MeasureInfoAndResult();
            measureInfoAndResult.info = psadBasics;
            measureInfoAndResult.result = mr;

            return measureInfoAndResult;
        }

        public int GetMeasurementCount()
        {
            return GetTableCount("MeasurementTable");
        }

        public static int GetPatientCount()
        {
            return GetTableCount("PatientTable");
        }
        public static int GetOrganisationCount()
        {
            return GetTableCount("OrganisationTable");
        }

        public static int GetOperatorCount()
        {
            return GetTableCount("OperatorTable");
        }

        public static int GetPsadCount()
        {
            return GetTableCount("PsadTable");
        }

        public static int DeleteRecord(string tablename, string fieldName, string Key)
        {
            string stmt = string.Format("DELETE FROM {0} where {1} like '{2}' ", tablename, fieldName, Key);
            try
            {
                using (SqlConnection thisConnection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                    {
                        thisConnection.Open();
                        cmdCount.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;
        }

        public static int GetTableCount(string tablename, string fieldName, string Key)
        {
            string stmt = string.Format("SELECT COUNT(*) FROM {0} where {1} like '{2}' ", tablename, fieldName, Key);
            int count = 0;
            try
            {
                using (SqlConnection thisConnection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                    {
                        thisConnection.Open();
                        count = (int)cmdCount.ExecuteScalar();
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int GetTableCount(string tablename)
        {
            string stmt = string.Format("SELECT COUNT(*) FROM {0}", tablename);
            int count = 0;
            try
            {
                using (SqlConnection thisConnection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                    {
                        thisConnection.Open();
                        count = (int)cmdCount.ExecuteScalar();
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int ExecuteSqlStatement(string sqlStatement)
        {
            int count = 0;
            try
            {
                using (SqlConnection thisConnection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmdCount = new SqlCommand(sqlStatement, thisConnection))
                    {
                        thisConnection.Open();
                        cmdCount.ExecuteScalar();
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


  
        private CSVClass csvIO = new CSVClass();
        private static string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\MSSQLSERVER\HECPsadDB.mdf;Integrated Security=True;Connect Timeout=30";
    }

    public class MeasureInfoAndResult
    {
        public PsadMeasureInfo info = new PsadMeasureInfo();
        public PsadMeasureResult result = new PsadMeasureResult();
    }
}
