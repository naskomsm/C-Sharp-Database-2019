CREATE DATABASE School

USE School

CREATE TABLE Students
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(30) NOT NULL,
	MiddleName NVARCHAR(30),
	LastName NVARCHAR(30) NOT NULL,
	Age INT CHECK(Age > 0),
	[Address] NVARCHAR(50),
	Phone VARCHAR(10)
)

CREATE TABLE Subjects
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(20) NOT NULL,
	Lessons INT NOT NULL
)

CREATE TABLE StudentsSubjects
(
	Id INT PRIMARY KEY IDENTITY,
	StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NULL,
	Grade DECIMAL(18, 2) NOT NULL CHECK(Grade >= 2 AND Grade <= 6)
)

CREATE TABLE Exams
(
	Id INT PRIMARY KEY IDENTITY,
	[Date] DATETIME,
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NULL
)

CREATE TABLE StudentsExams
(
	StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(Id),
	ExamId INT NOT NULL FOREIGN KEY REFERENCES Exams(Id),
	Grade DECIMAL(18,2) CHECK (Grade >= 2 AND Grade <= 6) NOT NULL

	CONSTRAINT PK_StudentsExams
	PRIMARY KEY(StudentId, ExamId)
)

CREATE TABLE Teachers
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(20) NOT NULL,
	LastName NVARCHAR(20) NOT NULL,
	[Address] NVARCHAR(20),
	Phone VARCHAR(10),
	SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id)
)

CREATE TABLE StudentsTeachers
(
	StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(Id),
	TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)

	CONSTRAINT PK_StudentsTeachers
	PRIMARY KEY(StudentId, TeacherId),
)

INSERT INTO Subjects (Name,Lessons) VALUES
('Geometry',12),
('Health',10),
('Drama',7),
('Sports',9)


INSERT INTO Teachers (FirstName,LastName,Address,Phone,SubjectId) VALUES
('Ruthanne','Bamb','84948 Mesta Junction','3105500146',6),
('Gerrard','Lowin','370 Talisman Plaza','3324874824',2),
('Merrile','Lambdin','81 Dahle Plaza','4373065154',5),
('Bert','Ivie','2 Gateway Circle','4409584510',4)

UPDATE StudentsSubjects
SET Grade = 6.00
WHERE SubjectId IN (1,2) AND Grade >= 5.50

DELETE FROM StudentsTeachers
WHERE TeacherId IN
(
	SELECT Id 
		FROM Teachers
		WHERE Phone LIKE '%72%'
)

DELETE FROM Teachers
WHERE Phone LIKE '%72%'

SELECT s.FirstName,s.LastName,s.Age
	FROM Students AS s
	WHERE Age >= 12 
	ORDER BY s.FirstName,s.LastName

SELECT s.FirstName,s.LastName, COUNT(s.Id) AS [TeachersCount]
	FROM Students AS s
	JOIN StudentsTeachers AS st ON s.Id = st.StudentId
	GROUP BY s.FirstName,s.LastName

SELECT CONCAT(s.FirstName, ' ', s.LastName) AS [Full Name]
	FROM Students AS s
	LEFT JOIN StudentsExams AS se ON s.Id = se.StudentId
	WHERE se.ExamId IS NULL
	ORDER BY [Full Name]

SELECT TOP(10) s.FirstName,s.LastName, CAST(ROUND(AVG(se.Grade),2) AS DECIMAL(18,2)) AS [Grade]
	FROM Students AS s
	JOIN StudentsExams AS se ON s.Id = se.StudentId
	GROUP BY s.FirstName,s.LastName
	ORDER BY [Grade] DESC, s.FirstName, s.LastName

SELECT CONCAT(COALESCE(s.FirstName + ' ',''),COALESCE(s.MiddleName + ' ',''),COALESCE(s.LastName + ' ','')) AS [Full Name]
	FROM Students AS s
	LEFT JOIN StudentsSubjects AS ss ON s.Id = ss.StudentId
	WHERE ss.SubjectId IS NULL
	ORDER BY [Full Name]

SELECT s.[Name], AVG(ss.Grade) AS [AverageGrade]
	FROM Subjects AS s
	JOIN StudentsSubjects AS ss ON s.Id = ss.SubjectId
	GROUP BY s.[Name],s.Id
	ORDER BY s.Id

	--last 2 tasks
	--programability