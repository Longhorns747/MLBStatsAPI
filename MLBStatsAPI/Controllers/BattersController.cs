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
    public class BattersController : ApiController
    {
        string[] STATS = { "teamID", "G", "AB", "H", "HR", "RBI", "SB", "BB",
                             @"CAST(ROUND((H + 0.0) / (AB + 0.0), 3) as decimal(38, 3)) AS AVG",
                             @"CAST(ROUND((H+BB+HBP + 0.0) / (AB+BB+HBP+COALESCE(SF,0) + 0.0), 3) as decimal(38, 3)) AS OBP",
                             @"CAST(ROUND(((H + ""2B"" + 2 * ""3B"" + 3 * HR) + 0.0) / (AB + 0.0), 3) as decimal(38, 3)) AS SLG" };
        public IHttpActionResult GetTeam(string firstName, string lastName)
        {
            string statString = "";

            foreach (string stat in STATS)
            {
                statString += stat + ", ";
            }

            statString = statString.Trim();
            statString = statString.TrimEnd(',');

            string query = @"SELECT nameLast, nameFirst, yearID, " + statString +
                @" FROM localbaseballdb.master as m join localbaseballdb.batting as b on m.playerID = b.playerID
                WHERE nameLast = '" + lastName + "' and nameFirst = '" + firstName + "'";

            SqlDataReader reader = DBUtils.dbConnect(query);
            DataTable schemaTable = reader.GetSchemaTable();
            // Data is accessible through the DataReader object here.
            Batter battingData = new Batter();
            battingData.yearRecords = new Dictionary<int, Dictionary<string, string>>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    battingData.nameLast = (battingData.nameLast == null) ? reader["nameLast"].ToString() : battingData.nameLast;
                    battingData.nameFirst = (battingData.nameFirst == null) ? reader["nameFirst"].ToString() : battingData.nameFirst;
                    Dictionary<string, string> currYear = new Dictionary<string, string>();

                    foreach (string stat in STATS)
                    {
                        string statIdx = stat.Split(' ').Last();
                        currYear[statIdx] = reader[statIdx].ToString();
                    }

                    battingData.yearRecords[(int)reader["yearId"]] = currYear;
                }
            }

            reader.Close();

            if (battingData.nameLast == null)
            {
                return NotFound();
            }
            return Ok(battingData);
        }
    }
}
