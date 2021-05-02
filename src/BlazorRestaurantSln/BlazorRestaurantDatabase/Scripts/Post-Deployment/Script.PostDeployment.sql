/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
IF NOT EXISTS( SELECT * FROM dbo.ApplicationRole AR WHERE AR.[Name] = 'Admin')
BEGIN
    INSERT INTO dbo.ApplicationRole([Name],[Description]) VALUES('Admin', 'Members of this role are allowed to perform administration tasks')
END
IF NOT EXISTS( SELECT * FROM dbo.ApplicationRole AR WHERE AR.[Name] = 'User')
BEGIN
    INSERT INTO dbo.ApplicationRole([Name], [Description]) VALUES('User', 'Memebers of this group are limited to user basic functionality')
END


IF NOT EXISTS (SELECT * FROM [products].[ProductType])
BEGIN
	INSERT INTO [products].[ProductType]([Name],[Description]) VALUES ('Pizza', 'Our collections of pizza')
	INSERT INTO [products].[ProductType]([Name],[Description]) VALUES ('Beverages', 'Our menu of selected beverages')
	INSERT INTO [products].[ProductType]([Name],[Description]) VALUES ('Fish', 'Our seafood dishes')
END
