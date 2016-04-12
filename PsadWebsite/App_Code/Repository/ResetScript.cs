using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PsadWebsite.App_Code.Repository
{
    public static class ResetScript
    {
        public static string NewCsvPath = "~/Data/";
        public static string OrganisationTableName = "Organisations";
        public static string OperatorTableName = "Operators";
        public static string PatientTableName = "Patients";
        public static string MeasurentTableName = "Measurements";


        private static List<string> organisationFileList = new List<string>();
        private static List<string> operatorFileList = new List<string>();
        private static List<string> patientFileList = new List<string>();
        private static List<string> measurementFileList = new List<string>();


        public static void Reset()
        {
            ImportCSVFiles(NewCsvPath + OrganisationTableName);
            ImportCSVFiles(NewCsvPath + OperatorTableName);
            ImportCSVFiles(NewCsvPath + PatientTableName);
            ImportCSVFiles(NewCsvPath + MeasurentTableName);

            MoveFiles(organisationFileList);
            MoveFiles(operatorFileList);
            MoveFiles(patientFileList);
            MoveFiles(measurementFileList);

            CleanseDb();
        }

        private static void MoveFileToStorage(string file)
        {
            string relativePath = NewCsvPath + Path.GetFileName(file);
            string destination = HostingEnvironment.MapPath(relativePath);

            File.Move(file, destination);
        }

        private static void MoveFiles(List<string> list)
        {
            foreach (string file in list)
            {
                MoveFileToStorage(file);
            }
        }

        private static void ImportCSVFiles(string relativeDirectory)
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

        private static void CleanseDb()
        {
            SqlHandler.ExecuteNonQuery("CleanseDb", true);
        }    
    }
}