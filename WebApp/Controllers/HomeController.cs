using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.ViewModels;
using WebApp.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebApp.Models;
using System.IO;
using System.Configuration;
using WebApp.Extentions;
using System.Net;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        // список страниц
        public IActionResult Index(int? id)
        {
            #region debug core

            string conn = ConfigurationManager.AppSettings["conn"];
            ContentResult cr = new ContentResult();
            cr.Content = conn;
            //return cr;

            #endregion

            PageServices pageServices = new PageServices();
            IndexViewModel VM = new IndexViewModel();
            VM.selectedCat_ID = id;
            VM.webRoot = _env.WebRootPath;

            VM.pageCategories = pageServices.GetPageCategories().ToList();
            VM.pages = pageServices.GetPages(id);

            return View(VM);
        }

        // список страниц
        public IActionResult Page(int id)
        {

            PageServices pageServices = new PageServices();
            PageViewModel VM = new PageViewModel();
            VM.page = pageServices.GetPage(id);
            VM.webRoot = _env.WebRootPath;
            //VM.pageCat = pageServices.GetPageCategory(VM.page.PagCategoryId); ;


            return View(VM);
        }

        [Authorize]
        public IActionResult EditPage(int id)
        {
            PageServices pageServices = new PageServices();
            EditPageViewModel VM = new EditPageViewModel();
            VM.webRoot = _env.WebRootPath;
            VM.page = pageServices.GetPage(id);
            if (VM.page == null)
                VM.page = new Models.Pages();

            ViewData["result"] = TempData["result"];
            return View(VM);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditPage(ICollection<IFormFile> files, 
            FormCollection formValues, 
            string add, string save, string delete)
        {
            int page_ID = Int32.Parse(HttpContext.Request.Form["ID"]);
            int category_ID = Int32.Parse(HttpContext.Request.Form["category_ID"]);
            bool isTop = HttpContext.Request.Form["isTop"] == "true";

            PageServices pageServices = new PageServices();
            string result = "";

            if (!String.IsNullOrEmpty(add))
            {
                return RedirectToAction("EditPage", new { id = 0 });
            }
            else if (!String.IsNullOrEmpty(delete))
            {
                ResultModel rm = pageServices.DeletePage(page_ID);
                result = rm.message;

                TempData["result"] = result;
                return RedirectToAction("EditPage", new { id = 0 });
            }
            else
            {
                

                ResultModel rm = pageServices.SavePage(page_ID,
                    HttpContext.Request.Form["header"],
                    HttpContext.Request.Form["html"],
                    category_ID, isTop);

                result = rm.message;

                string path = System.IO.Path.Combine(_env.WebRootPath, "Images\\QR");
                if (!System.IO.File.Exists(System.IO.Path.Combine(path, rm.ID + ".png")))
                {
                    #region QR-code

                    try
                    {
                        string URL = HttpContext.Request.Host.ToString().TrimEnd('/')
                        + "/home/page/" + rm.ID;

                        var url = string.Format("http://chart.apis.google.com/chart?cht=qr&chs={1}x{2}&chl={0}",
                            URL, 200, 200);
                        WebResponse response = default(WebResponse);
                        Stream remoteStream = default(Stream);
                        StreamReader readStream = default(StreamReader);
                        WebRequest request = WebRequest.Create(url);
                        response = request.GetResponse();
                        remoteStream = response.GetResponseStream();
                        readStream = new StreamReader(remoteStream);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);

                        var uploads = Path.Combine(_env.WebRootPath, "Images\\QR");
                        img.Save(Path.Combine(uploads, rm.ID + ".png"));
                        response.Close();
                        remoteStream.Close();
                        readStream.Close();
                    }
                    catch (Exception ex)
                    {
                    }

                    #endregion
                }
                if (rm.ID != 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "Images\\PageImages");
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                ResultModel rm1 = pageServices.SavePageImage(rm.ID, file.FileName);
                                result += " | " + rm.message;
                            }
                        }
                    }
                }

                TempData["result"] = result;
                return RedirectToAction("EditPage", new { id = rm.ID });
            }
            
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
