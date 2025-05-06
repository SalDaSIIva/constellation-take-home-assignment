using System;

namespace CarAuctionManagementSystem.Models
{
    public enum VehicleType
    {
        Sedan,
        SUV,
        Hatchback,
        Truck
    }

    public abstract class Vehicle
    {
        public string Id { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public int Year { get; }
        public decimal StartingBid { get; }
        public VehicleType Type { get; }

        protected Vehicle(string id, string manufacturer, string model, int year, decimal startingBid, VehicleType type)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Vehicle ID cannot be empty", nameof(id));
            
            if (string.IsNullOrWhiteSpace(manufacturer))
                throw new ArgumentException("Manufacturer cannot be empty", nameof(manufacturer));
            
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty", nameof(model));
            
            if (year < 1900 || year > DateTime.Now.Year + 1)
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 1900 and next year");
            
            if (startingBid <= 0)
                throw new ArgumentOutOfRangeException(nameof(startingBid), "Starting bid must be greater than zero");

            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
            Type = type;
        }
    }
} 