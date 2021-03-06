﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using PsadWebsite.App_Code.Repository;
using PsadWebsite.App_Code.EnitityModels;
using System.Diagnostics;
using PsadWebsite.App_Code;

namespace PsadWebsite
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //DataTable datatable = SearchHandler.GetPatientsBasedOnOpereator("Johan");

            //SqlCommand cmd = new SqlCommand();
            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EIPsad"].ConnectionString);
            //cmd.Connection = conn;
            //cmd.CommandText = "SELECT id FROM test_table";

            //SqlDataAdapter adap = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //adap.Fill(ds);
            //RepeaterTest.DataSource = ds;
            //RepeaterTest.DataBind();


       
        }

        protected void ButtonEntityFramework_Click(object sender, EventArgs e)
        {
            using (var db = new PsadDatabase())
            {
                var measurement = new Measurements();
                measurement.MeasureGuid = Guid.NewGuid();
                measurement.PsadGuid = Guid.NewGuid();
                measurement.MeasureDateTime = DateTime.Now;
                measurement.RecID = 1;

                db.Measurements.Add(measurement);
                db.SaveChanges();

                //db.Database.SqlQuery()
                
                var query = from m in db.Measurements orderby m.MeasureDateTime select m;

                foreach (var item in query)
                {
                    Debug.WriteLine(item.MeasureGuid + " " + item.RecID);
                }

                for (int i = 1; i <= 3; i++)
                {
                    var data = new MeasurementData();
                    data.MeasurementID = measurement.MeasureGuid;
                    data.Acc1X = 3 * i;
                    db.MeasurementData.Add(data);
                }
                db.SaveChanges();

                PsadCalculation.Accelration1Average(measurement.MeasureGuid);
            }
        }

        protected void ButtonClean_Click(object sender, EventArgs e)
        {
            PsadData data = new PsadData(true);
            
        }

        protected void ButtonReset_Click(object sender, EventArgs e)
        {
            ResetScript.Reset();
        }
    }
}