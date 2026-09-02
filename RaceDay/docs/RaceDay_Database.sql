USE master;
GO

IF DB_ID('RaceDayDB') IS NULL
BEGIN
    CREATE DATABASE RaceDayDB;
END;
GO

USE RaceDayDB;
GO

IF OBJECT_ID('dbo.Results', 'U') IS NOT NULL DROP TABLE dbo.Results;
GO
IF OBJECT_ID('dbo.Enrolments', 'U') IS NOT NULL DROP TABLE dbo.Enrolments;
GO
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL DROP TABLE dbo.Categories;
GO
IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL DROP TABLE dbo.Events;
GO
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users
(
    UserID INT IDENTITY(1,1) NOT NULL,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Users_CreatedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Users PRIMARY KEY (UserID),
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT CK_Users_Role CHECK (Role IN ('Organiser', 'Participant'))
);
GO

CREATE TABLE dbo.Events
(
    EventID INT IDENTITY(1,1) NOT NULL,
    OrganiserID INT NOT NULL,
    EventName VARCHAR(100) NOT NULL,
    Description VARCHAR(500) NULL,
    EventDate DATE NOT NULL,
    Location VARCHAR(150) NOT NULL,
    DistanceKM DECIMAL(6,2) NOT NULL,
    Status VARCHAR(20) NOT NULL
        CONSTRAINT DF_Events_Status DEFAULT 'Upcoming',
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Events_CreatedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Events PRIMARY KEY (EventID),
    CONSTRAINT FK_Events_Organiser FOREIGN KEY (OrganiserID)
        REFERENCES dbo.Users(UserID),
    CONSTRAINT CK_Events_Distance CHECK (DistanceKM > 0),
    CONSTRAINT CK_Events_Status CHECK
        (Status IN ('Upcoming', 'Open', 'Closed', 'Completed', 'Cancelled'))
);
GO

CREATE TABLE dbo.Categories
(
    CategoryID INT IDENTITY(1,1) NOT NULL,
    EventID INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    EntryFee DECIMAL(10,2) NOT NULL,
    MaxParticipants INT NULL,

    CONSTRAINT PK_Categories PRIMARY KEY (CategoryID),
    CONSTRAINT FK_Categories_Event FOREIGN KEY (EventID)
        REFERENCES dbo.Events(EventID),
    CONSTRAINT CK_Categories_EntryFee CHECK (EntryFee >= 0),
    CONSTRAINT CK_Categories_MaxParticipants CHECK
        (MaxParticipants IS NULL OR MaxParticipants > 0),
    CONSTRAINT UQ_Categories_Event_Name UNIQUE (EventID, CategoryName)
);
GO

CREATE TABLE dbo.Enrolments
(
    EnrolmentID INT IDENTITY(1,1) NOT NULL,
    ParticipantID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL
        CONSTRAINT DF_Enrolments_Date DEFAULT SYSDATETIME(),
    Status VARCHAR(20) NOT NULL
        CONSTRAINT DF_Enrolments_Status DEFAULT 'Confirmed',

    CONSTRAINT PK_Enrolments PRIMARY KEY (EnrolmentID),
    CONSTRAINT FK_Enrolments_Participant FOREIGN KEY (ParticipantID)
        REFERENCES dbo.Users(UserID),
    CONSTRAINT FK_Enrolments_Category FOREIGN KEY (CategoryID)
        REFERENCES dbo.Categories(CategoryID),
    CONSTRAINT CK_Enrolments_Status CHECK
        (Status IN ('Pending', 'Confirmed', 'Cancelled')),
    CONSTRAINT UQ_Enrolments_Participant_Category
        UNIQUE (ParticipantID, CategoryID)
);
GO

CREATE TABLE dbo.Results
(
    ResultID INT IDENTITY(1,1) NOT NULL,
    EnrolmentID INT NOT NULL,
    FinishTime TIME NULL,
    Position INT NULL,
    ResultStatus VARCHAR(20) NOT NULL
        CONSTRAINT DF_Results_Status DEFAULT 'Finished',
    RecordedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Results_RecordedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Results PRIMARY KEY (ResultID),
    CONSTRAINT FK_Results_Enrolment FOREIGN KEY (EnrolmentID)
        REFERENCES dbo.Enrolments(EnrolmentID),
    CONSTRAINT CK_Results_Position CHECK (Position IS NULL OR Position > 0),
    CONSTRAINT CK_Results_Status CHECK
        (ResultStatus IN ('Finished', 'DNF', 'DNS'))
);
GO

INSERT INTO dbo.Users (FirstName, LastName, Email, PasswordHash, Role)
VALUES
('Thabo', 'Mokoena', 'thabo.organiser@raceday.co.za', 'HASHED_PASSWORD_001', 'Organiser'),
('Lerato', 'Molefe', 'lerato.organiser@raceday.co.za', 'HASHED_PASSWORD_002', 'Organiser'),
('Sipho', 'Dlamini', 'sipho.participant@raceday.co.za', 'HASHED_PASSWORD_003', 'Participant'),
('Naledi', 'Nkosi', 'naledi.participant@raceday.co.za', 'HASHED_PASSWORD_004', 'Participant');
GO

INSERT INTO dbo.Events
(OrganiserID, EventName, Description, EventDate, Location, DistanceKM, Status)
VALUES
(1, 'Pretoria City Run', 'Annual road running event through Pretoria.',
 '2026-10-18', 'Pretoria, Gauteng', 10.00, 'Open'),
(1, 'Soweto Community Marathon', 'Community road running event in Soweto.',
 '2026-11-08', 'Soweto, Gauteng', 42.20, 'Upcoming'),
(2, 'Cape Town Charity Cycle', 'Charity cycling event supporting local communities.',
 '2026-12-06', 'Cape Town, Western Cape', 50.00, 'Upcoming');
GO

INSERT INTO dbo.Categories (EventID, CategoryName, EntryFee, MaxParticipants)
VALUES
(1, '10KM Open', 150.00, 1000),
(1, '10KM Student', 100.00, 500),
(2, 'Marathon Open', 350.00, 1500),
(2, 'Marathon Veteran', 300.00, 500),
(3, '50KM Open Cycle', 450.00, 800),
(3, '50KM Social Cycle', 350.00, 400);
GO

INSERT INTO dbo.Enrolments (ParticipantID, CategoryID, Status)
VALUES
(3, 1, 'Confirmed'),
(3, 3, 'Confirmed'),
(4, 2, 'Confirmed'),
(4, 5, 'Confirmed');
GO

INSERT INTO dbo.Results (EnrolmentID, FinishTime, Position, ResultStatus)
VALUES
(1, '00:52:35', 15, 'Finished'),
(2, '04:12:48', 86, 'Finished');
GO

SELECT * FROM dbo.Users;
SELECT * FROM dbo.Events;
SELECT * FROM dbo.Categories;
SELECT * FROM dbo.Enrolments;
SELECT * FROM dbo.Results;
GO
