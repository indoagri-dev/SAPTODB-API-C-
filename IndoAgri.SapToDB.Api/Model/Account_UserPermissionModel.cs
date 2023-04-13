using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMobileAPI.Models
{
    [Table("ViewUserPermission")]
    public class Account_UserPermissionModel
    {
        [Key]
        public int ID { get; set; }
        public string AppName { get; set; }
        public string AppDescription { get; set; }
        public string AppPackage { get; set; }
        public string AppPackageClass { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
        public string ApkUrl { get; set; }
        public string AssetsIcon { get; set; }
        public string AssetsIconUrl { get; set; }
        public int Active { get; set; }
        public string GroupLevel { get; set; }
        public string UserAD { get; set; }
        public string Kode_Group { get; set; }
        public string UpdateDescription { get; set; }
        public string ApproverSAPServer { get; set; }
    }
}