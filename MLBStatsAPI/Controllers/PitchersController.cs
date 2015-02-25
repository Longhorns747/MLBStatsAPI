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
    public class PitchersController : ApiController
    {
        string[] STATS = { "teamID", "G", "W", "L", "CAST(IPOuts / 3.0 as decimal(38,1)) AS IP", "GS", "SO", "BB",
                             "CG", "SHO", "SV", "ERA", "BAOpp", 
                             @"CAST((SO*9.0) / CAST(IPOuts / 3.0 as decimal(38,1)) as decimal(38, 2)) AS Kper9",
                             @"CAST((BB*9.0) / CAST(IPOuts / 3.0 as decimal(38,1)) as decimal(38, 2)) AS BBper9"
                         };
        public IHttpActionResult GetPitcher(string firstName, string lastName)
        {
            string statString = "";

            foreach (string stat in STATS)
            {
                statString += stat + ", ";
            }

            statString = statString.Trim();
            statString = statString.TrimEnd(',');

            string query = @"SELECT nameLast, nameFirst, yearID, " + statString +
                @" FROM localbaseballdb.master as m join localbaseballdb.pitching as p on m.playerID = p.playerID
                WHERE nameLast = '" + lastName + "' and nameFirst = '" + firstName + "'";

            SqlDataReader reader = DBUtils.dbConnect(query);
            DataTable schemaTable = reader.GetSchemaTable();
            // Data is accessible through the DataReader object here.
            Pitcher pitchingData = new Pitcher();
            pitchingData.yearRecords = new Dictionary<int, Dictionary<string, string>>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    pitchingData.nameLast = (pitchingData.nameLast == null) ? reader["nameLast"].ToString() : pitchingData.nameLast;
                    pitchingData.nameFirst = (pitchingData.nameFirst == null) ? reader["nameFirst"].ToString() : pitchingData.nameFirst;
                    Dictionary<string, string> currYear = new Dictionary<string, string>();

                    foreach (string stat in STATS)
                    {
                        string statIdx = stat.Split(' ').Last();
                        currYear[statIdx] = reader[statIdx].ToString();
                    }

                    pitchingData.yearRecords[(int)reader["yearId"]] = currYear;
                }
            }

            reader.Close();

            if (pitchingData.nameLast == null)
            {
                return NotFound();
            }
            return Ok(pitchingData);
        }
    }
}
