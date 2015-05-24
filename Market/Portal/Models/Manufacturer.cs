using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Commodity = new HashSet<Commodity>();
        }

        [Key]
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Commodity> Commodity { get; set; }
    }
}