using System;
using System.Collections.Generic;

namespace CarAuctionManagementSystem.Models
{
    public class Auction
    {
        public string Id { get; }
        public Vehicle Vehicle { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; private set; }
        public decimal CurrentHighestBid { get; private set; }
        public string? CurrentHighestBidder { get; private set; }
        public bool IsActive => EndTime == null;
        public List<Bid> BidHistory { get; } = new List<Bid>();

        public Auction(string id, Vehicle vehicle, DateTime startTime)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Auction ID cannot be empty", nameof(id));

            Id = id;
            Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));
            StartTime = startTime;
            CurrentHighestBid = vehicle.StartingBid;
            CurrentHighestBidder = null;
        }

        public void PlaceBid(string bidder, decimal amount)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot place bid on an inactive auction");

            if (string.IsNullOrWhiteSpace(bidder))
                throw new ArgumentException("Bidder name cannot be empty", nameof(bidder));

            if (amount <= CurrentHighestBid)
                throw new ArgumentOutOfRangeException(nameof(amount), $"Bid amount must be greater than current highest bid of {CurrentHighestBid}");

            CurrentHighestBid = amount;
            CurrentHighestBidder = bidder;

            BidHistory.Add(new Bid(bidder, amount, DateTime.Now));
        }

        public void CloseAuction()
        {
            if (!IsActive)
                throw new InvalidOperationException("Auction is already closed");

            EndTime = DateTime.Now;
        }
    }

    public class Bid
    {
        public string Bidder { get; }
        public decimal Amount { get; }
        public DateTime Timestamp { get; }

        public Bid(string bidder, decimal amount, DateTime timestamp)
        {
            Bidder = bidder;
            Amount = amount;
            Timestamp = timestamp;
        }
    }
} 