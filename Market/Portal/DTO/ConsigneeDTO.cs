using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.DTO
{
    public class ConsigneeDTO
    {
        public int Index { get; set; } 
            
        public string ConsigneeName { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Town { get; set; }

        public string Address { get; set; }
    }
}