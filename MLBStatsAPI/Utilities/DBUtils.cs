using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MLBStatsAPI.Utilities
{
    public class DBUtils
    {
        public static SqlDataReader dbConnect(string query)
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