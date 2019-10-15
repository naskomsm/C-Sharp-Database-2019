CREATE DATABASE ColonialJourney 

USE ColonialJourney

CREATE TABLE Planets
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL
)	

CREATE TABLE Spaceports
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	PlanetId INT FOREIGN KEY REFERENCES Planets(Id) NOT NULL
)

CREATE TABLE Spaceships
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	Manufacturer VARCHAR(30) NOT NULL,
	LightSpeedRate INT DEFAULT 0
)

CREATE TABLE Colonists
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	Ucn VARCHAR(10) NOT NULL UNIQUE,
	BirthDate DATE NOT NULL
)

CREATE TABLE Journeys
(
	Id INT PRIMARY KEY IDENTITY,
	JourneyStart DATETIME2 NOT NULL,
	JourneyEnd DATETIME2 NOT NULL,
	Purpose VARCHAR(11) CHECK(Purpose IN ('Medical','Technical','Educational','Military')),
	DestinationSpaceportId INT FOREIGN KEY REFERENCES Spaceports(Id) NOT NULL,
	SpaceshipId INT FOREIGN KEY REFERENCES Spaceships(Id) NOT NULL
)

CREATE TABLE TravelCards
(
	Id INT PRIMARY KEY IDENTITY,
	CardNumber CHAR(10) NOT NULL UNIQUE,
	JobDuringJourney VARCHAR(8) CHECK(JobDuringJourney IN ('Pilot','Engineer','Trooper','Cleaner','Cook')),
	ColonistId INT FOREIGN KEY REFERENCES Colonists(Id) NOT NULL,
	JourneyId INT FOREIGN KEY REFERENCES Journeys(Id) NOT NULL
)

INSERT INTO Planets ([Name]) VALUES
('Mars'),
('Earth'),
('Jupiter'),
('Saturn')

INSERT INTO Spaceships ([Name],Manufacturer,LightSpeedRate) VALUES
('Golf','VW',3),
('WakaWaka','Wakanda',4),
('Falcon9','SpaceX',1),
('Bed','Vidolov',6)

UPDATE Spaceships
SET LightSpeedRate += 1
WHERE Id BETWEEN 8 AND 12

DELETE
FROM TravelCards
WHERE JourneyId IN (1,2,3)

DELETE
FROM Journeys
WHERE Id IN (1,2,3)

SELECT CardNumber,JobDuringJourney 
	FROM TravelCards
	ORDER BY CardNumber

SELECT Id,CONCAT(FirstName,' ',LastName) AS [Full Name], Ucn
	FROM Colonists
	ORDER BY FirstName,LastName,Id

SELECT Id,FORMAT (JourneyStart, 'dd/MM/yyyy'),FORMAT (JourneyEnd,'dd/MM/yyyy')
	FROM Journeys
	WHERE Purpose = 'Military'
	ORDER BY JourneyStart

SELECT c.Id,CONCAT(c.FirstName,' ',c.LastName) 
	FROM Colonists AS c
	JOIN TravelCards AS tc ON c.Id = tc.ColonistId
	WHERE tc.JobDuringJourney = 'Pilot'
	ORDER BY c.Id

SELECT COUNT(c.Id)
	FROM Colonists AS c
	JOIN TravelCards AS tc ON c.Id = tc.ColonistId
	JOIN Journeys AS j ON tc.JourneyId = j.Id
	WHERE j.Purpose = 'Technical'

SELECT TOP(1) s.[Name] AS [SpaceshipName],sp.[Name] AS [SpaceportName]
	FROM Spaceships AS s
	JOIN Journeys AS j ON s.Id = j.SpaceshipId
	JOIN Spaceports AS sp ON j.DestinationSpaceportId = sp.Id
	ORDER BY s.LightSpeedRate DESC

SELECT ss.Name, ss.Manufacturer
	FROM Spaceships AS ss
	JOIN Journeys AS j ON ss.Id = j.SpaceshipId
	JOIN TravelCards AS tc ON j.Id = tc.JourneyId
	JOIN Colonists AS c ON tc.ColonistId = c.Id
	WHERE DATEDIFF(year, c.BirthDate,'2019/01/01') < 30 AND tc.JobDuringJourney = 'Pilot'
	ORDER BY ss.[Name]