using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class ReservationDAOTests : NPCampgroundTests
    {
        [TestMethod]
        public void BookReservation_Should_IncreaseCountBy1()
        {
            // Arrange
            Reservation reservation = new Reservation();
            reservation.SiteId = SiteId;
            reservation.Name = "Ricky Bobby";
            reservation.FromDate = Convert.ToDateTime("03/12/23");
            reservation.ToDate = Convert.ToDateTime("03/15/27");
            reservation.CreateDate = Convert.ToDateTime(DateTime.Now);

            ReservationSqlDAO dao = new ReservationSqlDAO(ConnectionString);
            int startingRowCount = GetRowCount("reservation");

            dao.BookReservation(reservation);

            int endingRowCount = GetRowCount("reservation");
            Assert.AreNotEqual(startingRowCount, endingRowCount);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void BookReservation_Should_Fail_If_SiteId_DoesNotExist()
        {
            // Arrange
            Reservation reservation = new Reservation();
            reservation.SiteId = 100;
            reservation.Name = "George Clinton and the Parliament-Funkadelic";
            reservation.FromDate = Convert.ToDateTime("03/12/23");
            reservation.ToDate = Convert.ToDateTime("03/15/27");
            reservation.CreateDate = Convert.ToDateTime(DateTime.Now);

            ReservationSqlDAO dao = new ReservationSqlDAO(ConnectionString);
            int startingRowCount = GetRowCount("reservation");

            dao.BookReservation(reservation);

            int endingRowCount = GetRowCount("reservation");
            Assert.AreNotEqual(startingRowCount, endingRowCount);
        }



    }
}
