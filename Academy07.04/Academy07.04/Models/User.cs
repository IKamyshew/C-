using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Academy07._04.Models
{
    public class User
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "First name, Last name")]
        [MaxLength(50, ErrorMessage = "Max length is 50 symbols")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Login")]
        [MaxLength(50, ErrorMessage = "Max length is 50 symbols")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        [MaxLength(50, ErrorMessage = "Max length is 50 symbols")]
        public string Password { get; set; }


    }
}