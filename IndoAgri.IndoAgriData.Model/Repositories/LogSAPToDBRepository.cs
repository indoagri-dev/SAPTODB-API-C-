using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
   public class LogSAPToDBRepository
    {

          private INDOAGRI_DATAEntities _db;
          public LogSAPToDBRepository()
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

          public void Insert(LogSAPToDB logSAPToDB)
        {
            _db.LogSAPToDBs.Add(logSAPToDB);
        }

          public void Update(LogSAPToDB logSAPToDB)
          {
            var asset = (from a in _db.LogSAPToDBs where a.LogId == logSAPToDB.LogId select a).Any();
            if (asset)
            {
                _db.LogSAPToDBs.Attach(logSAPToDB);
                _db.Entry(logSAPToDB).State = System.Data.Entity.EntityState.Modified;
            }
          }
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
