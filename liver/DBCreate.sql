USE master
IF NOT EXISTS(select * from sys.databases where name='InoDemo1')
CREATE DATABASE InoDemo1
GO

USE InoDemo1
GO

DROP TABLE IF EXISTS dbo.[MiningDigs]
DROP TABLE IF EXISTS dbo.[DifficultySetting]
GO

CREATE TABLE [dbo].[MiningDigs]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Client NVARCHAR(60) NOT NULL,
	CoinsMined DECIMAL(9,3) NOT NULL DEFAULT(0),
	MillisecondTaken DECIMAL(9,3) DEFAULT(0),
	DigDate datetime default(getdate())
)
CREATE TABLE [dbo].[DifficultySetting]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	DifficultyLevel int NOT NULL
)
INSERT [DifficultySetting] VALUES (5)