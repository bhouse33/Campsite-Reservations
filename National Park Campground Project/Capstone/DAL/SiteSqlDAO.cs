using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAL
{

    public class SiteSqlDAO : ISiteDAO
    {
        private string connectionString;

        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Gets a list of available dates for a chosen campground's sites.
        /// </summary>
        /// <param name="campgroundId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IList<Site> GetAvailableSites(int campgroundId, DateTime startDate, DateTime endDate)
        {
            List<Site> availableSites = new List<Site>();

            try
            {
                // Create a connection to the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // Create a command to send to the database
                    SqlCommand cmd = new SqlCommand(@"select top 5 * from site as s 
                                where campground_id = @camp and site_id not in 
                                (select s.site_id from site as s 
                                join reservation as r on r.site_id = s.site_id 
                                where s.campground_id = @camp 
                                AND @startDate < r.to_date 
                                AND @endDate > r.from_date)
                                order by s.site_id"
                                        , conn);
                    cmd.Parameters.AddWithValue("@camp", campgroundId);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    // Execute the command
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Read each row
                    while (reader.Read())
                    {
                        Site site = ConvertReaderToSite(reader);
                        availableSites.Add(site);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred communicating with the database. ");
                Console.WriteLine(ex.Message);
                throw;
            }
            return availableSites;
        }

        private Site ConvertReaderToSite(SqlDataReader reader)
        {
            Site site = new Site();

            site.SiteId = Convert.ToInt32(reader["site_id"]);
            site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.Accessible = Convert.ToBoolean(reader["accessible"]);
            site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
            site.Utilities = Convert.ToBoolean(reader["utilities"]);

            return site;
        }
    }
}
