CREATE TABLE [dbo].[Country]
(
	[CountryId] BIGINT NOT NULL CONSTRAINT PK_Country PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[ISOCode] NVARCHAR(4) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_Country_Name] ON [dbo].[Country] ([Name])

GO

CREATE UNIQUE INDEX [UI_Country_ISOCode] ON [dbo].[Country] ([ISOCode])
