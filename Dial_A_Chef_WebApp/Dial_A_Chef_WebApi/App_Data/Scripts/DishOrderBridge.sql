CREATE TABLE [dbo].[DishOrderBridge]
(
	[DO_Id] INT IDENTITY(1,1) NOT NULL,
	[Dish_Id] INT NOT NULL,
	[Order_Id] INT NOT NULL,

	PRIMARY KEY([DO_Id] ASC),
	UNIQUE NONCLUSTERED ([DO_Id] ASC),
    FOREIGN KEY ([Order_ID]) REFERENCES [dbo].[Order] ([O_ID]),
    FOREIGN KEY ([Dish_ID]) REFERENCES [dbo].[Dish] ([M_Id])

)
