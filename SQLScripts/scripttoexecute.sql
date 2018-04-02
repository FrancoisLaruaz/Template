CREATE TABLE [dbo].[Search](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Pattern] [nvarchar](255) NULL,
	[Date] Datetime not null,
	[UserId] [int] NOT NULL
 CONSTRAINT [PK_Search_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Search]  WITH CHECK ADD  CONSTRAINT [FK_Search_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[Search] CHECK CONSTRAINT [FK_Search_UserId]
GO


CREATE TABLE [dbo].[SearchResult](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](255) NULL,
	[Date] Datetime not null,
	[SearchId] [int] NOT NULL
 CONSTRAINT [PK_SearchResult_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SearchResult]  WITH CHECK ADD  CONSTRAINT [FK_SearchResult_Search] FOREIGN KEY([SearchId])
REFERENCES [dbo].[Search] ([Id])
GO

ALTER TABLE [dbo].[SearchResult] CHECK CONSTRAINT [FK_SearchResult_Search]
GO

alter table dbo.Search
add ResultsNumber int not null

alter table dbo.search
alter column UserId int null




ALTER procedure  [dbo].[DeleteUserById]
        @UserId integer
        AS
        BEGIN
    SET NOCOUNT ON;  
	SET XACT_ABORT ON; --> the only change
    declare @trancount int;
	declare @UserName varchar(256)
	declare @AspNetUserId varchar(256)

    set @trancount = @@trancount;
    begin try
	print 'BEGIN : '+cast(@UserId as varchar(200))
			BEGIN TRAN  
				set @UserName=(select top 1 UserName from dbo.AspNetUsers where id= (select top 1 AspNetUserId from dbo.[user] where id=@UserId))
				set @AspNetUserId=(select top 1 AspNetUserId from dbo.[user] where id=@UserId)

				delete from dbo.scheduledtask where UserId=@UserId;
				update dbo.log4net set UserLogin=null where UserLogin=@UserName
				update dbo.news set LastModificationUserId=null where LastModificationUserId=@UserId;
				update dbo.Search set UserId=null where UserId=@UserId	
				delete from dbo.emailaudit where UserId=@UserId		
				delete from dbo.[user] where Id=@UserId;	
				delete from dbo.AspNetUserClaims where UserId=@AspNetUserId;
				delete from dbo.AspNetUserRoles where UserId=@AspNetUserId;
				delete from dbo.socialmediaconnection where ProviderKeyUserFriend in  (select providerKey from dbo.AspNetUserLogins where UserId=@AspNetUserId);
				delete from dbo.socialmediaconnection where ProviderKeyUserSignedUp in  (select providerKey from dbo.AspNetUserLogins where UserId=@AspNetUserId)
				delete from dbo.AspNetUserLogins where UserId=@AspNetUserId;
				delete from dbo.AspNetUsers where Id=@AspNetUserId;
				

		   COMMIT TRAN
		   print 'END : '+cast(@UserId as varchar(200))+' : COMMIT'
		   return 1
    end try
    begin catch
		IF @@TRANCOUNT > 0 ROLLBACK TRAN   

		 print 'END : '+cast(@UserId as varchar(200))+' : ERROR  : rollback => '+ERROR_MESSAGE()
			INSERT INTO [dbo].[Log4Net]
           ([Date]
           ,[Thread]
           ,[Level]
           ,[Logger]
           ,[Message]
           ,[Exception]
           ,[UserLogin])
     VALUES
           (getdate()
           ,0
           ,'ERROR'
           ,'STORED PROCEDURE'
           ,'Error in [dbo].[DeleteUserById] stored procedure. @UserId = '+cast(@UserId as varchar(250))+' and @trancount = '+cast(@trancount as varchar(250))
           , ERROR_MESSAGE()
           ,'')

		   return 0
    end catch
end



