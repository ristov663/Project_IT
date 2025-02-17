using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineArtGalleryProject.Models
{
    public class OrderModel
    {

        [Required(ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your address.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public decimal TotalAmount { get; set; }
    }
}