CREATE TABLE People
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
  	Picture VARBINARY(2000),
  	Height DECIMAL(3,2),
  	[Weight] DECIMAL(5,2),
  	Gender CHAR(1) NOT NULL,
  	BirthDate DATE NOT NULL,
  	Biography NVARCHAR(MAX)
)

INSERT INTO People([Name],Picture,Height,[Weight],Gender,BirthDate,Biography) VALUES
('Atanas',1500,1.78,75.7,'m','2008-11-11','Biography one'),
('Ivan',1700,1.71,71.7,'m','2008-11-11','Biography one'),
('Stoqn',1150,1.85,72.7,'m','2008-11-11','Biography one'),
('Kris',1200,1.81,73.7,'m','2008-11-11','Biography one'),
('Petar',1200,1.81,73.7,'m','2008-11-11','Biography one')