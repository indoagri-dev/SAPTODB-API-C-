﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndoAgri.Approval.Api.Models
{
    public class UserProfileSAP
    {
        public string CompanyCode { get; set; }
        public string BACode { get; set; }
        public string Division { get; set; }
        public string TCode { get; set; }
        public string Activity { get; set; }
    }
}