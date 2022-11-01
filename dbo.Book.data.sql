INSERT INTO [dbo].[Condition] ([ConditionName]) VALUES (N'New')
INSERT INTO [dbo].[Genre] ([Name]) VALUES (N'Horror')
INSERT INTO [dbo].[Publisher] ([Name]) VALUES (N'Penguin')
INSERT INTO [dbo].[Type] ([TypeName]) VALUES (N'Paperback')

INSERT INTO [dbo].[Book] ([Name], [Author], [Year], [Publisher_Id], [ISBN], [Type_Id], [Genre_Id], [FrontUrl], [BackUrl], [LeftUrl], [RightUrl], [AudioBookUrl], [Summary], [CreatedBy], [CreatedOn], [UpdatedOn]) VALUES (N'Clean Code: A Handbook of Agile Software Craftsmanship', N'Robert C. Martin', 2009, 1, N'9780132350884', 1, 1, N'1', N'1', N'1', N'1', N'1', N'Even bad code can function. But if code isn’t clean, it can bring a development organization to its knees. Every year, countless hours and significant resources are lost because of poorly written code. But it doesn’t have to be that way. ', N'admin user', N'2022-10-28 00:00:00', N'2022-10-28 00:00:00')