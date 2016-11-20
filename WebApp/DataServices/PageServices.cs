using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Models;
//using EFGetStarted.AspNetCore.ExistingDb.Models;
//using Microsoft.Extensions.Configuration;

namespace WebApp.DataServices
{
    public class PageServices
    {
        //https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
        public ApplicationDbContext context { get; set; }

        public List<PageCategories> GetPageCategories()
        {
                return context.PageCategories.ToList();
        }

        public List<Pages> GetPages(int? id)
        {
            if (id == null)
                return context.Pages.OrderByDescending(p => p.IsTop).ToList();
            else 
                return context.Pages.Where(p => p.PagCategoryId == (int)id).OrderByDescending(p => p.IsTop).ToList();
        }

        public Pages GetPage(int id)
        {
            return context.Pages.FirstOrDefault(p => p.Id == id);
        }

        // ctor
        public PageServices()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string conn = ConfigurationManager.AppSettings["conn"];
            optionsBuilder.UseSqlServer(conn);

            context = new ApplicationDbContext(optionsBuilder.Options);
        }

        public ResultModel SavePage(
            int ID, 
            string header, 
            string html, 
            int category_ID,
            bool isTop)
        {
            ResultModel rm = new ResultModel();

            Pages page = new Pages();
            if (ID != 0)
                page = context.Pages.FirstOrDefault(p => p.Id == ID);

            page.Header = header;
            page.Html = html;
            page.PagCategoryId = category_ID;
            page.IsTop = isTop;
            page.Created = DateTime.Now;

            if (ID == 0)
                context.Pages.Add(page);

            try {
                context.SaveChanges();
                rm.ID = page.Id;
                rm.message = "Сохранили страницу";
            }
            catch (Exception ex)
            {
                rm.message = "Ошибка сохранения страницы: " + ex.Message;
            }

            return rm;
        }

        public ResultModel DeletePage(int ID)
        {
            ResultModel rm = new ResultModel();

            Pages page = context.Pages.FirstOrDefault(p => p.Id == ID);

            try
            {
                context.Pages.Remove(page);
                context.SaveChanges();
                rm.ID = page.Id;
                rm.message = "Удалили страницу";
            }
            catch (Exception ex)
            {
                rm.message = "Ошибка удаления страницы: " + ex.Message;
            }

            return rm;
        }

        public ResultModel SavePageImage(
            int ID,
            string image)
        {
            ResultModel rm = new ResultModel();

            Pages page = context.Pages.FirstOrDefault(p => p.Id == ID);

            page.Image = image;

            try
            {
                context.SaveChanges();
                rm.ID = page.Id;
                rm.message = "Сохранили картинку";
            }
            catch (Exception ex)
            {
                rm.message = "Ошибка сохранения картинки: " + ex.Message;
            }

            return rm;
        }
    }
}
