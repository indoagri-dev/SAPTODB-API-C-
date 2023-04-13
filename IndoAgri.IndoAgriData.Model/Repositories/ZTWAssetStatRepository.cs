using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndoAgri.Security;
using IndoAgri.IndoAgriData.Model.Models;
using System.Data;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class ZTWAssetStatRepository
    {
        private INDOAGRI_DATAEntities _db;
        public ZTWAssetStatRepository()
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

        public IQueryable<ZTW_ASSETSTAT> FindAll() 
        {
            return from a in _db.ZTW_ASSETSTAT select a;
        }

        public void Insert(ZTW_ASSETSTAT assetStat)
        {
            _db.ZTW_ASSETSTAT.Add(assetStat);
        }

        public void Update(ZTW_ASSETSTAT assetStat)
        {
            var asset = (from a in _db.ZTW_ASSETSTAT where a.AssetStatId == assetStat.AssetStatId select a).Any();
            if (asset)
            {
                _db.ZTW_ASSETSTAT.Attach(assetStat);
                _db.Entry(assetStat).State = System.Data.Entity.EntityState.Modified;
            }

           
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}