﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MarketDB : DbContext
    {
        public MarketDB()
            : base("name=MarketDB")
        {
        }
    
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    throw new UnintentionalCodeFirstException();
        //}
    
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Commodity> Commodities { get; set; }
        public virtual DbSet<ParentCategory> ParentCategories { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ShoppingTrolley> ShoppingTrolleys { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<RequiredCommodity> RequiredCommodities { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<SiteFeedback> SiteFeedbacks { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<CommodityInOrder> CommodityInOrders { get; set; }
        public virtual DbSet<UserProfileCommodity> UserProfileCommodities { get; set; }
        public virtual DbSet<CommodityInShoppingTrolley> CommodityInShoppingTrolleys { get; set; }
        public virtual DbSet<CommodityInFavorite> CommodityInFavorites { get; set; }
        public virtual DbSet<CommodityInfo> CommodityInfoes { get; set; }
    }
}
