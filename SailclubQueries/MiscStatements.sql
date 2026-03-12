----------------------------------------
------- Misc. statements (Opg 3) -------
----------------------------------------
---- Lukas, Kasper, Maria, Frederik ----
----------------------------------------

-- 1. UPDATE: opdater en members email og telefon baseret på Id
UPDATE SailClubMember SET Mail = 'New@Mail.com' WHERE MemberId = 1
-- 2. DELETE: slet en booking (sørg for at du ikke utilsigtet sletter relaterede rækker)
DELETE FROM BOOKING WHERE BookingId = 1
-- 3. SELECT: list alle members (simpelt)
SELECT * FROM SailClubMember
-- 4. SELECT med JOIN: vis bookings med medlem og båd-oplysninger
SELECT bo.BookingId, bo.StartDate, bo.EndDate, bo.SailCompleted, bo.Destination, m.*, b.*
FROM Booking bo
INNER JOIN SailClubMember m
ON m.MemberId = bo.MemberId
INNER JOIN Boat b
ON b.BoatId = bo.BoatId
-- 5. SELECT med filter på enum (find kun junior-medlemmer)
SELECT * FROM SailClubMember WHERE MemberType = 0
-- 6. SELECT med søgning: søg i fornavn/efternavn/phone    Lav separate sql sætninger
SELECT * FROM SailClubMember WHERE FirstName LIKE 'P%'
SELECT * FROM SailClubMember WHERE SurName LIKE '%e_r%'
SELECT * FROM SailClubMember WHERE PhoneNumber = '12345678'
-- 7. AGGREGAT: antal bookings per båd
SELECT b.SailNumber, COUNT(*) AS BookingAmount
FROM Booking INNER JOIN Boat b ON b.BoatId = Booking.BoatId
GROUP BY b.SailNumber
-- 8. Vis aktuelle aktive bookings (beregnet i DB ud fra NOW)
SELECT * FROM BOOKING
WHERE GETDATE() BETWEEN Booking.StartDate AND Booking.EndDate