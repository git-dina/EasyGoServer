USE [EasyGoDB]
GO
/****** Object:  Trigger [TokensTable_Trg]    Script Date: 11/5/2023 1:37:33 PM ******/
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
ALTER TABLE [dbo].[PurInvoiceItem] DROP CONSTRAINT [FK_PurInvoiceItem_PurchaseInvoice]
GO
ALTER TABLE [dbo].[PurInvoiceItem] DROP CONSTRAINT [FK_PurInvoiceItem_ItemUnit]
GO
ALTER TABLE [dbo].[PurchaseInvoice] DROP CONSTRAINT [FK_PurchaseInvoice_Supplier]
GO
ALTER TABLE [dbo].[POS] DROP CONSTRAINT [FK_POS_Branch]
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
/****** Object:  Table [dbo].[UserSettingValue]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSettingValue]') AND type in (N'U'))
DROP TABLE [dbo].[UserSettingValue]
GO
/****** Object:  Table [dbo].[UserRequest]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRequest]') AND type in (N'U'))
DROP TABLE [dbo].[UserRequest]
GO
/****** Object:  Table [dbo].[UserLog]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserLog]') AND type in (N'U'))
DROP TABLE [dbo].[UserLog]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Unit]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Unit]') AND type in (N'U'))
DROP TABLE [dbo].[Unit]
GO
/****** Object:  Table [dbo].[TokensTable]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TokensTable]') AND type in (N'U'))
DROP TABLE [dbo].[TokensTable]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Supplier]') AND type in (N'U'))
DROP TABLE [dbo].[Supplier]
GO
/****** Object:  Table [dbo].[PurInvoiceItem]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PurInvoiceItem]') AND type in (N'U'))
DROP TABLE [dbo].[PurInvoiceItem]
GO
/****** Object:  Table [dbo].[PurchaseInvoice]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchaseInvoice]') AND type in (N'U'))
DROP TABLE [dbo].[PurchaseInvoice]
GO
/****** Object:  Table [dbo].[ProgramDetails]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProgramDetails]') AND type in (N'U'))
DROP TABLE [dbo].[ProgramDetails]
GO
/****** Object:  Table [dbo].[POS]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[POS]') AND type in (N'U'))
DROP TABLE [dbo].[POS]
GO
/****** Object:  Table [dbo].[ItemUnit]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND type in (N'U'))
DROP TABLE [dbo].[ItemUnit]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND type in (N'U'))
DROP TABLE [dbo].[Item]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
DROP TABLE [dbo].[Customer]
GO
/****** Object:  Table [dbo].[CountryCode]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryCode]') AND type in (N'U'))
DROP TABLE [dbo].[CountryCode]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
DROP TABLE [dbo].[Category]
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Branch]') AND type in (N'U'))
DROP TABLE [dbo].[Branch]
GO
/****** Object:  Table [dbo].[AppSettingValue]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppSettingValue]') AND type in (N'U'))
DROP TABLE [dbo].[AppSettingValue]
GO
/****** Object:  Table [dbo].[AppSetting]    Script Date: 11/5/2023 1:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppSetting]') AND type in (N'U'))
DROP TABLE [dbo].[AppSetting]
GO
/****** Object:  Table [dbo].[AppSetting]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[AppSettingValue]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[Branch]    Script Date: 11/5/2023 1:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branch](
	[BranchId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[Address] [ntext] NULL,
	[Email] [nvarchar](200) NULL,
	[Phone] [nvarchar](100) NULL,
	[Mobile] [nvarchar](100) NULL,
	[Notes] [ntext] NULL,
	[ParentId] [int] NULL,
	[Type] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[BranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[CountryCode]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[Customer]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[Item]    Script Date: 11/5/2023 1:37:33 PM ******/
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
	[Taxes] [decimal](20, 3) NULL,
	[MinUnitId] [int] NULL,
	[MaxUnitId] [int] NULL,
	[AvgPurchasePrice] [decimal](20, 3) NULL,
	[Notes] [ntext] NULL,
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
/****** Object:  Table [dbo].[ItemUnit]    Script Date: 11/5/2023 1:37:33 PM ******/
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
	[PurchasePrice] [decimal](20, 3) NULL,
	[PackCost] [decimal](20, 3) NULL,
	[Notes] [ntext] NULL,
	[UnitCount] [int] NOT NULL,
	[SmallestUnitId] [int] NULL,
	[IsActive] [bit] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
 CONSTRAINT [PK_ItemUnit] PRIMARY KEY CLUSTERED 
