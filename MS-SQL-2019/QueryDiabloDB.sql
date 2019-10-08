SELECT TOP(50) [Name],FORMAT([Start],'yyyy-MM-dd') AS [Start]
	FROM Games
	WHERE [Start] BETWEEN '2011-01-01' AND '2013-01-01'
	ORDER BY [Start],[Name]

SELECT Username, REPLACE(SUBSTRING(Email, CHARINDEX('@',Email), LEN(Email)),'@','') AS [Email Provider]
	FROM Users
	ORDER BY [Email Provider],Username

SELECT Username, IpAddress
	FROM Users
	WHERE IpAddress LIKE '___.1%.%.___'
	ORDER BY Username

SELECT [Name] AS [Game],
CASE
	WHEN DATEPART(HOUR, [Start]) BETWEEN 0 AND 11 THEN 'Morning'
	WHEN DATEPART(HOUR, [Start]) BETWEEN 12 AND 17 THEN 'Afternoon'
	WHEN DATEPART(HOUR, [Start]) BETWEEN 18 AND 23 THEN 'Evening'
END AS [Part of the Day],
CASE
	WHEN Duration <= 3 THEN 'Extra Short'
	WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
	WHEN Duration > 6 THEN 'Long'
	WHEN Duration IS NULL THEN 'Extra Long'
END AS Duration
		FROM Games
		ORDER BY [Name], [Duration], [Part of the Day]

CREATE FUNCTION ufn_CashInUsersGames()
RETURNS @resultTable TABLE (SumCash DECIMAL(10,2))
AS
BEGIN
	INSERT INTO @resultTable
	SELECT t.Cash FROM
	(SELECT *,Row_Number() OVER(ORDER BY Cash DESC) AS RowNumber  
		FROM (SELECT * FROM UsersGames) AS ug
		WHERE ug.GameId = 49) AS t
		WHERE t.RowNumber % 2 = 1

	RETURN
END

SELECT * FROM ufn_CashInUsersGames()

CREATE FUNCTION ufn_CashInUsersGames(@gameName VARCHAR(MAX))
RETURNS @output TABLE (SumCash DECIMAL(18,4))
AS
BEGIN 
	INSERT INTO @output SELECT 
	(		SELECT SUM(Cash) AS [SumCash]
			FROM
			(SELECT *, ROW_NUMBER() OVER (ORDER BY Cash DESC) AS [RowNum]
				FROM UsersGames
				WHERE GameId IN
				(
					SELECT Id	
						FROM Games
						WHERE [Name] = @gameName
				)
			) AS [RowNumTable]
			WHERE [RowNum] % 2 <> 0
	)

	RETURN
END

SELECT * FROM dbo.ufn_CashInUsersGames('Love in a mist') 