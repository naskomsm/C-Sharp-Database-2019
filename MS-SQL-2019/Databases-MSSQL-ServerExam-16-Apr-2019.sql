CREATE DATABASE Airport

USE Airport

CREATE TABLE Planes
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) NOT NULL,
	Seats INT NOT NULL,
	[Range] INT NOT NULL
)

CREATE TABLE Flights
(
	Id INT PRIMARY KEY IDENTITY,
	DepartureTime DATETIME,
	ArrivalTime DATETIME,
	Origin NVARCHAR(50) NOT NULL,
	Destination NVARCHAR(50) NOT NULL,
	PlaneId INT NOT NULL FOREIGN KEY REFERENCES Planes(Id)
)

CREATE TABLE Passengers
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	Age INT NOT NULL,
	[Address] NVARCHAR(30) NOT NULL,
	PassportId CHAR(11) NOT NULL
)

--
CREATE TABLE LuggageTypes
(
	Id INT PRIMARY KEY IDENTITY,
	[Type] NVARCHAR(30) NOT NULL
)

CREATE TABLE Luggages
(
	Id INT PRIMARY KEY IDENTITY,
	LuggageTypeId INT NOT NULL FOREIGN KEY REFERENCES LuggageTypes(Id),
	PassengerId INT NOT NULL FOREIGN KEY REFERENCES Passengers(Id)
)


CREATE TABLE Tickets
(
	Id INT PRIMARY KEY IDENTITY,
	PassengerId INT NOT NULL FOREIGN KEY REFERENCES Passengers(Id),
	FlightId INT NOT NULL FOREIGN KEY REFERENCES Flights(Id),
	LuggageId INT NOT NULL FOREIGN KEY REFERENCES Luggages(Id),
	Price DECIMAL(10,2) NOT NULL
)

INSERT INTO Planes([Name],Seats,[Range]) VALUES
('Airbus 336',112,5132),
('Airbus 330',432,5325),
('Boeing 369',231,2355),
('Stelt 297',254,2143),
('Boeing 338',165,5111),
('Airbus 558',387,1342),
('Boeing 128',345,5541)

INSERT INTO LuggageTypes([Type]) VALUES
('Crossbody Bag'),
('School Backpack'),
('Shoulder Bag')

UPDATE Tickets
SET Price += Price * 0.13
FROM Tickets AS t
INNER JOIN Flights AS f
ON
t.FlightId = f.Id

ALTER TABLE Tickets NOCHECK CONSTRAINT ALL
DELETE FROM Flights 
WHERE Destination = 'Ayn Halagim'
ALTER TABLE Tickets CHECK CONSTRAINT ALL

--AFTER EXECUTION OF DATASET ( 5TH TASK !!!! )
SELECT Origin,Destination
	FROM Flights
	ORDER BY Origin,Destination DESC

SELECT *
	FROM Planes
	WHERE Name LIKE '%tr%'
	ORDER BY Id,Name,Seats,Range

SELECT f.Id AS [FlightId],SUM(t.Price) AS Price
	FROM Flights AS f
	JOIN Tickets AS t ON f.Id = t.FlightId
	GROUP BY f.Id
	ORDER BY SUM(t.Price) DESC,f.Id

SELECT TOP(10) p.FirstName,p.LastName,t.Price
	FROM Passengers AS p
	JOIN Tickets AS t ON p.Id = t.PassengerId
	ORDER BY t.Price DESC, p.FirstName,p.LastName

SELECT lt.Type, COUNT(l.Id) AS [MostUsedLuggage]
	FROM Luggages AS l
	JOIN LuggageTypes AS lt ON l.LuggageTypeId = lt.Id
	GROUP BY lt.Type
	ORDER BY COUNT(l.Id) DESC, lt.Type

SELECT CONCAT(p.FirstName,' ',p.LastName) AS [FullName],f.Origin,f.Destination
	FROM Passengers AS p
	JOIN Tickets AS t ON p.Id = t.PassengerId
	JOIN Flights AS f ON t.FlightId = f.Id
	ORDER BY [FullName],f.Origin,f.Destination

