using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;
using System.Web;

namespace PsadWebsite.App_Code.Repository
{
    class CSVClass
    {
        public CSVClass()
        {
            EnsurePath(_dataFolder);
        }

        public string CreateCSVFile(object obj, string FilePrefix, string folder)
        {
            Guid measureGuid = Guid.NewGuid();
            string fileName = GenerateFileName(FilePrefix, folder, measureGuid);
            File.WriteAllText(fileName, ConvertOBJFields2CSV(obj));
            return fileName;
        }

        public string CreateFile(string FilePrefix, string folder, ref Guid measureGuid)
        {
            measureGuid = Guid.NewGuid();
            string fileName = GenerateFileName(FilePrefix, folder, measureGuid);
            return fileName;
        }

        public string GenerateFileName(string FilePrefix, string folder, Guid guid)
        {
            string fileName = string.Format("{0}{1}_{2}{3}", FullPath(folder), FilePrefix, guid, _fileextension);
            return fileName;
        }


        public string FullPath(string Folder)
        {
            return Directory.GetCurrentDirectory() + Folder;
        }

        public string GetDataFileName(object obj)
        {
            string[] files = System.IO.Directory.GetFiles(FullPath(_dataFolder), obj.GetType().Name + "*" + _fileextension);
            if (files.Count() > 0)
                return files[0];
            else
                return GetNewFileName(obj, _dataFolder);
        }

        public string GetNewFileName(object obj, string folder)
        {
            return CreateCSVFile(obj, obj.GetType().Name, folder);
        }

        public bool EnsurePath(string Path)
        {
            string path = HttpContext.Current.Server.MapPath(_dataFolder);

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            return true;
        }

        //public bool EnsurePath(string Path)
        //{
        //    string[] paths = FullPath(Path).Split('\\');

        //    string path = null;
        //    for (int i = 0; i < paths.Length; i++)
        //    {
        //        path = path + paths[i];
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        path += @"\";
        //    }
        //    return true;
        //}

