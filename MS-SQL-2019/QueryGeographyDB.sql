USE Geography

SELECT CountryName,IsoCode
	FROM Countries
	WHERE CountryName LIKE '%a%a%a%'
	ORDER BY IsoCode

SELECT peaks.PeakName, rivers.RiverName, LOWER(LEFT(peaks.PeakName,LEN(peaks.PeakName) - 1) + rivers.RiverName) AS Mix
	FROM Peaks peaks, Rivers rivers
	WHERE RIGHT(PeakName,1) = LEFT(RiverName,1)
	ORDER BY Mix

SELECT p.PeakName,r.RiverName,LOWER(LEFT(p.PeakName,LEN(p.PeakName) - 1) + r.RiverName) AS Mix
	FROM Peaks AS p
	JOIN Rivers AS r ON RIGHT(p.PeakName,1) = LEFT(r.RiverName,1)
	ORDER BY [Mix]

	--12
SELECT mc.CountryCode, m.MountainRange, p.PeakName, p.Elevation
	FROM Mountains AS m
	JOIN Peaks AS p ON p.MountainId = m.Id
	JOIN MountainsCountries AS mc ON m.Id = mc.MountainId
	WHERE mc.CountryCode = 'BG' AND p.Elevation > 2835
	ORDER BY p.Elevation DESC

--13
SELECT mc.CountryCode, COUNT(m.MountainRange) AS MountainRanges
	FROM MountainsCountries AS mc
	JOIN Mountains AS m ON m.Id = mc.MountainId
	WHERE mc.CountryCode IN ('RU','BG','US')
	GROUP BY mc.CountryCode

--14
SELECT TOP(5) c.CountryName, r.RiverName
	FROM Countries AS c
	LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
	LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
	WHERE c.ContinentCode = 'AF'
	ORDER BY c.CountryName

--15
SELECT k.ContinentCode,k.CurrencyCode,k.CountOFCurrency 
FROM 
	(SELECT c.ContinentCode,c.CurrencyCode, COUNT(c.CurrencyCode) AS CountOFCurrency,
			 DENSE_RANK() OVER (PARTITION BY c.ContinentCode ORDER BY COUNT(c.CurrencyCode) DESC) AS CurrencyRank
		FROM Countries AS c
		GROUP BY c.ContinentCode,c.CurrencyCode
		HAVING COUNT(c.CurrencyCode) > 1) AS k
WHERE k.CurrencyRank = 1
ORDER BY k.ContinentCode

--16
SELECT COUNT(*) AS [Count]
	FROM Countries AS c
	LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
	WHERE mc.MountainId IS NULL

--17
SELECT TOP(5) c.CountryName, MAX(p.Elevation) AS HighestPeakElevation, MAX(r.Length) AS MaxRiverLength
	FROM Countries AS c
	FULL OUTER JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
	FULL OUTER JOIN Peaks AS p ON mc.MountainId = p.MountainId
	FULL OUTER JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
	FULL OUTER JOIN Rivers AS r ON r.Id = cr.RiverId
	GROUP BY c.CountryName
	ORDER BY MAX(p.Elevation) DESC, MAX(r.Length) DESC, c.CountryName

--18
SELECT TOP(5) k.CountryName,k.[Highest Peak Name],k.[Highest Peak Elevation],k.Mountain
	FROM
		(SELECT c.CountryName,
			 ISNULL(p.PeakName,'(no highest peak)') AS [Highest Peak Name],
			 ISNULL(p.Elevation,0) AS [Highest Peak Elevation],
			 ISNULL(m.MountainRange,'(no mountain)') AS [Mountain],
			 DENSE_RANK() OVER (PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS ElevationRank
		FROM Countries AS c
		LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
		LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
		LEFT JOIN Peaks AS p ON p.MountainId = m.Id
		) AS k
	WHERE k.ElevationRank = 1
	ORDER BY k.CountryName, k.[Highest Peak Name] DESC

