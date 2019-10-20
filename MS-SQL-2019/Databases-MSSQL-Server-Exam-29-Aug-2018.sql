CREATE DATABASE Supermarket

USE Supermarket

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) NOT NULL
)

CREATE TABLE Items
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) NOT NULL,
	Price DECIMAL(18,2) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Phone CHAR(12) NOT NULL,
	Salary DECIMAL(18,2) NOT NUll
)

CREATE TABLE Orders
(
	Id INT PRIMARY KEY IDENTITY,
	[DateTime] DATETIME2 NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL
)

CREATE TABLE OrderItems
(
	OrderId INT FOREIGN KEY REFERENCES Orders(Id) NOT NULL,
	ItemId INT FOREIGN KEY REFERENCES Items(Id) NOT NULL,
	Quantity INT CHECK(Quantity > 0) NOT NULL,

	PRIMARY KEY (OrderId,ItemId)
)

CREATE TABLE Shifts
(
	Id INT IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CheckIn DATETIME2 NOT NULL,
	CheckOut DATETIME2 NOT NULL,

	PRIMARY KEY (Id,EmployeeId)
)

INSERT INTO Employees (FirstName,LastName,Phone,Salary) VALUES
('Stoyan','Petrov','888-785-8573',500.25),
('Stamat','Nikolov','789-613-1122',999995.25),
('Evgeni','Petkov','645-369-9517',1234.51),
('Krasimir','Vidolov','321-471-9982',50.25)

INSERT INTO Items ([Name],Price,CategoryId) VALUES
('Tesla battery',154.25,8),
('Chess',30.25,8),
('Juice',5.32,1),
('Glasses',10,8),
('Bottle of water',1,1)