﻿using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private MarketContext db = new MarketContext();

        // GET: Home
        public ActionResult Index(string searchString, string sortOrder, string currentFilter, int? page, string subCategory, int? id)
        {
            ViewBag.CategoryList = GetViewBag.GetCategoryViewBag();

            var commodities = from item in this.db.Commodities
                              select item;

            if (!string.IsNullOrEmpty(subCategory))
            {
                commodities = commodities.Where(s => s.SubCategory.CategoryName == subCategory);
            }

            if (!string.IsNullOrEmpty(searchString) || id.HasValue)
            {
                commodities = commodities.Where(s => s.Description.Contains(searchString) || s.CommodityId == id);
            }

            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CreationDate = sortOrder == "creationdate" ? "creationdate_desc" : "creationdate";
            switch (sortOrder)
            {
                case "creationdate":
                    commodities = commodities.OrderBy(s => s.CreationDate);
                    break;
                case "creationdate_desc":
                    commodities = commodities.OrderByDescending(s => s.CreationDate);
                    break;
                default:
                    commodities = commodities.OrderBy(s => s.CommodityId);
                    break;
            }

            int pageSize = 6;
            int pageNumber = page ?? 1;

            ViewBag.ShoppingTrolleysCount = GetViewBag.GetShoppingTrolleyViewBag(User.Identity.Name);

            return View(commodities.ToPagedList(pageNumber, pageSize));
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
    }
}