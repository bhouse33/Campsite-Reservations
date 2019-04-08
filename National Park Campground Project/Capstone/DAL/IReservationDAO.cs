using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        /// <summary>
        /// Books a reservation if it is available, and returns a confirmation id.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        void BookReservation(Reservation reservation);
    }
}
