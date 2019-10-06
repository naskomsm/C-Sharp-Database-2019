CREATE TABLE Directors
(
	Id INT PRIMARY KEY IDENTITY,
	DirectorName VARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Genres
(
	Id INT PRIMARY KEY IDENTITY,
	GenreName VARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName VARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Movies
(
	Id INT PRIMARY KEY IDENTITY,
	Title VARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear DATE,
	[Length] TINYINT,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Rating TINYINT,
	Notes NVARCHAR(MAX)
)


INSERT INTO Directors(DirectorName,Notes) VALUES 
('Stoqn',NULL),
('Gosho',NULL),
('Pesho',NULL),
('Atanas',NULL),
('Naiden',NULL)

INSERT INTO Categories(CategoryName,Notes) VALUES 
('Category1',NULL),
('Category2',NULL),
('Category3',NULL),
('Category4',NULL),
('Category5',NULL)

INSERT INTO Genres(GenreName,Notes) VALUES 
('Genre1',NULL),
('Genre2',NULL),
('Genre3',NULL),
('Genre4',NULL),
('Genre5',NULL)

INSERT INTO Movies(Title,CopyrightYear,[Length],Rating,Notes) VALUES
('Fast and Furious 1','2008-11-11',5,1,NULL),
('Fast and Furious 2','2008-11-11',55,1,NULL),
('Fast and Furious 3','2008-11-11',25,1,NULL),
('Fast and Furious 4','2008-11-11',15,1,NULL),
('Fast and Furious 5','2008-11-11',3,1,NULL)