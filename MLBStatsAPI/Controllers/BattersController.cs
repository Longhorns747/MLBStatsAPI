using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MLBStatsAPI.Models;
using MLBStatsAPI.Utilities;
using System.Data.SqlClient;
using System.Data;

namespace MLBStatsAPI.Controllers
{
    public class BattersController : PlayersController
    {
        public IHttpActionResult GetBatter(string firstName, string lastName)
        {
            stats = new string[] { "teamID", "G", "AB", "H", "HR", "RBI", "SB", "BB",
                             @"CAST(ROUND((H + 0.0) / (AB + 0.0), 3) as decimal(38, 3)) AS AVG",
                             @"CAST(ROUND((H+BB+HBP + 0.0) / (AB+BB+HBP+COALESCE(SF,0) + 0.0), 3) as decimal(38, 3)) AS OBP",
                             @"CAST(ROUND(((H + ""2B"" + 2 * ""3B"" + 3 * HR) + 0.0) / (AB + 0.0), 3) as decimal(38, 3)) AS SLG",
                             @"CAST(CAST(ROUND(((H + ""2B"" + 2 * ""3B"" + 3 * HR) + 0.0) / (AB + 0.0), 3) as decimal(38, 3)) 
                                - CAST(ROUND((H + 0.0) / (AB + 0.0), 3) as decimal(38, 3)) as decimal(38, 3)) AS ISO",
                             @"CAST((H - HR + 0.0)/(AB - SO - HR + SF) as decimal(38, 3)) AS BABIP"};
            table = "batting";

            Player battingData = RetrievePlayer(firstName, lastName);

            if (battingData.nameLast == null)
            {
                return NotFound();
            }
            return Ok(battingData);
        }
    }
}
