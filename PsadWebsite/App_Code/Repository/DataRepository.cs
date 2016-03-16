using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;
using System.Globalization;

namespace PsadWebsite.App_Code.Repository
{
    public class DataRepository
    {
        private CSVClass io = new CSVClass();

        public IEnumerable<object> GetPatients()
        {
            foreach (Patients rec in GetData<Patients>(0, 9999999))
            {
                if (rec.Status != "Deleted")
                    yield return rec;
            }
        }

        public IEnumerable<object> GetOperators()
        {
            foreach (Operators rec in GetData<Operators>(0, 9999999))
                if (rec.Status != "Deleted")
                    yield return rec;
        }

        public IEnumerable<object> GetOrganisations()
        {
            foreach (Organisations rec in GetData<Organisations>(0, 9999999))
                if (rec.Status != "Deleted")
                    yield return rec;
        }

        public IEnumerable<T> GetData<T>(int start, int max)
        {
            object obj = GetNewObject(typeof(T));

            DataTable csvData = io.GetDataTableFromCSVFile(io.GetDataFileName(obj), start, max);

            foreach (DataRow dr in csvData.Rows)
            {
                CopyDataRowToObject(dr, obj, typeof(T));
                yield return (T)obj;
            }
        }

        public static object GetNewObject(Type t)
        {
            try
            {
                return t.GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            catch
            {
                return null;
            }
        }

