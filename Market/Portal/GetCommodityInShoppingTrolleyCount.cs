using Portal.Authentication;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal
{
    public static class GetViewBag
    {
        public static int GetShoppingTrolleyViewBag(string userName)
        {
            int count;
            using (MarketContext db = new MarketContext())
            {
                var commodityInShoppingTrolley = (from shoppingTrolleyItem in db.ShoppingTrolleys
                                                  from commodityInShoppingTrolleyItem in db.CommodityInShoppingTrolleys
                                                  where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                                  && shoppingTrolleyItem.UserProfile.UserName == userName
                                                  select commodityInShoppingTrolleyItem).ToList();
                count = commodityInShoppingTrolley.Count;
                return count;
            }
        }

        public static List<CategoryGroup> GetCategoryViewBag()
        {
            using (MarketContext db = new MarketContext())
            {
                var parentCategory = (from p in db.ParentCategories
                                      select p).ToList();
                List<CategoryGroup> categoryGroupList = new List<CategoryGroup>();
                foreach (var p in parentCategory)
                {
                    CategoryGroup categoryGroup = new CategoryGroup();
                    var categories = (from item in db.SubCategories
                                      where item.ParentCategoryId == p.ParentCategoryId
                                      select item.CategoryName).ToList();
                    categoryGroup.ParentCategory = p.CategoryName;
                    categoryGroup.SubCategory = categories;

                    categoryGroupList.Add(categoryGroup);
                }

                return categoryGroupList;
            }
        }
    }
}