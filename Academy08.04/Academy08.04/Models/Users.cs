﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Academy08._04.Models
{
    public class Users
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Login")]
        [MaxLength(50, ErrorMessage = "Maximum length Exceeded")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        [MaxLength(50, ErrorMessage = "Maximum length Exceeded")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "First name")]
        [MaxLength(50, ErrorMessage = "Maximum length Exceeded")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [MaxLength(50, ErrorMessage = "Maximum length Exceeded")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int? RoleId { get; set; }
        public Roles Role { get; set; }

        [Display(Name = "Group")]
        [MaxLength(50, ErrorMessage = "Maximum length Exceeded")]
        public int? GroupId { get; set; }
        public Groups Group { get; set; }

    }
}