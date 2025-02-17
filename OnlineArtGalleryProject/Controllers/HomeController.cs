using OnlineArtGalleryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace OnlineArtGalleryProject.Controllers
{
    public class HomeController : Controller
    {

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }




        public ActionResult GiftCard()
        {
            return View(new GiftCardModel());
        }

        // POST: /Home/SendGiftCard
        [HttpPost]
        public ActionResult SendGiftCard(GiftCardModel giftCard)
        {
            if (ModelState.IsValid)
            {
                
                SendEmailWithGiftCard(giftCard);

                return RedirectToAction("Index", "Home");
            }

            return View("GiftCard", giftCard);
        }

       
        private void SendEmailWithGiftCard(GiftCardModel giftCard)
        {
            
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("onlineartgallery211151@gmail.com");
            mail.To.Add(giftCard.RecipientEmail);
            mail.Subject = "Gift Card";
            mail.Body = $"Dear {giftCard.RecipientName},\n\nYou recieved a gift card worth ${giftCard.Value} from {giftCard.SenderName}.\nYou can use your gift card at any time on our website, no later than one month from the day it was given.\nWe hope you like the gift card and buy yourself something nice.\n\nBest wishes,\nOnline art gallery";

            SmtpClient smtp = new SmtpClient("smtp.sendgrid.net");
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("apikey", "APIKEY_SECRET");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }


       
    }
}