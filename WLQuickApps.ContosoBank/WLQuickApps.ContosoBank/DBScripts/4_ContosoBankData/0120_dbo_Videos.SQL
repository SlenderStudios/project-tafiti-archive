
-- Delete all records first
DELETE FROM [dbo].[Videos]
GO

SET IDENTITY_INSERT [dbo].[Videos] ON


INSERT INTO [dbo].[Videos] ([VideoId], [VideoTitle], [VideoURL], [NumViews], [Rating], [FrameImage], [UploadDate])
VALUES (1, N'Interview 1', N'streaming:/56451/Interview01a/interview 01.wmv', 5, 3, N'/thumbnails/interview 01_Thumb.jpg', '20080401')

INSERT INTO [dbo].[Videos] ([VideoId], [VideoTitle], [VideoURL], [NumViews], [Rating], [FrameImage], [UploadDate])
VALUES (3, N'Interview 2', N'streaming:/56451/Interview 02/interview 02.wmv', 15, 4, N'/thumbnails/interview 02_Thumb.jpg', '20080212')

INSERT INTO [dbo].[Videos] ([VideoId], [VideoTitle], [VideoURL], [NumViews], [Rating], [FrameImage], [UploadDate])
VALUES (4, N'Interview 2', N'streaming:/56451/Interview 03/interview 03.wmv', 111, 2, N'/thumbnails/interview 03_Thumb.jpg', '20080322')

INSERT INTO [dbo].[Videos] ([VideoId], [VideoTitle], [VideoURL], [NumViews], [Rating], [FrameImage], [UploadDate])
VALUES (8, N'Interview 4', N'streaming:/56451/Interview 04/interview 04.wmv', 11, 1, N'/thumbnails/interview 04_Thumb.jpg', '20080324')
SET IDENTITY_INSERT [dbo].[Videos] OFF

