USE SoftUni

SELECT FirstName,LastName
	FROM Employees
	WHERE FirstName LIKE 'Sa%'


SELECT FirstName,LastName
	FROM Employees
	WHERE LastName LIKE '%ei%'

SELECT FirstName
	FROM Employees
	WHERE DepartmentID IN (3,10) 
	AND (HireDate BETWEEN '1995-01-01' AND '2006-01-01')

SELECT FirstName,LastName
	FROM Employees
	WHERE JobTitle NOT LIKE '%engineer%'

SELECT [Name]
	FROM Towns
	WHERE LEN([Name]) IN (5,6)
	ORDER BY [Name]

SELECT TownID,[Name]
	FROM Towns
	WHERE [Name] LIKE '[MKBE]%'
	ORDER BY [Name]

SELECT TownID,[Name]
	FROM Towns
	WHERE [Name] NOT LIKE '[RBD]%'
	ORDER BY [Name]

CREATE VIEW V_EmployeesHiredAfter2000
	AS
	SELECT FirstName,LastName
	FROM Employees
	WHERE HireDate > '2001-01-01'

SELECT * FROM V_EmployeesHiredAfter2000

SELECT FirstName,LastName
	FROM Employees
	WHERE LEN(LastName) = 5


SELECT * FROM (SELECT EmployeeID,FirstName,LastName,Salary, DENSE_RANK() OVER (PARTITION BY Salary ORDER BY EmployeeID) AS [Rank]
	FROM Employees
	WHERE Salary BETWEEN 10000 AND 50000) AS temp
	WHERE temp.[Rank] = 2
	ORDER BY temp.Salary DESC

-----

SELECT e.DepartmentID, SUM(e.Salary) as [TotalSalary] 
	FROM Employees as e
	GROUP BY e.DepartmentID


SELECT e.DepartmentID, MIN(e.Salary) as [MinimumSalary]
	FROM Employees as e
	WHERE e.HireDate > '01-01-2000'
	GROUP BY e.DepartmentID
	HAVING e.DepartmentID IN (2,5,7)

SELECT * INTO 
	[NewTable]
	FROM Employees
	WHERE Salary > 30000

DELETE FROM NewTable WHERE ManagerID = 42

UPDATE NewTable
SET Salary += 5000
WHERE DepartmentID = 1

SELECT DepartmentID, AVG(Salary) AS [AverageSalary]
	FROM NewTable
	GROUP BY DepartmentID

SELECT DepartmentID, MAX(Salary) as [MaxSalary]
	FROM Employees
	GROUP BY DepartmentID
	HAVING MAX(Salary) < 30000 OR MAX(Salary) > 70000

SELECT COUNT(e.EmployeeID) as [Count]
	FROM Employees as e
	WHERE e.ManagerID IS NULL

SELECT DISTINCT DepartmentID, Salary AS [ThirdHighestSalary]
	FROM (SELECT DepartmentID,Salary ,DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS [Rank]
				FROM Employees) AS [RankTable]
	WHERE [Rank] = 3

SELECT TOP(10) FirstName,LastName, DepartmentID
	FROM Employees AS e1
	WHERE Salary > (SELECT AVG(Salary) AS [AvgSalary]
					FROM Employees AS e2
					WHERE e2.DepartmentID = e1.DepartmentID
					GROUP BY DepartmentID) 
ORDER BY e1.DepartmentID

SELECT TOP(5) e.EmployeeID, e.JobTitle, e.AddressID, a.AddressText
	FROM Employees AS e
	JOIN Addresses AS a ON a.AddressID = e.AddressID
	ORDER BY e.AddressID

--2
SELECT TOP(50) e.FirstName,e.LastName, t.Name,a.AddressText
	FROM Employees AS e
	JOIN Addresses AS a ON a.AddressID = e.AddressID
	JOIN Towns AS t ON a.TownID = t.TownID
	ORDER BY e.FirstName,e.LastName

--3
SELECT e.EmployeeID,e.FirstName,e.LastName,d.Name
	FROM Employees AS e
	JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
	WHERE d.Name = 'Sales'
	ORDER BY e.EmployeeID

--4
SELECT TOP(5) e.EmployeeID,e.FirstName,e.Salary,d.Name
	FROM Employees AS e
	JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
	WHERE e.Salary > 15000
	ORDER BY d.DepartmentID

--5
SELECT TOP(3) e.EmployeeID,e.FirstName
	FROM Employees AS e
	LEFT OUTER JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
	WHERE ep.ProjectID IS NULL
	ORDER BY e.EmployeeID

--6
SELECT e.FirstName,e.LastName,e.HireDate,d.[Name] AS [DeptName]
	FROM Employees AS e
	JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
	WHERE e.HireDate > '1999-1-1' AND d.[Name] IN ('Sales','Finance')
	ORDER BY e.HireDate

--7
SELECT TOP(5) e.EmployeeID,e.FirstName,p.Name AS ProjectName
	FROM Employees AS e
	JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
	JOIN Projects AS p ON p.ProjectID = ep.ProjectID
	WHERE p.StartDate > '2002-08-13' AND p.EndDate IS NULL
	ORDER BY e.EmployeeID

