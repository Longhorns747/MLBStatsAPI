using MLBStatsAPI.Models;
using MLBStatsAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MLBStatsAPI.Controllers
{
    public class PitchersController : PlayersController
    {
        public IHttpActionResult GetPitcher(string firstName, string lastName)
        {
            stats = new string[] { "teamID", "G", "W", "L", "CAST(IPOuts / 3.0 as decimal(38,1)) AS IP", "GS", "SO", "BB",
                             "CG", "SHO", "SV", "ERA", "BAOpp",
                             @"CAST((SO*9.0) / CAST(IPOuts / 3.0 as decimal(38,1)) as decimal(38, 2)) AS Kper9",
                             @"CAST((BB*9.0) / CAST(IPOuts / 3.0 as decimal(38,1)) as decimal(38, 2)) AS BBper9",
                             @"CAST((H - HR + 0.0)/((BFP - HBP - IBB - BB - SH) - SO - HR + SF) as decimal(38, 3)) AS BABIP"
                         };
            table = "pitching";
            
            Player pitchingData = RetrievePlayer(firstName, lastName);

            if (pitchingData.nameLast == null)
            {
                return NotFound();
            }
            return Ok(pitchingData);
        }
    }
}
