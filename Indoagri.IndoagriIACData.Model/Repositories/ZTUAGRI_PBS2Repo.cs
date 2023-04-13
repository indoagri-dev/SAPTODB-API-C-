using Indoagri.IndoagriIACData.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriIACData.Model.Repositories
{
    public class ZTUAGRI_PBS2Repo
    {
        private IAC_AGRIDATAEntities _db;
        public ZTUAGRI_PBS2Repo()
        {
            try
            {
                _db = new IAC_AGRIDATAEntities();
            }
            catch (Exception)
            {
                _db = new IAC_AGRIDATAEntities();
            }
        }

        public IQueryable<ZTUAGRI_PBS2> FindAll()
        {
            return from a in _db.ZTUAGRI_PBS2 select a;
        }

        public void Insert(PBS2_Mark form)
        {
            DateTime? ShipdateTime = null;
            DateTime? LoadDate = null;
            DateTime? ZDate = null;
            if (form.SHIPDATE == "0000-00-00")
            {
                ShipdateTime = null;
            }
            else
            {
                ShipdateTime = DateTime.ParseExact(form.SHIPDATE, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            if (form.LOADDATE == "0000-00-00")
            {
                LoadDate = null;
            }
            else
            {
                LoadDate = DateTime.ParseExact(form.LOADDATE, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture); ;
            }
            if (form.ZDATE == "0000-00-00")
            {
                ZDate = null;
            }
            else
            {
                ZDate = DateTime.ParseExact(form.ZDATE, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            var t = new ZTUAGRI_PBS2//Make sure you have a table called test in DB
            {      
                ZYEAR = form.ZYEAR,
                SPBSNUM = form.SPBSNUM,
                COMPANY_CODE = form.COMPANY_CODE,
                ESTATE = form.ESTATE,
                DIVISI = form.DIVISI,
                SPBSDATE = form.SPBSDATE,
                PLASMAID = form.PLASMAID,
                ASSTID = form.ASSTID,
                KRANI = form.KRANI,
                MILL = form.MILL,
                MILLEX = form.MILLEX,
                SOPIR = form.SOPIR,
                JARAK = form.JARAK,
                NOPOL = form.NOPOL,
                WBNUM = form.WBNUM,
                NET = form.NET,
                CREATEDDATE = form.CREATEDDATE,
                LASTUPDATEDDATE = form.LASTUPDATEDDATE,
                LASTUPDATEDBY = form.LASTUPDATEDBY,
                UOM = form.UOM,
                UOMJRK = form.UOMJRK,
                REVIEW = form.REVIEW,
                SAPNUM = form.SAPNUM,
                REFDOC = form.REFDOC,
                STATUS = form.STATUS,
                INEX = form.INEX,
                RECALNUM = form.RECALNUM,
                LOADDATE = LoadDate,
                MULTI = form.MULTI,
                ACTPLASMA = form.ACTPLASMA,
                UPLOAD = form.UPLOAD,
                LBKNUM = form.LBKNUM,
                RUNNING_ACCOUNT = form.RUNNING_ACCOUNT,
                LINENUM = form.LINENUM,
                BLOCKID = form.BLOCKID,
                ZDATE = ZDate,
                JANJANG = form.JANJANG,
                JJGUOM = form.JJGUOM,
                TAKJANJANG = form.TAKJANJANG,
                TAKBROND = form.TAKBROND,
                TAKTOTAL = form.TAKTOTAL,
                ACTUAL = form.ACTUAL,
                LBKNUM_ITEM = form.LBKNUM_ITEM,
                SHIPTYPE = form.SHIPTYPE,
                SHIPDATE = ShipdateTime,
                SHIPTRIP = form.SHIPTRIP,
                WEEK = form.WEEK,
                PERIODE = form.PERIODE,
                kernet = form.kernet,
                WBNUMEST = form.WBNUMEST,
                NETEST = form.NETEST,
                InsertDate = form.InsertDate
            };
            _db.ZTUAGRI_PBS2.Add(t);
        }

        public void Update(ZTUAGRI_PBS2 hk)
        {
            var hkData = (from a in _db.ZTUAGRI_PBS2 where a.SPBSNUM == hk.SPBSNUM && a.ZYEAR == hk.ZYEAR select a).Any();
            if (hkData)
            {
                _db.ZTUAGRI_PBS2.Attach(hk);
                _db.Entry(hk).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(ZTUAGRI_PBS2 item)
        {
            var asset = (from a in _db.ZTUAGRI_PBS2
                         where
                             a.ZYEAR == item.ZYEAR &&
                             a.SPBSNUM == item.SPBSNUM &&
                             a.COMPANY_CODE == item.COMPANY_CODE &&
                             a.ESTATE == item.ESTATE &&
                                a.LINENUM == item.LINENUM
                         select a).Any();
            if (asset)
            {
                _db.ZTUAGRI_PBS2.Remove(item);
            }
        }

        public void DeleteParam(ZTUAGRI_PBS2 item)
        {
            var asset = (from a in _db.ZTUAGRI_PBS2
                         where
                             a.ZYEAR == item.ZYEAR &&
                             a.SPBSNUM == item.SPBSNUM
                         select a).Any();
            if (asset)
            {
                _db.ZTUAGRI_PBS2.Remove(item);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
