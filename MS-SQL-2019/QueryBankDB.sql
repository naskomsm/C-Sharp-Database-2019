CREATE PROC usp_GetHoldersFullName
AS
SELECT CONCAT(a.FirstName, ' ', a.LastName) AS [Full Name]
	FROM AccountHolders AS a

EXEC usp_GetHoldersFullName

--

CREATE PROC usp_GetHoldersWithBalanceHigherThan (@number DECIMAL)
AS
SELECT ah.FirstName, ah.LastName
	FROM Accounts AS a
	JOIN AccountHolders AS ah ON ah.Id = a.AccountHolderId
	WHERE a.Balance > @number
	GROUP BY ah.FirstName,ah.LastName
	ORDER BY ah.FirstName,ah.LastName

EXEC usp_GetHoldersWithBalanceHigherThan 0

--
--

