using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Diagnostics;
using PsadWebsite.App_Code.EnitityModels;

namespace PsadWebsite.App_Code.Repository
{
    public static class PsadCalculation
    {
        public static float Accelration1Average(DataTable dataTable) // with specific datatable
        {
             return Average(dataTable, "Acc1X");
        }

        public static double Accelration1Average(Guid id) // With entity framework
        {
            double sum = 0;
            double count = 0;

            using (var db = new PsadDatabase())
            {
                // double array
                var query = from a in db.MeasurementData where a.MeasurementID == id select a.Acc1X;

                foreach (var item in query)
                {
                    sum += item.Value;
                    count++;
                }
            }

            return sum / count;
        }

        public static float Average(DataTable dataTable, string column) // with datatable
        {
            float sum = 0;
            float count = 0;
            try
            {

                foreach (DataRow row in dataTable.Rows)
                {
                    //sum += row[column].ToString();
                    float a;
                    if (float.TryParse(row[column].ToString(), out a))
                    {
                        sum += a;
                    }

                    count++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return sum / count;
        }

    }
}