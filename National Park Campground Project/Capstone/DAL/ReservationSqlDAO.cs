using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {

        private string connectionString;

        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public void BookReservation(Reservation reservation)
        {
            try
            {
                // Create a connection to the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation VALUES (@newSiteId, @name, @fromDate, @ToDate, @createDate);", conn);
                    cmd.Parameters.AddWithValue("newSiteid", reservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", reservation.Name);
                    cmd.Parameters.AddWithValue("@fromDate", reservation.FromDate);
                    cmd.Parameters.AddWithValue("@toDate", reservation.ToDate);
                    cmd.Parameters.AddWithValue("@createDate", reservation.CreateDate);


                    cmd.ExecuteNonQuery();

                    // Now print the new city id.
                    cmd = new SqlCommand("SELECT MAX(reservation_id) FROM reservation;", conn);
                    int confirmationId = Convert.ToInt32(cmd.ExecuteScalar());

                    Console.WriteLine($"The reservation has been made and the confirmation id is {confirmationId}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred communicating with the database. ");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private Reservation ConvertReaderToReservation(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationId = Convert.ToInt32(reader["reservvation_id"]);
            reservation.SiteId = Convert.ToInt32(reader["site_id"]);
            reservation.Name = Convert.ToString(reader["name"]);
            reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
            reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
            reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);

            return reservation;
        }
    }
}
