using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriIACData.Model.Repositories
{
    public class ZC0032_Prod
    {
     private INDOAGRI_DATAEntities _db;
     public ZC0032_Prod()
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
     public IQueryable<ZC0032> FindAll()
        {
            return from a in _db.ZC0032 select a;
        }

     public void Insert(ZC0032 cdv)
        {
            _db.ZC0032.Add(cdv);
        }

     public void Update(ZC0032 hk)
        {
            var hkData = (from a in _db.ZC0032 where 
                              a.MANDT == hk.MANDT 
                              && a.GJAHR== hk.GJAHR
                               && a.MONAT == hk.MONAT
                                && a.WERKS == hk.WERKS
                                 && a.TYPE  == hk.TYPE
                                  && a.ITEM == hk.ITEM
                                   && a.ITEMTEXT == hk.ITEMTEXT
                          select a).Any();
            if (hkData)
            {
                _db.ZC0032.Attach(hk);
                _db.Entry(hk).State = System.Data.Entity.EntityState.Modified;
            }
        }

     public void Delete(ZC0032 hk)
        {
            var asset = (from a in _db.ZC0032
                         where
                               a.MANDT == hk.MANDT
                              && a.GJAHR == hk.GJAHR
                               && a.MONAT == hk.MONAT
                                && a.WERKS == hk.WERKS
                                 && a.TYPE == hk.TYPE
                                  && a.ITEM == hk.ITEM
                                   && a.ITEMTEXT == hk.ITEMTEXT
                         select a).Any();
            if (asset)
            {
                _db.ZC0032.Remove(hk);
            }
        }

     public void DeleteParam(ZC0032 hk)
     {
         var asset = (from a in _db.ZC0032
                      where
                        a.MANDT == hk.MANDT
                              && a.GJAHR == hk.GJAHR
                               && a.MONAT == hk.MONAT
                                && a.WERKS == hk.WERKS
                                 && a.TYPE == hk.TYPE
                                  && a.ITEM == hk.ITEM
                                   && a.ITEMTEXT == hk.ITEMTEXT
                      select a).Any();
         if (asset)
         {
             _db.ZC0032.Remove(hk);
         }
     }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
