using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class ZTPAGRI_MILLDATARepository
    {
           private INDOAGRI_DATAEntities _db;
           public ZTPAGRI_MILLDATARepository()
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

        public IQueryable<ZTPAGRI_MILLDATA> FindAll() 
        {
            return from a in _db.ZTPAGRI_MILLDATA select a;
        }

        public void Insert(ZTPAGRI_MILLDATA cdv)
        {
            _db.ZTPAGRI_MILLDATA.Add(cdv);
        }

        public void Update(ZTPAGRI_MILLDATA milldata)
        {
            var asset = (from a in _db.ZTPAGRI_MILLDATA where a.WERKS == milldata.WERKS && a.GJAHR == milldata.GJAHR && a.MONAT == milldata.MONAT && a.PRD_DATE == milldata.PRD_DATE select a).Any();
            if (asset)
            {
                _db.ZTPAGRI_MILLDATA.Attach(milldata);
                _db.Entry(milldata).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    
    }
}
