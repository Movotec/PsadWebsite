using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using PsadWebsite.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.ComponentModel;
using System.Web.Services.Description;
using System.Security.Policy;
using PsadWebsite.App_Code;
using System.Threading.Tasks;

namespace PsadWebsite.Account
{
    public partial class Register : Page
    {
  

        protected void Page_Load(object sender, EventArgs e)
        {
            // Autofill email field
            if (!Page.IsCallback)
            Email.Text = "cmeisterham@gmail.com";
            //Password.Text = "asd";
            //ConfirmPassword.Text = "asd";

            // Only access is authenticed admin
            if (!HttpContext.Current.User.IsInRole("Admin") || !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect(SiteMaster.HomepageLink);
            }            

            if (Request.QueryString["email"] != null)
            {
                Email.Text = Request.QueryString["email"];
            }
        }


        protected void CreateUser_Click(object sender, EventArgs e)
        {
            // Password Generation
            string password = Membership.GeneratePassword(12, 0) + new Random().Next(1000,9999);
            // Perhaps first send the password after the email has been verified...

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() {UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, password);

            //if user does not already exist----


            if (result.Succeeded)
            {
                // Send verification email to newly created user
                Task verify = GenericEmail.VerifyAccount(manager, user, password, Request);

                if (verify.IsCanceled)
                {
                    ErrorMessage.Text = "Cancelled sending mail";
                }
                else if (verify.IsFaulted)
                {
                    ErrorMessage.Text = "Failed to send message";
                }
                else if (verify.IsCompleted)
                {
                    ErrorMessage.Text = "Message successfully sent";// + message.To + ", from " + message.From;
                }

                #region OLD NONFUNCTIONAL CODE


                //new EmailService().SendAsync()??


                // This is semi functional email code, trying to implement a solution that is more integrated with Identity framework

                //using (SmtpClient client = new SmtpClient(office, 587))
                //{
                //    client.EnableSsl = true;
                //    client.Credentials = new NetworkCredential(email, clientPass);

                //    MailMessage message = new MailMessage(clientEmail, email);
                //    message.Subject = "Verify account on PsadWeb";
                //    message.SubjectEncoding = Encoding.UTF8;

                //    message.Body = "Verify user for email " + Email.Text + " by going to " + " <a href='http://localhost:50196/'>link</a> <br/>Your password is \"" + password + "\"";
                //    message.IsBodyHtml = true;
                //    message.BodyEncoding = Encoding.UTF8;

                //    client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                //    return client.SendAsync(message, message); //SendAsync();                
                //}
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                //signInManager.SignIn( user, isPersistent: false, rememberBrowser: false);
                //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);


                #endregion

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            using (MailMessage message = (MailMessage)e.UserState)
            {
                if (e.Cancelled)
                {
                    ErrorMessage.Text = "Cancelled sending mail";
                }

                if (e.Error != null)
                {
                    ErrorMessage.Text = "Failed to send message";
                }
                else
                {
                    ErrorMessage.Text = "Message successfully sent to " + message.To + ", from " + message.From;
                }          
            }
        }
    }
}