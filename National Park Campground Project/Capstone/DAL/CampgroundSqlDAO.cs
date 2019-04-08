using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;

        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        /// <summary>
        /// Returns a list of all the campgrounds.
        /// </summary>
        /// <returns></returns>
        public IList<Campground> GetCampgrounds(int parkId)
        {
            List<Campground> getCampgrounds = new List<Campground>();

            try
            {
                // Create a connection to the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // Returns sql query command that gives list of available campgrounds information for chosen park.
                    SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @parkId;", conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    // Execute the command
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Read each row
                    while (reader.Read())
                    {
                        Campground campground = ConvertReaderToCampground(reader);
                        getCampgrounds.Add(campground);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred communicating with the database. ");
                Console.WriteLine(ex.Message);
                throw;
            }
            return getCampgrounds;
        }

        private string MonthConversion(int month)
        {
            Dictionary<int, string> monthName = new Dictionary<int, string>()
            {
                { 1, "January" },
                { 2, "February" },
                { 3, "March" },
                { 4, "April" },
                {5, "May" },
                { 6, "June"},
                { 7, "July"},
                { 8, "August"},
                { 9, "September"},
                { 10, "October"},
                { 11, "November"},
                { 12, "December"},
            };

            string monthWritten = monthName[month];
            return monthWritten;
        }

        private Campground ConvertReaderToCampground(SqlDataReader reader)
        {
            Campground campground = new Campground();

            campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            campground.ParkId = Convert.ToInt32(reader["park_id"]);
            campground.Name = Convert.ToString(reader["name"]);
            campground.OpenFrom = Convert.ToInt32(reader["open_from_mm"]);
            campground.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
            campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
            campground.FromMonth = MonthConversion(campground.OpenFrom);
            campground.ToMonth = MonthConversion(campground.OpenTo);

            return campground;
        }
    }
}
