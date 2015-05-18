using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Academy.Model.Entities
{
    public class Role
    {
        public int Id { get; set; }

        [DisplayName("Role")]
        public string Name { get; set; }
    }
}
