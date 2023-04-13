using Indoagri.IndoagriPOICAT.Model.Models;
using IndoAgri.SapToDB.Api.Emails;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;
namespace Indoagri.IndoagriPOICAT.Model.Repositories
{
    public class PurchaseOrderHeaderRepositories
    {
        private DB_POICATEntities _db;
        public PurchaseOrderHeaderRepositories()
        {
            try
            {
                _db = new DB_POICATEntities();
            }
            catch (Exception)
            {
                _db = new DB_POICATEntities();
            }
        }

        public IQueryable<PurchaseOrderHeader> FindAll()
        {
            return from a in _db.PurchaseOrderHeaders select a;
        }

        public void Insert(PURCHASEORDER_HEADER_MAPING po, int ID)
        {
            bool ISPKP = false;
            if (po.ISPKP== "1")
            {
                ISPKP = true;
            }
            else
            {
                ISPKP = false;
            }
            var crid = System.Guid.NewGuid();
            var t = new PurchaseOrderHeader//Make sure you have a table called test in DB
            {
                ID = crid,
                PO_Number = po.EBELN,
                PO_Type = po.BSART,
                EKGRP = po.EKGRP,
                PurchaseID = ID,
                VendorCode = po.LIFNR,
                ActionWERKS = null,
                WERKS = "0",
                Area_ID = null,
                DeliveryAddWERKS = null,
                DeliveryAddrNUMBER = null,
                InvoiceAddrWERKS = null,
                InvoiceAddrNUMBER = null,
                UPDeliveryAddress = null,
                Incoterm = null,
                Status = "New Order",
                StatusId = 1,
                IsPKP = ISPKP,
                UserOrderId = null,
                CreatedOn = DateTime.Now,
                CreatedBy = 1,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 1,
                DeletedOn = null
            };
            _db.PurchaseOrderHeaders.Add(t);
        }

