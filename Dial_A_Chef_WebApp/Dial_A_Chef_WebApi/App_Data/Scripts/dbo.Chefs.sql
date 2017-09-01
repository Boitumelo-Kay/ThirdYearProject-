CREATE TABLE [dbo].[Chefs] (
    [Ch_ID]     INT           NOT NULL,
    [Ch_Type]   VARCHAR (MAX) NOT NULL,
    [Ch_Rating] INT           NOT NULL,
    [Ch_Bio]    VARCHAR (MAX) NULL,
	[Ch_Points] INT NULL

    PRIMARY KEY CLUSTERED ([Ch_ID] ASC),
    UNIQUE NONCLUSTERED ([Ch_ID] ASC),
    FOREIGN KEY ([Ch_ID]) REFERENCES [dbo].[Account] ([U_ID])
);

