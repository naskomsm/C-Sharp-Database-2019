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

SELECT p.[Name],sp.[Name]
	FROM Planets AS p
	JOIN Spaceports AS sp ON p.Id = sp.PlanetId
	JOIN Journeys AS j ON j.DestinationSpaceportId = sp.Id
	WHERE j.Purpose = 'Educational'
	ORDER BY sp.[Name] DESC

SELECT p.[Name], COUNT(j.Id)
	FROM Planets AS p
	JOIN Spaceports AS sp ON p.Id = sp.PlanetId
	JOIN Journeys AS j ON j.DestinationSpaceportId = sp.Id
	GROUP BY p.[Name]
	ORDER BY COUNT(j.Id) DESC, p.[Name]

SELECT TOP (1) j.Id, p.[Name],sp.[Name],j.Purpose
	FROM Planets AS p
	JOIN Spaceports AS sp ON p.Id = sp.PlanetId
	JOIN Journeys AS j ON j.DestinationSpaceportId = sp.Id
	ORDER BY DATEDIFF(DAY, j.JourneyStart,j.JourneyEnd)

SELECT TOP(1) j.Id,tc.JobDuringJourney
	FROM Planets AS p
	JOIN Spaceports AS sp ON p.Id = sp.PlanetId
	JOIN Journeys AS j ON j.DestinationSpaceportId = sp.Id
	JOIN TravelCards AS tc ON tc.JourneyId = j.Id
	ORDER BY DATEDIFF(DAY, j.JourneyStart,j.JourneyEnd) DESC

SELECT p.[Name], COUNT(sp.Id)
	FROM Planets AS p
	LEFT JOIN Spaceports AS sp ON p.Id = sp.PlanetId
	GROUP BY p.[Name]
	ORDER BY COUNT(sp.Id) DESC, p.[Name]

CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR(30))
RETURNS INT
BEGIN
	DECLARE @colonistsCount INT
	SET @colonistsCount =
	(
		SELECT COUNT(*)
			FROM Colonists AS c
			JOIN TravelCards AS tc ON c.Id = tc.ColonistId
			JOIN Journeys AS j ON j.Id = tc.JourneyId
			JOIN Spaceports AS sp ON j.DestinationSpaceportId = sp.Id
			JOIN Planets AS p ON sp.PlanetId = p.Id
			WHERE p.[Name] = @PlanetName 
	)

	RETURN @colonistsCount;
END

SELECT dbo.udf_GetColonistsCount('Otroyphus')

CREATE OR ALTER PROC usp_ChangeJourneyPurpose (@JourneyId INT, @NewPurpose VARCHAR(11))
AS 
BEGIN
	DECLARE @doesJourneyExist INT
	SET @doesJourneyExist = 
	(
		SELECT Id
			FROM Journeys
			WHERE Id = @JourneyId
	)

	IF(@doesJourneyExist IS NULL)
		BEGIN
			RAISERROR('The journey does not exist!',16,1)
		END

	DECLARE @currentPurpose VARCHAR(11)
	SET @currentPurpose = 
	(
		SELECT Purpose	
			FROM Journeys
			WHERE Id = @JourneyId
	)

	IF(@currentPurpose = @NewPurpose)
		BEGIN
			RAISERROR('You cannot change the purpose!',16,1)
		END

	UPDATE Journeys
	SET Purpose = @NewPurpose
	WHERE Id = @JourneyId
END

EXEC usp_ChangeJourneyPurpose 1, 'Technical'
SELECT * FROM Journeys