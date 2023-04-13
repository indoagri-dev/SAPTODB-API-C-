using Indoagri.IndoagriDB217.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriDB217.Model.Repositories
{
    public class PurchaseRequisitionRepositories
    {
        private DB_ICAT3_217Entities _db;
        public PurchaseRequisitionRepositories()
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

        public IQueryable<PurchaseRequisition> FindAll()
        {
            return from a in _db.PurchaseRequisitions select a;
        }

        public void Insert(PurchaseRequisition po)
        {
            _db.PurchaseRequisitions.Add(po);
        }

        public void Update(PurchaseRequisition pr)
        {
            var hkData = (from a in _db.PurchaseRequisitions where a.BANFN == pr.BANFN && a.BNFPO == pr.BNFPO select a).Any();
            if (hkData)
            {
                _db.PurchaseRequisitions.Attach(pr);
                _db.Entry(pr).State = System.Data.Entity.EntityState.Modified;
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
