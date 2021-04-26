CREATE TABLE [promo].[Promotion]
(
	[PromotionId] BIGINT NOT NULL CONSTRAINT PK_Promotion PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(250) NOT NULL, 
    [ImageUrl] NVARCHAR(1000) NOT NULL
)
