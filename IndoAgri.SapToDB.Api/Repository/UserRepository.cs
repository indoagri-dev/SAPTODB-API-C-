using IMobileAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace IMobileAPI.Repository
{
    public class UserRepository
    {
        private Context_MobileAgri _dbAgri = null;
        public UserRepository()
        {
            _dbAgri = new Context_MobileAgri();
        }

        // FIND //

        public string GetIP()
        {
            string ipAddress = "null";
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            return ipAddress = Convert.ToString(ipHostInfo.AddressList.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork));
        }

        public Account_UsersModel FindUser(Account_UsersModel user)
        {
            var result = (from a in _dbAgri.Users where a.userName == user.userName select a).FirstOrDefault();
            if (result != null)
            {
                result = (from a in _dbAgri.Users where a.userName == user.userName select a).FirstOrDefault();
            }

            return result;
        }
     
        public List<Account_UsersModel> FindUserList(Account_UsersModel user)
        {
            var result = (from a in _dbAgri.Users where a.userName == user.userName select a).ToList();
            if (result != null)
            {
                result = (from a in _dbAgri.Users where a.userName == user.userName select a).ToList();
            }

            return result;
        }
        public Account_UsersNotification FindUserNotification(Account_UsersNotification userData)
        {
            var result = (from a in _dbAgri.UserNotification where a.UserID == userData.UserID && a.Package == userData.Package select a).FirstOrDefault();
            if (result != null)
            {
                result = (from a in _dbAgri.UserNotification where a.UserID == userData.UserID && a.Package == userData.Package select a).FirstOrDefault();
            }

            return result;
        }

        public List<Account_UsersNotification> SendFindUserNotificationGPS(Account_UsersNotification userData)
        {
            DateTime dt1 = DateTime.Now;
            DateTime dt2 = dt1.AddMinutes(+15);
            var result = (from a in _dbAgri.UserNotification where a.UserCode == userData.UserCode && a.Activate == 1 && a.Package == userData.Package && (a.NotifyDate <= dt1 || a.NotifyDate >= dt2) select a).ToList();
            if (result != null)
            {
                result = (from a in _dbAgri.UserNotification where a.UserCode == userData.UserCode && a.Activate == 1 && a.Package == userData.Package && (a.NotifyDate <= dt1 || a.NotifyDate >= dt2) select a).ToList();
            }

            return result;
        }

        public List<UsersActivities> SendFindUserNotificationCDV(UsersActivities Datas)
        {
            var result = (from a in _dbAgri.UserActivities where a.userSAP == Datas.userSAP && a.Package == Datas.Package && a.LoggedIn == Datas.LoggedIn select a).ToList();
            var Datass = result
                              .GroupBy(l => l.UserCode)
                              .Select(i =>
                                  new UsersActivities()
                                  {
                                      UserCode = i.Key,
                                      userName = i.First().userName,
                                      TokenFireBase = i.First().TokenFireBase
                                  }
                              ).ToList();
            if (Datass != null)
            {
               Datass = result
                                 .GroupBy(l => l.UserCode)
                                 .Select(i =>
                                     new UsersActivities()
                                     {
                                         UserCode = i.Key,
                                         TokenFireBase = i.First().TokenFireBase
                                     }
                                 ).ToList();
                //result = (from a in _dbAgri.UserActivities where a.userSAP == Datas.userSAP && a.Package == Datas.Package && a.LoggedIn == Datas.LoggedIn select a).ToList();
            }

            return Datass;
        }

        public List<UsersActivities> SendFindUserNotificationHotspot(UsersActivities Datas)
        {
            var result = (from a in _dbAgri.UserActivities where a.KodeAreaHS == Datas.KodeAreaHS && a.Package == Datas.Package && a.LoggedIn == Datas.LoggedIn select a).ToList();
            if (result != null)
            {
                result = (from a in _dbAgri.UserActivities where a.KodeAreaHS == Datas.KodeAreaHS && a.Package == Datas.Package && a.LoggedIn == Datas.LoggedIn select a).ToList();
            }

            return result;
        }
        public List<Account_UsersNotification> TesSendFindUserNotification(Account_UsersNotification userData)
        {
            var result = (from a in _dbAgri.UserNotification where a.Activate == 1 && a.Package == userData.Package select a).ToList();
            if (result != null)
            {
                result = (from a in _dbAgri.UserNotification where a.Activate == 1 && a.Package == userData.Package select a).ToList();
            }

            return result;
        }


        public Account_UsersModel FindUserLog(string username)
        {
            var result = (from a in _dbAgri.Users where a.userName == username select a).FirstOrDefault();
            if (result != null)
            {
                result = (from a in _dbAgri.Users where a.userName == username select a).FirstOrDefault();
            }

            return result;
        }
        public UsersActivities FindUsersActivitie(string token)
        {
            var result = (from a in _dbAgri.UserActivities where a.TokenFireBase == token select a).FirstOrDefault();
            if (result != null)
            {
                result = (from a in _dbAgri.UserActivities where a.TokenFireBase == token select a).FirstOrDefault();
            }

            return result;
        }
        public List<UsersActivities> FindUsersActivitiesList(string deviceID)
        {
            var result = (from a in _dbAgri.UserActivities where a.userName == deviceID select a).ToList();
            if (result != null)
            {
                result = (from a in _dbAgri.UserActivities where a.userName == deviceID select a).ToList();
            }

            return result;
        }


        // INSERT // 
        public void InsertUserNotification(Account_UsersNotification userData)
        {
            _dbAgri.UserNotification.Add(userData);
        }


        // UPDATE //
        public void UpdateUserTime(Account_UsersModel userData)
        {
            var DATAS = (from a in _dbAgri.Users where a.userName == userData.userName select a).Any();
            if (DATAS)
            {
                var updateStatus = (from p in _dbAgri.Users where p.userName == userData.userName select p);
                foreach (var entity in updateStatus)
                {

                        entity.userEmail = userData.userEmail.ToLower();
                        entity.userSurname = userData.userSurname;
                        entity.userDescription = userData.userDescription;
                        entity.userProvince = userData.userProvince;
                        entity.userName = userData.userName;
                        entity.Domain = userData.Domain;
                        entity.Company = userData.Company;
                        entity.Department = userData.Department;
                        entity.EmployeeID = userData.EmployeeID;
                        entity.Fullname = userData.Fullname;
                        entity.EmailLDAP = userData.EmailLDAP.ToLower();
                        entity.userLevel = userData.userLevel;
                        entity.DeviceID = userData.DeviceID;
                        entity.updatedAt = DateTime.Now;
                        entity.userSAP = userData.userSAP;
                        entity.LoggedIn = userData.LoggedIn;
                        entity.myIP = GetIP();
                }
                try
                {
                    _dbAgri.SaveChanges();
                }
                catch (Exception e)
                {
                    
                }
            }
            else
            {
                _dbAgri.Users.Add(userData);
                _dbAgri.SaveChanges();
            }
        }
        public void UpdateNotification(Account_UsersNotification userData, Account_UsersNotification UpdateData)
        {
            var result = _dbAgri.UserNotification.SingleOrDefault(b => b.UserID == userData.UserID && b.Package == userData.Package);
            if (result != null)
            {
                result.UserCode = UpdateData.UserCode;
                result.TokenFireBase = UpdateData.TokenFireBase;
                result.Activate = UpdateData.Activate;
                _dbAgri.SaveChanges();
            }

        }

        
        public void UpdateStatusPush(string userData)
        {
            DateTime dt = DateTime.Now;
            var result = _dbAgri.UserNotification.SingleOrDefault(b => b.TokenFireBase == userData);
            if (result != null)
            {
                result.NotifyDate = dt;
                result.IpAddress = GetIP();
                _dbAgri.SaveChanges();
            }


        }

      
        public void Save()
        {
            _dbAgri.SaveChanges();
        }

    }
}