--8 ( NOT FINISHED )
SELECT e.EmployeeID,e.FirstName,
	CASE
		WHEN p.StartDate >= '2005-01-01' THEN NULL
		ELSE p.[Name]
	END AS ProjectName
	FROM Employees AS e
	LEFT JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
	LEFT JOIN Projects AS p ON ep.ProjectID = p.ProjectID
	WHERE e.EmployeeID = 24


--9
SELECT e2.EmployeeID,e2.FirstName,e.EmployeeID AS ManagerID,e.FirstName AS ManagerName
	FROM Employees AS e
	JOIN Employees AS e2 ON e.EmployeeID = e2.ManagerID
	WHERE e.EmployeeID IN (3,7)
	ORDER BY e2.EmployeeID

--10
SELECT TOP(50) e.EmployeeID,
				CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
				CONCAT(m.FirstName, ' ', m.LastName) AS ManagerName,
				d.Name AS DepartmentName
	FROM Employees AS e
		LEFT JOIN Employees AS m ON m.EmployeeID = e.ManagerID
		LEFT JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
	ORDER BY e.EmployeeID

--11
SELECT MIN(AvgSalaries.AverageSalary) AS MinAverageSalary
	FROM
		(SELECT AVG(e.Salary) as AverageSalary
			FROM Employees AS e
			GROUP BY e.DepartmentID) AS [AvgSalaries]

CREATE PROC usp_GetEmployeesSalaryAbove35000
AS
SELECT e.FirstName, e.LastName 
	FROM Employees AS e
	WHERE e.Salary > 35000

EXEC usp_GetEmployeesSalaryAbove35000

--

CREATE PROC usp_GetEmployeesSalaryAboveNumber(@number DECIMAL(18,4))
AS
SELECT e.FirstName, e.LastName 
	FROM Employees AS e
	WHERE e.Salary >= @number

EXEC usp_GetEmployeesSalaryAboveNumber 48100

--

CREATE PROC usp_GetTownsStartingWith (@random NVARCHAR(50))
AS
SELECT t.[Name]
	FROM Towns AS t
	WHERE t.[Name] LIKE @random + '%' 

EXEC usp_GetTownsStartingWith 'b'

--

CREATE PROC usp_GetEmployeesFromTown(@town NVARCHAR(50))
AS
SELECT e.FirstName,e.LastName
	FROM Employees AS e
	JOIN Addresses AS a ON e.AddressID = a.AddressID
	JOIN Towns AS t ON t.TownID = a.TownID
	WHERE t.[Name] = @town
	
EXEC usp_GetEmployeesFromTown 'Sofia'

--

CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS NVARCHAR(30)
AS
BEGIN
	DECLARE @salaryLevel NVARCHAR(30)
	IF (@salary < 30000)
		SET @salaryLevel = 'Low'
	ELSE IF (@salary BETWEEN 30000 AND 50000)
		SET @salaryLevel = 'Average'
	ELSE
		SET @salaryLevel = 'High'

	RETURN @salaryLevel
END

SELECT e.Salary, dbo.ufn_GetSalaryLevel(e.Salary) AS [Salary Level]
	FROM Employees AS e

--

CREATE PROC usp_EmployeesBySalaryLevel(@levelOfSalary NVARCHAR(10))
AS
SELECT information.FirstName, information.LastName
	FROM 
	(SELECT e.FirstName,e.LastName,e.Salary, dbo.ufn_GetSalaryLevel(e.Salary) AS [Salary Level]
	FROM Employees AS e) AS information
	WHERE information.[Salary Level] = @levelOfSalary

EXEC usp_EmployeesBySalaryLevel 'High'

--

CREATE OR ALTER FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(20), @word NVARCHAR(20))
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT
	DECLARE @cnt INT = 1

	WHILE @cnt <= LEN(@word)
	BEGIN
		DECLARE @currentLetter CHAR(1)
		SET @currentLetter = SUBSTRING(@word, @cnt, 1)

		IF(CHARINDEX(@currentLetter,@setOfLetters) > 0)
			SET @result = 1
		ELSE
			BEGIN
				SET @result = 0
				BREAK
			END

		SET @cnt = @cnt + 1;
	END

	RETURN @result
END


--

CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
	DELETE FROM EmployeesProjects
	WHERE EmployeeID IN 
	(
		SELECT EmployeeID
		FROM Employees
		WHERE DepartmentID = @departmentId
	)

	UPDATE Employees
	SET ManagerID = NULL
	WHERE ManagerID IN 
	(
		SELECT EmployeeID
		FROM Employees
		WHERE DepartmentID = @departmentId
	)

	ALTER TABLE Departments
	ALTER COLUMN ManagerId INT

	UPDATE Departments
	SET ManagerID = NULL
	WHERE DepartmentID = @departmentId

	DELETE 
		FROM Employees
		WHERE DepartmentID = @departmentId

	DELETE 
		FROM Departments
		WHERE DepartmentID = @departmentId

	SELECT COUNT(*)
		FROM Employees
		WHERE DepartmentID = @departmentId

EXEC usp_DeleteEmployeesFromDepartment 1