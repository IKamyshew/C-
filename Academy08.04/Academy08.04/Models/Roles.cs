using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Academy08._04.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum RoleNames
    {
        Manager = 1,
        Teacher = 2,
        Student = 3
    }
}