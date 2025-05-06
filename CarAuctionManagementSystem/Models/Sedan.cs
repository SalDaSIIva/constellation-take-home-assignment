using System;

namespace CarAuctionManagementSystem.Models
{
    public class Sedan : Vehicle
    {
        public int NumberOfDoors { get; }

        public Sedan(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid, VehicleType.Sedan)
        {
            if (numberOfDoors < 2 || numberOfDoors > 5)
                throw new ArgumentOutOfRangeException(nameof(numberOfDoors), "Number of doors must be between 2 and 5");

            NumberOfDoors = numberOfDoors;
        }
    }
} 