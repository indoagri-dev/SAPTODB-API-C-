using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class ZTMAGRI_STOCKPRDRepository
    {
           private INDOAGRI_DATAEntities _db;
           public ZTMAGRI_STOCKPRDRepository()
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

        public IQueryable<ZTMAGRI_STOCKPRD> FindAll() 
        {
            return from a in _db.ZTMAGRI_STOCKPRD select a;
        }

        public void Insert(ZTMAGRI_STOCKPRD cdv)
        {
            _db.ZTMAGRI_STOCKPRD.Add(cdv);
        }

        public void Update(ZTMAGRI_STOCKPRD stock)
        {
            var asset = (from a in _db.ZTMAGRI_STOCKPRD where a.WERKS == stock.BDATJ && a.BDATJ == stock.POPER && a.POPER == stock.BUKRS && a.WERKS == stock.WERKS && a.MATNR == stock.MATNR && a.BWTAR == stock.BWTAR select a).Any();
            if (asset)
            {
                _db.ZTMAGRI_STOCKPRD.Attach(stock);
                _db.Entry(stock).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    
    }
}
