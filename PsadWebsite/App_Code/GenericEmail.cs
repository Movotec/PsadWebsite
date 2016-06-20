﻿using Microsoft.AspNet.Identity;
using PsadWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace PsadWebsite.App_Code
{
    public class GenericEmail
    {
        public static void VerifyAccount(ApplicationUserManager manager, ApplicationUser user, string password, HttpRequest request)
        {
            string currentUrl = "http://localhost:50196/";
            string verificationPage = "Account/Confirm.aspx"; // The confirmation page gives an error when trying to verify
            // most likeley it is caused by the fact that an admin is creating the users, so when a user token is created it is based on the admins pc... will have to look more into a work around or simply make a custom verification
            string verificationCode = manager.GenerateEmailConfirmationToken(user.Id);

            string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(verificationCode, user.Id, request);

            string subject = "Psad - Confirm your account";
            string body = string.Format(@"You will first be able to use your account it when you have verified it.<br/>
                Please verify your account via <a href='{0}'>link</a><br/>
                Or copy the url to a browser address bar: {0}<br/>
                <br/> Your password is <b>{1}</b>",
                callbackUrl, password);

            // The task stops if i put await on any scope of the method, it runs without it but i does not seeem to send the email.
            manager.SendEmailAsync(user.Id, subject, body);
        }
    }
}