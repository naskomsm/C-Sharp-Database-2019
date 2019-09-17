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
	DirectorId INT NOT NULL FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear DATE,
	[Length] TINYINT,
	GenreId INT NOT NULL FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
	Rating TINYINT,
	Notes NVARCHAR(MAX)
)

INSERT INTO Directors(DirectorName,Notes) VALUES 
('Stoqn','note'),
('Gosho','note2'),
('Pesho','note3'),
('Atanas',NULL),
('Naiden',NULL)

INSERT INTO Categories(CategoryName,Notes) VALUES 
('Category1','note'),
('Category2','note2'),
('Category3','note3'),
('Category4',NULL),
('Category5',NULL)

INSERT INTO Genres(GenreName,Notes) VALUES 
('Genre1','note'),
('Gengre2','note2'),
('Gengre3','note3'),
('Gengre4',NULL),
('Gengre5',NULL)

INSERT INTO Movies(Title,CopyrightYear,[Length],Rating,Notes) VALUES
('Fast and Furious','2008-11-11',5,1,'note'),
('Fast and Furious 2','2008-11-11',55,1,NULL),
('Fast and Furious 3','2008-11-11',25,1,'note'),
('Fast and Furious 4','2008-11-11',15,1,NULL),
('Fast and Furious 5','2008-11-11',3,1,'note')