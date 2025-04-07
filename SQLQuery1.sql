CREATE DATABASE EventEase;
GO
USE EventEaseDB;
GO

-- Step 1: Create Venue Table
CREATE TABLE Venue (
    VenueId INT PRIMARY KEY IDENTITY(1,1),
    VenueName NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200),
    Capacity INT NOT NULL,
    ImageUrl NVARCHAR(255)
);

-- Step 2: Create Event Table
CREATE TABLE Event (
    EventId INT PRIMARY KEY IDENTITY(1,1),
    EventName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    EventDate DATETIME NOT NULL,
    VenueId INT NOT NULL,
    CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Step 3: Create Booking Table
CREATE TABLE Booking (
    BookingId INT PRIMARY KEY IDENTITY(1,1),
    EventId INT NOT NULL,
    BookingDate DATETIME NOT NULL,
    UniqueBookingId UNIQUEIDENTIFIER DEFAULT NEWID(),
    CONSTRAINT FK_Booking_Event FOREIGN KEY (EventId) REFERENCES Event(EventId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
