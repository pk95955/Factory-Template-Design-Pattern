using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceImport_Factory_Template.Models
{
    public class AttendanceModel : ActionFilterAttribute
    {
       public string empNo{get;set;}
       public string empName { get;set;}
       public DateTime attendanceDate { get;set;}
       public string inTime {get;set; }
       public string ouTime{get;set; }
          

    }
}