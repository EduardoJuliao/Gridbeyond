CREATE DATABASE GridBeyondMarketAnalysis
GO
USE GridBeyondMarketAnalysis
GO
CREATE TABLE MarketData
(
   [Id] INT NOT NULL IDENTITY(1,1),
   [Date] DATETIME NOT NULL,
   [MarketValueEX1] DECIMAL(18,10) NOT NULL
)
GO
CREATE TABLE ProcessHistory
(
   [Id] INT NOT NULL IDENTITY(1,1),
   [ProcessDate] DATETIME NOT NULL,
   [TotalRecords] INT NOT NULL,
   [ValidRecords] INT NOT NULL,
   [MalformedRecords] INT NOT NULL,
   [NewRecords] INT NOT NULL
)