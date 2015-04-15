using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Academy08._04.Models
{
    public class Group
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Group")]
        [MaxLength(50, ErrorMessage = "Maximum length Exceeded")]
        public string Name { get; set; }
    }
}