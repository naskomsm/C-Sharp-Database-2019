SELECT CountryName,IsoCode
	FROM Countries
	WHERE CountryName LIKE 'a_%_%'
	ORDER BY IsoCode