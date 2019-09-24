SELECT TOP(50) [Name],CONVERT(VARCHAR,[Start],23) AS [Start]
	FROM Games
	WHERE [Start] BETWEEN '2011-01-01' AND '2013-01-01'
	ORDER BY [Start],[Name]

SELECT Username, REPLACE(SUBSTRING(Email, CHARINDEX('@',Email), LEN(Email)),'@','') AS [Email Provider]
	FROM Users
	ORDER BY [Email Provider],Username