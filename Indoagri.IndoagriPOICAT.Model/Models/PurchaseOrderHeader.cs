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
    
    public partial class PurchaseOrderHeader
    {
        public System.Guid ID { get; set; }
        public string PO_Number { get; set; }
        public string PO_Type { get; set; }
        public Nullable<int> PurchaseID { get; set; }
        public string VendorCode { get; set; }
        public string ActionWERKS { get; set; }
        public string WERKS { get; set; }
        public string Area_ID { get; set; }
        public string DeliveryAddWERKS { get; set; }
        public string DeliveryAddrNUMBER { get; set; }
        public string InvoiceAddrWERKS { get; set; }
        public string InvoiceAddrNUMBER { get; set; }
        public string UPDeliveryAddress { get; set; }
        public string Incoterm { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public Nullable<bool> IsPKP { get; set; }
        public Nullable<int> UserOrderId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string EKGRP { get; set; }
        public string Email { get; set; }
        public Nullable<int> EmailCounter { get; set; }
    }
}
