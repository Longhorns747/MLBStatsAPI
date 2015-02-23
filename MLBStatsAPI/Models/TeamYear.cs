using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLBStatsAPI.Models
{
    public class TeamYear
    {
        public int year { get; set; }
        public int W { get; set; }
        public int L { get; set; }
        public int R { get; set; }
    }
}