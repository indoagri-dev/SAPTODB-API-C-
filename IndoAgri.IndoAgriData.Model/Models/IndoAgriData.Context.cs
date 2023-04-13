﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IndoAgri.IndoAgriData.Model.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class INDOAGRI_DATAEntities : DbContext
    {
        public INDOAGRI_DATAEntities()
            : base("name=INDOAGRI_DATAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<LogPostData> LogPostDatas { get; set; }
        public virtual DbSet<ZTW_ASSETSTAT> ZTW_ASSETSTAT { get; set; }
        public virtual DbSet<ZTUAGRI_PBS2_RPT_TEMP> ZTUAGRI_PBS2_RPT_TEMP { get; set; }
        public virtual DbSet<ZTUAGRI_HK_Temp> ZTUAGRI_HK_Temp { get; set; }
        public virtual DbSet<ZTUAGRI_BP_RPT_Temp> ZTUAGRI_BP_RPT_Temp { get; set; }
        public virtual DbSet<ZTUAGRI_BP_RPT> ZTUAGRI_BP_RPT { get; set; }
        public virtual DbSet<SALES_MOBILE_STOCK_TEMP> SALES_MOBILE_STOCK_TEMP { get; set; }
        public virtual DbSet<RUNACC_RASIO_TEMP> RUNACC_RASIO_TEMP { get; set; }
        public virtual DbSet<ZTM_YTDTPURC_MIP_TEMP> ZTM_YTDTPURC_MIP_TEMP { get; set; }
        public virtual DbSet<ZTPAGRI_MILLDATA> ZTPAGRI_MILLDATA { get; set; }
        public virtual DbSet<ZTPAGRI_MILLPROD> ZTPAGRI_MILLPROD { get; set; }
        public virtual DbSet<ZTMAGRI_STOCKPRD> ZTMAGRI_STOCKPRD { get; set; }
        public virtual DbSet<ZTM_PR_LIST> ZTM_PR_LIST { get; set; }
        public virtual DbSet<ZTF_CDV_LASTMAIL> ZTF_CDV_LASTMAIL { get; set; }
        public virtual DbSet<ZTM_ORDER_PO> ZTM_ORDER_PO { get; set; }
        public virtual DbSet<LogSAPToDB> LogSAPToDBs { get; set; }
        public virtual DbSet<ZTM_MATPRC_MIP_TEMP> ZTM_MATPRC_MIP_TEMP { get; set; }
        public virtual DbSet<ZTU_PRCALCHV> ZTU_PRCALCHV { get; set; }
        public virtual DbSet<ZTUAGRI_BP> ZTUAGRI_BP { get; set; }
        public virtual DbSet<ZTUAGRI_HK> ZTUAGRI_HK { get; set; }
        public virtual DbSet<ZTUAGRI_PBS2> ZTUAGRI_PBS2 { get; set; }
        public virtual DbSet<ZC0032> ZC0032 { get; set; }
    
        public virtual ObjectResult<Nullable<int>> SP_ZTUAGRI_HK_MOVE(Nullable<System.DateTime> lASTUPDATEDDATE, string estate)
        {
            var lASTUPDATEDDATEParameter = lASTUPDATEDDATE.HasValue ?
                new ObjectParameter("LASTUPDATEDDATE", lASTUPDATEDDATE) :
                new ObjectParameter("LASTUPDATEDDATE", typeof(System.DateTime));
    
            var estateParameter = estate != null ?
                new ObjectParameter("Estate", estate) :
                new ObjectParameter("Estate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_ZTUAGRI_HK_MOVE", lASTUPDATEDDATEParameter, estateParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_ZTUAGRI_PBS2_RPT_MOVE(string lOADDATEFROM, string lOADDATETO, string estate)
        {
            var lOADDATEFROMParameter = lOADDATEFROM != null ?
                new ObjectParameter("LOADDATEFROM", lOADDATEFROM) :
                new ObjectParameter("LOADDATEFROM", typeof(string));
    
            var lOADDATETOParameter = lOADDATETO != null ?
                new ObjectParameter("LOADDATETO", lOADDATETO) :
                new ObjectParameter("LOADDATETO", typeof(string));
    
            var estateParameter = estate != null ?
                new ObjectParameter("Estate", estate) :
                new ObjectParameter("Estate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_ZTUAGRI_PBS2_RPT_MOVE", lOADDATEFROMParameter, lOADDATETOParameter, estateParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_ZTUAGRI_BP_RPT_MOVE(string bPDATEFROM, string bPDATETO, string estate)
        {
            var bPDATEFROMParameter = bPDATEFROM != null ?
                new ObjectParameter("BPDATEFROM", bPDATEFROM) :
                new ObjectParameter("BPDATEFROM", typeof(string));
    
            var bPDATETOParameter = bPDATETO != null ?
                new ObjectParameter("BPDATETO", bPDATETO) :
                new ObjectParameter("BPDATETO", typeof(string));
    
            var estateParameter = estate != null ?
                new ObjectParameter("Estate", estate) :
                new ObjectParameter("Estate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_ZTUAGRI_BP_RPT_MOVE", bPDATEFROMParameter, bPDATETOParameter, estateParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_COMSATIC_STOCK_MOVE(string mATERIAL, string aSOFDATE, string aREA)
        {
            var mATERIALParameter = mATERIAL != null ?
                new ObjectParameter("MATERIAL", mATERIAL) :
                new ObjectParameter("MATERIAL", typeof(string));
    
            var aSOFDATEParameter = aSOFDATE != null ?
                new ObjectParameter("ASOFDATE", aSOFDATE) :
                new ObjectParameter("ASOFDATE", typeof(string));
    
            var aREAParameter = aREA != null ?
                new ObjectParameter("AREA", aREA) :
                new ObjectParameter("AREA", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_COMSATIC_STOCK_MOVE", mATERIALParameter, aSOFDATEParameter, aREAParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_RUNACC_RASIO(string cOMPANYCODE, string eSTATE, string pERIOD, string yEAR)
        {
            var cOMPANYCODEParameter = cOMPANYCODE != null ?
                new ObjectParameter("COMPANYCODE", cOMPANYCODE) :
                new ObjectParameter("COMPANYCODE", typeof(string));
    
            var eSTATEParameter = eSTATE != null ?
                new ObjectParameter("ESTATE", eSTATE) :
                new ObjectParameter("ESTATE", typeof(string));
    
            var pERIODParameter = pERIOD != null ?
                new ObjectParameter("PERIOD", pERIOD) :
                new ObjectParameter("PERIOD", typeof(string));
    
            var yEARParameter = yEAR != null ?
                new ObjectParameter("YEAR", yEAR) :
                new ObjectParameter("YEAR", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_RUNACC_RASIO", cOMPANYCODEParameter, eSTATEParameter, pERIODParameter, yEARParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_ZTM_MATPRC_MIP_MOVE(string mATGRP, string aREA, string mATNR, string mANDT, string lSTPRD)
        {
            var mATGRPParameter = mATGRP != null ?
                new ObjectParameter("MATGRP", mATGRP) :
                new ObjectParameter("MATGRP", typeof(string));
    
            var aREAParameter = aREA != null ?
                new ObjectParameter("AREA", aREA) :
                new ObjectParameter("AREA", typeof(string));
    
            var mATNRParameter = mATNR != null ?
                new ObjectParameter("MATNR", mATNR) :
                new ObjectParameter("MATNR", typeof(string));
    
            var mANDTParameter = mANDT != null ?
                new ObjectParameter("MANDT", mANDT) :
                new ObjectParameter("MANDT", typeof(string));
    
            var lSTPRDParameter = lSTPRD != null ?
                new ObjectParameter("LSTPRD", lSTPRD) :
                new ObjectParameter("LSTPRD", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_ZTM_MATPRC_MIP_MOVE", mATGRPParameter, aREAParameter, mATNRParameter, mANDTParameter, lSTPRDParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_ZTM_YTDTPURC_MIP_MOVE(string cOMP_GROUP, string mATGRP, string mANDT, string lSTPRD)
        {
            var cOMP_GROUPParameter = cOMP_GROUP != null ?
                new ObjectParameter("COMP_GROUP", cOMP_GROUP) :
                new ObjectParameter("COMP_GROUP", typeof(string));
    
            var mATGRPParameter = mATGRP != null ?
                new ObjectParameter("MATGRP", mATGRP) :
                new ObjectParameter("MATGRP", typeof(string));
    
            var mANDTParameter = mANDT != null ?
                new ObjectParameter("MANDT", mANDT) :
                new ObjectParameter("MANDT", typeof(string));
    
            var lSTPRDParameter = lSTPRD != null ?
                new ObjectParameter("LSTPRD", lSTPRD) :
                new ObjectParameter("LSTPRD", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_ZTM_YTDTPURC_MIP_MOVE", cOMP_GROUPParameter, mATGRPParameter, mANDTParameter, lSTPRDParameter);
        }
    }
}