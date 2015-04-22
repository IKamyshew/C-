using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy08._04.Models
{
    public class Marks
    {
        [Key]
        [Column(Order = 0)]
        public int? StudentId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Date { get; set; }

        public int Mark { get; set; }
    }

}