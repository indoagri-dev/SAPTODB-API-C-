using Indoagri.IndoagriPOICAT.Model.Models;
using IndoAgri.SapToDB.Api.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace IndoAgri.SapToDB.Api.Emails
{
    public class SendEmail
    {
         private DB_POICATEntities _db;
         public SendEmail()
        {
            try
            {
                _db = new DB_POICATEntities();
            }
            catch (Exception)
            {
                _db = new DB_POICATEntities();
            }
        }


        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void sendEmailAll(string to, string cc, string bcc, string template, string Subject)
        {
            SmtpClient mailServer = new SmtpClient("10.0.0.1");
            MailMessage mess = new MailMessage();
            mess.From = new MailAddress("@email.com");
            if (true)
                mess.To.Add(to);
            else
                mess.To.Add("@email.com");

            foreach (string itemCC in ParamHelper.GetEmailCC())
            {
                mess.CC.Add(itemCC);
            }
            mess.Subject = Subject;
            mess.Body = template;
            mess.IsBodyHtml = true;

            mailServer.Send(mess);
            mess = null;
            mailServer = null;
            GC.Collect();
        }

        public static void sendEmail(string to, string cc, string bcc, string template, string Subject)
        {
            try
            {
                Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(BaseConstants.BASE64_ICAT_LOGO));
                System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
                to = to.Replace(",", ";");
                using (SmtpClient client = new SmtpClient("10.0.0.1", 25))
                {
                    MailMessage newMail = new MailMessage();
                    string DefaultEmailTo = "@email.com";
                    bool ActiveNotification = true;
                    newMail.From = new MailAddress("@email.com");
                    List<string> lstBlockEmail = ParamHelper.GetEmailBlock();
                    if (ActiveNotification)
                    {
                        foreach (string temp in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (!lstBlockEmail.Any(temp.Contains))
                            {
                                if (!newMail.To.Any(s => s.Address == temp))
                                {
                                    newMail.To.Add(temp);
                                }
                            }
                        }
                    }
                    else
                    {
                        newMail.To.Add(DefaultEmailTo);
                    }

                    foreach (string itemCC in ParamHelper.GetEmailCC())
                    {
                        if (!lstBlockEmail.Any(itemCC.Contains))
                        {
                            if (itemCC.ToLower() != DefaultEmailTo.ToLower())
                            {
                                newMail.Bcc.Add(itemCC);
                            }
                        }
                    }

                    newMail.Subject = Subject;
                    newMail.IsBodyHtml = true;
                    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(template, null, MediaTypeNames.Text.Html);
                    var imageToInline = new LinkedResource(streamBitmap, MediaTypeNames.Image.Jpeg);
                    imageToInline.ContentId = "MyImage";
                    alternateView.LinkedResources.Add(imageToInline);
                    newMail.AlternateViews.Add(alternateView);

                    client.Send(newMail);
                    client.Dispose();
                    newMail = null;
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                string ErrMess = ex.Message;
            }
        }


        public static string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", string.Empty); sbText.Replace(" ", string.Empty);
            return sbText.ToString();
        }

        public List<ListCounter> sendEmailToken(int curentUser)
        {
            string Token = "";
            string TokenDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt");
            List<ListCounter> dataCounterToken = new List<ListCounter>();
            // send email to approval 
            try
            {
                #region get email to
                var dataApproval = (from a in _db.Users
                                    where a.Status == true && a.Id == curentUser
                                    select new
                                    {
                                        Name = a.FirstName + " " + a.LastName,
                                        email = a.Email
                                    }).FirstOrDefault();
                #endregion

                var temp = (from b in _db.TemplateNotifEmails
                            where b.tpl_ProcessNameCode == "Release" && b.tpl_ModuleCode == "Purchase"
                            select b).FirstOrDefault();

                string toname = dataApproval.Name;
                string email_to = dataApproval.email;
                //string email_to = "maxicat.mail@gmail.com";
                //string Code = item.Code;
                Token = RandomString();
                string Subject = temp.tpl_Subject;
                Subject = Subject.Replace("@Date@", TokenDate);

                var message = new string[] { "OK", "0" };

                try
                {
                    string template = temp.tpl_Template;
                    template = template.Replace("@approvalname@", toname);
                    template = template.Replace("@Date@", TokenDate);
                    template = template.Replace("@Token@", Token);

                    sendEmail(email_to, "", "", template, Subject);
                }
                catch (Exception e)
                {
                    message = new string[] { "Error", e.Message };
                }

                dataCounterToken.Add(new ListCounter()
                {
                    ReleaseDate = TokenDate,
                    Token = Token
                });
            }
            catch (Exception ex)
            {
                string ErrMess = ex.Message;
                Token = "";
            }

            return dataCounterToken;
        }

        public static TemplateNotifEmail getEmailTemplate(string processName, string modulCode)
        {
            TemplateNotifEmail objTemplate = new TemplateNotifEmail();

            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                objTemplate = db.TemplateNotifEmails.Where(x => x.tpl_ProcessNameCode == processName
                && x.tpl_ModuleCode == modulCode
                && x.tpl_IsEnabled == 1).FirstOrDefault();
            }
          
            objTemplate.tpl_Template = objTemplate.tpl_Template.Replace(System.Environment.NewLine, string.Empty);
            objTemplate.tpl_Subject = objTemplate.tpl_Subject.Replace(System.Environment.NewLine, string.Empty);
            objTemplate.tpl_Template = objTemplate.tpl_Template.Replace("@Logo@", BaseConstants.BASE64_ICAT_LOGO);
            return objTemplate;
        }

        public static string userEmail(int id)
        {
            string email = "@email.com";
            if (true)
            {
                using (DB_POICATEntities db = new DB_POICATEntities())
                {
                    email = db.Users.Where(x => x.Status == true && x.Id == id).Select(x => x.Email).FirstOrDefault();
                }
            }
            return email;
        }

        public class ListCounter
        {
            public string ReleaseDate { get; set; }
            public string Token { get; set; }
        }



    }
}