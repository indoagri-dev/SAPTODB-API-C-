using Indoagri.IndoagriPOICAT.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriPOICAT.Model.Repositories
{
    public class PurchaseRequestRepositories
    {
        private DB_POICATEntities _db;
        public PurchaseRequestRepositories()
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

        public IQueryable<PurchaseRequest> FindAll()
        {
            return from a in _db.PurchaseRequests select a;
        }

        public void Insert(PURCHASEORDER_HEADER_MAPING po)
        {
            var t = new PurchaseRequest()
            {

            };
            _db.PurchaseRequests.Add(t);
        }
        public int ToInsert(PURCHASEORDER_HEADER_MAPING po)
        {

            //var seqNumber = _db.Database.SqlQuery<long>("SELECT NEXT VALUE FOR SequencePR").FirstOrDefault();
            //var seqString = seqNumber.ToString().PadLeft(10, '0');
            //var code = "PR" + seqString;
            var createdby = Convert.ToInt32(1);
            var code = GetSequencePR("R", DateTime.Now.Year, DateTime.Now.Month);
            var t = new PurchaseRequest()
            {
                Code = code,
                WERKS = "0",
                VendorCode = po.LIFNR,
                CityID = null,
                Approval = 1, //2989 diganti 1
                Status = "Approved",
                Stage = 4,
                Token = "SAP",
                TokenCreatedOn = null,
                ClosedComment = null,
                ClosedDate = null,
                ClosedBy = null,
                CreatedOn = DateTime.Now,
                CreatedBy = createdby,
                ModifiedOn = DateTime.Now,
                ModifiedBy = createdby,
                DeletedOn = null,
                SiteManagerComment = null
            };

            _db.PurchaseRequests.Add(t);
            _db.SaveChanges();
            return t.ID;
        }

        public void Update(PURCHASEORDER_DETAIL_MAPING po, int PurchaseID)
        {
            var itemRequest = _db.PurchaseRequests.Where(a =>
                                 a.ID == PurchaseID).FirstOrDefault();
            itemRequest.WERKS = po.WERKS;
            itemRequest.ModifiedOn = DateTime.Now;
            var hkData = (from a in _db.PurchaseRequests where a.ID == PurchaseID select a).Any();
            if (hkData)
            {
                _db.PurchaseRequests.Attach(itemRequest);
                _db.Entry(itemRequest).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(PurchaseRequest item)
        {
            var asset = (from a in _db.PurchaseRequests
                         where
                             a.ID == item.ID &&
                             a.Code == item.Code
                         select a).Any();
            if (asset)
            {
                _db.PurchaseRequests.Remove(item);
            }
        }

        public void DeleteParam(PurchaseRequest item)
        {
            var asset = (from a in _db.PurchaseRequests
                         where
                               a.ID == item.ID &&
                             a.Code == item.Code
                         select a).Any();
            if (asset)
            {
                _db.PurchaseRequests.Remove(item);
                //_db.SaveChanges();
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }


        public object ToInsert(List<PURCHASEORDER_HEADER_MAPING> param)
        {
            throw new NotImplementedException();
        }
        public string GetSequencePR(string code, int year, int month)
        {

            RunningNumber rn = (from f in _db.RunningNumbers
                                where f.rn_FormCode == code && f.rn_Year == year && f.rn_Month == month
                                select f).FirstOrDefault();
            if (rn == null)
            {
                var alterSequencePR = @"ALTER SEQUENCE [dbo].[SequencePR]
                                        RESTART WITH 1
                                        INCREMENT BY 1
                                        MINVALUE 1
                                        MAXVALUE 9223372036854775807
                                        NO CYCLE
                                        NO CACHE";
                _db.Database.ExecuteSqlCommand(alterSequencePR);

                rn = new RunningNumber();
                rn.rn_FormCode = code;
                rn.rn_Year = year;
                rn.rn_Month = month;
                rn.rn_number = 1;
                _db.RunningNumbers.Add(rn);
                _db.SaveChanges();
            }


            string yyString = DateTime.Now.ToString("yy");
            string monthAlphabeth = getMonth(DateTime.Now.Month);

            var seqNumber = _db.Database.SqlQuery<long>("SELECT NEXT VALUE FOR SequencePR").FirstOrDefault();
            var seqString = seqNumber.ToString().PadLeft(6, '0');
            string result = code + yyString + monthAlphabeth + seqString;
            return result;
        }

        public string getMonth(int month)
        {
            string monthalphabet = null;
            if (month == 1)
            {
                monthalphabet = "A";

            } if (month == 2)
            {
                monthalphabet = "B";

            } if (month == 3)
            {
                monthalphabet = "C";

            } if (month == 4)
            {
                monthalphabet = "D";

            } if (month == 5)
            {
                monthalphabet = "E";

            } if (month == 6)
            {
                monthalphabet = "F";

            } if (month == 7)
            {
                monthalphabet = "G";

            } if (month == 8)
            {
                monthalphabet = "H";

            } if (month == 9)
            {
                monthalphabet = "I";

            } if (month == 10)
            {
                monthalphabet = "J";

            } if (month == 11)
            {
                monthalphabet = "K";

            } if (month == 12)
            {
                monthalphabet = "L";

            }
            return monthalphabet;
        }
    }
}