(
	[ItemUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[POS]    Script Date: 11/5/2023 1:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[POS](
	[PosId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[Balance] [decimal](20, 3) NULL,
	[BranchId] [int] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
	[IsActive] [bit] NULL,
	[Notes] [ntext] NULL,
	[BalanceAll] [decimal](20, 3) NULL,
	[BoxState] [nvarchar](20) NULL,
	[IsAdminClose] [tinyint] NOT NULL,
 CONSTRAINT [PK_POS] PRIMARY KEY CLUSTERED 
(
	[PosId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramDetails]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[PurchaseInvoice]    Script Date: 11/5/2023 1:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseInvoice](
	[InvoiceId] [bigint] IDENTITY(1,1) NOT NULL,
	[InvNumber] [nvarchar](100) NULL,
	[InvType] [nvarchar](50) NULL,
	[SupplierId] [bigint] NULL,
	[DiscountType] [nvarchar](10) NULL,
	[DiscountValue] [decimal](20, 3) NULL,
	[DiscountPercentage] [decimal](20, 3) NULL,
	[Total] [decimal](20, 3) NULL,
	[TotalNet] [decimal](20, 3) NULL,
	[Paid] [decimal](20, 3) NULL,
	[Deserved] [decimal](20, 3) NULL,
	[DeservedDate] [date] NULL,
	[BranchCreatorId] [int] NULL,
	[BranchId] [int] NULL,
	[Tax] [decimal](20, 3) NULL,
	[TaxType] [nvarchar](10) NULL,
	[TaxPercentage] [decimal](20, 3) NOT NULL,
	[InvDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
	[InvoiceMainId] [int] NULL,
	[Notes] [ntext] NULL,
	[VendorInvNum] [nvarchar](100) NULL,
	[VendorInvDate] [datetime2](7) NULL,
	[PosId] [int] NULL,
	[IsApproved] [tinyint] NULL,
	[IsActive] [bit] NOT NULL,
	[ShippingCost] [decimal](20, 3) NOT NULL,
 CONSTRAINT [PK_PurchaseInvoice] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurInvoiceItem]    Script Date: 11/5/2023 1:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurInvoiceItem](
	[InvItemId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceId] [bigint] NULL,
	[Quantity] [int] NULL,
	[Notes] [ntext] NULL,
	[Price] [decimal](20, 3) NOT NULL,
	[Total] [decimal](20, 3) NOT NULL,
	[ItemUnitId] [bigint] NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateUserId] [bigint] NULL,
	[UpdateUserId] [bigint] NULL,
 CONSTRAINT [PK_PurInvoiceItem] PRIMARY KEY CLUSTERED 
(
	[InvItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[TokensTable]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[Unit]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[UserLog]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[UserRequest]    Script Date: 11/5/2023 1:37:33 PM ******/
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
/****** Object:  Table [dbo].[UserSettingValue]    Script Date: 11/5/2023 1:37:33 PM ******/
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
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [Code], [Name], [Details], [Image], [IsActive], [ParentId], [Notes], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (1, N'A1', N'اكسسوارات', N'', NULL, 1, NULL, N'', CAST(N'2023-10-19T18:32:44.0122325' AS DateTime2), CAST(N'2023-11-03T18:13:26.5174401' AS DateTime2), NULL, NULL)
INSERT [dbo].[Category] ([CategoryId], [Code], [Name], [Details], [Image], [IsActive], [ParentId], [Notes], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (2, N'A1-01', N'عقد', N'', NULL, 1, 1, N'', CAST(N'2023-10-31T20:45:37.9353994' AS DateTime2), CAST(N'2023-11-03T18:13:26.5254229' AS DateTime2), NULL, NULL)
INSERT [dbo].[Category] ([CategoryId], [Code], [Name], [Details], [Image], [IsActive], [ParentId], [Notes], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (3, N'A1-02', N'أقراط', N'', NULL, 1, 1, N'', CAST(N'2023-10-31T20:46:06.3983352' AS DateTime2), CAST(N'2023-11-03T18:13:26.5284168' AS DateTime2), NULL, NULL)
INSERT [dbo].[Category] ([CategoryId], [Code], [Name], [Details], [Image], [IsActive], [ParentId], [Notes], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (4, N'A2', N'مكياج', N'', NULL, 1, NULL, N'', CAST(N'2023-10-31T20:47:06.5569665' AS DateTime2), CAST(N'2023-10-31T20:47:06.5659420' AS DateTime2), NULL, NULL)
INSERT [dbo].[Category] ([CategoryId], [Code], [Name], [Details], [Image], [IsActive], [ParentId], [Notes], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (5, N'A1-03', N'خواتم', N'', NULL, 1, 1, N'', CAST(N'2023-11-03T18:13:50.4196578' AS DateTime2), CAST(N'2023-11-03T18:13:50.4246445' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Category] OFF
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
SET IDENTITY_INSERT [dbo].[Item] ON 

INSERT [dbo].[Item] ([ItemId], [Code], [Name], [Details], [Type], [Image], [IsActive], [Min], [Max], [CategoryId], [IsExpired], [Taxes], [MinUnitId], [MaxUnitId], [AvgPurchasePrice], [Notes], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (2, N'A0-01', N'عقد لولو 100 حبة لون أبيض', N'', N'normal', N'c37858823db0093766eee74d8ee1f1e5.jpg', 1, NULL, NULL, 2, 0, NULL, NULL, NULL, CAST(0.000 AS Decimal(20, 3)), N'', CAST(N'2023-10-29T23:50:50.8617663' AS DateTime2), CAST(N'2023-11-04T21:03:03.8776857' AS DateTime2), NULL, NULL)
INSERT [dbo].[Item] ([ItemId], [Code], [Name], [Details], [Type], [Image], [IsActive], [Min], [Max], [CategoryId], [IsExpired], [Taxes], [MinUnitId], [MaxUnitId], [AvgPurchasePrice], [Notes], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (4, N'365 AED', N'خاتم ذهب - شكل قلب - عيار 18', N'لون الذهب مزيج من الألوان', N'normal', N'd2ec5f1ed83abfca0dfec76506b696b3.jpg', 1, NULL, NULL, 5, 0, NULL, NULL, NULL, CAST(0.000 AS Decimal(20, 3)), N'', CAST(N'2023-11-04T20:44:13.9429122' AS DateTime2), CAST(N'2023-11-04T20:46:48.3870506' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Item] OFF
GO
SET IDENTITY_INSERT [dbo].[ItemUnit] ON 

INSERT [dbo].[ItemUnit] ([ItemUnitId], [ItemId], [UnitId], [SubUnitId], [UnitValue], [IsDefaultSale], [IsDefaultPurchase], [Price], [Cost], [Barcode], [PurchasePrice], [PackCost], [Notes], [UnitCount], [SmallestUnitId], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (1, 2, 2, 2, 1, 1, 1, CAST(11.000 AS Decimal(20, 3)), NULL, N'2087046985100', CAST(11.000 AS Decimal(20, 3)), NULL, N'', 1, 2, 1, CAST(N'2023-10-31T00:57:49.9246175' AS DateTime2), CAST(N'2023-11-04T20:32:45.1161716' AS DateTime2), 0, 0)
INSERT [dbo].[ItemUnit] ([ItemUnitId], [ItemId], [UnitId], [SubUnitId], [UnitValue], [IsDefaultSale], [IsDefaultPurchase], [Price], [Cost], [Barcode], [PurchasePrice], [PackCost], [Notes], [UnitCount], [SmallestUnitId], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (2, 2, 5, 2, 10, 0, 0, CAST(95.000 AS Decimal(20, 3)), NULL, N'2087046874602', CAST(90.000 AS Decimal(20, 3)), NULL, N'', 10, 2, 1, CAST(N'2023-10-31T20:15:31.6300493' AS DateTime2), CAST(N'2023-10-31T20:15:31.6380276' AS DateTime2), 0, 0)
INSERT [dbo].[ItemUnit] ([ItemUnitId], [ItemId], [UnitId], [SubUnitId], [UnitValue], [IsDefaultSale], [IsDefaultPurchase], [Price], [Cost], [Barcode], [PurchasePrice], [PackCost], [Notes], [UnitCount], [SmallestUnitId], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (3, 2, 6, 5, 10, 0, 0, CAST(1000.000 AS Decimal(20, 3)), NULL, N'2087086679700', CAST(800.000 AS Decimal(20, 3)), NULL, N'', 100, 2, 1, CAST(N'2023-11-04T19:33:57.2887166' AS DateTime2), CAST(N'2023-11-04T19:33:57.3126529' AS DateTime2), 0, 0)
INSERT [dbo].[ItemUnit] ([ItemUnitId], [ItemId], [UnitId], [SubUnitId], [UnitValue], [IsDefaultSale], [IsDefaultPurchase], [Price], [Cost], [Barcode], [PurchasePrice], [PackCost], [Notes], [UnitCount], [SmallestUnitId], [IsActive], [CreateDate], [UpdateDate], [CreateUserId], [UpdateUserId]) VALUES (4, 4, 2, 2, 1, 1, 1, CAST(2200.000 AS Decimal(20, 3)), NULL, N'5087087121700', CAST(2000.000 AS Decimal(20, 3)), NULL, N'', 1, 2, 1, CAST(N'2023-11-04T20:47:21.3280441' AS DateTime2), CAST(N'2023-11-04T20:47:21.3350246' AS DateTime2), 0, 0)
SET IDENTITY_INSERT [dbo].[ItemUnit] OFF
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
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (70, N'1334220328487474600', CAST(N'2023-10-19T18:32:43.6456069' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (71, N'1334262767679351140', CAST(N'2023-10-24T16:25:49.1366507' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (72, N'1334262763944463350', CAST(N'2023-10-24T16:26:09.6898974' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (73, N'1334264398058575070', CAST(N'2023-10-24T20:58:08.7186965' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (74, N'1334264443808851570', CAST(N'2023-10-24T21:04:33.9976327' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (75, N'1334264434157718510', CAST(N'2023-10-24T21:04:34.5152604' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (76, N'1334264666058796410', CAST(N'2023-10-24T21:43:03.6667635' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (77, N'1334264659422466790', CAST(N'2023-10-24T21:43:04.3996551' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (78, N'1334264669004713270', CAST(N'2023-10-24T21:44:10.0871553' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (79, N'1334264790813647970', CAST(N'2023-10-24T22:03:09.1285265' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (80, N'1334264787619274260', CAST(N'2023-10-24T22:03:09.6229726' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (81, N'1334265163931800740', CAST(N'2023-10-24T23:06:41.1359360' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (82, N'1334265160773257350', CAST(N'2023-10-24T23:06:41.7277985' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (83, N'1334265214981182950', CAST(N'2023-10-24T23:14:19.6964791' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (84, N'1334265222070634010', CAST(N'2023-10-24T23:14:20.8915260' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (85, N'1334265226593199000', CAST(N'2023-10-24T23:14:50.7117992' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (86, N'1334265216194952150', CAST(N'2023-10-24T23:14:51.3211690' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (87, N'1334265220436527820', CAST(N'2023-10-24T23:16:29.3546850' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (88, N'1334265234198486380', CAST(N'2023-10-24T23:18:48.8217290' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (89, N'1334265237524490170', CAST(N'2023-10-24T23:18:58.0259850' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (90, N'1334265325690613590', CAST(N'2023-10-24T23:33:37.1975848' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (91, N'1334265330592973370', CAST(N'2023-10-24T23:33:37.7233580' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (92, N'1334274267945693370', CAST(N'2023-10-26T00:23:24.9553288' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (93, N'1334274265460577330', CAST(N'2023-10-26T00:23:25.8525388' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (94, N'1334274386066222960', CAST(N'2023-10-26T00:40:52.2134150' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (95, N'1334274369248652970', CAST(N'2023-10-26T00:41:02.0309331' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (96, N'1334274375757313810', CAST(N'2023-10-26T00:43:04.4748166' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (97, N'1334297291295964670', CAST(N'2023-10-28T15:22:05.0924887' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (98, N'1334300012890204260', CAST(N'2023-10-28T22:53:33.4802647' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (99, N'1334300030048523060', CAST(N'2023-10-28T22:56:14.6617930' AS DateTime2))
GO
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (100, N'1334300041596005980', CAST(N'2023-10-28T22:56:59.7828357' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (101, N'1334300443252044020', CAST(N'2023-10-29T00:03:54.9772415' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (102, N'1334300461143471000', CAST(N'2023-10-29T00:07:12.9042834' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (103, N'1334300453719381770', CAST(N'2023-10-29T00:07:41.3144546' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (104, N'1334308562556049200', CAST(N'2023-10-29T22:39:45.1605138' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (105, N'1334308561185560700', CAST(N'2023-10-29T22:39:46.2944811' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (106, N'1334308564086198930', CAST(N'2023-10-29T22:40:17.4539869' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (107, N'1334308639365702280', CAST(N'2023-10-29T22:50:50.3900297' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (108, N'1334308644504099070', CAST(N'2023-10-29T22:50:51.5628906' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (109, N'1334308632318412020', CAST(N'2023-10-29T22:51:18.5911651' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (110, N'1334308638756102020', CAST(N'2023-10-29T22:51:18.8125724' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (111, N'1334308632679663880', CAST(N'2023-10-29T22:51:34.8844379' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (112, N'1334308632481943850', CAST(N'2023-10-29T22:51:35.0968705' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (113, N'1334308664133115350', CAST(N'2023-10-29T22:55:07.4040997' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (114, N'1334308674266992530', CAST(N'2023-10-29T22:55:52.1505798' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (115, N'1334308673998595920', CAST(N'2023-10-29T22:55:55.3228836' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (116, N'1334308659275310980', CAST(N'2023-10-29T22:56:26.0119157' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (117, N'1334308671534152120', CAST(N'2023-10-29T22:56:26.5719878' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (118, N'1334309059984465380', CAST(N'2023-10-30T00:02:42.7083715' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (119, N'1334309075685645400', CAST(N'2023-10-30T00:02:43.8462729' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (120, N'1334309067511744520', CAST(N'2023-10-30T00:02:44.1970589' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (121, N'1334309269047275900', CAST(N'2023-10-30T00:34:53.1706950' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (122, N'1334309252965931620', CAST(N'2023-10-30T00:34:54.1069121' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (123, N'1334309259500391690', CAST(N'2023-10-30T00:34:54.5826713' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (124, N'1334309273943874100', CAST(N'2023-10-30T00:36:53.6730713' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (125, N'1334309263685787270', CAST(N'2023-10-30T00:36:54.0708833' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (126, N'1334309279146660460', CAST(N'2023-10-30T00:39:30.8013539' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (127, N'1334309304805164880', CAST(N'2023-10-30T00:41:43.4877878' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (128, N'1334309290783321460', CAST(N'2023-10-30T00:41:45.0992342' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (129, N'1334309307147601220', CAST(N'2023-10-30T00:43:00.6163126' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (130, N'1334309314389730080', CAST(N'2023-10-30T00:43:01.7626650' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (131, N'1334309306784527540', CAST(N'2023-10-30T00:43:01.8145268' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (132, N'1334309321321366920', CAST(N'2023-10-30T00:43:20.8353789' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (133, N'1334309318086240000', CAST(N'2023-10-30T00:43:24.1339665' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (134, N'1334309313466529170', CAST(N'2023-10-30T00:44:39.1031293' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (135, N'1334309322652850210', CAST(N'2023-10-30T00:46:27.5791700' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (136, N'1334309323425911940', CAST(N'2023-10-30T00:46:28.0821901' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (137, N'1334309322820698000', CAST(N'2023-10-30T00:46:29.9086425' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (138, N'1334309323990918990', CAST(N'2023-10-30T00:46:42.7092370' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (139, N'1334309327898083910', CAST(N'2023-10-30T00:46:42.8863490' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (140, N'1334309332109245830', CAST(N'2023-10-30T00:46:43.1672938' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (141, N'1334309328718855420', CAST(N'2023-10-30T00:46:44.8614155' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (142, N'1334309343028558470', CAST(N'2023-10-30T00:47:04.4902643' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (143, N'1334309335000284480', CAST(N'2023-10-30T00:47:04.7324343' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (144, N'1334309334955828450', CAST(N'2023-10-30T00:47:05.0075691' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (145, N'1334309333074631840', CAST(N'2023-10-30T00:47:07.3332152' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (146, N'1334317082951739940', CAST(N'2023-10-30T22:19:21.3661285' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (147, N'1334317128885389510', CAST(N'2023-10-30T22:24:43.4213048' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (148, N'1334317116194106170', CAST(N'2023-10-30T22:24:43.8679698' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (149, N'1334317120592442790', CAST(N'2023-10-30T22:24:55.9086802' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (150, N'1334317123331330580', CAST(N'2023-10-30T22:25:03.1506735' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (151, N'1334317126444184220', CAST(N'2023-10-30T22:25:03.2528779' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (152, N'1334317129903741600', CAST(N'2023-10-30T22:25:09.5816064' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (153, N'1334317122861566810', CAST(N'2023-10-30T22:25:09.6896186' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (154, N'1334317161990070300', CAST(N'2023-10-30T22:32:26.3831916' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (155, N'1334317159762453700', CAST(N'2023-10-30T22:32:26.5537333' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (156, N'1334317171479837900', CAST(N'2023-10-30T22:35:14.2417109' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (157, N'1334317192897327180', CAST(N'2023-10-30T22:35:14.4312049' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (158, N'1334317220633601590', CAST(N'2023-10-30T22:40:29.4335685' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (159, N'1334317220409205870', CAST(N'2023-10-30T22:40:29.6611304' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (160, N'1334317210823865860', CAST(N'2023-10-30T22:40:34.3126814' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (161, N'1334317216359625220', CAST(N'2023-10-30T22:40:34.4762444' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (162, N'1334317225285309690', CAST(N'2023-10-30T22:41:29.4877024' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (163, N'1334317220094670380', CAST(N'2023-10-30T22:41:29.6283278' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (164, N'1334317229343200010', CAST(N'2023-10-30T22:41:31.9785296' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (165, N'1334317236255325660', CAST(N'2023-10-30T22:43:43.3574292' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (166, N'1334317248246278450', CAST(N'2023-10-30T22:44:45.2538268' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (167, N'1334317440977143570', CAST(N'2023-10-30T23:17:47.0632240' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (168, N'1334317443035606570', CAST(N'2023-10-30T23:19:29.0794045' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (169, N'1334317577683931230', CAST(N'2023-10-30T23:40:31.1476133' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (170, N'1334317589468546650', CAST(N'2023-10-30T23:42:46.2781012' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (171, N'1334317611395769380', CAST(N'2023-10-30T23:46:18.7792922' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (172, N'1334317617339708000', CAST(N'2023-10-30T23:46:19.7427154' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (173, N'1334317601825734070', CAST(N'2023-10-30T23:46:51.8607901' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (174, N'1334317607052659560', CAST(N'2023-10-30T23:46:52.0632484' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (175, N'1334317615239214680', CAST(N'2023-10-30T23:46:57.5046905' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (176, N'1334317633948588780', CAST(N'2023-10-30T23:51:30.8719928' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (177, N'1334317644984880340', CAST(N'2023-10-30T23:51:31.3826263' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (178, N'1334317645942329910', CAST(N'2023-10-30T23:51:38.3140849' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (179, N'1334317638994752660', CAST(N'2023-10-30T23:51:38.4936028' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (180, N'1334317648903025170', CAST(N'2023-10-30T23:51:40.6707773' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (181, N'1334317647601672240', CAST(N'2023-10-30T23:53:33.7757519' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (182, N'1334317643886385930', CAST(N'2023-10-30T23:53:34.2255464' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (183, N'1334317659099781260', CAST(N'2023-10-30T23:53:40.4249611' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (184, N'1334317653980509620', CAST(N'2023-10-30T23:53:40.5655854' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (185, N'1334317660330872930', CAST(N'2023-10-30T23:53:43.0828499' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (186, N'1334317655746894420', CAST(N'2023-10-30T23:55:56.3533142' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (187, N'1334317673916525280', CAST(N'2023-10-30T23:55:56.8310350' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (188, N'1334317662281968970', CAST(N'2023-10-30T23:56:01.2432314' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (189, N'1334317661582187230', CAST(N'2023-10-30T23:56:05.1673884' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (190, N'1334317678410393250', CAST(N'2023-10-30T23:56:10.0448251' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (191, N'1334317674759620800', CAST(N'2023-10-30T23:56:36.7802999' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (192, N'1334317673709664940', CAST(N'2023-10-30T23:57:49.5206981' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (193, N'1334317682517280980', CAST(N'2023-10-30T23:59:02.5508344' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (194, N'1334324035684366270', CAST(N'2023-10-31T17:36:21.9897392' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (195, N'1334324027366098490', CAST(N'2023-10-31T17:36:23.6941791' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (196, N'1334324023290502290', CAST(N'2023-10-31T17:36:34.6562212' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (197, N'1334324033474591580', CAST(N'2023-10-31T17:36:38.2605768' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (198, N'1334324034435038020', CAST(N'2023-10-31T17:36:38.4480753' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (199, N'1334324278601577660', CAST(N'2023-10-31T18:19:02.8656348' AS DateTime2))
GO
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (200, N'1334324295302641850', CAST(N'2023-10-31T18:19:03.4779966' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (201, N'1334324279046783440', CAST(N'2023-10-31T18:19:08.9210872' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (202, N'1334324290142861580', CAST(N'2023-10-31T18:19:10.6567036' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (203, N'1334324288491348620', CAST(N'2023-10-31T18:19:10.8152796' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (204, N'1334324290393583970', CAST(N'2023-10-31T18:19:34.0804023' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (205, N'1334324297937640210', CAST(N'2023-10-31T18:20:14.2914891' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (206, N'1334324310935677550', CAST(N'2023-10-31T18:25:07.4703418' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (207, N'1334324311636298160', CAST(N'2023-10-31T18:25:08.8423842' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (208, N'1334324325953174470', CAST(N'2023-10-31T18:27:08.8092559' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (209, N'1334324326630708010', CAST(N'2023-10-31T18:27:12.6524146' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (210, N'1334324342052625100', CAST(N'2023-10-31T18:27:12.8459549' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (211, N'1334324551966502430', CAST(N'2023-10-31T19:04:11.1408650' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (212, N'1334324564179152230', CAST(N'2023-10-31T19:04:11.5914188' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (213, N'1334324558532369310', CAST(N'2023-10-31T19:04:16.6407353' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (214, N'1334324553336887740', CAST(N'2023-10-31T19:04:19.2212985' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (215, N'1334324562206986380', CAST(N'2023-10-31T19:04:19.4606595' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (216, N'1334324624701083730', CAST(N'2023-10-31T19:15:29.1077966' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (217, N'1334324614826232610', CAST(N'2023-10-31T19:15:36.2636542' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (218, N'1334324649087032120', CAST(N'2023-10-31T19:19:38.3592881' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (219, N'1334324639332890410', CAST(N'2023-10-31T19:19:39.0859780' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (220, N'1334324647098982460', CAST(N'2023-10-31T19:19:42.4189074' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (221, N'1334324655340952440', CAST(N'2023-10-31T19:19:45.3641600' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (222, N'1334324646187212000', CAST(N'2023-10-31T19:19:45.4170174' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (223, N'1334324652971151880', CAST(N'2023-10-31T19:20:56.7752832' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (224, N'1334324666654193260', CAST(N'2023-10-31T19:20:58.9267112' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (225, N'1334324659765634660', CAST(N'2023-10-31T19:22:05.3646257' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (226, N'1334324655580114830', CAST(N'2023-10-31T19:22:08.2280935' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (227, N'1334324675952178310', CAST(N'2023-10-31T19:22:58.9796441' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (228, N'1334324678784399500', CAST(N'2023-10-31T19:23:02.1401907' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (229, N'1334324694891735900', CAST(N'2023-10-31T19:27:43.2952853' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (230, N'1334324705612007160', CAST(N'2023-10-31T19:27:46.1535601' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (231, N'1334324729783123980', CAST(N'2023-10-31T19:32:03.9649600' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (232, N'1334324720068922340', CAST(N'2023-10-31T19:32:04.3918184' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (233, N'1334324722589800810', CAST(N'2023-10-31T19:32:08.5676185' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (234, N'1334324717600400510', CAST(N'2023-10-31T19:32:11.1666842' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (235, N'1334324728556264710', CAST(N'2023-10-31T19:32:11.4055955' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (236, N'1334324809514939750', CAST(N'2023-10-31T19:45:13.5415741' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (237, N'1334324803816398760', CAST(N'2023-10-31T19:45:37.9184461' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (238, N'1334324814240081700', CAST(N'2023-10-31T19:45:38.2166470' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (239, N'1334324812828463480', CAST(N'2023-10-31T19:46:06.3953424' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (240, N'1334324807264440200', CAST(N'2023-10-31T19:46:06.6895566' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (241, N'1334324808187831140', CAST(N'2023-10-31T19:47:06.5509839' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (242, N'1334324804337529460', CAST(N'2023-10-31T19:47:06.6896108' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (243, N'1334324816727896390', CAST(N'2023-10-31T19:47:17.1502140' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (244, N'1334324818592003410', CAST(N'2023-10-31T19:48:04.5889865' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (245, N'1334324818530966240', CAST(N'2023-10-31T19:48:21.8483271' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (246, N'1334324815306828350', CAST(N'2023-10-31T19:48:26.1207861' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (247, N'1334324824621211250', CAST(N'2023-10-31T19:48:51.6365652' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (248, N'1334324825374713950', CAST(N'2023-10-31T19:50:00.0641319' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (249, N'1334338928628954610', CAST(N'2023-11-02T11:01:22.3894108' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (250, N'1334338932403928780', CAST(N'2023-11-02T11:01:23.7629305' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (251, N'1334338947764903370', CAST(N'2023-11-02T11:01:24.6346296' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (252, N'1334349699574467230', CAST(N'2023-11-03T16:55:56.9309110' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (253, N'1334349697241929970', CAST(N'2023-11-03T16:56:04.6357136' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (254, N'1334349715684238780', CAST(N'2023-11-03T16:56:05.2120259' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (255, N'1334349723060926190', CAST(N'2023-11-03T16:59:26.2826105' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (256, N'1334349732479396430', CAST(N'2023-11-03T16:59:28.4029198' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (257, N'1334349777463849260', CAST(N'2023-11-03T17:08:32.9229301' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (258, N'1334349792127644720', CAST(N'2023-11-03T17:11:09.0992452' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (259, N'1334349818497256310', CAST(N'2023-11-03T17:13:02.1536639' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (260, N'1334349801100835760', CAST(N'2023-11-03T17:13:17.7981433' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (261, N'1334349801579297550', CAST(N'2023-11-03T17:13:18.6029530' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (262, N'1334349804397881560', CAST(N'2023-11-03T17:13:26.5124541' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (263, N'1334349800948789530', CAST(N'2023-11-03T17:13:26.6112239' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (264, N'1334349813468914590', CAST(N'2023-11-03T17:13:50.4086872' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (265, N'1334349821402752740', CAST(N'2023-11-03T17:13:50.6450547' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (266, N'1334349819578520110', CAST(N'2023-11-03T17:14:55.9973684' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (267, N'1334349830222971100', CAST(N'2023-11-03T17:16:58.7567104' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (268, N'1334349831352267170', CAST(N'2023-11-03T17:18:27.5387759' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (269, N'1334349846526914200', CAST(N'2023-11-03T17:19:00.6798229' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (270, N'1334349842732577840', CAST(N'2023-11-03T17:19:00.8024961' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (271, N'1334349852530741660', CAST(N'2023-11-03T17:19:05.1180562' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (272, N'1334349846028031460', CAST(N'2023-11-03T17:19:05.5638636' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (273, N'1334349856123297900', CAST(N'2023-11-03T17:20:36.0451060' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (274, N'1334349851061847230', CAST(N'2023-11-03T17:20:36.2375917' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (275, N'1334349844756761180', CAST(N'2023-11-03T17:20:39.1796038' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (276, N'1334349847972947290', CAST(N'2023-11-03T17:21:12.6273429' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (277, N'1334349861873459210', CAST(N'2023-11-03T17:21:14.5488094' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (278, N'1334357720622975340', CAST(N'2023-11-04T15:14:13.4987432' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (279, N'1334357734053566910', CAST(N'2023-11-04T15:14:15.9102036' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (280, N'1334357746931775430', CAST(N'2023-11-04T15:14:17.4008163' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (281, N'1334358492702789190', CAST(N'2023-11-04T17:20:26.3017431' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (282, N'1334358486510724250', CAST(N'2023-11-04T17:20:28.6614328' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (283, N'1334358586188598630', CAST(N'2023-11-04T17:35:04.4809753' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (284, N'1334358573739764120', CAST(N'2023-11-04T17:35:05.2130174' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (285, N'1334358574769282780', CAST(N'2023-11-04T17:35:10.3472857' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (286, N'1334358586807568630', CAST(N'2023-11-04T17:35:18.2052711' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (287, N'1334358577833822590', CAST(N'2023-11-04T17:35:18.4466331' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (288, N'1334358582459318980', CAST(N'2023-11-04T17:35:36.7426951' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (289, N'1334358584325519310', CAST(N'2023-11-04T17:35:39.0186140' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (290, N'1334358828549371360', CAST(N'2023-11-04T18:17:40.7570573' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (291, N'1334358837415438130', CAST(N'2023-11-04T18:17:43.9056372' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (292, N'1334358918406367910', CAST(N'2023-11-04T18:31:25.7809335' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (293, N'1334358934225923340', CAST(N'2023-11-04T18:33:56.9494470' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (294, N'1334358926830551270', CAST(N'2023-11-04T18:33:57.7752986' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (295, N'1334359226475647580', CAST(N'2023-11-04T19:22:07.9076855' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (296, N'1334359221875713270', CAST(N'2023-11-04T19:23:03.8859798' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (297, N'1334359232261376340', CAST(N'2023-11-04T19:23:23.0646866' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (298, N'1334359239356184300', CAST(N'2023-11-04T19:23:24.9855501' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (299, N'1334359240017963880', CAST(N'2023-11-04T19:23:25.2398708' AS DateTime2))
GO
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (300, N'1334359223554614650', CAST(N'2023-11-04T19:23:30.5367039' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (301, N'1334359239224569990', CAST(N'2023-11-04T19:23:32.2830349' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (302, N'1334359280786231580', CAST(N'2023-11-04T19:32:43.6231643' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (303, N'1334359294798637070', CAST(N'2023-11-04T19:32:45.9868432' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (304, N'1334359331831732340', CAST(N'2023-11-04T19:41:18.5507264' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (305, N'1334359346596224290', CAST(N'2023-11-04T19:41:19.1454390' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (306, N'1334359357806021780', CAST(N'2023-11-04T19:44:13.9149864' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (307, N'1334359353868082970', CAST(N'2023-11-04T19:44:15.0629177' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (308, N'1334359359233208380', CAST(N'2023-11-04T19:44:15.3102556' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (309, N'1334359357214518020', CAST(N'2023-11-04T19:44:15.5256798' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (310, N'1334359346583463240', CAST(N'2023-11-04T19:44:23.5990873' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (311, N'1334359365977282440', CAST(N'2023-11-04T19:44:25.4002703' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (312, N'1334359351040427170', CAST(N'2023-11-04T19:44:25.5528630' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (313, N'1334359368786906430', CAST(N'2023-11-04T19:46:31.3076644' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (314, N'1334359380137557550', CAST(N'2023-11-04T19:46:31.7833923' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (315, N'1334359373758946230', CAST(N'2023-11-04T19:46:32.0287372' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (316, N'1334359380372769630', CAST(N'2023-11-04T19:46:48.3611186' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (317, N'1334359379830261570', CAST(N'2023-11-04T19:46:48.7680329' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (318, N'1334359380285195240', CAST(N'2023-11-04T19:46:49.0642382' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (319, N'1334359371540598700', CAST(N'2023-11-04T19:46:51.2938787' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (320, N'1334359369041988090', CAST(N'2023-11-04T19:46:53.2012782' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (321, N'1334359379731225500', CAST(N'2023-11-04T19:46:53.4186951' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (322, N'1334359384181628540', CAST(N'2023-11-04T19:47:21.2891455' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (323, N'1334359378977407360', CAST(N'2023-11-04T19:47:21.7748474' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (324, N'1334359443591609170', CAST(N'2023-11-04T19:58:39.4550986' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (325, N'1334359442889922900', CAST(N'2023-11-04T19:58:58.4782192' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (326, N'1334359442980012420', CAST(N'2023-11-04T19:59:08.4635162' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (327, N'1334359462972244180', CAST(N'2023-11-04T20:02:54.5635952' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (328, N'1334359469025443640', CAST(N'2023-11-04T20:02:54.8448441' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (329, N'1334359467475537390', CAST(N'2023-11-04T20:03:03.8637239' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (330, N'1334359468317121410', CAST(N'2023-11-04T20:03:04.0801447' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (331, N'1334359463107240390', CAST(N'2023-11-04T20:03:04.4242238' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (332, N'1334359461040171210', CAST(N'2023-11-04T20:03:04.5508854' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (333, N'1334359460851264880', CAST(N'2023-11-04T20:03:06.6213477' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (334, N'1334359463112569480', CAST(N'2023-11-04T20:03:06.8627036' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (335, N'1334359465243430580', CAST(N'2023-11-04T20:03:08.9930047' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (336, N'1334359473030084140', CAST(N'2023-11-04T20:03:10.8839493' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (337, N'1334359487837077760', CAST(N'2023-11-04T20:06:58.6742241' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (338, N'1334360709976195340', CAST(N'2023-11-04T23:29:30.5397429' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (339, N'1334360694811187880', CAST(N'2023-11-04T23:29:30.5387457' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (340, N'1334360708203360690', CAST(N'2023-11-04T23:29:30.5387457' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (341, N'1334360711695627810', CAST(N'2023-11-04T23:29:32.9239620' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (342, N'1334360701681435730', CAST(N'2023-11-04T23:29:32.9259563' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (343, N'1334360698234384150', CAST(N'2023-11-04T23:29:33.1893251' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (344, N'1334360697749286130', CAST(N'2023-11-04T23:29:34.3997047' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (345, N'1334360700719541680', CAST(N'2023-11-04T23:29:34.4026974' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (346, N'1334360776996707380', CAST(N'2023-11-04T23:42:01.2133966' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (347, N'1334360787181159030', CAST(N'2023-11-04T23:42:03.0297370' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (348, N'1334360852493608500', CAST(N'2023-11-04T23:54:06.4971878' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (349, N'1334360853958573960', CAST(N'2023-11-04T23:54:07.3030301' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (350, N'1334365691439640050', CAST(N'2023-11-05T13:19:06.7116549' AS DateTime2))
INSERT [dbo].[TokensTable] ([Id], [Token], [CreateDate]) VALUES (351, N'1334365684235152900', CAST(N'2023-11-05T13:19:08.2679383' AS DateTime2))
SET IDENTITY_INSERT [dbo].[TokensTable] OFF
GO
SET IDENTITY_INSERT [dbo].[Unit] ON 

INSERT [dbo].[Unit] ([UnitId], [Name], [Notes], [IsActive], [CreateDate], [CreateUserId], [UpdateUserId], [UpdateDate]) VALUES (2, N'قطعة', N'للتعبير عن أصغر وحدة', 1, CAST(N'2023-10-30T23:25:09.5895863' AS DateTime2), 0, 0, CAST(N'2023-10-30T23:25:09.5915795' AS DateTime2))
INSERT [dbo].[Unit] ([UnitId], [Name], [Notes], [IsActive], [CreateDate], [CreateUserId], [UpdateUserId], [UpdateDate]) VALUES (3, N'متر', N'', 1, CAST(N'2023-10-30T23:32:26.3881760' AS DateTime2), 0, 0, CAST(N'2023-10-30T23:32:26.3911687' AS DateTime2))
INSERT [dbo].[Unit] ([UnitId], [Name], [Notes], [IsActive], [CreateDate], [CreateUserId], [UpdateUserId], [UpdateDate]) VALUES (4, N'بكرة', N'', 1, CAST(N'2023-10-30T23:35:14.2486923' AS DateTime2), 0, 0, CAST(N'2023-10-30T23:35:14.2556763' AS DateTime2))
INSERT [dbo].[Unit] ([UnitId], [Name], [Notes], [IsActive], [CreateDate], [CreateUserId], [UpdateUserId], [UpdateDate]) VALUES (5, N'علبة', N'', 1, CAST(N'2023-10-30T23:40:29.4385546' AS DateTime2), 0, 0, CAST(N'2023-10-30T23:40:29.4445386' AS DateTime2))
INSERT [dbo].[Unit] ([UnitId], [Name], [Notes], [IsActive], [CreateDate], [CreateUserId], [UpdateUserId], [UpdateDate]) VALUES (6, N'صندوق', N'', 1, CAST(N'2023-10-30T23:40:34.3156733' AS DateTime2), 0, 0, CAST(N'2023-10-30T23:40:34.3166723' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Unit] OFF
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
ALTER TABLE [dbo].[POS]  WITH CHECK ADD  CONSTRAINT [FK_POS_Branch] FOREIGN KEY([BranchId])
REFERENCES [dbo].[Branch] ([BranchId])
GO
ALTER TABLE [dbo].[POS] CHECK CONSTRAINT [FK_POS_Branch]
GO
ALTER TABLE [dbo].[PurchaseInvoice]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseInvoice_Supplier] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([SupplierId])
GO
ALTER TABLE [dbo].[PurchaseInvoice] CHECK CONSTRAINT [FK_PurchaseInvoice_Supplier]
GO
ALTER TABLE [dbo].[PurInvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_PurInvoiceItem_ItemUnit] FOREIGN KEY([ItemUnitId])
REFERENCES [dbo].[ItemUnit] ([ItemUnitId])
GO
ALTER TABLE [dbo].[PurInvoiceItem] CHECK CONSTRAINT [FK_PurInvoiceItem_ItemUnit]
GO
ALTER TABLE [dbo].[PurInvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_PurInvoiceItem_PurchaseInvoice] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[PurchaseInvoice] ([InvoiceId])
GO
ALTER TABLE [dbo].[PurInvoiceItem] CHECK CONSTRAINT [FK_PurInvoiceItem_PurchaseInvoice]
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
/****** Object:  Trigger [dbo].[TokensTable_Trg]    Script Date: 11/5/2023 1:37:34 PM ******/
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
