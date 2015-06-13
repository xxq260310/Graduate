
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/13/2015 14:55:09
-- Generated from EDMX file: F:\Graduate\Market\Market.DAL\MarketModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Market];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserProfileRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserProfiles] DROP CONSTRAINT [FK_UserProfileRole];
GO
IF OBJECT_ID(N'[dbo].[FK_SubCategoryParentCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubCategories] DROP CONSTRAINT [FK_SubCategoryParentCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_CommoditySubCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Commodities] DROP CONSTRAINT [FK_CommoditySubCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_CommodityManufacturer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Commodities] DROP CONSTRAINT [FK_CommodityManufacturer];
GO
IF OBJECT_ID(N'[dbo].[FK_FeedbackCommodity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Feedbacks] DROP CONSTRAINT [FK_FeedbackCommodity];
GO
IF OBJECT_ID(N'[dbo].[FK_FeedbackUserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Feedbacks] DROP CONSTRAINT [FK_FeedbackUserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteFeedbackUserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiteFeedbacks] DROP CONSTRAINT [FK_SiteFeedbackUserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_RequiredCommodityUserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequiredCommodities] DROP CONSTRAINT [FK_RequiredCommodityUserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProfileUserProfileCommodity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserProfileCommodities] DROP CONSTRAINT [FK_UserProfileUserProfileCommodity];
GO
IF OBJECT_ID(N'[dbo].[FK_CommodityUserProfileCommodity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserProfileCommodities] DROP CONSTRAINT [FK_CommodityUserProfileCommodity];
GO
IF OBJECT_ID(N'[dbo].[FK_CommodityCommodityInShoppingTrolley]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommodityInShoppingTrolleys] DROP CONSTRAINT [FK_CommodityCommodityInShoppingTrolley];
GO
IF OBJECT_ID(N'[dbo].[FK_ShoppingTrolleyCommodityInShoppingTrolley]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommodityInShoppingTrolleys] DROP CONSTRAINT [FK_ShoppingTrolleyCommodityInShoppingTrolley];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProfileShoppingTrolley]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ShoppingTrolleys] DROP CONSTRAINT [FK_UserProfileShoppingTrolley];
GO
IF OBJECT_ID(N'[dbo].[FK_CommodityCommodityInFavorite]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommodityInFavorites] DROP CONSTRAINT [FK_CommodityCommodityInFavorite];
GO
IF OBJECT_ID(N'[dbo].[FK_FavoriteCommodityInFavorite]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommodityInFavorites] DROP CONSTRAINT [FK_FavoriteCommodityInFavorite];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProfileFavorite]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Favorites] DROP CONSTRAINT [FK_UserProfileFavorite];
GO
IF OBJECT_ID(N'[dbo].[FK_CommodityInfoCommodity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommodityInfoes] DROP CONSTRAINT [FK_CommodityInfoCommodity];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProfileOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_UserProfileOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_CommodityInOrderCommodity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommodityInOrders] DROP CONSTRAINT [FK_CommodityInOrderCommodity];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderCommodityInOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommodityInOrders] DROP CONSTRAINT [FK_OrderCommodityInOrder];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserProfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProfiles];
GO
IF OBJECT_ID(N'[dbo].[Commodities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Commodities];
GO
IF OBJECT_ID(N'[dbo].[ParentCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParentCategories];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[ShoppingTrolleys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShoppingTrolleys];
GO
IF OBJECT_ID(N'[dbo].[SubCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubCategories];
GO
IF OBJECT_ID(N'[dbo].[Announcements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Announcements];
GO
IF OBJECT_ID(N'[dbo].[Favorites]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Favorites];
GO
IF OBJECT_ID(N'[dbo].[RequiredCommodities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequiredCommodities];
GO
IF OBJECT_ID(N'[dbo].[Manufacturers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Manufacturers];
GO
IF OBJECT_ID(N'[dbo].[SiteFeedbacks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SiteFeedbacks];
GO
IF OBJECT_ID(N'[dbo].[Feedbacks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Feedbacks];
GO
IF OBJECT_ID(N'[dbo].[CommodityInOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommodityInOrders];
GO
IF OBJECT_ID(N'[dbo].[UserProfileCommodities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProfileCommodities];
GO
IF OBJECT_ID(N'[dbo].[CommodityInShoppingTrolleys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommodityInShoppingTrolleys];
GO
IF OBJECT_ID(N'[dbo].[CommodityInFavorites]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommodityInFavorites];
GO
IF OBJECT_ID(N'[dbo].[CommodityInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommodityInfoes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserProfiles'
CREATE TABLE [dbo].[UserProfiles] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserName] varchar(12)  NULL,
    [RoleId] int  NOT NULL,
    [Nickname] varchar(12)  NULL,
    [Sex] varchar(6)  NULL,
    [Email] varchar(20)  NULL,
    [Password] varchar(max)  NOT NULL,
    [Contact] char(11)  NULL,
    [Address] varchar(100)  NULL,
    [ImageData] varbinary(max)  NULL,
    [ImageType] nvarchar(max)  NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Commodities'
CREATE TABLE [dbo].[Commodities] (
    [CommodityId] int IDENTITY(1,1) NOT NULL,
    [CommodityName] varchar(30)  NULL,
    [SubCategoryId] int  NOT NULL,
    [ManufacturerId] int  NOT NULL,
    [UnitPrice] float  NULL,
    [SubPrice] float  NULL,
    [DiscountPrice] float  NULL,
    [MakeCompany] varchar(20)  NULL,
    [Degree] char(6)  NULL,
    [Quantity] int  NULL,
    [Description] varchar(max)  NULL,
    [Brand] varchar(10)  NULL,
    [IsRecommended] bit  NULL,
    [ManufacturerDate] datetime  NULL,
    [PromotionStartOn] datetime  NULL,
    [PromotionEndOn] datetime  NULL,
    [Weight] varchar(10)  NULL,
    [Component] varchar(500)  NULL,
    [ImageData] varbinary(max)  NULL,
    [ImageType] nvarchar(max)  NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'ParentCategories'
CREATE TABLE [dbo].[ParentCategories] (
    [ParentCategoryId] int IDENTITY(1,1) NOT NULL,
    [CategoryName] varchar(20)  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleId] int IDENTITY(1,1) NOT NULL,
    [RoleName] varchar(20)  NULL,
    [Description] varchar(max)  NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [OrderId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Contact] char(11)  NULL,
    [TotalCost] float  NULL,
    [Address] varchar(50)  NULL,
    [Email] nvarchar(max)  NULL,
    [State] varchar(20)  NULL,
    [Delivery] varchar(20)  NULL,
    [ConsigneeName] varchar(10)  NULL,
    [Payfor] varchar(20)  NULL,
    [Province] varchar(20)  NULL,
    [City] varchar(20)  NULL,
    [Town] varchar(20)  NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL,
    [Postage] float  NULL
);
GO

-- Creating table 'ShoppingTrolleys'
CREATE TABLE [dbo].[ShoppingTrolleys] (
    [UserId] int  NOT NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'SubCategories'
CREATE TABLE [dbo].[SubCategories] (
    [SubCategoryId] int IDENTITY(1,1) NOT NULL,
    [CategoryName] varchar(20)  NULL,
    [ParentCategoryId] int  NOT NULL
);
GO

-- Creating table 'Announcements'
CREATE TABLE [dbo].[Announcements] (
    [AnnouncementId] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(50)  NULL,
    [Content] varchar(4000)  NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Favorites'
CREATE TABLE [dbo].[Favorites] (
    [UserId] int  NOT NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'RequiredCommodities'
CREATE TABLE [dbo].[RequiredCommodities] (
    [RequiredCommodityId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Content] varchar(4000)  NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL,
    [CommodityName] varchar(30)  NULL,
    [Price] float  NULL
);
GO

-- Creating table 'Manufacturers'
CREATE TABLE [dbo].[Manufacturers] (
    [ManufacturerId] int IDENTITY(1,1) NOT NULL,
    [ManufacturerName] varchar(20)  NULL,
    [Description] varchar(max)  NULL
);
GO

-- Creating table 'SiteFeedbacks'
CREATE TABLE [dbo].[SiteFeedbacks] (
    [SiteFeedbackId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Title] varchar(50)  NULL,
    [Content] varchar(4000)  NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Feedbacks'
CREATE TABLE [dbo].[Feedbacks] (
    [FeedbackId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [CommodityId] int  NOT NULL,
    [Title] varchar(50)  NULL,
    [Content] varchar(4000)  NULL,
    [Reply] varchar(4000)  NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'CommodityInOrders'
CREATE TABLE [dbo].[CommodityInOrders] (
    [OrderId] int  NOT NULL,
    [CommodityId] int  NOT NULL,
    [UnitPrice] float  NULL,
    [Quantity] int  NULL,
    [Color] varchar(10)  NULL,
    [Size] varchar(10)  NULL,
    [Capacity] varchar(10)  NULL,
    [SellerId] int  NULL
);
GO

-- Creating table 'UserProfileCommodities'
CREATE TABLE [dbo].[UserProfileCommodities] (
    [UserId] int  NOT NULL,
    [CommodityId] int  NOT NULL
);
GO

-- Creating table 'CommodityInShoppingTrolleys'
CREATE TABLE [dbo].[CommodityInShoppingTrolleys] (
    [UserId] int  NOT NULL,
    [CommodityId] int  NOT NULL,
    [UnitPrice] float  NULL,
    [Quantity] int  NULL,
    [Color] varchar(10)  NULL,
    [Size] varchar(10)  NULL,
    [Capacity] varchar(10)  NULL
);
GO

-- Creating table 'CommodityInFavorites'
CREATE TABLE [dbo].[CommodityInFavorites] (
    [UserId] int  NOT NULL,
    [CommodityId] int  NOT NULL
);
GO

-- Creating table 'CommodityInfoes'
CREATE TABLE [dbo].[CommodityInfoes] (
    [CommodityInfoId] int IDENTITY(1,1) NOT NULL,
    [CommodityId] int  NOT NULL,
    [Color] varchar(10)  NULL,
    [Size] varchar(10)  NULL,
    [Quantity] int  NULL,
    [Capacity] varchar(10)  NULL,
    [ImageType] nvarchar(max)  NULL,
    [ImageData] varbinary(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserId] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles]
ADD CONSTRAINT [PK_UserProfiles]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [CommodityId] in table 'Commodities'
ALTER TABLE [dbo].[Commodities]
ADD CONSTRAINT [PK_Commodities]
    PRIMARY KEY CLUSTERED ([CommodityId] ASC);
GO

-- Creating primary key on [ParentCategoryId] in table 'ParentCategories'
ALTER TABLE [dbo].[ParentCategories]
ADD CONSTRAINT [PK_ParentCategories]
    PRIMARY KEY CLUSTERED ([ParentCategoryId] ASC);
GO

-- Creating primary key on [RoleId] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleId] ASC);
GO

-- Creating primary key on [OrderId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([OrderId] ASC);
GO

-- Creating primary key on [UserId] in table 'ShoppingTrolleys'
ALTER TABLE [dbo].[ShoppingTrolleys]
ADD CONSTRAINT [PK_ShoppingTrolleys]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [SubCategoryId] in table 'SubCategories'
ALTER TABLE [dbo].[SubCategories]
ADD CONSTRAINT [PK_SubCategories]
    PRIMARY KEY CLUSTERED ([SubCategoryId] ASC);
GO

-- Creating primary key on [AnnouncementId] in table 'Announcements'
ALTER TABLE [dbo].[Announcements]
ADD CONSTRAINT [PK_Announcements]
    PRIMARY KEY CLUSTERED ([AnnouncementId] ASC);
GO

-- Creating primary key on [UserId] in table 'Favorites'
ALTER TABLE [dbo].[Favorites]
ADD CONSTRAINT [PK_Favorites]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [RequiredCommodityId] in table 'RequiredCommodities'
ALTER TABLE [dbo].[RequiredCommodities]
ADD CONSTRAINT [PK_RequiredCommodities]
    PRIMARY KEY CLUSTERED ([RequiredCommodityId] ASC);
GO

-- Creating primary key on [ManufacturerId] in table 'Manufacturers'
ALTER TABLE [dbo].[Manufacturers]
ADD CONSTRAINT [PK_Manufacturers]
    PRIMARY KEY CLUSTERED ([ManufacturerId] ASC);
GO

-- Creating primary key on [SiteFeedbackId] in table 'SiteFeedbacks'
ALTER TABLE [dbo].[SiteFeedbacks]
ADD CONSTRAINT [PK_SiteFeedbacks]
    PRIMARY KEY CLUSTERED ([SiteFeedbackId] ASC);
GO

-- Creating primary key on [FeedbackId] in table 'Feedbacks'
ALTER TABLE [dbo].[Feedbacks]
ADD CONSTRAINT [PK_Feedbacks]
    PRIMARY KEY CLUSTERED ([FeedbackId] ASC);
GO

-- Creating primary key on [CommodityId], [OrderId] in table 'CommodityInOrders'
ALTER TABLE [dbo].[CommodityInOrders]
ADD CONSTRAINT [PK_CommodityInOrders]
    PRIMARY KEY CLUSTERED ([CommodityId], [OrderId] ASC);
GO

-- Creating primary key on [UserId], [CommodityId] in table 'UserProfileCommodities'
ALTER TABLE [dbo].[UserProfileCommodities]
ADD CONSTRAINT [PK_UserProfileCommodities]
    PRIMARY KEY CLUSTERED ([UserId], [CommodityId] ASC);
GO

-- Creating primary key on [CommodityId], [UserId] in table 'CommodityInShoppingTrolleys'
ALTER TABLE [dbo].[CommodityInShoppingTrolleys]
ADD CONSTRAINT [PK_CommodityInShoppingTrolleys]
    PRIMARY KEY CLUSTERED ([CommodityId], [UserId] ASC);
GO

-- Creating primary key on [CommodityId], [UserId] in table 'CommodityInFavorites'
ALTER TABLE [dbo].[CommodityInFavorites]
ADD CONSTRAINT [PK_CommodityInFavorites]
    PRIMARY KEY CLUSTERED ([CommodityId], [UserId] ASC);
GO

-- Creating primary key on [CommodityInfoId] in table 'CommodityInfoes'
ALTER TABLE [dbo].[CommodityInfoes]
ADD CONSTRAINT [PK_CommodityInfoes]
    PRIMARY KEY CLUSTERED ([CommodityInfoId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RoleId] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles]
ADD CONSTRAINT [FK_UserProfileRole]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileRole'
CREATE INDEX [IX_FK_UserProfileRole]
ON [dbo].[UserProfiles]
    ([RoleId]);
GO

-- Creating foreign key on [ParentCategoryId] in table 'SubCategories'
ALTER TABLE [dbo].[SubCategories]
ADD CONSTRAINT [FK_SubCategoryParentCategory]
    FOREIGN KEY ([ParentCategoryId])
    REFERENCES [dbo].[ParentCategories]
        ([ParentCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubCategoryParentCategory'
CREATE INDEX [IX_FK_SubCategoryParentCategory]
ON [dbo].[SubCategories]
    ([ParentCategoryId]);
GO

-- Creating foreign key on [SubCategoryId] in table 'Commodities'
ALTER TABLE [dbo].[Commodities]
ADD CONSTRAINT [FK_CommoditySubCategory]
    FOREIGN KEY ([SubCategoryId])
    REFERENCES [dbo].[SubCategories]
        ([SubCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommoditySubCategory'
CREATE INDEX [IX_FK_CommoditySubCategory]
ON [dbo].[Commodities]
    ([SubCategoryId]);
GO

-- Creating foreign key on [ManufacturerId] in table 'Commodities'
ALTER TABLE [dbo].[Commodities]
ADD CONSTRAINT [FK_CommodityManufacturer]
    FOREIGN KEY ([ManufacturerId])
    REFERENCES [dbo].[Manufacturers]
        ([ManufacturerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommodityManufacturer'
CREATE INDEX [IX_FK_CommodityManufacturer]
ON [dbo].[Commodities]
    ([ManufacturerId]);
GO

-- Creating foreign key on [CommodityId] in table 'Feedbacks'
ALTER TABLE [dbo].[Feedbacks]
ADD CONSTRAINT [FK_FeedbackCommodity]
    FOREIGN KEY ([CommodityId])
    REFERENCES [dbo].[Commodities]
        ([CommodityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FeedbackCommodity'
CREATE INDEX [IX_FK_FeedbackCommodity]
ON [dbo].[Feedbacks]
    ([CommodityId]);
GO

-- Creating foreign key on [UserId] in table 'Feedbacks'
ALTER TABLE [dbo].[Feedbacks]
ADD CONSTRAINT [FK_FeedbackUserProfile]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FeedbackUserProfile'
CREATE INDEX [IX_FK_FeedbackUserProfile]
ON [dbo].[Feedbacks]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'SiteFeedbacks'
ALTER TABLE [dbo].[SiteFeedbacks]
ADD CONSTRAINT [FK_SiteFeedbackUserProfile]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteFeedbackUserProfile'
CREATE INDEX [IX_FK_SiteFeedbackUserProfile]
ON [dbo].[SiteFeedbacks]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'RequiredCommodities'
ALTER TABLE [dbo].[RequiredCommodities]
ADD CONSTRAINT [FK_RequiredCommodityUserProfile]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RequiredCommodityUserProfile'
CREATE INDEX [IX_FK_RequiredCommodityUserProfile]
ON [dbo].[RequiredCommodities]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'UserProfileCommodities'
ALTER TABLE [dbo].[UserProfileCommodities]
ADD CONSTRAINT [FK_UserProfileUserProfileCommodity]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CommodityId] in table 'UserProfileCommodities'
ALTER TABLE [dbo].[UserProfileCommodities]
ADD CONSTRAINT [FK_CommodityUserProfileCommodity]
    FOREIGN KEY ([CommodityId])
    REFERENCES [dbo].[Commodities]
        ([CommodityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommodityUserProfileCommodity'
CREATE INDEX [IX_FK_CommodityUserProfileCommodity]
ON [dbo].[UserProfileCommodities]
    ([CommodityId]);
GO

-- Creating foreign key on [CommodityId] in table 'CommodityInShoppingTrolleys'
ALTER TABLE [dbo].[CommodityInShoppingTrolleys]
ADD CONSTRAINT [FK_CommodityCommodityInShoppingTrolley]
    FOREIGN KEY ([CommodityId])
    REFERENCES [dbo].[Commodities]
        ([CommodityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'CommodityInShoppingTrolleys'
ALTER TABLE [dbo].[CommodityInShoppingTrolleys]
ADD CONSTRAINT [FK_ShoppingTrolleyCommodityInShoppingTrolley]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[ShoppingTrolleys]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ShoppingTrolleyCommodityInShoppingTrolley'
CREATE INDEX [IX_FK_ShoppingTrolleyCommodityInShoppingTrolley]
ON [dbo].[CommodityInShoppingTrolleys]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'ShoppingTrolleys'
ALTER TABLE [dbo].[ShoppingTrolleys]
ADD CONSTRAINT [FK_UserProfileShoppingTrolley]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CommodityId] in table 'CommodityInFavorites'
ALTER TABLE [dbo].[CommodityInFavorites]
ADD CONSTRAINT [FK_CommodityCommodityInFavorite]
    FOREIGN KEY ([CommodityId])
    REFERENCES [dbo].[Commodities]
        ([CommodityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'CommodityInFavorites'
ALTER TABLE [dbo].[CommodityInFavorites]
ADD CONSTRAINT [FK_FavoriteCommodityInFavorite]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Favorites]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FavoriteCommodityInFavorite'
CREATE INDEX [IX_FK_FavoriteCommodityInFavorite]
ON [dbo].[CommodityInFavorites]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Favorites'
ALTER TABLE [dbo].[Favorites]
ADD CONSTRAINT [FK_UserProfileFavorite]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CommodityId] in table 'CommodityInfoes'
ALTER TABLE [dbo].[CommodityInfoes]
ADD CONSTRAINT [FK_CommodityInfoCommodity]
    FOREIGN KEY ([CommodityId])
    REFERENCES [dbo].[Commodities]
        ([CommodityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommodityInfoCommodity'
CREATE INDEX [IX_FK_CommodityInfoCommodity]
ON [dbo].[CommodityInfoes]
    ([CommodityId]);
GO

-- Creating foreign key on [UserId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_UserProfileOrder]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileOrder'
CREATE INDEX [IX_FK_UserProfileOrder]
ON [dbo].[Orders]
    ([UserId]);
GO

-- Creating foreign key on [CommodityId] in table 'CommodityInOrders'
ALTER TABLE [dbo].[CommodityInOrders]
ADD CONSTRAINT [FK_CommodityInOrderCommodity]
    FOREIGN KEY ([CommodityId])
    REFERENCES [dbo].[Commodities]
        ([CommodityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [OrderId] in table 'CommodityInOrders'
ALTER TABLE [dbo].[CommodityInOrders]
ADD CONSTRAINT [FK_OrderCommodityInOrder]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([OrderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderCommodityInOrder'
CREATE INDEX [IX_FK_OrderCommodityInOrder]
ON [dbo].[CommodityInOrders]
    ([OrderId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------