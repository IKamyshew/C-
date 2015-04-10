using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy08._04.Models
{
    public class TeachersGroups
    {
        [Key][Column(Order = 0)]
        public int? TeacherId { get; set; }
        [Key][Column(Order = 1)]
        public int? GroupId { get; set; }
    }
}