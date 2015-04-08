using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Academy08._04.Models
{
    public class Schedule
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Lesson { get; set; }

        public int? Groupid { get; set; }

        public int? SubjectId { get; set; }

        public int Classroom { get; set; }

    }
}