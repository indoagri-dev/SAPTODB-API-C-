using IndoAgri.SapToDB.Api.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IMobileAPI.Models
{
    public class Context_MobileAgri : DbContext
    {
        public Context_MobileAgri() : base("MobileAgriContext") { }

        public virtual  DbSet<Account_UsersModel> Users { get; set; }
        public DbSet<Account_UserPermissionModel> ModelPermitted { get; set; }
        public DbSet<Account_LogSheetsMobile> UserLogSheetsMobile { get; set; }
        public DbSet<Account_UsersNotification> UserNotification { get; set; }
        public DbSet<UsersActivities> UserActivities { get; set; }

        // LOG SAP //
        public DbSet<Log_ZustJobBatch> LogSAPZUSTJOBBATCH { get; set; }
    }
}