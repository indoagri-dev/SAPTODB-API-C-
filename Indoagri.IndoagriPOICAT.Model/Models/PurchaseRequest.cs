//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Indoagri.IndoagriPOICAT.Model.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseRequest
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string WERKS { get; set; }
        public string VendorCode { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> Approval { get; set; }
        public string Status { get; set; }
        public int Stage { get; set; }
        public string Token { get; set; }
        public Nullable<System.DateTime> TokenCreatedOn { get; set; }
        public string ClosedComment { get; set; }
        public string SiteManagerComment { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public Nullable<int> ClosedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    }
}
