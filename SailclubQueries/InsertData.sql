----------------------------------------
--- SQL Insert SailClub data (Opg 2) ---
----------------------------------------
---- Lukas, Kasper, Maria, Frederik ----
----------------------------------------

-- Reset
DELETE FROM Booking;
DELETE FROM SailClubMember;
DELETE FROM Boat;
DBCC CHECKIDENT ('SailClubMember', RESEED, 0);
DBCC CHECKIDENT ('Boat', RESEED, 0);
DBCC CHECKIDENT ('Booking', RESEED, 0);
-- Insert
INSERT INTO SailClubMember (FirstName, SurName, PhoneNumber, MemberAddress, City, Mail, MemberType, MemberRole, MemberImage)
VALUES ('Poul', 'Henriksen', '12345678', 'Gaden 129', 'Roskilde', 'Poul@gmail.com', 0, 0, NULL);
INSERT INTO Boat (Model, SailNumber, EngineInfo, Draft, Width, BoatLength, YearOfConstruction, BoatType)
VALUES ('Model', '16-335', 'Is very good', 2.2, 3.3, 2.2, '2026', 2);
INSERT INTO Booking (StartDate, EndDate, SailCompleted, Destination, MemberId, BoatId)
VALUES ('2026-03-12', '2026-04-02', 0, 'Karrebæksminde', 1, 1);