        public DataTable GetDataTableFromCSVFile(string csvFilePath, int start, int MaxCount)
        {
            DataTable csvData = new DataTable();
            int count = 0;
            MaxCount += start;

            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csvFilePath, System.Text.Encoding.Default))
                {
                    csvReader.SetDelimiters(new string[] { _delimeter });
                    csvReader.HasFieldsEnclosedInQuotes = true;



                    while (count++ < start) //Skip line
                        csvReader.ReadFields();

                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData && count++ < MaxCount)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == string.Empty)
                            {
                                fieldData[i] = null;
                            }
                        }

                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("CSVClass " + ex.ToString());
            }
            return csvData;
        }

        public DataTable SaveDataTableFromCSVFile(string csv_file_path, DataTable csvData)
        {
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path, System.Text.Encoding.Default))
                {
                    csvReader.SetDelimiters(new string[] { _delimeter });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == string.Empty)
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvData;
        }

        private string[] ClassValuesToArray(object obj)
        {
            List<string> fieldList = new List<string>();
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo f in fields)
            {
                var fieldValue = f.GetValue(obj);
                fieldList.Add((fieldValue == null) ? string.Empty : fieldValue.ToString());
            }
            return fieldList.ToArray();
        }

        public string AddRecord(object obj, bool generateNewID)
        {
            string[] Record;
            try
            {
                Record = ClassValuesToArray(obj);
                if (generateNewID == true)
                    Record[0] = Guid.NewGuid().ToString();

                string FileName = GetDataFileName(obj);
                string delimeter = string.Empty;
                string recordLine = string.Empty;
                List<string> lineList = new List<string>();

                string[] lines = System.IO.File.ReadAllLines(FileName, System.Text.Encoding.Default);
                lines[0] = ConvertOBJFields2CSV(obj);

                foreach (string line in lines)
                {
                    if (line.Length > 0 && !line.Contains(_synchronized))
                    {
                        lineList.Add(line);
                    }
                }

                for (int i = 0; i < Record.Length; i++)
                {
                    recordLine += delimeter + Record[i];
                    delimeter = _delimeter;
                }

                lineList.Add(recordLine);

                System.IO.File.WriteAllLines(FileName, lineList, System.Text.Encoding.Default);
            }
            catch (Exception)
            {

                return null;
            }

            return Record[0];
        }

        public void SaveObjListToFile(string FileName, Object obj)
        {
            List<string> lineList = new List<string>();
            string lineStr = ConvertFields2CSV(obj);
            lineList.Add(lineStr);
            lineStr = ConvertFieldValues2CSV(obj);
            lineList.Add(lineStr);
            System.IO.File.AppendAllLines(FileName, lineList, System.Text.Encoding.Default);
        }

        public void SaveObjListToFile(string FileName, Object[] objList)
        {
            List<string> lineList = new List<string>();
            string lineStr = ConvertFields2CSV(objList[0]);
            lineList.Add(lineStr);
            foreach (object obj in objList)
            {
                lineStr = ConvertFieldValues2CSV(obj);
                lineList.Add(lineStr);
            }
            System.IO.File.AppendAllLines(FileName, lineList, System.Text.Encoding.Default);

        }

        public bool DeleteRecord(object obj, Guid ID)
        {
            bool found = false;

            try
            {
                string[] objFields = ConvertObjectFieldsToArray(obj);
                string FileName = GetDataFileName(obj);
                string delimeter = string.Empty;
                string recordLine = string.Empty;
                List<string> lineList = new List<string>();

                string[] lines = System.IO.File.ReadAllLines(FileName, System.Text.Encoding.Default);

                foreach (string line in lines)
                {
                    if (line.Length > 0 && !line.Contains(_synchronized))
                    {
                        if (IsRecordFound(ID, line) == true)
                            found = true;
                        else
                            lineList.Add(line);
                    }
                }

                if (found == true)
                    System.IO.File.WriteAllLines(FileName, lineList, System.Text.Encoding.Default);
            }
            catch (Exception)
            {
                return false;
            }

            return found;
        }

        private static bool IsRecordFound(Guid ID, string line)
        {
            string[] fields = line.Split(_delimeter[0]);
            if (fields[0] == ID.ToString())
                return true;
            return false;
        }

        public static string[] ConvertObjectFieldsToArray(object obj)
        {
            List<string> fieldList = new List<string>();
            foreach (FieldInfo f in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                fieldList.Add(f.Name);

            return fieldList.ToArray();
        }

        public string ConvertOBJFields2CSV(object obj)
        {
            string csvStr = null;
            string delimeter = string.Empty;
            foreach (System.Reflection.MemberInfo member in obj.GetType().GetMembers())
            {
                if (member.GetType().UnderlyingSystemType.FullName == "System.Reflection.RtFieldInfo")
                {
                    csvStr += (delimeter + member.Name);
                    delimeter = _delimeter;
                }
            }
            return csvStr;
        }

        public string ConvertFields2CSV(object obj)
        {
            string csvStr = null;
            string delimeter = string.Empty;
            string[] fields = ConvertObjectFieldsToArray(obj);
            for (int i = 0; i < fields.Length; i++)
            {
                csvStr += (delimeter + fields[i]);
                delimeter = _delimeter;
            }
            return csvStr;
        }

        public string ConvertFieldValues2CSV(object obj)
        {
            string csvStr = null;
            string delimeter = string.Empty;
            string[] values = ClassValuesToArray(obj);
            for (int i = 0; i < values.Length; i++)
            {
                csvStr += (delimeter + values[i]);
                delimeter = _delimeter;
            }
            return csvStr;
        }


        private const string _dataFolder = @"~\Data\";
        private const string _fileextension = @".csv";
        private const string _delimeter = @";";
        private const string _synchronized = "Synchronized";
    }
}