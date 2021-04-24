﻿CREATE TABLE [dbo].[ApplicationUser] (
    [ApplicationUserId] BIGINT NOT NULL IDENTITY CONSTRAINT PK_ApplicationUser PRIMARY KEY, 
    [Full Name] NVARCHAR(150) NOT NULL, 
    [EmailAddress] NVARCHAR(150) NOT NULL, 
    [LastLogIn] DATETIMEOFFSET NOT NULL, 
    [AzureAdB2CObjectId] UNIQUEIDENTIFIER NOT NULL
);


GO

CREATE UNIQUE INDEX [UI_ApplicationUser_AzureAdB2CObjectId] ON [dbo].[ApplicationUser] ([AzureAdB2CObjectId])
