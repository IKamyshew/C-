using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Academy.Model.Entities
{
    public class AcademyContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
    }
}
