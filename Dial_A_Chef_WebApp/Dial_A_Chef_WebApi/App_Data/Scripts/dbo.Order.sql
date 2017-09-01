CREATE TABLE [dbo].[Order] (
    [O_Id]       INT      IDENTITY (1, 1) NOT NULL,
    [O_Date]     DATETIME NOT NULL,
    [Dish_ID]    INT      NOT NULL,
    [Cust_ID]    INT      NOT NULL,
    [Chef_ID]    INT      NULL,
	[Delivery_Method] VARCHAR(20) NULL,
    [IsAccepted] BIT      NOT NULL,
    [SeenByChef] BIT      NULL,
    PRIMARY KEY CLUSTERED ([O_Id] ASC),
    UNIQUE NONCLUSTERED ([O_Id] ASC),
    FOREIGN KEY ([Cust_ID]) REFERENCES [dbo].[Customers] ([Cu_ID]),
    FOREIGN KEY ([Dish_ID]) REFERENCES [dbo].[Dish] ([M_Id]),
    FOREIGN KEY ([Chef_ID]) REFERENCES [dbo].[Chefs] ([Ch_ID])
);

