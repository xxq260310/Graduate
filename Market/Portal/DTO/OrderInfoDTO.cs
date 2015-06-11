using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.DTO
{
    public class OrderInfoDTO
    {
        public string Address { get; set; }

        public string Delivery { get; set; }

        public string Payfor { get; set; }

        public string OrderId { get; set; }
    }
}