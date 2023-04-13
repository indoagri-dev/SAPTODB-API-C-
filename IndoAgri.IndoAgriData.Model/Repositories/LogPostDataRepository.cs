using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class LogPostDataRepository
    {
        private INDOAGRI_DATAEntities _db;
        public LogPostDataRepository()
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

        public void Insert(LogPostData logApproval)
        {
            _db.LogPostDatas.Add(logApproval);
        }

        public void Update(LogPostData logApproval)
        {
            var asset = (from a in _db.LogPostDatas where a.LogId == logApproval.LogId select a).Any();
            if (asset)
            {
                _db.LogPostDatas.Attach(logApproval);
                _db.Entry(logApproval).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        
    }
}