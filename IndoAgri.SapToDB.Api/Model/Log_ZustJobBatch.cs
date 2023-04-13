using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndoAgri.SapToDB.Api.Model
{
     [Table("LOG_SAP_ZUST_JOB_BATCH")]
    public class Log_ZustJobBatch
    {
        [Key]
        public int ID { get; set; }
        public string JOBNAME { get; set; }
        public DateTime SDLDATE { get; set; }
        public string SDLTIME { get; set; }
        public string SDLUNAME { get; set; }
        public string STATUS { get; set; }
        public string STATUS_JOB { get; set; }
    }
}