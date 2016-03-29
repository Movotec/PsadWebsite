using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using PsadWebsite.App_Code.Repository;

namespace PsadWebsite
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected void ButtonCsvToSql_Click(object sender, EventArgs e)
        {
            PsadData data = new PsadData();
            data.ImportCSVFiles();

            Response.Redirect(Request.RawUrl);

            //List<string> columns;
            //columns = data.GetSqlColumnNames("Measurements");
            //string val = columns[3];
        }

    }
}