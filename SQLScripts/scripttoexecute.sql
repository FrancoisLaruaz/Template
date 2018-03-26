USE [Template]
GO

INSERT INTO [dbo].[AspNetRoles]
           ([Id]
           ,[Name])
     VALUES
           ('1'
           ,'Admin')
GO



INSERT INTO [dbo].[AspNetUserRoles]
           ([UserId]
           ,[RoleId])
select id,1 from dbo.AspNetUsers
