using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Academy08._04.Models
{
    public class Marks
    {
        public int? UserId { get; set; }
        public int? SubjectId { get; set; }
        public DateTime Date { get; set; }
        public int Mark { get; set; }
    }

}