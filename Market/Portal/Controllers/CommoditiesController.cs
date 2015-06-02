using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal;
using Portal.Models;
using Portal.ViewModels;
using WebGrease;
using Portal.Authentication;

namespace Portal.Controllers
{
    [Authorize]
    [Portal.Filter.AuthorizationFilter]
    public class CommoditiesController : Controller
    {
        private MarketContext db = new MarketContext();

        public FileContentResult GetImageByCommodityInfoId(int id)
        {
            CommodityInfo model = this.db.CommodityInfoes.FirstOrDefault(p => p.CommodityInfoId == id);
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

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            if (GetInfo.GetRoleNameByUserName(User.Identity.Name) == "Administrator")
            {
                return View(db.Commodities.ToList());
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [Authorize(Roles = "TemporaryAdmin")]
        public ActionResult TemporaryAdminIndex()
        {
            if (GetInfo.GetRoleNameByUserName(User.Identity.Name) == "TemporaryAdmin")
            {
                var userProfileCommodity = (from commodity in this.db.UserProfileCommodities
                                            where commodity.UserId == GetInfo.GetUserIdByUserName(User.Identity.Name)
                                            select commodity.Commodity).ToList();
                var commodityViewModel = new List<CommodityViewModel>();
                foreach (var p in userProfileCommodity)
                {
                    CommodityViewModel model = new CommodityViewModel()
                    {
                        CommodityId = p.CommodityId,
                        CommodityName = p.CommodityName,
                        SubCategoryName = p.SubCategory.CategoryName,
                        ManufacturerName = p.Manufacturer.ManufacturerName,
                        UnitPrice = p.UnitPrice,
                        SubPrice = p.SubPrice,
                        DiscountPrice = p.DiscountPrice,
                        MakeCompany = p.MakeCompany,
                        Degree = p.Degree,
                        Quantity = p.Quantity,
                        Description = p.Description,
                        Brand = p.Brand,
                        IsRecommended = p.IsRecommended,
                        ManufacturerDate = p.ManufacturerDate,
                        PromotionStartOn = p.PromotionStartOn,
                        PromotionEndOn = p.PromotionEndOn,
                        Weight = p.Weight,
                        Component = p.Component,
                        ImageData = p.ImageData,
                        ImageType = p.ImageType,
                        CreationDate = p.CreationDate,
                        UpdateDate = p.UpdateDate
                    };
                    commodityViewModel.Add(model);
                }

                ViewBag.UserProfileCommodityList = commodityViewModel;
                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "TemporaryAdmin")]
        public ActionResult TemporaryAdminDetails(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "TemporaryAdmin")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }

            return View(commodity);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Commodities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "Administrator")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }

