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

----------

SELECT e.DepartmentID, SUM(e.Salary) as [TotalSalary] 
	FROM Employees as e
	GROUP BY e.DepartmentID


SELECT e.DepartmentID, MIN(e.Salary) as [MinimumSalary]
	FROM Employees as e
	WHERE e.HireDate > '01-01-2000'
	GROUP BY e.DepartmentID
	HAVING e.DepartmentID IN (2,5,7)

--not finished
SELECT e.DepartmentID, AVG(e.Salary) as [AverageSalary]
	FROM Employees as e
	WHERE e.Salary > 30000 AND e.ManagerID <> 42
	GROUP BY e.DepartmentID

--not finished
SELECT e.DepartmentID, MAX(e.Salary) as [MaxSalary]
	FROM Employees as e
	WHERE e.Salary < 30000 OR e.Salary > 70000
	GROUP BY e.DepartmentID

SELECT COUNT(e.EmployeeID) as [Count]
	FROM Employees as e
	WHERE e.ManagerID IS NULL


