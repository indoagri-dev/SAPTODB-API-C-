using Indoagri.IndoagriDB217.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriDB217.Model.Repositories
{
    public class PurchaseOrderHeaderRepositories
    {
        private DB_ICAT3_217Entities _db;
        public PurchaseOrderHeaderRepositories()
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

        public IQueryable<PurchaseOrderHeader> FindAll()
        {
            return from a in _db.PurchaseOrderHeaders select a;
        }

        public void Insert(PURCHASEORDER_HEADER_MAPING po, int ID)
        {
            bool ISPKP = false;
            if (po.ISPKP== "1")
            {
                ISPKP = true;
            }
            else
            {
                ISPKP = false;
            }
            var t = new PurchaseOrderHeader//Make sure you have a table called test in DB
            {
                PO_Number = po.EBELN,
                PO_Type = po.BSART,
                VendorCode = po.LIFNR,
                IsPKP = ISPKP,
                PurchaseID = ID,
                Status = "New Order",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            _db.PurchaseOrderHeaders.Add(t);
        }

        public void Update(PurchaseOrderHeader po)
        {
            var hkData = (from a in _db.PurchaseOrderHeaders where a.PO_Number == po.PO_Number select a).Any();
            if (hkData)
            {
                _db.PurchaseOrderHeaders.Attach(po);
                _db.Entry(po).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(PurchaseOrderHeader item)
        {
            var asset = (from a in _db.PurchaseOrderHeaders
                         where
                             a.PO_Number == item.PO_Number
                         select a).Any();
            if (asset)
            {
                _db.PurchaseOrderHeaders.Remove(item);
            }
        }

        public void DeleteParam(PurchaseOrderHeader item)
        {
            var asset = (from a in _db.PurchaseOrderHeaders
                         where
                             a.PO_Number == item.PO_Number 
                         select a).Any();
            if (asset)
            {
                _db.PurchaseOrderHeaders.Remove(item);
                //_db.SaveChanges();
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
