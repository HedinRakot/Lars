USE [MyTems]
GO

/****** Object:  Table [dbo].[Orders]    Script Date: 05.10.2023 13:55:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Orders](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[Address] [nvarchar](30) NOT NULL,
	[City] [nvarchar](20) NOT NULL,
	[State] [nvarchar](20) NOT NULL,
	[PostalCode] [nvarchar](10) NOT NULL,
	[Country] [nvarchar](20) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](20) NOT NULL,
	[Total] [decimal](18, 0) NULL,
	[Date] [datetimeoffset](7) NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