SELECT p.FirstName,p.LastName,p.Age
	FROM Passengers AS p
	FULL OUTER JOIN Tickets AS t ON p.Id = t.PassengerId
	WHERE t.Id IS NULL
	ORDER BY p.Age DESC,p.FirstName,p.LastName

SELECT p.PassportId,p.Address
	FROM Passengers AS p
	FULL OUTER JOIN Luggages AS l ON p.Id = l.PassengerId
	WHERE l.Id IS NULL
	ORDER BY p.PassportId,p.Address

SELECT p.FirstName,p.LastName,COUNT(t.FlightId) AS [Total Trips]
	FROM Passengers AS p
	FULL OUTER JOIN Tickets AS T ON p.Id = t.PassengerId
	GROUP BY p.FirstName,p.LastName
	ORDER BY [Total Trips] DESC,p.FirstName,p.LastName

SELECT CONCAT(p.FirstName, ' ',p.LastName) AS [Full Name],pl.Name AS [Plane Name],CONCAT(f.Origin,' - ',f.Destination) AS [Trip],lt.Type AS [Luggage Type]
	FROM Passengers AS p
	JOIN Tickets AS t ON p.Id = t.PassengerId
	JOIN Flights AS f ON t.FlightId = f.Id
	JOIN Planes AS pl ON f.PlaneId = pl.Id
	JOIN Luggages AS l ON t.LuggageId = l.Id
	JOIN LuggageTypes AS lt ON l.LuggageTypeId = lt.Id
	ORDER BY [Full Name],[Plane Name],Origin,Destination,[Luggage Type]

SELECT k.FirstName, k.LastName, k.Destination, k.Price
	  FROM (
		SELECT p.FirstName, p.LastName, f.Destination, t.Price,
			   DENSE_RANK() OVER(PARTITION BY p.FirstName, p.LastName ORDER BY t.Price DESC) As PriceRank
		  FROM Passengers AS p
		  JOIN Tickets AS t ON t.PassengerId = p.Id
		  JOIN Flights AS f ON f.Id = t.FlightId
	  ) AS k 
	  WHERE k.PriceRank = 1
	  ORDER BY k.Price DESC, k.FirstName, k.LastName, k.Destination


SELECT fl.Destination, COUNT(t.FlightId) AS [FilesCount]
	FROM Tickets AS t
	FULL OUTER JOIN Flights AS fl ON fl.Id = t.FlightId
	GROUP BY fl.Destination
	ORDER BY [FilesCount] DESC, fl.Destination

SELECT pl.[Name], pl.Seats, COUNT(t.Id) AS [Passangers Count]
	FROM Planes AS pl
	LEFT JOIN Flights AS fl ON pl.Id = fl.PlaneId
	LEFT JOIN Tickets AS t ON fl.Id = t.FlightId
	LEFT JOIN Passengers AS p ON p.Id = t.PassengerId
	GROUP BY pl.[Name], pl.Seats
	ORDER BY [Passangers Count] DESC, pl.[Name], pl.Seats

CREATE OR ALTER FUNCTION udf_CalculateTickets(@origin NVARCHAR(50), @destination NVARCHAR(50), @peopleCount INT)
RETURNS NVARCHAR(50)
AS
BEGIN 
	DECLARE @totalPrice DECIMAL(18,2)

	SET @totalPrice = 
	( 
		SELECT t.Price * @peopleCount
			FROM Flights AS f
			JOIN Tickets AS t ON f.Id = t.FlightId
			WHERE f.Origin = @origin AND f.Destination = @destination
	)

	IF (@totalPrice <= 0)
		RETURN 'Invalid people count!'

	IF (@totalPrice IS NULL)
		RETURN 'Invalid flight!'

	RETURN 'Total price ' + CAST(@totalPrice AS NVARCHAR(50))
END

SELECT dbo.udf_CalculateTickets('Kolysaaaahley','Rancabolang', 33)

CREATE OR ALTER PROC usp_CancelFlights
AS
BEGIN 
	UPDATE Flights
	SET ArrivalTime = NULL,DepartureTime = NULL
	WHERE ArrivalTime < DepartureTime
END

EXEC usp_CancelFlights

SELECT f.PlaneId,f.ArrivalTime,f.DepartureTime
	FROM Flights AS f
	WHERE f.ArrivalTime < f.DepartureTime
