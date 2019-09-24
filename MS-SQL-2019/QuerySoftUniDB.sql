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


SELECT EmployeeID,FirstName,LastName,Salary,
	DENSE_RANK() OVER
	PARTITION BY Salary ORDER BY EmployeeID AS [Rank]
	FROM (Employees
	WHERE Salary BETWEEN 10000 AND 50000
	ORDER BY Salary DESC)
