USE [EasyGoDB]
GO
/****** Object:  Trigger [TokensTable_Trg]    Script Date: 10/19/2023 4:01:02 PM ******/
DROP TRIGGER [dbo].[TokensTable_Trg]
GO
ALTER TABLE [dbo].[UserSettingValue] DROP CONSTRAINT [FK_UserSettingValue_User]
GO
ALTER TABLE [dbo].[UserSettingValue] DROP CONSTRAINT [FK_UserSettingValue_AppSettingValue]
GO
ALTER TABLE [dbo].[UserSettingValue] DROP CONSTRAINT [FK_UserSettingValue_AppSetting]
GO
ALTER TABLE [dbo].[UserRequest] DROP CONSTRAINT [FK_UserRequest_User]
GO
ALTER TABLE [dbo].[UserLog] DROP CONSTRAINT [FK_UserLog_User]
GO
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [FK_ItemUnit_Unit1]
GO
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [FK_ItemUnit_Unit]
GO
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [FK_ItemUnit_Item]
GO
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_Unit1]
GO
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_Unit]
GO
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_Category]
GO
ALTER TABLE [dbo].[Category] DROP CONSTRAINT [FK_Category_Category]
GO
ALTER TABLE [dbo].[AppSettingValue] DROP CONSTRAINT [FK_AppSettingValue_AppSetting]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_IsActive]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_IsAdmin]
GO
ALTER TABLE [dbo].[Supplier] DROP CONSTRAINT [DF_Supplier_IsActive]
GO
ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [DF_Customer_IsActive]
GO
/****** Object:  Table [dbo].[UserSettingValue]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSettingValue]') AND type in (N'U'))
DROP TABLE [dbo].[UserSettingValue]
GO
/****** Object:  Table [dbo].[UserRequest]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRequest]') AND type in (N'U'))
DROP TABLE [dbo].[UserRequest]
GO
/****** Object:  Table [dbo].[UserLog]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserLog]') AND type in (N'U'))
DROP TABLE [dbo].[UserLog]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Unit]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Unit]') AND type in (N'U'))
DROP TABLE [dbo].[Unit]
GO
/****** Object:  Table [dbo].[TokensTable]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TokensTable]') AND type in (N'U'))
DROP TABLE [dbo].[TokensTable]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Supplier]') AND type in (N'U'))
DROP TABLE [dbo].[Supplier]
GO
/****** Object:  Table [dbo].[ProgramDetails]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProgramDetails]') AND type in (N'U'))
DROP TABLE [dbo].[ProgramDetails]
GO
/****** Object:  Table [dbo].[ItemUnit]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND type in (N'U'))
DROP TABLE [dbo].[ItemUnit]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND type in (N'U'))
DROP TABLE [dbo].[Item]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
DROP TABLE [dbo].[Customer]
GO
/****** Object:  Table [dbo].[CountryCode]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryCode]') AND type in (N'U'))
DROP TABLE [dbo].[CountryCode]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
DROP TABLE [dbo].[Category]
GO
/****** Object:  Table [dbo].[AppSettingValue]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppSettingValue]') AND type in (N'U'))
DROP TABLE [dbo].[AppSettingValue]
GO
/****** Object:  Table [dbo].[AppSetting]    Script Date: 10/19/2023 4:01:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppSetting]') AND type in (N'U'))
DROP TABLE [dbo].[AppSetting]
GO
/****** Object:  Table [dbo].[AppSetting]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSetting](
	[SettingId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Notes] [ntext] NULL,
 CONSTRAINT [PK_AppSetting] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppSettingValue]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSettingValue](
	[ValId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [ntext] NULL,
	[IsDefault] [int] NULL,
	[IsSystem] [int] NULL,
	[Notes] [ntext] NULL,
	[SettingId] [int] NULL,
 CONSTRAINT [PK_AppSettingValue] PRIMARY KEY CLUSTERED 
(
	[ValId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[Details] [ntext] NULL,
	[Image] [ntext] NULL,
	[IsActive] [bit] NULL,
	[ParentId] [int] NULL,
	[Notes] [ntext] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CountryCode]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryCode](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Currency] [nvarchar](50) NULL,
	[Name] [nvarchar](100) NULL,
	[IsDefault] [tinyint] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[TimeZoneName] [nvarchar](100) NULL,
	[TimeZoneOffset] [nvarchar](100) NULL,
 CONSTRAINT [PK_CountryCode] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Code] [nvarchar](100) NULL,
	[Company] [nvarchar](100) NULL,
	[Address] [ntext] NULL,
	[Email] [nvarchar](200) NULL,
	[Mobile] [nvarchar](100) NULL,
	[Image] [ntext] NULL,
	[Balance] [decimal](20, 3) NULL,
	[BalanceType] [tinyint] NULL,
	[Fax] [nvarchar](100) NULL,
	[IsLimited] [bit] NOT NULL,
	[MaxDeserve] [decimal](20, 3) NULL,
	[PayType] [nvarchar](20) NULL,
	[Notes] [ntext] NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](200) NULL,
	[Details] [ntext] NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Image] [ntext] NULL,
	[IsActive] [bit] NULL,
	[Min] [int] NULL,
	[Max] [int] NULL,
	[CategoryId] [int] NULL,
	[IsExpired] [bit] NOT NULL,
	[AlertDays] [int] NOT NULL,
	[IsTaxExempt] [bit] NOT NULL,
	[Taxes] [decimal](20, 3) NULL,
	[MinUnitId] [int] NULL,
	[MaxUnitId] [int] NULL,
	[AvgPurchasePrice] [decimal](20, 3) NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemUnit]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemUnit](
	[ItemUnitId] [bigint] IDENTITY(1,1) NOT NULL,
	[ItemId] [bigint] NULL,
	[UnitId] [int] NULL,
	[SubUnitId] [int] NULL,
	[UnitValue] [int] NULL,
	[IsDefaultSale] [bit] NULL,
	[IsDefaultPurchase] [bit] NULL,
	[Price] [decimal](20, 3) NULL,
	[Cost] [decimal](20, 3) NULL,
	[Barcode] [nvarchar](200) NULL,
	[IsActive] [bit] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [int] NULL,
	[UpdateUserId] [int] NULL,
	[purchasePrice] [decimal](20, 3) NULL,
 CONSTRAINT [PK_ItemUnit] PRIMARY KEY CLUSTERED 
(
	[ItemUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramDetails]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramDetails](
	[Id] [int] NOT NULL,
	[ProgramName] [nvarchar](500) NULL,
	[BranchCount] [int] NOT NULL,
	[PosCount] [int] NOT NULL,
	[UserCount] [int] NOT NULL,
	[VendorCount] [int] NOT NULL,
	[CustomerCount] [int] NOT NULL,
	[ItemCount] [int] NOT NULL,
	[SaleinvCount] [int] NOT NULL,
	[ProgramIncId] [int] NULL,
	[VersionIncId] [int] NULL,
	[VersionName] [nvarchar](500) NULL,
	[StoreCount] [int] NOT NULL,
	[PackageSaleCode] [nvarchar](500) NULL,
	[CustomerServerCode] [nvarchar](500) NULL,
	[ExpireDate] [datetime2](7) NULL,
	[IsOnlineServer] [bit] NULL,
	[PackageNumber] [nvarchar](500) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsLimitDate] [bit] NULL,
	[IsLimitCount] [bit] NULL,
	[IsActive] [bit] NOT NULL,
	[PackageName] [nvarchar](500) NULL,
	[BookDate] [datetime2](7) NULL,
	[PId] [int] NULL,
	[PcdId] [int] NULL,
	[CustomerName] [nvarchar](500) NULL,
	[CustomerLastName] [nvarchar](500) NULL,
	[IsServerActivated] [bit] NOT NULL,
	[Activatedate] [datetime2](7) NULL,
	[PocrDate] [datetime2](7) NULL,
	[PoId] [int] NULL,
	[Upnum] [nvarchar](500) NULL,
	[Notes] [nvarchar](500) NULL,
	[IsDemo] [nvarchar](10) NULL,
 CONSTRAINT [PK_ProgramDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Code] [nvarchar](100) NULL,
	[Company] [nvarchar](100) NULL,
	[Address] [ntext] NULL,
	[Email] [nvarchar](200) NULL,
	[Mobile] [nvarchar](100) NULL,
	[Image] [ntext] NULL,
	[Balance] [decimal](20, 3) NULL,
	[BalanceType] [tinyint] NULL,
	[Fax] [nvarchar](100) NULL,
	[IsLimited] [bit] NOT NULL,
	[MaxDeserve] [decimal](20, 3) NULL,
	[PayType] [nvarchar](20) NULL,
	[Notes] [ntext] NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TokensTable]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TokensTable](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](100) NOT NULL,
	[CreateDate] [datetime2](7) NULL,
 CONSTRAINT [PK_TokensTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Unit]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Unit](
	[UnitId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
	[UpdateDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED 
(
	[UnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Notes] [ntext] NULL,
	[Image] [ntext] NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [ntext] NULL,
	[IsOnline] [bit] NULL,
	[IsAdmin] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
	[RoleId] [bigint] NULL,
	[Balance] [decimal](20, 3) NULL,
	[BalanceType] [tinyint] NULL,
	[HasCommission] [bit] NOT NULL,
	[CommissionValue] [decimal](20, 3) NULL,
	[CommissionRatio] [decimal](20, 3) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLog]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLog](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[SInDate] [datetime2](7) NULL,
	[SOutDate] [datetime2](7) NULL,
	[PosId] [bigint] NULL,
	[UserId] [bigint] NULL,
 CONSTRAINT [PK_UserLog] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRequest]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRequest](
	[UserRequestId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[SInDate] [datetime2](7) NULL,
	[SOutDate] [datetime2](7) NULL,
	[LastRequestDate] [datetime2](7) NULL,
 CONSTRAINT [PK_UserRequest] PRIMARY KEY CLUSTERED 
(
	[UserRequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSettingValue]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSettingValue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[ValId] [int] NULL,
	[Note] [ntext] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
	[SettingId] [int] NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserSettingValue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AppSetting] ON 

INSERT [dbo].[AppSetting] ([SettingId], [Name], [Notes]) VALUES (1, N'language', NULL)
INSERT [dbo].[AppSetting] ([SettingId], [Name], [Notes]) VALUES (2, N'dateForm', NULL)
INSERT [dbo].[AppSetting] ([SettingId], [Name], [Notes]) VALUES (3, N'region', NULL)
INSERT [dbo].[AppSetting] ([SettingId], [Name], [Notes]) VALUES (4, N'report_lang', NULL)
INSERT [dbo].[AppSetting] ([SettingId], [Name], [Notes]) VALUES (5, N'menuIsOpen', NULL)
INSERT [dbo].[AppSetting] ([SettingId], [Name], [Notes]) VALUES (6, N'invoice_lang', NULL)
INSERT [dbo].[AppSetting] ([SettingId], [Name], [Notes]) VALUES (7, N'currency', NULL)
INSERT [dbo].[AppSetting] ([SettingId], [Name], [Notes]) VALUES (8, N'com_logo', NULL)
SET IDENTITY_INSERT [dbo].[AppSetting] OFF
GO
SET IDENTITY_INSERT [dbo].[AppSettingValue] ON 

INSERT [dbo].[AppSettingValue] ([ValId], [Value], [IsDefault], [IsSystem], [Notes], [SettingId]) VALUES (1, N'ShortDatePattern', 1, 1, NULL, 2)
SET IDENTITY_INSERT [dbo].[AppSettingValue] OFF
GO
SET IDENTITY_INSERT [dbo].[CountryCode] ON 

INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (1, N'+965', N'KWD', N'Kuwait', 1, 0, N'Arab Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (2, N'+966', N'SAR', N'Saudi Arabia', 0, 1, N'Arab Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (3, N'+968', N'OMR', N'Oman', 0, 2, N'Arabian Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (4, N'+971', N'AED', N'United Arab Emirates', 0, 3, N'Arabian Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (5, N'+974', N'QAR', N'Qatar', 0, 4, N'Arabian Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (6, N'+973', N'BHD', N'Bahrain', 0, 5, N'Arabian Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (7, N'+964', N'IQD', N'Iraq', 0, 6, N'Arabic Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (8, N'+961', N'LBP', N'Lebanon', 0, 7, N'Middle East Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (9, N'+963', N'SYP', N'Syria', 0, 8, N'Syria Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (10, N'+967', N'YER', N'Yemen', 0, 9, N'Arab Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (11, N'+962', N'JOD', N'Jordan', 0, 10, N'Jordan Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (12, N'+213', N'DZD', N'Algeria', 0, 11, N'W. Central Africa Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (13, N'+20', N'EGP', N'Egypt', 0, 12, N'Egypt Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (14, N'+216', N'TND', N'Tunisia', 0, 13, N'W. Central Africa Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (15, N'+249', N'SDG', N'Sudan', 0, 14, N'Sudan Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (16, N'+212', N'MAD', N'Morocco', 0, 15, N'Morocco Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (17, N'+218', N'LYD', N'Libya', 0, 16, N'Libya Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (18, N'+252', N'SOS', N'Somalia', 0, 17, N'E. Africa Standard Time', NULL)
INSERT [dbo].[CountryCode] ([CountryId], [Code], [Currency], [Name], [IsDefault], [CurrencyId], [TimeZoneName], [TimeZoneOffset]) VALUES (19, N'+90', N'TRY', N'Turkey', 0, 18, N'Turkey Standard Time', NULL)
SET IDENTITY_INSERT [dbo].[CountryCode] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([CustomerId], [Name], [Code], [Company], [Address], [Email], [Mobile], [Image], [Balance], [BalanceType], [Fax], [IsLimited], [MaxDeserve], [PayType], [Notes], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (1, N'عمار السويس', N'c-000001', NULL, N'Syria - Aleppo - AL Zhour street', N'Ammar@gmail.com', N'58521458745', NULL, NULL, 0, NULL, 0, NULL, NULL, N'', 1, CAST(N'2023-10-19T15:33:53.8506045' AS DateTime2), CAST(N'2023-10-19T15:43:40.2703993' AS DateTime2), 0, 0)
INSERT [dbo].[Customer] ([CustomerId], [Name], [Code], [Company], [Address], [Email], [Mobile], [Image], [Balance], [BalanceType], [Fax], [IsLimited], [MaxDeserve], [PayType], [Notes], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (2, N'أحمد نبيل عامر', N'c-000002', NULL, N'حلب - بجوار مدرسة العبور', N'Ahmad123@gmail.com', N'147852369', NULL, NULL, 0, NULL, 0, NULL, NULL, N'', 1, CAST(N'2023-10-19T15:42:51.4980351' AS DateTime2), CAST(N'2023-10-19T15:42:51.4980351' AS DateTime2), 0, 0)
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Supplier] ON 

INSERT [dbo].[Supplier] ([SupplierId], [Name], [Code], [Company], [Address], [Email], [Mobile], [Image], [Balance], [BalanceType], [Fax], [IsLimited], [MaxDeserve], [PayType], [Notes], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (1, N'سماح عبد الله', N'v-000001', N'جمعية السيدة زينب', N'سراج مدينة نصر - شارع فيصل', N'samah4175@gmail.com', N'147852369', NULL, NULL, 0, N'475125896', 0, NULL, NULL, N'', 1, CAST(N'2023-10-19T15:51:36.8202173' AS DateTime2), CAST(N'2023-10-19T15:52:08.4511705' AS DateTime2), 0, NULL)
SET IDENTITY_INSERT [dbo].[Supplier] OFF
GO
SET IDENTITY_INSERT [dbo].[TokensTable] ON 

INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (1, N'1334184785569396930', CAST(N'2023-10-15T15:47:54.1050427' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (2, N'1334184783633035730', CAST(N'2023-10-15T15:48:35.6728368' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (3, N'1334186999221881750', CAST(N'2023-10-15T21:59:49.3671459' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (4, N'1334187022039915410', CAST(N'2023-10-15T22:00:28.1361506' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (5, N'1334187010010251070', CAST(N'2023-10-15T22:01:10.8246881' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (6, N'1334187011322402600', CAST(N'2023-10-15T22:01:47.4338275' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (7, N'1334187031049340130', CAST(N'2023-10-15T22:01:57.9853368' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (8, N'1334187032002236540', CAST(N'2023-10-15T22:03:44.5798629' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (9, N'1334187111012158980', CAST(N'2023-10-15T22:18:03.7021742' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (10, N'1334187134095449270', CAST(N'2023-10-15T22:20:32.5741006' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (11, N'1334187142237967320', CAST(N'2023-10-15T22:22:07.6256489' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (12, N'1334187137867000910', CAST(N'2023-10-15T22:22:37.1817338' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (13, N'1334187165359524050', CAST(N'2023-10-15T22:24:33.6434167' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (14, N'1334187154932395050', CAST(N'2023-10-15T22:24:34.2348309' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (15, N'1334187149946687890', CAST(N'2023-10-15T22:24:34.3106829' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (16, N'1334187163112632850', CAST(N'2023-10-15T22:24:50.2900585' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (17, N'1334187154470124670', CAST(N'2023-10-15T22:24:50.5772894' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (18, N'1334187153314697830', CAST(N'2023-10-15T22:24:50.7079411' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (19, N'1334187173615983380', CAST(N'2023-10-15T22:26:16.8151837' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (20, N'1334187164189977740', CAST(N'2023-10-15T22:26:41.4315169' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (21, N'1334187165858405520', CAST(N'2023-10-15T22:26:58.2184525' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (22, N'1334187196871676500', CAST(N'2023-10-15T22:31:16.4457379' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (23, N'1334187193778863890', CAST(N'2023-10-15T22:31:43.6180076' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (24, N'1334187201709664270', CAST(N'2023-10-15T22:31:46.9611300' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (25, N'1334187196799256450', CAST(N'2023-10-15T22:31:47.0179788' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (26, N'1334187195279708370', CAST(N'2023-10-15T22:31:52.1096762' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (27, N'1334187254275449190', CAST(N'2023-10-15T22:40:41.7951379' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (28, N'1334187272446501250', CAST(N'2023-10-15T22:42:02.1975723' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (29, N'1334187260906504690', CAST(N'2023-10-15T22:42:04.3313616' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (30, N'1334187270874927210', CAST(N'2023-10-15T22:42:25.7816741' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (31, N'1334187266883607180', CAST(N'2023-10-15T22:42:26.0659148' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (32, N'1334187291598546580', CAST(N'2023-10-15T22:45:27.2209772' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (33, N'1334187281477699360', CAST(N'2023-10-15T22:45:27.6667992' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (34, N'1334187298621780080', CAST(N'2023-10-15T22:46:17.8024947' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (35, N'1334187298123873000', CAST(N'2023-10-15T22:47:48.7876976' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (36, N'1334187314893209150', CAST(N'2023-10-15T22:49:27.5759486' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (37, N'1334187322579562170', CAST(N'2023-10-15T22:52:38.7438903' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (38, N'1334187381022890700', CAST(N'2023-10-15T23:02:53.7653628' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (39, N'1334187690256007130', CAST(N'2023-10-15T23:53:59.8698347' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (40, N'1334187752356799050', CAST(N'2023-10-16T00:02:57.8656928' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (41, N'1334188176173700470', CAST(N'2023-10-16T01:13:20.0667264' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (42, N'1334204631002823790', CAST(N'2023-10-17T22:56:16.8026460' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (43, N'1334204685257756270', CAST(N'2023-10-17T23:04:35.6973092' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (44, N'1334204706172282660', CAST(N'2023-10-17T23:09:48.4997232' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (45, N'1334204755742545910', CAST(N'2023-10-17T23:19:01.1464564' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (46, N'1334204787435292790', CAST(N'2023-10-17T23:22:47.6837082' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (47, N'1334204795951133550', CAST(N'2023-10-17T23:23:33.2399447' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (48, N'1334204784606710230', CAST(N'2023-10-17T23:23:33.7774568' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (49, N'1334204789794939640', CAST(N'2023-10-17T23:24:06.3719025' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (50, N'1334204792849432960', CAST(N'2023-10-17T23:24:08.1703708' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (51, N'1334204805729438970', CAST(N'2023-10-17T23:24:08.2773611' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (52, N'1334204801791772120', CAST(N'2023-10-17T23:24:17.5605943' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (53, N'1334219055774087030', CAST(N'2023-10-19T15:02:08.7071621' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (54, N'1334219194041718650', CAST(N'2023-10-19T15:24:40.1439809' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (55, N'1334219237610518780', CAST(N'2023-10-19T15:32:53.7921181' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (56, N'1334219256177846760', CAST(N'2023-10-19T15:33:00.6785671' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (57, N'1334219249339713320', CAST(N'2023-10-19T15:33:53.6504289' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (58, N'1334219259771507990', CAST(N'2023-10-19T15:33:54.2398694' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (59, N'1334219297169047940', CAST(N'2023-10-19T15:42:51.1503433' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (60, N'1334219316659894280', CAST(N'2023-10-19T15:42:51.8988783' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (61, N'1334219313311754650', CAST(N'2023-10-19T15:43:40.2058829' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (62, N'1334219356935951710', CAST(N'2023-10-19T15:49:55.5279456' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (63, N'1334219353176509490', CAST(N'2023-10-19T15:51:36.4731049' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (64, N'1334219362885777820', CAST(N'2023-10-19T15:51:37.2800294' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (65, N'1334219355041454730', CAST(N'2023-10-19T15:52:08.3893365' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (66, N'1334219366150225560', CAST(N'2023-10-19T15:52:40.5904208' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (67, N'1334219364984327380', CAST(N'2023-10-19T15:52:40.8268789' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (68, N'1334219375016625780', CAST(N'2023-10-19T15:52:44.3791498' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (69, N'1334219376779148810', CAST(N'2023-10-19T15:52:44.5154397' AS DateTime2))
SET IDENTITY_INSERT [dbo].[TokensTable] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [UserName], [Password], [FirstName], [LastName], [Mobile], [Notes], [Image], [Email], [Address], [IsOnline], [IsAdmin], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId], [RoleId], [Balance], [BalanceType], [HasCommission], [CommissionValue], [CommissionRatio]) VALUES (1, N'admin', N'1b8baf4f819e5b304e1a176e1c590c84', N'Admin', NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, CAST(0.000 AS Decimal(20, 3)), 0, 1, NULL, NULL)
INSERT [dbo].[User] ([UserId], [UserName], [Password], [FirstName], [LastName], [Mobile], [Notes], [Image], [Email], [Address], [IsOnline], [IsAdmin], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId], [RoleId], [Balance], [BalanceType], [HasCommission], [CommissionValue], [CommissionRatio]) VALUES (2, N'Support@EasyGo', N'1b8baf4f819e5b304e1a176e1c590c84', N'Support', NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, CAST(0.000 AS Decimal(20, 3)), 0, 0, NULL, NULL)
INSERT [dbo].[User] ([UserId], [UserName], [Password], [FirstName], [LastName], [Mobile], [Notes], [Image], [Email], [Address], [IsOnline], [IsAdmin], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId], [RoleId], [Balance], [BalanceType], [HasCommission], [CommissionValue], [CommissionRatio]) VALUES (3, N'dina', N'2a4554a27781afd443288798eddaa9fb', N'dina', N'nemah', N'959353886', N'', N'71f020248a405d21e94d1de52043bed4.jfif', N'dina@gmail.com', N'Aleppo', NULL, 0, 1, CAST(N'2023-10-15T22:20:32.7576157' AS DateTime2), CAST(N'2023-10-15T22:31:44.3307003' AS DateTime2), NULL, NULL, NULL, CAST(0.000 AS Decimal(20, 3)), 0, 0, NULL, NULL)
INSERT [dbo].[User] ([UserId], [UserName], [Password], [FirstName], [LastName], [Mobile], [Notes], [Image], [Email], [Address], [IsOnline], [IsAdmin], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId], [RoleId], [Balance], [BalanceType], [HasCommission], [CommissionValue], [CommissionRatio]) VALUES (5, N'Yasin', N'c82c767fa7af27a9bb75f188541b9bb2', N'Yasin', N'Idlbi', N'963696969', N'', N'f96f8a89e2143f1e43a2ba7953fb5413.jfif', N'YasinIdlbi@gmail.com', N'', NULL, 0, 1, CAST(N'2023-10-17T23:23:33.4198674' AS DateTime2), CAST(N'2023-10-17T23:24:06.4612659' AS DateTime2), NULL, NULL, NULL, CAST(0.000 AS Decimal(20, 3)), 0, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Supplier] ADD  CONSTRAINT [DF_Supplier_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AppSettingValue]  WITH CHECK ADD  CONSTRAINT [FK_AppSettingValue_AppSetting] FOREIGN KEY([SettingId])
REFERENCES [dbo].[AppSetting] ([SettingId])
GO
ALTER TABLE [dbo].[AppSettingValue] CHECK CONSTRAINT [FK_AppSettingValue_AppSetting]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Category] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Category]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Category]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Unit] FOREIGN KEY([MinUnitId])
REFERENCES [dbo].[Unit] ([UnitId])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Unit]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Unit1] FOREIGN KEY([MaxUnitId])
REFERENCES [dbo].[Unit] ([UnitId])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Unit1]
GO
ALTER TABLE [dbo].[ItemUnit]  WITH CHECK ADD  CONSTRAINT [FK_ItemUnit_Item] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([ItemId])
GO
ALTER TABLE [dbo].[ItemUnit] CHECK CONSTRAINT [FK_ItemUnit_Item]
GO
ALTER TABLE [dbo].[ItemUnit]  WITH CHECK ADD  CONSTRAINT [FK_ItemUnit_Unit] FOREIGN KEY([UnitId])
REFERENCES [dbo].[Unit] ([UnitId])
GO
ALTER TABLE [dbo].[ItemUnit] CHECK CONSTRAINT [FK_ItemUnit_Unit]
GO
ALTER TABLE [dbo].[ItemUnit]  WITH CHECK ADD  CONSTRAINT [FK_ItemUnit_Unit1] FOREIGN KEY([SubUnitId])
REFERENCES [dbo].[Unit] ([UnitId])
GO
ALTER TABLE [dbo].[ItemUnit] CHECK CONSTRAINT [FK_ItemUnit_Unit1]
GO
ALTER TABLE [dbo].[UserLog]  WITH CHECK ADD  CONSTRAINT [FK_UserLog_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserLog] CHECK CONSTRAINT [FK_UserLog_User]
GO
ALTER TABLE [dbo].[UserRequest]  WITH CHECK ADD  CONSTRAINT [FK_UserRequest_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserRequest] CHECK CONSTRAINT [FK_UserRequest_User]
GO
ALTER TABLE [dbo].[UserSettingValue]  WITH CHECK ADD  CONSTRAINT [FK_UserSettingValue_AppSetting] FOREIGN KEY([SettingId])
REFERENCES [dbo].[AppSetting] ([SettingId])
GO
ALTER TABLE [dbo].[UserSettingValue] CHECK CONSTRAINT [FK_UserSettingValue_AppSetting]
GO
ALTER TABLE [dbo].[UserSettingValue]  WITH CHECK ADD  CONSTRAINT [FK_UserSettingValue_AppSettingValue] FOREIGN KEY([ValId])
REFERENCES [dbo].[AppSettingValue] ([ValId])
GO
ALTER TABLE [dbo].[UserSettingValue] CHECK CONSTRAINT [FK_UserSettingValue_AppSettingValue]
GO
ALTER TABLE [dbo].[UserSettingValue]  WITH CHECK ADD  CONSTRAINT [FK_UserSettingValue_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserSettingValue] CHECK CONSTRAINT [FK_UserSettingValue_User]
GO
/****** Object:  Trigger [dbo].[TokensTable_Trg]    Script Date: 10/19/2023 4:01:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TokensTable_Trg]
   ON  [dbo].[TokensTable] for delete
AS 
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;

  DBCC CHECKIDENT (TokensTable, RESEED);
  
;  
    -- Insert statements for trigger here

END
GO
ALTER TABLE [dbo].[TokensTable] ENABLE TRIGGER [TokensTable_Trg]
GO
