using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office.CustomUI;
using OnlineArtGalleryProject.Models;
using OnlineArtGalleryProject.ViewModel;
using OnlineArtGalleryProject.Controllers;
using OnlineArtGalleryProject.Migrations;

namespace OnlineArtGalleryProject.Controllers
{
    public class PaintingsController : Controller
    {

        

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Paintings
        public ActionResult Index()
        {
            var paintings = db.Paintings.Include(p => p.ArtGallery);
            return View(paintings.ToList());
        }

        // GET: Paintings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Painting painting = db.Paintings.Find(id);
            if (painting == null)
            {
                return HttpNotFound();
            }
            return View(painting);
        }

        // GET: Paintings/Create
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult Create()
        {
            ViewBag.ArtGalleryId = new SelectList(db.ArtGalleries, "Id", "Name");
            return View();
        }

        // POST: Paintings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,Category,ImageURL,ArtGalleryId")] Painting painting)
        {
            if (ModelState.IsValid)
            {
                db.Paintings.Add(painting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtGalleryId = new SelectList(db.ArtGalleries, "Id", "Name", painting.ArtGalleryId);
            return View(painting);
        }

        // GET: Paintings/Edit/5
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Painting painting = db.Paintings.Find(id);
            if (painting == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtGalleryId = new SelectList(db.ArtGalleries, "Id", "Name", painting.ArtGalleryId);
            return View(painting);
        }

        // POST: Paintings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,Category,ImageURL,ArtGalleryId")] Painting painting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(painting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtGalleryId = new SelectList(db.ArtGalleries, "Id", "Name", painting.ArtGalleryId);
            return View(painting);
        }

        // GET: Paintings/Delete/5
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Painting painting = db.Paintings.Find(id);
            if (painting == null)
            {
                return HttpNotFound();
            }
            return View(painting);
        }

        // POST: Paintings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult DeleteConfirmed(int id)
        {
            Painting painting = db.Paintings.Find(id);
            db.Paintings.Remove(painting);
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

        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult AddNewArtist(int id)
        {
            PaintingArtist model = new PaintingArtist();
            model.PaintingId = id;
            model.Artists = db.Artists.ToList();
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult AddNewArtist(PaintingArtist model)
        {
            var artist = db.Artists.Find(model.ArtistId);
            var painting = db.Paintings.Find(model.PaintingId);
            painting.Artists.Add(artist);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = model.PaintingId });
        }



        public ActionResult DeleteArtist(int paintingId, int artistId)
        {
            var artist = db.Artists.Find(artistId);
            var painting = db.Paintings.Find(paintingId);
            painting.Artists.Remove(artist);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = paintingId });
        }



       


        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            // Прво, треба да провериме дали веќе постои сесија за кошничката
            if (Session["CartItems"] == null)
            {
                // Ако не постои, креираме нова листа за кошничката
                List<int> cartItems = new List<int>();
                // Додаваме го делото во кошничката
                cartItems.Add(id);
                // Го зачувуваме во сесијата
                Session["CartItems"] = cartItems;
            }
            else
            {
                // Ако постои, додаваме го делото во постоечката кошничка
                List<int> cartItems = (List<int>)Session["CartItems"];
                cartItems.Add(id);
                Session["CartItems"] = cartItems;
            }

            // Врати празен JSON објект како одговор
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public ActionResult SubmitCommentAndRating(string comment, int rating)
        {
            try
            {
                // Тука додадете код за зачувување на коментарот и оценката
                // Пример:
                // CommentRepository.SaveCommentAndRating(comment, rating);
                // или
                // PaintingService.SubmitCommentAndRating(comment, rating);

                // Вратете успех како JSON објект
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Ако се јави грешка, вратете грешка како JSON објект
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }





        /*[HttpPost]
        public JsonResult Index(string ItemId)
        {
            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Item objItem = objECartDbEntities.Items.Single(model => model.ItemId.ToString() == ItemId);
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            }
            if (listOfShoppingCartModels.Any(model => model.ItemId == ItemId))
            {
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ItemId == ItemId);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ImagePath = objItem.ImageURL;
                objShoppingCartModel.ItemName = objItem.IName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.Price;
                objShoppingCartModel.UnitPrice = objItem.Price;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartItem"] = listOfShoppingCartModels;
            return Json(new { Success = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ShoppingCart()
        {
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            return View(listOfShoppingCartModels);
        }

        [HttpPost]
        public ActionResult AddOrder()
        {
            int OrderId = 0;
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            Order orderObj = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyHHmmsss}", DateTime.Now)
            };
            objECartDbEntities.Orders.Add(orderObj);
            objECartDbEntities.SaveChanges();
            OrderId = orderObj.OrderId;

            foreach (var item in listOfShoppingCartModels)
            {
                OrderDetails objOrderDetail = new OrderDetails();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = OrderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objECartDbEntities.OrderDetails.Add(objOrderDetail);
                objECartDbEntities.SaveChanges();
            }

            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            return RedirectToAction("Index");
        }*/

    }
}
