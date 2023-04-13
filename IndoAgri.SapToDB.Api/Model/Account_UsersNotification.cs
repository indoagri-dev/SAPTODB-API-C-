using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMobileAPI.Models
{
       [Table("Users_Notification")]
    public class Account_UsersNotification
    {
        [Key]
        public string UniqueCode { get; set; }
        public string UserCode { get; set; }
        public string UserID { get; set; }
        public string TokenFireBase { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Activate { get; set; }
        public DateTime NotifyDate { get; set; }
        public string Package { get; set; }
        public string IpAddress { get; set; }
    }
}