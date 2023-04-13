using Indoagri.IndoagriPOICAT.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriPOICAT.Model.Common
{
    public class ParamHelper
    {
        public static string SMTPClient { get; set; } 
        public static int SMTPPort { get; set; } 
        public static string DefaultEmailFrom { get; set; }
        public static string DefaultEmailTo { get; set; }
        public static bool ActiveNotification { get; set; }
        private DB_POICATEntities db;

        public static List<string> GetEmailCC()
        {
            List<string> lstResult = new List<string>();
            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                lstResult = (from a in db.ParTables
                             join b in db.ParEntries on a.Id equals b.TabId
                             where a.Sdesc.ToUpper() == "EmailBlock".ToUpper()
                             select b.Vac1.ToLower()).ToList();
            }
            return lstResult;
        }

        public static List<string> GetEmailBlock()
        {
            List<string> lstResult = new List<string>();
            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                lstResult = (from a in db.ParTables
                             join b in db.ParEntries on a.Id equals b.TabId
                             where a.Sdesc.ToUpper() == "EmailBlock".ToUpper()
                             select b.Vac1.ToLower()).ToList();
            }
            return lstResult;
        }

        private static string getSMTPClient()
        {
            string result = "";
            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                result = (from a in db.ParTables
                          join b in db.ParEntries on a.Id equals b.TabId
                          where a.Sdesc.ToUpper() == "NotificationConfiguration".ToUpper()
                          && b.Sdesc.ToUpper() == "SMTPClient".ToUpper()
                          select b.Vac1).FirstOrDefault();
            }
          
            return result;
        }

        private static int getSMTPPort()
        {
            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                int result = (from a in db.ParTables
                              join b in db.ParEntries on a.Id equals b.TabId
                              where a.Sdesc.ToUpper() == "NotificationConfiguration".ToUpper()
                              && b.Sdesc.ToUpper() == "SMTPClient".ToUpper()
                              select b.Van1 ?? 0).FirstOrDefault();
                return result;
            }
        }

        private static string getDefaultEmailFrom()
        {
            string result = "";
            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                result = (from a in db.ParTables
                          join b in db.ParEntries on a.Id equals b.TabId
                          where a.Sdesc.ToUpper() == "NotificationConfiguration".ToUpper()
                          && b.Sdesc.ToUpper() == "DefaultEmailFrom".ToUpper()
                          select b.Vac1.ToLower()).FirstOrDefault();
            }
            return result;
        }
        private static string getDefaultEmailTo()
        {
            string result = "";
            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                result = (from a in db.ParTables
                          join b in db.ParEntries on a.Id equals b.TabId
                          where a.Sdesc.ToUpper() == "NotificationConfiguration".ToUpper()
                          && b.Sdesc.ToUpper() == "DefaultEmailTo".ToUpper()
                          select b.Vac1.ToLower()).FirstOrDefault();
            }
            return result;
        }

        public static Object getParEntries(int tabID, int parEntriesID)
        {
            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                var tempResult = (from a in db.ParEntries where a.TabId == tabID && a.Id == parEntriesID select a).FirstOrDefault();
                return tempResult;
            }
        }

        private static bool getActiveNotification()
        {
            string tempResult = "";
            using (DB_POICATEntities db = new DB_POICATEntities())
            {
                tempResult = (from a in db.ParEntries where a.TabId == 13 && a.Id == 1 select a.Vac1).ToList().FirstOrDefault();
            }
            return (tempResult.ToUpper() == "TRUE" ? true : false);
        }
    }
}
