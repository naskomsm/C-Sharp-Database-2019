CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	Title VARCHAR(15) NOT NULL,
	Notes VARCHAR(MAX)
)

CREATE TABLE Customers
(
	AccountNumber INT NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	PhoneNumber INT NOT NULL,
	EmergencyName VARCHAR(50) NOT NULL,
	EmergencyNumber INT NOT NULL,
	Notes VARCHAR(MAX)
)

CREATE TABLE RoomStatus
(
	RoomStatus BIT NOT NULL,
	Notes VARCHAR(MAX)
)

CREATE TABLE RoomTypes
(
	RoomType VARCHAR(20) NOT NULL,
	Notes VARCHAR(MAX)
)

CREATE TABLE BedTypes
(
	BedType VARCHAR(20) NOT NULL,
	Notes VARCHAR(MAX)
)

CREATE TABLE Rooms
(
	RoomNumber INT PRIMARY KEY IDENTITY,
	RoomType VARCHAR(20) NOT NULL,
	BedType VARCHAR(20) NOT NULL,
	Rate TINYINT,
	RoomStatus BIT NOT NULL,
	Notes VARCHAR(MAX)
)

CREATE TABLE Payments
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	PaymentDate DATE NOT NULL,
	AccountNumber INT NOT NULL,
	FirstDateOccupied DATE NOT NULL,
	LastDateOccupied DATE NOT NULL,
	TotalDays INT NOT NULL,
	AmountCharged INT NOT NULL,
	TaxRate TINYINT,
	TaxAmount TINYINT,
	PaymentTotal INT NOT NULL,
	Notes VARCHAR(MAX)
)

CREATE TABLE Occupancies
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	DateOccupied DATE NOT NULL,
	AccountNumber INT NOT NULL,
	RoomNumber INT NOT NULL,
	RateApplied BIT NOT NULL,
	PhoneCharge BIT NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Employees(FirstName,LastName,Title,Notes) VALUES
('Atanas','Kolev','Programmer',NULL),
('Daniel','Ivanov','Programmer',NULL),
('Ivan','Panagonov','Programmer',NULL)

INSERT INTO Customers(AccountNumber,FirstName,LastName,PhoneNumber,EmergencyName,EmergencyNumber,Notes) VALUES
(12345,'Stoqn','Shopov',0899115617,'Stoiko',123456,NULL),
(12343,'IVAKA','Shopov',0899115617,'AAAA',123456,NULL),
(12344,'KOLIO','Shopov',0899115617,'BBBBB',123456,NULL)

INSERT INTO RoomStatus(RoomStatus,Notes) VALUES
(1,NULL),
(0,NULL),
(1,NULL)

INSERT INTO RoomTypes(RoomType,Notes) VALUES
('Cool ROOM',NULL),
('Cool ROOM',NULL),
('Cool ROOM',NULL)

INSERT INTO BedTypes(BedType,Notes) VALUES
('Cool Bed',NULL),
('Cool Bed',NULL),
('Cool Bed',NULL)

INSERT INTO Rooms(RoomType,BedType,Rate,RoomStatus,Notes) VALUES
('cool ROOM','cool Bed',NULL,1,NULL),
('cool ROOM','cool Bed',NULL,1,NULL),
('cool ROOM','cool Bed',NULL,1,NULL)

INSERT INTO Payments(PaymentDate,AccountNumber,FirstDateOccupied,LastDateOccupied,TotalDays,AmountCharged,
TaxRate,TaxAmount,PaymentTotal,Notes) VALUES
('2019-11-11',12345,'2019-11-11','2019-11-11',1590,123,NULL,NULL,150,NULL),
('2019-11-11',12345,'2019-11-11','2019-11-11',1590,123,NULL,NULL,150,NULL),
('2019-11-11',12345,'2019-11-11','2019-11-11',1590,123,NULL,NULL,150,NULL)

INSERT INTO Occupancies(DateOccupied,AccountNumber,RoomNumber,RateApplied,PhoneCharge,Notes) VALUES
('2019-11-11',12345,2,1,1,NULL),
('2019-11-11',12345,2,1,1,NULL),
('2019-11-11',12345,2,1,1,NULL)