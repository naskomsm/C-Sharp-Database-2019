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

--not finished
SELECT w.DepositGroup,w.MagicWandCreator, w.DepositCharge
	FROM WizzardDeposits as w
	ORDER BY w.MagicWandCreator,w.DepositGroup
	
--not finished
SELECT w.Age as [AgeGroup], Count(w.Id)
	FROM WizzardDeposits as w
	GROUP BY
	CASE
		WHEN w.Age BETWEEN 0 AND 10 THEN '[0-10]'
		WHEN w.Age BETWEEN 11 AND 20 THEN '[11-20]'
		WHEN w.Age BETWEEN 21 AND 30 THEN '[21-30]'
		WHEN w.Age BETWEEN 31 AND 40 THEN '[30-40]'
		WHEN w.Age BETWEEN 41 AND 50 THEN '[41-50]'
		WHEN w.Age BETWEEN 51 AND 60 THEN '[51-60]'
		ELSE '[61+]'
	END

--not finished
SELECT LEFT(w.FirstName,1) as FirstLetter,w.DepositGroup
	FROM WizzardDeposits as w
	WHERE w.DepositGroup = 'Troll Chest'
	ORDER BY FirstLetter