using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            this.Feedbacks = new HashSet<Feedback>();
            this.SiteFeedbacks = new HashSet<SiteFeedback>();
            this.RequiredCommoditys = new HashSet<RequiredCommodity>();
            this.UserProfileCommoditys = new HashSet<UserProfileCommodity>();
            this.Orders = new HashSet<Order>();
        }

        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<SiteFeedback> SiteFeedbacks { get; set; }
        public virtual ICollection<RequiredCommodity> RequiredCommoditys { get; set; }
        public virtual ICollection<UserProfileCommodity> UserProfileCommoditys { get; set; }
        public virtual ShoppingTrolley ShoppingTrolley { get; set; }
        public virtual Favorite Favorite { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}