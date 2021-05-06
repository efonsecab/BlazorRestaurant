CREATE TABLE [profile].[Location]
(
	[LocationId] INT NOT NULL CONSTRAINT PK_Location PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(150) NOT NULL, 
    [Description] NVARCHAR(1000) NOT NULL, 
    [FreeFormAddress] NVARCHAR(1000) NOT NULL, 
    [Latitude] FLOAT NOT NULL, 
    [Longitude] FLOAT NOT NULL, 
    [ImageUrl] NVARCHAR(1000) NOT NULL, 
    [IsDefault] BIT NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL 
)

GO

CREATE UNIQUE INDEX [UI_Location_Name] ON [profile].[Location] ([Name])
