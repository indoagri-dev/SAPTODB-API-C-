using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.IndoAgriData.Model.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IndoAgri.SapToDB.Api.Controllers
{
      [RoutePrefix("api/Assets")]
    public class ZTWAssetStatController : ApiController
    {
        private ZTWAssetStatRepository _ZTWAssetStatRepository;
        private LogPostDataRepository _logPostDataRepository;
        public ZTWAssetStatController()
        {
            _ZTWAssetStatRepository = new ZTWAssetStatRepository();
            _logPostDataRepository = new LogPostDataRepository();
        }

        [HttpPost]
        [Route("move")]
        public IHttpActionResult InsertAssetStats([FromBody] object value, string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogPostData log = new LogPostData();
            log.ApplicationId = IndoAgri.IndoAgriData.Model.Models.EnumApplicationID.ApplicationID.AssetStat.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = value.ToString();
            //log.LogId 
            log.ControllerAction = "ZTWAssetStat/AddRangeAssetStat";
            //log.Status
            _logPostDataRepository.Insert(log);
            _logPostDataRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZTW_ASSETSTAT>>(value.ToString());

                    foreach (var item in param)
                    {
                        var assetStat = _ZTWAssetStatRepository.FindAll().Where(a => a.Werks == item.Werks && a.Erdat == item.Erdat && a.Eqart == item.Eqart).SingleOrDefault();
                        if (assetStat == null)
                        {
                            item.AssetStatId = Guid.NewGuid().ToString();
                            _ZTWAssetStatRepository.Insert(item);
                            _ZTWAssetStatRepository.Save();
                        }
                        else
                        {
                            assetStat.Broken = item.Broken;
                            assetStat.Endat = item.Endat;
                            assetStat.Eqart = item.Eqart;
                            assetStat.Erdat = item.Erdat;
                            assetStat.Ernam = item.Ernam;
                            assetStat.Good = item.Good;
                            assetStat.Rzet = item.Rzet;
                            assetStat.Total = item.Total;
                            assetStat.Werks = item.Werks;
                            assetStat.UpdateDate = item.UpdateDate;
                            assetStat.UpdateTime = item.UpdateTime;
                            _ZTWAssetStatRepository.Update(assetStat);
                            _ZTWAssetStatRepository.Save();
                        }
                    }

                    log.Status = "success";
                    _logPostDataRepository.Update(log);
                    _logPostDataRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logPostDataRepository.Update(log);
                    _logPostDataRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                _logPostDataRepository.Update(log);
                _logPostDataRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        /**
        [HttpGet]
        public IEnumerable<ZTW_ASSETSTAT> GetAssetStatByBACode(string BACode, string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            if (secretkey == secretkeyConfig)
            {
                var assetStat = _ZTWAssetStatRepository.FindAll().Where(a => a.Werks == BACode);
                return assetStat;
            }
            else
            {
                return new List<ZTW_ASSETSTAT>();
            }
        }
         * **/

    }
}
