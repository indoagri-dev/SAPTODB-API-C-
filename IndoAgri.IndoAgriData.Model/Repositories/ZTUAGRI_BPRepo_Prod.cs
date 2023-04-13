using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriIACData.Model.Repositories
{
    public class ZTUAGRI_BPRepo_Prod
    {
     private INDOAGRI_DATAEntities _db;
     public ZTUAGRI_BPRepo_Prod()
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


        public IQueryable<ZTUAGRI_BP> FindAll()
        {
            return from a in _db.ZTUAGRI_BP select a;
        }

        public void Insert(ZTUAGRI_BP cdv)
        {
            _db.ZTUAGRI_BP.Add(cdv);
        }

        public void Update(ZTUAGRI_BP hk)
        {
            var hkData = (from a in _db.ZTUAGRI_BP where a.BKMNUM == hk.BKMNUM && a.ZYEAR == hk.ZYEAR select a).Any();
            if (hkData)
            {
                _db.ZTUAGRI_BP.Attach(hk);
                _db.Entry(hk).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(ZTUAGRI_BP item)
        {
            var asset = (from a in _db.ZTUAGRI_BP
                         where
                             a.ZYEAR == item.ZYEAR &&
                             a.PANENNUM == item.PANENNUM &&
                             a.COMPANY_CODE == item.COMPANY_CODE &&
                             a.ESTATE == item.ESTATE &&
                             a.BKMNUM == item.BKMNUM &&
                             a.LINENUM == item.LINENUM &&
                              a.BPDATE == item.BPDATE &&
                             a.LINENUM_I == item.LINENUM_I
                         select a).Any();
            if (asset)
            {
                _db.ZTUAGRI_BP.Remove(item);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
