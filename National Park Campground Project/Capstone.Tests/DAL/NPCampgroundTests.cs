using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;


namespace Capstone.Tests.DAL
{
    [TestClass]
    public class NPCampgroundTests
    {
        protected string ConnectionString { get; } = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";

        protected int ParkId { get; private set; }

        protected int CampgroundId { get; private set; }

        protected int SiteId { get; private set; }

        protected int ReservationId { get; private set; }

        private TransactionScope transaction;

        [TestInitialize]
        public void Setup()
        {
            transaction = new TransactionScope();

            string sql = File.ReadAllText("test-script.sql");
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ParkId = Convert.ToInt32(reader["newParkId"]);
                    CampgroundId = Convert.ToInt32(reader["newCampgroundId"]);
                    SiteId = Convert.ToInt32(reader["newSiteId"]);
                    ReservationId = Convert.ToInt32(reader["newReservationId"]);
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }



        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
        }
    }
}
