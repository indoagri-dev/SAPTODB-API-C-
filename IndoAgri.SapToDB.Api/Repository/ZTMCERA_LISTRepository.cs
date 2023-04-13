using IndoAgri.SapToDB.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class ZTMCERA_LISTRepository
    {
        private DB_ICATEntities _db;
           public ZTMCERA_LISTRepository()
        {
            try
            {
                string connectionEncrypt = System.Configuration.ConfigurationManager.ConnectionStrings["DB_ICATEntities"].ConnectionString;
                _db = new DB_ICATEntities();
            }
            catch (Exception)
            {
                _db = new DB_ICATEntities();
            }
        }

        public IQueryable<ZTM_CERA_LIST> FindAll() 
        {
            return from a in _db.ZTM_CERA_LIST select a;
        }

        public void Insert(ZTM_CERA_LIST cera)
        {
            _db.ZTM_CERA_LIST.Add(cera);
        }

        public void Update(ZTM_CERA_LIST cera)
        {
            var asset = (from a in _db.ZTM_CERA_LIST
                         where 
                            a.MANDT == cera.MANDT
                            && a.BUKRS == cera.BUKRS
                             && a.WERKS == cera.WERKS
                             && a.GJAHR == cera.GJAHR
                             && a.POSNR == cera.POSNR
                             && a.POSID == cera.POSID
                             && a.DOCNUM == cera.DOCNUM
                         select a).Any();
            if (asset)
            {
                _db.ZTM_CERA_LIST.Attach(cera);
                _db.Entry(cera).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
