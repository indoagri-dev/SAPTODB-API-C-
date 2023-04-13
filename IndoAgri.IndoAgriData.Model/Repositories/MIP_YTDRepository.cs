using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class MIP_YTDRepository
    {
         private INDOAGRI_DATAEntities _db;
         public MIP_YTDRepository()
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

        public IQueryable<ZTM_YTDTPURC_MIP_TEMP> FindAll() 
        {
            return from a in _db.ZTM_YTDTPURC_MIP_TEMP select a;
        }

        public void Insert(ZTM_YTDTPURC_MIP_TEMP assetStat)
        {
            _db.ZTM_YTDTPURC_MIP_TEMP.Add(assetStat);
        }

        public void Update(ZTM_YTDTPURC_MIP_TEMP assetStat)
        {
            var asset = (from a in _db.ZTM_YTDTPURC_MIP_TEMP where a.ID == assetStat.ID select a).Any();
            if (asset)
            {
                _db.ZTM_YTDTPURC_MIP_TEMP.Attach(assetStat);
                _db.Entry(assetStat).State = System.Data.Entity.EntityState.Modified;
            }

           
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
