SELECT CountryName,IsoCode
	FROM Countries
	WHERE CountryName LIKE '%a%a%a%'
	ORDER BY IsoCode

SELECT peaks.PeakName, rivers.RiverName, LOWER(LEFT(peaks.PeakName,LEN(peaks.PeakName) - 1)+rivers.RiverName) AS Mix
	FROM Peaks peaks, Rivers rivers
	WHERE RIGHT(PeakName,1) = LEFT(RiverName,1)
	ORDER BY Mix