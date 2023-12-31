USE [ScheduleMaster]
GO

-- [Users]
----------

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- [EventData]
--------------

CREATE TABLE [dbo].[EventData](
	[EventDataId] [nvarchar](256) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Day] [nvarchar](25) NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Info] [nvarchar](256) NULL,
	[Start] [nvarchar](256) NOT NULL,
	[End] [nvarchar](256) NOT NULL,
	[Colour] [nvarchar](256) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EventData]  WITH CHECK ADD  CONSTRAINT [FK_EventData_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[EventData] CHECK CONSTRAINT [FK_EventData_AspNetUsers]
GO