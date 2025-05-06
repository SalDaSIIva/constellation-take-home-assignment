using System;
using System.Collections.Generic;
using System.Linq;
using CarAuctionManagementSystem.Exceptions;
using CarAuctionManagementSystem.Models;

namespace CarAuctionManagementSystem.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IVehicleInventoryService _vehicleInventory;
        private readonly List<Auction> _auctions = new List<Auction>();

        public AuctionService(IVehicleInventoryService vehicleInventory)
        {
            _vehicleInventory = vehicleInventory ?? throw new ArgumentNullException(nameof(vehicleInventory));
        }

        public Auction StartAuction(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be empty", nameof(vehicleId));

            var vehicle = _vehicleInventory.GetVehicle(vehicleId);

            if (GetActiveAuction(vehicleId) != null)
                throw new AuctionAlreadyActiveException(vehicleId);

            var auction = new Auction(Guid.NewGuid().ToString(), vehicle, DateTime.Now);
            _auctions.Add(auction);
            return auction;
        }

        public void PlaceBid(string vehicleId, string bidder, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be empty", nameof(vehicleId));

            if (string.IsNullOrWhiteSpace(bidder))
                throw new ArgumentException("bidder cannot be empty", nameof(bidder));

            var auction = GetActiveAuction(vehicleId);
            if (auction == null)
                throw new AuctionNotFoundException(vehicleId);

            try
            {
                auction.PlaceBid(bidder, amount);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidBidException(vehicleId, amount, auction.CurrentHighestBid);
            }
        }

        public Auction CloseAuction(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be empty", nameof(vehicleId));

            var auction = GetActiveAuction(vehicleId);
            if (auction == null)
                throw new AuctionNotFoundException(vehicleId);

            auction.CloseAuction();
            return auction;
        }

        public Auction? GetActiveAuction(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be empty", nameof(vehicleId));

            return _auctions.FirstOrDefault(a => a.Vehicle.Id == vehicleId && a.IsActive);
        }

        public IEnumerable<Auction> GetAllActiveAuctions()
        {
            return _auctions.Where(a => a.IsActive).ToList();
        }

        public IEnumerable<Auction> GetAuctionHistory(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be empty", nameof(vehicleId));

            return _auctions.Where(a => a.Vehicle.Id == vehicleId).ToList();
        }
    }
} 