using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndoAgri.SapToDB.Api.Helper
{
    public class ParamHelper
    {
        public static string SMTPClient()
        {
            string client = "10.0.0.1";
            return client;
        }
        public static int SMTPPort()
        {
            int port = 25;
            return port;
        }

        public static string DefaultEmailTo()
        {
            string DefaultEmailTo = "@email.com";
            return DefaultEmailTo;
        }
        public static bool ActiveNotification()
        {
            bool ActiveNotification = true;
            return ActiveNotification;
        }
        public static string DefaultEmailFrom()
        {
            string DefaultEmailFrom = "@email.com";
            return DefaultEmailFrom;
        }

        public static List<string> GetEmailBlock()
        {
            List<string> email = new List<string>();
            email.Add("Istanbul");
            email.Add("Athens");
            email.Add("Sofia");
            return email;
        }

        public static List<string> GetEmailCC()
        {
            List<string> email = new List<string>();
            email.Add("email.com");
            return email;
        }
    }
}