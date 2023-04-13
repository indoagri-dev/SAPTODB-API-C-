using IMobileAPI.Models;
using IMobileAPI.Repository;
using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.IndoAgriData.Model.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace IndoAgri.SapToDB.Api.Controllers
{
       [RoutePrefix("api/CDV")]
    public class CDVController : ApiController
    {
        private LogPostDataRepository _logApprovalRepository;
        private ZtfCDVLastMailRepository _ztfCDVLastMailRepository;
        private ZTMAGRI_STOCKPRDRepository _ztmStockPRDRepository;
        private UserRepository _user;
        public CDVController()
        {
            _user = new UserRepository();
            _ztmStockPRDRepository = new ZTMAGRI_STOCKPRDRepository();
            _ztfCDVLastMailRepository = new ZtfCDVLastMailRepository();
            _logApprovalRepository = new LogPostDataRepository();
        }


        [HttpPost]
        [Route("InsertCDVs")]
        public IHttpActionResult InsertCDVs([FromBody] object value, string client,string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogPostData log = new LogPostData();
            log.ApplicationId = EnumApplicationID.ApplicationID.AssetStat.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = value.ToString();
            //log.LogId 
            log.ControllerAction = "CDV/InsertCDVs";
            //log.Status
            _logApprovalRepository.Insert(log);
            _logApprovalRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZTF_CDV_LASTMAIL>>(value.ToString());

                    foreach (var item in param)
                    {
                        var cdv = _ztfCDVLastMailRepository.FindAll().Where(a => a.Cdcomp == item.Cdcomp && a.Client == client && a.Cdgsber == item.Cdgsber && a.Approver == item.Approver && a.Nrcdv == item.Nrcdv).FirstOrDefault();
                        if (cdv == null)
                        {
                            item.CDVId = Guid.NewGuid().ToString();
                            item.CreatedDate = DateTime.Now;
                            item.Client = client;
                            _ztfCDVLastMailRepository.Insert(item);
                            _ztfCDVLastMailRepository.Save();
                        }
                        else
                        {
                                cdv.Amtotal = item.Amtotal;
                                cdv.Approver = item.Approver;
                                cdv.Cdcomp = item.Cdcomp;
                                cdv.Cdgsber = item.Cdgsber;
                                cdv.Docnum = item.Docnum;
                                cdv.Cutotal = item.Cutotal;
                                cdv.Nrbelnr = item.Nrbelnr;
                                cdv.Nrcdv = item.Nrcdv;
                                cdv.Status = item.Status;
                                cdv.NMGSBER = item.NMGSBER;
                                cdv.NMPAYEE = item.NMPAYEE;
                                cdv.Txberita = item.Txberita;
                                cdv.Txberita1 = item.Txberita1;
                                cdv.Txberita2 = item.Txberita2;
                                cdv.Client = client;
                                _ztfCDVLastMailRepository.Update(cdv);
                                _ztfCDVLastMailRepository.Save();
                        }
                    }

                    log.Status = "success";
                    _logApprovalRepository.Update(log);
                    _logApprovalRepository.Save();
                    if (PORTALCDVTESTING(param))
                    {
                        return Ok();
                    }
                    else
                    {
                        return Ok();
                    }
                    
                }
                else
                {
                    log.Status = "not_authorize";
                    _logApprovalRepository.Update(log);
                    _logApprovalRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                _logApprovalRepository.Update(log);
                _logApprovalRepository.Save();
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("ZM017_AGRI")]
        public IHttpActionResult ZM017_AGRI([FromBody] object value, string client, string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogPostData log = new LogPostData();
            log.ApplicationId = EnumApplicationID.ApplicationID.AssetStat.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = value.ToString();
            //log.LogId 
            log.ControllerAction = "SAPTODB/ZM017_AGRI";
            //log.Status
            _logApprovalRepository.Insert(log);
            _logApprovalRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZTMAGRI_STOCKPRD>>(value.ToString());

                    foreach (var item in param)
                    {
                        var stockprd = _ztmStockPRDRepository.FindAll().Where(a => a.WERKS == item.BDATJ && a.BDATJ == item.POPER && a.POPER == item.BUKRS && a.WERKS == item.WERKS && a.MATNR == item.MATNR && a.BWTAR == item.BWTAR).FirstOrDefault();
                        if (stockprd == null)
                        {
                            item.INSERTDATE = DateTime.Now;
                            item.CLIENT = client;
                            _ztmStockPRDRepository.Insert(item);
                            _ztmStockPRDRepository.Save();
                        }
                        else
                        {
                            stockprd.BDATJ = item.BDATJ;
                            stockprd.POPER = item.POPER;
                            stockprd.BUKRS = item.BUKRS;
                            stockprd.WERKS = item.WERKS;
                            stockprd.MATNR = item.MATNR;
                            stockprd.BWTAR = item.BWTAR;
                            stockprd.MTART = item.MTART;
                            stockprd.BKLAS = item.BKLAS;
                            stockprd.MATKL = item.MATKL;
                            stockprd.KTEXT = item.KTEXT;
                            stockprd.MEINS = item.MEINS;
                            stockprd.PVPRS = item.PVPRS;
                            stockprd.PEINH = item.PEINH;
                            stockprd.LBKUM = item.LBKUM;
                            stockprd.SALK3 = item.SALK3;
                            stockprd.SPART = item.SPART;
                            stockprd.PSPNR = item.PSPNR;
                            stockprd.MLAST = item.MLAST;
                            stockprd.VPRSV = item.VPRSV;
                            stockprd.WAERS = item.WAERS;
                            stockprd.STATUS = item.STATUS;
                            stockprd.STATUSTEXT = item.STATUSTEXT;
                            stockprd.CREATEDDATE = item.CREATEDDATE;
                            stockprd.CREATEDNAME = item.CREATEDNAME;
                            stockprd.CLIENT = item.CLIENT;
                            _ztmStockPRDRepository.Update(stockprd);
                            _ztmStockPRDRepository.Save();
                        }
                    }

                    log.Status = "success";
                    _logApprovalRepository.Update(log);
                    _logApprovalRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logApprovalRepository.Update(log);
                    _logApprovalRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                _logApprovalRepository.Update(log);
                _logApprovalRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        public bool PORTALCDVTESTING(List<ZTF_CDV_LASTMAIL> DocCDV)
        {
            var firebaseOptionsServerId = "AAAAshvcR5A:APA91bEnbi3wjhlOWUfq4kEtxVn3X4zU9iBNEQGXvhUUlRzLKjh5DDAgIL4uCog5i340PZVx4iy04Gu4nC3WfBhJFFSR2w8jDAYAqrlX3kEZCEGE36NDNBCOstgJrHZjkQB83hF_T7rV";
            var firebaseOptionsSenderId = "764971599760";
            String titleNotification = "CDV Document For You";
            string applicationId = firebaseOptionsServerId;
            string senderId = firebaseOptionsSenderId;
            var x = new AndroidFCMPushNotificationStatus();
            List<InitialUser> Result = new List<InitialUser>();
            List<ZTF_CDV_LASTMAIL> DataCdv = new List<ZTF_CDV_LASTMAIL>();
            List<ZTF_CDV_LASTMAIL> DataCdvGroup = new List<ZTF_CDV_LASTMAIL>();
            ZtfCDVLastMailRepository _cdv = new ZtfCDVLastMailRepository();
           // DataCdv = _cdv.FindAll().OrderByDescending(a => a.CreatedDate).Where(a => a.Status == null).ToList();
            //DataCdvGroup = DataCdv
            //                  .GroupBy(l => l.Approver)
            //                  .Select(i =>
            //                      new ZTF_CDV_LASTMAIL()
            //                      {
            //                          Approver = i.Key,
            //                          Cdcomp = i.First().Cdcomp,
            //                          Cdgsber = i.First().Cdgsber,
            //                          Txberita = i.First().Txberita,
            //                          Txberita1 = i.First().Txberita1,
            //                          Txberita2 = i.First().Txberita2,
            //                          Docnum = i.First().Docnum,
            //                          Status = i.First().Status,
            //                          Client = i.First().Client
            //                      }
            //                  ).ToList();
            foreach (ZTF_CDV_LASTMAIL FR in DocCDV)
            {
                var notif = new UsersActivities()
                {
                    Package = "com.simp.portal",
                    userSAP = FR.Approver,
                    LoggedIn = true
                };

                var NotifyRow = _user.SendFindUserNotificationCDV(notif);
                foreach (var Notif in NotifyRow)
                {
                    var Row = new InitialUser();
                    Row.body = "CDV " + FR.Txberita + " " + FR.Txberita1 + " / " + FR.Docnum;
                    Row.token = Notif.TokenFireBase;
                    Result.Add(Row);
                }
            }
            string response = null;
            foreach (var dir in Result)
            {
                response = x.SendNotificationNonProxy(applicationId, senderId, dir.token, titleNotification, dir.body);
                _user.UpdateStatusPush(dir.token);
            }
            if (response == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public class AndroidFCMPushNotificationStatus
        {
            public string SendNotification(string applicationID, string senderId, string deviceId, string title, string message)
            {
                string MyProxyHostString = "10.126.111.123";
                int MyProxyPort = 4480;
                //string MyProxyHostString = "172.20.3.24";
                //int MyProxyPort = 8080;
                string username = "dwi.fajar";
                string password = "covid19!";

                var urlFireBase = new Uri("https://fcm.googleapis.com/fcm/send");
                try
                {
                    HttpWebRequest tRequest = WebRequest.Create(urlFireBase) as HttpWebRequest;
                    WebProxy myProxy = new WebProxy();
                    myProxy = new WebProxy(MyProxyHostString, MyProxyPort);
                    myProxy.Credentials = new NetworkCredential(username, password);
                    tRequest.Proxy = myProxy;
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        to = deviceId,
                        notification = new
                        {
                            body = message,
                            title = title,

                        },
                        priority = "high"
                    };

                    string postbody = JsonConvert.SerializeObject(data).ToString();

                    Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                return (sResponseFromServer);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
            public string SendNotificationNonProxy(string applicationID, string senderId, string deviceId, string title, string message)
            {

                var urlFireBase = new Uri("https://fcm.googleapis.com/fcm/send");
                try
                {
                    HttpWebRequest tRequest = WebRequest.Create(urlFireBase) as HttpWebRequest;
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        to = deviceId,
                        notification = new
                        {
                            body = message,
                            title = title,

                        },
                        priority = "high"
                    };

                    string postbody = JsonConvert.SerializeObject(data).ToString();

                    Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                return (sResponseFromServer);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }

            public string SendNotificationNonProxyCDV(string applicationID, string senderId, string deviceId, string title, string message)
            {

                var urlFireBase = new Uri("https://fcm.googleapis.com/fcm/send");
                try
                {
                    HttpWebRequest tRequest = WebRequest.Create(urlFireBase) as HttpWebRequest;
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        to = deviceId,
                        notification = new
                        {
                            body = message,
                            title = title,
                            click_action = "com.simp.portal.submenu.cdv"
                        },
                        priority = "high"
                    };

                    string postbody = JsonConvert.SerializeObject(data).ToString();

                    Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                return (sResponseFromServer);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
        }


        public class FormPushFCM
        {
            public string[] deviceId { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public string click_action { get; set; }
        }
        public class FCMResponse
        {
            public long multicast_id { get; set; }
            public int success { get; set; }
            public int failure { get; set; }
            public int canonical_ids { get; set; }
            public List<FCMResult> results { get; set; }
        }
        public class FCMResult
        {
            public string message_id { get; set; }
        }
        public class InitialUser
        {
            public string token { get; set; }
            public string body { get; set; }
        }

        /**
        [HttpGet]
        public IEnumerable<ZTF_CDV_LASTMAIL> GetCDVByApprover(string approver, string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            if (secretkey == secretkeyConfig)
            {
                var data = _ztfCDVLastMailRepository.FindAll().Where(a => a.Approver == approver);
                return data;
            }
            else
            {
                return new List<ZTF_CDV_LASTMAIL>();
            }
        }


        [HttpPost]
        public IHttpActionResult PostApproval(string documentNumber, string userSAP, string password, string status, string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            if (secretkey == secretkeyConfig)
            {

                return Ok();
            }
            else
            {
                return BadRequest("Not Authorize.");
            }
        }
         **/
    
    }
}
