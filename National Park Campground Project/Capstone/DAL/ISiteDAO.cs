using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISiteDAO
    {
        /// <summary>
        /// Returns a list of available sites for specified date range and campground.
        /// </summary>
        /// <returns></returns>
        IList<Site> GetAvailableSites(int campgroundId, DateTime fromDate, DateTime toDate);

    }
}
