using System.Collections.Generic;
using CarAuctionManagementSystem.Models;

namespace CarAuctionManagementSystem.Services
{
    public interface IAuctionService
    {
        /// <summary>
        /// Starts a new auction for a vehicle
        /// </summary>
        /// <param name="vehicleId">ID of the vehicle to auction</param>
        /// <returns>The created auction</returns>
        /// <exception cref="CarAuctionManagementSystem.Exceptions.VehicleNotFoundException">Thrown when the vehicle doesn't exist</exception>
        /// <exception cref="CarAuctionManagementSystem.Exceptions.AuctionAlreadyActiveException">Thrown when an auction is already active for this vehicle</exception>
        Auction StartAuction(string vehicleId);

        /// <summary>
        /// Places a bid on an active auction
        /// </summary>
        /// <param name="vehicleId">ID of the vehicle being auctioned</param>
        /// <param name="bidder">Name of the bidder</param>
        /// <param name="amount">Bid amount</param>
        /// <exception cref="CarAuctionManagementSystem.Exceptions.AuctionNotFoundException">Thrown when no active auction exists for the vehicle</exception>
        /// <exception cref="CarAuctionManagementSystem.Exceptions.InvalidBidException">Thrown when the bid amount is not higher than the current highest bid</exception>
        void PlaceBid(string vehicleId, string bidder, decimal amount);

        /// <summary>
        /// Closes an active auction
        /// </summary>
        /// <param name="vehicleId">ID of the vehicle being auctioned</param>
        /// <returns>The closed auction</returns>
        /// <exception cref="CarAuctionManagementSystem.Exceptions.AuctionNotFoundException">Thrown when no active auction exists for the vehicle</exception>
        Auction CloseAuction(string vehicleId);

        /// <summary>
        /// Gets the active auction for a vehicle, if one exists
        /// </summary>
        /// <param name="vehicleId">ID of the vehicle</param>
        /// <returns>The active auction or null if none exists</returns>
        Auction? GetActiveAuction(string vehicleId);

        /// <summary>
        /// Gets all active auctions
        /// </summary>
        /// <returns>A list of all active auctions</returns>
        IEnumerable<Auction> GetAllActiveAuctions();

        /// <summary>
        /// Gets the auction history for a vehicle
        /// </summary>
        /// <param name="vehicleId">ID of the vehicle</param>
        /// <returns>A list of all auctions (active and closed) for the vehicle</returns>
        IEnumerable<Auction> GetAuctionHistory(string vehicleId);
    }
} 