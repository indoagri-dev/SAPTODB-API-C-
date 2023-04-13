using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class ZTPAGRI_MILLPRODRepository
    {
          private INDOAGRI_DATAEntities _db;
           public ZTPAGRI_MILLPRODRepository()
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

        public IQueryable<ZTPAGRI_MILLPROD> FindAll() 
        {
            return from a in _db.ZTPAGRI_MILLPROD select a;
        }

        public void Insert(ZTPAGRI_MILLPROD cdv)
        {
            _db.ZTPAGRI_MILLPROD.Add(cdv);
        }

        public void Update(ZTPAGRI_MILLPROD millprod)
        {
            var asset = (from a in _db.ZTPAGRI_MILLPROD where a.WERKS == millprod.WERKS && a.GJAHR == millprod.GJAHR && a.MONAT == millprod.MONAT && a.AUFNR == millprod.AUFNR select a).Any();
            if (asset)
            {
                _db.ZTPAGRI_MILLPROD.Attach(millprod);
                _db.Entry(millprod).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    
    }
}
