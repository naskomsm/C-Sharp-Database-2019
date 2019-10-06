CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName VARCHAR(50) NOT NULL,
	DailyRate TINYINT,
	WeeklyRate TINYINT,
	MonthlyRate TINYINT,
	WeekendRate TINYINT
)

CREATE TABLE Cars
(
	Id INT PRIMARY KEY IDENTITY,
	PlateNumber INT NOT NULL,
	ManuFacturer VARCHAR(20) NOT NULL,
	Model VARCHAR(10) NOT NULL,
	CarYear DATE NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Doors TINYINT NOT NULL,
	Picture VARBINARY(900),
	Condition VARCHAR(MAX),
	Available BIT NOT NULL
)

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
	Id INT PRIMARY KEY IDENTITY,
	DriverLicenceNumber INT NOT NULL,
	FullName VARCHAR(20) NOT NULL,
	[Address] VARCHAR(20),
	City VARCHAR(20),
	ZIPCode INT,
	Notes VARCHAR(MAX)
)

CREATE TABLE RentalOrders
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
	CarId INT FOREIGN KEY REFERENCES Cars(Id),
	TankLevel TINYINT NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage INT NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	TotalDays INT NOT NULL,
	RateApplied TINYINT,
	TaxRate TINYINT,
	OrderStatus BIT NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Categories(CategoryName,DailyRate,WeeklyRate,MonthlyRate,WeekendRate) VALUES
('Category1',NULL,NULL,NULL,NULL),
('Category2',NULL,NULL,NULL,NULL),
('Category3',NULL,NULL,NULL,NULL)

INSERT INTO Cars(PlateNumber,ManuFacturer,Model,CarYear,Doors,Picture,Condition,Available) VALUES
(1531,'Germany','M5','2019-11-11',4,800,NULL,1),
(1532,'Germany','M3','2019-11-11',3,500,NULL,1),
(1533,'Germany','M4','2019-11-11',3,810,NULL,0)

INSERT INTO Employees(FirstName,LastName,Title,Notes) VALUES
('Atanas','Kolev','Programmer',NULL),
('Daniel','Ivanov','Programmer',NULL),
('Ivan','Panagonov','Designer',NULL)

INSERT INTO Customers(DriverLicenceNumber,FullName,[Address],City,ZIPCode,Notes) VALUES
(123456,'Stoqn',NULL,NULL,NULL,NULL),
(654321,'Kolio',NULL,NULL,NULL,NULL),
(123654,'Naiden',NULL,NULL,NULL,NULL)

INSERT INTO RentalOrders(TankLevel,KilometrageStart,KilometrageEnd,TotalKilometrage,StartDate,EndDate,TotalDays,
						RateApplied,TaxRate,OrderStatus,Notes) VALUES
(7,25000,110000,200000,'2002-11-11','2005-11-11',150,NULL,NULL,1,NULL),
(1,22000,170000,270000,'2004-11-11','2008-11-11',110,NULL,NULL,1,NULL),
(2,24000,130000,600000,'2001-11-11','2009-11-11',120,NULL,NULL,0,NULL)