        public void Update(PURCHASEORDER_DETAIL_MAPING po, int requestid)
        {
            var itemRequest = _db.PurchaseRequests.Where(a =>
                                 a.ID == requestid).FirstOrDefault();
            var itemHeader = _db.PurchaseOrderHeaders.Where(a =>
                                a.PurchaseID == requestid).FirstOrDefault();
            itemHeader.WERKS = po.WERKS;
            itemHeader.ModifiedOn = DateTime.Now;
            var hkData = (from a in _db.PurchaseOrderHeaders where a.PurchaseID == requestid select a).Any();
            if (hkData)
            {
                _db.PurchaseOrderHeaders.Attach(itemHeader);
                _db.Entry(itemHeader).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(PurchaseOrderHeader item)
        {
            var asset = (from a in _db.PurchaseOrderHeaders
                         where
                             a.PO_Number == item.PO_Number
                         select a).Any();
            if (asset)
            {
                _db.PurchaseOrderHeaders.Remove(item);
            }
        }

        public void DeleteParam(PurchaseOrderHeader item)
        {
            var asset = (from a in _db.PurchaseOrderHeaders
                         where
                             a.PO_Number == item.PO_Number 
                         select a).Any();
            if (asset)
            {
                _db.PurchaseOrderHeaders.Remove(item);
                //_db.SaveChanges();
            }
        }

      public void Save()
        {
            _db.SaveChanges();
        }

      public void ReleasePOSendMail(string POCode, string Url)
          {
              try
              {
                  decimal TotalPrice = 0;
                  string werks = "";
                  string addrNumb = "";
                  string vendorid = "";
                  int prID = 0;
                  string cur = "";

                  var poData = (from a in _db.PurchaseOrderHeaders
                                join b in _db.PurchaseOrderDetails on a.ID equals b.POHeader_ID
                                where a.PO_Number == POCode
                                select new
                                {
                                    qty = b.Qty,
                                    nettprice = b.SalePrice,
                                    werks = b.deliveryAddrWERKS,
                                    addrNumber = b.deliveryAddrADDRNUMBER,
                                    vendorid = b.VendorCode,
                                    prID = b.PurchaseID,
                                    cur = b.Currency
                                }).ToList();

                  string POType = _db.PurchaseOrderHeaders.Where(x => x.PO_Number == POCode).Select(x => x.PO_Type).FirstOrDefault();

                  foreach (var item in poData)
                  {
                      TotalPrice += item.qty * item.nettprice.Value;
                      vendorid = item.vendorid;
                      werks = item.werks;
                      addrNumb = item.addrNumber;
                      prID = item.prID;
                      cur = item.cur;
                  }

                  if (POType == "NB")
                  {
                      VendorData vend = _db.VendorDatas.Where(x => x.SAPVendorCode == vendorid).FirstOrDefault();
                      if (vend != null)
                      {
                          if (vend.IsPKP)
                          {
                              TotalPrice = ((TotalPrice * 110) / 100);
                          }
                      }

                      var PlantArea = (from a in _db.MasterPlantAreas
                                       join b in _db.MasterCompanies on a.BUKRS equals b.BUKRS
                                       where a.WERKS == werks
                                       select new
                                       {
                                           a.AREA,
                                           b.BUKRS,
                                           b.BUTXT
                                       }
                                       ).FirstOrDefault();
                      string plantName = _db.MasterPlants.Where(x => x.WERKS == werks).Select(x => x.NAME1).FirstOrDefault();

                      var req = (from a in _db.PurchaseRequestDetails
                                 join b in _db.PurchaseRequests on a.prd_PurchaseID equals b.ID
                                 where a.prd_PO_Number == POCode
                                 select new { b.Approval, b.CreatedBy }).FirstOrDefault();

                      var purchaseRequestDetails = (from a in _db.PurchaseRequestDetails
                                                    join b in _db.PurchaseRequests on a.prd_PurchaseID equals b.ID
                                                    where a.prd_PO_Number == POCode
                                                    select new { a.prd_UserId }).Distinct().ToList();
                      MasterDeliveryAddress deliveryAddress = _db.MasterDeliveryAddresses.Where(x => x.WERKS == werks && x.ADDRNUMBER == addrNumb).FirstOrDefault();

                      var emailUsers = string.Empty;
                      for (int i = 0; i < purchaseRequestDetails.Count(); i++)
                      {
                          if (i == 0)
                          {
                              var emailUser = SendEmail.userEmail(Convert.ToInt32(purchaseRequestDetails[i].prd_UserId));
                              emailUsers = emailUser;
                          }
                          else if (i > 0)
                          {
                              emailUsers = emailUsers + ";" + SendEmail.userEmail(Convert.ToInt32(purchaseRequestDetails[i].prd_UserId));
                          }
                      }

                      string appremail = SendEmail.userEmail(req.Approval.Value);
                      string reqemail = SendEmail.userEmail(req.CreatedBy);

                      TemplateNotifEmail template = SendEmail.getEmailTemplate("UserPORelease", "PORelease");
                      string strTemplate = template.tpl_Template;

                      string email = appremail + ";" + reqemail + ";" + emailUsers;

                      string Address = "";
                      if (!string.IsNullOrWhiteSpace(deliveryAddress.STREET))
                      {
                          Address += deliveryAddress.STREET;
                      }
                      if (!string.IsNullOrWhiteSpace(deliveryAddress.CITY1))
                      {
                          Address += " " + deliveryAddress.CITY1;
                      }
                      if (!string.IsNullOrWhiteSpace(deliveryAddress.POST_CODE1))
                      {
                          Address += " " + deliveryAddress.POST_CODE1;
                      }
                      if (!string.IsNullOrWhiteSpace(deliveryAddress.TEL_NUMBER))
                      {
                          Address += " Telp: " + deliveryAddress.TEL_NUMBER;
                      }

                      strTemplate = strTemplate.Replace("@CompanyName@", PlantArea.BUKRS + " - " + PlantArea.BUTXT);
                      strTemplate = strTemplate.Replace("@PlantName@", werks + " - " + plantName);
                      strTemplate = strTemplate.Replace("@VendorCode@", vend.SAPVendorCode);
                      strTemplate = strTemplate.Replace("@VendorName@", vend.VendorName);
                      strTemplate = strTemplate.Replace("@PONumber@", POCode);
                      strTemplate = strTemplate.Replace("@TotalPO@", cur + " " + TotalPrice.ToString("n0", CultureInfo.CreateSpecificCulture("id-IDR")));
                      strTemplate = strTemplate.Replace("@AlamatDeliveryName@", deliveryAddress.NAME1);
                      strTemplate = strTemplate.Replace("@AlamatDeliveryStreet@", Address);
                      strTemplate = strTemplate.Replace("@DetailPO@", Url);

                      var subjectEmailInternal = template.tpl_Subject + " - " + POCode + " - " + plantName + " - " + vend.SAPVendorCode + " - " + vend.VendorName;
                      SendEmail.sendEmail(email, "", "", strTemplate, subjectEmailInternal);

                      string vendemail = vend.Email;
                      if (!string.IsNullOrWhiteSpace(vend.EmailCC))
                      {
                          vendemail += ";" + vend.EmailCC;
                      }
                      if (!string.IsNullOrWhiteSpace(vend.EmailCC2))
                      {
                          vendemail += ";" + vend.EmailCC2;
                      }
                      TemplateNotifEmail vendtemplate = SendEmail.getEmailTemplate("VendorPORelease", "PORelease");
                      string strVendTemplate = vendtemplate.tpl_Template;
                      strVendTemplate = WebUtility.HtmlDecode(strVendTemplate);
                      strVendTemplate = strVendTemplate.Replace("@VendorName@", PlantArea.BUTXT);
                      strVendTemplate = strVendTemplate.Replace("@PONumber@", POCode);
                      strVendTemplate = strVendTemplate.Replace("@TotalPO@", cur + " " + TotalPrice.ToString("n0", CultureInfo.CreateSpecificCulture("id-IDR")));
                      strVendTemplate = strVendTemplate.Replace("@Area@", PlantArea.AREA);
                      strVendTemplate = strVendTemplate.Replace("@AlamatDeliveryName@", deliveryAddress.NAME1);
                      strVendTemplate = strVendTemplate.Replace("@AlamatDeliveryStreet@", Address);
                      strVendTemplate = strVendTemplate.Replace("@DetailPesanan@", Url);

                      var subjectEmailVendor = vendtemplate.tpl_Subject + " - " + POCode + " - " + plantName;
                      SendEmail.sendEmail(vendemail, "", "", strVendTemplate, subjectEmailVendor);
                  }
              }
              catch (Exception ex) {}
          }
    }
}
