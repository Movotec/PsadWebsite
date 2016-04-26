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

// Perhaps we could get data from sql database that has enumerators once and store their values (names) in variables to be used later,
// since we will always get the same data from them.

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

        // Only works if css is named exactly the same as db values
        private static void ValueToCss(RepeaterItem item, string id)
        {
            Label label = (Label)item.FindControl(id);
            //string gender = Eval("gender").ToString();
            string css = " " + label.Text.ToLower();
            label.CssClass += css;
        }

        private void RepeaterCss(Repeater repeater)
        {
            foreach (RepeaterItem item in repeater.Items)
            {
                ValueToCss(item, "gender");
                ValueToCss(item, "status");
            }
        }

        

        #region If Enums Are Used
        private string GetGenderName(Gender gender)
        {
            if (gender == Gender.Male)
                return "Male";
            else
                return "Female";
        }

        // This would be unnessesary if db contained fk that correspond to the enums
        private Gender DetermineGender(string gender)
        {
            if (gender == "male")
                return Gender.Male;
            else return Gender.Female;
        }
        #endregion

        private void FindPeople(string query)
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
            // This will be done differently if enums are used
            // Binding multple tables to the repeater, or changeing the architecture of the table
            // Or perhaps just reading as rows and parsing values respectively
            RepeaterCss(RepeaterPatients);
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
                    FindPeople(query);
                }
            }
        }

        

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string search = TextBoxSearch.Text;

            if (search != string.Empty)
            {
                Response.Redirect(SearchHandler.QueryString(SiteMaster.SearchpageLink, search));
            }
            else
            {
                Response.Redirect(SiteMaster.SearchpageLink);
            }
            //Text
            //GetPatient(); //.... lots of different simples queries for that small stuff
        }
    }
}