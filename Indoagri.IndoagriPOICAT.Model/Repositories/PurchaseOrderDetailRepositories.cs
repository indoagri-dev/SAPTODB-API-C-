using Indoagri.IndoagriPOICAT.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriPOICAT.Model.Repositories
{
    public class PurchaseOrderDetailRepositories
    {
        private DB_POICATEntities _db;
        public PurchaseOrderDetailRepositories()
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

        public IQueryable<PurchaseOrderDetail> FindAll()
        {
            return from a in _db.PurchaseOrderDetails select a;
        }

        public void Insert(PURCHASEORDER_DETAIL_MAPING po,int requestid)
        {
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
            string itemsak = null;
            if (po.SAKNR != null && po.SAKTO == null)
            {
                itemsak = po.SAKNR;
            }
            if (po.SAKTO != null && po.SAKNR == null)
            {
                itemsak = po.SAKTO;
            }
            var HeaderPO = _db.PurchaseOrderHeaders.Where(a =>
                                  a.PO_Number == po.EBELN).FirstOrDefault();
             
                string code = po.MATNR;
                //string productcode = code.Substring(code.Length - 15);
                var crid = System.Guid.NewGuid();
                var DetailRequest = _db.PurchaseRequestDetails.Where(a =>
                                   a.prd_PurchaseID == requestid && a.prd_ProductCode == code).FirstOrDefault();
            var t = new PurchaseOrderDetail()
            {
                ID = crid,
                POHeader_ID = HeaderPO.ID,
                PO_Number = po.EBELN,
                POItem = po.EBELP,
                ProductCode = code,
                ProductName = po.TXZ01,
                VendorCode = HeaderPO.VendorCode,
                PurchaseID = requestid,
                PurchaseDetailID = DetailRequest.prd_ID,
                WERKS = po.WERKS,
                CapexOpex = false,
                GoodsType = po.GOODSTYPE,
                PSPNR_WBS = po.PSPSPPNR,
                POSID_WBS = po.POSID,
                BUKRS = po.BUKRS,
                ANLN1_Asset = po.ANLN1,
                KOKRS_CostCenter = "0010",
                KOSTL_CostCenter = po.KOSTL,
                KTOPL_GLAcc = "ISM",
                SAKNR_GLAcc = itemsak,
                Qty  = menge,
                OutsQty = menge,
                DeliveryDate = delivery,
                Remark = null,
                CashflowCode = null,
                Currency = po.WAERS,
                Price = price,
                SalePrice = netpr,
                DiscountQty = discpercent,
                UserId = 1,
                deliveryAddrWERKS = po.WERKS,
                deliveryAddrADDRNUMBER = po.ADRNRPO,
                emailDeliveryAddress = null,
                invoiceAddrWERKS = po.WERKS,
                invoiceAddrADDRNUMBER = po.ADRNRINV,
                invoiceAddrLGORT = "OF02",
                UOM = po.MEINS,
                StatusId = 1,
                Status = "New Order",
                DOStatusId = null,
                DOStatus = null,
                isClose = null,
                isReject = null,
                isDO = null,
                isGR = null,
                isINV = null,
                Reject_Comment = null,
                BANFN = po.BANFN,
                BNFPO = po.BNFPO,
                PRNo  = po.BANFN,
                PRItem = po.BNFPO,
                ProcessDate = null,
                RejectDate = null,
                CreatedBy = "1",
                CreatedOn = DateTime.Now,
                ModifiedBy = "1",
                ModifiedOn = DateTime.Now,
                Closed_Comment = null
            };
            _db.PurchaseOrderDetails.Add(t);
        }

        public void Update(PurchaseOrderDetail po)
        {
            var hkData = (from a in _db.PurchaseOrderDetails where a.PO_Number == po.PO_Number select a).Any();
            if (hkData)
            {
                _db.PurchaseOrderDetails.Attach(po);
                _db.Entry(po).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(PurchaseOrderDetail item)
        {
            var asset = (from a in _db.PurchaseOrderDetails
                         where
                             a.PO_Number == item.PO_Number
                         select a).Any();
            if (asset)
            {
                _db.PurchaseOrderDetails.Remove(item);
            }
        }

        public void DeleteParam(PurchaseOrderDetail item)
        {
            var asset = (from a in _db.PurchaseOrderDetails
                         where
                             a.PO_Number == item.PO_Number 
                         select a).Any();
            if (asset)
            {
                _db.PurchaseOrderDetails.Remove(item);
                //_db.SaveChanges();
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
