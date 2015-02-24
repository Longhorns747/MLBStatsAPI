using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MLBStatsAPI.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MLBStatsAPI.Utilities;

namespace MLBStatsAPI.Controllers
{
    public class TeamsController : ApiController
    {
        string[] STATS = { "W", "L", "R", "RA", "HR", "FP",
                             "CAST(ROUND((H + 0.0) / (AB + 0.0), 3) as decimal(38, 3)) AS AVG", "CAST(ERA as decimal(38, 2)) AS ERA" };
        public IHttpActionResult GetTeam(string id)
        {
            string statString = "";

            foreach(string stat in STATS) {
                statString += stat + ", ";
            }

            statString = statString.Trim();
            statString = statString.TrimEnd(',');

            string query = @"SELECT TOP 10 name, yearID," + statString +
                @" FROM localbaseballdb.teams as t
                WHERE t.teamID = '" + id + @"'
                ORDER BY t.yearID desc";

            SqlDataReader reader = DBUtils.dbConnect(query);
            DataTable schemaTable = reader.GetSchemaTable();
            // Data is accessible through the DataReader object here.
            Team teamData = new Team();
            teamData.yearRecords = new Dictionary<int, Dictionary<string, string>>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    teamData.name = (teamData.name == null) ? reader["name"].ToString() : teamData.name;
                    Dictionary<string, string> currYear = new Dictionary<string, string>();

                    foreach (string stat in STATS)
                    {
                        string statIdx = stat.Split(' ').Last();
                        currYear[statIdx] = reader[statIdx].ToString();
                    }

                    teamData.yearRecords[(int)reader["yearId"]] = currYear;
                }
            }

            reader.Close();

            if (teamData.name == null)
            {
                return NotFound();
            }
            return Ok(teamData);
        }
    }
}
