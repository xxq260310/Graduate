//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Market.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.CommodityInOrders = new HashSet<CommodityInOrder>();
        }
    
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Contact { get; set; }
        public Nullable<double> TotalCost { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Delivery { get; set; }
        public string ConsigneeName { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Email { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<CommodityInOrder> CommodityInOrders { get; set; }
    }
}
