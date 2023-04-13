using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriIACData.Model.Repositories
{
    public class ZTUAGRI_HKRepo_Prod
    {
     private INDOAGRI_DATAEntities _db;
     public ZTUAGRI_HKRepo_Prod()
        {
            try
            {
                string connectionEncrypt = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                string connectionString = Md5Config.Decrypt(connectionEncrypt, "IndoAgri", true);
                _db = new INDOAGRI_DATAEntities(connectionString);
            }
            catch (Exception)
            {
                _db = new INDOAGRI_DATAEntities();
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
                             a.ZYEAR == item.ZYEAR &&
                             a.COMPANY_CODE == item.COMPANY_CODE &&
                             a.ESTATE == item.ESTATE &&
                             a.LINENUM == item.LINENUM && 
                             a.DIVISI == item.DIVISI
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
