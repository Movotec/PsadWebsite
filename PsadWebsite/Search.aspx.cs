using PsadWebsite.App_Code;
using PsadWebsite.App_Code.Repository;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PsadWebsite
{
    public partial class Search : Page
    {
        private void BindToRepeater(Repeater repeater, DataTable table)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        private void ShowIfNotEmpty(Panel panel, DataTable table)
        {
            if (table.Rows.Count > 0)
            {
                panel.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection qs = Page.Request.QueryString;
            //DataTable dt = null;
            bool advanced = false;


            if (qs.Count > 0)
            {
                string query = null;

                if (qs[SearchHandler.Q] != null)
                {
                    query = qs[SearchHandler.Q];
                }


                if (advanced)
                {

                }
                else
                {
                    SqlParameter para1 = new SqlParameter("@name", SqlDbType.NVarChar, 50);
                    SqlParameter para2 = new SqlParameter("@name", SqlDbType.NVarChar, 50);
                    SqlParameter para3 = new SqlParameter("@name", SqlDbType.NVarChar, 50);

                    para1.Value = query;
                    para2.Value = query;
                    para3.Value = query;

                    DataTable patients, operators, organisations;

                    patients = SqlHandler.QueryDataTable("GetPatientsByName", para1);
                    operators = SqlHandler.QueryDataTable("GetOperatorsByName", para2);
                    organisations = SqlHandler.QueryDataTable("GetOrganisationsByName", para3);

                    ShowIfNotEmpty(PanelPatients, patients);
                    //ShowIfNotEmpty(RepeaterOperators, operators);
                    //ShowIfNotEmpty(RepeaterOrganisations, organisations);

                    BindToRepeater(RepeaterPatients, patients);
                    BindToRepeater(RepeaterOperators, operators);
                    BindToRepeater(RepeaterOrganisations, organisations);
                }
            }
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string search = TextBoxSearch.Text;

            if (search != string.Empty)
            {
                Response.Redirect(SearchHandler.QueryString(SiteMaster.searchPage, search));
            }
            else
            {
                Response.Redirect(SiteMaster.searchPage);
            }
            //Text
            //GetPatient(); //.... lots of different simples queries for that small stuff
        }
    }
}