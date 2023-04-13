using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriIACData.Model.Repositories
{
    public class ZTUAGRI_PBS2Repo_Prod
    {
     private INDOAGRI_DATAEntities _db;
     public ZTUAGRI_PBS2Repo_Prod()
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

        public IQueryable<ZTUAGRI_PBS2> FindAll()
        {
            return from a in _db.ZTUAGRI_PBS2 select a;
        }

        public void Insert(ZTUAGRI_PBS2 cdv)
        {
            _db.ZTUAGRI_PBS2.Add(cdv);
        }

        public void Update(ZTUAGRI_PBS2 hk)
        {
            var hkData = (from a in _db.ZTUAGRI_PBS2 where a.SPBSNUM == hk.SPBSNUM && a.ZYEAR == hk.ZYEAR select a).Any();
            if (hkData)
            {
                _db.ZTUAGRI_PBS2.Attach(hk);
                _db.Entry(hk).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(ZTUAGRI_PBS2 item)
        {
            var asset = (from a in _db.ZTUAGRI_PBS2
                         where
                             a.ZYEAR == item.ZYEAR &&
                             a.SPBSNUM == item.SPBSNUM &&
                             a.COMPANY_CODE == item.COMPANY_CODE &&
                             a.ESTATE == item.ESTATE &&
                             a.LINENUM == item.LINENUM
                         select a).Any();
            if (asset)
            {
                _db.ZTUAGRI_PBS2.Remove(item);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
