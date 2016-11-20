using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Pages
    {
        public int Id { get; set; }
        public int PagCategoryId { get; set; }
        public string Header { get; set; }
        public string Html { get; set; }
        public DateTime Created { get; set; }
        public string Image { get; set; }
        public bool IsTop { get; set; }

        public virtual PageCategories PagCategory { get; set; }
    }
}
