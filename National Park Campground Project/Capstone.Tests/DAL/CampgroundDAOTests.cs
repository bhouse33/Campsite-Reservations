using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;



namespace Capstone.Tests.DAL
{
    [TestClass]
    public class CampgroundDAOTests : NPCampgroundTests
    {
        [TestMethod]
        public void GetAllCampgroundsTest_ShouldReturnAllCampgrounds()
        {
            CampgroundSqlDAO dao = new CampgroundSqlDAO(ConnectionString);

            IList<Campground> campgrounds = dao.GetCampgrounds(ParkId);

            Assert.AreEqual(1, campgrounds.Count);
        }

        [TestMethod]
        public void GetAllCampgroundsTest_ShouldReturnZero_When_ParkId_IsNot_Valid()
        {
            CampgroundSqlDAO dao = new CampgroundSqlDAO(ConnectionString);

            IList<Campground> campgrounds = dao.GetCampgrounds(9999999999);

            Assert.AreEqual(0, campgrounds.Count);
        }



    }
}
