using ERPConnect;
using IndoAgri.IndoAgriData.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoAgri.IndoAgriData.Model.Models
{
    public class SAPService
    {
        LogPostDataRepository _logPostDataRepository;
        public SAPService()
        {
            _logPostDataRepository = new LogPostDataRepository();
        }

        public bool IsAuthotizeSAP(string userName, string password, out List<UserProfileSAP>  userProfileSAPs)
        {
            userProfileSAPs = new List<UserProfileSAP>();
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

                UserProfileSAP objDummy = new UserProfileSAP();
                result = s.ZuagriGsberDefaultEmpty(param);
                foreach (ServiceAuthSAP.ZustAgriAuthorization item in result.Result)
                {
                    objDummy.CompanyCode = item.Bukrs;
                    objDummy.BACode = item.Gsber;
                    objDummy.Activity = item.Activity;
                    objDummy.Division = item.Divisi;
                    objDummy.TCode = item.Tcode;
                    userProfileSAPs.Add(objDummy);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
                #endregion

            if (userProfileSAPs.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PostApprovalCDV(string NRCDV, string usernameSAP, string passwordSAP)
        {
            var isSuccess = false;
            using (ERPConnect.R3Connection r3con = SAPConnection.R3Connection(usernameSAP, passwordSAP))
            {
                r3con.Open();
                RFCFunction func = r3con.CreateFunction("ZFFM_CDV_GET_DATA_EMAIL");
                func.Exports["NRCDV"].ParamValue = NRCDV;
                RFCTable tableReturn = func.Tables["RETURN"];
              
                func.Execute();

                if (tableReturn.Rows.Count > 0)
                {
                    isSuccess = false;
                    for (int i = 0; i < tableReturn.Rows.Count; i++)
                    {
                        LogPostData log = new LogPostData();
                        log.ApplicationId = EnumApplicationID.ApplicationID.AssetStat.ToString();
                        log.CreatedAt = DateTime.Now;
                        log.Json = NRCDV;
                        log.ControllerAction = "CDV/PostApproval";
                       
                        log.ErrorMessage = tableReturn.Rows[i][0].ToString();
                        log.Status = "error";
                        _logPostDataRepository.Insert(log);
                        _logPostDataRepository.Save();
                    }
                }
                else
                {
                    LogPostData log = new LogPostData();
                    log.ApplicationId = EnumApplicationID.ApplicationID.AssetStat.ToString();
                    log.CreatedAt = DateTime.Now;
                    log.Json = NRCDV;
                    log.ControllerAction = "CDV/PostApproval";
                    //log.ErrorMessage = fingerTableReturn.Rows[i][0].ToString();
                    log.Status = "success";
                    _logPostDataRepository.Insert(log);
                    _logPostDataRepository.Save();
                }
            }
        }
    }
}
