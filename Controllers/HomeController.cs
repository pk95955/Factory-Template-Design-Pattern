using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using AttendanceImport_Factory_Template.Models;

namespace AttendanceImport_Factory_Template.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //Attendance Process Data
        [HttpPost]

         public ActionResult AttnProcessData()
        {
            if (Request.Files.Count> 0)
            {
                var FileData = Request.Files.Get("AttnFile");
               
                string FileExt = FileData.FileName.Substring(FileData.FileName.LastIndexOf(".")+1, FileData.FileName.Length - FileData.FileName.LastIndexOf(".")-1);
                List < AttendanceModel > attData =  new ProcessFileData(FileData.InputStream, FileExt).run();
            }
            return View("Index");
        }
    }
}