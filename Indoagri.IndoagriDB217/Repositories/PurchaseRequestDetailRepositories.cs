using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriDB217.Model.Repositories
{
    public class PurchaseRequestDetailRepositories
    {
        private DB_ICAT217Entities _db;
        public PurchaseRequestDetailRepositories()
        {
            try
            {
                _db = new DB_ICAT217Entities();
            }
            catch (Exception)
            {
                _db = new DB_ICAT217Entities();
            }
        }

        public IQueryable<PurchaseRequestDetail> FindAll()
        {
            return from a in _db.PurchaseRequestDetails select a;
        }

        public void Insert(PurchaseRequestDetail po)
        {
            _db.PurchaseRequestDetails.Add(po);
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
