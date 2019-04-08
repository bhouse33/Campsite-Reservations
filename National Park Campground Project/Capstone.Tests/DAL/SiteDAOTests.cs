using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;
using System;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class SiteDAOTests : NPCampgroundTests
    {

        [DataTestMethod]
        [DataRow(1, 2, "2004-10-10", "2004-11-10")]
        public void GetAvailableSiteTest_ShouldReturn_CorrectCount_Of_Sites( int expectedResult, int campgroundId, string startDate, string endDate)
        {
            DateTime start = Convert.ToDateTime(startDate);
            DateTime end = Convert.ToDateTime(endDate);

            SiteSqlDAO dao = new SiteSqlDAO(ConnectionString);

            IList<Site> sites = dao.GetAvailableSites(CampgroundId, start, end);

            int actualResult = sites.Count;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
