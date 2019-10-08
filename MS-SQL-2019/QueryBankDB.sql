CREATE PROC usp_GetHoldersFullName
AS
SELECT CONCAT(a.FirstName, ' ', a.LastName) AS [Full Name]
	FROM AccountHolders AS a

EXEC usp_GetHoldersFullName

--

CREATE PROC usp_GetHoldersWithBalanceHigherThan (@number MONEY)
AS
BEGIN
	SELECT ah.FirstName, ah.LastName
		FROM Accounts AS a
		JOIN AccountHolders AS ah ON ah.Id = a.AccountHolderId
		GROUP BY ah.FirstName,ah.LastName
		HAVING SUM(a.Balance) > @number
		ORDER BY ah.FirstName,ah.LastName
END

EXEC dbo.usp_GetHoldersWithBalanceHigherThan 1000




