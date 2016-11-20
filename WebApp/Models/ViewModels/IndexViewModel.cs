using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<PageCategories> pageCategories { get; set; }
        public List<Pages> pages { get; set; }
        public int? selectedCat_ID { get; set; }
        public string webRoot { get; set; }
        //ctor
        public IndexViewModel()
        {
            pageCategories = new List<PageCategories>();
            pages = new List<Pages>();
        }
    }
}
