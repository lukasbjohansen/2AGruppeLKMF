----------------------------------------
-- SQL Create SailClub tables (Opg 1) --
----------------------------------------
---- Lukas, Kasper, Maria, Frederik ----
----------------------------------------

-- Reset
DROP TABLE IF EXISTS Booking;
DROP TABLE IF EXISTS SailClubMember;
DROP TABLE IF EXISTS Boat;
-- Create
CREATE TABLE SailClubMember(
    MemberId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(30) NOT NULL,
    SurName NVARCHAR(20) NOT NULL,
    PhoneNumber VARCHAR(11) NOT NULL UNIQUE,
    MemberAddress NVARCHAR(50),
    City NVARCHAR(30),
    Mail NVARCHAR(100) NOT NULL UNIQUE,
    MemberType int DEFAULT 0 CHECK (MemberType >= 0),
    MemberRole int DEFAULT 0 CHECK (MemberRole >= 0), 
    MemberImage NVARCHAR(MAX)
);
CREATE TABLE Boat(
    BoatId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Model NVARCHAR(30),
    SailNumber NVARCHAR(10) NOT NULL UNIQUE,
    EngineInfo NVARCHAR(20),
    Draft FLOAT,
    Width FLOAT,
    BoatLength FLOAT,
    YearOfConstruction VARCHAR(4),
    BoatType int DEFAULT 0 CHECK (BoatType >= 0)  
);
CREATE TABLE Booking(
    BookingId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    SailCompleted BIT DEFAULT 0,
    Destination NVARCHAR(30),
    MemberId int NOT NULL,
    BoatId int NOT NULL,
    FOREIGN KEY (MemberId) REFERENCES SailClubMember (MemberId) ON DELETE CASCADE,
    FOREIGN KEY (BoatId) REFERENCES Boat (BoatId) ON DELETE CASCADE,
    CONSTRAINT CHK_Dates CHECK (EndDate >= StartDate)
);