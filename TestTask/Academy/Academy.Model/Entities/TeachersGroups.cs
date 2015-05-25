using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Model.Entities
{
    public class TeachersGroups
    {
        [Key]
        [Column(Order = 0)]
        public int TeacherId { get; set; }
        public User Teacher { get; set; }

        [Key]
        [Column(Order = 1)]
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
