CREATE TABLE [orders].[Order]
(
	[OrderId] BIGINT NOT NULL CONSTRAINT PK_Order PRIMARY KEY IDENTITY, 
    [DestinationFreeFormAddress] NVARCHAR(1000) NOT NULL, 
    [DestinationLatitude] FLOAT NOT NULL, 
    [DestinationLongitude] FLOAT NOT NULL,
    [Total] MONEY NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL
)
