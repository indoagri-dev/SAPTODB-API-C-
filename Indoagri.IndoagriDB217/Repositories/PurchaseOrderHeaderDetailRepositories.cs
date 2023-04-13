using Indoagri.IndoagriDB217.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriDB217.Model.Repositories
{
    public class PurchaseOrderDetailRepositories
    {
        private DB_ICAT3_217Entities _db;
        public PurchaseOrderDetailRepositories()
        {
            try
            {
                _db = new DB_ICAT3_217Entities();
            }
            catch (Exception)
            {
                _db = new DB_ICAT3_217Entities();
            }
        }

        public IQueryable<PurchaseOrderDetail> FindAll()
        {
            return from a in _db.PurchaseOrderDetails select a;
        }

        public void Insert(PURCHASEORDER_DETAIL_MAPING po)
        {

            var t = new PurchaseOrderDetail()
            {

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
