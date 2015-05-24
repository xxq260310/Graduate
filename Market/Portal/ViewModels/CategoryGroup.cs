using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class CategoryGroup
    {
        public string ParentCategory { get; set; }

        public List<string> SubCategory { get; set; }
    }
}