        public void CopyDataRowToObject(DataRow dr, object obj, Type myType)
        {
            string[] fields = CSVClass.ConvertObjectFieldsToArray(obj);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo myFieldInfo = myType.GetField(fields[i]);
                try
                {
                    if (myFieldInfo.FieldType == typeof(Guid))
                        myFieldInfo.SetValue(obj, Guid.Parse(dr.ItemArray[i].ToString()));
                    else
                        if (myFieldInfo.FieldType == typeof(String))
                            myFieldInfo.SetValue(obj, dr.ItemArray[i].ToString());
                        else
                            if (myFieldInfo.FieldType == typeof(DateTime))
                                myFieldInfo.SetValue(obj, DateTime.ParseExact(dr.ItemArray[i].ToString(), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                            else
                                if (myFieldInfo.FieldType == typeof(Double))
                                {
                                    double d;
                                    double.TryParse(dr.ItemArray[i].ToString(), out d);
                                    myFieldInfo.SetValue(obj, d);
                                }
                                else
                                    if (myFieldInfo.FieldType == typeof(Boolean))
                                        myFieldInfo.SetValue(obj, (dr.ItemArray[i].ToString() == "True") ? true : false);
                                    else
                                        if (myFieldInfo.FieldType == typeof(Byte))
                                        {
                                            int d;
                                            int.TryParse(dr.ItemArray[i].ToString(), out d);
                                            myFieldInfo.SetValue(obj, (Byte)d);
                                        }
                                        else
                                            if (myFieldInfo.FieldType == typeof(Int32))
                                            {
                                                int d;
                                                int.TryParse(dr.ItemArray[i].ToString(), out d);
                                                myFieldInfo.SetValue(obj, d);
                                            }
                                            else
                                                myFieldInfo.SetValue(obj, dr.ItemArray[i].ToString());

                }
                catch (Exception ex) 
                {
                //    MessageBox.Show("Unsuported field: " + ex); 
                }
            }
        }


        // Byte 0-11 reserved Shimmer
        // Byte 12-99 reserved Movotec
        // Byte 100-511 Data
        public static byte[] CompactToCSV(object obj)
        {
            List<string> fieldList = new List<string>();
            StringBuilder CSVString = new StringBuilder();
            int x = 0;

            foreach (FieldInfo f in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var fieldValue = f.GetValue(obj);
                CSVString.Append(fieldValue.ToString());
                CSVString.Append(";");
                x++;
            }
            return System.Text.Encoding.Default.GetBytes(CSVString.ToString());
        }


        private static bool _firstError = true;
        public static T Expand<T>(byte[] data)
        {
            object obj = GetNewObject(typeof(T));

            if (data == null)
                return (T)Convert.ChangeType(obj, typeof(T));

            string[] fields = CSVClass.ConvertObjectFieldsToArray(obj);
            string s = System.Text.Encoding.Default.GetString(data);
            string[] ItemArray = s.Split(';');
            if (ItemArray.Length < fields.Length)
                return (T)Convert.ChangeType(obj, typeof(T));

            Type myType = typeof(T);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo myFieldInfo = myType.GetField(fields[i]);
                try
                {
                    if (myFieldInfo.FieldType == typeof(Guid))
                        myFieldInfo.SetValue(obj, Guid.Parse(ItemArray[i]));
                    else
                        if (myFieldInfo.FieldType == typeof(String))
                            myFieldInfo.SetValue(obj, ItemArray[i]);
                        else
                            if (myFieldInfo.FieldType == typeof(DateTime))
                                myFieldInfo.SetValue(obj, DateTime.ParseExact(ItemArray[i], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                            else
                                if (myFieldInfo.FieldType == typeof(Double))
                                {
                                    double d;
                                    double.TryParse(ItemArray[i], out d);
                                    d = Math.Round(d, 6);
                                    myFieldInfo.SetValue(obj, d);
                                }
                                else
                                    if (myFieldInfo.FieldType == typeof(Boolean))
                                        myFieldInfo.SetValue(obj, (ItemArray[i] == "True") ? true : false);
                                    else
                                        if (myFieldInfo.FieldType == typeof(Byte))
                                        {
                                            int d;
                                            int.TryParse(ItemArray[i], out d);
                                            myFieldInfo.SetValue(obj, (Byte)d);
                                        }
                                        else
                                            if (myFieldInfo.FieldType == typeof(Int32))
                                            {
                                                int d;
                                                int.TryParse(ItemArray[i], out d);
                                                myFieldInfo.SetValue(obj, d);
                                            }
                                            else
                                                myFieldInfo.SetValue(obj, ItemArray[i]);

                }
                catch (Exception ex)
                {
                    if (_firstError == true)
                    {
                        _firstError = false;
                    //    MessageBox.Show("Unsuported field: " + ex);
                    }

                }
            }

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public Patients GetPatient(Guid ID)
        {

            foreach (Patients rec in GetPatients())
            {
                if (rec.RecID == ID)
                    return rec;
            }
            return null;
        }

        public Operators GetOperator(Guid ID)
        {

            foreach (Operators rec in GetOperators())
            {
                if (rec.RecID == ID)
                    return rec;
            }
            return null;
        }

        public Organisations GetOrganisation(Guid ID)
        {

            foreach (Organisations rec in GetOrganisations())
            {
                if (ID == Guid.Empty)
                    return rec;

                if (rec.RecID == ID)
                    return rec;
            }
            return null;
        }

        public string AddPatient(object obj, bool NewID)
        {
            return io.AddRecord(obj, NewID);
        }

        public string AddOperator(object obj, bool NewID)
        {
            return io.AddRecord(obj, NewID);
        }

        public string AddOrganisation(object obj, bool NewID)
        {
            return io.AddRecord(obj, NewID);
        }

        public bool DeletePatient(Guid ID)
        {
            return io.DeleteRecord(new Patients(), ID);
        }

        public bool DeleteOperator(Guid ID)
        {
            return io.DeleteRecord(new Operators(), ID);
        }

        public bool DeleteOrganisation(Guid ID)
        {
            return io.DeleteRecord(new Organisations(), ID);
        }


        public bool UpdatePatient(Repository.Patients obj)
        {
            DeletePatient(obj.RecID);
            AddPatient(obj, false);
            return true;
        }

        public bool UpdateOperator(Operators obj)
        {
            DeleteOperator(obj.RecID);
            AddOperator(obj, false);
            return true;

        }

        public bool UpdateOrganisation(Organisations obj)
        {
            DeleteOrganisation(obj.RecID);
            AddOrganisation(obj, false);
            return true;
        }
    }

    public class Patients
    {
        public Guid RecID;
        public string PatientName;
        public string Address;
        public string BornYear;
        public string BornMonth;
        public string BornDay;
        public string Weight;
        public string Height;
        public string Phone;
        public string Email;
        public string City;
        public string ShoeSize;
        public string Comment;
        public string AnkleCircumSize;
        public string Gender;
        public string Status;
    }

    public class Operators
    {
        public Guid RecID;
        public Guid OrganisationID;
        public string OperatorName;
        public string Address;
        public string City;
        public string Email;
        public string Phone;
        public string Gender;
        public string Type;
        public string Status;
    }

    public class Organisations
    {
        public Guid RecID;
        public string OrganisationName;
        public string Address;
        public string City;
        public string Phone;
        public string Contact;
        public string Email;
        public string Status;
    }
}