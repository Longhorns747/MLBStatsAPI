using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLBStatsAPI.Models
{
    public class Pitcher
    {
        public Dictionary<int, Dictionary<string, string>> yearRecords { get; set; }
        public string nameLast { get; set; }
        public string nameFirst { get; set; }
    }
}