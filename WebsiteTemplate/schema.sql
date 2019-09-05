USE [master]
GO
/****** Object:  Database [WebsiteDB]    Script Date: 12/06/2019 15:46:33 ******/
CREATE DATABASE [WebsiteDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebsiteDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\WebsiteDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WebsiteDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\WebsiteDB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [WebsiteDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebsiteDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebsiteDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebsiteDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebsiteDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebsiteDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebsiteDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebsiteDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WebsiteDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebsiteDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebsiteDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebsiteDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebsiteDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebsiteDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebsiteDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebsiteDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebsiteDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [WebsiteDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebsiteDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebsiteDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebsiteDB] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [WebsiteDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebsiteDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [WebsiteDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebsiteDB] SET RECOVERY FULL 
GO
ALTER DATABASE [WebsiteDB] SET  MULTI_USER 
GO
ALTER DATABASE [WebsiteDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebsiteDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebsiteDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebsiteDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WebsiteDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'WebsiteDB', N'ON'
GO
ALTER DATABASE [WebsiteDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [WebsiteDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
USE [WebsiteDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [WebsiteDB]
GO
/****** Object:  User [dbAdminUser]    Script Date: 12/06/2019 15:46:33 ******/
CREATE USER [dbAdminUser] FOR LOGIN [dbAdminLogin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [dbAdminUser]
GO
/****** Object:  UserDefinedTableType [dbo].[UrlList]    Script Date: 12/06/2019 15:46:33 ******/
CREATE TYPE [dbo].[UrlList] AS TABLE(
	[Url] [nvarchar](450) NOT NULL
)
GO
/****** Object:  Table [dbo].[Log]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Type] [char](1) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoginAttempts]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginAttempts](
	[Id] [int] IDENTITY(1200,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[IpAddress] [nchar](24) NOT NULL,
	[UserAgent] [nvarchar](256) NULL,
 CONSTRAINT [PK_LoginAttempts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleClaims]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_RoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[NormalizedName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_Roles_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserClaims]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLogins]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [int] NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[NormalizedEmail] [nvarchar](256) NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Options] [nvarchar](max) NULL,
	[ApiKey] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Users_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersRoles]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UsersRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTokens]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTokens](
	[UserId] [int] NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_RoleClaims_RoleId]    Script Date: 12/06/2019 15:46:33 ******/
CREATE NONCLUSTERED INDEX [IX_RoleClaims_RoleId] ON [dbo].[RoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Roles_Name]    Script Date: 12/06/2019 15:46:33 ******/
CREATE NONCLUSTERED INDEX [IX_Roles_Name] ON [dbo].[Roles]
(
	[NormalizedName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserClaims_UserId]    Script Date: 12/06/2019 15:46:33 ******/
CREATE NONCLUSTERED INDEX [IX_UserClaims_UserId] ON [dbo].[UserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_ApiKey]    Script Date: 12/06/2019 15:46:33 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_ApiKey] ON [dbo].[Users]
(
	[ApiKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_NormalizedEmail]    Script Date: 12/06/2019 15:46:33 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_NormalizedEmail] ON [dbo].[Users]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_NormalizedUserName]    Script Date: 12/06/2019 15:46:33 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_NormalizedUserName] ON [dbo].[Users]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Log] ADD  DEFAULT (getutcdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Log] ADD  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[LoginAttempts] ADD  CONSTRAINT [DF__LoginAttempt__Times__47DBAE45]  DEFAULT (getutcdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF__Roles__Timestamp__5CD6CB2B]  DEFAULT (getutcdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__tmp_ms_xx__Times__29221CFB]  DEFAULT (getutcdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__tmp_ms_xx__Email__2A164134]  DEFAULT ((0)) FOR [EmailConfirmed]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_PhoneNumberConfirmed]  DEFAULT ((0)) FOR [PhoneNumberConfirmed]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_TwoFactorEnabled]  DEFAULT ((0)) FOR [TwoFactorEnabled]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_LockoutEnabled]  DEFAULT ((0)) FOR [LockoutEnabled]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_AccessFailedCount]  DEFAULT ((0)) FOR [AccessFailedCount]
GO
ALTER TABLE [dbo].[LoginAttempts]  WITH CHECK ADD  CONSTRAINT [fk_LoginAttempts_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoginAttempts] CHECK CONSTRAINT [fk_LoginAttempts_Users]
GO
ALTER TABLE [dbo].[RoleClaims]  WITH CHECK ADD  CONSTRAINT [fk_RoleClaims_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleClaims] CHECK CONSTRAINT [fk_RoleClaims_Roles]
GO
ALTER TABLE [dbo].[UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_UserClaims_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserClaims] CHECK CONSTRAINT [FK_UserClaims_Users]
GO
ALTER TABLE [dbo].[UserLogins]  WITH CHECK ADD  CONSTRAINT [fk_UserLogins_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserLogins] CHECK CONSTRAINT [fk_UserLogins_Users]
GO
ALTER TABLE [dbo].[UsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsersRoles] CHECK CONSTRAINT [FK_UsersRoles_Roles]
GO
ALTER TABLE [dbo].[UsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsersRoles] CHECK CONSTRAINT [FK_UsersRoles_Users]
GO
ALTER TABLE [dbo].[UserTokens]  WITH CHECK ADD  CONSTRAINT [FK_UserTokens_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserTokens] CHECK CONSTRAINT [FK_UserTokens_Users]
GO
/****** Object:  StoredProcedure [dbo].[AddLog]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddLog]
	@type char(1),
	@message nvarchar(max)
AS
	INSERT INTO Log (Type, Message) VALUES (@type, @message);
GO
/****** Object:  StoredProcedure [dbo].[AddLoginAttempt]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddLoginAttempt] 
	@userId INT, 
	@ipAddress NCHAR(24),
	@userAgent NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO LoginAttempts (UserId, IpAddress, UserAgent) VALUES(@userId, @ipAddress, @userAgent);

END
GO
/****** Object:  StoredProcedure [dbo].[AddRole]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRole]
	@name NVARCHAR(256),
	@normalizedName NVARCHAR(256)
AS

	INSERT INTO Roles (Name, NormalizedName) VALUES (@name, @normalizedName);
	SELECT scope_identity();
GO
/****** Object:  StoredProcedure [dbo].[AddRoleClaim]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRoleClaim]
@roleId INT,
@claimType NVARCHAR(max),
@claimValue NVARCHAR(max)
AS
BEGIN
INSERT INTO RoleClaims (RoleId, ClaimType, ClaimValue)
VALUES (@roleId, @claimType, @claimValue);
END
GO
/****** Object:  StoredProcedure [dbo].[AddToRole]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddToRole]
	@userId INT,
	@roleName NVARCHAR(256)
AS

	DECLARE @roleId INT = (SELECT r.Id FROM Roles r WHERE r.NormalizedName = @roleName);
	INSERT INTO UsersRoles (UserId, RoleId) VALUES(@userId, @roleId);
GO
/****** Object:  StoredProcedure [dbo].[AddUser]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddUser]
	@email NVARCHAR(256),
	@normalizedEmail NVARCHAR(256),
	@passwordHash NVARCHAR(MAX),
	@securityStamp NVARCHAR(MAX) = NULL,
	
	@twoFactorEnabled BIT,
	@userName NVARCHAR(256),
	@normalizedUserName NVARCHAR(256),
	@phoneNumber NVARCHAR(MAX),
	@phoneNumberConfirmed BIT,
	@lockoutEnabled BIT,
	
	@apiKey NVARCHAR(50),
	@options NVARCHAR(max) = NULL

AS

	INSERT INTO Users (Email, NormalizedEmail, PasswordHash, SecurityStamp, TwoFactorEnabled, UserName, NormalizedUserName, PhoneNumber, PhoneNumberConfirmed, LockoutEnabled, ApiKey, Options) 
	VALUES (@email, @normalizedEmail, @passwordHash, @securityStamp, @twoFactorEnabled, @userName, @normalizedUserName, @phoneNumber, @phoneNumberConfirmed, @lockoutEnabled, @apiKey, @options);
	SELECT scope_identity();
GO
/****** Object:  StoredProcedure [dbo].[AddUserClaim]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddUserClaim]
@userId INT,
@claimType NVARCHAR(max),
@claimValue NVARCHAR(max)
AS
BEGIN
INSERT INTO UserClaims (UserId, ClaimType, ClaimValue)
VALUES (@userId, @claimType, @claimValue);
END
GO
/****** Object:  StoredProcedure [dbo].[AddUserLogin]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddUserLogin]
@loginProvider NVARCHAR(128),
@providerKey NVARCHAR(128),
@userId INT,
@providerDisplayName NVARCHAR(max)
AS
BEGIN
INSERT INTO UserLogins (LoginProvider, ProviderKey, UserId, ProviderDisplayName)
VALUES (@loginProvider, @providerKey, @userId, @providerDisplayName);
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteOldLogs]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteOldLogs]
	@minTime DATETIME = NULL
AS

IF(@minTime IS NULL)
	TRUNCATE TABLE Log;
ELSE
	DELETE FROM Log WHERE Timestamp < @minTime;
GO
/****** Object:  StoredProcedure [dbo].[DeleteRole]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRole]
@id INT
AS
BEGIN
DELETE FROM Roles WHERE Id=@id;
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteRoleClaim]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[DeleteRoleClaim]
@roleId INT,
@claimType NVARCHAR(max),
@claimValue NVARCHAR(max)
AS
BEGIN
DELETE FROM RoleClaims WHERE RoleId=@roleId AND ClaimType=@claimType AND ClaimValue=@claimValue;
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserClaim]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUserClaim]
@userId INT,
@claimType NVARCHAR(max),
@claimValue NVARCHAR(max)
AS
BEGIN
DELETE FROM UserClaims WHERE UserId=@userId AND ClaimType=@claimType AND ClaimValue=@claimValue;
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserLogin]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUserLogin]
@userId INT,
@loginProvider NVARCHAR(200),
@providerKey NVARCHAR(200)
AS
BEGIN
DELETE FROM UserLogins WHERE UserId=@userId AND LoginProvider=@loginProvider AND ProviderKey=@providerKey;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAdminStats]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAdminStats]
	@maxAge DATETIME,
	@logItems INT
AS
	
	SELECT
	(SELECT COUNT(*) FROM Users) AS UsersCount,
	(SELECT COUNT(*) FROM LoginAttempts) AS LoginAttemptCount,
				
	(SELECT COUNT(*) FROM Log) AS LogCount,

	(SELECT COUNT(*) FROM Users WHERE Timestamp > @maxAge) AS UsersNewCount

	--Get log items
	SELECT TOP(@logItems) * FROM Log ORDER BY Timestamp DESC;
GO
/****** Object:  StoredProcedure [dbo].[GetLoginAttempts]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetLoginAttempts] 
	@userId INT, 
	@count INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(@count) * FROM LoginAttempts WHERE UserId=@userId ORDER BY Timestamp DESC;

END
GO
/****** Object:  StoredProcedure [dbo].[GetRole]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRole]
@id INT
AS
BEGIN
SELECT * FROM Roles WHERE Id=@id
END
GO
/****** Object:  StoredProcedure [dbo].[GetRoleByName]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetRoleByName]
(
    @normalizedName NVARCHAR(256)
)
AS
BEGIN
    SET NOCOUNT ON

	SELECT r.* FROM Roles r WHERE r.NormalizedName=@normalizedName;
END
GO
/****** Object:  StoredProcedure [dbo].[GetRoleClaims]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[GetRoleClaims]
@roleid INT
AS
BEGIN
SELECT * FROM RoleClaims WHERE RoleId=@roleid
END
GO
/****** Object:  StoredProcedure [dbo].[GetRoles]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoles]
	@userId INT
AS

	SELECT r.* FROM Roles r
	JOIN UsersRoles ur ON ur.RoleId = r.Id
	JOIN Users u ON ur.UserId = u.Id
	WHERE u.Id = @userId;
GO
/****** Object:  StoredProcedure [dbo].[GetUnreadLogs]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUnreadLogs]
	@type char(1) = NULL
AS
	--SELECT * FROM LOG WHERE IsRead=0;

	UPDATE Log 
	SET IsRead=1 
	OUTPUT inserted.*
	WHERE IsRead=0 AND (@type IS NULL OR Type=@type);
GO
/****** Object:  StoredProcedure [dbo].[GetUser]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUser]
	@userId int
AS

	SELECT u.* FROM Users u WHERE u.Id=@userId;
GO
/****** Object:  StoredProcedure [dbo].[GetUserAllInfo]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserAllInfo] 
	-- Add the parameters for the stored procedure here
	@maxAge DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT u.*, 
	(SELECT Count(ul.Id) FROM LoginAttempts ul WHERE ul.UserId=u.Id) AS LoginCount
	FROM Users u 
	WHERE u.Timestamp > @maxAge
	ORDER BY u.Timestamp DESC;

END
GO
/****** Object:  StoredProcedure [dbo].[GetUserByApiKey]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserByApiKey]
	@apiKey NVARCHAR(50)
AS

	SELECT u.* FROM Users u WHERE u.ApiKey=@apiKey;
GO
/****** Object:  StoredProcedure [dbo].[GetUserByEmail]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserByEmail]
	@normalizedEmail NVARCHAR(450)
AS
	
	SELECT u.* FROM Users u WHERE u.NormalizedEmail=@normalizedEmail;
GO
/****** Object:  StoredProcedure [dbo].[GetUserByUserName]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[GetUserByUserName]
	@normalizedUserName NVARCHAR(450)
AS
	
	SELECT u.* FROM Users u WHERE u.NormalizedUserName=@normalizedUserName;
GO
/****** Object:  StoredProcedure [dbo].[GetUserClaims]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserClaims]
@userid INT
AS
BEGIN
SELECT * FROM UserClaims WHERE UserId=@userid
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserLogin]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserLogin]
@loginProvider NVARCHAR(200),
@providerKey NVARCHAR(200)
AS
BEGIN
SELECT * FROM UserLogins WHERE LoginProvider=@loginProvider AND ProviderKey=@providerKey
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserLogins]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserLogins]
@userId INT
AS
BEGIN
SELECT * FROM UserLogins WHERE UserId=@userId;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUsersForClaim]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsersForClaim]
	@type NVARCHAR(max),
	@value NVARCHAR(max)
AS	
	SELECT u.* FROM Users u 
	JOIN UserClaims uc ON uc.UserId=u.Id
	WHERE uc.ClaimType=@type AND uc.ClaimValue=@value;
GO
/****** Object:  StoredProcedure [dbo].[GetUsersInRole]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsersInRole]
	@roleName NVARCHAR(256)
AS

	DECLARE @roleId INT = (SELECT r.Id FROM Roles r WHERE r.NormalizedName = @roleName);
	
	SELECT u.* FROM Users u 
	JOIN UsersRoles ur ON ur.UserId=u.Id
	WHERE ur.RoleId = @roleId;
GO
/****** Object:  StoredProcedure [dbo].[GetUserToken]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserToken]
(
    @userId INT,
    @loginProvider NVARCHAR(400),	
    @name NVARCHAR(400)
)
AS
BEGIN
    SET NOCOUNT ON

 	SELECT u.Value FROM UserTokens u WHERE u.UserId=@userId AND u.LoginProvider=@loginProvider AND u.Name=@name;

END
GO
/****** Object:  StoredProcedure [dbo].[IsInRole]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsInRole]
	@userId INT,
	@roleName NVARCHAR(256)
AS

	IF EXISTS(SELECT 1 FROM Roles r
	JOIN UsersRoles ur ON ur.RoleId = r.Id
	JOIN Users u ON ur.UserId = u.Id
	WHERE u.Id = @userId AND r.NormalizedName=@roleName)
	SELECT 1 AS 'result'
	ELSE
	SELECT 0 AS 'result';
GO
/****** Object:  StoredProcedure [dbo].[RemoveFromRole]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveFromRole]
	@userId INT,
	@roleName NVARCHAR(256)
AS

	DECLARE @roleId INT = (SELECT r.Id FROM Roles r WHERE r.NormalizedName = @roleName);
	DELETE FROM UsersRoles WHERE UserId=@userId AND RoleId=@roleId;
GO
/****** Object:  StoredProcedure [dbo].[ReplaceUserClaim]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReplaceUserClaim]
@userId INT,
@claimType NVARCHAR(max),
@claimValue NVARCHAR(max),
@newClaimType NVARCHAR(max),
@newClaimValue NVARCHAR(max)
AS
BEGIN
UPDATE UserClaims SET
ClaimType=@newClaimType,
ClaimValue=@newClaimValue
WHERE UserId=@userId AND ClaimType=@claimType AND ClaimValue=@claimValue
END
GO
/****** Object:  StoredProcedure [dbo].[SetUserToken]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetUserToken]
(
    @userId INT,
    @loginProvider NVARCHAR(128),	
    @name NVARCHAR(128),
    @value NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON

	UPDATE UserTokens SET Value = @value WHERE UserId = @userId AND LoginProvider = @loginProvider AND Name = @name

    IF (@@ROWCOUNT = 0)
	       INSERT INTO UserTokens (UserId, LoginProvider, Name, Value) VALUES (@userId, @loginProvider, @name, @value);

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateRole]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateRole]
	@roleId int,
	@name NVARCHAR(256),
	@nameNormalized NVARCHAR(256)

AS
	UPDATE Roles SET 
	Name=@name, 
	NormalizedName=@nameNormalized
	WHERE Id=@roleId
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUser]
	@userId int,
	@email NVARCHAR(256),
	@normalizedEmail NVARCHAR(256),
	@emailConfirmed BIT,
	@passwordHash NVARCHAR(MAX),
	@securityStamp NVARCHAR(MAX) = NULL,
		
	@twoFactorEnabled BIT,
	@userName NVARCHAR(256),
	@normalizedUserName NVARCHAR(256),
	@phoneNumber NVARCHAR(MAX),
	@phoneNumberConfirmed BIT,
	@lockoutEnd DATETIMEOFFSET(7),
	@lockoutEnabled BIT,
	@accessFailedCount INT

AS
	UPDATE Users SET 
	Email=@email, 
	NormalizedEmail=@normalizedEmail, 
	EmailConfirmed=@emailConfirmed,
	PasswordHash=@passwordHash, 
	SecurityStamp=@securityStamp, 

	TwoFactorEnabled = @twoFactorEnabled,
	UserName = @userName,
	NormalizedUserName = @normalizedUserName,
	PhoneNumber = @phoneNumber,
	PhoneNumberConfirmed = @phoneNumberConfirmed,
	LockoutEnd = @lockoutEnd,
	LockoutEnabled = @lockoutEnabled,
	AccessFailedCount = @accessFailedCount

	WHERE Id=@userId
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserOptions]    Script Date: 12/06/2019 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUserOptions]
	@userId int,
	@options NVARCHAR(max)
AS
	UPDATE Users SET Options=@options WHERE Id=@userId;
GO
USE [master]
GO
ALTER DATABASE [WebsiteDB] SET  READ_WRITE 
GO
