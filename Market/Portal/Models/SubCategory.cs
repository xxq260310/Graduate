using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class SubCategory
    {
        public SubCategory()
        {
            this.Commoditys = new HashSet<Commodity>();
        }

        [Key]
        public int SubCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ParentCategoryId { get; set; }

        public virtual ParentCategory ParentCategory { get; set; }
        public virtual ICollection<Commodity> Commoditys { get; set; }
    }
}