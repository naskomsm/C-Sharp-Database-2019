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

