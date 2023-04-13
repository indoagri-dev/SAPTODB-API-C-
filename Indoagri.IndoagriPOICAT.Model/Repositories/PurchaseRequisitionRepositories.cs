using Indoagri.IndoagriPOICAT.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriPOICAT.Model.Repositories
{
    public class PurchaseRequisitionRepositories
    {
        private DB_POICATEntities _db;
        public PurchaseRequisitionRepositories()
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

        public IQueryable<PurchaseRequisition> FindAll()
        {
            return from a in _db.PurchaseRequisitions select a;
        }

        public void Insert(PurchaseRequisition po)
        {
            _db.PurchaseRequisitions.Add(po);
        }

        public void Update(PURCHASEORDER_DETAIL_MAPING po, int requestid)
        {
            var porequisition = _db.PurchaseRequisitions.Where(a =>
                                 a.BNFPO == po.EBELN).FirstOrDefault(); 
            var poicatdetail = _db.PurchaseOrderDetails.Where(a =>
                                 a.PurchaseID == requestid).FirstOrDefault(); 
            var hkData = (from a in _db.PurchaseRequisitions where a.BNFPO == po.EBELN && a.BNFPO ==po.EBELN select a).Any();
            if (hkData)
            {
                _db.PurchaseRequisitions.Attach(porequisition);
                _db.Entry(porequisition).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(PurchaseRequisition item)
        {
            var asset = (from a in _db.PurchaseRequisitions
                         where
                            a.BANFN == item.BANFN && a.BNFPO == item.BNFPO
                         select a).Any();
            if (asset)
            {
                _db.PurchaseRequisitions.Remove(item);
            }
        }

        public void DeleteParam(PurchaseRequisition item)
        {
            var asset = (from a in _db.PurchaseRequisitions
                         where
                                 a.BANFN == item.BANFN && a.BNFPO == item.BNFPO
                         select a).Any();
            if (asset)
            {
                _db.PurchaseRequisitions.Remove(item);
                //_db.SaveChanges();
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
