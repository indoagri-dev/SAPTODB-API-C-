using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Repositories
{
    public class ZTM_Order_PORepository
    {
          private INDOAGRI_DATAEntities _db;
         public ZTM_Order_PORepository()
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

        public IQueryable<ZTM_ORDER_PO> FindAll() 
        {
            return from a in _db.ZTM_ORDER_PO select a;
        }

        public void Insert(ZTM_ORDER_PO po)
        {
            _db.ZTM_ORDER_PO.Add(po);
        }

        public void Update(ZTM_ORDER_PO item)
        {
            var asset = (from a in _db.ZTM_ORDER_PO
                         where
                             a.EBELN == item.EBELN &&
                             a.EBELP == item.EBELP &&
                             a.MBLNR == item.MBLNR
                         select a).Any();
            if (asset)
            {
                _db.ZTM_ORDER_PO.Attach(item);
                _db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
        }
        public void Delete(ZTM_ORDER_PO item)
        {
            var asset = (from a in _db.ZTM_ORDER_PO
                         where
                             a.EBELN == item.EBELN &&
                             a.EBELP == item.EBELP &&
                             a.MBLNR == item.MBLNR
                         select a).Any();
            if (asset)
            {
                _db.ZTM_ORDER_PO.Remove(item);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
