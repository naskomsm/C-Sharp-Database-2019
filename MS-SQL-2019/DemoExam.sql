CREATE DATABASE Bitbucket

USE Bitbucket

CREATE TABLE Users
(
	Id INT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL,
	[Password] VARCHAR(30) NOT NULL,
	Email VARCHAR(50) NOT NULL,
)


CREATE TABLE Repositories
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE RepositoriesContributors
(
	RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
	ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

ALTER TABLE RepositoriesContributors
ADD CONSTRAINT PrimaryCompositeKey_RepoContributorId 
PRIMARY KEY (RepositoryId,ContributorId)

CREATE TABLE Issues
(
	Id INT PRIMARY KEY IDENTITY,
	Title VARCHAR(255) NOT NULL,
	IssueStatus CHAR(6) NOT NULL,
	RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
	AssigneeId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Commits
(
	Id INT PRIMARY KEY IDENTITY,
	[Message] VARCHAR(255) NOT NULL,
	IssueId INT FOREIGN KEY REFERENCES Issues(Id),
	RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
	ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Files
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(100) NOT NULL,
	Size DECIMAL(18,2) NOT NULL,
	ParentId INT FOREIGN KEY REFERENCES Files(Id),
	CommitId INT FOREIGN KEY REFERENCES Commits(Id) NOT NULL
)

INSERT INTO Files (Name,Size) VALUES
('Trade.idk',2598.0),
('menu.net',9238.31),
('Administrate.soshy',1246.93),
('Controller.php',7353.15),
('Find.java',9957.86),
('Controller.json',14034.87),
('Operate.xix',7662.92)


INSERT INTO Issues (Title,IssueStatus) VALUES
('Critical Problem with HomeController.cs file','open'),
('Typo fix in Judge.html','open'),
('Implement documentation for UsersService.cs','closed'),
('Unreachable code in Index.cs','open')


UPDATE Issues
SET IssueStatus = 'closed'
WHERE AssigneeId = 6


ALTER TABLE Files NOCHECK CONSTRAINT ALL

DELETE FROM Commits
WHERE RepositoryId = ( SELECT r.Id
				FROM Repositories AS r
				WHERE r.Name = 'Softuni-Teamwork')

DELETE FROM Issues
WHERE RepositoryId = ( SELECT r.Id
				FROM Repositories AS r
				WHERE r.Name = 'Softuni-Teamwork')

DELETE FROM RepositoriesContributors
WHERE RepositoryId = ( SELECT r.Id
				FROM Repositories AS r
				WHERE r.Name = 'Softuni-Teamwork')

DELETE FROM Repositories
WHERE Name = 'Softuni-Teamwork' 

SELECT Id,Message,RepositoryId,ContributorId
	FROM Commits
	ORDER BY Id,Message,RepositoryId,ContributorId

SELECT Id,Name,Size
	FROM Files
	WHERE Size > 1000 AND Name LIKE '%html%'
	ORDER BY Size DESC,Id,Name

SELECT i.Id, CONCAT(u.Username,' : ',i.Title) AS [IssueAssignee]
	FROM Issues AS i
	JOIN Users AS u ON i.AssigneeId = u.Id
	ORDER BY i.Id DESC,i.AssigneeId

	-- not finished ( 8 )
SELECT f1.Id,f1.Name, CONCAT(f1.Size,'KB') AS [Size]
	FROM Files AS f1
	LEFT JOIN Files AS f2 ON f1.Id = f2.ParentId
	WHERE f2.Id IS NULL
	ORDER BY f1.Id,f2.Name,Size desc

SELECT TOP(5) r.Id,r.Name, COUNT(c.RepositoryId) AS [Commits]
	FROM Commits AS c
	JOIN RepositoriesContributors AS rc ON c.RepositoryId = rc.RepositoryId
	JOIN Repositories AS r ON r.Id = rc.RepositoryId
	GROUP BY r.Id,r.Name
	ORDER BY [Commits] DESC,r.Id,r.Name

SELECT u.Username, AVG(f.Size) AS [Size]
	FROM Commits AS c
	JOIN Users AS u ON c.ContributorId = u.Id
	JOIN Files AS f ON c.Id = f.CommitId
	GROUP BY u.Username
	ORDER BY [Size] DESC,u.Username

CREATE OR ALTER FUNCTION udf_UserTotalCommits(@username VARCHAR(50))
RETURNS INT
AS
BEGIN
	DECLARE @commitsCount INT
	SET @commitsCount = 
	(
		SELECT COUNT(*)
			FROM Commits AS c
			JOIN Users AS u ON c.ContributorId = u.Id
			WHERE u.Username = @username
	)

	RETURN @commitsCount
END

SELECT dbo.udf_UserTotalCommits('UnderSinduxrein')

CREATE OR ALTER PROC usp_FindByExtension(@extension VARCHAR(10))
AS
BEGIN
	SELECT f.Id,f.Name, CONCAT(f.Size,'KB') AS [Size]
		FROM Files AS f
		WHERE f.Name LIKE '%' + @extension
		ORDER BY f.Id,f.Name,f.Size DESC
END



EXEC usp_FindByExtension 'net'
