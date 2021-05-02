CREATE TABLE [products].[ProductType]
(
	[ProductTypeId] SMALLINT NOT NULL CONSTRAINT PK_ProductType PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL
)
