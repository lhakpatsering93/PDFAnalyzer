using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PDFReader;

namespace PDFAnalyzer.Controllers
{
    public class PdfReaderController : Controller
    {
        // GET: PdfReader
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult UploadFiles()
        { 
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);
                    }
                    return Json(new { success = true, responseText = "File Uploaded Successfully!" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, responseText = "Error occurred. Error details: " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        [HttpGet]
        public ActionResult Validate()
        {
            var files = Server.MapPath("~/Uploads/");
            var fileName = Directory.GetFiles(files).FirstOrDefault();
            pdfReadOutput class1 = new pdfReadOutput();
            var text = class1.CallFunctions(fileName);
            return Json(new { success = true, responseData = text }, JsonRequestBehavior.AllowGet);
        }
    }
}
