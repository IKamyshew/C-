using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Academy08._04.Models
{
    public class ScheduleView
    {
        public DateTime date;
        public int group;

        public List<Schedule> schedules = new List<Schedule>(8);

        public ScheduleView() { }

        public ScheduleView(DateTime date, int? group, List<Schedule> schedules)
        {
            this.date = date;
            this.group = (int)group;
            this.schedules = schedules;
        }
    }
}