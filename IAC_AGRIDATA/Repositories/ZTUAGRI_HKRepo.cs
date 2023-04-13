using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAC_AGRIDATA.Repositories
{
    public class ZTUAGRI_HKRepo
    {
        private IAC_AGRIDATAEntities _db;
        public ZTUAGRI_HKRepo()
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

        public IQueryable<ZTUAGRI_HK> FindAll()
        {
            return from a in _db.ZTUAGRI_HK select a;
        }

        public void Insert(ZTUAGRI_HK cdv)
        {
            _db.ZTUAGRI_HK.Add(cdv);
        }

        public void Update(ZTUAGRI_HK hk)
        {
            var hkData = (from a in _db.ZTUAGRI_HK where a.BKMNUM == hk.BKMNUM && a.ZYEAR == hk.ZYEAR select a).Any();
            if (hkData)
            {
                _db.ZTUAGRI_HK.Attach(hk);
                _db.Entry(hk).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(ZTUAGRI_HK item)
        {
            var asset = (from a in _db.ZTUAGRI_HK
                         where
                             a.BKMNUM == item.BKMNUM &&
                             a.ZYEAR == item.ZYEAR
                         select a).Any();
            if (asset)
            {
                _db.ZTUAGRI_HK.Remove(item);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
