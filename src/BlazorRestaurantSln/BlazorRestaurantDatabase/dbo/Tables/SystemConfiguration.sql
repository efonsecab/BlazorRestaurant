CREATE TABLE [dbo].[SystemConfiguration]
(
	[SystemConfigurationId] INT NOT NULL CONSTRAINT PK_SystemConfiguration PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Value] NVARCHAR(MAX) NOT NULL
)

GO

CREATE UNIQUE INDEX [UI_SystemConfiguration_Name] ON [dbo].[SystemConfiguration] ([Name])
