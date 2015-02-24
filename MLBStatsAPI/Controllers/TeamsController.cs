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

            SqlDataReader reader = dbConnect(query);
            DataTable schemaTable = reader.GetSchemaTable();
            // Data is accessible through the DataReader object here.
            Team teamData = new Team();
            teamData.yearRecords = new Dictionary<int, Dictionary<string, string>>();

            while (reader.Read())
            {
                teamData.name = (teamData.name == null) ? (string)reader["name"] : teamData.name;
                Dictionary<string, string> currYear = new Dictionary<string, string>();

                foreach (string stat in STATS)
                {
                    string statIdx = stat.Split(' ').Last(); 
                    currYear[statIdx] = reader[statIdx].ToString();
                }

                teamData.yearRecords[(int)reader["yearId"]] = currYear;
            }

            reader.Close();

            if (teamData == null)
            {
                return NotFound();
            }
            return Ok(teamData);
        }

        public SqlDataReader dbConnect(string query)
        {
            string connStr = ConfigurationManager.ConnectionStrings["dbConnectionStr"].ConnectionString;

            SqlConnection sqlConnection1 = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            reader = cmd.ExecuteReader();
            return reader;
        }
    }
}
