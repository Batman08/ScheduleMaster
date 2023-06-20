USE [ScheduleMaster]
GO

-- [Users]
----------

CREATE TABLE [dbo].[Users](
	[UserId] [nvarchar](256) NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- [EventData]
--------------

CREATE TABLE [dbo].[EventData](
	[EventDataId] [nvarchar](256) NOT NULL,
	[UserId] [nvarchar](256) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Info] [nvarchar](256) NULL,
	[Start] [nvarchar](256) NOT NULL,
	[End] [nvarchar](256) NOT NULL,
	[Colour] [nvarchar](256) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EventData]  WITH CHECK ADD  CONSTRAINT [FK_EventData_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[EventData] CHECK CONSTRAINT [FK_EventData_Users]
GO
