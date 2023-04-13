using Indoagri.IndoagriIACData.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriIACData.Model.Repositories
{
    public class ZTUAGRI_PRCALCHVRepo
    {
        private IAC_AGRIDATAEntities _db;
        public ZTUAGRI_PRCALCHVRepo()
        {
            try
            {
                _db = new IAC_AGRIDATAEntities();
            }
            catch (Exception)
            {
                _db = new IAC_AGRIDATAEntities();
            }
        }

        public IQueryable<ZTU_PRCALCHV> FindAll()
        {
            return from a in _db.ZTU_PRCALCHV select a;
        }

        public void Insert(ZTU_PRCALCHV cdv)
        {
            _db.ZTU_PRCALCHV.Add(cdv);
        }

        public void Update(ZTU_PRCALCHV hk)
        {
            var hkData = (from a in _db.ZTU_PRCALCHV where a.BKMNUM == hk.BKMNUM && a.ZYEAR == hk.ZYEAR select a).Any();
            if (hkData)
            {
                _db.ZTU_PRCALCHV.Attach(hk);
                _db.Entry(hk).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(ZTU_PRCALCHV item)
        {
            var asset = (from a in _db.ZTU_PRCALCHV
                         where
                             a.COMPANY == item.COMPANY &&
                             a.ESTATE == item.ESTATE &&
                             a.ZYEAR == item.ZYEAR &&
                             a.BKMNUM == item.BKMNUM &&
                             a.BKMDATE == item.BKMDATE &&
                             a.DIVISI == item.DIVISI &&
                              a.MDRAN == item.MDRAN &&
                             a.EMPID == item.EMPID &&
                             a.LINENUM == item.LINENUM &&
                             a.BLOCKID == item.BLOCKID &&
                             a.PREMITYP == item.PREMITYP &&
                             a.PRODTYPE == item.PRODTYPE
                         select a).Any();
            if (asset)
            {
                _db.ZTU_PRCALCHV.Remove(item);
            }
        }

        public void DeleteParam(ZTU_PRCALCHV item)
        {
            var asset = (from a in _db.ZTU_PRCALCHV
                         where
                             a.COMPANY == item.COMPANY &&
                             a.ESTATE == item.ESTATE &&
                             a.ZYEAR == item.ZYEAR &&
                             a.BKMNUM == item.BKMNUM
                         select a).Any();
            if (asset)
            {
                _db.ZTU_PRCALCHV.Remove(item);
                //_db.SaveChanges();
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
