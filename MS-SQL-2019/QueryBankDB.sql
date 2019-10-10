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

--

CREATE FUNCTION ufn_CalculateFutureValue(@sum DECIMAL,@yearlyInterestRate FLOAT,@numberOfYears INT)
RETURNS DECIMAL(18,4)
AS
BEGIN
	DECLARE @futureValue DECIMAL(18,4)
	SET @futureValue = @sum * (POWER((1 + @yearlyInterestRate),@numberOfYears))
	RETURN @futureValue
END


--

CREATE PROC usp_CalculateFutureValueForAccount(@accountId INT, @yearlyInterestRate FLOAT)
AS
BEGIN
	SELECT @accountId AS [Account Id], ah.FirstName,ah.LastName, a.Balance, dbo.ufn_CalculateFutureValue(a.Balance, @yearlyInterestRate,5) AS [Balance in 5 years]
		FROM Accounts AS a
		JOIN AccountHolders AS ah ON a.AccountHolderId = ah.Id
		WHERE a.Id = @accountId
END

EXEC usp_CalculateFutureValueForAccount 1, 0.1