CREATE DATABASE Service

USE Service

CREATE TABLE Users
(
	Id INT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(50) NOT NULL,
	[Name] VARCHAR(50),
	Birthdate DATETIME2,
	Age INT CHECK(Age BETWEEN 14 AND 110),
	Email VARCHAR(50) NOT NULL
)

CREATE TABLE Departments
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(25),
	LastName VARCHAR(25),
	Birthdate DATETIME2,
	Age INT CHECK(Age BETWEEN 18 AND 110),
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL
)

CREATE TABLE Status
(
	Id INT PRIMARY KEY IDENTITY,
	[Label] VARCHAR(30) NOT NULL
)

CREATE TABLE Reports
(
	Id INT PRIMARY KEY IDENTITY,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	StatusId INT FOREIGN KEY REFERENCES Status(Id) NOT NULL,
	OpenDate DATETIME2 NOT NULL,
	CloseDate DATETIME2,
	[Description] VARCHAR(200) NOT NULL,
	UserId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id)
)

--INSERT
INSERT INTO Employees(FirstName,LastName,Birthdate,DepartmentId) VALUES
('Marlo',	'O''Malley',	'1958-9-21',	1),
('Niki',	'Stanaghan',	'1969-11-26',	4),
('Ayrton',	'Senna',		'1960-03-21',	9),
('Ronnie',	'Peterson',		'1944-02-14',	9),
('Giovanna',	'Amati',	'1959-07-20',	5)

INSERT INTO Reports (CategoryId,StatusId,OpenDate,CloseDate,Description,UserId,EmployeeId) VALUES
(1,	1,	'2017-04-13', NULL,	'Stuck Road on Str.133', 6,	2),
(6,	3,	'2015-09-05', '2015-12-06',	'Charity trail running', 3,	5),
(14, 2,	'2015-09-07', NULL , 'Falling bricks on Str.58', 5,	2),
(4, 3,	'2017-07-03', '2017-07-06' , 'Cut off streetlight on Str.11', 1, 1)

--UPDATE
UPDATE Reports
SET CloseDate = GETDATE()
WHERE CloseDate IS NULL

--DELETE
DELETE FROM Reports
WHERE Id = 4

--Querying

--5
SELECT Description, FORMAT (OpenDate, 'dd-MM-yyyy') AS [OpenDate]
	FROM Reports
	WHERE EmployeeId IS NULL
	ORDER BY CAST([OpenDate] AS DATETIME) , Description

--6
SELECT r.Description,c.Name
	FROM Reports AS r
	JOIN Categories AS c ON r.CategoryId = c.Id
	ORDER BY r.Description, c.Name

--7
SELECT TOP(5) c.[Name], COUNT(r.Id)
	FROM Categories AS c
	JOIN Reports AS r ON r.CategoryId = c.Id
	GROUP BY c.[Name]
	ORDER BY COUNT(r.Id) DESC, c.[Name]

--8
SELECT u.Username, c.[Name]
	FROM Users AS u
	JOIN Reports AS r ON u.Id = r.UserId
	JOIN Categories AS c ON r.CategoryId = c.Id
	WHERE DAY(u.Birthdate) = DAY(r.OpenDate) AND MONTH(u.Birthdate) = MONTH(r.OpenDate)
	ORDER BY u.Username, c.[Name]

--9
SELECT CONCAT(e.FirstName, ' ',e.LastName), COUNT(u.Username)
	FROM Employees AS e
	LEFT JOIN Reports AS r ON e.Id = r.EmployeeId
	LEFT JOIN Users AS u ON r.UserId = u.Id
	GROUP BY CONCAT(e.FirstName, ' ',e.LastName)
	ORDER BY COUNT(u.Username) DESC, CONCAT(e.FirstName, ' ',e.LastName)

--10 ( FIX THE REPEATING )
SELECT CONCAT(e.Firstname, ' ', e.LastName) AS [Employee], 
								d.[Name] AS [Department],
								c.[Name] AS [Category],
								r.[Description],
								FORMAT(r.OpenDate,'dd.MM.yyyy') AS [OpenDate],
								s.[Label],
								u.[Name]
	FROM Reports AS r
	FULL JOIN Employees AS e ON r.EmployeeId = e.Id
	FULL JOIN Departments AS d ON e.DepartmentId = d.Id
	JOIN Categories AS c ON c.Id = r.CategoryId
	JOIN Status AS s ON r.StatusId = s.Id
	JOIN Users AS u ON r.UserId = u.Id
	ORDER BY e.FirstName DESC,
				e.LastName DESC,
				d.[Name], 
				c.[Name], 
				r.[Description], 
				CAST(r.OpenDate AS DATETIME), 
				s.[Label], 
				u.[Name]

--11
CREATE OR ALTER FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
BEGIN
	DECLARE @totalHours INT

	IF(@StartDate IS NULL OR @EndDate IS NULL)
		BEGIN
			SET @totalHours = 0
		END

	ELSE
		BEGIN
			SET @totalHours = DATEDIFF(HH, @StartDate, @EndDate)
		END

	RETURN @totalHours
END

SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
   FROM Reports

--12
CREATE OR ALTER PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN
	DECLARE @employeeDepartment INT
	SET @employeeDepartment =
	(
		SELECT DepartmentId
			FROM Employees
			WHERE Id = @EmployeeId
	)

	DECLARE @reportDepartment INT
	SET @reportDepartment =
	(
		SELECT c.DepartmentId
			FROM Reports AS r 
			JOIN Categories AS c ON r.CategoryId = c.Id
			WHERE r.Id = @ReportId
	)

	IF(@employeeDepartment != @reportDepartment)
		BEGIN 
			RAISERROR('Employee doesn''t belong to the appropriate department!',16,1)
		END
		
	ELSE
		BEGIN
			UPDATE Reports
			SET EmployeeId = @EmployeeId
			WHERE Id = @ReportId
		END
END

EXEC usp_AssignEmployeeToReport 30, 1