﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Academy.Model.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }

    }
}
