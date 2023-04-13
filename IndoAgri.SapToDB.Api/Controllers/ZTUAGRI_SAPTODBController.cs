using Indoagri.IndoagriIACData.Model.Models;
using Indoagri.IndoagriIACData.Model.Repositories;
using Indoagri.IndoagriPOICAT.Model.Models;
using Indoagri.IndoagriPOICAT.Model.Repositories;
using IndoAgri.IndoAgriData.Model;
using IndoAgri.IndoAgriData.Model.Models;
using IndoAgri.IndoAgriData.Model.Repositories;
using IndoAgri.SapToDB.Api.Helper;
using IndoAgri.SapToDB.Api.Libs;
using IndoAgri.SapToDB.Api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Http;
namespace IndoAgri.SapToDB.Api.Controllers
{
    [RoutePrefix("api/SAPTODB")]
    public class ZTUAGRI_SAPTODBController : ApiController
    {
        private LogSAPToDBRepository _logSAPToDBRepository;
        private LogPostDataRepository _logApprovalRepository;
        private MIP_MATERIALRepository _mipRepository;
        private ZTPAGRI_MILLDATARepository _ztpMillDataRepository;
        private ZTPAGRI_MILLPRODRepository _ztpMillProdRepository;
        private ZTMAGRI_STOCKPRDRepository _ztmStockPRDRepository;
        private ZTM_PR_LISTRepository _ztmPRListRepository;
        private ZTM_Order_PORepository _ztmorderPOListRepository;
        private ZTMCERA_LISTRepository _ztmceraListRepository;
        private ZTUAGRI_HKRepo _ztuagriHKRepo;
        private ZTUAGRI_BPRepo _ztuagriBPRepo;
        private ZTUAGRI_PRCALCHVRepo _ztuagriPRCALCHVRepo;
        private ZTUAGRI_PBS2Repo _ztuagriPBS2Repo;
        private ZC0032_Prod _ZC0032_Prod;
        private ZTUAGRI_LOGBOOKRepo _logbookRepo;
        private PurchaseOrderHeaderRepositories _poicatheader;
        private PurchaseOrderDetailRepositories _poicatitem;
        private PurchaseRequestRepositories _poicatrequest;
        private PurchaseRequestDetailRepositories _poicatrequestitem;
        private PurchaseRequisitionRepositories _poicatrequisition;
        private MIP_YTDRepository _mipYTD;
        public ZTUAGRI_SAPTODBController()
        {
            _logSAPToDBRepository = new LogSAPToDBRepository();
            _ztpMillDataRepository = new ZTPAGRI_MILLDATARepository();
            _ztpMillProdRepository = new ZTPAGRI_MILLPRODRepository();
            _mipRepository = new MIP_MATERIALRepository();
            _ztmStockPRDRepository = new ZTMAGRI_STOCKPRDRepository();
            _mipYTD = new MIP_YTDRepository();
            _logApprovalRepository = new LogPostDataRepository();
            _ztmPRListRepository = new ZTM_PR_LISTRepository();
            _ztmceraListRepository = new ZTMCERA_LISTRepository();
            _ztmorderPOListRepository = new ZTM_Order_PORepository();
            _ztuagriHKRepo = new ZTUAGRI_HKRepo();
            _ztuagriBPRepo = new ZTUAGRI_BPRepo();
            _ztuagriPRCALCHVRepo = new ZTUAGRI_PRCALCHVRepo();
            _ztuagriPBS2Repo = new ZTUAGRI_PBS2Repo();
            _ZC0032_Prod = new ZC0032_Prod();
            _logbookRepo = new ZTUAGRI_LOGBOOKRepo();
            _poicatheader = new PurchaseOrderHeaderRepositories();
            _poicatitem = new PurchaseOrderDetailRepositories();
            _poicatrequest = new PurchaseRequestRepositories();
            _poicatrequestitem = new PurchaseRequestDetailRepositories();
        }


