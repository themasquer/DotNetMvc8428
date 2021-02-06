USE [MoviesMvc8428DB]
GO

delete from MovieDirectors
delete from Reviews
delete from Movies
delete from Directors
delete from AspNetUsers

SET IDENTITY_INSERT [dbo].[Directors] ON 
GO
INSERT [dbo].[Directors] ([Id], [Name], [Surname], [Retired]) VALUES (13, N'James', N'Cameron', 0)
GO
INSERT [dbo].[Directors] ([Id], [Name], [Surname], [Retired]) VALUES (14, N'Guy', N'Ritchie', 0)
GO
INSERT [dbo].[Directors] ([Id], [Name], [Surname], [Retired]) VALUES (15, N'F. Gary', N'Gray', 0)
GO
SET IDENTITY_INSERT [dbo].[Directors] OFF
GO
SET IDENTITY_INSERT [dbo].[Movies] ON 
GO
INSERT [dbo].[Movies] ([Id], [Name], [ProductionYear], [BoxOfficeReturn]) VALUES (17, N'Avatar', N'2009', 1000000)
GO
INSERT [dbo].[Movies] ([Id], [Name], [ProductionYear], [BoxOfficeReturn]) VALUES (18, N'Sherlock Holmes', N'2009', NULL)
GO
INSERT [dbo].[Movies] ([Id], [Name], [ProductionYear], [BoxOfficeReturn]) VALUES (19, N'Law Abiding Citizen', N'2009', 300000)
GO
INSERT [dbo].[Movies] ([Id], [Name], [ProductionYear], [BoxOfficeReturn]) VALUES (20, N'Aliens', N'1986', 10000000)
GO
SET IDENTITY_INSERT [dbo].[Movies] OFF
GO
SET IDENTITY_INSERT [dbo].[MovieDirectors] ON 
GO
INSERT [dbo].[MovieDirectors] ([Id], [MovieId], [DirectorId]) VALUES (17, 17, 13)
GO
INSERT [dbo].[MovieDirectors] ([Id], [MovieId], [DirectorId]) VALUES (18, 18, 14)
GO
INSERT [dbo].[MovieDirectors] ([Id], [MovieId], [DirectorId]) VALUES (19, 19, 15)
GO
INSERT [dbo].[MovieDirectors] ([Id], [MovieId], [DirectorId]) VALUES (20, 20, 13)
GO
SET IDENTITY_INSERT [dbo].[MovieDirectors] OFF
GO
SET IDENTITY_INSERT [dbo].[Reviews] ON 
GO
INSERT [dbo].[Reviews] ([Id], [Content], [Rating], [Reviewer], [MovieId]) VALUES (5, N'Very good movie.', 9, N'Çağıl Alsaç', 17)
GO
SET IDENTITY_INSERT [dbo].[Reviews] OFF
GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'89642074-3b31-4b8d-86a7-7ac591694820', N'cagil@alsac.com', 0, N'APuwHTOUWYJamo4YgNTS1ih9v672VS8pvpRXji8BVd3ea8cGmhZrLooUxkR6p9Ybew==', N'0c6af012-9f56-461c-9910-1161394c68e9', NULL, 0, 0, NULL, 1, 0, N'cagil@alsac.com')
GO