using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PsadWebsite.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;

namespace PsadWebsite
{
    //public class EmailService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        // Plug in your email service here to send an email.
    //        return Task.FromResult(0);
    //    }

    //    internal void SendAsync(object indetityMessage)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    // This is a overide of EmailService wich can and is in my case inteded to be used for email verification
    // It is still unclear how this would be called, it seems that i have to access/write/overwrite code in the identity framework to be able to access it

        // It is called via an instance of ApplicationUserManager, where user.Id is supplied as an IdentityMessage containing subject and body
        //This is specifically used for account verification, but technically this could be made more generically for sending emails and the context should be created
        //outside in the corresponding page. Ergo: Register.aspx should contain all the context for the email where as this "Task" should simply be sending the email
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Alertinative
            //ConfigurationManager.AppSettings["emailServiceUserName"],
            //     ConfigurationManager.AppSettings["emailServicePassword"]
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup group = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            SmtpSection smtp = group.Smtp;

            // All sender email values are stored in web.config under system.net -> mailSettings
            string office = smtp.Network.Host;
            string clientEmail = smtp.Network.UserName; // smpt.From alterativly
            string clientPass = smtp.Network.Password;
            int port = smtp.Network.Port;
            //string text = string.Format("Please clock on this link to {0}: {1}", message.Subject, message.Body);
            //string html = "Please confirm your account by clicking this link: <a href='" + message.Body + "'>link</a><br/>";

            //html += HttpUtility.HtmlEncode(@"Or copy the following link to your browser: " + message.Body);

            MailMessage msg = new MailMessage(clientEmail, message.Destination, message.Subject, message.Body);
            msg.IsBodyHtml = true;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.BodyEncoding = Encoding.UTF8;

            using (SmtpClient client = new SmtpClient(office, port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(clientEmail, clientPass);

                //client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                await client.SendMailAsync(msg); //SendAsync();        
            }

        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = true,
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
