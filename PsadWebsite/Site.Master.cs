using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using PsadWebsite.App_Code;
using System.IO;

namespace PsadWebsite
{
    public partial class SiteMaster : MasterPage
    {
        public const string siteName = "PSAD";
        private const string scriptLink = "~/";
        private const string prefix = "~/";
        private const string ext = ".aspx";
        //public const string HomepageLink = "~/";
        //public const string SearchpageLink = "~/search.aspx";
        //public const string AboutpageLink = "~/about.aspx";
        //public const string ContactpageLink = "~/contact.aspx";

        private static List<Link> links = new List<Link>
        {
            new Link("Home", "Default", prefix, ext),
            new Link("Search", prefix, ext),
            new Link("About", prefix, ext),
            new Link("Contact", prefix, ext),
        };

        private static List<Link> adminLinks = new List<Link>
        {
            new Link("Register", prefix, ext),
            new Link("Manage", prefix, ext),
        };

        private static Dictionary<string, string> pageLinks = new Dictionary<string, string>()
        {
            { "Home", "~/" },
            { "Search", "~/Search" },
            { "About", "~/About" },
            { "Contact",  "~/Contact" }
        };

        private static Dictionary<string, string> adminLinks2 = new Dictionary<string, string>()
        {
            { "Register", "~/Account/Register" },
            { "Manage", "~/Account/Manage" },
        };

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        public static string HomepageLink
        {
            get { return pageLinks["Home"]; }
        }
        public static string SearchpageLink
        {
            get { return pageLinks["Search"]; }
        }
        public static string AboutpageLink
        {
            get { return pageLinks["About"]; }
        }
        public static string ContactpageLink
        {
            get { return pageLinks["Contact"]; }
        }

        private string TrimLink(string url)
        {
            string trimmed = url.TrimStart('~');
            trimmed = trimmed.TrimEnd('.');
            return trimmed;

        }

        // Need a smart way to compare links, right now link from dictionary tries to compare against clean url, perhaps just compare against name
        // But that would fail if you rename it to danish
        //private bool IsCurrentPage(string url)
        //{
        //    string name = Path.GetFileName(Request.Url.AbsolutePath);
        


        //    return boo;
        //}

        private string CurrentPage()
        {
            return Path.GetFileName(Request.Url.AbsolutePath);
        }

        private void SetCurrentPageAttributes(ListItem item, Link link, string idPrefix, string css)
        {
            item.Attributes.Add("ID", idPrefix + link.Name);

            string curpage = CurrentPage();


            if (link.Equals(CurrentPage()))
                item.Attributes.Add("class", css);

        }

        private void SetNavigation()
        {
            BulletedListNavigation.DisplayMode = BulletedListDisplayMode.HyperLink;

            foreach (Link link in links)
            {
                ListItem listItem = new ListItem(link.Name, link.FullLink);
                SetCurrentPageAttributes(listItem, link, "HyperLink", "current-page");
                BulletedListNavigation.Items.Add(listItem);
            }

            //foreach (KeyValuePair<string, string> item in pageLinks)
            //{
            //    ListItem listItem = new ListItem(item.Key, item.Value);
            //    SetCurrentPageAttributes(listItem, "current-page");
            //    BulletedListNavigation.Items.Add(listItem);
            //}

            //if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.IsInRole("Admin"))
            //{
            //    foreach (KeyValuePair<string, string> item in adminLinks)
            //    {
            //        ListItem listItem = new ListItem(item.Key, item.Value);
            //        SetCurrentPageAttributes(listItem, "current-page");
            //        BulletedListNavigation.Items.Add(listItem);
            //    }
            //}


        }

        protected void Page_Init(object sender, EventArgs e)
        {

            LiteralSiteName.Text = siteName;
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetNavigation();
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string search = TextBoxSearch.Text;

            if (search != string.Empty)
            {                
                Response.Redirect(SearchHandler.QueryString(SearchpageLink, search));
            }
            else
            {
                Response.Redirect(SearchpageLink);
            }
            //Text
            //GetPatient(); //.... lots of different simples queries for that small stuff
        }
    }

}