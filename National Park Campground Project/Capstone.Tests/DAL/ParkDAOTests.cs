using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class ParkDAOTests : NPCampgroundTests
    {
        [TestMethod]
        public void GetAllParksTest_ShouldReturnAllParks()
        {
            ParkSqlDAO dao = new ParkSqlDAO(ConnectionString);

            IList<Park> parks = dao.GetAllParks();

            Assert.AreEqual(1, parks.Count);

        }
    }
}
