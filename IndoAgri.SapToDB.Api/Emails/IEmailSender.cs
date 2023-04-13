﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace IndoAgri.SapToDB.Api.Emails
{
    public interface IEmailSender
    {
        void SendEmail(EmailAccount emailAccount, string subject, string body, string fromAddress, IEnumerable<string> toAddresses, IEnumerable<string> replyToAddresses = null, IEnumerable<string> bccAddresses = null, IEnumerable<string> ccAddresses = null, string attachmentFilePath = null, string attachmentFileName = null);

        void SendEmail(EmailAccount emailAccount, string subject, string body, MailAddress fromAddress, IEnumerable<MailAddress> toAddresses, IEnumerable<MailAddress> replyToAddresses, IEnumerable<MailAddress> bccMailAddresses, IEnumerable<MailAddress> ccMailAddresses, string attachmentFilePath = null, string attachmentFileName = null);
    }
}