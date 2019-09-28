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