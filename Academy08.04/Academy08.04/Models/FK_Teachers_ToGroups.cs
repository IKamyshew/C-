﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Academy08._04.Models
{
    public class FK_Teachers_ToGroups
    {
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
    }
}