            return View(commodity);
        }

        [Authorize(Roles = "TemporaryAdmin")]
        public ActionResult TemporaryAdminCreate()
        {
            if (GetInfo.GetRoleNameByUserName(User.Identity.Name) == "TemporaryAdmin")
            {
                ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "ManufacturerId", "ManufacturerName");
                ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "CategoryName");
                List<SelectListItem> selList = InitialData();
                ViewBag.Color = selList;
                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TemporaryAdminCreate(Commodity commodity, HttpPostedFileBase Image, IEnumerable<HttpPostedFileBase> files, IEnumerable<CommodityInfo> commodityInfoList)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    commodity.ImageType = Image.ContentType;
                    commodity.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(commodity.ImageData, 0, Image.ContentLength);

                    Commodity dbEntry = new Commodity();
                    dbEntry.CommodityName = commodity.CommodityName;
                    dbEntry.SubCategoryId = commodity.SubCategoryId;
                    dbEntry.ManufacturerId = commodity.ManufacturerId;
                    dbEntry.UnitPrice = commodity.UnitPrice;
                    dbEntry.SubPrice = commodity.SubPrice;
                    dbEntry.DiscountPrice = commodity.DiscountPrice;
                    dbEntry.MakeCompany = commodity.MakeCompany;
                    dbEntry.Degree = commodity.Degree;
                    dbEntry.Quantity = commodity.Quantity;
                    dbEntry.Description = commodity.Description;
                    dbEntry.ImageData = commodity.ImageData;
                    dbEntry.ImageType = commodity.ImageType;
                    dbEntry.Brand = commodity.Brand;
                    dbEntry.IsRecommended = commodity.IsRecommended;
                    dbEntry.ManufacturerDate = commodity.ManufacturerDate;
                    dbEntry.PromotionStartOn = commodity.PromotionStartOn;
                    dbEntry.PromotionEndOn = commodity.PromotionEndOn;
                    dbEntry.Weight = commodity.Weight;
                    dbEntry.Component = commodity.Component;
                    dbEntry.CreationDate = System.DateTime.Now;
                    db.Commodities.Add(dbEntry);
                }

                var p = new List<CommodityInfo>();
                List<ImageViewModel> imageViewModel = new List<ImageViewModel>();
                ImageViewModel model = new ImageViewModel();

                if (files != null)
                {
                    foreach (var s in files)
                    {
                        if (s != null)
                        {
                            model.ImageType = s.ContentType;
                            model.ImageData = new byte[s.ContentLength];
                            s.InputStream.Read(model.ImageData, 0, s.ContentLength);

                            imageViewModel.Add(model);
                        }
                    }
                }

                int i = 0;
                if (commodityInfoList != null)
                {
                    foreach (var item in commodityInfoList)
                    {

                        for (; i < imageViewModel.Count; )
                        {
                            CommodityInfo commodityInfo = new CommodityInfo();
                            commodityInfo.CommodityId = commodity.CommodityId;
                            commodityInfo.Color = item.Color;
                            commodityInfo.Size = item.Size;
                            commodityInfo.Quantity = item.Quantity;
                            commodityInfo.Capacity = item.Capacity;
                            commodityInfo.ImageData = imageViewModel[i].ImageData;
                            commodityInfo.ImageType = imageViewModel[i].ImageType;

                            i++;
                            p.Add(commodityInfo);
                            break;
                        }
                    }
                }

                db.CommodityInfoes.AddRange(p);
                UserProfileCommodity userProfileCommodity = new UserProfileCommodity();
                userProfileCommodity.UserId = GetInfo.GetUserIdByUserName(User.Identity.Name);
                userProfileCommodity.CommodityId = commodity.CommodityId;
                this.db.UserProfileCommodities.Add(userProfileCommodity);
                db.SaveChanges();
                return RedirectToAction("TemporaryAdminIndex");
            }

            return View(commodity);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Commodities/Create
        public ActionResult Create()
        {
            if (GetInfo.GetRoleNameByUserName(User.Identity.Name) == "Administrator")
            {
                ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "ManufacturerId", "ManufacturerName");
                ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "CategoryName");
                List<SelectListItem> selList = InitialData();
                ViewBag.Color = selList;
                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Commodity commodity, HttpPostedFileBase Image, IEnumerable<HttpPostedFileBase> files, IEnumerable<CommodityInfo> commodityInfoList)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    commodity.ImageType = Image.ContentType;
                    commodity.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(commodity.ImageData, 0, Image.ContentLength);

                    Commodity dbEntry = new Commodity();
                    dbEntry.CommodityName = commodity.CommodityName;
                    dbEntry.SubCategoryId = commodity.SubCategoryId;
                    dbEntry.ManufacturerId = commodity.ManufacturerId;
                    dbEntry.UnitPrice = commodity.UnitPrice;
                    dbEntry.SubPrice = commodity.SubPrice;
                    dbEntry.DiscountPrice = commodity.DiscountPrice;
                    dbEntry.MakeCompany = commodity.MakeCompany;
                    dbEntry.Degree = commodity.Degree;
                    dbEntry.Quantity = commodity.Quantity;
                    dbEntry.Description = commodity.Description;
                    dbEntry.ImageData = commodity.ImageData;
                    dbEntry.ImageType = commodity.ImageType;
                    dbEntry.Brand = commodity.Brand;
                    dbEntry.IsRecommended = commodity.IsRecommended;
                    dbEntry.ManufacturerDate = commodity.ManufacturerDate;
                    dbEntry.PromotionStartOn = commodity.PromotionStartOn;
                    dbEntry.PromotionEndOn = commodity.PromotionEndOn;
                    dbEntry.Weight = commodity.Weight;
                    dbEntry.Component = commodity.Component;
                    dbEntry.CreationDate = System.DateTime.Now;
                    db.Commodities.Add(dbEntry);
                }

                var p = new List<CommodityInfo>();
                List<ImageViewModel> imageViewModel = new List<ImageViewModel>();
                ImageViewModel model = new ImageViewModel();

                if (files != null)
                {
                    foreach (var s in files)
                    {
                        if (s != null)
                        {
                            model.ImageType = s.ContentType;
                            model.ImageData = new byte[s.ContentLength];
                            s.InputStream.Read(model.ImageData, 0, s.ContentLength);

                            imageViewModel.Add(model);
                        }
                    }
                }

                int i = 0;
                if (commodityInfoList != null)
                {
                    foreach (var item in commodityInfoList)
                    {
                        for (; i < imageViewModel.Count; )
                        {
                            CommodityInfo commodityInfo = new CommodityInfo();
                            commodityInfo.CommodityId = commodity.CommodityId;
                            commodityInfo.Color = item.Color;
                            commodityInfo.Size = item.Size;
                            commodityInfo.Quantity = item.Quantity;
                            commodityInfo.Capacity = item.Capacity;
                            commodityInfo.ImageData = imageViewModel[i].ImageData;
                            commodityInfo.ImageType = imageViewModel[i].ImageType;

                            i++;
                            p.Add(commodityInfo);
                            break;
                        }
                    }
                }

                db.CommodityInfoes.AddRange(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commodity);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Image(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "Administrator")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Commodity commodity = this.db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }

            return View(commodity);
        }

        [Authorize(Roles = "TemporaryAdmin")]
        public ActionResult TemporaryAdminImage(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "TemporaryAdmin")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Commodity commodity = this.db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }

            return View(commodity);
        }

        public List<SelectListItem> InitialData()
        {
            List<SelectListItem> colorSelList = new List<SelectListItem>() { 
                new SelectListItem(){ Value = "red", Text="红色",},
                new SelectListItem(){ Value = "blue", Text = "蓝色"},
                new SelectListItem(){ Value = "black", Text = "黑色"},
                new SelectListItem(){ Value ="green", Text = "绿色"},
                new SelectListItem(){ Value ="gray", Text="灰色"},
                new SelectListItem(){ Value ="dark", Text="深色"},
                new SelectListItem(){ Value ="yellow", Text="黄色"},
                new SelectListItem(){ Value ="white", Text="白色"},
                new SelectListItem(){ Value ="orange", Text="橘黄色"}
            };
            return colorSelList;
        }

        public void InitialViewBag(Commodity commodity)
        {
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "ManufacturerId", "ManufacturerName", commodity.ManufacturerId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "CategoryName", commodity.SubCategoryId);
            List<SelectListItem> selList = InitialData();
            ViewBag.Color = selList;
        }

        public List<CommodityInfoCollectionViewModel> InitialDataBeforeEdit(int? id)
        {
            List<SelectListItem> selList = InitialData();
            var selectedCommodity = (from commodityInfo in this.db.CommodityInfoes
                                     where commodityInfo.CommodityId == id
                                     select new
                                     {
                                         Color = commodityInfo.Color,
                                         Size = commodityInfo.Size,
                                         Quantity = commodityInfo.Quantity.HasValue ? commodityInfo.Quantity : 0,
                                         ImageType = commodityInfo.ImageType,
                                         ImageData = commodityInfo.ImageData,
                                         CommodityInfoId = commodityInfo.CommodityInfoId,
                                         Capacity = commodityInfo.Capacity
                                     }).ToList();
            var collection = new List<CommodityInfoCollectionViewModel>();
            foreach (var item in selectedCommodity)
            {
                CommodityInfoCollectionViewModel model = new CommodityInfoCollectionViewModel();

                model.Color = new SelectList(selList, "Value", "Text", item.Color);
                model.Quantity = item.Quantity.Value;
                model.Size = item.Size;
                model.Capacity = item.Capacity;
                model.ImageData = item.ImageData;
                model.ImageType = item.ImageType;
                model.CommodityInfoId = item.CommodityInfoId;

                collection.Add(model);
            }

            return collection;
        }

        [Authorize(Roles = "TemporaryAdmin")]
        // GET: Commodities/Edit/5
        public ActionResult TemporaryAdminEdit(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "TemporaryAdmin")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }


            InitialViewBag(commodity);

            ViewBag.SelectedCommodity = InitialDataBeforeEdit(id);
            return View(commodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TemporaryAdminEdit(Commodity commodity, HttpPostedFileBase image, IEnumerable<HttpPostedFileBase> imageFiles, IEnumerable<CommodityInfo> commodityInfoList)
        {
            if (ModelState.IsValid)
            {
                Commodity dbEntry = this.db.Commodities.Single(x => x.CommodityId == commodity.CommodityId);
                if (dbEntry != null)
                {
                    if (image != null)
                    {
                        commodity.ImageType = image.ContentType;
                        commodity.ImageData = new byte[image.ContentLength];
                        image.InputStream.Read(commodity.ImageData, 0, image.ContentLength);
                    }

                    dbEntry.CommodityName = commodity.CommodityName;
                    dbEntry.SubCategoryId = commodity.SubCategoryId;
                    dbEntry.ManufacturerId = commodity.ManufacturerId;
                    dbEntry.UnitPrice = commodity.UnitPrice;
                    dbEntry.SubPrice = commodity.SubPrice;
                    dbEntry.DiscountPrice = commodity.DiscountPrice;
                    dbEntry.MakeCompany = commodity.MakeCompany;
                    dbEntry.Degree = commodity.Degree;
                    dbEntry.Quantity = commodity.Quantity;
                    dbEntry.Description = commodity.Description;
                    dbEntry.Brand = commodity.Brand;
                    dbEntry.IsRecommended = commodity.IsRecommended;
                    dbEntry.ManufacturerDate = commodity.ManufacturerDate;
                    dbEntry.ImageData = commodity.ImageData;
                    dbEntry.ImageType = commodity.ImageType;
                    dbEntry.Component = commodity.Component;
                    dbEntry.PromotionEndOn = commodity.PromotionEndOn;
                    dbEntry.PromotionStartOn = commodity.PromotionStartOn;
                    dbEntry.Weight = commodity.Weight;
                    dbEntry.UpdateDate = System.DateTime.Now;

                }

                //Delete related info
                var commodityInfoes = (from commodityInfo in this.db.CommodityInfoes
                                       where commodityInfo.CommodityId == commodity.CommodityId
                                       select commodityInfo).ToList();
                this.db.CommodityInfoes.RemoveRange(commodityInfoes);

                var p = new List<CommodityInfo>();
                List<ImageViewModel> imageViewModel = new List<ImageViewModel>();
                ImageViewModel model = new ImageViewModel();
                //read image 
                if (imageFiles != null)
                {
                    foreach (var s in imageFiles)
                    {
                        if (s != null)
                        {
                            model.ImageType = s.ContentType;
                            model.ImageData = new byte[s.ContentLength];
                            s.InputStream.Read(model.ImageData, 0, s.ContentLength);

                            imageViewModel.Add(model);
                        }
                    }
                }

                //add commodityInfo
                int i = 0;
                int count = 0;
                if (commodityInfoList != null)
                {
                    foreach (var item in commodityInfoList)
                    {
                        for (; i < commodityInfoList.Count(); )
                        {
                            CommodityInfo commodityInfo = new CommodityInfo();
                            commodityInfo.CommodityId = commodity.CommodityId;
                            commodityInfo.Color = item.Color;
                            commodityInfo.Size = item.Size;
                            commodityInfo.Quantity = item.Quantity;
                            commodityInfo.Capacity = item.Capacity;
                            if (imageViewModel.Count > 0)
                            {
                                commodityInfo.ImageData = imageViewModel[i].ImageData;
                                commodityInfo.ImageType = imageViewModel[i].ImageType;
                            }

                            i++;
                            count = count + item.Quantity.Value;
                            p.Add(commodityInfo);
                            break;
                        }
                    }
                }

                //check quantity 
                if (commodity.Quantity != count)
                {
                    this.TemporaryAdminEdit(commodity.CommodityId);
                    Response.Write("<script> alert('请确保商品总数大于各子类总数之和！')</script>");
                }
                else
                {
                    db.CommodityInfoes.AddRange(p);
                    db.SaveChanges();
                    return RedirectToAction("TemporaryAdminIndex");
                }
            }

            return View(commodity);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Commodities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "Administrator")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }

            InitialViewBag(commodity);

            ViewBag.SelectedCommodity = InitialDataBeforeEdit(id); ;
            return View(commodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Commodity commodity, HttpPostedFileBase image, IEnumerable<HttpPostedFileBase> imageFiles, IEnumerable<CommodityInfo> commodityInfoList)
        {
            if (ModelState.IsValid)
            {
                Commodity dbEntry = this.db.Commodities.Single(x => x.CommodityId == commodity.CommodityId);
                if (dbEntry != null)
                {
                    if (image != null)
                    {
                        commodity.ImageType = image.ContentType;
                        commodity.ImageData = new byte[image.ContentLength];
                        image.InputStream.Read(commodity.ImageData, 0, image.ContentLength);
                    }

                    dbEntry.CommodityName = commodity.CommodityName;
                    dbEntry.SubCategoryId = commodity.SubCategoryId;
                    dbEntry.ManufacturerId = commodity.ManufacturerId;
                    dbEntry.UnitPrice = commodity.UnitPrice;
                    dbEntry.SubPrice = commodity.SubPrice;
                    dbEntry.DiscountPrice = commodity.DiscountPrice;
                    dbEntry.MakeCompany = commodity.MakeCompany;
                    dbEntry.Degree = commodity.Degree;
                    dbEntry.Quantity = commodity.Quantity;
                    dbEntry.Description = commodity.Description;
                    dbEntry.Brand = commodity.Brand;
                    dbEntry.IsRecommended = commodity.IsRecommended;
                    dbEntry.ManufacturerDate = commodity.ManufacturerDate;
                    dbEntry.ImageData = commodity.ImageData;
                    dbEntry.ImageType = commodity.ImageType;
                    dbEntry.Component = commodity.Component;
                    dbEntry.PromotionEndOn = commodity.PromotionEndOn;
                    dbEntry.PromotionStartOn = commodity.PromotionStartOn;
                    dbEntry.Weight = commodity.Weight;
                    dbEntry.UpdateDate = System.DateTime.Now;

                }

                //Delete related info
                var commodityInfoes = (from commodityInfo in this.db.CommodityInfoes
                                       where commodityInfo.CommodityId == commodity.CommodityId
                                       select commodityInfo).ToList();
                this.db.CommodityInfoes.RemoveRange(commodityInfoes);

                List<CommodityInfo> p = new List<CommodityInfo>();
                List<ImageViewModel> imageViewModel = new List<ImageViewModel>();
                ImageViewModel model = new ImageViewModel();
                //read image 
                if (imageFiles != null)
                {
                    foreach (var s in imageFiles)
                    {
                        if (s != null)
                        {
                            model.ImageType = s.ContentType;
                            model.ImageData = new byte[s.ContentLength];
                            s.InputStream.Read(model.ImageData, 0, s.ContentLength);

                            imageViewModel.Add(model);
                        }
                    }
                }

                //add commodityInfo
                int i = 0;
                int count = 0;
                if (commodityInfoList != null)
                {
                    foreach (var item in commodityInfoList)
                    {
                        for (; i < commodityInfoList.Count(); )
                        {
                            CommodityInfo commodityInfo = new CommodityInfo();
                            commodityInfo.CommodityId = commodity.CommodityId;
                            commodityInfo.Color = item.Color;
                            commodityInfo.Size = item.Size;
                            commodityInfo.Quantity = item.Quantity;
                            commodityInfo.Capacity = item.Capacity;
                            if (imageViewModel.Count > 0)
                            {
                                commodityInfo.ImageData = imageViewModel[i].ImageData;
                                commodityInfo.ImageType = imageViewModel[i].ImageType;
                            }

                            i++;
                            count = count + item.Quantity.Value;
                            p.Add(commodityInfo);
                            break;
                        }
                    }
                }

                //check quantity 
                if (commodity.Quantity != count)
                {
                    this.Edit(commodity.CommodityId);
                    Response.Write("<script> alert('请确保商品总数大于各子类总数之和！')</script>");
                }
                else
                {
                    db.CommodityInfoes.AddRange(p);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(commodity);
        }

        [Authorize(Roles = "TemporaryAdmin")]
        public ActionResult TemporaryAdminDelete(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "TemporaryAdmin")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }

            return View(commodity);
        }



        [HttpPost, ActionName("TemporaryAdminDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult TemporaryAdminDeleteConfirmed(int id)
        {
            Commodity commodity = db.Commodities.Find(id);
            db.UserProfileCommodities.RemoveRange(commodity.UserProfileCommoditys);
            db.Commodities.Remove(commodity);
            db.SaveChanges();
            return RedirectToAction("TemporaryAdminIndex");
        }



        [Authorize(Roles = "Administrator")]
        // GET: Commodities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "Administrator")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }

            return View(commodity);
        }

        // POST: Commodities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commodity commodity = db.Commodities.Find(id);
            db.Commodities.Remove(commodity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
