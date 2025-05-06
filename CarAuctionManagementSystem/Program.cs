using System;
using System.Linq;
using CarAuctionManagementSystem.Exceptions;
using CarAuctionManagementSystem.Models;
using CarAuctionManagementSystem.Services;

namespace CarAuctionManagementSystem
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Car Auction Management System Demo\n");

            var auctionSystem = new AuctionManagementSystem();

            // Add some sample vehicles
            try
            {
                Console.WriteLine("Adding sample vehicles to inventory...");
                
                auctionSystem.AddVehicle(auctionSystem.CreateSedan("SED001", "Toyota", "Camry", 2020, 18000m, 4));
                auctionSystem.AddVehicle(auctionSystem.CreateSedan("SED002", "Honda", "Accord", 2021, 20000m, 4));
                auctionSystem.AddVehicle(auctionSystem.CreateHatchback("HAT001", "Volkswagen", "Golf", 2019, 16000m, 5));
                auctionSystem.AddVehicle(auctionSystem.CreateSUV("SUV001", "Ford", "Explorer", 2022, 35000m, 7));
                auctionSystem.AddVehicle(auctionSystem.CreateTruck("TRU001", "Chevrolet", "Silverado", 2021, 32000m, 2.5m));
                
                Console.WriteLine("Successfully added 5 vehicles to inventory.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding vehicles: {ex.Message}\n");
            }

            // Demonstrate search functionality
            Console.WriteLine("Searching for all Toyota vehicles:");
            var toyotaVehicles = auctionSystem.SearchVehicles(manufacturer: "Toyota");
            PrintVehicles(toyotaVehicles);

            Console.WriteLine("\nSearching for all 2021 vehicles:");
            var vehicles2021 = auctionSystem.SearchVehicles(year: 2021);
            PrintVehicles(vehicles2021);

            Console.WriteLine("\nSearching for all Sedans:");
            var sedans = auctionSystem.SearchVehicles(type: VehicleType.Sedan);
            PrintVehicles(sedans);

            // Start an auction
            try
            {
                Console.WriteLine("\nStarting auction for Toyota Camry (SED001)...");
                var auction = auctionSystem.StartAuction("SED001");
                Console.WriteLine($"Auction started: ID={auction.Id}, Starting Bid=${auction.Vehicle.StartingBid}\n");

                // Place some bids
                Console.WriteLine("Placing bids...");
                auctionSystem.PlaceBid("SED001", "Alex", 18500m);
                Console.WriteLine("Alex bids $18500");
                auctionSystem.PlaceBid("SED001", "Alvaro", 19000m);
                Console.WriteLine("Alvaro bids $19000");
                auctionSystem.PlaceBid("SED001", "Vlad", 19250m);
                Console.WriteLine("Vlad bids $19250");

                // Try an invalid bid
                try
                {
                    auctionSystem.PlaceBid("SED001", "Hannah", 19000m);
                }
                catch (InvalidBidException ex)
                {
                    Console.WriteLine($"Invalid bid rejected: {ex.Message}");
                }

                // Show auction status
                var activeAuction = auctionSystem.GetActiveAuction("SED001") ?? throw new AuctionNotFoundException("SED001");
                Console.WriteLine($"\nCurrent auction status for Toyota Camry:");
                Console.WriteLine($"Highest bidder: {activeAuction.CurrentHighestBidder}");
                Console.WriteLine($"Highest bid: ${activeAuction.CurrentHighestBid}");
                Console.WriteLine($"Number of bids: {activeAuction.BidHistory.Count}");

                // Close the auction
                Console.WriteLine("\nClosing auction...");
                var closedAuction = auctionSystem.CloseAuction("SED001");
                Console.WriteLine($"Auction closed. Winner: {closedAuction.CurrentHighestBidder} with bid of ${closedAuction.CurrentHighestBid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during auction: {ex.Message}");
            }

            // Demonstrate starting auctions for multiple vehicles and getting active auctions
            try
            {
                Console.WriteLine("\nStarting auctions for multiple vehicles...");
                auctionSystem.StartAuction("HAT001");
                auctionSystem.StartAuction("SUV001");
                
                Console.WriteLine("Getting all active auctions:");
                var activeAuctions = auctionSystem.GetAllActiveAuctions();
                foreach (var auction in activeAuctions)
                {
                    Console.WriteLine($"Active auction for {auction.Vehicle.Manufacturer} {auction.Vehicle.Model}, starting bid: ${auction.CurrentHighestBid}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Demonstrate error handling for a vehicle that doesn't exist
            try
            {
                Console.WriteLine("\nTrying to start auction for non-existent vehicle...");
                auctionSystem.StartAuction("CYBERTRUCK");
            }
            catch (VehicleNotFoundException ex)
            {
                Console.WriteLine($"Error handled: {ex.Message}");
            }

            // Demonstrate error handling for duplicate vehicle ID
            try
            {
                Console.WriteLine("\nTrying to add a vehicle with duplicate ID...");
                auctionSystem.AddVehicle(auctionSystem.CreateSedan("SED001", "Nissan", "Altima", 2020, 17000m, 4));
            }
            catch (DuplicateVehicleException ex)
            {
                Console.WriteLine($"Error handled: {ex.Message}");
            }

            Console.WriteLine("\nDemo completed successfully.");
        }

        private static void PrintVehicles(IEnumerable<Vehicle> vehicles)
        {
            if (!vehicles.Any())
            {
                Console.WriteLine("No vehicles found matching criteria.");
                return;
            }

            foreach (var vehicle in vehicles)
            {
                Console.WriteLine($"ID: {vehicle.Id}, Type: {vehicle.Type}, Manufacturer: {vehicle.Manufacturer}, " +
                               $"Model: {vehicle.Model}, Year: {vehicle.Year}, Starting Bid: ${vehicle.StartingBid}");
                
                switch (vehicle)
                {
                    case Sedan sedan:
                        Console.WriteLine($"  Doors: {sedan.NumberOfDoors}");
                        break;
                    case Hatchback hatchback:
                        Console.WriteLine($"  Doors: {hatchback.NumberOfDoors}");
                        break;
                    case SUV suv:
                        Console.WriteLine($"  Seats: {suv.NumberOfSeats}");
                        break;
                    case Truck truck:
                        Console.WriteLine($"  Load Capacity: {truck.LoadCapacity} tons");
                        break;
                }
            }
        }
    }
} 