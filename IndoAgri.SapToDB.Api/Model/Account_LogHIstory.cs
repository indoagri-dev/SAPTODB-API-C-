using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace IMobileAPI.Models
{
     [Table("Log_SheetsMobile")]
    public class Account_LogSheetsMobile
    {
        [Key]
        public int ID { get; set; }
        public string USERS { get; set; }
        public string APPS { get; set; }
        public string CONTROLLERACTION { get; set; }
        public string MESSAGE { get; set; }
        public string JSONRESULT { get; set; }
        public string STATUS { get; set; }
        public DateTime CREATEDDATE { get; set; }
    }

}