CREATE PROCEDURE SelectAllPeople @Age INT
AS
SELECT * FROM People WHERE Age > @Age

CREATE PROCEDURE SelectPerson @Name VARCHAR(50), @Id INT
AS
SELECT * FROM People WHERE [Name] = @Name AND Id = @Id

EXEC SelectAllPeople 
@Age = 12

EXEC SelectPerson
@Name = 'Atanas',
@Id = 1

CREATE FUNCTION f_calculateSum()
	RETURNS VARCHAR(50)
BEGIN
	DECLARE @result AS VARCHAR(50) = 
	(
		SELECT [Name] FROM People WHERE Id = 3
	)
	RETURN @result
END

DROP FUNCTION f_calculateSum


SELECT dbo.f_calculateSum()


