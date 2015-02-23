using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using MLBStatsAPI.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace MLBStatsAPI.Controllers
{
    public class TeamsController : ApiController
    {
        public IHttpActionResult GetTeam(string id)
        {
            string query = @"SELECT TOP 10 name, yearID, W, L, R
                FROM localbaseballdb.teams as t
                WHERE t.teamID = '" + id + @"'
                ORDER BY t.yearID desc";

            string connStr = ConfigurationManager.ConnectionStrings["dbConnectionStr"].ConnectionString;

            SqlConnection sqlConnection1 = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.
            List<TeamYear> yearStats = new List<TeamYear>();
            Team teamData = new Team();

            while (reader.Read())
            {
                teamData.name = (teamData.name == null) ? reader.GetString(0) : teamData.name;

                TeamYear currYear = new TeamYear();
                currYear.year = reader.GetInt32(1);
                currYear.W = reader.GetInt32(2);
                currYear.L = reader.GetInt32(3);
                currYear.R = reader.GetInt32(4);
                yearStats.Add(currYear);
            }

            teamData.yearRecords = yearStats;

            sqlConnection1.Close();

            if (teamData == null)
            {
                return NotFound();
            }
            return Ok(teamData);
        }
    }
}
