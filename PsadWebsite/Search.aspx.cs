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
            if (label != null)
            {
                string css = " " + label.Text.ToLower();
                label.CssClass += css;
            }
        }

        private void RepeaterCss(Repeater repeater)
        {
            foreach (RepeaterItem item in repeater.Items)
            {
                ValueToCss(item, "gender");
                ValueToCss(item, "status");
            }
        }

        private void CreateRadioButtons()
        {
            //RadioButton patients = new RadioButton();
            //patients.Text = "Patients";
            //patients.Attributes["value"] = "" + (int)Data.Patients;
            //patients.Attributes.Add("data-toggle");
              
            //PanelGender.Controls.Add(patients);

        }

        #region If Enums Are Used
        private string GetGenderName(EGender gender)
        {
            if (gender == EGender.Male)
                return "Male";
            else
                return "Female";
        }

        // This would be unnessesary if db contained fk that correspond to the enums
        private EGender DetermineGender(string gender)
        {
            if (gender == "male")
                return EGender.Male;
            else return EGender.Female;
        }
        #endregion

        private void FindPeople(string query)
        {
            DataTable patients, operators, organisations;

            patients = SearchHandler.FindPeople(query, EData.Patients);
            operators = SearchHandler.FindPeople(query, EData.Operators);
            organisations = SearchHandler.FindPeople(query, EData.Organisations);

            ShowIfNotEmpty(PanelPatients, patients);
            ShowIfNotEmpty(PanelOperators, operators);
            ShowIfNotEmpty(PanelOrganisations, organisations);

            BindToRepeater(RepeaterPatients, patients);
            BindToRepeater(RepeaterOperators, operators);
            BindToRepeater(RepeaterOrganisations, organisations);

            // This will be done differently if enums are used
            // Binding multple tables to the repeater, or changeing the architecture of the table
            // Or perhaps just reading as rows and parsing values respectively
            RepeaterCss(RepeaterPatients);
            RepeaterCss(RepeaterOperators);
            RepeaterCss(RepeaterOrganisations);

        }

        private void FindPeople(string query, EData group)
        {
            DataTable table = SearchHandler.FindPeople(query, group);

            switch (group)
            {
                case EData.Patients:
                    ShowIfNotEmpty(PanelPatients, table);
                    BindToRepeater(RepeaterPatients, table);
                    RepeaterCss(RepeaterPatients);

                    break;
                case EData.Operators:
                    ShowIfNotEmpty(PanelOperators, table);
                    BindToRepeater(RepeaterOperators, table);
                    RepeaterCss(RepeaterOperators);
                    break;
                case EData.Organisations:
                    ShowIfNotEmpty(PanelOrganisations, table);
                    BindToRepeater(RepeaterOrganisations, table);
                    RepeaterCss(RepeaterOrganisations);
                    break;
            }

            // This will be done differently if enums are used
            // Binding multple tables to the repeater, or changeing the architecture of the table
            // Or perhaps just reading as rows and parsing values respectively
        }

        private void FindPeople(string query, EData group, EGender gender)
        {
            DataTable table = SearchHandler.FindPeople(query, group, gender);

            switch (group)
            {
                case EData.Patients:
                    ShowIfNotEmpty(PanelPatients, table);
                    BindToRepeater(RepeaterPatients, table);
                    RepeaterCss(RepeaterPatients);

                    break;
                case EData.Operators:
                    ShowIfNotEmpty(PanelPatients, table);
                    BindToRepeater(RepeaterOperators, table);
                    RepeaterCss(RepeaterOperators);
                    break;
            }

            // This will be done differently if enums are used
            // Binding multple tables to the repeater, or changeing the architecture of the table
            // Or perhaps just reading as rows and parsing values respectively
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection qs = Page.Request.QueryString;
            //DataTable dt = null;
            bool advanced = true;
            EData group = 0;
            EGender gender = 0;


            if (qs.Count > 0)
            {
                string query = null;

                if (qs[SearchHandler.Query] != null)
                {
                    query = qs[SearchHandler.Query];

                    if (qs[SearchHandler.Group] != null)
                    {
                        Enum.TryParse(qs[SearchHandler.Group], out group);
                        //alternative
                        //group = (EData)Enum.Parse(typeof(EData), qs[SearchHandler.Group]);
                    }
                    if (qs[SearchHandler.Gender] != null)
                    {
                        Enum.TryParse(qs[SearchHandler.Gender], out gender);
                    }

                    if (group > 0)
                    {

                        if (gender > 0)
                        {
                            // search in specific group and after specific gender
                            FindPeople(query, group, gender);
                        }
                        else
                        {
                            //Search in specific group
                            FindPeople(query, group);
                        }
                    }
                    else
                    {
                        FindPeople(query);
                    }
                }
            }
            //foreach (ListItem item in RadioButtonListGroup.Items)
            //{
            //    item.Attributes[]
            //}


            CreateRadioButtons();
            //control..Attributes.Add("data-toggle", "collapse");
            //control.Attributes.Add("data-target", "#advancedGender");


            //RadioButton checkedButton = PanelGender.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            //checkedButton.Attributes["value"].
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string search = TextBoxSearch.Text;
            string searchPage = SiteMaster.SearchpageLink;
            List<string> queryParameters = new List<string>();
            string queryRedirect;

            if (search != string.Empty)
            {
                int group;
                int.TryParse(RadioButtonListGroup.SelectedValue, out group);

                int gender;
                int.TryParse(RadioButtonListGender.SelectedValue, out gender);

                if (group > 0)
                {
                    queryParameters.Add(SearchHandler.Group + SearchHandler.Delimiter + group);
                }

                if (gender > 0)
                {
                    queryParameters.Add(SearchHandler.Gender + SearchHandler.Delimiter + gender);
                }

                queryRedirect = SearchHandler.QueryString(searchPage, search, queryParameters.ToArray());
                Response.Redirect(queryRedirect);
            }
            else
            {
                Response.Redirect(searchPage);
            }
            //Text
            //GetPatient(); //.... lots of different simples queries for that small stuff
        }

        protected void RadioButtonListGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //// show gender choice if patients or operators
            //int group;
            //int.TryParse(RadioButtonListGroup.SelectedValue, out group);
            //if (group > 0)
            //{

            //}

            //always show status
        }
    }
}