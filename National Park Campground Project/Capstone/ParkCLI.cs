using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
    public class ParkCLI
    {

        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;

        public ParkCLI(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        public void RunCLI()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                Console.Clear();
                PrintHeader();
                keepGoing = PrintMainMenu();
            }
        }

        private void PrintHeader()
        {
            Console.WriteLine("Welcome to the National Park Campsite Reservation Service");
        }

        private bool PrintMainMenu()
        {
            Console.WriteLine("Select a Park for Further Details");
            Console.WriteLine();

            IList<Park> allParks = parkDAO.GetAllParks();

            foreach (Park park in allParks)
            {
                Console.WriteLine($"{park.ParkId}) {park.Name}");
            }

            Console.WriteLine("0 - Quit");
            Console.WriteLine();


            int parkSelection = CLIHelper.GetInteger("Select park number to display more information");
            if (parkSelection == 0)
            {
                return false;
            }
            foreach (Park park in allParks)
            {
                if (park.ParkId == parkSelection)
                {
                    PrintParkInfoMenu(park);
                    return true;
                }
            }

            Console.WriteLine("you gave an incorrect park id");
            Console.ReadLine();
            return true;

        }

        private bool PrintParkInfoMenu(Park selectedPark)
        {
            Console.Clear();
            Console.WriteLine($"{selectedPark.Name} National Park");
            Console.WriteLine();
            Console.WriteLine($"Location:         {selectedPark.Location}       ");
            Console.WriteLine($"Established:      {selectedPark.EstablishDate.ToString("MM/dd/yyyy")}       ");
            Console.WriteLine($"Area:             {selectedPark.Area.ToString("N0")} sq km       ");
            Console.WriteLine($"Annual Visitors:  {selectedPark.Visitors.ToString("N0")}       ");
            Console.WriteLine();
            Console.WriteLine(selectedPark.Description);
            Console.WriteLine();

            Console.WriteLine("Select a Command");
            Console.WriteLine("   1) View Campgrounds ");
            Console.WriteLine("   2) Search for Reservation");
            Console.WriteLine("   3) Return to Previous Screen");
            Console.WriteLine();

            int input = CLIHelper.GetInteger("Make a selection: ");

            switch (input)
            {
                case 1:
                    PrintCampgroundMenu(selectedPark);
                    return true;

                case 2:
                    PrintReservationSearchPrompt(selectedPark);
                    return true;

                case 3:
                    return false;

                default:
                    Console.WriteLine("invalid input");
                    Console.ReadLine();
                    return true;
            }
        }

        private bool PrintCampgroundMenu(Park selectedPark)
        {
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(selectedPark.ParkId);

            Console.Clear();
            Console.WriteLine("Park Campgrounds");
            Console.WriteLine($"{selectedPark.Name} National Park Campgrounds");
            Console.WriteLine();

            Console.WriteLine($"        Name                             Open           Close         Daily Fee");

            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine($"#{campground.CampgroundId, -5}{campground.Name, -35}{campground.FromMonth, -15}{campground.ToMonth, -15}{$"{campground.DailyFee:C2}", -10}");
            }

            Console.WriteLine();
            Console.WriteLine("Select a Command");
            Console.WriteLine("1) Search for Available Reservation");
            Console.WriteLine("2) Return to Previous Screen");
            Console.WriteLine();

            int input = CLIHelper.GetInteger("Make a selection: ");

            switch (input)
            {
                case 1:
                    PrintReservationSearchPrompt(selectedPark);
                    return true;

                case 2:
                    return false;

                default:
                    Console.WriteLine("invalid input");
                    Console.ReadLine();
                    PrintCampgroundMenu(selectedPark);
                    return true;
            }


        }

        private void PrintReservationSearchPrompt(Park selectedPark)
        {
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(selectedPark.ParkId);
            Console.Clear();
            Console.WriteLine("Search for Campground Reservation");
            Console.WriteLine();

            Console.WriteLine($"        Name                             Open           Close         Daily Fee");

            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine($"#{campground.CampgroundId,-5}{campground.Name,-35}{campground.FromMonth,-15}{campground.ToMonth,-15}{$"{campground.DailyFee:C2}",-10}");
            }
            Console.WriteLine();
            int campgroundId = CLIHelper.GetInteger(" Which Campground (enter 0 to cancel)?:");
            if (campgroundId ==0)
            {
                return;
            }

            bool isValidCampground = false;

            foreach (Campground campground in campgrounds)
            {
                if(campgroundId==campground.CampgroundId)
                {
                    isValidCampground = true;
                }
            }

            if(!isValidCampground)
            {
                Console.WriteLine("Please enter a valid campground id..");
                Console.ReadLine();
                PrintReservationSearchPrompt(selectedPark);
            }

            DateTime arrival = CLIHelper.GetDateTime("What is the arrival date? (as mm/dd/yyyy):");
            DateTime departure = CLIHelper.GetDateTime("What is the departure date? (as mm/dd/yyyy):");
            Console.WriteLine();

            if (departure < arrival)
            {
                Console.WriteLine("The departure date must be after the arrival date");
                Console.ReadLine();
                PrintReservationSearchPrompt(selectedPark);
                return;
            }

            TimeSpan dateRange = departure.Date - arrival.Date;

            int daysOfStay = dateRange.Days;
            decimal dailyFee = 0;

            IList<Site> sites = siteDAO.GetAvailableSites(campgroundId, arrival, departure);

            foreach (Campground campground in campgrounds)
            {
                if (campground.CampgroundId == campgroundId)
                {
                    dailyFee = campground.DailyFee;
                }
            }

            decimal costOfTotalStay = dailyFee * daysOfStay;

            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine($"{"Site No.",-10}{"Max Occup.", -12}{"Accessible?",-12}{"Max RV Length",-14}{"Utility",-10}{"Cost",-8}");

            foreach (Site site in sites)
            {
                string rvLength = "N/A";
                string accessible = "No";
                string utility = "N/A";

                if (site.Utilities==true)
                {
                    utility = "Yes";
                }

                if (site.Accessible == true)
                {
                    accessible = "Yes";
                }

                if (site.MaxRVLength!=0)
                {
                    rvLength = site.MaxRVLength.ToString();
                }                
                Console.WriteLine($"{site.SiteId,-10}{site.MaxOccupancy,-12}{accessible,-12}{rvLength,-14}{utility,-10}{$"{costOfTotalStay:C2}",-8}");
            }
            Console.WriteLine();
            int siteId = CLIHelper.GetInteger("Which site should be reserved (enter 0 to cancel)?");
            if (siteId == 0)
            {
                return;
            }

            bool isValidSite = false;

            foreach (Site site in sites)
            {
                if (siteId == site.SiteId)
                {
                    isValidSite = true;
                }
            }

            if (!isValidSite)
            {
                Console.WriteLine("Please enter a valid site id..");
                Console.ReadLine();
                PrintReservationSearchPrompt(selectedPark);
            }

            string name = CLIHelper.GetString("What name should the reservation be made under?");
            Console.WriteLine();

            Reservation reservation = new Reservation
            {
                SiteId = siteId,
                Name = name,
                FromDate = arrival,
                ToDate = departure,
                CreateDate = DateTime.Now,
            };
            reservationDAO.BookReservation(reservation);
            Console.WriteLine("Thanks for booking your reservation, press enter to return to main menu");
            Console.ReadLine();
        }
    }
}
