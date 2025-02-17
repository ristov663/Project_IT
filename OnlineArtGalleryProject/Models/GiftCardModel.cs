using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineArtGalleryProject.Models
{
    public class GiftCardModel
    {

        [Required(ErrorMessage = "Please enter the recipient's name.")]
        public string RecipientName { get; set; }
        
        [Required(ErrorMessage = "Please enter the sender's name.")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Please enter the value of the gift card.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than 0.")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "Please enter the recipient's email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string RecipientEmail { get; set; }
    }
}