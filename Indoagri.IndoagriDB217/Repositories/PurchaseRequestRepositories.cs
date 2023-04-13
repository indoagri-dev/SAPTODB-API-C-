using Indoagri.IndoagriDB217.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriICAT.Model.Repositories
{
    public class PurchaseRequestRepositories
    {
        private DB_ICAT217Entities _db;
        public PurchaseRequestRepositories()
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

        public IQueryable<PurchaseRequest> FindAll()
        {
            return from a in _db.PurchaseRequests select a;
        }

        public void Insert(PURCHASEORDER_HEADER_MAPING po)
        {
            var t = new PurchaseRequest()
            {

            };
            _db.PurchaseRequests.Add(t);
        }
        public int ToInsert(PURCHASEORDER_HEADER_MAPING po)
        {
            var t = new PurchaseRequest()
            {

            };

            _db.PurchaseRequests.Add(t);
            _db.SaveChanges();
            return t.ID;
        }

        public void Update(PurchaseRequest pr)
        {
            var hkData = (from a in _db.PurchaseRequests where a.ID == pr.ID && a.Code == pr.Code select a).Any();
            if (hkData)
            {
                _db.PurchaseRequests.Attach(pr);
                _db.Entry(pr).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(PurchaseRequest item)
        {
            var asset = (from a in _db.PurchaseRequests
                         where
                             a.ID == item.ID &&
                             a.Code == item.Code
                         select a).Any();
            if (asset)
            {
                _db.PurchaseRequests.Remove(item);
            }
        }

        public void DeleteParam(PurchaseRequest item)
        {
            var asset = (from a in _db.PurchaseRequests
                         where
                               a.ID == item.ID &&
                             a.Code == item.Code
                         select a).Any();
            if (asset)
            {
                _db.PurchaseRequests.Remove(item);
                //_db.SaveChanges();
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }



        public object ToInsert(List<PURCHASEORDER_HEADER_MAPING> param)
        {
            throw new NotImplementedException();
        }
    }
}
