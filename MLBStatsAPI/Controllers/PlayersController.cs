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
    public class PlayersController : ApiController
    {
        protected string[] stats;
        protected string table;

        public Player RetrievePlayer(string firstName, string lastName)
        {
            string statString = "";

            foreach (string stat in stats)
            {
                statString += stat + ", ";
            }

            statString = statString.Trim();
            statString = statString.TrimEnd(',');

            string query = @"SELECT nameLast, nameFirst, yearID, " + statString +
                @" FROM localbaseballdb.master as m join localbaseballdb." + table + @" as t on m.playerID = t.playerID
                WHERE nameLast = '" + lastName + "' and nameFirst = '" + firstName + "'";

            SqlDataReader reader = DBUtils.dbConnect(query);
            DataTable schemaTable = reader.GetSchemaTable();
            // Data is accessible through the DataReader object here.
            Player playerData = new Player();
            playerData.yearRecords = new Dictionary<int, Dictionary<string, string>>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    playerData.nameLast = (playerData.nameLast == null) ? reader["nameLast"].ToString() : playerData.nameLast;
                    playerData.nameFirst = (playerData.nameFirst == null) ? reader["nameFirst"].ToString() : playerData.nameFirst;
                    Dictionary<string, string> currYear = new Dictionary<string, string>();

                    foreach (string stat in stats)
                    {
                        string statIdx = stat.Split(' ').Last();
                        currYear[statIdx] = reader[statIdx].ToString();
                    }

                    playerData.yearRecords[(int)reader["yearId"]] = currYear;
                }
            }

            reader.Close();

            return playerData;
        }
    }
}
