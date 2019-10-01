USE TestDB
--- One to One Relationship
CREATE TABLE Persons 
(
	PersonID INT NOT NULL,
	FirstName NVARCHAR(30),
	Salary FLOAT,
	PassportID INT NOT NULL
)

CREATE TABLE Passports
(
	PassportID INT NOT NULL,
	PassportNumber NVARCHAR(30)
)

ALTER TABLE Persons 
ADD CONSTRAINT PK_Person 
PRIMARY KEY (PersonID)

ALTER TABLE Passports 
ADD CONSTRAINT PK_Passport
PRIMARY KEY (PassportID)

ALTER TABLE Persons 
ADD FOREIGN KEY (PassportID) 
REFERENCES Passports(PassportID)

INSERT INTO Passports (PassportID, PassportNumber) VALUES 
(101,'N34FG21B'),
(102,'K65LO4R7'),
(103,'ZE657QP2')


INSERT INTO Persons (PersonID, FirstName, Salary, PassportID) VALUES
(1,'Roberto',43300.00,102),
(2,'Tom',56100.00,103),
(3,'Yana',60200.00,101)

--- One to Many Relationship

CREATE TABLE Models 
(
	ModelID INT NOT NULL,
	[Name] NVARCHAR(30),
	ManufacturerID INT NOT NULL
)

CREATE TABLE Manufacturers
(
	ManufacturerID INT NOT NULL,
	[Name] NVARCHAR(30),
	EstablishedOn DATE 
)

ALTER TABLE Models
ADD CONSTRAINT PK_Model
PRIMARY KEY (ModelID)

ALTER TABLE Manufacturers
ADD CONSTRAINT PK_Manufacturer
PRIMARY KEY (ManufacturerID)

ALTER TABLE Models 
ADD FOREIGN KEY (ManufacturerID) 
REFERENCES Manufacturers(ManufacturerID)

INSERT INTO Manufacturers (ManufacturerID,[Name],EstablishedOn) VALUES
(1,'BMW','07-03-1916'),
(2,'Tesla','01-01-2003'),
(3,'Lada','01-05-1966')

INSERT INTO Models (ModelID,[Name],ManufacturerID) VALUES
(101,'X1',1),
(102,'i6',1),
(103,'Model S',2),
(104,'Model X',2),
(105,'Model 3',2),
(106,'Nova',3)

--- Many to Many Relationship

CREATE TABLE Students
(
	StudentID INT NOT NULL,
	[Name] NVARCHAR(30)
)

CREATE TABLE Exams
(
	ExamID INT NOT NULL,
	[Name] NVARCHAR(30)
)

CREATE TABLE StudentsExams
(
	StudentID INT NOT NULL,
	ExamID INT NOT NULL
)

ALTER TABLE Students
ADD CONSTRAINT PK_Student
PRIMARY KEY (StudentID)

ALTER TABLE Exams
ADD CONSTRAINT PK_Exams
PRIMARY KEY (ExamID)

ALTER TABLE StudentsExams
ADD CONSTRAINT PK_StudentsExam
PRIMARY KEY (StudentID,ExamID)

ALTER TABLE StudentsExams 
ADD FOREIGN KEY (StudentID) 
REFERENCES Students(StudentID)

ALTER TABLE StudentsExams 
ADD FOREIGN KEY (ExamID) 
REFERENCES Exams(ExamID)

INSERT INTO Students (StudentID,[Name]) VALUES
(1,'Mila'),
(2,'Toni'),
(3,'Ron')

INSERT INTO Exams (ExamID,[Name]) VALUES
(101,'SpringMVC'),
(102,'Neo4j'),
(103,'Oracle 11g')

INSERT INTO StudentsExams (StudentID,ExamID) VALUES
(1,101),
(1,102),
(2,101),
(3,103),
(2,102),
(2,103)

--- Self-Referencing 
CREATE TABLE Teachers
(
	TeacherID INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(30),
	ManagerID INT FOREIGN KEY REFERENCES Teachers(TeacherID) 
)

--- Online Store Database
CREATE TABLE ItemTypes
(
	ItemTypeID INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50)
)

CREATE TABLE Items
(
	ItemID INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50),
	ItemTypeID INT NOT NULL FOREIGN KEY REFERENCES ItemTypes(ItemTypeID) 
)

CREATE TABLE Cities
(
	CityID INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50)
)

CREATE TABLE Customers
(
	CustomerID INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50),
	Birthday DATE,
	CityID INT NOT NULL FOREIGN KEY REFERENCES Cities(CityID) 
)

CREATE TABLE Orders
(
	OrderID INT NOT NULL PRIMARY KEY,
	CustomerID INT NOT NULL FOREIGN KEY REFERENCES Customers(CustomerID) 
)

CREATE TABLE OrderItems
(
	OrderID INT NOT NULL FOREIGN KEY REFERENCES Orders(OrderID),
	ItemID INT NOT NULL FOREIGN KEY REFERENCES Items(ItemID) 
)

ALTER TABLE OrderItems
ADD CONSTRAINT PK_OrderItem
PRIMARY KEY (OrderID,ItemID)

--- University Database
CREATE TABLE Majors
(
	MajorID INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50)
)

CREATE TABLE Subjects
(
	SubjectID INT NOT NULL PRIMARY KEY,
	SubjectName NVARCHAR(50)
)

CREATE TABLE Students
(
	StudentID INT NOT NULL PRIMARY KEY,
	StudentNumber NVARCHAR(50),
	StudentName NVARCHAR(50),
	MajorID INT NOT NULL FOREIGN KEY REFERENCES Majors(MajorID)
)

CREATE TABLE Agenda
(
	StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID),
	SubjectID INT NOT NULL FOREIGN KEY REFERENCES Subjects(SubjectID)
)

ALTER TABLE Agenda
ADD CONSTRAINT PK_StudentSubject
PRIMARY KEY (StudentID,SubjectID)

CREATE TABLE Payments
(
	PaymentID INT NOT NULL PRIMARY KEY,
	PaymentDate DATE,
	PaymentAmount INT,
	StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID)
)

--- Peaks in Rila

USE Geography
SELECT m.MountainRange, p.PeakName, p.Elevation
	FROM Mountains AS m
	JOIN Peaks AS p ON m.Id = p.MountainId
	WHERE m.MountainRange = 'Rila'
	ORDER BY p.Elevation DESC

