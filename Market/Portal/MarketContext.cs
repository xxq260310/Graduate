using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Portal
{
    public class MarketContext : DbContext
    {
        public MarketContext()
            : base("name=MarketContext")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserProfileCommodity> UserProfileCommodities { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Commodity> Commodities { get; set; }

        public DbSet<CommodityInfo> CommodityInfoes { get; set; }

        public DbSet<SiteFeedback> SiteFeedbacks { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<ShoppingTrolley> ShoppingTrolleys { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<CommodityInFavorite> CommodityInFavorites { get; set; }

        public DbSet<CommodityInOrder> CommodityInOrders { get; set; }

        public DbSet<CommodityInShoppingTrolley> CommodityInShoppingTrolleys { get; set; }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<ParentCategory> ParentCategories { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<RequiredCommodity> RequiredCommodities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfileCommodity>().HasKey(p => new { p.UserId, p.CommodityId });
            modelBuilder.Entity<CommodityInFavorite>().HasKey(p => new { p.CommodityId, p.UserId });
            modelBuilder.Entity<CommodityInOrder>().HasKey(p => new { p.CommodityId, p.OrderId });
            modelBuilder.Entity<CommodityInShoppingTrolley>().HasKey(p => new { p.UserId, p.CommodityId });
        }
    }
}