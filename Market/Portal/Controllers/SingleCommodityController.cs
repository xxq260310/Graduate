using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class SingleCommodityController : Controller
    {
        private MarketContext db = new MarketContext();

        public FileContentResult GetImageByCommodityInfoId(int? id)
        {
            CommodityInfo model = this.db.CommodityInfoes.Where(x => x.CommodityInfoId == id).FirstOrDefault();
            if (model != null)
            {
                if (model.ImageData != null)
                {
                    return File(model.ImageData, model.ImageType);
                }

                return null;
            }

            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetQuantity(CommodityPropertyViewModel model)
        {
            var quantity = (from commodityInfo in this.db.CommodityInfoes
                            where commodityInfo.Color == model.Color
                            && commodityInfo.Size == model.Size
                            && commodityInfo.Capacity == model.Capacity
                            && commodityInfo.CommodityId == model.CommodityId
                            select commodityInfo.Quantity).FirstOrDefault();
            return this.Json(quantity);
        }

        public FileContentResult GetImageByCommodityId(int id)
        {
            Commodity commodity = this.db.Commodities.FirstOrDefault(p => p.CommodityId == id);
            if (commodity != null)
            {
                if (commodity.ImageData != null)
                {
                    return File(commodity.ImageData, commodity.ImageType);
                }

                return null;
            }

            else
            {
                return null;
            }
        }

        public ActionResult SingleCommodity(int? id)
        {
            var parentCategory = (from p in this.db.ParentCategories
                                  select p).ToList();
            List<CategoryGroup> categoryGroupList = new List<CategoryGroup>();
            foreach (var p in parentCategory)
            {
                CategoryGroup categoryGroup = new CategoryGroup();
                var categories = (from subCategory in this.db.SubCategories
                                  where subCategory.ParentCategoryId == p.ParentCategoryId
                                  select subCategory.CategoryName).ToList();
                categoryGroup.ParentCategory = p.CategoryName;
                categoryGroup.SubCategory = categories;

                categoryGroupList.Add(categoryGroup);
            }

            ViewBag.CategoryList = categoryGroupList;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var commodityInfo = (from commodity in this.db.Commodities
                                .Include("CommodityInfoes")
                                 where commodity.CommodityId == id
                                 select new CommodityViewModel()
                                 {
                                     CommodityId = id.Value,
                                     CommodityName = commodity.CommodityName,
                                     Description = commodity.Description,
                                     UnitPrice = commodity.UnitPrice,
                                     SubPrice = commodity.SubPrice,
                                     DiscountPrice = commodity.DiscountPrice,
                                     Degree = commodity.Degree,
                                     Brand = commodity.Brand,
                                     IsRecommended = commodity.IsRecommended,
                                     MakeCompany = commodity.MakeCompany,
                                     ManufacturerName = commodity.Manufacturer.ManufacturerName,
                                     ManufacturerDate = commodity.ManufacturerDate,
                                     Quantity = commodity.Quantity,
                                     Component = commodity.Component,
                                     Weight = commodity.Weight,
                                     ImageData = commodity.ImageData,
                                     ImageType = commodity.ImageType,
                                     CommodityInfos = commodity.CommodityInfos.Select(u => new CommodityInfoViewModel()
                                     {
                                         Color = u.Color,
                                         Size = u.Size,
                                         Quantity = u.Quantity,
                                         Capacity = u.Capacity,
                                         CommodityId = id.Value,
                                         ImageData = u.ImageData,
                                         ImageType = u.ImageType,
                                         CommodityInfoId = u.CommodityInfoId
                                     })
                                 }).FirstOrDefault();

            var commodityInfoModel = (from item in this.db.CommodityInfoes
                                      where item.CommodityId == id
                                      select item).ToList();
            List<CommodityInfoViewModel> list = new List<CommodityInfoViewModel>();
            foreach (var p in commodityInfoModel)
            {
                CommodityInfoViewModel model = new CommodityInfoViewModel();
                model.Size = p.Size;
                model.Quantity = p.Quantity;
                model.Capacity = p.Capacity;
                model.Color = p.Color;
                model.CommodityId = p.CommodityId;
                model.CommodityInfoId = p.CommodityInfoId;

                list.Add(model);
            }

            if (list.Count > 0)
            {
                List<string> colorList = new List<string>();
                List<string> sizeList = new List<string>();
                List<string> capacityList = new List<string>();
                colorList.Add(list[0].Color);
                sizeList.Add(list[0].Size);
                capacityList.Add(list[0].Capacity);
                for (int i = 1; i < list.Count; i++)
                {
                    if (list[0].Color != list[i].Color)
                    {
                        colorList.Add(list[i].Color);
                    }

                    if (list[0].Size != list[i].Size)
                    {
                        sizeList.Add(list[i].Size);
                    }

                    if (list[0].Capacity != list[i].Capacity)
                    {
                        capacityList.Add(list[i].Capacity);
                    }
                }

                ViewBag.Color = colorList;
                ViewBag.ColorCount = colorList.Count;
                ViewBag.Size = sizeList;
                ViewBag.SizeCount = sizeList.Count;
                ViewBag.Capacity = capacityList;
                ViewBag.CapacityCount = capacityList.Count;
            }
            
            var feedback = (from feedbacks in this.db.Feedbacks
                            where feedbacks.CommodityId == id
                            select feedbacks).ToList();
            List<FeedbackViewModel> feedbackModel = new List<FeedbackViewModel>();
            foreach (var p in feedback)
            {
                FeedbackViewModel feedbackViewModel = new FeedbackViewModel();
                feedbackViewModel.Content = p.Content;
                feedbackViewModel.CreationDate = p.CreationDate;
                feedbackViewModel.Reply = p.Reply;
                feedbackViewModel.NickName = p.UserProfile.Nickname;
                feedbackViewModel.UserName = p.UserProfile.UserName;
                feedbackViewModel.FeedbackId = p.FeedbackId;

                feedbackModel.Add(feedbackViewModel);
            }

            ViewBag.Feedbacks = feedbackModel;


            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View(commodityInfo);
        }

        [HttpPost]
        public ActionResult SingleCommodity(FormCollection formcollection, int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var size = formcollection["CheckedSize"].ToString();
            var color = formcollection["CheckedColor"].ToString();
            var capacity = formcollection["CheckedCapacity"].ToString();
            var count = formcollection["Select"].ToString();
            var price = formcollection["actual"].ToString();
            var userId = GetInfo.GetUserIdByUserName(User.Identity.Name);

            int quantity = int.Parse(count);
            var p = this.db.ShoppingTrolleys.SingleOrDefault(e => e.UserId == userId);
            if (p == null)
            {
                ShoppingTrolley shoppingTrolley = new ShoppingTrolley()
                {
                    UserId = userId,
                    CreationDate = System.DateTime.Now
                };
                this.db.ShoppingTrolleys.Add(shoppingTrolley);
            }

            var q = this.db.CommodityInShoppingTrolleys.SingleOrDefault(e => e.UserId == userId && e.CommodityId == id);
            if (q == null)
            {
                CommodityInShoppingTrolley model = new CommodityInShoppingTrolley();
                model.CommodityId = id.Value;
                model.UnitPrice = Convert.ToInt32(price);
                model.Size = size;
                model.Capacity = capacity;
                model.Color = color;
                model.UserId = userId;
                model.Quantity = quantity;
                this.db.CommodityInShoppingTrolleys.Add(model);
            }
            
            this.db.SaveChanges();
            return RedirectToAction("Home", "ShoppingTrolleys");
        }
    }
}