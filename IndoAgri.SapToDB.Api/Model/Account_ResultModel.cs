using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMobileAPI.Models
{
    public class Account_ResultModel
    {
        public int? userId { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string userDomain { get; set; }
        public string userIP { get; set; }
        public bool LoggedIn { get; set; }
        public Nullable<byte> userActive { get; set; }
        public Nullable<DateTime> createdAt { get; set; }
        public Nullable<DateTime> updatedAt { get; set; }
        public Account_UsersModel userDetail { get; set; }
        public List<Account_UserPermissionModel> permissionApps { get; set; }
        public Account_UserPermissionModel hotspotApps { get; set; }
    }
}