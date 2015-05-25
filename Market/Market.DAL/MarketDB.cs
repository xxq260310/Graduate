using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL
{
    public partial class MarketDB
    {
        public MarketDB(DbConnection connection)
            : base(connection, true)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().HasKey(e => e.UserId);
            modelBuilder.Entity<Role>().HasKey(e => e.RoleId);
            modelBuilder.Entity<UserProfileCommodity>().HasKey(e => new { e.CommodityId, e.UserId});
            modelBuilder.Entity<SubCategory>().HasKey(e => e.SubCategoryId);
            modelBuilder.Entity<ParentCategory>().HasKey(e => e.ParentCategoryId);
            modelBuilder.Entity<Feedback>().HasKey(e => e.FeedbackId);
            modelBuilder.Entity<SiteFeedback>().HasKey(e => e.SiteFeedbackId);
            modelBuilder.Entity<ShoppingTrolley>().HasKey(e => e.UserId).HasRequired(e => e.UserProfile);
            modelBuilder.Entity<RequiredCommodity>().HasKey(e => e.RequiredCommodityId);
            modelBuilder.Entity<Order>().HasKey(e => e.OrderId);
            modelBuilder.Entity<Manufacturer>().HasKey(e => e.ManufacturerId);
            modelBuilder.Entity<Favorite>().HasKey(e => e.UserId).HasRequired(e => e.UserProfile);
            modelBuilder.Entity<Commodity>().HasKey(e => e.CommodityId);
            modelBuilder.Entity<CommodityInFavorite>().HasKey(e => new { e.CommodityId,e.UserId});
            modelBuilder.Entity<CommodityInOrder>().HasKey(e => new { e.OrderId,e.CommodityId});
            modelBuilder.Entity<CommodityInShoppingTrolley>().HasKey(e => new { e.CommodityId, e.UserId});
            modelBuilder.Entity<Announcement>().HasKey(e => e.AnnouncementId);
        }
    }
}
