//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IndoagriDB217
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseOrderDetail
    {
        public System.Guid ID { get; set; }
        public System.Guid POHeader_ID { get; set; }
        public string PO_Number { get; set; }
        public string POItem { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string VendorCode { get; set; }
        public int PurchaseID { get; set; }
        public Nullable<int> PurchaseDetailID { get; set; }
        public string WERKS { get; set; }
        public Nullable<bool> CapexOpex { get; set; }
        public string GoodsType { get; set; }
        public string PSPNR_WBS { get; set; }
        public string POSID_WBS { get; set; }
        public string BUKRS { get; set; }
        public string ANLN1_Asset { get; set; }
        public string KOKRS_CostCenter { get; set; }
        public string KOSTL_CostCenter { get; set; }
        public string KTOPL_GLAcc { get; set; }
        public string SAKNR_GLAcc { get; set; }
        public decimal Qty { get; set; }
        public Nullable<decimal> OutsQty { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string Remark { get; set; }
        public string CashflowCode { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<decimal> DiscountQty { get; set; }
        public Nullable<int> UserId { get; set; }
        public string deliveryAddrWERKS { get; set; }
        public string deliveryAddrADDRNUMBER { get; set; }
        public string emailDeliveryAddress { get; set; }
        public string invoiceAddrWERKS { get; set; }
        public string invoiceAddrADDRNUMBER { get; set; }
        public string invoiceAddrLGORT { get; set; }
        public string UOM { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string Status { get; set; }
        public Nullable<int> DOStatusId { get; set; }
        public string DOStatus { get; set; }
        public string isClose { get; set; }
        public string isReject { get; set; }
        public string isDO { get; set; }
        public string isGR { get; set; }
        public string isINV { get; set; }
        public string Reject_Comment { get; set; }
        public string Closed_Comment { get; set; }
        public string BANFN { get; set; }
        public string BNFPO { get; set; }
        public string PRNo { get; set; }
        public string PRItem { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> ProcessDate { get; set; }
        public Nullable<System.DateTime> RejectDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    }
}
