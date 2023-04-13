using Indoagri.IndoagriPOICAT.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriPOICAT.Model.Repositories
{
    public class PurchaseRequestDetailRepositories
    {
        private DB_POICATEntities _db;
        public PurchaseRequestDetailRepositories()
        {
            try
            {
                _db = new DB_POICATEntities();
            }
            catch (Exception)
            {
                _db = new DB_POICATEntities();
            }
        }

        public IQueryable<PurchaseRequestDetail> FindAll()
        {
            return from a in _db.PurchaseRequestDetails select a;
        }

        public void Insert(PURCHASEORDER_DETAIL_MAPING po, int requestid)
        {
            var POHeader = _db.PurchaseOrderHeaders.Where(a =>
                               a.PurchaseID == requestid).FirstOrDefault();
            decimal discpercent = 0;
            decimal menge = 0;
            decimal price = 0;
            decimal netpr = 0;
            decimal freight = 0;
            if (po.MENGE != null)
            {
                menge = decimal.Parse(po.MENGE);
            }
            if (po.PRICE != null)
            {
                price = decimal.Parse(po.PRICE);
            }
            if (po.NETPR != null)
            {
                netpr = decimal.Parse(po.NETPR);
            }
            if (po.DISC != null)
            {
                discpercent = decimal.Parse(po.DiscP);
            }
            if (po.FREIGHT != null)
            {
                freight = decimal.Parse(po.FREIGHT);
            }
            DateTime? delivery = null;
            if (po.EINDT == "0000-00-00")
            {
                delivery = null;
            }
            else
            {
                delivery = DateTime.ParseExact(po.EINDT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            var crid = System.Guid.NewGuid();
            var createdby = Convert.ToInt32(1);
            string code = po.MATNR;
            if (po.MATNR == null)
            {
                code = null;
            }
            else
            {
                code = po.MATNR;
            }
            string itemsak = null;
            if(po.SAKNR !=null && po.SAKTO ==null){
                itemsak = po.SAKNR;
            }
            if (po.SAKTO != null && po.SAKNR == null){
                itemsak = po.SAKTO;
            }
            //string productcode = code.Substring(code.Length - 15);
            var t = new PurchaseRequestDetail()
            {
                prd_ID = 0,
                prd_crID = crid,
                prd_PO_Number = po.EBELN,
                prd_Status = "New Order",
                prd_ProductCode = code,
                prd_ProductName = po.TXZ01,
                prd_VendorCode = POHeader.VendorCode,
                prd_CityID = 1,
                prd_PurchaseID = requestid,
                prd_WERKS = po.WERKS,
                prd_CapexOpex = null,
                prd_GoodsType = po.GOODSTYPE,
                prd_PSPNR_WBS = po.PSPSPPNR,
                prd_POSID_WBS = po.POSID,
                prd_BUKRS = po.BUKRS,
                prd_ANLN1_Asset = po.ANLN1,
                prd_KOKRS_CostCenter = null,
                prd_KOSTL_CostCenter = po.KOSTL,
                prd_KTOPL_GLAcc = "ISM",
                prd_SAKNR_GLAcc = itemsak,
                prd_UOM = po.MEINS,
                prd_Qty = menge,
                prd_DeliveryDate = delivery,
                prd_Remark = null,
                prd_CashflowCode = null,
                prd_Currency = po.WAERS,
                prd_Price = price,
                prd_SalePrice = netpr,
                prd_DiscountQty = discpercent,
                prd_UserId = null,
                deliveryAddrWERKS = po.WERKS,
                deliveryAddrADDRNUMBER = po.ADRNRPO,
                prd_emailDeliveryAddress = null,
                invoiceAddrWERKS = po.WERKS,
                invoiceAddrADDRNUMBER = po.ADRNRINV,
                invoiceAddrLGORT = "OF02",
                ThirdPartyTransporter = null,
                Transporter = po.TRANSPORTER,
                Freight = freight,
                prd_BANFN = po.BANFN,
                prd_BNFPO = po.BNFPO,
                prd_PRNo = po.BANFN,
                prd_PRItem =po.BNFPO,
                prd_OrderNo = "0",
                RejectCommentAMAAME = null,
                CreatedOn = DateTime.Now,
                CreatedBy = createdby,
                ModifiedOn = DateTime.Now,
                ModifiedBy = createdby,


            };
            _db.PurchaseRequestDetails.Add(t);
        }

        public void Update(PurchaseRequestDetail pr)
        {
            var hkData = (from a in _db.PurchaseRequestDetails where a.prd_PO_Number == pr.prd_PO_Number select a).Any();
            if (hkData)
            {
                _db.PurchaseRequestDetails.Attach(pr);
                _db.Entry(pr).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(PurchaseRequestDetail item)
        {
            var asset = (from a in _db.PurchaseRequestDetails
                         where
                             a.prd_PO_Number == item.prd_PO_Number
                         select a).Any();
            if (asset)
            {
                _db.PurchaseRequestDetails.Remove(item);
            }
        }

        public void DeleteParam(PurchaseRequestDetail item)
        {
            var asset = (from a in _db.PurchaseRequestDetails
                         where
                               a.prd_PO_Number == item.prd_PO_Number
                         select a).Any();
            if (asset)
            {
                _db.PurchaseRequestDetails.Remove(item);
                //_db.SaveChanges();
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

       
    }
}
