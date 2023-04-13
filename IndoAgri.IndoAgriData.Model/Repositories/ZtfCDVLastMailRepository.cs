using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class ZtfCDVLastMailRepository
    {
           private INDOAGRI_DATAEntities _db;
           public ZtfCDVLastMailRepository()
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

        public IQueryable<ZTF_CDV_LASTMAIL> FindAll() 
        {
            return from a in _db.ZTF_CDV_LASTMAIL select a;
        }

        public void Insert(ZTF_CDV_LASTMAIL cdv)
        {
            _db.ZTF_CDV_LASTMAIL.Add(cdv);
        }

        public void Update(ZTF_CDV_LASTMAIL cdv)
        {
            var asset = (from a in _db.ZTF_CDV_LASTMAIL where a.CDVId == cdv.CDVId select a).Any();
            if (asset)
            {
                _db.ZTF_CDV_LASTMAIL.Attach(cdv);
                _db.Entry(cdv).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}