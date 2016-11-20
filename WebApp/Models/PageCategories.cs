using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class PageCategories
    {
        public PageCategories()
        {
            Pages = new HashSet<Pages>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Pages> Pages { get; set; }
    }
}
