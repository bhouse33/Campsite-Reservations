using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        /// <summary>
        /// Gets a list of campgrounds for park chosen.
        /// </summary>
        /// <returns></returns>
        IList<Campground> GetCampgrounds(int parkId);

    }
}
