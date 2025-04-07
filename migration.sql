IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Venues] (
    [VenueId] int NOT NULL IDENTITY,
    [VenueName] nvarchar(100) NOT NULL,
    [Location] nvarchar(200) NOT NULL,
    [Capacity] int NOT NULL,
    [ImagePath] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_Venues] PRIMARY KEY ([VenueId])
);

CREATE TABLE [Events] (
    [EventId] int NOT NULL IDENTITY,
    [EventName] nvarchar(100) NOT NULL,
    [EventDate] datetime2 NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [VenueId] int NOT NULL,
    [ImagePath] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([EventId]),
    CONSTRAINT [FK_Events_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([VenueId]) ON DELETE NO ACTION
);

CREATE TABLE [Bookings] (
    [BookingId] int NOT NULL IDENTITY,
    [EventId] int NOT NULL,
    [VenueId] int NOT NULL,
    [BookingDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY ([BookingId]),
    CONSTRAINT [FK_Bookings_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([EventId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Bookings_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([VenueId]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Bookings_EventId] ON [Bookings] ([EventId]);

CREATE UNIQUE INDEX [IX_Bookings_VenueId_EventId] ON [Bookings] ([VenueId], [EventId]);

CREATE INDEX [IX_Events_VenueId] ON [Events] ([VenueId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250405160321_FixedImageUploadModel', N'9.0.3');

COMMIT;
GO

