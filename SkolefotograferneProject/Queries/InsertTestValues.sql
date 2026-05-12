-- School
INSERT INTO School (SchoolName, SchoolAddress, PostalCode) VALUES ('Roskilde Skole', 'Skolevej 1', '4000');
INSERT INTO School (SchoolName, SchoolAddress, PostalCode) VALUES ('Østerbro Skole', 'Østerbrogade 10', '2100');

-- Secretary
INSERT INTO Secretary (SecretaryName, PhoneNumber, Mail, SecretaryPassword, SchoolID) VALUES ('Hanne Nielsen', '12345678', 'hanne@roskildeskole.dk', 'pass123', 1);
INSERT INTO Secretary (SecretaryName, PhoneNumber, Mail, SecretaryPassword, SchoolID) VALUES ('Lone Pedersen', '87654321', 'lone@oesterbro.dk', 'pass456', 2);

-- Teacher
INSERT INTO Teacher (TeacherName, Mail, TeacherPassword, PhoneNumber) VALUES ('Mads Hansen', 'mads@laerer.dk', 'laerer123', '11223344');
INSERT INTO Teacher (TeacherName, Mail, TeacherPassword, PhoneNumber) VALUES ('Sofie Larsen', 'sofie@laerer.dk', 'laerer456', '44332211');

-- SchoolClass
INSERT INTO SchoolClass (SchoolClassName, SchoolClassYear, SchoolID, TeacherID) VALUES ('3A', 2024, 1, 1);
INSERT INTO SchoolClass (SchoolClassName, SchoolClassYear, SchoolID, TeacherID) VALUES ('5B', 2024, 2, 2);

-- Parent
INSERT INTO Parent (ParentName, Mail, PhoneNumber, ParentAddress, ParentPassword, PostalCode) VALUES ('Lars Jensen', 'lars@gmail.com', '55667788', 'Ahornvej 5', 'forælder123', '4000');
INSERT INTO Parent (ParentName, Mail, PhoneNumber, ParentAddress, ParentPassword, PostalCode) VALUES ('Pia Christensen', 'pia@gmail.com', '88776655', 'Birkevej 12', 'forælder456', '2100');
-- Student
INSERT INTO Student (StudentName, PhotoCode, SchoolClassID,ParentID) VALUES ('Emil Jensen', 'FOTO001', 1,1);
INSERT INTO Student (StudentName, PhotoCode, SchoolClassID,ParentID) VALUES ('Maja Christensen', 'FOTO002', 2,2);


---- ParentStudent
--INSERT INTO ParentStudent (StudentID, ParentID) VALUES (1, 1);
--INSERT INTO ParentStudent (StudentID, ParentID) VALUES (2, 2);

-- Photographer
INSERT INTO Photographer (PhotographerName, Mail, PhoneNumber, CVR, PhotographerPassword) VALUES ('Jonas Foto', 'jonas@foto.dk', '99887766', '12345678', 'foto123');
INSERT INTO Photographer (PhotographerName, Mail, PhoneNumber, CVR, PhotographerPassword) VALUES ('Anna Billeder', 'anna@billeder.dk', '66778899', '87654321', 'foto456');

-- Photo
INSERT INTO Photo (FilePath, PhotoDate, PhotographerID, StudentID) VALUES ('/billeder/emil_2024.jpg', '2024-09-10', 1, 1);
INSERT INTO Photo (FilePath, PhotoDate, PhotographerID, StudentID) VALUES ('/billeder/maja_2024.jpg', '2024-09-11', 2, 2);

-- PhotoEvent
INSERT INTO PhotoEvent (StartDate, EndDate, PhotoEventLocation, PhotographerID, SchoolClassID) VALUES ('2024-09-10', '2024-09-10', 'Roskilde Skole, Gymsal', 1, 1);
INSERT INTO PhotoEvent (StartDate, EndDate, PhotoEventLocation, PhotographerID, SchoolClassID) VALUES ('2024-09-11', '2024-09-11', 'Østerbro Skole, Aula', 2, 2);

-- PhotoOrder
INSERT INTO PhotoOrder (PhotoOrderDate, TotalPrice, ParentID) VALUES ('2024-10-01', 249.95, 1);
INSERT INTO PhotoOrder (PhotoOrderDate, TotalPrice, ParentID) VALUES ('2024-10-03', 349.50, 2);

-- OrderLine
INSERT INTO OrderLine (PhotoType, PhotoColorFormat, Price, PhotoDimensions, Quantity, PhotoOrderID, PhotoID) VALUES (1, 1, 124.95, 2, 1, 1, 1);
INSERT INTO OrderLine (PhotoType, PhotoColorFormat, Price, PhotoDimensions, Quantity, PhotoOrderID, PhotoID) VALUES (2, 2, 174.75, 3, 2, 2, 2);