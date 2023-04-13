using IndoAgri.Approval.Api.Models;
using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.IndoAgriData.Model.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IndoAgri.Approval.Api.Controllers
{
    public class CDVController : ApiController
    {
        private LogPostDataRepository _logApprovalRepository;
        private ZtfCDVLastMailRepository _ztfCDVLastMailRepository;
        public CDVController()
        {
            _ztfCDVLastMailRepository = new ZtfCDVLastMailRepository();
            _logApprovalRepository = new LogPostDataRepository();
        }

        /**
        [HttpPost]
        public IHttpActionResult InsertCDVs([FromBody] object value, string secretkey)
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
                        var assetStat = _ztfCDVLastMailRepository.FindAll().Where(a => a.Cdcomp == item.Cdcomp && a.Cdgsber == item.Cdgsber && a.Approver == item.Approver && a.Nrcdv == item.Nrcdv).SingleOrDefault();
                        if (assetStat == null)
                        {
                            item.CDVId = Guid.NewGuid().ToString();
                            item.CreatedDate = DateTime.Now;
                            _ztfCDVLastMailRepository.Insert(item);
                            _ztfCDVLastMailRepository.Save();
                        }
                        else
                        {
                            if (assetStat.Status == null)
                            {
                                _ztfCDVLastMailRepository.Update(item);
                                _ztfCDVLastMailRepository.Save();
                            }
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
        **/

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

        //http://PRDECC01.hec.indofood.co.id:8081/sap/bc/srt/rfc/sap/zsduagri_gsber_default3/800/zsduagri_gsber_default3/zbnuagri_gsber_default3
        //http://qaecc.hec.indofood.co.id:8020/sap/bc/srt/rfc/sap/zsduagri_gsber_default3/620/zsduagri_gsber_default3/zbnuagri_gsber_default3
        [HttpPost]
        public IHttpActionResult PostApproval(string documentNumber, string usernameSAP, string passwordSAP, string status)
        {
            //string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            SAPService sapService = new SAPService();
            List<IndoAgri.IndoAgriData.Model.Models.UserProfileSAP> userProfiles = new List<IndoAgri.IndoAgriData.Model.Models.UserProfileSAP>();

            if (sapService.IsAuthotizeSAP(usernameSAP, passwordSAP, out userProfiles))
            // if(true)
            {
                var cdvs = _ztfCDVLastMailRepository.FindAll().Where(a => a.Docnum == documentNumber).ToList();
              
                foreach (var item in cdvs)
                {
                    var cdv = _ztfCDVLastMailRepository.FindAll().Where(a => a.CDVId == item.CDVId).SingleOrDefault();
                    cdv.Status = status;
                    cdv.ApprovedDate = DateTime.Now;
                    if (status.ToLower() == "approve")
                    {
                        sapService.PostApprovalCDV(item.Nrcdv, usernameSAP, passwordSAP);
                    }
                    _ztfCDVLastMailRepository.Update(item);
                    _ztfCDVLastMailRepository.Save();
                }
                return Ok();
            }
            else
            {
                return BadRequest("Not Authorize.");
            }
        }

        private bool IsAuthotizeSAP(string userName, string password)
        {
            List<IndoAgri.IndoAgriData.Model.Models.UserProfileSAP> sapCheck = new List<IndoAgri.IndoAgriData.Model.Models.UserProfileSAP>();
            try
            {
                #region Port
                ServiceAuthSAP.ZSDUAGRI_GSBER_DEFAULT3Client s = new ServiceAuthSAP.ZSDUAGRI_GSBER_DEFAULT3Client();
                s.ClientCredentials.UserName.UserName = userName;
                s.ClientCredentials.UserName.Password = password;

                //assign zero to result param
                ServiceAuthSAP.ZuagriGsberDefaultEmpty param = new ServiceAuthSAP.ZuagriGsberDefaultEmpty();
                param.Result = new ServiceAuthSAP.ZustAgriAuthorization[0];

                ServiceAuthSAP.ZuagriGsberDefaultEmptyResponse result;

                IndoAgri.IndoAgriData.Model.Models.UserProfileSAP objDummy = new IndoAgri.IndoAgriData.Model.Models.UserProfileSAP();
                result = s.ZuagriGsberDefaultEmpty(param);
                foreach (ServiceAuthSAP.ZustAgriAuthorization item in result.Result)
                {
                    objDummy.CompanyCode = item.Bukrs;
                    objDummy.BACode = item.Gsber;
                    objDummy.Activity = item.Activity;
                    objDummy.Division = item.Divisi;
                    objDummy.TCode = item.Tcode;
                    sapCheck.Add(objDummy);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
                #endregion

            if (sapCheck.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
