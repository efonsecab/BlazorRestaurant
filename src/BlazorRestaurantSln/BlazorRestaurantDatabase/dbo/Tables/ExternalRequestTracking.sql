CREATE TABLE [dbo].[ExternalRequestTracking]
(
	[ExternalRequestTrackingId] BIGINT NOT NULL CONSTRAINT PK_RequestTracking PRIMARY KEY IDENTITY, 
    [RequestContentHeaders] NVARCHAR(MAX) NULL, 
    [RequestContentString] NVARCHAR(MAX) NULL, 
    [RequestHeaders] NVARCHAR(MAX) NOT NULL, 
    [RequestMethod] NVARCHAR(50) NOT NULL, 
    [RequestUrl] NVARCHAR(MAX) NOT NULL, 
    [ResponseContentHeaders] NVARCHAR(MAX) NOT NULL, 
    [ResponseContentString] NVARCHAR(MAX) NULL, 
    [ResponseReasonPhrase] NVARCHAR(MAX) NULL, 
    [ResponseStatusCode] INT NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL
)
