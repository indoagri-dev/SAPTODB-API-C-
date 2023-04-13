using IMobileAPI.Models;
using IndoAgri.SapToDB.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IndoAgri.SapToDB.Api.Controllers
{
    [RoutePrefix("api/Log")]
    public class Log_SAPController : ApiController
    {
        private Context_MobileAgri _dbAgri = null;
        Object jsonResult = null;
        public Log_SAPController()
        {
            _dbAgri = new Context_MobileAgri();
            jsonResult = new Object();
        }

        [HttpGet]
        [Route("InsertJobbatch")]
        public async Task<IHttpActionResult> Insertjobbatch(string jobname, string sdldate, string sdltime, string sdluname, string status, string statusjob)
        {
            DateTime dt1 = DateTime.ParseExact(sdldate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            var jobbatch = new Log_ZustJobBatch();
            jobbatch.JOBNAME = jobname;
            jobbatch.SDLDATE = dt1;
            jobbatch.SDLTIME = sdltime;
            jobbatch.SDLUNAME = sdluname;
            jobbatch.STATUS = status;
            jobbatch.STATUS_JOB = statusjob;
            _dbAgri.LogSAPZUSTJOBBATCH.Add(jobbatch);
            _dbAgri.SaveChanges();
            var CariData = _dbAgri.LogSAPZUSTJOBBATCH.Where(a => a.JOBNAME == jobname).FirstOrDefault();
            if (CariData != null)
            {
                try
                {
                    return Ok("Berhasil");

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("Tidak ada Aktifitas");
        }

        [HttpGet]
        [Route("Jobbatch")]
        public async Task<IHttpActionResult> Getjobbatch(string jobname = null, string startdate = null, string enddate = null)
        {
            var CariData = new List<Log_ZustJobBatch>();
            DateTime dt1 = DateTime.ParseExact(startdate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dt2 = DateTime.ParseExact(enddate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            if (jobname != null)
            {
                CariData = _dbAgri.LogSAPZUSTJOBBATCH.Where(a => a.JOBNAME == jobname && a.SDLDATE >= dt1 && a.SDLDATE <= dt2).ToList();
            }
            else
            {
                CariData = _dbAgri.LogSAPZUSTJOBBATCH.Where(a => a.SDLDATE >= dt1 && a.SDLDATE <= dt2).ToList();
            }

            if (CariData != null)
            {
                try
                {
                    var JSONData = new
                    {
                        Log = CariData
                    };

                    return Ok(JSONData);

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("Tidak ada Aktifitas");
        }


    }
}
