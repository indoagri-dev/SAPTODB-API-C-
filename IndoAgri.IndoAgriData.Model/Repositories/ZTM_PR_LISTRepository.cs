using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class ZTM_PR_LISTRepository
    {
         private INDOAGRI_DATAEntities _db;
         public ZTM_PR_LISTRepository()
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

        public IQueryable<ZTM_PR_LIST> FindAll() 
        {
            return from a in _db.ZTM_PR_LIST select a;
        }

        public void Insert(ZTM_PR_LIST cdv)
        {
            _db.ZTM_PR_LIST.Add(cdv);
        }

        public void Update(ZTM_PR_LIST item)
        {
            var asset = (from a in _db.ZTM_PR_LIST
                         where
                             a.BUKRS == item.BUKRS &&
                             a.WERKS == item.WERKS &&
                             a.BANFN == item.BANFN &&
                             a.BNFPO == item.BNFPO &&
                             a.EBELN == item.EBELN &&
                             a.EBELP == item.EBELP &&
                             a.EKGRP == item.EKGRP &&
                             a.EKGRP_PO == item.EKGRP_PO
                         select a).Any();
            if (asset)
            {
                _db.ZTM_PR_LIST.Attach(item);
                _db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
        }
        public void Delete(ZTM_PR_LIST item)
        {
            var asset = (from a in _db.ZTM_PR_LIST
                         where
                             a.BUKRS == item.BUKRS &&
                             a.WERKS == item.WERKS &&
                             a.BANFN == item.BANFN &&
                             a.BNFPO == item.BNFPO
                         select a).Any();
            if (asset)
            {
                _db.ZTM_PR_LIST.Remove(item);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    
    }
}
