# Car Auction Management System
Take Home Assignment Solution for a Software Engineer position at CONSTELLATION Automotive Group. Developed by Alexandre Filipe Salvador da Silva.

A C# implementation of a car auction management system that handles different types of vehicles, including Sedans, SUVs, Hatchbacks, and Trucks.

## Assumptions

This implementation makes the following assumptions:

- **In-Memory Storage**: All vehicle and auction data is stored in memory and will be lost when the application terminates. As requested in the Problem Statement, no database or file persistence is implemented.
- **Basic Auction Logic**: Auctions follow very simple rules, the highest bid wins.
- **Single Context**: The system operates within a single, fixed context, without support for user accounts, roles, or authentication.

## Design Decisions

1. **Abstract Base Class for Vehicles**: Using an abstract `Vehicle` class allows for common property validation logic while enabling type-specific properties and constraints in derived classes.

2. **Immutable Core Properties**: Properties like vehicle IDs, manufacturer, model, etc. are immutable to ensure data integrity throughout the auction lifecycle.

3. **Service-Based Architecture**: The system uses service interfaces to clearly define system capabilities.

4. **Facade Pattern**: The `AuctionManagementSystem` class simplifies interaction with the system by providing a unified API.

5. **Custom Exceptions**: Domain-specific exceptions improve error handling and client code clarity.

6. **Validation at Construction**: Objects validate their inputs at construction time to ensure they are always in a valid state.

## Design Overview

The system is designed using object-oriented principles, with clean separation of concerns and a focus on maintainability and extensibility.

### Domain Model

The system uses a hierarchical class structure for vehicles:
- `Vehicle` (abstract base class): Provides common properties and validation logic
  - `Sedan`: Extends Vehicle with _number of doors_
  - `Hatchback`: Extends Vehicle with _number of doors_
  - `SUV`: Extends Vehicle with _number of seats_
  - `Truck`: Extends Vehicle with _load capacity_

The `Auction` class represents an auction for a vehicle, including bid history, current state, and the ability to place bids and close the auction.

### Service Layer

The system implements a service-oriented design with these key services:
- `IVehicleInventoryService`: Manages the vehicle inventory
  - `VehicleInventoryService`: Concrete implementation
- `IAuctionService`: Manages the auction lifecycle
  - `AuctionService`: Concrete implementation

A facade class `AuctionManagementSystem` unifies the services and provides a simplified API for the code.

### Error Handling

Custom exceptions are used to communicate specific error conditions:
- `DuplicateVehicleException`: Thrown when attempting to add a vehicle with an ID that already exists
- `VehicleNotFoundException`: Thrown when attempting to work with a non-existent vehicle
- `AuctionAlreadyActiveException`: Thrown when attempting to start an auction for a vehicle that already has an active auction
- `AuctionNotFoundException`: Thrown when attempting to place a bid or close an auction that doesn't exist
- `InvalidBidException`: Thrown when a bid amount is not higher than the current highest bid

Each domain object also performs input validation and throws appropriate standard exceptions like `ArgumentNullException` and `ArgumentOutOfRangeException`.

## Running the Application

The solution includes a sample program that demonstrates all system capabilities:
1. Adding vehicles of different types
2. Searching for vehicles
3. Starting and closing auctions
4. Placing bids
5. Handling error cases

The solution includes a sample console program (`CarAuctionManagementSystem/Program.cs`) that demonstrates the system's capabilities. To run the demo application:

```bash
dotnet run --project CarAuctionManagementSystem/CarAuctionManagementSystem.csproj
```

To run the unit and integration tests:

```bash
dotnet test
```