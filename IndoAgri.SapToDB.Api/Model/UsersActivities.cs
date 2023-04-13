using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMobileAPI.Models
{
      [Table("View_UsersActivity")]
    public class UsersActivities
    {
            [Key]
          public string TokenFireBase { get; set; }
            public DateTime NotifyDate { get; set; }
          public string Package { get; set; }
          public string UserCode { get; set; }
            public string UniqueCode { get; set; }
            public string UserID { get; set; }
            public string userName { get; set; }
            public string userSurname { get; set; }
            public string userEmail { get; set; }
            public string EmailLDAP{ get; set; }
            public string Domain { get; set; }
            public string userDescription { get; set; }
            public string Company { get; set; }
            public string userProvince { get; set; }
            public string Department { get; set; }
            public string EmployeeID { get; set; }
            public string Fullname { get; set; }
            public string DeviceID { get; set; }
            public string userLevel { get; set; }
            public string userSAP { get; set; }
            public Nullable<DateTime> updatedAt { get; set; }
            public string myIP { get; set; }
            public bool LoggedIn { get; set; }
            public string KodeAreaHS { get; set; }
            public string KodeGroupHS { get; set; }
    }
}