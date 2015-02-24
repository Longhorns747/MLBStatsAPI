using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLBStatsAPI.Models
{
    public class Team
    {
        public Dictionary<int, Dictionary<string, string>> yearRecords { get; set; }
        public String name { get; set; }
    }
}