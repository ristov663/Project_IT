using OnlineArtGalleryProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace OnlineArtGalleryProject.Controllers
{
    public class ShoppingCartController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            
            List<int> cartItems = (List<int>)Session["CartItems"];
            List<Painting> paintingsInCart = new List<Painting>();

            if (cartItems != null)
            {
                foreach (int itemId in cartItems)
                {
                    
                    Painting painting = db.Paintings.Find(itemId);
                    paintingsInCart.Add(painting);
                }
            }

            return View(paintingsInCart);
        }





        [HttpPost]
        public ActionResult DeleteFromCart(int id)
        {
            try
            {
                if (Session["CartItems"] != null)
                {
                    List<int> cartItems = (List<int>)Session["CartItems"];
                    cartItems.Remove(id);
                    Session["CartItems"] = cartItems;
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ErrorMessage = ex.Message });
            }
        }


        public ActionResult Order()
        {
            return View(new OrderModel());
        }


        [HttpPost]
        public ActionResult ProcessOrder(OrderModel order)
        {


            if (ModelState.IsValid)
            {
                SendEmail(order);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Order", order);
            }

        }

        private void SendEmail(OrderModel order)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("onlineartgallery211151@gmail.com");
            mail.To.Add(order.Email);
            mail.Subject = "Order Confirmation";
            mail.Body = $"Dear {order.FirstName},\n\nYour order has been successfully placed.\nExpect your order to arrive in a maximum of 7 days from the day of ordering. Thank you for being our customers.\n\nBest regards,\nOnline art gallery";

            SmtpClient smtp = new SmtpClient("smtp.sendgrid.net");
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("apikey", "APIKEY_SECRET");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }



    }
}