        [HttpPost]
        [Route("hk")]
        public async Task<IHttpActionResult> InsertHK([FromBody] object value, string lastupdateddate, string ba)
        {
            var Result = new Object();
            DateTime datetime = DateTime.ParseExact(lastupdateddate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            try
            {
                var HasilJson = JsonConvert.DeserializeObject<List<ZTUAGRI_HK_Temp>>(value.ToString());
                List<ZTUAGRI_HK_Temp> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);
                    await db.SaveDataInTables(dt, "ZTUAGRI_HK_Temp");
                    var db2 = new INDOAGRI_DATAEntities();
                    var j = db2.SP_ZTUAGRI_HK_MOVE(datetime, ba);
                    Result = "Berhasil";
                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {
                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "hk";
                logSAPToDB.ControllerAction = "hk";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("spbs")]
        public async Task<IHttpActionResult> InsertSPBS([FromBody] object value, string loaddatefrom, string loaddateto, string ba)
        {
            var Result = new Object();
            DateTime from = DateTime.ParseExact(loaddatefrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(loaddateto, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            try
            {
                var HasilJson = JsonConvert.DeserializeObject<List<ZTUAGRI_PBS2_RPT_TEMP>>(value.ToString());
                List<ZTUAGRI_PBS2_RPT_TEMP> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);

                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);
                    await db.SaveDataInTables(dt, "ZTUAGRI_PBS2_RPT_TEMP");
                    var db2 = new INDOAGRI_DATAEntities();
                    var j = db2.SP_ZTUAGRI_PBS2_RPT_MOVE(loaddatefrom, loaddateto, ba);
                    Result = "Berhasil";
                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {

                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "spbs";
                logSAPToDB.ControllerAction = "spbs";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("spbstes")]
        public async Task<IHttpActionResult> InsertSPBSTes([FromBody] object value, string loaddatefrom, string loaddateto, string ba)
        {
            var Result = new Object();
            DateTime from = DateTime.ParseExact(loaddatefrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(loaddateto, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            try
            {
                var HasilJson = JsonConvert.DeserializeObject<List<ZTUAGRI_PBS2_RPT_TEMP>>(value.ToString());
                List<ZTUAGRI_PBS2_RPT_TEMP> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);

                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);
                    //await db.SaveDataInTables(dt, "ZTUAGRI_PBS2_RPT_TEMP");
                    //var db2 = new INDOAGRI_DATAEntities();
                    //var j = db2.SP_ZTUAGRI_PBS2_RPT_MOVE(loaddatefrom, loaddateto, ba);
                    await db.DeleteAsyncSPBS(ba, loaddatefrom, loaddateto);
                    await db.SaveDataInTables(dt, "ZTUAGRI_PBS2_RPT_TEMP");
                    Result = "Berhasil";
                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {

                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "spbs";
                logSAPToDB.ControllerAction = "spbs";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("bp")]
        public async Task<IHttpActionResult> InsertBP([FromBody] object value, string bpdatefrom, string bpdateto, string ba)
        {
            var Result = new Object();
            DateTime from = DateTime.ParseExact(bpdatefrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(bpdateto, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            try
            {
                var HasilJson = JsonConvert.DeserializeObject<List<ZTUAGRI_BP_RPT_Temp>>(value.ToString());
                List<ZTUAGRI_BP_RPT_Temp> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                //IEnumerable<ZTUAGRI_BP_RPT_Temp> param = JsonConvert.DeserializeObject<List<ZTUAGRI_BP_RPT_Temp>>(value.ToString());
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                //DataTable dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);
                    await db.SaveDataInTables(dt, "ZTUAGRI_BP_RPT_Temp");
                    var db2 = new INDOAGRI_DATAEntities();
                    var j = db2.SP_ZTUAGRI_BP_RPT_MOVE(bpdatefrom, bpdateto, ba);
                    Result = "Berhasil";
                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {
                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "bp";
                logSAPToDB.ControllerAction = "bp";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("bptes")]
        public async Task<IHttpActionResult> InsertBPTes([FromBody] object value, string bpdatefrom, string bpdateto, string ba)
        {
            var Result = new Object();
            DateTime from = DateTime.ParseExact(bpdatefrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(bpdateto, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            try
            {
                var HasilJson = JsonConvert.DeserializeObject<List<ZTUAGRI_BP_RPT_Temp>>(value.ToString());
                List<ZTUAGRI_BP_RPT_Temp> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);
                    //await db.SaveDataInTables(dt, "ZTUAGRI_BP_RPT_Temp");
                    //var db2 = new INDOAGRI_DATAEntities();
                    //var j = db2.SP_ZTUAGRI_BP_RPT_MOVE(bpdatefrom, bpdateto, ba);
                    await db.DeleteAsyncBP(ba, bpdatefrom, bpdateto);
                    await db.SaveDataInTables(dt, "ZTUAGRI_BP_RPT_Temp");
                    Result = "Berhasil";
                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {
                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "bp";
                logSAPToDB.ControllerAction = "bp";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("mipytd")]
        public async Task<IHttpActionResult> InsertMIPYTD([FromBody] object value, string bpdatefrom = null, string bpdateto = null, string ba = null, string mandt = null, string compgrp = null, string matgrp = null, string lstprd = null)
        {
            var Result = new Object();
            int? response = 0;
            //    DateTime from = DateTime.ParseExact(bpdatefrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            //    DateTime to = DateTime.ParseExact(bpdateto, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            try
            {
                var HasilJson = JsonConvert.DeserializeObject<List<ZTM_YTDTPURC_MIP_TEMP>>(value.ToString());
                List<ZTM_YTDTPURC_MIP_TEMP> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                //IEnumerable<ZTUAGRI_HK_Temp> param = JsonConvert.DeserializeObject<List<ZTUAGRI_HK_Temp>>(value.ToString());
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                var dictionary = new Dictionary<string, object>();
                dictionary.Add("compgrp", compgrp);
                dictionary.Add("matgrp", matgrp);
                dictionary.Add("mandt", mandt);
                dictionary.Add("lstprd", lstprd);
                string jsnParams = JsonConvert.SerializeObject(dictionary);
                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);
                    var datas = _mipYTD.FindAll().ToList();
                    if (datas.Count == 0)
                    {
                        await db.SaveDataInTables(dt, "ZTM_YTDTPURC_MIP_TEMP");
                        var db2 = new INDOAGRI_DATAEntities();
                        string slstprd = Right(lstprd, 6);
                        response = db2.SP_ZTM_YTDTPURC_MIP_MOVE(compgrp, matgrp, mandt, slstprd).FirstOrDefault();
                        if (response > 0)
                        {
                            Result = "Successfully";
                        }
                        else
                        {
                            LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                            LogSAPToDB logSAPToDB = new LogSAPToDB();
                            logSAPToDB.ApplicationId = "ZTM_MATPRC_MIP";
                            logSAPToDB.ControllerAction = "mipytd";
                            logSAPToDB.CreatedAt = DateTime.Now;
                            logSAPToDB.ErrorMessage = "Not Successfull";
                            logSAPToDB.JsonMessageError = jsnParams;
                            logSAPToDB.Json = json;
                            logSAPToDB.Status = "error";
                            logSAPToDBRepository.Insert(logSAPToDB);
                            logSAPToDBRepository.Save();
                            Result = "Not Successfull " + jsnParams;
                        }
                    }
                    else
                    {
                        var db2 = new INDOAGRI_DATAEntities();
                        string slstprd = Right(lstprd, 6);
                        response = db2.SP_ZTM_YTDTPURC_MIP_MOVE(compgrp, matgrp, mandt, slstprd).FirstOrDefault();
                        if (response > 0)
                        {
                            Result = "Successfully";
                        }
                        else
                        {
                            LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                            LogSAPToDB logSAPToDB = new LogSAPToDB();
                            logSAPToDB.ApplicationId = "ZTM_MATPRC_MIP";
                            logSAPToDB.ControllerAction = "mipytd";
                            logSAPToDB.CreatedAt = DateTime.Now;
                            logSAPToDB.ErrorMessage = "Not Successfull";
                            logSAPToDB.JsonMessageError = jsnParams;
                            logSAPToDB.Json = json;
                            logSAPToDB.Status = "error";
                            logSAPToDBRepository.Insert(logSAPToDB);
                            logSAPToDBRepository.Save();
                            Result = "Not Successfull " + jsnParams;
                        }
                    }
                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {
                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "SAPTODB_MIP_YTDTPURC";
                logSAPToDB.ControllerAction = "mipytd";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("mipmat")]
        public async Task<IHttpActionResult> InsertMIPYTD([FromBody] object value, string bpdatefrom = null, string bpdateto = null, string ba = null, string mandt = null, string matgrp = null, string area = null, string matnr = null, string lstprd = null)
        {
            var Result = new Object();
            int? response = 0;
            // DateTime from = DateTime.ParseExact(bpdatefrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            // DateTime to = DateTime.ParseExact(bpdateto, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            try
            {
                var HasilJson = JsonConvert.DeserializeObject<List<ZTM_MATPRC_MIP_TEMP>>(value.ToString());
                List<ZTM_MATPRC_MIP_TEMP> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                //IEnumerable<ZTUAGRI_HK_Temp> param = JsonConvert.DeserializeObject<List<ZTUAGRI_HK_Temp>>(value.ToString());
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                var dictionary = new Dictionary<string, object>();
                dictionary.Add("matgrp", matgrp);
                dictionary.Add("mandt", mandt);
                dictionary.Add("area1", area);
                dictionary.Add("matnr", matnr);
                dictionary.Add("lstprd", lstprd);
                string jsnParams = JsonConvert.SerializeObject(dictionary);
                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);

                    var datas = _mipRepository.FindAll().ToList();
                    if (datas.Count == 0)
                    {
                        await db.SaveDataInTables(dt, "ZTM_MATPRC_MIP_TEMP");
                        var db2 = new INDOAGRI_DATAEntities();
                        string slstprd = Right(lstprd, 6);
                        response = db2.SP_ZTM_MATPRC_MIP_MOVE(matgrp, area, matnr, mandt, slstprd).FirstOrDefault();
                        if (response > 0)
                        {
                            Result = "Successfully";
                        }
                        else
                        {
                            LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                            LogSAPToDB logSAPToDB = new LogSAPToDB();
                            logSAPToDB.ApplicationId = "ZTM_MATPRC_MIP";
                            logSAPToDB.ControllerAction = "mipmat";
                            logSAPToDB.CreatedAt = DateTime.Now;
                            logSAPToDB.ErrorMessage = "Not Successfull";
                            logSAPToDB.JsonMessageError = jsnParams;
                            logSAPToDB.Json = json;
                            logSAPToDB.Status = "error";
                            logSAPToDBRepository.Insert(logSAPToDB);
                            logSAPToDBRepository.Save();
                            Result = "Not Successfull " + jsnParams;
                        }

                    }
                    else
                    {
                        var db2 = new INDOAGRI_DATAEntities();
                        string slstprd = Right(lstprd, 6);
                        response = db2.SP_ZTM_MATPRC_MIP_MOVE(matgrp, area, matnr, mandt, slstprd).FirstOrDefault();
                        if (response > 0)
                        {
                            Result = "Successfully";
                        }
                        else
                        {
                            LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                            LogSAPToDB logSAPToDB = new LogSAPToDB();
                            logSAPToDB.ApplicationId = "ZTM_MATPRC_MIP";
                            logSAPToDB.ControllerAction = "mipmat";
                            logSAPToDB.CreatedAt = DateTime.Now;
                            logSAPToDB.ErrorMessage = "Not Successfull";
                            logSAPToDB.JsonMessageError = jsnParams;
                            logSAPToDB.Json = json;
                            logSAPToDB.Status = "error";
                            logSAPToDBRepository.Insert(logSAPToDB);
                            logSAPToDBRepository.Save();
                            Result = "Not Successfull " + jsnParams;
                        }
                    }

                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {
                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "ZTM_MATPRC_MIP";
                logSAPToDB.ControllerAction = "mipmat";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("comsaticStock")]
        public async Task<IHttpActionResult> InsertComsaticStock([FromBody] object value, string bukrs, string plant, string asofdate)
        {
            var Result = new Object();
            DateTime datetime = DateTime.ParseExact(asofdate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            try
            {
                var HasilJson = JsonConvert.DeserializeObject<List<SALES_MOBILE_STOCK_TEMP>>(value.ToString());
                List<SALES_MOBILE_STOCK_TEMP> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);
                    await db.SaveDataInTables(dt, "SALES_MOBILE_STOCK_TEMP");
                    var db2 = new INDOAGRI_DATAEntities();
                    var j = db2.SP_COMSATIC_STOCK_MOVE(plant, asofdate, bukrs);
                    Result = "Berhasil";
                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {
                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "COMSATIC";
                logSAPToDB.ControllerAction = "COMSATIC";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("runrasio")]
        public async Task<IHttpActionResult> RunningAccountRasio([FromBody] object value, string companycode, string estate, string period, string year)
        {
            var Result = new Object();
            DateTime datetime = DateTime.ParseExact(year, "yyyy", System.Globalization.CultureInfo.InvariantCulture);
            try
            {

                var HasilJson = JsonConvert.DeserializeObject<List<RUNACC_RASIO_TEMP>>(value.ToString());
                List<RUNACC_RASIO_TEMP> objlist = HasilJson;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt.Rows.Count > 0)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["INDOAGRI_DATAEntities"].ConnectionString;
                    var db = new Entities(connectionString);
                    await db.SaveDataInTables(dt, "RUNACC_RASIO_TEMP");
                    var db2 = new INDOAGRI_DATAEntities();
                    var j = db2.SP_RUNACC_RASIO(companycode, estate, period, year);
                    Result = "Berhasil";
                }
                var JSONData = new
                {
                    Message = Result
                };
                string SerializeData = JsonConvert.SerializeObject(JSONData);
                return Ok(JSONData);
            }
            catch (Exception ex)
            {
                LogSAPToDBRepository logSAPToDBRepository = new LogSAPToDBRepository();
                LogSAPToDB logSAPToDB = new LogSAPToDB();
                logSAPToDB.ApplicationId = "COMSATIC";
                logSAPToDB.ControllerAction = "COMSATIC";
                logSAPToDB.CreatedAt = DateTime.Now;
                logSAPToDB.ErrorMessage = ex.ToString().Length > 499 ? ex.ToString().Substring(0, 499) : ex.ToString();
                logSAPToDB.Json = value.ToString().Length > 499 ? value.ToString().Substring(0, 499) : value.ToString();
                logSAPToDB.Status = "error";
                logSAPToDBRepository.Insert(logSAPToDB);
                logSAPToDBRepository.Save();
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("insertmilldata")]
        public IHttpActionResult InsertMillData([FromBody] object value, string client, string secretkey)
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
            log.ControllerAction = "SAPTODB/InsertMillData";
            //log.Status
            _logApprovalRepository.Insert(log);
            _logApprovalRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZTPAGRI_MILLDATA>>(value.ToString());

                    foreach (var item in param)
                    {
                        var milldata = _ztpMillDataRepository.FindAll().Where(a => a.WERKS == item.WERKS && a.Client == client && a.GJAHR == item.GJAHR && a.MONAT == item.MONAT && a.PRD_DATE == item.PRD_DATE).FirstOrDefault();
                        if (milldata == null)
                        {
                            item.CreatedDate = DateTime.Now;
                            item.Client = client;
                            _ztpMillDataRepository.Insert(item);
                            _ztpMillDataRepository.Save();
                        }
                        else
                        {
                            milldata.MANDT = item.MANDT;
                            milldata.BUKRS = item.BUKRS;
                            milldata.WERKS = item.WERKS;
                            milldata.GJAHR = item.GJAHR;
                            milldata.MONAT = item.MANDT;
                            milldata.PRD_DATE = item.PRD_DATE;
                            milldata.CAPACITY = item.CAPACITY;
                            milldata.FFBOPN = item.FFBOPN;
                            milldata.FFBRTY = item.FFBRTY;
                            milldata.FFBRTD = item.FFBRTD;
                            milldata.FFBPTY = item.FFBPTY;
                            milldata.FFBPTD = item.FFBPTD;
                            milldata.FFBCLS = item.FFBCLS;
                            milldata.CPOOPN1 = item.CPOOPN1;
                            milldata.CPOOPN2 = item.CPOOPN2;
                            milldata.CPOOPN3 = item.CPOOPN3;
                            milldata.CPOOPN4 = item.CPOOPN4;
                            milldata.CPOOPEN = item.CPOOPEN;
                            milldata.CPOPRD1 = item.CPOPRD1;
                            milldata.CPOPRD2 = item.CPOPRD2;
                            milldata.CPOPRD3 = item.CPOPRD3;
                            milldata.CPOPRD4 = item.CPOPRD4;
                            milldata.CPOPTY = item.CPOPTY;
                            milldata.CPOPTD = item.CPOPTD;
                            milldata.OER = item.OER;
                            milldata.OERTD = item.OERTD;
                            milldata.FFAPTY = item.FFAPTY;
                            milldata.FFAPTD = item.FFAPTD;
                            milldata.CPODIS1 = item.CPODIS1;
                            milldata.CPODIS2 = item.CPODIS2;
                            milldata.CPODIS3 = item.CPODIS3;
                            milldata.CPODIS4 = item.CPODIS4;
                            milldata.CPODTY = item.CPODTY;
                            milldata.CPODTD = item.CPODTD;
                            milldata.CPOOTR1 = item.CPOOTR1;
                            milldata.CPOOTR2 = item.CPOOTR2;
                            milldata.CPOOTR3 = item.CPOOTR3;
                            milldata.CPOOTR4 = item.CPOOTR4;
                            milldata.CPOOGL1 = item.CPOOGL1;
                            milldata.CPOOGL2 = item.CPOOGL2;
                            milldata.CPOOGL3 = item.CPOOGL3;
                            milldata.CPOOGL4 = item.CPOOGL4;
                            milldata.CPOCLS1 = item.CPOCLS1;
                            milldata.FFACPO1 = item.FFACPO1;
                            milldata.CPOCLS2 = item.CPOCLS2;
                            milldata.FFACPO2 = item.FFACPO2;
                            milldata.CPOCLS3 = item.CPOCLS3;
                            milldata.FFACPO3 = item.FFACPO3;
                            milldata.CPOCLS4 = item.CPOCLS4;
                            milldata.FFACPO4 = item.FFACPO4;
                            milldata.CPOCLOSE = item.CPOCLOSE;
                            milldata.PKOPNBS = item.PKOPNBS;
                            milldata.PKOPNOT = item.PKOPNOT;
                            milldata.PKOPEN = item.PKOPEN;
                            milldata.PKPROD = item.PKPROD;
                            milldata.PKPRDFN = item.PKPRDFN;
                            milldata.PKPRDTY = item.PKPRDTY;
                            milldata.PKPRDTD = item.PKPRDTD;
                            milldata.PKEXTRT = item.PKEXTRT;
                            milldata.PKEXTD = item.PKEXTD;
                            milldata.PKDISTY = item.PKDISTY;
                            milldata.PKDISTD = item.PKDISTD;
                            milldata.PKOTHTFN = item.PKOTHTFN;
                            milldata.PKOTHRFN = item.PKOTHRFN;
                            milldata.PKOTHGL = item.PKOTHGL;
                            milldata.PKOTHTD = item.PKOTHTD;
                            milldata.PKCLSBS = item.PKCLSBS;
                            milldata.PKCLSOT = item.PKCLSOT;
                            milldata.PKCLOSE = item.PKCLOSE;
                            milldata.MILLSTR = item.MILLSTR;
                            milldata.MILLSTP = item.MILLSTP;
                            milldata.PERHRTY = item.PERHRTY;
                            milldata.PERHRTD = item.PERHRTD;
                            milldata.BREAKDO = item.BREAKDO;
                            milldata.UTYHRTY = item.UTYHRTY;
                            milldata.UTYHRTD = item.UTYHRTD;
                            milldata.MILUTTY = item.MILUTTY;
                            milldata.MILUTTD = item.MILUTTD;
                            milldata.MILTHTY = item.MILTHTY;
                            milldata.MILTHTD = item.MILTHTD;
                            milldata.REMARK = item.REMARK;
                            milldata.ERSDA = item.ERSDA;
                            milldata.CPUTM = item.CPUTM;
                            milldata.ERNAM = item.ERNAM;
                            milldata.LAEDA = item.LAEDA;
                            milldata.UPDDT = item.UPDDT;
                            milldata.AENAM = item.AENAM;
                            milldata.CPOPRD5 = item.CPOPRD5;
                            milldata.CPOPRD6 = item.CPOPRD6;
                            milldata.CPODIS5 = item.CPODIS5;
                            milldata.CPODIS6 = item.CPODIS6;
                            milldata.CPOOGL5 = item.CPOOGL5;
                            milldata.CPOOGL6 = item.CPOOGL6;
                            milldata.CPOCLS5 = item.CPOCLS5;
                            milldata.CPOCLS6 = item.CPOCLS6;
                            milldata.FFACPO5 = item.FFACPO5;
                            milldata.FFACPO6 = item.FFACPO6;
                            milldata.CPOOTR5 = item.CPOOTR5;
                            milldata.CPOOTR6 = item.CPOOTR6;
                            milldata.CPOOPN5 = item.CPOOPN5;
                            milldata.CPOOPN6 = item.CPOOPN6;
                            //cdv.ApprovedDate 

                            _ztpMillDataRepository.Update(milldata);
                            _ztpMillDataRepository.Save();
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

        [HttpPost]
        [Route("insertmillprod")]
        public IHttpActionResult InsertMillProd([FromBody] object value, string client, string secretkey)
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
            log.ControllerAction = "SAPTODB/InsertMillProd";
            //log.Status
            _logApprovalRepository.Insert(log);
            _logApprovalRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZTPAGRI_MILLPROD>>(value.ToString());

                    foreach (var item in param)
                    {
                        var millProd = _ztpMillProdRepository.FindAll().Where(a => a.WERKS == item.WERKS && a.GJAHR == item.GJAHR && a.MONAT == item.MONAT && a.AUFNR == item.AUFNR).FirstOrDefault();
                        if (millProd == null)
                        {
                            item.CreatedDate = DateTime.Now;
                            item.Client = client;
                            _ztpMillProdRepository.Insert(item);
                            _ztpMillProdRepository.Save();
                        }
                        else
                        {
                            millProd.MANDT = item.MANDT;
                            millProd.AUFNR = item.AUFNR;
                            millProd.WERKS = item.WERKS;
                            millProd.GJAHR = item.GJAHR;
                            millProd.MONAT = item.MANDT;
                            millProd.STATUS = item.STATUS;
                            millProd.ERSDA = item.ERSDA;
                            millProd.CPUTM = item.CPUTM;
                            millProd.ERNAM = item.ERNAM;
                            millProd.LAEDA = item.LAEDA;
                            millProd.UPDDT = item.UPDDT;
                            millProd.AENAM = item.AENAM;
                            millProd.Client = item.Client;
                            _ztpMillProdRepository.Update(millProd);
                            _ztpMillProdRepository.Save();
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

        [HttpPost]
        [Route("ZM017_AGRI")]
        public IHttpActionResult ZM017_AGRI([FromBody] object value, string client, string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZM017_AGRI";
            //log.Status
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZTMAGRI_STOCKPRD>>(value.ToString());
                    log.Count = param.Count().ToString();

                    foreach (var item in param)
                    {
                        var stockprd = _ztmStockPRDRepository.FindAll().Where(a => a.BDATJ == item.BDATJ && a.POPER == item.POPER && a.BUKRS == item.BUKRS && a.WERKS == item.WERKS && a.MATNR == item.MATNR && a.BWTAR == item.BWTAR).FirstOrDefault();
                        JsonMessageError = JsonConvert.SerializeObject(item);
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
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ZTM_PR_LIST")]
        public IHttpActionResult ZTM_PR_LIST([FromBody] object value, string client, string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            //log.LogId 
            log.ControllerAction = "SAPTODB/ZTM_PR_LIST";
            //log.Status
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZTM_PR_LIST>>(value.ToString());
                    log.Count = param.Count().ToString();

                    foreach (var item in param)
                    {
                        var ztmPRLIST = _ztmPRListRepository.FindAll().Where(a =>
                            a.BUKRS == item.BUKRS &&
                            a.WERKS == item.WERKS &&
                            a.BANFN == item.BANFN &&
                            a.BNFPO == item.BNFPO).FirstOrDefault();
                        JsonMessageError = JsonConvert.SerializeObject(item);
                          //  parametersMessage = "BUKRS: "+item.BUKRS+" "+"WERKS: "+item.WERKS+" "+"BANFN: "+item.BANFN+" "+"BNFPO: "+item.BNFPO;
                        //var ztmPRLIST = _ztmPRListRepository.FindAll().Where(a =>
                        //   a.BUKRS == item.BUKRS &&
                        //   a.WERKS == item.WERKS &&
                        //   a.BANFN == item.BANFN &&
                        //   a.BNFPO == item.BNFPO &&
                        //   a.EBELN == item.EBELN &&
                        //   a.EBELP == item.EBELP &&
                        //   a.EKGRP == item.EKGRP &&
                        //   a.EKGRP_PO == item.EKGRP_PO).FirstOrDefault();
                        if (ztmPRLIST == null)
                        {
                            item.INSERTDATE = DateTime.Now;
                            item.CLIENT = client;
                            _ztmPRListRepository.Insert(item);
                            _ztmPRListRepository.Save();
                        }
                        else
                        {
                            ztmPRLIST.BUKRS = item.BUKRS;
                            ztmPRLIST.WERKS = item.WERKS;
                            ztmPRLIST.BANFN = item.BANFN;
                            ztmPRLIST.BNFPO = item.BNFPO;
                            ztmPRLIST.MATNR = item.MATNR;
                            ztmPRLIST.NOMOR = item.NOMOR;
                            ztmPRLIST.NAME1 = item.NAME1;
                            ztmPRLIST.ELIKZ = item.ELIKZ;
                            ztmPRLIST.AREA = item.AREA;
                            ztmPRLIST.AREA1 = item.AREA1;
                            ztmPRLIST.CMGRP = item.CMGRP;
                            ztmPRLIST.SHORT = item.SHORT;
                            ztmPRLIST.KNTTP = item.KNTTP;
                            ztmPRLIST.TXZ01 = item.TXZ01;
                            ztmPRLIST.CAPITAL = item.CAPITAL;
                            ztmPRLIST.CAPDESC = item.CAPDESC;
                            ztmPRLIST.MATGRP = item.MATGRP;
                            ztmPRLIST.WGBEZ2 = item.WGBEZ2;
                            ztmPRLIST.SUBGRP = item.SUBGRP;
                            ztmPRLIST.SDESC = item.SDESC;
                            ztmPRLIST.MATKL = item.MATKL;
                            ztmPRLIST.WGBEZ = item.WGBEZ;
                            ztmPRLIST.MENGE = item.MENGE;
                            ztmPRLIST.BSMNG = item.BSMNG;
                            ztmPRLIST.OQTY = item.OQTY;
                            ztmPRLIST.EBAKZ = item.EBAKZ;
                            ztmPRLIST.MEINS = item.MEINS;
                            ztmPRLIST.LFDAT = item.LFDAT;
                            ztmPRLIST.FRGDT = item.FRGDT;
                            ztmPRLIST.BADAT = item.BADAT;
                            ztmPRLIST.RELDT = item.RELDT;
                            ztmPRLIST.AFNAM = item.AFNAM;
                            ztmPRLIST.IDNLF = item.IDNLF;
                            ztmPRLIST.HDRTXT = item.HDRTXT;
                            ztmPRLIST.STRTXT = item.STRTXT;
                            ztmPRLIST.EKNAM = item.EKNAM;
                            ztmPRLIST.LOEKZ = item.LOEKZ;
                            ztmPRLIST.FRGKZ = item.FRGKZ;
                            ztmPRLIST.ERDAT = item.ERDAT;
                            ztmPRLIST.FRGZU = item.FRGZU;
                            ztmPRLIST.ANLN1 = item.ANLN1;
                            ztmPRLIST.TXT502 = item.TXT502;
                            ztmPRLIST.KOSTL = item.KOSTL;
                            ztmPRLIST.LTEXT = item.LTEXT;
                            ztmPRLIST.SAKTO = item.SAKTO;
                            ztmPRLIST.TXT50 = item.TXT50;
                            ztmPRLIST.AUFNR = item.AUFNR;
                            ztmPRLIST.POSID = item.POSID;
                            ztmPRLIST.POST1 = item.POST1;
                            ztmPRLIST.ERNAM = item.ERNAM;
                            ztmPRLIST.BEDAT = item.BEDAT;
                            ztmPRLIST.POQTY = item.POQTY;
                            ztmPRLIST.FRGGR = item.FRGGR;
                            ztmPRLIST.FRGST = item.FRGST;
                            ztmPRLIST.PREIS = item.PREIS;
                            ztmPRLIST.PEINH = item.PEINH;
                            ztmPRLIST.AMTPR = item.AMTPR;
                            ztmPRLIST.WAERS = item.WAERS;
                            ztmPRLIST.STATUS = item.STATUS;
                            ztmPRLIST.STRSTATUS = item.STRSTATUS;
                            ztmPRLIST.SBTTL = item.SBTTL;
                            ztmPRLIST.GRPCHG = item.GRPCHG;
                            ztmPRLIST.ESTKZ = item.ESTKZ;
                            ztmPRLIST.PORELDT = item.PORELDT;
                            ztmPRLIST.RFQDT = item.RFQDT;
                            ztmPRLIST.OUTDAYS = item.OUTDAYS;
                            ztmPRLIST.COTYP = item.COTYP;
                            ztmPRLIST.BLCKD = item.BLCKD;
                            ztmPRLIST.BLCKT = item.BLCKT;
                            ztmPRLIST.OVERPOSTO = item.OVERPOSTO;
                            ztmPRLIST.SWERK = item.SWERK;
                            ztmPRLIST.REMARK = item.REMARK;
                            ztmPRLIST.JOBDAT = item.JOBDAT;
                            ztmPRLIST.JOBNAM = item.JOBNAM;
                            ztmPRLIST.INSERTDATE = DateTime.Now;
                            ztmPRLIST.CLIENT = client;
                            _ztmPRListRepository.Delete(ztmPRLIST);
                            _ztmPRListRepository.Save();

                            item.INSERTDATE = DateTime.Now;
                            item.CLIENT = client;
                            _ztmPRListRepository.Insert(item);
                            _ztmPRListRepository.Save();

                        }
                    }

                    log.Status = "success ";
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                
                var messages = new List<string>();
                do
                {
                    messages.Add(ex.Message);
                    ex = ex.InnerException;
                }
                while (ex != null);
                var message = string.Join(" - ", messages);
                log.Status = "error ";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("ZTM_ORDER_PO")]
        public IHttpActionResult ZTM_ORDER_PO([FromBody] object value, string client, string secretkey)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZTM_ORDE_PO";
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZTM_ORDER_PO>>(value.ToString());
                    log.Count = param.Count().ToString();

                    foreach (var item in param)
                    {
                        JsonMessageError = JsonConvert.SerializeObject(item);
                        var ztmOrderPO = _ztmorderPOListRepository.FindAll().Where(a =>
                            a.EBELN == item.EBELN &&
                            a.EBELP == item.EBELP &&
                            a.MBLNR == item.MBLNR).FirstOrDefault();
                        if (ztmOrderPO == null)
                        {
                            item.INSERTDATE = DateTime.Now;
                            item.CLIENT = client;
                            _ztmorderPOListRepository.Insert(item);
                            _ztmorderPOListRepository.Save();
                        }
                        else
                        {
                            ztmOrderPO.EBELN = item.EBELN;
                            ztmOrderPO.EBELP = item.EBELP;
                            ztmOrderPO.MBLNR = item.MBLNR;
                            ztmOrderPO.MATNR = item.MATNR;
                            ztmOrderPO.OBJEK = item.OBJEK;
                            ztmOrderPO.ERNAM = item.ERNAM;
                            ztmOrderPO.BUKRS = item.BUKRS;
                            ztmOrderPO.BUTXT = item.BUTXT;
                            ztmOrderPO.EKGRP = item.BUKRS;
                            ztmOrderPO.EKNAM = item.EKNAM;
                            ztmOrderPO.AREA = item.AREA;
                            ztmOrderPO.WERKS = item.WERKS;
                            ztmOrderPO.PLTNM = item.PLTNM;
                            ztmOrderPO.BSART = item.BSART;
                            ztmOrderPO.TRANS_CODE = item.TRANS_CODE;
                            ztmOrderPO.TRANS_NAME = item.TRANS_NAME;
                            ztmOrderPO.PR_TEXT = item.PR_TEXT;
                            ztmOrderPO.PO_HTEXT = item.PO_HTEXT;
                            ztmOrderPO.PO_ITEXT = item.PO_ITEXT;
                            ztmOrderPO.LIFNR = item.LIFNR;
                            ztmOrderPO.ANRED = item.ANRED;
                            ztmOrderPO.NAME1 = item.NAME1;
                            ztmOrderPO.LIFRE = item.LIFRE;
                            ztmOrderPO.LIFNM = item.LIFNM;
                            ztmOrderPO.STRAS = item.STRAS;
                            ztmOrderPO.ORT01 = item.ORT01;
                            ztmOrderPO.PSTLZ = item.PSTLZ;
                            ztmOrderPO.TELF1 = item.TELF1;
                            ztmOrderPO.TELFX = item.TELFX;
                            ztmOrderPO.EMAIL = item.EMAIL;
                            ztmOrderPO.VERKF = item.VERKF;
                            ztmOrderPO.BEDAT = item.BEDAT;
                            ztmOrderPO.PORELDT = item.PORELDT;
                            ztmOrderPO.KPOSN = item.KPOSN;
                            ztmOrderPO.KNTTP = item.KNTTP;
                            ztmOrderPO.PSTYP = item.PSTYP;
                            ztmOrderPO.TXZ01 = item.TXZ01;
                            ztmOrderPO.CAPITAL = item.CAPITAL;
                            ztmOrderPO.CAPDESC = item.CAPDESC;
                            ztmOrderPO.MATGRP = item.MATGRP;
                            ztmOrderPO.WGBEZ2 = item.WGBEZ2;
                            ztmOrderPO.SUBGRP = item.SUBGRP;
                            ztmOrderPO.SDESC = item.SDESC;
                            ztmOrderPO.MATKL = item.MATKL;
                            ztmOrderPO.WGBEZ = item.WGBEZ;
                            ztmOrderPO.EXTWG = item.EXTWG;
                            ztmOrderPO.GRULNM = item.GRULNM;
                            ztmOrderPO.MENGE = item.MENGE;
                            ztmOrderPO.MEINS = item.MEINS;
                            ztmOrderPO.PEINH = item.PEINH;
                            ztmOrderPO.WAERS = item.WAERS;
                            ztmOrderPO.P001 = item.P001;
                            ztmOrderPO.RA00 = item.RA00;
                            ztmOrderPO.PCTRA = item.PCTRA;
                            ztmOrderPO.FRA1 = item.FRA1;
                            ztmOrderPO.ZOTH = item.ZOTH;
                            ztmOrderPO.ZINS = item.ZINS;
                            ztmOrderPO.ZCLR = item.ZCLR;
                            ztmOrderPO.ZPPN = item.ZPPN;
                            ztmOrderPO.ZDEL = item.ZDEL;
                            ztmOrderPO.ZTA1 = item.ZTA1;
                            ztmOrderPO.ZCS1 = item.ZCS1;
                            ztmOrderPO.ZCSTOT = item.ZCSTOT;
                            ztmOrderPO.XZPPN = item.XZPPN;
                            ztmOrderPO.XZDEL = item.XZDEL;
                            ztmOrderPO.XZTA1 = item.XZTA1;
                            ztmOrderPO.XZCS1 = item.XZCS1;
                            ztmOrderPO.PRCORG = item.PRCORG;
                            ztmOrderPO.VALORG = item.VALORG;
                            ztmOrderPO.WKURS = item.WKURS;
                            ztmOrderPO.PRCIDR = item.PRCIDR;
                            ztmOrderPO.VALIDR = item.VALIDR;
                            ztmOrderPO.WAERS2 = item.WAERS2;
                            ztmOrderPO.INCO1 = item.INCO1;
                            ztmOrderPO.INCO2 = item.INCO2;
                            ztmOrderPO.LGORT = item.LGORT;
                            ztmOrderPO.ZTERM = item.ZTERM;
                            ztmOrderPO.TEXT1 = item.TEXT1;
                            ztmOrderPO.FRGGR = item.FRGGR;
                            ztmOrderPO.FRGSX = item.FRGSX;
                            ztmOrderPO.FRGKE = item.FRGKE;
                            ztmOrderPO.FRGZU = item.FRGZU;
                            ztmOrderPO.EINDT = item.EINDT;
                            ztmOrderPO.WEMNG = item.WEMNG;
                            ztmOrderPO.LOEKZ = item.LOEKZ;
                            ztmOrderPO.UEBTK = item.UEBTK;
                            ztmOrderPO.BUDAT = item.BUDAT;
                            ztmOrderPO.GRQTY = item.GRQTY;
                            ztmOrderPO.GRUOM = item.GRUOM;
                            ztmOrderPO.GRLOC = item.GRLOC;
                            ztmOrderPO.GRPRC = item.GRPRC;
                            ztmOrderPO.GRUOM = item.GRUOM;
                            ztmOrderPO.OUTPO = item.OUTPO;
                            ztmOrderPO.OUTPOV = item.OUTPO;
                            ztmOrderPO.ELIKZ = item.ELIKZ;
                            ztmOrderPO.ACHPCT = item.ACHPCT;
                            ztmOrderPO.XBLNR = item.XBLNR;
                            ztmOrderPO.BANFN = item.BANFN;
                            ztmOrderPO.BNFPO = item.BNFPO;
                            ztmOrderPO.BADAT = item.BADAT;
                            ztmOrderPO.RELDT2 = item.RELDT2;
                            ztmOrderPO.FRGDT = item.FRGDT;
                            ztmOrderPO.FRGKZ = item.FRGKZ;
                            ztmOrderPO.LFDAT = item.LFDAT;
                            ztmOrderPO.MENGE2 = item.MENGE2;
                            ztmOrderPO.MEINS2 = item.MEINS2;
                            ztmOrderPO.BSMNG = item.BSMNG;
                            ztmOrderPO.DISPO = item.DISPO;
                            ztmOrderPO.AFNAM = item.AFNAM;
                            ztmOrderPO.BEDNR = item.BEDNR;
                            ztmOrderPO.ERNAM2 = item.ERNAM2;
                            ztmOrderPO.SUBMI = item.SUBMI;
                            ztmOrderPO.ANFNR = item.ANFNR;
                            ztmOrderPO.SAKTO = item.SAKTO;
                            ztmOrderPO.TXT50 = item.TXT502;
                            ztmOrderPO.KOSTL = item.KOSTL;
                            ztmOrderPO.LTEXT = item.LTEXT;
                            ztmOrderPO.ANLN1 = item.ANLN1;
                            ztmOrderPO.TXT502 = item.TXT502;
                            ztmOrderPO.OBJNR = item.OBJNR;
                            ztmOrderPO.PS_PSP_PNR = item.PS_PSP_PNR;
                            ztmOrderPO.POST1 = item.POST1;
                            ztmOrderPO.POSID = item.POSID;
                            ztmOrderPO.PRART = item.PRART;
                            ztmOrderPO.PRATX = item.PRATX;
                            ztmOrderPO.COTYP = item.COTYP;
                            ztmOrderPO.AUFNR = item.AUFNR;
                            ztmOrderPO.AUART = item.AUART;
                            ztmOrderPO.TXT = item.TXT;
                            ztmOrderPO.BELNR = item.BELNR;
                            ztmOrderPO.INVAMT = item.INVAMT;
                            ztmOrderPO.BUDAT2 = item.BUDAT2;
                            ztmOrderPO.DUEDAT = item.DUEDAT;
                            ztmOrderPO.ZFBDT = item.ZFBDT;
                            ztmOrderPO.ZBD1T = item.ZBD1T;
                            ztmOrderPO.MWSKZ = item.MWSKZ;
                            ztmOrderPO.EBAKZ = item.EBAKZ;
                            ztmOrderPO.KNUMV = item.KNUMV;
                            ztmOrderPO.OBJECTID = item.OBJECTID;
                            ztmOrderPO.KEYPO = item.KEYPO;
                            ztmOrderPO.WEBRE = item.WEBRE;
                            ztmOrderPO.LD01 = item.LD01;
                            ztmOrderPO.LD02 = item.LD02;
                            ztmOrderPO.LD03 = item.LD03;
                            ztmOrderPO.LD04 = item.LD04;
                            ztmOrderPO.KALSK = item.KALSK;
                            ztmOrderPO.EKORG = item.EKORG;
                            ztmOrderPO.POCTR = item.POCTR;
                            ztmOrderPO.HTCHG = item.HTCHG;
                            ztmOrderPO.ITCHG = item.ITCHG;
                            ztmOrderPO.EAN11 = item.EAN11;
                            ztmOrderPO.ATWTB1 = item.ATWTB1;
                            ztmOrderPO.ATWTB2 = item.ATWTB2;
                            ztmOrderPO.ATWTB3 = item.ATWTB3;
                            ztmOrderPO.ATWTB4 = item.ATWTB4;
                            ztmOrderPO.ATWTB5 = item.ATWTB5;
                            ztmOrderPO.JOBDAT = item.JOBDAT;
                            ztmOrderPO.JOBNAM = item.JOBNAM;
                            ztmOrderPO.AEDAT = item.AEDAT;


                            ztmOrderPO.INSERTDATE = DateTime.Now;
                            ztmOrderPO.CLIENT = client;
                            _ztmorderPOListRepository.Delete(ztmOrderPO);
                            _ztmorderPOListRepository.Save();

                            item.INSERTDATE = DateTime.Now;
                            item.CLIENT = client;
                            _ztmorderPOListRepository.Insert(item);
                            _ztmorderPOListRepository.Save();

                        }
                    }

                    log.Status = "success";
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("workflowstatusd")]
        //public IHttpActionResult ZTM_CERddA([FromBody] object value, string mandt = null, string bukrs = null, string werks = null,
        //    string gjahr = null,string posnr = null, string posid = null, string docnum = null, string banfn_i= null, 
        //    string bnfpo = null){
        //    string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
        //    // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    LogSAPToDB log = new LogSAPToDB();
        //    log.ApplicationId = EnumApplicationID.ApplicationID.ICAT_SERA.ToString();
        //    log.CreatedAt = DateTime.Now;
        //    log.ErrorMessage = string.Empty;
        //    log.Json = JsonConvert.SerializeObject(value);
        //    var JsonMessageError = string.Empty;
        //    log.ControllerAction = "SAPTODB/ZTM_CERA_LIST";
        //    //log.Status
        //    _logSAPToDBRepository.Insert(log);
        //    _logSAPToDBRepository.Save();

        //    try
        //    {
        //            var param = JsonConvert.DeserializeObject<List<ZTM_CERA_LIST>>(value.ToString());
        //            log.Count = param.Count().ToString();

        //            foreach (var item in param)
        //            {
        //                var ceraList = _ztmceraListRepository.FindAll().
        //                    Where(a => a.MANDT == mandt
        //                     && a.BUKRS == bukrs
        //                        && a.WERKS == werks
        //                     && a.GJAHR == gjahr
        //                     && a.POSNR == posnr
        //                     && a.POSID == posid
        //                     && a.DOCNUM == docnum
        //                       && a.BANFN_I == banfn_i
        //                         && a.BNFPO == bnfpo).FirstOrDefault();
        //                JsonMessageError = JsonConvert.SerializeObject(item);
        //                if (ceraList == null)
        //                {
        //                    item.INSERTDATE = DateTime.Now;
        //                    _ztmceraListRepository.Insert(item);
        //                    _ztmceraListRepository.Save();
        //                }
        //                else
        //                {
        //                    ceraList.MANDT = item.MANDT;
        //                    ceraList.BUKRS = item.BUKRS;
        //                    ceraList.WERKS = item.WERKS;
        //                    ceraList.GJAHR = item.GJAHR;
        //                    ceraList.POSNR = item.POSNR;
        //                    ceraList.POSID = item.POSID;
        //                    ceraList.DOCNUM = item.DOCNUM;
        //                    ceraList.BANFN_I = item.BANFN_I;
        //                    ceraList.BNFPO = item.BNFPO;
        //                    ceraList.CEK = item.CEK;
        //                    ceraList.BUTXT = item.BUTXT;
        //                    ceraList.WERKS_NAME = item.WERKS_NAME;
        //                    ceraList.WAERS_IDR = item.WAERS_IDR;
        //                    ceraList.WAERS = item.WAERS;
        //                    ceraList.KOSTL = item.KOSTL;
        //                    ceraList.BUYER = item.BUYER;
        //                    ceraList.LOCATION = item.LOCATION;
        //                    ceraList.PURPOSE = item.PURPOSE;
        //                    ceraList.BLART = item.BLART;
        //                    ceraList.BUDGETCAT = item.BUDGETCAT;
        //                    ceraList.KNTTP = item.KNTTP;
        //                    ceraList.ZCONVERT = item.ZCONVERT;
        //                    ceraList.APPROVALCODE = item.APPROVAL_CODE;
        //                    ceraList.ATTYP = item.ATTYP;
        //                    ceraList.EKGRP = item.EKGRP;
        //                    ceraList.BUDAT = item.BUDAT;
        //                    ceraList.QUARTAL = item.QUARTAL;
        //                    ceraList.FRGGR = item.FRGGR;
        //                    ceraList.FRGSX = item.FRGSX;
        //                    ceraList.FRGKE = item.FRGKE;
        //                    ceraList.WSTATU = item.WSTATU;
        //                    ceraList.JUSTIFICATION = item.JUSTIFICATION;
        //                    ceraList.TECH_REVIEW = item.TECHREVIEW;
        //                    ceraList.BANFN = item.BANFN;
        //                    ceraList.CP_REVIEW = item.CP_REVIEW;
        //                    ceraList.CTRL_REVIEW = item.CTRL_REVIEW;
        //                    ceraList.FRGZU = item.FRGZU;
        //                    ceraList.LOEKZ = item.LOEKZ;
        //                    ceraList.REJECTION = item.REJECTION;
        //                    ceraList.UNAME_RALL = item.UNAME_RALL;
        //                    ceraList.ANLN1 = item.ANLN1;
        //                    ceraList.ANLN2 = item.ANLN2;
        //                    ceraList.SAKTO = item.SAKTO;
        //                    ceraList.KOSTL_I = item.KOSTL_I;
        //                    ceraList.PS_PSP_PNR = item.PS_PSP_PNR;
        //                    ceraList.POST1 = item.POST1;
        //                    ceraList.ASSCL = item.ASSCL;
        //                    ceraList.MATKL = item.MATKL;
        //                    ceraList.MEINS = item.MEINS;
        //                    ceraList.MENGE = item.MENGE;
        //                    ceraList.ORI_COST = item.ORI_COST;
        //                    ceraList.ACTUAL_COST = item.ACTUAL_COST;
        //                    ceraList.TOTAL_ACTUAL = item.TOTAL_ACTUAL;
        //                    ceraList.BUDGET_COST = item.BUDGET_COST;
        //                    ceraList.TOTAL_BUDGET = item.TOTAL_BUDGET;
        //                    ceraList.VAR_AMOUNT = item.VAR_AMOUNT;
        //                    ceraList.VAR_PERCENT = item.VAR_PERCENT;
        //                    ceraList.POSNR_BNFPO = item.POSNR_BNFPO;
        //                    ceraList.LFDAT = item.LFDAT;
        //                    ceraList.LOEKZ_I = item.LOEKZ_I;
        //                    ceraList.POSTP = item.POSTP;
        //                    ceraList.POSTPONE_WCODE = item.POSTPONE_WCODE;
        //                    ceraList.POSTP_NAME = item.POSTP_NAME;
        //                    ceraList.POSTPDT = item.POSTPDT;
        //                    ceraList.L01_REL_ID = item.L01_REL_ID;
        //                    ceraList.L01_REL_DT = item.L01_REL_DT;
        //                    ceraList.L01_REL_TM = item.L01_REL_TM;
        //                    ceraList.L01_UNAME = item.L01_UNAME;
        //                    ceraList.L0L1 = item.L0L1;
        //                    ceraList.L02_REL_ID = item.L02_REL_ID;
        //                    ceraList.L02_REL_DT = item.L02_REL_DT;
        //                    ceraList.L02_REL_TM = item.L02_REL_TM;
        //                    ceraList.L02_UNAME = item.L02_UNAME;
        //                    ceraList.L1L2 = item.L1L2;
        //                    ceraList.L03_REL_ID = item.L03_REL_ID;
        //                    ceraList.L03_REL_DT = item.L03_REL_DT;
        //                    ceraList.L03_REL_TM = item.L03_REL_TM;
        //                    ceraList.L03_UNAME = item.L03_UNAME;
        //                    ceraList.L2L3 = item.L2L3;
        //                    ceraList.L04_REL_ID = item.L04_REL_ID;
        //                    ceraList.L04_REL_DT = item.L04_REL_DT;
        //                    ceraList.L04_REL_TM = item.L04_REL_TM;
        //                    ceraList.L04_UNAME = item.L04_UNAME;
        //                    ceraList.L3L4 = item.L3L4;
        //                    ceraList.L05_REL_ID = item.L05_REL_ID;
        //                    ceraList.L05_REL_DT = item.L05_REL_DT;
        //                    ceraList.L05_REL_TM = item.L05_REL_TM;
        //                    ceraList.L05_UNAME = item.L05_UNAME;
        //                    ceraList.L4L5 = item.L4L5;
        //                    ceraList.L06_REL_ID = item.L06_REL_ID;
        //                    ceraList.L06_REL_DT = item.L06_REL_DT;
        //                    ceraList.L06_REL_TM = item.L06_REL_TM;
        //                    ceraList.L06_UNAME = item.L06_UNAME;
        //                    ceraList.L5L6 = item.L5L6;
        //                    ceraList.L07_REL_ID = item.L07_REL_ID;
        //                    ceraList.L07_REL_DT = item.L07_REL_DT;
        //                    ceraList.L07_REL_TM = item.L07_REL_TM;
        //                    ceraList.L07_UNAME = item.L07_UNAME;
        //                    ceraList.L6L7 = item.L6L7;
        //                    ceraList.L08_REL_ID = item.L08_REL_ID;
        //                    ceraList.L08_REL_DT = item.L08_REL_DT;
        //                    ceraList.L08_REL_TM = item.L08_REL_TM;
        //                    ceraList.L08_UNAME = item.L08_UNAME;
        //                    ceraList.L7L8 = item.L7L8;
        //                    ceraList.L09_REL_ID = item.L09_REL_ID;
        //                    ceraList.L09_REL_DT = item.L09_REL_DT;
        //                    ceraList.L09_REL_TM = item.L09_REL_TM;
        //                    ceraList.L09_UNAME = item.L09_UNAME;
        //                    ceraList.L8L9 = item.L8L9;
        //                    ceraList.L10_REL_ID = item.L10_REL_ID;
        //                    ceraList.L10_REL_DT = item.L10_REL_DT;
        //                    ceraList.L10_REL_TM = item.L10_REL_TM;
        //                    ceraList.L10_UNAME = item.L10_UNAME;
        //                    ceraList.L9L10 = item.L9L10;
        //                    ceraList.L11_REL_ID = item.L11_REL_ID;
        //                    ceraList.L11_REL_DT = item.L11_REL_DT;
        //                    ceraList.L11_REL_TM = item.L11_REL_TM;
        //                    ceraList.L11_UNAME = item.L11_UNAME;
        //                    ceraList.L10L11 = item.L10L11;
        //                    ceraList.L12_REL_ID = item.L12_REL_ID;
        //                    ceraList.L12_REL_DT = item.L12_REL_DT;
        //                    ceraList.L12_REL_TM = item.L12_REL_TM;
        //                    ceraList.L12_UNAME = item.L12_UNAME;
        //                    ceraList.L11L12 = item.L11L12;
        //                    ceraList.L13_REL_ID = item.L13_REL_ID;
        //                    ceraList.L13_REL_DT = item.L13_REL_DT;
        //                    ceraList.L13_REL_TM = item.L13_REL_TM;
        //                    ceraList.L13_UNAME = item.L13_UNAME;
        //                    ceraList.L12L13 = item.L12L13;
        //                    ceraList.L14_REL_ID = item.L14_REL_ID;
        //                    ceraList.L15_REL_DT = item.L14_REL_DT;
        //                    ceraList.L14_REL_TM = item.L14_REL_TM;
        //                    ceraList.L14_UNAME = item.L14_UNAME;
        //                    ceraList.L13L14 = item.L13L14;
        //                    ceraList.L15_REL_ID = item.L15_REL_ID;
        //                    ceraList.L15_REL_DT = item.L15_REL_DT;
        //                    ceraList.L15_REL_TM = item.L15_REL_TM;
        //                    ceraList.L15_UNAME = item.L15_UNAME;
        //                    ceraList.L14L15 = item.L14L15;
        //                    ceraList.L16_REL_ID = item.L16_REL_ID;
        //                    ceraList.L16_REL_DT = item.L16_REL_DT;
        //                    ceraList.L16_REL_TM = item.L16_REL_TM;
        //                    ceraList.L16_UNAME = item.L16_UNAME;
        //                    ceraList.L15L16 = item.L15L16;
        //                    ceraList.L17_REL_ID = item.L17_REL_ID;
        //                    ceraList.L17_REL_DT = item.L17_REL_DT;
        //                    ceraList.L17_REL_TM = item.L17_REL_TM;
        //                    ceraList.L17_UNAME = item.L17_UNAME;
        //                    ceraList.L16L17 = item.L16L17;
        //                    ceraList.L18_REL_ID = item.L18_REL_ID;
        //                    ceraList.L18_REL_DT = item.L18_REL_DT;
        //                    ceraList.L18_REL_TM = item.L18_REL_TM;
        //                    ceraList.L18_UNAME = item.L18_UNAME;
        //                    ceraList.L17L18 = item.L17L18;
        //                    ceraList.L19_REL_ID = item.L19_REL_ID;
        //                    ceraList.L19_REL_DT = item.L19_REL_DT;
        //                    ceraList.L19_REL_TM = item.L19_REL_TM;
        //                    ceraList.L19_UNAME = item.L19_UNAME;
        //                    ceraList.L18L19 = item.L18L19;
        //                    ceraList.L20_REL_ID = item.L20_REL_ID;
        //                    ceraList.L20_REL_DT = item.L20_REL_DT;
        //                    ceraList.L20_REL_TM = item.L20_REL_TM;
        //                    ceraList.L20_UNAME = item.L20_UNAME;
        //                    ceraList.L19L20 = item.L19L20;
        //                    ceraList.ZASSIGN = item.ZASSIGN;
        //                    ceraList.ERNAM = item.ERNAM;
        //                    ceraList.ERDAT = item.ERDAT;
        //                    ceraList.ERZET = item.ERZET;
        //                    ceraList.AENAM = item.AENAM;
        //                    ceraList.AEDAT = item.AEDAT;
        //                    ceraList.AEZET = item.AEZET;
        //                    ceraList.FLAG = item.FLAG;
        //                    ceraList.INSERTDATE = item.INSERTDATE;
        //                    _ztmceraListRepository.Update(ceraList);
        //                    _ztmceraListRepository.Save();
        //                }
        //            }

        //            log.Status = "success";
        //            log.Count = param.Count().ToString();
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return Ok();
                
        //    }
        //        catch (Exception ex)
        //    {
        //        log.Status = "error";
        //        log.ErrorMessage = ex.Message;
        //        log.JsonMessageError = JsonMessageError;
        //        _logSAPToDBRepository.Update(log);
        //        _logSAPToDBRepository.Save();
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("workflowstatus")]
        public IHttpActionResult ZTM_CERA([FromBody] object value, string mandt = null, string bukrs = null, string werks = null,
            string gjahr = null,string posnr = null, string posid = null, string docnum = null, string banfni= null, 
            string bnfpo = null){
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.ICAT_SERA.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZTM_CERA_LIST";
            //log.Status
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                var param = JsonConvert.DeserializeObject<List<ZTM_CERA_LIST>>(value.ToString());
                log.Count = param.Count().ToString();
                string abanfni = banfni ?? " ";
                string abnfpo = bnfpo ?? "00000";
                string abukrs = bukrs ?? "0";
                string awerks= werks ?? "0";
                string agjahr = gjahr ?? "0";
                string aposnr = posnr ?? "0";
                string aposid = posid ?? "0";
                string adocnum = docnum ?? "0";
                string amandt = mandt ?? "0";
                foreach (var item in param)
                {
                    var ceraList = _ztmceraListRepository.FindAll().
                        Where(a => a.MANDT == amandt
                         && a.BUKRS == abukrs
                         && a.WERKS == awerks
                         && a.GJAHR == agjahr
                         && a.POSNR == aposnr
                         && a.POSID == aposid
                         && a.DOCNUM == adocnum).FirstOrDefault();
                    JsonMessageError = JsonConvert.SerializeObject(item);
                    if (ceraList == null)
                    {
                        if (item.MANDT == null)
                        {
                            item.MANDT = "0";
                        }
                        if (item.BUKRS== null)
                        {
                            item.BUKRS = "0";
                        }
                        if (item.WERKS == null)
                        {
                            item.WERKS = "0";
                        }
                        if (item.GJAHR== null)
                        {
                            item.GJAHR = "0";
                        }
                        if (item.POSNR== null)
                        {
                            item.POSNR= "0";
                        }
                        if (item.POSID == null)
                        {
                            item.POSID = "0";
                        }
                        if (item.DOCNUM == null)
                        {
                            item.DOCNUM = "0";
                        }
                        if (item.BANFNI == null)
                        {
                            item.BANFNI = " ";
                        }
                        if (item.BNFPO == null)
                        {
                            item.BNFPO = "00000";
                        }
                        item.INSERTDATE = DateTime.Now;
                        _ztmceraListRepository.Insert(item);
                        _ztmceraListRepository.Save();
                    }
                    else
                    {
                        if (item.MANDT == null)
                        {
                            item.MANDT = "0";
                        }
                        if (item.BUKRS == null)
                        {
                            item.BUKRS = "0";
                        }
                        if (item.WERKS == null)
                        {
                            item.WERKS = "0";
                        }
                        if (item.GJAHR == null)
                        {
                            item.GJAHR = "0";
                        }
                        if (item.POSNR == null)
                        {
                            item.POSNR = "0";
                        }
                        if (item.POSID == null)
                        {
                            item.POSID = "0";
                        }
                        if (item.DOCNUM == null)
                        {
                            item.DOCNUM = "0";
                        }
                        if (item.BANFNI == null)
                        {
                            item.BANFNI = " ";
                        }
                        if (item.BNFPO == null)
                        {
                            item.BNFPO = "00000";
                        }
                        ceraList.MANDT = item.MANDT;
                        ceraList.BUKRS = item.BUKRS;
                        ceraList.WERKS = item.WERKS;
                        ceraList.GJAHR = item.GJAHR;
                        ceraList.POSNR = item.POSNR;
                        ceraList.POSID = item.POSID;
                        ceraList.DOCNUM = item.DOCNUM;
                        ceraList.BANFNI = item.BANFNI;
                        ceraList.BNFPO = item.BNFPO;
                        ceraList.CEK = item.CEK;
                        ceraList.BUTXT = item.BUTXT;
                        ceraList.WERKSNAME = item.WERKSNAME;
                        ceraList.WAERSIDR = item.WAERSIDR;
                        ceraList.WAERS = item.WAERS;
                        ceraList.KOSTL = item.KOSTL;
                        ceraList.BUYER = item.BUYER;
                        ceraList.LOCATION = item.LOCATION;
                        ceraList.PURPOSE = item.PURPOSE;
                        ceraList.BLART = item.BLART;
                        ceraList.BUDGETCAT = item.BUDGETCAT;
                        ceraList.KNTTP = item.KNTTP;
                        ceraList.ZCONVERT = item.ZCONVERT;
                        ceraList.APPROVALCODE = item.APPROVALCODE;
                        ceraList.ATTYP = item.ATTYP;
                        ceraList.EKGRP = item.EKGRP;
                        ceraList.BUDAT = item.BUDAT;
                        ceraList.QUARTAL = item.QUARTAL;
                        ceraList.FRGGR = item.FRGGR;
                        ceraList.FRGSX = item.FRGSX;
                        ceraList.FRGKE = item.FRGKE;
                        ceraList.WSTATU = item.WSTATU;
                        ceraList.JUSTIFICATION = item.JUSTIFICATION;
                        ceraList.TECHREVIEW = item.TECHREVIEW;
                        ceraList.BANFN = item.BANFN;
                        ceraList.CPREVIEW = item.CPREVIEW;
                        ceraList.CTRLREVIEW = item.CTRLREVIEW;
                        ceraList.FRGZU = item.FRGZU;
                        ceraList.LOEKZ = item.LOEKZ;
                        ceraList.REJECTION = item.REJECTION;
                        ceraList.UNAMERALL = item.UNAMERALL;
                        ceraList.ANLN1 = item.ANLN1;
                        ceraList.ANLN2 = item.ANLN2;
                        ceraList.SAKTO = item.SAKTO;
                        ceraList.KOSTLI = item.KOSTLI;
                        ceraList.PSPSPPNR = item.PSPSPPNR;
                        ceraList.POST1 = item.POST1;
                        ceraList.ASSCL = item.ASSCL;
                        ceraList.MATKL = item.MATKL;
                        ceraList.MEINS = item.MEINS;
                        ceraList.MENGE = item.MENGE;
                        ceraList.ORICOST = item.ORICOST;
                        ceraList.ACTUALCOST = item.ACTUALCOST;
                        ceraList.TOTALACTUAL = item.TOTALACTUAL;
                        ceraList.BUDGETCOST = item.BUDGETCOST;
                        ceraList.TOTALBUDGET = item.TOTALBUDGET;
                        ceraList.VARAMOUNT = item.VARAMOUNT;
                        ceraList.VARPERCENT = item.VARPERCENT;
                        ceraList.POSNRBNFPO = item.POSNRBNFPO;
                        ceraList.LFDAT = item.LFDAT;
                        ceraList.LOEKZI = item.LOEKZI;
                        ceraList.POSTP = item.POSTP;
                        ceraList.POSTPONEWCODE = item.POSTPONEWCODE;
                        ceraList.POSTPNAME = item.POSTPNAME;
                        ceraList.POSTPDT = item.POSTPDT;
                        ceraList.L01RELID = item.L01RELID;
                        ceraList.L01RELDT = item.L01RELDT;
                        ceraList.L01RELTM = item.L01RELTM;
                        ceraList.L01UNAME = item.L01UNAME;
                        ceraList.L0L1 = item.L0L1;
                        ceraList.L02RELID = item.L02RELID;
                        ceraList.L02RELDT = item.L02RELDT;
                        ceraList.L02RELTM = item.L02RELTM;
                        ceraList.L02UNAME = item.L02UNAME;
                        ceraList.L1L2 = item.L1L2;
                        ceraList.L03RELID = item.L03RELID;
                        ceraList.L03RELDT = item.L03RELDT;
                        ceraList.L03RELTM = item.L03RELTM;
                        ceraList.L03UNAME = item.L03UNAME;
                        ceraList.L2L3 = item.L2L3;
                        ceraList.L04RELID = item.L04RELID;
                        ceraList.L04RELDT = item.L04RELDT;
                        ceraList.L04RELTM = item.L04RELTM;
                        ceraList.L04UNAME = item.L04UNAME;
                        ceraList.L3L4 = item.L3L4;
                        ceraList.L05RELID = item.L05RELID;
                        ceraList.L05RELDT = item.L05RELDT;
                        ceraList.L05RELTM = item.L05RELTM;
                        ceraList.L05UNAME = item.L05UNAME;
                        ceraList.L4L5 = item.L4L5;
                        ceraList.L06RELID = item.L06RELID;
                        ceraList.L06RELDT = item.L06RELDT;
                        ceraList.L06RELTM = item.L06RELTM;
                        ceraList.L06UNAME = item.L06UNAME;
                        ceraList.L5L6 = item.L5L6;
                        ceraList.L07RELID = item.L07RELID;
                        ceraList.L07RELDT = item.L07RELDT;
                        ceraList.L07RELTM = item.L07RELTM;
                        ceraList.L07UNAME = item.L07UNAME;
                        ceraList.L6L7 = item.L6L7;
                        ceraList.L08RELID = item.L08RELID;
                        ceraList.L08RELDT = item.L08RELDT;
                        ceraList.L08RELTM = item.L08RELTM;
                        ceraList.L08UNAME = item.L08UNAME;
                        ceraList.L7L8 = item.L7L8;
                        ceraList.L09RELID = item.L09RELID;
                        ceraList.L09RELDT = item.L09RELDT;
                        ceraList.L09RELTM = item.L09RELTM;
                        ceraList.L09UNAME = item.L09UNAME;
                        ceraList.L8L9 = item.L8L9;
                        ceraList.L10RELID = item.L10RELID;
                        ceraList.L10RELDT = item.L10RELDT;
                        ceraList.L10RELTM = item.L10RELTM;
                        ceraList.L10UNAME = item.L10UNAME;
                        ceraList.L9L10 = item.L9L10;
                        ceraList.L11RELID = item.L11RELID;
                        ceraList.L11RELDT = item.L11RELDT;
                        ceraList.L11RELTM = item.L11RELTM;
                        ceraList.L11UNAME = item.L11UNAME;
                        ceraList.L10L11 = item.L10L11;
                        ceraList.L12RELID = item.L12RELID;
                        ceraList.L12RELDT = item.L12RELDT;
                        ceraList.L12RELTM = item.L12RELTM;
                        ceraList.L12UNAME = item.L12UNAME;
                        ceraList.L11L12 = item.L11L12;
                        ceraList.L13RELID = item.L13RELID;
                        ceraList.L13RELDT = item.L13RELDT;
                        ceraList.L13RELTM = item.L13RELTM;
                        ceraList.L13UNAME = item.L13UNAME;
                        ceraList.L12L13 = item.L12L13;
                        ceraList.L14RELID = item.L14RELID;
                        ceraList.L15RELDT = item.L14RELDT;
                        ceraList.L14RELTM = item.L14RELTM;
                        ceraList.L14UNAME = item.L14UNAME;
                        ceraList.L13L14 = item.L13L14;
                        ceraList.L15RELID = item.L15RELID;
                        ceraList.L15RELDT = item.L15RELDT;
                        ceraList.L15RELTM = item.L15RELTM;
                        ceraList.L15UNAME = item.L15UNAME;
                        ceraList.L14L15 = item.L14L15;
                        ceraList.L16RELID = item.L16RELID;
                        ceraList.L16RELDT = item.L16RELDT;
                        ceraList.L16RELTM = item.L16RELTM;
                        ceraList.L16UNAME = item.L16UNAME;
                        ceraList.L15L16 = item.L15L16;
                        ceraList.L17RELID = item.L17RELID;
                        ceraList.L17RELDT = item.L17RELDT;
                        ceraList.L17RELTM = item.L17RELTM;
                        ceraList.L17UNAME = item.L17UNAME;
                        ceraList.L16L17 = item.L16L17;
                        ceraList.L18RELID = item.L18RELID;
                        ceraList.L18RELDT = item.L18RELDT;
                        ceraList.L18RELTM = item.L18RELTM;
                        ceraList.L18UNAME = item.L18UNAME;
                        ceraList.L17L18 = item.L17L18;
                        ceraList.L19RELID = item.L19RELID;
                        ceraList.L19RELDT = item.L19RELDT;
                        ceraList.L19RELTM = item.L19RELTM;
                        ceraList.L19UNAME = item.L19UNAME;
                        ceraList.L18L19 = item.L18L19;
                        ceraList.L20RELID = item.L20RELID;
                        ceraList.L20RELDT = item.L20RELDT;
                        ceraList.L20RELTM = item.L20RELTM;
                        ceraList.L20UNAME = item.L20UNAME;
                        ceraList.L19L20 = item.L19L20;
                        ceraList.ZASSIGN = item.ZASSIGN;
                        ceraList.ERNAM = item.ERNAM;
                        ceraList.ERDAT = item.ERDAT;
                        ceraList.ERZET = item.ERZET;
                        ceraList.AENAM = item.AENAM;
                        ceraList.AEDAT = item.AEDAT;
                        ceraList.AEZET = item.AEZET;
                        ceraList.FLAG = item.FLAG;
                        ceraList.INSERTDATE = DateTime.Now;
                        _ztmceraListRepository.Update(ceraList);
                        _ztmceraListRepository.Save();
                    }
                }

                log.Status = "success";
                log.Count = param.Count().ToString();
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return Ok();

            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("ZTUAGRI_HK")]
        //public IHttpActionResult ZTUAGRI_HK([FromBody] object value, int zyear = 0, string bkmnum = null, string client = null, string secretkey = null)
        //{
        //    string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
        //    // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    LogSAPToDB log = new LogSAPToDB();
        //    log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
        //    log.CreatedAt = DateTime.Now;
        //    log.ErrorMessage = string.Empty;
        //    log.Json = JsonConvert.SerializeObject(value);
        //    var JsonMessageError = string.Empty;
        //    log.ControllerAction = "SAPTODB/ZTUAGRI_HK";
        //    _logSAPToDBRepository.Insert(log);
        //    _logSAPToDBRepository.Save();

        //    try
        //    {
        //        if (secretkey == secretkeyConfig)
        //        {
        //            var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.ZTUAGRI_HK>>(value.ToString());
        //            log.Count = param.Count().ToString();
        //            var itemDel = _ztuagriHKRepo.FindAll().Where(a =>
        //                 a.BKMNUM == bkmnum &&
        //                 a.ZYEAR == zyear).FirstOrDefault();
        //            if (itemDel != null)
        //            {
        //                _ztuagriHKRepo.DeleteParam(itemDel);
        //                _ztuagriHKRepo.Save();
        //            }
                   
        //            foreach (var item in param)
        //            {
        //                JsonMessageError = JsonConvert.SerializeObject(item);
        //                var ztmHK = _ztuagriHKRepo.FindAll().Where(a =>
        //                    a.BKMNUM == bkmnum &&
        //                    a.ZYEAR == zyear &&
        //                    a.COMPANY_CODE == item.COMPANY_CODE &&
        //                    a.ESTATE == item.ESTATE &&
        //                    a.DIVISI == item.DIVISI).FirstOrDefault();
        //                item.Exec_Date = DateTime.Now;
        //                item.InsertDate = DateTime.Now;
        //               // item.Exec_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture);
        //                item.Exec_Time = DateTime.Now.ToLongTimeString();
        //                if (ztmHK == null)
        //                {
        //                    //item.CLIENT = client;
        //                    _ztuagriHKRepo.Insert(item);
        //                    _ztuagriHKRepo.Save();
        //                }
        //                else
        //                {
        //                    ztmHK.BKMNUM= item.BKMNUM;
        //                    ztmHK.ZYEAR = item.ZYEAR;
        //                    ztmHK.COMPANY_CODE = item.COMPANY_CODE;
        //                    ztmHK.ESTATE = item.ESTATE;
        //                    ztmHK.DIVISI = item.DIVISI;
        //                    ztmHK.MDRAN = item.MDRAN;
        //                    ztmHK.BKMDATE = item.BKMDATE;
        //                    ztmHK.WEEK = item.WEEK;
        //                    ztmHK.PERIOD = item.PERIOD;
        //                    ztmHK.MANDOR = item.MANDOR;
        //                    ztmHK.KRANI = item.KRANI;
        //                    ztmHK.STATUS = item.STATUS;
        //                    ztmHK.REVIEW = item.REVIEW;
        //                    ztmHK.SAPNUM = item.SAPNUM;
        //                    ztmHK.REFDOC = item.REFDOC;
        //                    ztmHK.UPLOAD = item.UPLOAD;
        //                    ztmHK.LINENUM = item.LINENUM;
        //                    ztmHK.EMPID = item.EMPID;
        //                    ztmHK.ACTID = item.ACTID;
        //                    ztmHK.LOCID = item.LOCID;
        //                    ztmHK.LOCCC = item.LOCCC;
        //                    ztmHK.HK = item.HK;
        //                    ztmHK.UOM_HK = item.UOM_HK;
        //                    ztmHK.OT = item.OT;
        //                    ztmHK.UOM_OT = item.UOM_OT;
        //                    ztmHK.HASIL = item.HASIL;
        //                    ztmHK.UOM = item.UOM;
        //                    ztmHK.REMARK = item.REMARK;
        //                    ztmHK.PREMI = item.PREMI;
        //                    ztmHK.PENALTI = item.PENALTI;
        //                    ztmHK.WAERS = item.WAERS;
        //                    ztmHK.CREATEDDATE = item.CREATEDDATE;
        //                    ztmHK.CREATEDBY = item.CREATEDBY;
        //                    ztmHK.LASTUPDATEDDATE = item.LASTUPDATEDDATE;
        //                    ztmHK.LASTUPDATEDBY = item.LASTUPDATEDBY;
        //                    ztmHK.ABSTYPE = item.ABSTYPE;
        //                    ztmHK.KOSTL = item.KOSTL;
        //                    ztmHK.JOBPOS = item.JOBPOS;
        //                    ztmHK.TYPE = item.TYPE;
        //                    ztmHK.LINENUMREF = item.LINENUMREF;
        //                    ztmHK.TYPEID = item.TYPEID;
        //                    ztmHK.LogId = item.LogId;
        //                    //ztmOrderPO.CLIENT = client;
        //                    ztmHK.InsertDate = DateTime.Now;
        //                    //ztmHK.CLIENT = client;
        //                    _ztuagriHKRepo.Insert(item);
        //                    _ztuagriHKRepo.Save();

        //                }
        //            }

        //            log.Status = "success";
        //            log.Count = param.Count().ToString();
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return Ok();
        //        }
        //        else
        //        {
        //            log.Status = "not_authorize";
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return BadRequest("Not Authorize.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Status = "error";
        //        log.ErrorMessage = ex.Message;
        //        log.JsonMessageError = JsonMessageError;
        //        _logSAPToDBRepository.Update(log);
        //        _logSAPToDBRepository.Save();
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("ZTUAGRI_HK")]
        public IHttpActionResult ZTUAGRI_HK([FromBody] object value, int zyear = 0, string bkmnum = null, string client = null, string secretkey = null)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZTUAGRI_HK";
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.ZTUAGRI_HK>>(value.ToString());
                    log.Count = param.Count().ToString();
                    var itemDel = _ztuagriHKRepo.FindAll().Where(a =>
                         a.BKMNUM == bkmnum &&
                         a.ZYEAR == zyear).ToList();
                    if (itemDel != null)
                    {
                        foreach (var dd in itemDel)
                        {
                            _ztuagriHKRepo.DeleteParam(dd);
                            _ztuagriHKRepo.Save();
                        }
                    }

                    foreach (var item in param)
                    {
                        item.InsertDate = DateTime.Now;
                        JsonMessageError = JsonConvert.SerializeObject(item);
                            //item.CLIENT = client;
                            _ztuagriHKRepo.Insert(item);
                            _ztuagriHKRepo.Save();
                      
                    }

                    log.Status = "success";
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("ZTUAGRI_BP")]
        //public IHttpActionResult ZTUAGRI_BP([FromBody] object value, int zyear = 0, string panennum = null, string client = null, string secretkey = null)
        //{
        //    string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
        //    // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    LogSAPToDB log = new LogSAPToDB();
        //    log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
        //    log.CreatedAt = DateTime.Now;
        //    log.ErrorMessage = string.Empty;
        //    log.Json = JsonConvert.SerializeObject(value);
        //    var JsonMessageError = string.Empty;
        //    log.ControllerAction = "SAPTODB/ZTUAGRI_BP";
        //    _logSAPToDBRepository.Insert(log);
        //    _logSAPToDBRepository.Save();

        //    try
        //    {
        //        if (secretkey == secretkeyConfig)
        //        {
        //            var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.ZTUAGRI_BP>>(value.ToString());
        //            log.Count = param.Count().ToString();
        //            var itemDel = _ztuagriBPRepo.FindAll().Where(a =>
        //             a.PANENNUM == panennum&&
        //             a.ZYEAR == zyear).FirstOrDefault();
            
        //            if (itemDel != null)
        //            {
        //                _ztuagriBPRepo.DeleteParam(itemDel);
        //                _ztuagriBPRepo.Save();
        //            }
        //            foreach (var item in param)
        //            {
        //                JsonMessageError = JsonConvert.SerializeObject(item);
        //                var ztmBP = _ztuagriBPRepo.FindAll().Where(a =>
        //                   a.ZYEAR == zyear &&
        //                     a.PANENNUM == panennum &&
        //                     a.COMPANY_CODE == item.COMPANY_CODE &&
        //                     a.ESTATE == item.ESTATE &&
        //                     a.BKMNUM == item.BKMNUM &&
        //                     a.LINENUM == item.LINENUM &&
        //                      a.BPDATE == item.BPDATE).FirstOrDefault();
        //                item.InsertDate = DateTime.Now;
        //                // item.Exec_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture);
        //                //item.Exec_Time = DateTime.Now.ToLongTimeString();
        //                if (ztmBP == null)
        //                {
        //                    //item.CLIENT = client;
        //                    _ztuagriBPRepo.Insert(item);
        //                    _ztuagriBPRepo.Save();
        //                }
        //                else
        //                {
        //                    //ztmOrderPO.CLIENT = client;
        //                    //_ztuagriBPRepo.Delete(ztmBP);
        //                    //_ztuagriBPRepo.Save();

        //                    ztmBP.InsertDate = DateTime.Now;
        //                    //ztmHK.CLIENT = client;
        //                    _ztuagriBPRepo.Insert(item);
        //                    _ztuagriBPRepo.Save();

        //                }
        //            }

        //            log.Status = "success";
        //            log.Count = param.Count().ToString();
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return Ok();
        //        }
        //        else
        //        {
        //            log.Status = "not_authorize";
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return BadRequest("Not Authorize.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Status = "error";
        //        log.ErrorMessage = ex.Message;
        //        log.JsonMessageError = JsonMessageError;
        //        _logSAPToDBRepository.Update(log);
        //        _logSAPToDBRepository.Save();
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("ZTU_PRCALCHV")]
        //public IHttpActionResult ZTU_PRCALCHV([FromBody] object value, string company_code = null, string estate = null, int zyear = 0, string bkmnum = null, string client = null, string secretkey = null)
        //{
        //    string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
        //    // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    LogSAPToDB log = new LogSAPToDB();
        //    log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
        //    log.CreatedAt = DateTime.Now;
        //    log.ErrorMessage = string.Empty;
        //    log.Json = JsonConvert.SerializeObject(value);
        //    var JsonMessageError = string.Empty;
        //    log.ControllerAction = "SAPTODB/ZTU_PRCALCHV";
        //    _logSAPToDBRepository.Insert(log);
        //    _logSAPToDBRepository.Save();

        //    try
        //    {
        //        if (secretkey == secretkeyConfig)
        //        {
        //            var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.ZTU_PRCALCHV>>(value.ToString());
        //            log.Count = param.Count().ToString();
        //            var itemDel = _ztuagriPRCALCHVRepo.FindAll().Where(a =>
        //           a.COMPANY == company_code &&
        //                     a.ESTATE == estate &&
        //                     a.ZYEAR == zyear &&
        //                     a.BKMNUM == bkmnum).FirstOrDefault();
        //            if (itemDel != null)
        //            {
        //                _ztuagriPRCALCHVRepo.DeleteParam(itemDel);
        //                _ztuagriPRCALCHVRepo.Save();
        //            }

        //            foreach (var item in param)
        //            {
        //                JsonMessageError = JsonConvert.SerializeObject(item);
        //                var ztmPRCALCHV = _ztuagriPRCALCHVRepo.FindAll().Where(a =>
        //                  a.COMPANY == company_code &&
        //                     a.ESTATE == estate &&
        //                     a.ZYEAR == zyear &&
        //                     a.BKMNUM == bkmnum &&
        //                     a.BKMDATE == item.BKMDATE &&
        //                     a.DIVISI == item.DIVISI &&
        //                      a.MDRAN == item.MDRAN &&
        //                     a.EMPID == item.EMPID &&
        //                     a.LINENUM == item.LINENUM &&
        //                     a.BLOCKID == item.BLOCKID &&
        //                     a.PREMITYP == item.PREMITYP &&
        //                     a.PRODTYPE == item.PRODTYPE).FirstOrDefault();
        //                item.INSERTDATE = DateTime.Now;
        //                // item.Exec_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture);
        //                //item.Exec_Time = DateTime.Now.ToLongTimeString();
        //                if (ztmPRCALCHV == null)
        //                {
        //                    //item.CLIENT = client;
        //                    _ztuagriPRCALCHVRepo.Insert(item);
        //                    _ztuagriPRCALCHVRepo.Save();
        //                }
        //                else
        //                {
        //                    //ztmOrderPO.CLIENT = client;
        //                    _ztuagriPRCALCHVRepo.Delete(ztmPRCALCHV);
        //                    _ztuagriPRCALCHVRepo.Save();

        //                  //  ztmBP.InsertDate = DateTime.Now;
        //                    //ztmHK.CLIENT = client;
        //                    _ztuagriPRCALCHVRepo.Insert(item);
        //                    _ztuagriPRCALCHVRepo.Save();

        //                }
        //            }

        //            log.Status = "success";
        //            log.Count = param.Count().ToString();
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return Ok();
        //        }
        //        else
        //        {
        //            log.Status = "not_authorize";
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return BadRequest("Not Authorize.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Status = "error";
        //        log.ErrorMessage = ex.Message;
        //        log.JsonMessageError = JsonMessageError;
        //        _logSAPToDBRepository.Update(log);
        //        _logSAPToDBRepository.Save();
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("ZTUAGRI_BP")]
        public IHttpActionResult ZTUAGRI_BP([FromBody] object value, int zyear = 0, string panennum = null, string client = null, string secretkey = null)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZTUAGRI_BP";
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.ZTUAGRI_BP>>(value.ToString());
                    log.Count = param.Count().ToString();
                    var itemDel = _ztuagriBPRepo.FindAll().Where(a =>
                     a.PANENNUM == panennum &&
                     a.ZYEAR == zyear).ToList();

                    if (itemDel != null)
                    {
                        foreach (var dd in itemDel)
                        {
                            _ztuagriBPRepo.DeleteParam(dd);
                            _ztuagriBPRepo.Save();
                        }
                    }
                    foreach (var item in param)
                    {
                           item.InsertDate = DateTime.Now;
                        JsonMessageError = JsonConvert.SerializeObject(item);
                            //item.CLIENT = client;
                            _ztuagriBPRepo.Insert(item);
                            _ztuagriBPRepo.Save();
                    }

                    log.Status = "success";
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ZTU_PRCALCHV")]
        public IHttpActionResult ZTU_PRCALCHV([FromBody] object value, string company_code = null, string estate = null, int zyear = 0, string bkmnum = null, string client = null, string secretkey = null)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZTU_PRCALCHV";
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.ZTU_PRCALCHV>>(value.ToString());
                    log.Count = param.Count().ToString();
                    var itemDel = _ztuagriPRCALCHVRepo.FindAll().Where(a =>
                   a.COMPANY == company_code &&
                             a.ESTATE == estate &&
                             a.ZYEAR == zyear &&
                             a.BKMNUM == bkmnum).ToList();
                    if (itemDel != null)
                    {
                        foreach (var dd in itemDel)
                        {
                            _ztuagriPRCALCHVRepo.DeleteParam(dd);
                            _ztuagriPRCALCHVRepo.Save();
                        }
                    }

                    foreach (var item in param)
                    {
                        item.INSERTDATE = DateTime.Now;
                        JsonMessageError = JsonConvert.SerializeObject(item);
                            //item.CLIENT = client;
                            _ztuagriPRCALCHVRepo.Insert(item);
                            _ztuagriPRCALCHVRepo.Save();
                    }

                    log.Status = "success";
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("ZTUAGRI_PBS2")]
        //public IHttpActionResult ZTUAGRI_PBS2([FromBody] object value, int zyear = 0, ZTstring spbsnum = null, string client = null, string secretkey = null)
        //{
        //    string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
        //    // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    LogSAPToDB log = new LogSAPToDB();
        //    log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
        //    log.CreatedAt = DateTime.Now;
        //    log.ErrorMessage = string.Empty;
        //    log.Json = JsonConvert.SerializeObject(value);
        //    string valueObject = JsonConvert.SerializeObject(value);
        //    var JsonMessageError = string.Empty;
        //    log.ControllerAction = "SAPTODB/ZTUAGRI_PBS2";
        //    _logSAPToDBRepository.Insert(log);
        //    _logSAPToDBRepository.Save();

        //    try
        //    {
        //        if (secretkey == secretkeyConfig)
        //        {

        //            var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.PBS2_Mark>>(valueObject);
        //            log.Count = param.Count().ToString();
        //            var itemDel = _ztuagriPBS2Repo.FindAll().Where(a =>
        //           a.ZYEAR == zyear &&
        //           a.SPBSNUM == spbsnum).FirstOrDefault();

        //            if (itemDel != null)
        //            {
        //                _ztuagriPBS2Repo.DeleteParam(itemDel);
        //                _ztuagriPBS2Repo.Save();
        //            }
        //            foreach (var item in param)
        //            {

        //                JsonMessageError = JsonConvert.SerializeObject(item);
        //                var ztmPBS2 = _ztuagriPBS2Repo.FindAll().Where(a =>
        //               a.ZYEAR == item.ZYEAR &&
        //                     a.SPBSNUM == item.SPBSNUM &&
        //                     a.COMPANY_CODE == item.COMPANY_CODE &&
        //                     a.ESTATE == item.ESTATE &&
        //                     a.LINENUM == item.LINENUM).FirstOrDefault();
        //                item.InsertDate = DateTime.Now;
        //                // item.Exec_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture);
        //                //item.Exec_Time = DateTime.Now.ToLongTimeString();
        //                if (ztmPBS2 == null)
        //                {
        //                    //item.CLIENT = client;

        //                    _ztuagriPBS2Repo.Insert(item);
        //                    _ztuagriPBS2Repo.Save();
        //                }
        //                else
        //                {
        //                    //ztmOrderPO.CLIENT = client;
        //                    //_ztuagriPBS2Repo.Delete(ztmPBS2);
        //                    //_ztuagriPBS2Repo.Save();

        //                    //  ztmBP.InsertDate = DateTime.Now;
        //                    //ztmHK.CLIENT = client;
        //                    _ztuagriPBS2Repo.Insert(item);
        //                    _ztuagriPBS2Repo.Save();

        //                }
        //            }

        //            log.Status = "success";
        //            log.Count = param.Count().ToString();
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return Ok();
        //        }
        //        else
        //        {
        //            log.Status = "not_authorize";
        //            _logSAPToDBRepository.Update(log);
        //            _logSAPToDBRepository.Save();
        //            return BadRequest("Not Authorize.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Status = "error";
        //        log.ErrorMessage = ex.Message;
        //        log.JsonMessageError = JsonMessageError;
        //        _logSAPToDBRepository.Update(log);
        //        _logSAPToDBRepository.Save();
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("ZTUAGRI_PBS2")]
        public IHttpActionResult ZTUAGRI_PBS2([FromBody] object value, int zyear = 0, string spbsnum = null, string client = null, string secretkey = null)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            string valueObject = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZTUAGRI_PBS2";
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {

                    var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.PBS2_Mark>>(valueObject);
                    log.Count = param.Count().ToString();
                    var itemDel = _ztuagriPBS2Repo.FindAll().Where(a =>
                   a.ZYEAR == zyear &&
                   a.SPBSNUM == spbsnum).ToList();

                    if (itemDel != null)
                    {
                        foreach (var dd in itemDel)
                        {
                            _ztuagriPBS2Repo.DeleteParam(dd);
                            _ztuagriPBS2Repo.Save();
                        }
                    }
                    foreach (var item in param)
                    {

                            JsonMessageError = JsonConvert.SerializeObject(item);
                            item.InsertDate = DateTime.Now;
                            _ztuagriPBS2Repo.Insert(item);
                            _ztuagriPBS2Repo.Save();
                    }

                    log.Status = "success";
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ZC0032")]
        public IHttpActionResult ZC0032([FromBody] object value, string gjahr = null, string monat = null, string werks = null, string type = null, string client = null, string secretkey = null)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZC0032";
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<ZC0032>>(value.ToString());
                    log.Count = param.Count().ToString();
                    var itemDel = _ZC0032_Prod.FindAll().Where(a =>
                               a.GJAHR == gjahr
                               && a.MONAT == monat
                                && a.WERKS == werks
                                 && a.TYPE == type).ToList();
                    if (itemDel != null)
                    {
                        foreach (var dd in itemDel)
                        {
                            _ZC0032_Prod.DeleteParam(dd);
                            _ZC0032_Prod.Save();
                        }
                    }

                    foreach (var item in param)
                    {
                        item.INSERTDATE = DateTime.Now;
                        JsonMessageError = JsonConvert.SerializeObject(item);
                        //item.CLIENT = client;
                        _ZC0032_Prod.Insert(item);
                        _ZC0032_Prod.Save();
                    }

                    log.Status = "success";
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("zmfmsendpoicat")]
        public IHttpActionResult zmfmsendpoicat([FromBody] object value, string ebeln = null, string frgke = null,int requestid = 0, string client = null, string secretkey = null)
        
       {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/ZMFMPOICAT";
            //_logSAPToDBRepository.Insert(log);
            //_logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var set = new Result();
                    if (requestid == 0 )
                    {
                        var param = JsonConvert.DeserializeObject<PURCHASEORDER_HEADER_MAPING>(value.ToString());
                        log.Count = "1";
                        if (param.FRGKE.Equals("4"))
                        {
                            var itemDelRequestItem = _poicatrequestitem.FindAll().Where(a =>
                                     a.prd_PO_Number == ebeln).ToList();
                            if (itemDelRequestItem.Count > 0)
                            {
                                foreach (var item in itemDelRequestItem)
                                {
                                    _poicatrequestitem.DeleteParam(item);
                                    _poicatrequestitem.Save();
                                    var itemDelRequestHeader = _poicatrequest.FindAll().Where(a =>
                                    a.ID == item.prd_PurchaseID).FirstOrDefault();
                                    if (itemDelRequestHeader != null)
                                    {
                                        _poicatrequest.DeleteParam(itemDelRequestHeader);
                                        _poicatrequest.Save();
                                    }

                                }

                            }
                            var itemDel = _poicatheader.FindAll().Where(a =>a.PO_Number == ebeln).FirstOrDefault();
                            if (itemDel != null)
                            {
                                _poicatheader.DeleteParam(itemDel);
                                _poicatheader.Save();
                            }
                            set = new Result()
                            {
                                message = "berhasil",
                                data = "",
                                ID = -1,
                            };
                        }
                        else
                        {
                            var itemDelRequestItem = _poicatrequestitem.FindAll().Where(a =>
                                       a.prd_PO_Number == ebeln).ToList();
                            if (itemDelRequestItem.Count > 0)
                            {
                                foreach (var item in itemDelRequestItem)
                                {
                                    _poicatrequestitem.DeleteParam(item);
                                    _poicatrequestitem.Save();
                                    var itemDelRequestHeader = _poicatrequest.FindAll().Where(a =>
                                    a.ID == item.prd_PurchaseID).FirstOrDefault();
                                    if (itemDelRequestHeader != null)
                                    {
                                        _poicatrequest.DeleteParam(itemDelRequestHeader);
                                        _poicatrequest.Save();
                                    }

                                }

                            }
                            int InsertRequest = _poicatrequest.ToInsert(param);
                            if (InsertRequest != null)
                            {

                                var itemDel = _poicatheader.FindAll().Where(a =>
                                       a.PO_Number == ebeln).FirstOrDefault();
                                if (itemDel != null)
                                {
                                    _poicatheader.DeleteParam(itemDel);
                                    _poicatheader.Save();
                                }
                                _poicatheader.Insert(param, InsertRequest);
                                _poicatheader.Save();
                            }
                            set = new Result()
                            {
                                message = "berhasil",
                                data = "",
                                ID = InsertRequest,
                            };
                        }
                    }else {
                        var param = JsonConvert.DeserializeObject<List<PURCHASEORDER_DETAIL_MAPING>>(value.ToString());
                        log.Count = "1";
                        var itemDelPOItem = _poicatitem.FindAll().Where(a =>
                            a.PO_Number == ebeln).ToList();
                        var itemDelRequestItem = _poicatrequestitem.FindAll().Where(a =>
                            a.prd_PO_Number == ebeln).ToList();
                        if (itemDelPOItem.Count > 0)
                        {
                            foreach (var item in itemDelPOItem)
                            {
                                _poicatitem.DeleteParam(item);
                                _poicatitem.Save();
                            }
                        }
                        if (itemDelRequestItem.Count > 0)
                        {
                            foreach (var item in itemDelRequestItem)
                            {
                                _poicatrequestitem.DeleteParam(item);
                                _poicatrequestitem.Save();
                            }
                        }
                        foreach (var item in param)
                        {
                            if (!frgke.Equals("4"))
                            {
                                _poicatrequest.Update(item, requestid);
                                _poicatrequest.Save();
                                _poicatrequestitem.Insert(item, requestid);
                                _poicatrequestitem.Save();
                                _poicatitem.Insert(item, requestid);
                                _poicatitem.Save();
                                _poicatheader.Update(item, requestid);
                                _poicatheader.Save();
                            }
                                //_poicatrequisition.Update(item, requestid);
                                //_poicatrequisition.Save();
                        }
                        if (!frgke.Equals("4"))
                        {
                            var result = _poicatheader.FindAll().Where(a =>a.PurchaseID == requestid).FirstOrDefault();
                            set = new Result()
                            {
                                message = "Pengiriman Berhasil",
                                data = result.PO_Number,
                                ID = requestid,
                            };
                        }
                        else
                        {
                            set = new Result()
                            {
                                message = "Pengiriman Berhasil",
                                data = "",
                                ID = requestid,
                            };
                            
                        }

                    }
                    log.Status = "success";
                    log.Count = "1";
                    //_logSAPToDBRepository.Update(log);
                    //_logSAPToDBRepository.Save();
                    return Ok(set);
                }
                else
                {
                    log.Status = "not_authorize";
                    //_logSAPToDBRepository.Update(log);
                    //_logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (DbEntityValidationException ex)
            {
                 string errorMessage = null;
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        // get the error message 
                        errorMessage = validationError.ErrorMessage;
                      
                    }
                }
                log.Status = "error";
                log.ErrorMessage = errorMessage;
                log.JsonMessageError = JsonMessageError;
                //_logSAPToDBRepository.Update(log);
                //_logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                //Console.WriteLine(ex.InnerException);
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                //_logSAPToDBRepository.Update(log);
                //_logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("LOGBOOK")]
        public IHttpActionResult LOGBOOK([FromBody] object value, int zyear = 0, string estate = null, string lbknum = null, string client = null, string secretkey = null)
        {
            string secretkeyConfig = System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LogSAPToDB log = new LogSAPToDB();
            log.ApplicationId = EnumApplicationID.ApplicationID.DataStock.ToString();
            log.CreatedAt = DateTime.Now;
            log.ErrorMessage = string.Empty;
            log.Json = JsonConvert.SerializeObject(value);
            var JsonMessageError = string.Empty;
            log.ControllerAction = "SAPTODB/LOGBOOK";
            _logSAPToDBRepository.Insert(log);
            _logSAPToDBRepository.Save();

            try
            {
                if (secretkey == secretkeyConfig)
                {
                    var param = JsonConvert.DeserializeObject<List<Indoagri.IndoagriIACData.Model.Models.LB_mark>>(value.ToString());
                    log.Count = param.Count().ToString();
                    var itemDel = _logbookRepo.FindAll().Where(a =>
                                a.ESTATE == estate && a.LBKNUM == lbknum && a.ZYEAR == zyear).ToList();
                    if (itemDel != null)
                    {
                        foreach (var dd in itemDel)
                        {
                            _logbookRepo.DeleteParam(dd);
                            _logbookRepo.Save();
                        }
                    }

                    foreach (var item in param)
                    {
                        item.InsertDate = DateTime.Now;
                        JsonMessageError = JsonConvert.SerializeObject(item);
                        //item.CLIENT = client;
                        _logbookRepo.Insert(item);
                        _logbookRepo.Save();
                    }

                    log.Status = "success";
                    log.Count = param.Count().ToString();
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return Ok();
                }
                else
                {
                    log.Status = "not_authorize";
                    _logSAPToDBRepository.Update(log);
                    _logSAPToDBRepository.Save();
                    return BadRequest("Not Authorize.");
                }
            }
            catch (Exception ex)
            {
                log.Status = "error";
                log.ErrorMessage = ex.Message;
                log.JsonMessageError = JsonMessageError;
                _logSAPToDBRepository.Update(log);
                _logSAPToDBRepository.Save();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getHRK")]
        public IHttpActionResult GetHK()
        {
            var datas = _ztuagriHKRepo.FindAll().FirstOrDefault();
            return Ok(datas);
        }

        private string Left(string value, int length)
        {
            string result = value.Substring(0, length);
            return result;
        }
        private string Right(string value, int length)
        {
            string result = value.Substring(value.Length - length, length);
            return result;
        }

        //public static void sendEmail(string to, string cc, string bcc, string template, string Subject)
        //{
        //    try
        //    {
        //        Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(BaseConstants.BASE64_ICAT_LOGO));
        //        System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
        //        to = to.Replace(",", ";");
        //        using (SmtpClient client = new SmtpClient("10.0.0.1", 5432))
        //        {
        //            MailMessage newMail = new MailMessage();
        //            string DefaultEmailTo = ParamHelper.DefaultEmailTo;
        //            bool ActiveNotification = ParamHelper.ActiveNotification;
        //            newMail.From = new MailAddress(ParamHelper.DefaultEmailFrom);
        //            List<string> lstBlockEmail = ParamHelper.GetEmailBlock();
        //            if (ActiveNotification)
        //            {
        //                foreach (string temp in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
        //                {
        //                    if (!lstBlockEmail.Any(temp.Contains))
        //                    {
        //                        if (!newMail.To.Any(s => s.Address == temp))
        //                        {
        //                            newMail.To.Add(temp);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                newMail.To.Add(DefaultEmailTo);
        //            }

        //            foreach (string itemCC in ParamHelper.GetEmailCC())
        //            {
        //                if (!lstBlockEmail.Any(itemCC.Contains))
        //                {
        //                    if (itemCC.ToLower() != DefaultEmailTo.ToLower())
        //                    {
        //                        newMail.Bcc.Add(itemCC);
        //                    }
        //                }
        //            }

        //            newMail.Subject = Subject;
        //            newMail.IsBodyHtml = true;
        //            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(template, null, MediaTypeNames.Text.Html);
        //            var imageToInline = new LinkedResource(streamBitmap, MediaTypeNames.Image.Jpeg);
        //            imageToInline.ContentId = "MyImage";
        //            alternateView.LinkedResources.Add(imageToInline);
        //            newMail.AlternateViews.Add(alternateView);

        //            client.Send(newMail);
        //            client.Dispose();
        //            newMail = null;
        //            GC.Collect();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string ErrMess = ex.Message;
        //    }
        //}


        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }
    }



}
