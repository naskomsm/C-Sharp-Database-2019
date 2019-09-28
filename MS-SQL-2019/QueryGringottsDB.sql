SELECT Count(w.Id) as [Count]
	FROM WizzardDeposits as w

SELECT MAX(w.MagicWandSize) as LongestMagicWand
	FROM WizzardDeposits as w

SELECT TOP(2) w.DepositGroup
	FROM WizzardDeposits as w
	GROUP BY w.DepositGroup
	ORDER BY AVG(w.MagicWandSize)

SELECT w.DepositGroup, SUM(w.DepositAmount) as [TotalSum]
	FROM WizzardDeposits as w
	GROUP BY w.DepositGroup

SELECT w.DepositGroup, SUM(w.DepositAmount) as [TotalSum]
	FROM WizzardDeposits as w
	WHERE w.MagicWandCreator = 'Ollivander family'
	GROUP BY w.DepositGroup

SELECT w.DepositGroup, SUM(w.DepositAmount) as [TotalSum]
	FROM WizzardDeposits as w
	WHERE w.MagicWandCreator = 'Ollivander family'
	GROUP BY w.DepositGroup
	HAVING SUM(w.DepositAmount) < 150000
	ORDER BY SUM(w.DepositAmount) DESC

SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) AS [MinDepositCharge]
	FROM WizzardDeposits
	GROUP BY DepositGroup, MagicWandCreator
	ORDER BY MagicWandCreator, DepositGroup

SELECT 
	CASE
		WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
		WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
		WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
		WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
		WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
		WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
		ELSE '[61+]'
	END AS [AgeGroup], COUNT(Id) AS [WizzardCount]
	FROM WizzardDeposits
	GROUP BY CASE
		WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
		WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
		WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
		WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
		WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
		WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
		ELSE '[61+]'
	END

SELECT SUM([Difference]) AS [SumDifference] 
	FROM (SELECT FirstName AS [Host Wizard], DepositAmount AS [Wizard Deposit], 
				LEAD(FirstName) OVER(ORDER BY Id) AS [Guest Wizard], 
				LEAD(DepositAmount) OVER(ORDER BY Id) AS [Guest Wizard Deposit],
				(DepositAmount - LEAD(DepositAmount) OVER(ORDER BY Id)) AS [Difference]
		FROM WizzardDeposits) AS DiffTable

SELECT LEFT(FirstName,1) AS [FirstLetter]
	FROM WizzardDeposits
	WHERE DepositGroup = 'Troll Chest'
	GROUP BY LEFT(FirstName,1)
	ORDER BY [FirstLetter]

SELECT DepositGroup, IsDepositExpired, AVG(DepositInterest) AS [AverageInterest]
	FROM WizzardDeposits
	WHERE DepositStartDate > '01-01-1985'
	GROUP BY DepositGroup,IsDepositExpired
	ORDER BY DepositGroup DESC, IsDepositExpired

