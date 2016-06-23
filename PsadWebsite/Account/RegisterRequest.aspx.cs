using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PsadWebsite.Account
{
    public partial class RegisterRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsCallback)
                Email.Text = "cmeisterham@gmail.com";
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            Task sendRequest = GenericEmail.RequestAccount()
        }
    }
}