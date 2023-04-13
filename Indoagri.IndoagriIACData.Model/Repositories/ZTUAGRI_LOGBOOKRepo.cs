using Indoagri.IndoagriIACData.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indoagri.IndoagriIACData.Model.Repositories
{
    public class ZTUAGRI_LOGBOOKRepo
    {
        private IAC_AGRIDATAEntities _db;
        public ZTUAGRI_LOGBOOKRepo()
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

        public IQueryable<ZTUAGRI_LB> FindAll()
        {
            return from a in _db.ZTUAGRI_LB select a;
        }

        public void Insert(LB_mark form)
        {
            DateTime? CreateDateI = null;
            DateTime? LastUpdateI = null;
            if (form.CREATEDDATEI == "0000-00-00")
            {
                CreateDateI = null;
            }
            else
            {
                CreateDateI = DateTime.ParseExact(form.CREATEDDATEI, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            if (form.LASTUPDATEDDATEI == "0000-00-00")
            {
                LastUpdateI = null;
            }
            else
            {
                LastUpdateI = DateTime.ParseExact(form.LASTUPDATEDDATEI, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture); ;
            }
            
            var t = new ZTUAGRI_LB//Make sure you have a table called test in DB
            {
                COMPANY_CODE = form.COMPANY_CODE,
                LBKNUM = form.LBKNUM,
                ZYEAR = form.ZYEAR,
                ESTATE = form.ESTATE,
                DIVISI = form.DIVISI,
                MDRAN = form.MDRAN,
                LBKDATE = form.LBKDATE,
                WEEK = form.WEEK,
                PERIOD = form.PERIOD,
                MANDOR = form.MANDOR,
                KRANI = form.KRANI,
                STATUS = form.STATUS,
                CREATEDDATE = form.CREATEDDATE,
                CREATEDBY = form.CREATEDBY,
                LASTUPDATEDDATE = form.LASTUPDATEDDATE,
                LASTUPDATEDBY = form.LASTUPDATEDBY,
                REVIEW= form.REVIEW,
                SAPNUM = form.SAPNUM,
                REFDOC = form.REFDOC,
                UPLOAD = form.UPLOAD,
                HISTORY = form.HISTORY,
                SPBSNUM = form.SPBSNUM,
                LINENUM = form.LINENUM,
                RUNACC = form.RUNACC,
                EMPID = form.EMPID,
                ACTID = form.ACTID,
                TYPEID = form.TYPEID,
                LOCID = form.LOCID,
                LOCCC = form.LOCCC,
                JARAK = form.JARAK,
                UOM = form.UOM,
                TYPEANGK = form.TYPEANGK,
                QTY = form.QTY,
                UOMQTY = form.UOMQTY,
                KERNET = form.KERNET,
                CREATEDDATE_I = CreateDateI,
                CREATEDBY_I = form.CREATEDBYI,
                LASTUPDATEDDATEI = LastUpdateI,
                LASTUPDATEDBY_I = form.LASTUPDATEDBY_I,
                HSTART = form.HSTART,
                HEND = form.HEND,
                REMARK = form.REMARK,
                REVIEW_I= form.REVIEW_I,
                PCIO = form.PCIO,
                TRIPC = form.TRIPC,
                DEST = form.DEST,
                FTRIP = form.FTRIP,
                DOCNUM = form.DOCNUM,
                DOCLINE = form.DOCLINE,
                EBELN = form.EBELN,
                CONTRACTNUM = form.CONTRACTNUM,
                DONUM = form.DONUM,
                InsertDate = form.InsertDate
            };
            _db.ZTUAGRI_LB.Add(t);
        }

        public void Update(ZTUAGRI_LB LB)
        {
            var hkData = (from a in _db.ZTUAGRI_LB where a.ESTATE == LB.ESTATE && a.LBKNUM== LB.LBKNUM && a.ZYEAR == LB.ZYEAR select a).Any();
            if (hkData)
            {
                _db.ZTUAGRI_LB.Attach(LB);
                _db.Entry(LB).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(ZTUAGRI_LB item)
        {
            var asset = (from a in _db.ZTUAGRI_LB
                         where
                             a.ESTATE == item.ESTATE && 
                             a.LBKNUM == item.LBKNUM && 
                             a.ZYEAR == item.ZYEAR
                         select a).Any();
            if (asset)
            {
                _db.ZTUAGRI_LB.Remove(item);
            }
        }

        public void DeleteParam(ZTUAGRI_LB item)
        {
            var asset = (from a in _db.ZTUAGRI_LB
                         where
                             a.ESTATE == item.ESTATE && a.LBKNUM == item.LBKNUM && a.ZYEAR == item.ZYEAR
                         select a).Any();
            if (asset)
            {
                _db.ZTUAGRI_LB.Remove(item);
                //_db.SaveChanges();
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
