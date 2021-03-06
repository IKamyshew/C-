﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy08._04.Models
{
    public class Schedule
    {
        [Key][Column(Order = 0)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Key][Column(Order = 1)]
        [Required]
        public int Lesson { get; set; }

        [Key][Column(Order = 2)]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Required(ErrorMessage="Set classroom")]
        public int Classroom { get; set; }

        public readonly static  int MaxLessonsPerDay = 8;

    }
}