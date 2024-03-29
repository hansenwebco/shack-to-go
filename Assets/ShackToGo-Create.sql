SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ShackUsers](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[LastDbPull] [datetime] NOT NULL,
	[Password] [varchar](50) NULL,
	[EnableThreadedView] [bit] NOT NULL CONSTRAINT [DF_ShackUsers_EnableThreadedView]  DEFAULT (0),
	[EnableThreadTextDisplay] [bit] NOT NULL CONSTRAINT [DF_ShackUsers_EnableThreadTextDisplay]  DEFAULT (0),
	[TimeAdjustment] [int] NOT NULL CONSTRAINT [DF_ShackUsers_TimeAdjustment]  DEFAULT (0),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BookMarks](
	[BookMarkID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ThreadID] [varchar](50) NOT NULL,
	[StoryID] [varchar](50) NOT NULL,
	[Desc] [ntext] NOT NULL,
	[PosterName] [varchar](50) NOT NULL,
	[PostCreated] [varchar](50) NOT NULL,
	[ReplyCount] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_BookMarks_Deleted]  DEFAULT (0),
 CONSTRAINT [PK_BookMarks] PRIMARY KEY CLUSTERED 
(
	[BookMarkID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [BookMarks]  WITH NOCHECK ADD  CONSTRAINT [FK_BookMarks_ShackUsers] FOREIGN KEY([UserID])
REFERENCES [ShackUsers] ([UserID])
GO
ALTER TABLE [BookMarks] CHECK CONSTRAINT [FK_BookMarks_ShackUsers]
