using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebAPI
{
    public class UniversityYearInfo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string yearOfStudy { get; set; }
        public string domain { get; set; }
        public string specialty { get; set; }
        public string mark { get; set; }
        public string statute { get; set; }
        public string financialSource { get; set; }
        public DataTable marksTable { get; set; }
    }
}