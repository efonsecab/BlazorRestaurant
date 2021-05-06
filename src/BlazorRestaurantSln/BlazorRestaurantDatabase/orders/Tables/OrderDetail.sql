CREATE TABLE [orders].[OrderDetail]
(
	[OrderDetailId] BIGINT NOT NULL CONSTRAINT PK_OrderDetail PRIMARY KEY IDENTITY, 
    [OrderId] BIGINT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [ProductQty] INT NOT NULL, 
    [LineNumber] INT NOT NULL, 
    [LineTotal] MONEY NOT NULL,
    [RowCreationDateTime] DATETIMEOFFSET NOT NULL, 
    [RowCreationUser] NVARCHAR(256) NOT NULL,
    [SourceApplication] NVARCHAR(250) NOT NULL, 
    [OriginatorIPAddress] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY ([OrderId]) REFERENCES [orders].[Order]([OrderId]), 
    CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [products].[Product]([ProductId])
)

GO

CREATE UNIQUE INDEX [UI_OrderDetail_Product] ON [orders].[OrderDetail] ([OrderId], [ProductId])

GO

CREATE UNIQUE INDEX [UI_OrderDetail_Line] ON [orders].[OrderDetail] ([OrderId], [LineNumber])
