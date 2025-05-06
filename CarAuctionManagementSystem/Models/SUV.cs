using System;

namespace CarAuctionManagementSystem.Models
{
    public class SUV : Vehicle
    {
        public int NumberOfSeats { get; }

        public SUV(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
            : base(id, manufacturer, model, year, startingBid, VehicleType.SUV)
        {
            if (numberOfSeats < 2 || numberOfSeats > 7)
                throw new ArgumentOutOfRangeException(nameof(numberOfSeats), "Number of seats must be between 2 and 7");

            NumberOfSeats = numberOfSeats;
        }
    }
} 