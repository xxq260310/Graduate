using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class ParentCategory
    {
        public ParentCategory()
        {
            this.SubCategorys = new HashSet<SubCategory>();
        }

        [Key]
        public int ParentCategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<SubCategory> SubCategorys { get; set; }
    }
}