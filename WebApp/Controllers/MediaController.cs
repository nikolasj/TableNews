using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using WebApp.Models.ViewModels;
using System.Web;

namespace WebApp.Controllers
{
    public class MediaController : Controller
    {
        // http://www.mikesdotnetting.com/article/288/uploading-files-with-asp-net-core-1-0-mvc
        private IHostingEnvironment _env;

        public MediaController(IHostingEnvironment env)
        {
            _env = env;
        }
        // ckeditor
        public void uploadnow(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                string ImageName = upload.FileName;
                var path = Path.Combine(_env.WebRootPath, "/Images/Uploads/");
                string filename = Path.Combine(path, ImageName);
                upload.SaveAs(path);
            }
        }

        // ckeditor
        public ActionResult uploadPartial()
        {
            var path = Path.Combine(_env.WebRootPath, "/Images/Uploads/");
            var images = Directory.GetFiles(path).Select(x => new CKEditorImagesViewModel
            {
                Url = Url.Content("/images/uploads/" + Path.GetFileName(x))
            });
            return View(images);
        }
    }
}