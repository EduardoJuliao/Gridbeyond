CREATE DATABASE GridBeyondMarketAnalysis

USE GridBeyondMarketAnalysis

CREATE TABLE MarketData
(
   [Id] INT NOT NULL IDENTITY(1,1),
   [Date] DATETIME NOT NULL,
   [MarketValueEX1] DECIMAL(18,10) NOT NULL
)
