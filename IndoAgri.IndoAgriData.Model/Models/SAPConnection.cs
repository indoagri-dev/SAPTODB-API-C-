using IndoAgri.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Models
{
    public class SAPConnection
    {

        public static ERPConnect.R3Connection R3Connection()
        {
            ERPConnect.LIC.SetLic(System.Configuration.ConfigurationManager.AppSettings["SerialTheobald"].ToString());
            string client = System.Configuration.ConfigurationManager.AppSettings["SapClient"].ToString();
            string host = System.Configuration.ConfigurationManager.AppSettings["SapHost"].ToString();
            int system = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SapSystemNumber"]);
            string username = System.Configuration.ConfigurationManager.AppSettings["ERPUsername"].ToString();
            string password = Md5Config.Decrypt(System.Configuration.ConfigurationManager.AppSettings["ERPPassword"].ToString(), "IndoAgri", true);
            ERPConnect.R3Connection r3con = new ERPConnect.R3Connection(host, system, username, password, "EN", client);
            return r3con;
        }

        public static ERPConnect.R3Connection R3Connection(string username, string password)
        {
            ERPConnect.LIC.SetLic(System.Configuration.ConfigurationManager.AppSettings["SerialTheobald"].ToString());
            string client = System.Configuration.ConfigurationManager.AppSettings["SapClient"].ToString();
            string host = System.Configuration.ConfigurationManager.AppSettings["SapHost"].ToString();
            int system = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SapSystemNumber"]);
            ERPConnect.R3Connection r3con = new ERPConnect.R3Connection(host, system, username, password, "EN", client);
            return r3con;
        }
    }
}
