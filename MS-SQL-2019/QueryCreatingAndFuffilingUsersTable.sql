CREATE TABLE Users
(
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL,
  	[Password] VARCHAR(26) NOT NULL,
  	ProfilePicture VARBINARY(900),
	LastLoginTime DATE,
	IsDeleted BIT
)

INSERT INTO Users(Username,[Password],ProfilePicture,LastLoginTime,IsDeleted) VALUES
('Atanas','random123',800, '2008-11-11',0),
('Stoqn','random122',100, '2008-11-11',1),
('Evgeni','random121',300, '2008-11-11',1),
('Kris','random1234',500, '2008-11-11',0),
('Ivaylo','random120',700, '2008-11-11',1)
