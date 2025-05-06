using System;

namespace CarAuctionManagementSystem.Exceptions
{
    public class DuplicateVehicleException : Exception
    {
        public string VehicleId { get; }

        public DuplicateVehicleException(string vehicleId)
            : base($"Vehicle with ID '{vehicleId}' already exists in the inventory")
        {
            VehicleId = vehicleId;
        }
    }

    public class VehicleNotFoundException : Exception
    {
        public string VehicleId { get; }

        public VehicleNotFoundException(string vehicleId)
            : base($"Vehicle with ID '{vehicleId}' not found in the inventory")
        {
            VehicleId = vehicleId;
        }
    }

    public class AuctionAlreadyActiveException : Exception
    {
        public string VehicleId { get; }

        public AuctionAlreadyActiveException(string vehicleId)
            : base($"An auction is already active for vehicle with ID '{vehicleId}'")
        {
            VehicleId = vehicleId;
        }
    }

    public class AuctionNotFoundException : Exception
    {
        public string VehicleId { get; }

        public AuctionNotFoundException(string vehicleId)
            : base($"No active auction found for vehicle with ID '{vehicleId}'")
        {
            VehicleId = vehicleId;
        }
    }

    public class InvalidBidException : Exception
    {
        public string VehicleId { get; }
        public decimal BidAmount { get; }
        public decimal CurrentHighestBid { get; }

        public InvalidBidException(string vehicleId, decimal bidAmount, decimal currentHighestBid)
            : base($"Bid amount ${bidAmount} is not higher than current highest bid ${currentHighestBid} for vehicle '{vehicleId}'")
        {
            VehicleId = vehicleId;
            BidAmount = bidAmount;
            CurrentHighestBid = currentHighestBid;
        }
    }
} 