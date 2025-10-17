/*============================================================================
  File:     adven.sql

  Summary:  Creates additional tables and objects for the workshop mnsg. It comes from Microsoft AdventureWorks sample database. Run this on any version of SQL Server (2008R2 or later) to get AdventureWorks for your
  current version.  

  Date:     October 26, 2017
  Updated:  October 26, 2017

------------------------------------------------------------------------------
  This file is part of the Microsoft SQL Server Code Samples.

  Copyright (C) Microsoft Corporation.  All rights reserved.

  This source code is intended only as a supplement to Microsoft
  Development Tools and/or on-line documentation.  See these other
  materials for detailed information regarding Microsoft code samples.

  All data in this database is ficticious.

============================================================================*/
  
-- ------------ Write DROP-ROUTINE-stage scripts -----------

USE [BobsUsedBookStore]
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspUpdateAuthorLogin]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspUpdateAuthorLogin];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspUpdateAuthorPersonalInfo]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspUpdateAuthorPersonalInfo];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspGetBillOfMaterials]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspGetBillOfMaterials];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspGetAuthorManagers]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspGetAuthorManagers];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspGetManagerAuthors]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspGetManagerAuthors];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspGetWhereUsedProductID]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspGetWhereUsedProductID];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspLogError]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspLogError];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspPrintError]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspPrintError];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspSearchCandidateResumes]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspSearchCandidateResumes];
GO

IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspDeleteAuthor]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspDeleteAuthor];
GO

IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[uspGetProductData]') AND 
type = N'P')
DROP PROCEDURE [dbo].[uspGetProductData];
GO

IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetAccountingEndDate]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetAccountingEndDate];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetAccountingStartDate]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetAccountingStartDate];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetDocumentStatusText]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetDocumentStatusText];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetProductDealerPrice]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetProductDealerPrice];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetProductListPrice]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetProductListPrice];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetProductStandardCost]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetProductStandardCost];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetPurchaseOrderStatusText]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetPurchaseOrderStatusText];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetSalesOrderStatusText]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetSalesOrderStatusText];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetStock]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnGetStock];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnLeadingZeros]') AND 
type = N'FN')
DROP FUNCTION [dbo].[ufnLeadingZeros];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ufnGetContactInformation]') AND 
type = N'TF')
DROP FUNCTION [dbo].[ufnGetContactInformation];
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'ddlDatabaseTriggerLog' AND parent_class_desc = 'DATABASE')
BEGIN
    DROP TRIGGER [ddlDatabaseTriggerLog] ON DATABASE;
END
GO

-- Drop CONSTRAINT if exists      

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Book_ReferenceData_BookTypeId]'))
BEGIN
     ALTER TABLE [dbo].[Book] DROP CONSTRAINT [FK_Book_ReferenceData_BookTypeId]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Book_ReferenceData_ConditionId]'))
BEGIN
    ALTER TABLE [dbo].[Book] DROP CONSTRAINT [FK_Book_ReferenceData_ConditionId]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Book_ReferenceData_GenreId]'))
BEGIN
    ALTER TABLE [dbo].[Book] DROP CONSTRAINT [FK_Book_ReferenceData_GenreId]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Book_ReferenceData_PublisherId]'))
BEGIN
    ALTER TABLE [dbo].[Book] DROP CONSTRAINT [FK_Book_ReferenceData_PublisherId]
END
GO

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Product_DaysToManufacture' AND parent_object_id = OBJECT_ID('dbo.Product'))
BEGIN
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [CK_Product_DaysToManufacture];
END
GO

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Product_ListPrice' AND parent_object_id = OBJECT_ID('dbo.Product'))
BEGIN
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [CK_Product_ListPrice];
END
GO

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Product_ReorderPoint' AND parent_object_id = OBJECT_ID('dbo.Product'))
BEGIN
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [CK_Product_ReorderPoint];
END
GO

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Product_SafetyStockLevel' AND parent_object_id = OBJECT_ID('dbo.Product'))
BEGIN
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [CK_Product_SafetyStockLevel];
END
GO

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Product_SellEndDate' AND parent_object_id = OBJECT_ID('dbo.Product'))
BEGIN
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [CK_Product_SellEndDate];
END
GO

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Product_StandardCost' AND parent_object_id = OBJECT_ID('dbo.Product'))
BEGIN
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [CK_Product_StandardCost];
END
GO

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Product_Weight' AND parent_object_id = OBJECT_ID('dbo.Product'))
BEGIN
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [CK_Product_Weight];
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'PK_Product_ProductID' AND object_id = OBJECT_ID('dbo.Product'))
BEGIN
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [PK_Product_ProductID];
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'PK_BillOfMaterials_BillOfMaterialsID' AND object_id = OBJECT_ID('dbo.BillOfMaterials'))
BEGIN
    ALTER TABLE [dbo].[BillOfMaterials] DROP CONSTRAINT [PK_BillOfMaterials_BillOfMaterialsID];
END
GO

-- ------------ Write DROP-TABLE-stage scripts -----------

USE [BobsUsedBookStore]
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[Author]') AND 
type = N'U')
DROP TABLE [dbo].[Author];
GO

IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[Person]') AND 
type = N'U')
DROP TABLE [dbo].[Person];
GO

IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[BillOfMaterials]') AND 
type = N'U')
DROP TABLE [dbo].[BillOfMaterials];
GO

IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[Product]') AND 
type = N'U')
DROP TABLE [dbo].[Product];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[DatabaseLog]') AND 
type = N'U')
DROP TABLE [dbo].[DatabaseLog];
GO
            
IF  EXISTS (
SELECT * FROM sys.objects WHERE object_id = OBJECT_ID (N'[dbo].[ErrorLog]') AND 
type = N'U')
DROP TABLE [dbo].[ErrorLog];
GO
        
-- ------------ Write DROP-TYPE-stage scripts -----------

USE [BobsUsedBookStore]
GO

IF  EXISTS (
SELECT * FROM sys.types WHERE user_type_id = TYPE_ID(N'[dbo].[AccountNumber]') AND  is_user_defined = 1)
DROP TYPE [dbo].[AccountNumber];
GO
            
IF  EXISTS (
SELECT * FROM sys.types WHERE user_type_id = TYPE_ID(N'[dbo].[Flag]') AND  is_user_defined = 1)
DROP TYPE [dbo].[Flag];
GO
            
IF  EXISTS (
SELECT * FROM sys.types WHERE user_type_id = TYPE_ID(N'[dbo].[Name]') AND  is_user_defined = 1)
DROP TYPE [dbo].[Name];
GO
            
IF  EXISTS (
SELECT * FROM sys.types WHERE user_type_id = TYPE_ID(N'[dbo].[NameStyle]') AND  is_user_defined = 1)
DROP TYPE [dbo].[NameStyle];
GO
            
IF  EXISTS (
SELECT * FROM sys.types WHERE user_type_id = TYPE_ID(N'[dbo].[OrderNumber]') AND  is_user_defined = 1)
DROP TYPE [dbo].[OrderNumber];
GO
            
IF  EXISTS (
SELECT * FROM sys.types WHERE user_type_id = TYPE_ID(N'[dbo].[Phone]') AND  is_user_defined = 1)
DROP TYPE [dbo].[Phone];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND type = N'U'
)
DROP TABLE [dbo].[Members];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Shopping]') AND type = N'U'
)
DROP TABLE [dbo].[Shopping];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Coupons]') AND type = N'U'
)
DROP TABLE [dbo].[Coupons];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductSaleRegions]') AND type = N'U'
)
DROP TABLE [dbo].[ProductSaleRegions];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductSales]') AND type = N'U'
)
DROP TABLE [dbo].[ProductSales];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VwTopMembers]') AND type = N'V'
)
DROP VIEW [dbo].[VwTopMembers];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VwOpenCoupons]') AND type = N'V'
)
DROP VIEW [dbo].[VwOpenCoupons];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VwCustomerShopping]') AND type = N'V'
)
DROP VIEW [dbo].[VwCustomerShopping];
GO

IF EXISTS (
    SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VwRegionalSales]') AND type = N'V'
)
DROP VIEW [dbo].[VwRegionalSales];
GO

-- Drop stored procedure if it exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteOldCoupons]') AND type = N'P')
DROP PROCEDURE [dbo].[uspDeleteOldCoupons];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspProcessRefunds]') AND type = N'P')
DROP PROCEDURE [dbo].[uspProcessRefunds];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspShoppingLevelAmount]') AND type = N'P')
DROP PROCEDURE [dbo].[uspShoppingLevelAmount];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetTopRegion]') AND type = N'P')
DROP PROCEDURE [dbo].[uspGetTopRegion];
GO

-- Drop function if it exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufnCalculateCustomerLifetimeValue]') AND type = N'FN')
DROP FUNCTION [dbo].[ufnCalculateCustomerLifetimeValue];
GO
              
-- ------------ Write CREATE-TYPE-stage scripts -----------

USE [BobsUsedBookStore]
GO

CREATE TYPE [dbo].[AccountNumber]
FROM nvarchar(30) NULL;
GO
            
CREATE TYPE [dbo].[Flag]
FROM bit NOT NULL;
GO
            
CREATE TYPE [dbo].[Name]
FROM nvarchar(100) NULL;
GO
            
CREATE TYPE [dbo].[NameStyle]
FROM bit NOT NULL;
GO
            
CREATE TYPE [dbo].[OrderNumber]
FROM nvarchar(50) NULL;
GO
            
CREATE TYPE [dbo].[Phone]
FROM nvarchar(50) NULL;
GO      
            
-- ------------ Write CREATE-TABLE-stage scripts -----------

USE [BobsUsedBookStore]
GO
            
CREATE TABLE [dbo].[Author](
[BusinessEntityID] INT IDENTITY(1, 1) NOT NULL,
[NationalIDNumber] nvarchar(15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LoginID] nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrganizationNode] nvarchar(50) NULL,
[JobTitle] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BirthDate] date NOT NULL,
[MaritalStatus] nchar(1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Gender] nchar(1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HireDate] date NOT NULL,
[VacationHours] smallint NOT NULL DEFAULT ((0)),
[CurrentFlag] Flag NOT NULL DEFAULT ((1)),
[ModifiedDate] datetime NOT NULL DEFAULT (getdate())
)
ON [PRIMARY];
GO

CREATE TABLE [dbo].[Person](
[BusinessEntityID] int NOT NULL,
[PersonType] nchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NameStyle] NameStyle NOT NULL DEFAULT ((0)),
[Title] nvarchar(8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FirstName] Name NOT NULL,
[MiddleName] Name NULL,
[LastName] Name NOT NULL,
[Suffix] nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailPromotion] int NOT NULL DEFAULT ((0)),
[AdditionalContactInfo] xml NULL,
[Demographics] xml NULL,
[rowguid] uniqueidentifier ROWGUIDCOL NOT NULL DEFAULT (newid()),
[ModifiedDate] datetime NOT NULL DEFAULT (getdate())
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

CREATE TABLE [dbo].[BillOfMaterials](
[BillOfMaterialsID] int IDENTITY(1, 1) NOT NULL,
[ProductAssemblyID] int NULL,
[ComponentID] int NOT NULL,
[StartDate] datetime NOT NULL DEFAULT (getdate()),
[EndDate] datetime NULL,
[UnitMeasureCode] nchar(3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BOMLevel] smallint NOT NULL,
[PerAssemblyQty] decimal(8,2) NOT NULL DEFAULT ((1.00)),
[ModifiedDate] datetime NOT NULL DEFAULT (getdate())
)
ON [PRIMARY];
GO
            
CREATE TABLE [dbo].[Product](
[ProductID] int IDENTITY(1, 1) NOT NULL,
[Name] Name NOT NULL,
[ProductNumber] nvarchar(25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MakeFlag] Flag NOT NULL DEFAULT ((1)),
[FinishedGoodsFlag] Flag NOT NULL DEFAULT ((1)),
[Color] nvarchar(15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SafetyStockLevel] smallint NOT NULL,
[ReorderPoint] smallint NOT NULL,
[StandardCost] money NOT NULL,
[ListPrice] money NOT NULL,
[Size] nvarchar(5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SizeUnitMeasureCode] nchar(3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WeightUnitMeasureCode] nchar(3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Weight] decimal(8,2) NULL,
[DaysToManufacture] int NOT NULL,
[ProductLine] nchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Class] nchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Style] nchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ProductSubcategoryID] int NULL,
[ProductModelID] int NULL,
[SellStartDate] datetime NOT NULL,
[SellEndDate] datetime NULL,
[DiscontinuedDate] datetime NULL,
[rowguid] uniqueidentifier ROWGUIDCOL NOT NULL DEFAULT (newid()),
[ModifiedDate] datetime NOT NULL DEFAULT (getdate())
)
ON [PRIMARY];
GO

CREATE TABLE [dbo].[ErrorLog](
[ErrorLogID] int IDENTITY(1, 1) NOT NULL,
[ErrorTime] datetime NOT NULL DEFAULT (getdate()),
[UserName] nvarchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ErrorNumber] int NOT NULL,
[ErrorSeverity] int NULL,
[ErrorState] int NULL,
[ErrorProcedure] nvarchar(126) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ErrorLine] int NULL,
[ErrorMessage] nvarchar(4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
ON [PRIMARY];
GO

CREATE TABLE [dbo].[DatabaseLog](
[DatabaseLogID] int IDENTITY(1, 1) NOT NULL,
[PostTime] datetime NOT NULL,
[DatabaseUser] nvarchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Event] nvarchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Schema] nvarchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Object] nvarchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TSQL] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[XmlEvent] xml NOT NULL
)
ON [PRIMARY];
GO

-- ------------ Write CREATE-CONSTRAINT-stage scripts -----------

USE [BobsUsedBookStore]
GO
            
ALTER TABLE [dbo].[Author]
ADD CONSTRAINT [CK_Author_BirthDate] CHECK (([BirthDate]>='1930-01-01' AND [BirthDate]<=dateadd(year,(-18),getdate())));
GO
            
ALTER TABLE [dbo].[Author]
ADD CONSTRAINT [CK_Author_HireDate] CHECK (([HireDate]>='1996-07-01' AND [HireDate]<=dateadd(day,(1),getdate())));
GO
            
ALTER TABLE [dbo].[Author]
ADD CONSTRAINT [CK_Author_VacationHours] CHECK (([VacationHours]>=(-40) AND [VacationHours]<=(240)));
GO
            
ALTER TABLE [dbo].[Author]
ADD CONSTRAINT [PK_Author_BusinessEntityID] PRIMARY KEY CLUSTERED ([BusinessEntityID]);
GO
            
ALTER TABLE [dbo].[BillOfMaterials]
ADD CONSTRAINT [CK_BillOfMaterials_EndDate] CHECK (([EndDate]>[StartDate] OR [EndDate] IS NULL));
GO
            
ALTER TABLE [dbo].[BillOfMaterials]
ADD CONSTRAINT [CK_BillOfMaterials_PerAssemblyQty] CHECK (([PerAssemblyQty]>=(1.00)));
GO
            
ALTER TABLE [dbo].[BillOfMaterials]
ADD CONSTRAINT [CK_BillOfMaterials_ProductAssemblyID] CHECK (([ProductAssemblyID]<>[ComponentID]));
GO
            
ALTER TABLE [dbo].[BillOfMaterials]
ADD CONSTRAINT [PK_BillOfMaterials_BillOfMaterialsID] PRIMARY KEY ([BillOfMaterialsID]);
GO
            
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [CK_Product_DaysToManufacture] CHECK (([DaysToManufacture]>=(0)));
GO
            
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [CK_Product_ListPrice] CHECK (([ListPrice]>=(0.00)));
GO
            
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [CK_Product_ReorderPoint] CHECK (([ReorderPoint]>(0)));
GO
            
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [CK_Product_SafetyStockLevel] CHECK (([SafetyStockLevel]>(0)));
GO
            
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [CK_Product_SellEndDate] CHECK (([SellEndDate]>=[SellStartDate] OR [SellEndDate] IS NULL));
GO
            
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [CK_Product_StandardCost] CHECK (([StandardCost]>=(0.00)));
GO
            
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [CK_Product_Weight] CHECK (([Weight]>(0.00)));
GO
            
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [PK_Product_ProductID] PRIMARY KEY CLUSTERED ([ProductID]);
GO

ALTER TABLE [dbo].[DatabaseLog]
ADD CONSTRAINT [PK_DatabaseLog_DatabaseLogID] PRIMARY KEY ([DatabaseLogID]);
GO
            
ALTER TABLE [dbo].[ErrorLog]
ADD CONSTRAINT [PK_ErrorLog_ErrorLogID] PRIMARY KEY CLUSTERED ([ErrorLogID]);
GO

-- ------------ Write CREATE-INDEX-stage scripts -----------

USE [BobsUsedBookStore]
GO
            
CREATE UNIQUE NONCLUSTERED INDEX [AK_Author_LoginID]
    ON [dbo].[Author] ([LoginID] ASC);
GO
            
CREATE UNIQUE NONCLUSTERED INDEX [AK_Author_NationalIDNumber]
    ON [dbo].[Author] ([NationalIDNumber] ASC);
GO

CREATE UNIQUE NONCLUSTERED INDEX [AK_Person_rowguid]
    ON [dbo].[Person] ([rowguid] ASC);
GO
            
CREATE NONCLUSTERED INDEX [IX_Person_LastName_FirstName_MiddleName]
    ON [dbo].[Person] ([LastName] ASC, [FirstName] ASC, [MiddleName] ASC);
GO
            
CREATE NONCLUSTERED INDEX [IX_Author_OrganizationNode]
    ON [dbo].[Author] ([OrganizationNode] ASC);
GO

CREATE UNIQUE NONCLUSTERED INDEX [AK_BillOfMaterials_ProductAssemblyID_ComponentID_StartDate]
    ON [dbo].[BillOfMaterials] ([ProductAssemblyID] ASC, [ComponentID] ASC, [StartDate] ASC);
GO
            
CREATE NONCLUSTERED INDEX [IX_BillOfMaterials_UnitMeasureCode]
    ON [dbo].[BillOfMaterials] ([UnitMeasureCode] ASC);
GO
            
CREATE UNIQUE NONCLUSTERED INDEX [AK_Product_Name]
    ON [dbo].[Product] ([Name] ASC);
GO
            
CREATE UNIQUE NONCLUSTERED INDEX [AK_Product_ProductNumber]
    ON [dbo].[Product] ([ProductNumber] ASC);
GO
            
CREATE UNIQUE NONCLUSTERED INDEX [AK_Product_rowguid]
    ON [dbo].[Product] ([rowguid] ASC);
GO  

-- ------------ Write CREATE-DATABASE-TRIGGER-stage scripts -----------

USE [BobsUsedBookStore]
GO

CREATE TRIGGER [ddlDatabaseTriggerLog] ON DATABASE 
FOR DDL_DATABASE_LEVEL_EVENTS AS 
BEGIN
    SET NOCOUNT ON;

    DECLARE @data XML;
    DECLARE @schema sysname;
    DECLARE @object sysname;
    DECLARE @eventType sysname;

    SET @data = EVENTDATA();
    SET @eventType = @data.value('(/EVENT_INSTANCE/EventType)[1]', 'sysname');
    SET @schema = @data.value('(/EVENT_INSTANCE/SchemaName)[1]', 'sysname');
    SET @object = @data.value('(/EVENT_INSTANCE/ObjectName)[1]', 'sysname') 

    IF @object IS NOT NULL
        PRINT '  ' + @eventType + ' - ' + @schema + '.' + @object;
    ELSE
        PRINT '  ' + @eventType + ' - ' + @schema;

    IF @eventType IS NULL
        PRINT CONVERT(nvarchar(max), @data);

    INSERT [dbo].[DatabaseLog] 
        (
        [PostTime], 
        [DatabaseUser], 
        [Event], 
        [Schema], 
        [Object], 
        [TSQL], 
        [XmlEvent]
        ) 
    VALUES 
        (
        GETDATE(), 
        CONVERT(sysname, CURRENT_USER), 
        @eventType, 
        CONVERT(sysname, @schema), 
        CONVERT(sysname, @object), 
        @data.value('(/EVENT_INSTANCE/TSQLCommand)[1]', 'nvarchar(max)'), 
        @data
        );
END;
GO

-- ------------ Write CREATE-ROUTINE-stage scripts -----------

USE [BobsUsedBookStore]
GO
            
CREATE PROCEDURE [dbo].[uspGetBillOfMaterials]
    @StartProductID [int],
    @CheckDate [datetime]
AS
BEGIN
    SET NOCOUNT ON;
    WITH [BOM_cte]([ProductAssemblyID], [ComponentID], [ComponentDesc], [PerAssemblyQty], [StandardCost], [ListPrice], [BOMLevel], [RecursionLevel]) 
    AS (
        SELECT b.[ProductAssemblyID], b.[ComponentID], p.[Name], b.[PerAssemblyQty], p.[StandardCost], p.[ListPrice], b.[BOMLevel], 0 
        FROM [dbo].[BillOfMaterials] b
            INNER JOIN [dbo].[Product] p 
            ON b.[ComponentID] = p.[ProductID] 
        WHERE b.[ProductAssemblyID] = @StartProductID 
            AND @CheckDate >= b.[StartDate] 
            AND @CheckDate <= ISNULL(b.[EndDate], @CheckDate)
        UNION ALL
        SELECT b.[ProductAssemblyID], b.[ComponentID], p.[Name], b.[PerAssemblyQty], p.[StandardCost], p.[ListPrice], b.[BOMLevel], [RecursionLevel] + 1 
        FROM [BOM_cte] cte
            INNER JOIN [dbo].[BillOfMaterials] b 
            ON b.[ProductAssemblyID] = cte.[ComponentID]
            INNER JOIN [dbo].[Product] p 
            ON b.[ComponentID] = p.[ProductID] 
        WHERE @CheckDate >= b.[StartDate] 
            AND @CheckDate <= ISNULL(b.[EndDate], @CheckDate)
        )
    SELECT b.[ProductAssemblyID], b.[ComponentID], b.[ComponentDesc], SUM(b.[PerAssemblyQty]) AS [TotalQuantity] , b.[StandardCost], b.[ListPrice], b.[BOMLevel], b.[RecursionLevel]
    FROM [BOM_cte] b
    GROUP BY b.[ComponentID], b.[ComponentDesc], b.[ProductAssemblyID], b.[BOMLevel], b.[RecursionLevel], b.[StandardCost], b.[ListPrice]
    ORDER BY b.[BOMLevel], b.[ProductAssemblyID], b.[ComponentID]
    OPTION (MAXRECURSION 25) 
END;
GO
            
CREATE PROCEDURE [dbo].[uspGetAuthorManagers]
    @BusinessEntityID [int]
AS
BEGIN
    SET NOCOUNT ON;
    WITH [EMP_cte]([BusinessEntityID], [OrganizationNode], [FirstName], [LastName], [JobTitle], [RecursionLevel]) 
    AS (
        SELECT e.[BusinessEntityID], e.[OrganizationNode], p.[FirstName], p.[LastName], e.[JobTitle], 0 
        FROM [dbo].[Author] e 
            INNER JOIN [dbo].[Person] as p
            ON p.[BusinessEntityID] = e.[BusinessEntityID]
        WHERE e.[BusinessEntityID] = @BusinessEntityID
        UNION ALL
        SELECT e.[BusinessEntityID], e.[OrganizationNode], p.[FirstName], p.[LastName], e.[JobTitle], [RecursionLevel] + 1 
        FROM [dbo].[Author] e 
            INNER JOIN [EMP_cte]
            ON LEFT(e.[OrganizationNode], LEN(e.[OrganizationNode]) - 1) = [EMP_cte].[OrganizationNode]
            INNER JOIN [dbo].[Person] p 
            ON p.[BusinessEntityID] = e.[BusinessEntityID]
    )
    SELECT [EMP_cte].[RecursionLevel], [EMP_cte].[BusinessEntityID], [EMP_cte].[FirstName], [EMP_cte].[LastName], 
        [EMP_cte].[OrganizationNode] AS [OrganizationNode], p.[FirstName] AS 'ManagerFirstName', p.[LastName] AS 'ManagerLastName'  
    FROM [EMP_cte] 
        INNER JOIN [dbo].[Author] e 
        ON LEFT([EMP_cte].[OrganizationNode], LEN([EMP_cte].[OrganizationNode]) - 1) = e.[OrganizationNode]
        INNER JOIN [dbo].[Person] p 
        ON p.[BusinessEntityID] = e.[BusinessEntityID]
    ORDER BY [RecursionLevel], [EMP_cte].[OrganizationNode]
    OPTION (MAXRECURSION 25) 
END;
GO
            
CREATE PROCEDURE [dbo].[uspGetManagerAuthors]
    @BusinessEntityID [int]
AS
BEGIN
    SET NOCOUNT ON;

    WITH [EMP_cte]([BusinessEntityID], [OrganizationNode], [FirstName], [LastName], [RecursionLevel]) 
    AS (
        SELECT e.[BusinessEntityID], e.[OrganizationNode], p.[FirstName], p.[LastName], 0 
        FROM [dbo].[Author] e 
            INNER JOIN [dbo].[Person] p 
            ON p.[BusinessEntityID] = e.[BusinessEntityID]
        WHERE e.[BusinessEntityID] = @BusinessEntityID
        UNION ALL
        SELECT e.[BusinessEntityID], e.[OrganizationNode], p.[FirstName], p.[LastName], [RecursionLevel] + 1
        FROM [dbo].[Author] e 
            INNER JOIN [EMP_cte]
            ON e.[OrganizationNode] = LEFT([EMP_cte].[OrganizationNode], LEN([EMP_cte].[OrganizationNode]) - 1)
            INNER JOIN [dbo].[Person] p 
            ON p.[BusinessEntityID] = e.[BusinessEntityID]
        )
    SELECT [EMP_cte].[RecursionLevel], [EMP_cte].[OrganizationNode] as [OrganizationNode], 
           p.[FirstName] AS 'ManagerFirstName', p.[LastName] AS 'ManagerLastName',
           [EMP_cte].[BusinessEntityID], [EMP_cte].[FirstName], [EMP_cte].[LastName] 
    FROM [EMP_cte] 
        INNER JOIN [dbo].[Author] e 
        ON e.[OrganizationNode] = LEFT([EMP_cte].[OrganizationNode], LEN([EMP_cte].[OrganizationNode]) - 1)
        INNER JOIN [dbo].[Person] p 
        ON p.[BusinessEntityID] = e.[BusinessEntityID]
    ORDER BY [RecursionLevel], [EMP_cte].[OrganizationNode]
    OPTION (MAXRECURSION 25) 
END;
GO
            
CREATE PROCEDURE [dbo].[uspGetWhereUsedProductID]
    @StartProductID [int],
    @CheckDate [datetime]
AS
BEGIN
    SET NOCOUNT ON;
    WITH [BOM_cte]([ProductAssemblyID], [ComponentID], [ComponentDesc], [PerAssemblyQty], [StandardCost], [ListPrice], [BOMLevel], [RecursionLevel]) 
    AS (
        SELECT b.[ProductAssemblyID], b.[ComponentID], p.[Name], b.[PerAssemblyQty], p.[StandardCost], p.[ListPrice], b.[BOMLevel], 0 
        FROM [dbo].[BillOfMaterials] b
            INNER JOIN [dbo].[Product] p 
            ON b.[ProductAssemblyID] = p.[ProductID] 
        WHERE b.[ComponentID] = @StartProductID 
            AND @CheckDate >= b.[StartDate] 
            AND @CheckDate <= ISNULL(b.[EndDate], @CheckDate)
        UNION ALL
        SELECT b.[ProductAssemblyID], b.[ComponentID], p.[Name], b.[PerAssemblyQty], p.[StandardCost], p.[ListPrice], b.[BOMLevel], [RecursionLevel] + 1 
        FROM [BOM_cte] cte
            INNER JOIN [dbo].[BillOfMaterials] b 
            ON cte.[ProductAssemblyID] = b.[ComponentID]
            INNER JOIN [dbo].[Product] p 
            ON b.[ProductAssemblyID] = p.[ProductID] 
        WHERE @CheckDate >= b.[StartDate] 
            AND @CheckDate <= ISNULL(b.[EndDate], @CheckDate)
        )
    SELECT b.[ProductAssemblyID], b.[ComponentID], b.[ComponentDesc], SUM(b.[PerAssemblyQty]) AS [TotalQuantity] , b.[StandardCost], b.[ListPrice], b.[BOMLevel], b.[RecursionLevel]
    FROM [BOM_cte] b
    GROUP BY b.[ComponentID], b.[ComponentDesc], b.[ProductAssemblyID], b.[BOMLevel], b.[RecursionLevel], b.[StandardCost], b.[ListPrice]
    ORDER BY b.[BOMLevel], b.[ProductAssemblyID], b.[ComponentID]
    OPTION (MAXRECURSION 25) 
END;
GO

CREATE PROCEDURE [dbo].[uspPrintError] 
AS
BEGIN
    SET NOCOUNT ON;

    -- Print error information. 
    PRINT 'Error ' + CONVERT(varchar(50), ERROR_NUMBER()) +
          ', Severity ' + CONVERT(varchar(5), ERROR_SEVERITY()) +
          ', State ' + CONVERT(varchar(5), ERROR_STATE()) + 
          ', Procedure ' + ISNULL(ERROR_PROCEDURE(), '-') + 
          ', Line ' + CONVERT(varchar(5), ERROR_LINE());
    PRINT ERROR_MESSAGE();
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[uspLogError] 
    @ErrorLogID [int] = 0 OUTPUT 
AS
BEGIN
    SET NOCOUNT ON;
    SET @ErrorLogID = 0;

    BEGIN TRY
        IF ERROR_NUMBER() IS NULL
            RETURN;

        INSERT [dbo].[ErrorLog] 
            (
            [UserName], 
            [ErrorNumber], 
            [ErrorSeverity], 
            [ErrorState], 
            [ErrorProcedure], 
            [ErrorLine], 
            [ErrorMessage]
            ) 
        VALUES 
            (
            SUSER_SNAME(), 
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE()
            );

        SET @ErrorLogID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        PRINT 'An error occurred in stored procedure uspLogError: ';
        EXECUTE [dbo].[uspPrintError];
        RETURN -1;
    END CATCH
END;
GO

CREATE PROCEDURE [dbo].[uspUpdateAuthorLogin]
    @BusinessEntityID [int], 
    @OrganizationNode [nvarchar](4000), 
    @LoginID [nvarchar](256),
    @JobTitle [nvarchar](50),
    @HireDate [datetime],
    @CurrentFlag [dbo].[Flag]
WITH EXECUTE AS CALLER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE [dbo].[Author] 
        SET [OrganizationNode] = @OrganizationNode 
            ,[LoginID] = @LoginID 
            ,[JobTitle] = @JobTitle 
            ,[HireDate] = @HireDate 
            ,[CurrentFlag] = @CurrentFlag 
        WHERE [BusinessEntityID] = @BusinessEntityID;
    END TRY
    BEGIN CATCH
        EXECUTE [dbo].[uspLogError];
    END CATCH;
END;
GO
      
CREATE PROCEDURE [dbo].[uspUpdateAuthorPersonalInfo]
    @BusinessEntityID [int], 
    @NationalIDNumber [nvarchar](15), 
    @BirthDate [datetime], 
    @MaritalStatus [nchar](1), 
    @Gender [nchar](1)
WITH EXECUTE AS CALLER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE [dbo].[Author] 
        SET [NationalIDNumber] = @NationalIDNumber 
            ,[BirthDate] = @BirthDate 
            ,[MaritalStatus] = @MaritalStatus 
            ,[Gender] = @Gender 
        WHERE [BusinessEntityID] = @BusinessEntityID;
    END TRY
    BEGIN CATCH
        EXECUTE [dbo].[uspLogError];
    END CATCH;
END;
GO

CREATE PROCEDURE [dbo].[uspDeleteAuthor]
    @BusinessEntityID [int]
WITH EXECUTE AS CALLER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM [dbo].[Author]
        WHERE [BusinessEntityID] = @BusinessEntityID;

        -- Check if the delete was successful
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('No author found with the provided BusinessEntityID.', 16, 1);
            RETURN;
        END
    END TRY
    BEGIN CATCH
        -- Log the error and re-throw
        EXECUTE [dbo].[uspLogError];
        THROW;
    END CATCH;
END;
GO

CREATE PROCEDURE [dbo].[uspGetProductData]
    @my_cursor CURSOR VARYING OUTPUT
AS
BEGIN
    -- Open a cursor for the SELECT query
    SET @my_cursor = CURSOR FOR
    SELECT
        ProductID,
        Name,
        ProductNumber,
        SafetyStockLevel
    FROM dbo.Product;
    -- Open the cursor to make it available to the caller
    OPEN @my_cursor;
END;
GO

CREATE FUNCTION [dbo].[ufnGetAccountingEndDate]()
RETURNS [datetime] 
AS 
BEGIN
    RETURN DATEADD(millisecond, -2, CONVERT(datetime, '20040701', 112));
END;
GO
            
CREATE FUNCTION [dbo].[ufnGetAccountingStartDate]()
RETURNS [datetime] 
AS 
BEGIN
    RETURN CONVERT(datetime, '20030701', 112);
END;
GO
            
CREATE FUNCTION [dbo].[ufnGetDocumentStatusText](@Status [tinyint])
RETURNS [nvarchar](16) 
AS 
BEGIN
    DECLARE @ret [nvarchar](16);

    SET @ret = 
        CASE @Status
            WHEN 1 THEN N'Pending approval'
            WHEN 2 THEN N'Approved'
            WHEN 3 THEN N'Obsolete'
            ELSE N'** Invalid **'
        END;
    
    RETURN @ret
END;
GO
          
CREATE FUNCTION [dbo].[ufnGetPurchaseOrderStatusText](@Status [tinyint])
RETURNS [nvarchar](15) 
AS 
BEGIN
    DECLARE @ret [nvarchar](15);

    SET @ret = 
        CASE @Status
            WHEN 1 THEN 'Pending'
            WHEN 2 THEN 'Approved'
            WHEN 3 THEN 'Rejected'
            WHEN 4 THEN 'Complete'
            ELSE '** Invalid **'
        END;
    
    RETURN @ret
END;
GO
            
CREATE FUNCTION [dbo].[ufnGetSalesOrderStatusText](@Status [tinyint])
RETURNS [nvarchar](15) 
AS 
BEGIN
    DECLARE @ret [nvarchar](15);

    SET @ret = 
        CASE @Status
            WHEN 1 THEN 'In process'
            WHEN 2 THEN 'Approved'
            WHEN 3 THEN 'Backordered'
            WHEN 4 THEN 'Rejected'
            WHEN 5 THEN 'Shipped'
            WHEN 6 THEN 'Cancelled'
            ELSE '** Invalid **'
        END;
    
    RETURN @ret
END;
GO
                    
CREATE FUNCTION [dbo].[ufnLeadingZeros](
    @Value int
) 
RETURNS varchar(8) 
WITH SCHEMABINDING 
AS 
BEGIN
    DECLARE @ReturnValue varchar(8);

    SET @ReturnValue = CONVERT(varchar(8), @Value);
    SET @ReturnValue = REPLICATE('0', 8 - DATALENGTH(@ReturnValue)) + @ReturnValue;

    RETURN (@ReturnValue);
END;
GO

-- ------------ Write CREATE-OTHER-stage scripts -----------

USE [BobsUsedBookStore]
GO
            
CREATE TABLE [dbo].[Members](
    [CustomerID] INT PRIMARY KEY,
    [FirstName] NVARCHAR(50),
    [LastName] NVARCHAR(50),
    [Email] NVARCHAR(100),
    [Phone] NVARCHAR(20),
    [RegistrationDate] DATE,
    [TotalOrderSum] DECIMAL(10,2)
)
ON [PRIMARY];
GO

CREATE TABLE [dbo].[Shopping](
    [OrderID] INT PRIMARY KEY,
    [CustomerID] INT,
    [OrderDate] DATETIME,
    [TotalAmount] DECIMAL(10,2),
    [Status] NVARCHAR(20)
)
ON [PRIMARY];
GO

CREATE TABLE [dbo].[Coupons](
    [TicketID] INT PRIMARY KEY,
    [CustomerID] INT,
    [OrderID] INT,
    [IssueDescription] NVARCHAR(MAX),
    [CreatedDate] DATETIME,
    [ResolvedDate] DATETIME,
    [Status] NVARCHAR(20),
    [OSUser] VARCHAR(10)
)
ON [PRIMARY];
GO

CREATE TABLE [dbo].[ProductSaleRegions](
    [RegionID] INT NOT NULL,
    [RegionName] VARCHAR(50) NOT NULL
)
ON [PRIMARY];
GO

CREATE TABLE [dbo].[ProductSales](
    [SaleID] NUMERIC NOT NULL,
    [ProductID] INT NOT NULL,
    [SaleDate] DATE NOT NULL,
    [Quantity] NUMERIC(10,0) NOT NULL,
    [UnitPrice] NUMERIC(9,2) NOT NULL,
    [TotalAmount] NUMERIC(11,2) NOT NULL,
    [CustomerID] INT NOT NULL,
    [SalePrice] NUMERIC(9,2),
    [RegionID] NUMERIC(10,0)
)
ON [PRIMARY];
GO

-- Create views
CREATE VIEW [dbo].[VwTopMembers] AS
SELECT * 
FROM (
    SELECT TOP 50 PERCENT 
        c.[CustomerID], 
        c.[FirstName], 
        c.[LastName], 
        SUM(o.[TotalAmount]) AS [TotalSpent]
    FROM [dbo].[Members] c
    JOIN [dbo].[Shopping] o ON c.[CustomerID] = o.[CustomerID]
    GROUP BY c.[CustomerID], c.[FirstName], c.[LastName]
    ORDER BY [TotalSpent] DESC
) sub;
GO

CREATE VIEW [dbo].[VwOpenCoupons] AS
SELECT 
    t.[TicketID], 
    c.[FirstName], 
    c.[LastName], 
    t.[IssueDescription], 
    t.[CreatedDate], 
    DATEDIFF(DAY, t.[CreatedDate], GETDATE()) AS [DaysOpen]
FROM [dbo].[Coupons] t
JOIN [dbo].[Members] c ON t.[CustomerID] = c.[CustomerID]
WHERE t.[Status] = 'Open' AND t.[OSUser] = CURRENT_USER;
GO

CREATE VIEW [dbo].[VwCustomerShopping] AS
SELECT 
    o.[OrderID],
    c.[FirstName] + ' ' + c.[LastName] AS [CustomerName],
    o.[OrderDate],
    o.[TotalAmount],
    o.[Status],
    FORMATMESSAGE(
        'Hi, %s! Your order #%s for $%s will be shipped shortly. Thank you!',
        c.[FirstName],
        CAST(o.[OrderID] AS VARCHAR),
        CAST(o.[TotalAmount] AS VARCHAR)
    ) AS [GreetingMessage]
FROM [dbo].[Shopping] o
JOIN [dbo].[Members] c ON c.[CustomerID] = o.[CustomerID];
GO

CREATE VIEW [dbo].[VwRegionalSales] ([RegionName], [RegionSalesSum]) AS
SELECT 
    CHOOSE([RegionID], 
           'North America', 'Europe', 'Asia', 'South America', 'Africa', 
           'Australia', 'Middle East', 'Central America', 'Eastern Europe', 
           'Western Europe') AS [RegionName],
    SUM([Quantity] * [SalePrice]) AS [RegionSalesSum]
FROM [dbo].[ProductSales]
GROUP BY CHOOSE([RegionID], 
                'North America', 'Europe', 'Asia', 'South America', 'Africa', 
                'Australia', 'Middle East', 'Central America', 'Eastern Europe', 
                'Western Europe');
GO

-- Create procedures
CREATE PROCEDURE [dbo].[uspGetTopRegion]
    @Percent INT
AS
BEGIN
    WITH [HalfYearSales] AS (
        SELECT 
            psr.[RegionID],
            psr.[RegionName],
            COUNT(ps.[SaleID]) AS [NumberOfSales],
            SUM(ps.[TotalAmount]) AS [TotalSalesAmount],
            COUNT(DISTINCT ps.[CustomerID]) AS [NumberOfMembers]
        FROM 
            [dbo].[ProductSaleRegions] psr
        LEFT JOIN 
            [dbo].[ProductSales] ps ON psr.[RegionID] = ps.[RegionID]
        WHERE 
            ps.[SaleDate] >= DATEADD(MONTH, -6, GETDATE())
        GROUP BY 
            psr.[RegionID], psr.[RegionName]
    )
    SELECT * FROM (
        SELECT TOP 10 PERCENT * FROM [HalfYearSales] WHERE @Percent = 10 ORDER BY [NumberOfSales] DESC
        UNION ALL 
        SELECT TOP 20 PERCENT * FROM [HalfYearSales] WHERE @Percent = 20 ORDER BY [NumberOfSales] DESC
        UNION ALL 
        SELECT TOP 30 PERCENT * FROM [HalfYearSales] WHERE @Percent = 30 ORDER BY [NumberOfSales] DESC
        UNION ALL 
        SELECT TOP 40 PERCENT * FROM [HalfYearSales] WHERE @Percent = 40 ORDER BY [NumberOfSales] DESC
        UNION ALL 
        SELECT TOP 50 PERCENT * FROM [HalfYearSales] WHERE @Percent = 50 ORDER BY [NumberOfSales] DESC
    ) q
    ORDER BY [NumberOfSales] DESC, [TotalSalesAmount] DESC;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[uspDeleteOldCoupons]
    @DaysOld INT
AS
BEGIN
    DECLARE @DeletedCount INT = 0;
    
    SELECT [TicketID], [CreatedDate]
    INTO #CouponsToDelete
    FROM [dbo].[Coupons]
    WHERE TRIM([Status]) = 'Closed' AND [ResolvedDate] <= DATEADD(DAY, -@DaysOld, GETDATE());

    DECLARE @CouponsCount INT;
    SELECT @CouponsCount = COUNT(*) FROM #CouponsToDelete;

    IF @CouponsCount > 0
    BEGIN
        DELETE t
        FROM [dbo].[Coupons] t
        INNER JOIN #CouponsToDelete d ON t.[TicketID] = d.[TicketID];
        
        SET @DeletedCount = @@ROWCOUNT;
        PRINT 'Deleted ' + CAST(@DeletedCount AS NVARCHAR(10)) + ' coupons.';
    END
    ELSE
    BEGIN
        PRINT 'No coupons found matching the criteria.';
    END
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[uspProcessRefunds]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RefundCount INT = 0;
    DECLARE @MaxID INT;

    SELECT @MaxID = MAX([TicketID]) FROM [dbo].[Coupons];
    
    SELECT [OrderID], [CustomerID], [TotalAmount]
    INTO #RefundRequests
    FROM [dbo].[Shopping]
    WHERE TRIM([Status]) = 'Refund Requested';
    
    UPDATE o
    SET [Status] = 'Refunded'
    FROM [dbo].[Shopping] o
    INNER JOIN #RefundRequests r ON o.[OrderID] = r.[OrderID];
    
    INSERT INTO [dbo].[Coupons] ([TicketID], [CustomerID], [OrderID], [IssueDescription], [CreatedDate], [Status])
    SELECT 
        @MaxID + ROW_NUMBER() OVER (ORDER BY [OrderID]),
        [CustomerID],
        [OrderID],
        'Refund processed for $' + CAST([TotalAmount] AS NVARCHAR(20)),
        GETDATE(),
        'Closed'
    FROM #RefundRequests;

    SET @RefundCount = @@ROWCOUNT;

    PRINT 'Processed ' + CAST(@RefundCount AS NVARCHAR(10)) + ' refunds.';

    DROP TABLE #RefundRequests;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[uspShoppingLevelAmount]
    @Percent INT,
    @Amount NUMERIC
AS
BEGIN
    SELECT 
        [OrderID],
        '|' + CASE 
                WHEN @Percent > 100 THEN
                    CASE 
                        WHEN [TotalAmount] > @Amount THEN 'more than'
                        ELSE 'less than'
                    END
              END + '|' AS [AmountLevel]
    FROM [dbo].[Shopping];
END;
GO

CREATE OR ALTER FUNCTION [dbo].[ufnCalculateCustomerLifetimeValue]
(
    @CustomerID INT
)
RETURNS DECIMAL(10, 2)
AS
BEGIN
    DECLARE @TotalSpent DECIMAL(10, 2);
    DECLARE @FirstOrderDate DATE;
    DECLARE @DaysSinceFirstOrder INT;
    DECLARE @LifetimeValue DECIMAL(10, 2);
    
    SELECT 
        @TotalSpent = SUM([TotalAmount]),
        @FirstOrderDate = MIN([OrderDate])
    FROM [dbo].[Shopping]
    WHERE [CustomerID] = @CustomerID AND TRIM([Status]) != 'Refund Requested';
    
    SET @DaysSinceFirstOrder = DATEDIFF(DAY, @FirstOrderDate, GETDATE());
    
    IF @DaysSinceFirstOrder = 0
        SET @LifetimeValue = @TotalSpent;
    ELSE
        SET @LifetimeValue = (@TotalSpent / @DaysSinceFirstOrder) * 365.25;
    
    RETURN @LifetimeValue;
END;
GO

-- ============================================================================
-- APPENDING DATA FROM adven-data.sql
-- ============================================================================
/*============================================================================
  File:     mnsg-adven-data.sql

  Summary:  Insert sample data for the workshop mnsg. It comes from Microsoft AdventureWorks sample database. 

  Date:     October 26, 2017
  Updated:  October 26, 2017

------------------------------------------------------------------------------
  This file is part of the Microsoft SQL Server Code Samples.

  Copyright (C) Microsoft Corporation.  All rights reserved.

  This source code is intended only as a supplement to Microsoft
  Development Tools and/or on-line documentation.  See these other
  materials for detailed information regarding Microsoft code samples.

  All data in this database is ficticious.
  
  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
============================================================================*/

-- ******************************************************
-- Load data
-- ******************************************************

INSERT INTO [dbo].[Author] (
    [NationalIDNumber], [LoginID], [OrganizationNode],
    [JobTitle], [BirthDate], [MaritalStatus], [Gender],
    [HireDate], [VacationHours], [CurrentFlag],
    [ModifiedDate]    
) VALUES
    (N'295847284', N'adventure-works\ken0', NULL, N'Chief Executive Officer', '1969-01-29', N'S', N'M', '2009-01-14',99,1, '2014-06-30'),
    (N'245797967', N'adventure-works\terri0', N'58', N'Vice President of Engineering', '1971-08-01', N'S', N'F', '2008-01-31',1,1, '2014-06-30'),
    (N'509647174', N'adventure-works\roberto0', N'5AC0', N'Engineering Manager', '1974-11-12', N'M', N'M', '2007-11-11',2,1, '2014-06-30'),
    (N'112457891', N'adventure-works\rob0', N'5AD6', N'Senior Tool Designer', '1974-12-23', N'S', N'M', '2007-12-05',48,1, '2014-06-30'),
    (N'695256908', N'adventure-works\gail0', N'5ADA', N'Design Engineer', '1952-09-27', N'M', N'F', '2008-01-06',5,1, '2014-06-30'),
    (N'998320692', N'adventure-works\jossef0', N'5ADE', N'Design Engineer', '1959-03-11', N'M', N'M', '2008-01-24',6,1, '2014-06-30'),
    (N'134969118', N'adventure-works\dylan0', N'5AE1', N'Research and Development Manager', '1987-02-24', N'M', N'M', '2009-02-08',61,1, '2014-06-30'),
    (N'811994146', N'adventure-works\diane1', N'5AE158', N'Research and Development Engineer', '1986-06-05', N'S', N'F', '2008-12-29',62,1, '2014-06-30'),
    (N'658797903', N'adventure-works\gigi0', N'5AE168', N'Research and Development Engineer', '1979-01-21', N'M', N'F', '2009-01-16',63,1, '2014-06-30'),
    (N'879342154', N'adventure-works\michael6', N'5AE178', N'Research and Development Manager', '1984-11-30', N'M', N'M', '2009-05-03',16,1, '2014-06-30'),
    (N'974026903', N'adventure-works\ovidiu0', N'5AE3', N'Senior Tool Designer', '1978-01-17', N'S', N'M', '2010-12-05',7,1, '2014-06-30'),
    (N'480168528', N'adventure-works\thierry0', N'5AE358', N'Tool Designer', '1959-07-29', N'M', N'M', '2007-12-11',9,1, '2014-06-30'),
    (N'486228782', N'adventure-works\janice0', N'5AE368', N'Tool Designer', '1989-05-28', N'M', N'F', '2010-12-23',8,1, '2014-06-30'),
    (N'42487730', N'adventure-works\michael8', N'5AE5', N'Senior Design Engineer', '1979-06-16', N'S', N'M', '2010-12-30',3,1, '2014-06-30'),
    (N'56920285', N'adventure-works\sharon0', N'5AE7', N'Design Engineer', '1961-05-02', N'M', N'F', '2011-01-18',4,1, '2014-06-30'),
    (N'24756624', N'adventure-works\david0', N'68', N'Marketing Manager', '1975-03-19', N'S', N'M', '2007-12-20',40,1, '2014-06-30'),
    (N'253022876', N'adventure-works\kevin0', N'6AC0', N'Marketing Assistant', '1987-05-03', N'S', N'M', '2007-01-26',42,1, '2014-06-30'),
    (N'222969461', N'adventure-works\john5', N'6B40', N'Marketing Specialist', '1978-03-06', N'S', N'M', '2011-02-07',48,1, '2014-06-30'),
    (N'52541318', N'adventure-works\mary2', N'6BC0', N'Marketing Assistant', '1978-01-29', N'S', N'F', '2011-02-14',43,1, '2014-06-30'),
    (N'323403273', N'adventure-works\wanida0', N'6C20', N'Marketing Assistant', '1975-03-17', N'M', N'F', '2011-01-07',41,1, '2014-06-30'),
    (N'243322160', N'adventure-works\terry0', N'6C60', N'Marketing Specialist', '1986-02-04', N'M', N'M', '2009-03-02',44,1, '2014-06-30'),
    (N'95958330', N'adventure-works\sariya0', N'6CA0', N'Marketing Specialist', '1987-05-21', N'S', N'M', '2008-12-12',45,1, '2014-06-30'),
    (N'767955365', N'adventure-works\mary0', N'6CE0', N'Marketing Specialist', '1962-09-13', N'M', N'F', '2009-01-12',46,1, '2014-06-30'),
    (N'72636981', N'adventure-works\jill0', N'6D10', N'Marketing Specialist', '1979-06-18', N'M', N'F', '2009-01-18',47,1, '2014-06-30'),
    (N'519899904', N'adventure-works\james1', N'78', N'Vice President of Production', '1983-01-07', N'S', N'M', '2009-02-03',64,1, '2014-06-30'),
    (N'277173473', N'adventure-works\peter0', N'7AC0', N'Production Control Manager', '1982-11-03', N'M', N'M', '2008-12-01',43,1, '2014-12-26'),
    (N'446466105', N'adventure-works\jo0', N'7AD6', N'Production Supervisor - WC60', '1956-10-08', N'S', N'F', '2008-02-27',80,1, '2014-06-30'),
    (N'14417807', N'adventure-works\guy1', N'7AD6B0', N'Production Technician - WC60', '1988-03-13', N'M', N'M', '2006-06-30',21,1, '2014-06-30'),
    (N'948320468', N'adventure-works\mark1', N'7AD6D0', N'Production Technician - WC60', '1979-09-25', N'S', N'M', '2009-01-23',19,1, '2014-06-30'),
    (N'410742000', N'adventure-works\britta0', N'7AD6F0', N'Production Technician - WC60', '1989-09-28', N'M', N'F', '2009-01-29',14,1, '2014-06-30'),
    (N'750246141', N'adventure-works\margie0', N'7AD708', N'Production Technician - WC60', '1986-05-20', N'M', N'F', '2009-01-04',18,1, '2014-06-30'),
    (N'330211482', N'adventure-works\rebecca0', N'7AD718', N'Production Technician - WC60', '1977-07-10', N'M', N'F', '2008-12-29',23,1, '2014-06-30'),
    (N'801758002', N'adventure-works\annik0', N'7AD728', N'Production Technician - WC60', '1976-12-26', N'M', N'M', '2008-12-17',17,1, '2014-06-30'),
    (N'754372876', N'adventure-works\suchitra0', N'7AD738', N'Production Technician - WC60', '1987-06-10', N'M', N'F', '2009-02-16',15,1, '2014-06-30'),
    (N'999440576', N'adventure-works\brandon0', N'7AD744', N'Production Technician - WC60', '1977-01-10', N'M', N'M', '2009-02-08',22,1, '2014-06-30'),
    (N'788456780', N'adventure-works\jose0', N'7AD74C', N'Production Technician - WC60', '1984-07-31', N'M', N'M', '2009-02-10',20,1, '2014-06-30'),
    (N'442121106', N'adventure-works\chris2', N'7AD754', N'Production Technician - WC60', '1986-08-07', N'S', N'M', '2009-03-07',16,1, '2014-06-30'),
    (N'6298838', N'adventure-works\kim1', N'7AD75C', N'Production Technician - WC60', '1966-12-14', N'M', N'F', '2010-01-16',24,1, '2014-06-30'),
    (N'461786517', N'adventure-works\ed0', N'7AD764', N'Production Technician - WC60', '1971-09-11', N'S', N'M', '2010-02-05',25,1, '2014-06-30'),
    (N'309738752', N'adventure-works\jolynn0', N'7ADA', N'Production Supervisor - WC60', '1956-01-16', N'S', N'F', '2007-12-26',82,1, '2014-06-30'),
    (N'458159238', N'adventure-works\bryan0', N'7ADAB0', N'Production Technician - WC60', '1973-08-27', N'S', N'M', '2009-01-21',35,1, '2014-06-30'),
    (N'339712426', N'adventure-works\james0', N'7ADAD0', N'Production Technician - WC60', '1984-07-25', N'M', N'M', '2008-12-27',39,1, '2014-06-30'),
    (N'693325305', N'adventure-works\nancy0', N'7ADAF0', N'Production Technician - WC60', '1988-11-19', N'M', N'F', '2009-01-02',34,1, '2014-06-30'),
    (N'276751903', N'adventure-works\simon0', N'7ADB08', N'Production Technician - WC60', '1990-05-17', N'S', N'M', '2008-12-08',38,1, '2014-06-30'),
    (N'500412746', N'adventure-works\thomas0', N'7ADB18', N'Production Technician - WC60', '1986-01-10', N'M', N'M', '2009-02-26',37,1, '2014-06-30'),
    (N'66073987', N'adventure-works\eugene1', N'7ADB28', N'Production Technician - WC60', '1976-02-10', N'S', N'M', '2009-02-08',36,1, '2014-06-30'),
    (N'33237992', N'adventure-works\andrew0', N'7ADE', N'Production Supervisor - WC10', '1988-09-06', N'S', N'M', '2009-02-22',65,1, '2014-06-30'),
    (N'690627818', N'adventure-works\ruth0', N'7ADEB0', N'Production Technician - WC10', '1956-06-04', N'M', N'F', '2008-01-06',83,1, '2014-06-30'),
    (N'912265825', N'adventure-works\barry0', N'7ADED0', N'Production Technician - WC10', '1956-03-26', N'S', N'M', '2008-01-07',88,1, '2014-06-30'),
    (N'844973625', N'adventure-works\sidney0', N'7ADEF0', N'Production Technician - WC10', '1956-08-30', N'M', N'M', '2008-02-02',84,1, '2014-06-30'),
    (N'132674823', N'adventure-works\jeffrey0', N'7ADF08', N'Production Technician - WC10', '1956-07-11', N'S', N'M', '2008-02-20',85,1, '2014-06-30'),
    (N'565090917', N'adventure-works\doris0', N'7ADF18', N'Production Technician - WC10', '1956-04-04', N'M', N'F', '2008-03-10',86,1, '2014-06-30'),
    (N'9659517', N'adventure-works\diane0', N'7ADF28', N'Production Technician - WC10', '1956-03-29', N'M', N'F', '2008-03-28',87,1, '2014-06-30'),
    (N'109272464', N'adventure-works\bonnie0', N'7ADF38', N'Production Technician - WC10', '1986-09-10', N'M', N'F', '2010-01-01',89,1, '2014-06-30'),
    (N'233069302', N'adventure-works\taylor0', N'7AE1', N'Production Supervisor - WC50', '1956-04-01', N'M', N'M', '2008-02-08',79,1, '2014-06-30'),
    (N'652535724', N'adventure-works\denise0', N'7AE158', N'Production Technician - WC50', '1988-07-06', N'M', N'F', '2009-02-05',9,1, '2014-06-30'),
    (N'10708100', N'adventure-works\frank1', N'7AE168', N'Production Technician - WC50', '1971-07-24', N'S', N'M', '2009-02-23',10,1, '2014-06-30'),
    (N'571658797', N'adventure-works\kendall0', N'7AE178', N'Production Technician - WC50', '1986-05-30', N'M', N'M', '2008-12-05',11,1, '2014-06-30'),
    (N'843479922', N'adventure-works\bob0', N'7AE184', N'Production Technician - WC50', '1979-08-16', N'S', N'M', '2008-12-24',12,1, '2014-06-30'),
    (N'827686041', N'adventure-works\pete0', N'7AE18C', N'Production Technician - WC50', '1977-02-03', N'S', N'M', '2009-01-11',13,1, '2014-06-30'),
    (N'92096924', N'adventure-works\diane2', N'7AE194', N'Production Technician - WC50', '1989-08-09', N'S', N'F', '2009-01-18',8,1, '2014-06-30'),
    (N'494170342', N'adventure-works\john0', N'7AE3', N'Production Supervisor - WC60', '1956-08-07', N'M', N'M', '2008-03-17',81,1, '2014-06-30'),
    (N'414476027', N'adventure-works\maciej0', N'7AE358', N'Production Technician - WC60', '1955-01-30', N'S', N'M', '2010-01-29',30,1, '2014-06-30'),
    (N'582347317', N'adventure-works\michael7', N'7AE368', N'Production Technician - WC60', '1973-09-05', N'S', N'M', '2010-02-23',26,1, '2014-06-30'),
    (N'8066363', N'adventure-works\randy0', N'7AE378', N'Production Technician - WC60', '1970-04-28', N'M', N'M', '2010-02-23',29,1, '2014-06-30'),
    (N'834186596', N'adventure-works\karan0', N'7AE384', N'Production Technician - WC60', '1970-03-07', N'S', N'M', '2009-12-22',28,1, '2014-06-30'),
    (N'63179277', N'adventure-works\jay0', N'7AE38C', N'Production Technician - WC60', '1976-02-11', N'S', N'M', '2009-03-05',32,1, '2014-06-30'),
    (N'537092325', N'adventure-works\charles0', N'7AE394', N'Production Technician - WC60', '1971-09-02', N'S', N'M', '2009-12-03',27,1, '2014-06-30'),
    (N'752513276', N'adventure-works\steve0', N'7AE39C', N'Production Technician - WC60', '1991-04-06', N'S', N'M', '2009-02-15',31,1, '2014-06-30'),
    (N'36151748', N'adventure-works\david2', N'7AE3A2', N'Production Technician - WC60', '1984-12-29', N'M', N'M', '2008-12-15',33,1, '2014-06-30'),
    (N'578935259', N'adventure-works\michael3', N'7AE5', N'Production Supervisor - WC30', '1989-01-29', N'S', N'M', '2009-02-15',70,1, '2014-06-30'),
    (N'443968955', N'adventure-works\steven0', N'7AE558', N'Production Technician - WC30', '1977-05-14', N'M', N'M', '2008-12-01',41,1, '2014-06-30'),
    (N'138280935', N'adventure-works\carole0', N'7AE568', N'Production Technician - WC30', '1983-10-19', N'M', N'F', '2008-12-19',42,1, '2014-06-30'),
    (N'420023788', N'adventure-works\bjorn0', N'7AE578', N'Production Technician - WC30', '1989-11-06', N'S', N'M', '2009-01-07',43,1, '2014-06-30'),
    (N'363996959', N'adventure-works\michiko0', N'7AE584', N'Production Technician - WC30', '1982-06-27', N'S', N'M', '2009-01-26',44,1, '2014-06-30'),
    (N'227319668', N'adventure-works\carol0', N'7AE58C', N'Production Technician - WC30', '1988-10-17', N'M', N'F', '2009-02-12',45,1, '2014-06-30'),
    (N'301435199', N'adventure-works\merav0', N'7AE594', N'Production Technician - WC30', '1983-05-13', N'M', N'F', '2009-03-03',46,1, '2014-06-30'),
    (N'370989364', N'adventure-works\reuben0', N'7AE7', N'Production Supervisor - WC40', '1987-08-27', N'M', N'M', '2008-12-15',72,1, '2014-06-30'),
    (N'697712387', N'adventure-works\eric1', N'7AE758', N'Production Technician - WC40', '1966-12-08', N'M', N'M', '2010-01-24',60,1, '2014-06-30'),
    (N'943170460', N'adventure-works\sandeep0', N'7AE768', N'Production Technician - WC40', '1970-12-03', N'S', N'M', '2010-01-17',65,1, '2014-06-30'),
    (N'413787783', N'adventure-works\mihail0', N'7AE778', N'Production Technician - WC40', '1971-03-09', N'S', N'M', '2009-12-29',64,1, '2014-06-30'),
    (N'58791499', N'adventure-works\jack1', N'7AE784', N'Production Technician - WC40', '1973-08-29', N'S', N'M', '2010-03-03',62,1, '2014-06-30'),
    (N'988315686', N'adventure-works\patrick1', N'7AE78C', N'Production Technician - WC40', '1973-12-23', N'M', N'M', '2010-02-12',61,1, '2014-06-30'),
    (N'947029962', N'adventure-works\frank3', N'7AE794', N'Production Technician - WC40', '1952-03-02', N'M', N'M', '2010-02-05',66,1, '2014-06-30'),
    (N'1662732', N'adventure-works\brian2', N'7AE79C', N'Production Technician - WC40', '1970-12-23', N'S', N'M', '2009-12-11',63,1, '2014-06-30'),
    (N'769680433', N'adventure-works\ryan0', N'7AE7A2', N'Production Technician - WC40', '1972-06-13', N'M', N'M', '2009-01-05',59,1, '2014-06-30'),
    (N'7201901', N'adventure-works\cristian0', N'7AE880', N'Production Supervisor - WC10', '1984-04-11', N'M', N'M', '2008-12-22',67,1, '2014-06-30'),
    (N'294148271', N'adventure-works\betsy0', N'7AE8AC', N'Production Technician - WC10', '1966-12-17', N'S', N'F', '2009-12-18',99,1, '2014-06-30'),
    (N'90888098', N'adventure-works\patrick0', N'7AE8B4', N'Production Technician - WC10', '1986-09-10', N'S', N'M', '2010-02-01',96,1, '2014-06-30'),
    (N'82638150', N'adventure-works\danielle0', N'7AE8BC', N'Production Technician - WC10', '1986-09-07', N'S', N'F', '2010-02-20',97,1, '2014-06-30'),
    (N'390124815', N'adventure-works\kimberly0', N'7AE8C2', N'Production Technician - WC10', '1986-09-13', N'S', N'F', '2010-01-12',95,1, '2014-06-30'),
    (N'826454897', N'adventure-works\tom0', N'7AE8C6', N'Production Technician - WC10', '1986-10-01', N'M', N'M', '2010-03-10',98,1, '2014-06-30'),
    (N'778552911', N'adventure-works\kok-ho0', N'7AE980', N'Production Supervisor - WC50', '1980-04-28', N'S', N'M', '2008-12-27',78,1, '2014-06-30'),
    (N'718299860', N'adventure-works\russell0', N'7AE9AC', N'Production Technician - WC50', '1972-11-25', N'M', N'M', '2008-12-12',6,1, '2014-06-30'),
    (N'674171828', N'adventure-works\jim0', N'7AE9B4', N'Production Technician - WC50', '1986-09-08', N'M', N'M', '2008-12-19',1,1, '2014-06-30'),
    (N'912141525', N'adventure-works\elizabeth0', N'7AE9BC', N'Production Technician - WC50', '1990-01-25', N'M', N'F', '2009-03-02',5,1, '2014-06-30'),
    (N'370581729', N'adventure-works\mandar0', N'7AE9C2', N'Production Technician - WC50', '1986-03-21', N'S', N'M', '2009-02-10',0,1, '2014-06-30'),
    (N'152085091', N'adventure-works\sameer0', N'7AE9C6', N'Production Technician - WC50', '1978-06-26', N'M', N'M', '2009-02-11',4,1, '2014-06-30'),
    (N'431859843', N'adventure-works\nuan0', N'7AE9CA', N'Production Technician - WC50', '1979-03-29', N'S', N'M', '2009-01-06',2,1, '2014-06-30'),
    (N'204035155', N'adventure-works\lolan0', N'7AE9CE', N'Production Technician - WC50', '1973-01-24', N'M', N'M', '2009-01-12',7,1, '2014-06-30'),
    (N'153288994', N'adventure-works\houman0', N'7AE9D1', N'Production Technician - WC50', '1971-08-30', N'M', N'M', '2009-01-25',3,1, '2014-06-30'),
    (N'360868122', N'adventure-works\zheng0', N'7AEA80', N'Production Supervisor - WC10', '1983-10-26', N'S', N'M', '2008-12-03',66,1, '2014-06-30'),
    (N'455563743', N'adventure-works\ebru0', N'7AEAAC', N'Production Technician - WC10', '1986-09-22', N'S', N'M', '2009-12-06',93,1, '2014-06-30'),
    (N'717889520', N'adventure-works\mary1', N'7AEAB4', N'Production Technician - WC10', '1986-09-19', N'M', N'F', '2009-12-25',94,1, '2014-06-30'),
    (N'801365500', N'adventure-works\kevin2', N'7AEABC', N'Production Technician - WC10', '1986-09-19', N'S', N'M', '2009-12-25',90,1, '2014-06-30'),
    (N'561196580', N'adventure-works\john4', N'7AEAC2', N'Production Technician - WC10', '1986-09-28', N'S', N'M', '2010-02-27',92,1, '2014-06-30'),
    (N'393421437', N'adventure-works\christopher0', N'7AEAC6', N'Production Technician - WC10', '1986-10-01', N'M', N'M', '2010-02-08',91,1, '2014-06-30'),
    (N'630184120', N'adventure-works\jinghao0', N'7AEB80', N'Production Supervisor - WC50', '1989-02-05', N'S', N'M', '2008-12-08',77,1, '2014-06-30'),
    (N'113695504', N'adventure-works\alice0', N'7AEBAC', N'Production Technician - WC50', '1978-01-26', N'M', N'F', '2008-12-07',95,1, '2014-06-30'),
    (N'857651804', N'adventure-works\jun0', N'7AEBB4', N'Production Technician - WC50', '1979-07-06', N'S', N'M', '2008-12-14',90,1, '2014-06-30'),
    (N'415823523', N'adventure-works\suroor0', N'7AEBBC', N'Production Technician - WC50', '1978-02-25', N'S', N'M', '2008-12-17',93,1, '2014-06-30'),
    (N'981597097', N'adventure-works\john1', N'7AEBC2', N'Production Technician - WC50', '1978-05-31', N'S', N'M', '2009-01-01',91,1, '2014-06-30'),
    (N'54759846', N'adventure-works\linda0', N'7AEBC6', N'Production Technician - WC50', '1987-07-17', N'M', N'F', '2008-12-25',96,1, '2014-06-30'),
    (N'342607223', N'adventure-works\mindaugas0', N'7AEBCA', N'Production Technician - WC50', '1978-05-07', N'M', N'M', '2009-01-13',97,1, '2014-06-30'),
    (N'563680513', N'adventure-works\angela0', N'7AEBCE', N'Production Technician - WC50', '1991-05-31', N'S', N'F', '2009-01-20',92,1, '2014-06-30'),
    (N'398737566', N'adventure-works\michael2', N'7AEBD1', N'Production Technician - WC50', '1974-05-03', N'S', N'M', '2009-01-31',98,1, '2014-06-30'),
    (N'599942664', N'adventure-works\chad0', N'7AEBD3', N'Production Technician - WC50', '1990-08-04', N'M', N'M', '2009-02-18',99,1, '2014-06-30'),
    (N'222400012', N'adventure-works\don0', N'7AEBD5', N'Production Technician - WC50', '1971-06-13', N'M', N'M', '2009-02-13',88,1, '2014-06-30'),
    (N'334834274', N'adventure-works\michael4', N'7AEBD7', N'Production Technician - WC50', '1989-06-15', N'S', N'M', '2009-02-25',94,1, '2014-06-30'),
    (N'211789056', N'adventure-works\kitti0', N'7AEBD9', N'Production Technician - WC50', '1987-06-06', N'S', N'F', '2009-03-04',89,1, '2014-06-30'),
    (N'521265716', N'adventure-works\pilar0', N'7AEC80', N'Shipping and Receiving Supervisor', '1972-09-09', N'S', N'M', '2009-01-02',93,1, '2014-06-30'),
    (N'586486572', N'adventure-works\susan0', N'7AECAC', N'Stocker', '1978-02-17', N'S', N'F', '2008-12-07',97,1, '2014-06-30'),
    (N'337752649', N'adventure-works\vamsi0', N'7AECB4', N'Shipping and Receiving Clerk', '1977-03-18', N'M', N'M', '2008-12-07',95,1, '2014-06-30'),
    (N'420776180', N'adventure-works\kim0', N'7AECBC', N'Stocker', '1984-04-30', N'S', N'F', '2008-12-26',98,1, '2014-06-30'),
    (N'584205124', N'adventure-works\matthias0', N'7AECC2', N'Shipping and Receiving Clerk', '1973-11-11', N'M', N'M', '2009-01-20',94,1, '2014-06-30'),
    (N'652779496', N'adventure-works\jimmy0', N'7AECC6', N'Stocker', '1985-05-04', N'M', N'M', '2009-02-26',96,1, '2014-06-30'),
    (N'750905084', N'adventure-works\david4', N'7AED80', N'Production Supervisor - WC40', '1983-07-02', N'S', N'M', '2009-01-03',73,1, '2014-06-30'),
    (N'384162788', N'adventure-works\paul0', N'7AEDAC', N'Production Technician - WC40', '1980-11-13', N'S', N'M', '2008-12-04',68,1, '2014-06-30'),
    (N'502058701', N'adventure-works\gary0', N'7AEDB4', N'Production Technician - WC40', '1988-05-16', N'S', N'M', '2008-12-22',69,1, '2014-06-30'),
    (N'578953538', N'adventure-works\rob1', N'7AEDBC', N'Production Technician - WC40', '1973-08-04', N'S', N'M', '2009-02-13',71,1, '2014-06-30'),
    (N'273260055', N'adventure-works\baris0', N'7AEDC2', N'Production Technician - WC40', '1990-10-07', N'S', N'M', '2009-02-15',72,1, '2014-06-30'),
    (N'1300049', N'adventure-works\nicole0', N'7AEDC6', N'Production Technician - WC40', '1986-04-09', N'M', N'F', '2009-02-22',67,1, '2014-06-30'),
    (N'830150469', N'adventure-works\michael1', N'7AEDCA', N'Production Technician - WC40', '1991-01-04', N'S', N'M', '2009-01-10',70,1, '2014-06-30'),
    (N'45615666', N'adventure-works\eric0', N'7AEE80', N'Production Supervisor - WC20', '1985-01-19', N'M', N'M', '2009-01-14',40,1, '2014-06-30'),
    (N'964089218', N'adventure-works\ivo0', N'7AEEAC', N'Production Technician - WC20', '1982-01-03', N'M', N'M', '2008-12-04',9,1, '2014-06-30'),
    (N'701156975', N'adventure-works\sylvester0', N'7AEEB4', N'Production Technician - WC20', '1970-11-12', N'M', N'M', '2009-12-11',4,1, '2014-06-30'),
    (N'63761469', N'adventure-works\anibal0', N'7AEEBC', N'Production Technician - WC20', '1974-09-05', N'S', N'F', '2009-02-23',8,1, '2014-06-30'),
    (N'25011600', N'adventure-works\samantha0', N'7AEEC2', N'Production Technician - WC20', '1987-11-22', N'M', N'F', '2009-02-04',7,1, '2014-06-30'),
    (N'113393530', N'adventure-works\hung-fu0', N'7AEEC6', N'Production Technician - WC20', '1971-10-23', N'S', N'M', '2010-01-06',0,1, '2014-06-30'),
    (N'339233463', N'adventure-works\prasanna0', N'7AEECA', N'Production Technician - WC20', '1953-04-30', N'M', N'M', '2010-01-22',5,1, '2014-06-30'),
    (N'872923042', N'adventure-works\min0', N'7AEECE', N'Production Technician - WC20', '1974-09-10', N'M', N'M', '2010-01-24',1,1, '2014-06-30'),
    (N'163347032', N'adventure-works\olinda0', N'7AEED1', N'Production Technician - WC20', '1970-04-04', N'S', N'F', '2010-03-04',3,1, '2014-06-30'),
    (N'56772045', N'adventure-works\krishna0', N'7AEED3', N'Production Technician - WC20', '1971-09-05', N'S', N'M', '2010-02-13',2,1, '2014-06-30'),
    (N'886023130', N'adventure-works\paul1', N'7AEED5', N'Production Technician - WC20', '1990-11-04', N'M', N'M', '2009-01-17',6,1, '2014-06-30'),
    (N'386315192', N'adventure-works\cynthia0', N'7AEF80', N'Production Supervisor - WC30', '1981-08-18', N'S', N'F', '2009-01-27',69,1, '2014-06-30'),
    (N'160739235', N'adventure-works\jianshuo0', N'7AEFAC', N'Production Technician - WC30', '1989-06-25', N'S', N'M', '2008-12-07',36,1, '2014-06-30'),
    (N'604664374', N'adventure-works\sandra0', N'7AEFB4', N'Production Technician - WC30', '1975-11-05', N'M', N'F', '2008-12-26',37,1, '2014-06-30'),
    (N'733022683', N'adventure-works\jason0', N'7AEFBC', N'Production Technician - WC30', '1988-12-07', N'S', N'M', '2009-01-14',38,1, '2014-06-30'),
    (N'764853868', N'adventure-works\andy0', N'7AEFC2', N'Production Technician - WC30', '1983-10-20', N'M', N'M', '2009-01-31',39,1, '2014-06-30'),
    (N'878395493', N'adventure-works\michael5', N'7AEFC6', N'Production Technician - WC30', '1982-09-18', N'M', N'M', '2009-02-26',35,1, '2014-06-30'),
    (N'993310268', N'adventure-works\rostislav0', N'7AEFCA', N'Production Technician - WC30', '1977-09-13', N'M', N'M', '2009-02-19',40,1, '2014-06-30'),
    (N'319472946', N'adventure-works\yuhong0', N'7AF044', N'Production Supervisor - WC20', '1977-04-06', N'M', N'M', '2009-02-01',41,1, '2014-06-30'),
    (N'568596888', N'adventure-works\hanying0', N'7AF04560', N'Production Technician - WC20', '1974-10-16', N'S', N'M', '2008-12-16',15,1, '2014-06-30'),
    (N'97728960', N'adventure-works\raymond0', N'7AF045A0', N'Production Technician - WC20', '1967-03-02', N'M', N'M', '2008-12-23',10,1, '2014-06-30'),
    (N'212801092', N'adventure-works\fadi0', N'7AF045E0', N'Production Technician - WC20', '1989-02-15', N'S', N'M', '2009-01-04',16,1, '2014-06-30'),
    (N'322160340', N'adventure-works\lane0', N'7AF04610', N'Production Technician - WC20', '1974-09-23', N'M', N'M', '2009-01-11',11,1, '2014-06-30'),
    (N'812797414', N'adventure-works\linda1', N'7AF04630', N'Production Technician - WC20', '1977-10-05', N'S', N'F', '2009-02-03',12,1, '2014-06-30'),
    (N'300946911', N'adventure-works\shelley0', N'7AF04650', N'Production Technician - WC20', '1986-12-08', N'S', N'F', '2009-03-07',14,1, '2014-06-30'),
    (N'404159499', N'adventure-works\terrence0', N'7AF04670', N'Production Technician - WC20', '1984-12-08', N'S', N'M', '2009-02-16',13,1, '2014-06-30'),
    (N'712885347', N'adventure-works\jeff0', N'7AF04C', N'Production Supervisor - WC45', '1977-01-15', N'M', N'M', '2009-01-21',74,1, '2014-06-30'),
    (N'275962311', N'adventure-works\kirk0', N'7AF04D60', N'Production Technician - WC45', '1985-02-06', N'S', N'M', '2008-12-15',74,1, '2014-06-30'),
    (N'514829225', N'adventure-works\laura0', N'7AF04DA0', N'Production Technician - WC45', '1980-12-25', N'S', N'F', '2009-01-03',75,1, '2014-06-30'),
    (N'377784364', N'adventure-works\alex0', N'7AF04DE0', N'Production Technician - WC45', '1990-04-13', N'M', N'M', '2009-02-08',77,1, '2014-06-30'),
    (N'65848458', N'adventure-works\andrew1', N'7AF04E10', N'Production Technician - WC45', '1988-09-24', N'S', N'M', '2009-03-06',73,1, '2014-06-30'),
    (N'539490372', N'adventure-works\chris0', N'7AF04E30', N'Production Technician - WC45', '1988-12-16', N'M', N'M', '2009-01-22',76,1, '2014-06-30'),
    (N'60114406', N'adventure-works\jack0', N'7AF054', N'Production Supervisor - WC30', '1983-06-22', N'S', N'M', '2009-02-21',68,1, '2014-06-30'),
    (N'498138869', N'adventure-works\david1', N'7AF05560', N'Production Technician - WC30', '1979-11-02', N'S', N'M', '2008-12-02',25,1, '2014-06-30'),
    (N'271438431', N'adventure-works\garrett0', N'7AF055A0', N'Production Technician - WC30', '1984-08-25', N'S', N'M', '2008-12-07',34,1, '2014-06-30'),
    (N'351069889', N'adventure-works\susan1', N'7AF055E0', N'Production Technician - WC30', '1983-04-02', N'S', N'F', '2008-12-14',31,1, '2014-06-30'),
    (N'476115505', N'adventure-works\george0', N'7AF05610', N'Production Technician - WC30', '1977-04-16', N'M', N'M', '2008-12-21',26,1, '2014-06-30'),
    (N'746373306', N'adventure-works\david3', N'7AF05630', N'Production Technician - WC30', '1981-08-03', N'S', N'M', '2009-01-02',32,1, '2014-06-30'),
    (N'364818297', N'adventure-works\marc0', N'7AF05650', N'Production Technician - WC30', '1986-10-24', N'M', N'M', '2009-01-16',22,1, '2014-06-30'),
    (N'87268837', N'adventure-works\eugene0', N'7AF05670', N'Production Technician - WC30', '1987-07-15', N'S', N'M', '2009-01-21',33,1, '2014-06-30'),
    (N'585408256', N'adventure-works\benjamin0', N'7AF05688', N'Production Technician - WC30', '1986-01-05', N'S', N'M', '2009-01-27',28,1, '2014-06-30'),
    (N'259388196', N'adventure-works\reed0', N'7AF05698', N'Production Technician - WC30', '1989-01-08', N'M', N'M', '2009-02-02',23,1, '2014-06-30'),
    (N'860123571', N'adventure-works\david7', N'7AF056A8', N'Production Technician - WC30', '1985-09-23', N'M', N'M', '2009-02-14',29,1, '2014-06-30'),
    (N'551346974', N'adventure-works\russell1', N'7AF056B8', N'Production Technician - WC30', '1982-02-11', N'M', N'M', '2009-02-21',24,1, '2014-06-30'),
    (N'568626529', N'adventure-works\john3', N'7AF056C8', N'Production Technician - WC30', '1982-03-24', N'S', N'M', '2009-03-03',27,1, '2014-06-30'),
    (N'587567941', N'adventure-works\jan0', N'7AF056D8', N'Production Technician - WC30', '1974-11-15', N'S', N'M', '2009-03-05',30,1, '2014-06-30'),
    (N'862951447', N'adventure-works\katie0', N'7AF05C', N'Production Supervisor - WC20', '1984-11-18', N'S', N'F', '2009-02-20',42,1, '2014-06-30'),
    (N'545337468', N'adventure-works\michael0', N'7AF05D60', N'Production Technician - WC20', '1984-11-17', N'S', N'M', '2008-12-09',20,1, '2014-06-30'),
    (N'368920189', N'adventure-works\nitin0', N'7AF05DA0', N'Production Technician - WC20', '1986-12-01', N'S', N'M', '2008-12-28',21,1, '2014-06-30'),
    (N'969985265', N'adventure-works\barbara0', N'7AF05DE0', N'Production Technician - WC20', '1979-07-02', N'M', N'F', '2009-01-22',17,1, '2014-06-30'),
    (N'305522471', N'adventure-works\john2', N'7AF05E10', N'Production Technician - WC20', '1986-04-05', N'M', N'M', '2009-02-09',18,1, '2014-06-30'),
    (N'621932914', N'adventure-works\stefen0', N'7AF05E30', N'Production Technician - WC20', '1975-12-21', N'S', N'M', '2009-02-28',19,1, '2014-06-30'),
    (N'551834634', N'adventure-works\shane0', N'7AF064', N'Production Supervisor - WC45', '1990-05-24', N'S', N'M', '2009-02-08',75,1, '2014-06-30'),
    (N'713403643', N'adventure-works\yvonne0', N'7AF06560', N'Production Technician - WC45', '1989-04-15', N'M', N'F', '2008-12-09',79,1, '2014-06-30'),
    (N'435234965', N'adventure-works\douglas0', N'7AF065A0', N'Production Technician - WC45', '1985-11-24', N'M', N'M', '2008-12-27',80,1, '2014-06-30'),
    (N'187369436', N'adventure-works\janeth0', N'7AF065E0', N'Production Technician - WC45', '1972-07-24', N'S', N'F', '2009-01-15',81,1, '2014-06-30'),
    (N'456839592', N'adventure-works\robert0', N'7AF06610', N'Production Technician - WC45', '1985-02-28', N'S', N'M', '2009-02-02',82,1, '2014-06-30'),
    (N'399658727', N'adventure-works\lionel0', N'7AF06630', N'Production Technician - WC45', '1988-03-14', N'S', N'M', '2009-02-26',78,1, '2014-06-30'),
    (N'634335025', N'adventure-works\brenda0', N'7AF06C', N'Production Supervisor - WC40', '1983-02-28', N'M', N'F', '2009-03-05',71,1, '2014-06-30'),
    (N'761597760', N'adventure-works\alejandro0', N'7AF06D60', N'Production Technician - WC40', '1988-12-05', N'S', N'M', '2008-12-06',52,1, '2014-06-30'),
    (N'295971920', N'adventure-works\fred0', N'7AF06DA0', N'Production Technician - WC40', '1989-06-25', N'S', N'M', '2008-12-12',47,1, '2014-06-30'),
    (N'918737118', N'adventure-works\kevin1', N'7AF06DE0', N'Production Technician - WC40', '1985-12-25', N'S', N'M', '2008-12-17',58,1, '2014-06-30'),
    (N'370487086', N'adventure-works\shammi0', N'7AF06E10', N'Production Technician - WC40', '1980-10-04', N'M', N'M', '2008-12-24',53,1, '2014-06-30'),
    (N'632092621', N'adventure-works\rajesh0', N'7AF06E30', N'Production Technician - WC40', '1977-10-04', N'M', N'M', '2008-12-31',48,1, '2014-06-30'),
    (N'19312190', N'adventure-works\lorraine0', N'7AF06E50', N'Production Technician - WC40', '1988-11-26', N'M', N'F', '2009-01-04',49,1, '2014-06-30'),
    (N'992874797', N'adventure-works\paula1', N'7AF06E70', N'Production Technician - WC40', '1987-02-10', N'M', N'F', '2009-01-12',54,1, '2014-06-30'),
    (N'749211824', N'adventure-works\frank0', N'7AF06E88', N'Production Technician - WC40', '1987-09-06', N'M', N'M', '2009-01-17',56,1, '2014-06-30'),
    (N'746201340', N'adventure-works\brian0', N'7AF06E98', N'Production Technician - WC40', '1977-02-10', N'S', N'M', '2009-01-29',55,1, '2014-06-30'),
    (N'436757988', N'adventure-works\tawana0', N'7AF06EA8', N'Production Technician - WC40', '1989-11-10', N'S', N'M', '2009-02-05',50,1, '2014-06-30'),
    (N'693168613', N'adventure-works\ken1', N'7AF06EB8', N'Production Technician - WC40', '1981-05-28', N'M', N'M', '2009-02-24',51,1, '2014-06-30'),
    (N'440379437', N'adventure-works\gabe0', N'7AF06EC8', N'Production Technician - WC40', '1988-05-10', N'M', N'M', '2009-03-08',57,1, '2014-06-30'),
    (N'332349500', N'adventure-works\lori0', N'7AF074', N'Production Supervisor - WC45', '1980-07-18', N'S', N'F', '2009-02-26',76,1, '2014-06-30'),
    (N'835460180', N'adventure-works\stuart0', N'7AF07560', N'Production Technician - WC45', '1962-09-13', N'S', N'M', '2008-12-02',84,1, '2014-06-30'),
    (N'687685941', N'adventure-works\greg0', N'7AF075A0', N'Production Technician - WC45', '1970-10-18', N'S', N'M', '2008-12-02',85,1, '2014-06-30'),
    (N'199546871', N'adventure-works\scott0', N'7AF075E0', N'Production Technician - WC45', '1987-02-10', N'M', N'M', '2009-01-08',86,1, '2014-06-30'),
    (N'167554340', N'adventure-works\kathie0', N'7AF07610', N'Production Technician - WC45', '1990-11-01', N'M', N'F', '2009-01-27',87,1, '2014-06-30'),
    (N'20244403', N'adventure-works\belinda0', N'7AF07630', N'Production Technician - WC45', '1969-09-17', N'S', N'F', '2009-02-20',83,1, '2014-06-30'),
    (N'398223854', N'adventure-works\hazem0', N'7B40', N'Quality Assurance Manager', '1977-10-26', N'S', N'M', '2009-02-28',80,1, '2014-12-26'),
    (N'885055826', N'adventure-works\peng0', N'7B56', N'Quality Assurance Supervisor', '1976-03-18', N'M', N'M', '2008-12-09',81,1, '2014-06-30'),
    (N'343861179', N'adventure-works\sootha0', N'7B56B0', N'Quality Assurance Technician', '1966-12-05', N'M', N'M', '2010-02-23',85,1, '2014-06-30'),
    (N'131471224', N'adventure-works\andreas0', N'7B56D0', N'Quality Assurance Technician', '1989-03-28', N'M', N'M', '2009-02-02',84,1, '2014-06-30'),
    (N'381772114', N'adventure-works\mark0', N'7B56F0', N'Quality Assurance Technician', '1986-04-30', N'S', N'M', '2009-01-15',83,1, '2014-06-30'),
    (N'403414852', N'adventure-works\sean0', N'7B5708', N'Quality Assurance Technician', '1976-03-06', N'S', N'M', '2008-12-28',82,1, '2014-06-30'),
    (N'345106466', N'adventure-works\zainal0', N'7B5A', N'Document Control Manager', '1976-01-30', N'M', N'M', '2009-01-04',77,1, '2014-06-30'),
    (N'540688287', N'adventure-works\tengiz0', N'7B5AB0', N'Control Specialist', '1990-04-28', N'S', N'M', '2008-12-16',76,1, '2014-06-30'),
    (N'242381745', N'adventure-works\sean1', N'7B5AD0', N'Document Control Assistant', '1987-03-12', N'S', N'M', '2009-01-22',78,1, '2014-06-30'),
    (N'260770918', N'adventure-works\karen0', N'7B5AF0', N'Document Control Assistant', '1975-12-25', N'M', N'F', '2009-02-09',79,1, '2014-06-30'),
    (N'260805477', N'adventure-works\chris1', N'7B5B08', N'Control Specialist', '1987-05-26', N'M', N'M', '2009-03-06',75,1, '2014-06-30'),
    (N'685233686', N'adventure-works\ascott0', N'7BC0', N'Master Scheduler', '1968-09-17', N'S', N'M', '2008-12-12',44,1, '2014-12-26'),
    (N'981495526', N'adventure-works\sairaj0', N'7BD6', N'Scheduling Assistant', '1987-12-22', N'M', N'M', '2009-01-26',46,1, '2014-06-30'),
    (N'621209647', N'adventure-works\william0', N'7BDA', N'Scheduling Assistant', '1981-11-06', N'M', N'M', '2009-01-07',45,1, '2014-06-30'),
    (N'470689086', N'adventure-works\alan0', N'7BDE', N'Scheduling Assistant', '1984-03-29', N'M', N'M', '2009-02-13',47,1, '2014-06-30'),
    (N'368691270', N'adventure-works\brian1', N'7BE1', N'Scheduling Assistant', '1984-08-11', N'M', N'M', '2009-03-03',48,1, '2014-06-30'),
    (N'141165819', N'adventure-works\gary1', N'7C20', N'Facilities Manager', '1971-02-18', N'M', N'M', '2009-12-02',86,1, '2014-06-30'),
    (N'553069203', N'adventure-works\christian0', N'7C2B', N'Maintenance Supervisor', '1976-01-18', N'M', N'M', '2008-12-14',92,1, '2014-06-30'),
    (N'879334904', N'adventure-works\lori1', N'7C2B58', N'Janitor', '1970-07-31', N'M', N'F', '2010-02-16',90,1, '2014-06-30'),
    (N'28414965', N'adventure-works\stuart1', N'7C2B68', N'Janitor', '1971-12-17', N'M', N'M', '2010-03-05',88,1, '2014-06-30'),
    (N'153479919', N'adventure-works\jo1', N'7C2B78', N'Janitor', '1954-04-24', N'M', N'F', '2010-03-07',91,1, '2014-06-30'),
    (N'646304055', N'adventure-works\pat0', N'7C2B84', N'Janitor', '1970-12-03', N'S', N'M', '2010-01-27',89,1, '2014-06-30'),
    (N'552560652', N'adventure-works\magnus0', N'7C2D', N'Facilities Administrative Assistant', '1971-08-27', N'M', N'M', '2009-12-21',87,1, '2014-06-30'),
    (N'184188301', N'adventure-works\laura1', N'84', N'Chief Financial Officer', '1976-01-06', N'M', N'F', '2009-01-31',0,1, '2014-06-30'),
    (N'535145551', N'adventure-works\paula0', N'8560', N'Human Resources Manager', '1976-02-11', N'M', N'F', '2008-12-06',54,1, '2014-06-30'),
    (N'476980013', N'adventure-works\grant0', N'856B', N'Human Resources Administrative Assistant', '1976-04-16', N'S', N'M', '2009-02-25',53,1, '2014-06-30'),
    (N'416679555', N'adventure-works\hao0', N'856D', N'Human Resources Administrative Assistant', '1977-04-17', N'S', N'M', '2009-02-06',52,1, '2014-06-30'),
    (N'264306399', N'adventure-works\vidur0', N'856F', N'Recruiter', '1984-08-01', N'S', N'M', '2009-01-01',50,1, '2014-06-30'),
    (N'619308550', N'adventure-works\mindy0', N'857080', N'Benefits Specialist', '1984-11-20', N'M', N'F', '2008-12-25',51,1, '2014-06-30'),
    (N'332040978', N'adventure-works\willis0', N'857180', N'Recruiter', '1978-07-18', N'S', N'M', '2008-12-13',49,1, '2014-06-30'),
    (N'30845', N'adventure-works\david6', N'85A0', N'Accounts Manager', '1983-07-08', N'M', N'M', '2009-01-30',57,1, '2014-06-30'),
    (N'363923697', N'adventure-works\deborah0', N'85AB', N'Accounts Receivable Specialist', '1976-03-06', N'M', N'F', '2008-12-18',60,1, '2014-06-30'),
    (N'60517918', N'adventure-works\candy0', N'85AD', N'Accounts Receivable Specialist', '1976-02-23', N'S', N'F', '2009-01-06',61,1, '2014-06-30'),
    (N'931190412', N'adventure-works\bryan1', N'85AF', N'Accounts Receivable Specialist', '1984-09-20', N'S', N'M', '2009-01-24',62,1, '2014-06-30'),
    (N'363910111', N'adventure-works\barbara1', N'85B080', N'Accountant', '1976-01-04', N'M', N'F', '2009-02-18',58,1, '2014-06-30'),
    (N'663843431', N'adventure-works\dragan0', N'85B180', N'Accounts Payable Specialist', '1977-02-14', N'M', N'M', '2009-02-11',63,1, '2014-06-30'),
    (N'519756660', N'adventure-works\janet0', N'85B280', N'Accounts Payable Specialist', '1979-03-09', N'M', N'F', '2009-03-01',64,1, '2014-06-30'),
    (N'480951955', N'adventure-works\mike0', N'85B380', N'Accountant', '1979-07-01', N'S', N'M', '2009-03-08',59,1, '2014-06-30'),
    (N'121491555', N'adventure-works\wendy0', N'85E0', N'Finance Manager', '1984-10-11', N'S', N'F', '2008-12-25',55,1, '2014-06-30'),
    (N'895209680', N'adventure-works\sheela0', N'85EB', N'Purchasing Manager', '1978-02-10', N'S', N'F', '2011-02-25',49,1, '2014-06-30'),
    (N'603686790', N'adventure-works\mikael0', N'85EB58', N'Buyer', '1984-08-17', N'S', N'M', '2009-02-10',59,1, '2014-06-30'),
    (N'792847334', N'adventure-works\arvind0', N'85EB68', N'Buyer', '1974-08-21', N'M', N'M', '2009-02-28',60,1, '2014-06-30'),
    (N'407505660', N'adventure-works\linda2', N'85EB78', N'Buyer', '1970-11-30', N'M', N'F', '2009-12-17',56,1, '2014-06-30'),
    (N'482810518', N'adventure-works\fukiko0', N'85EB84', N'Buyer', '1970-11-24', N'M', N'M', '2010-01-04',57,1, '2014-06-30'),
    (N'466142721', N'adventure-works\gordon0', N'85EB8C', N'Buyer', '1966-11-29', N'M', N'M', '2010-01-11',52,1, '2014-06-30'),
    (N'367453993', N'adventure-works\frank2', N'85EB94', N'Buyer', '1952-05-12', N'M', N'M', '2010-01-23',58,1, '2014-06-30'),
    (N'381073001', N'adventure-works\eric2', N'85EB9C', N'Buyer', '1972-09-17', N'S', N'M', '2010-01-27',54,1, '2014-06-30'),
    (N'785853949', N'adventure-works\erin0', N'85EBA2', N'Buyer', '1971-01-04', N'S', N'F', '2010-01-31',53,1, '2014-06-30'),
    (N'20269531', N'adventure-works\ben0', N'85EBA6', N'Buyer', '1973-06-03', N'M', N'M', '2010-03-09',55,1, '2014-06-30'),
    (N'437296311', N'adventure-works\annette0', N'85EBAA', N'Purchasing Assistant', '1978-01-29', N'M', N'F', '2010-12-06',50,1, '2014-06-30'),
    (N'280633567', N'adventure-works\reinout0', N'85EBAE', N'Purchasing Assistant', '1978-01-17', N'M', N'M', '2010-12-25',51,1, '2014-06-30'),
    (N'231203233', N'adventure-works\david5', N'8610', N'Assistant to the Chief Financial Officer', '1964-06-21', N'S', N'M', '2009-01-12',56,1, '2014-06-30'),
    (N'441044382', N'adventure-works\jean0', N'8C', N'Information Services Manager', '1975-12-13', N'S', N'F', '2008-12-11',65,1, '2014-06-30'),
    (N'858323870', N'adventure-works\stephanie0', N'8D60', N'Network Manager', '1984-03-25', N'S', N'F', '2009-02-04',68,1, '2014-06-30'),
    (N'749389530', N'adventure-works\ashvini0', N'8D6B', N'Network Administrator', '1977-03-27', N'S', N'M', '2008-12-04',70,1, '2014-06-30'),
    (N'672243793', N'adventure-works\peter1', N'8D6D', N'Network Administrator', '1980-05-28', N'S', N'M', '2009-02-23',69,1, '2014-06-30'),
    (N'58317344', N'adventure-works\karen1', N'8DA0', N'Application Specialist', '1978-05-19', N'S', N'F', '2009-02-16',74,1, '2014-06-30'),
    (N'314747499', N'adventure-works\ramesh0', N'8DE0', N'Application Specialist', '1988-03-13', N'S', N'M', '2009-02-03',73,1, '2014-06-30'),
    (N'671089628', N'adventure-works\dan0', N'8E10', N'Application Specialist', '1987-05-26', N'M', N'M', '2009-01-11',72,1, '2014-06-30'),
    (N'643805155', N'adventure-works\fran√ßois0', N'8E30', N'Database Administrator', '1975-05-17', N'S', N'M', '2009-01-17',67,1, '2014-06-30'),
    (N'929666391', N'adventure-works\dan1', N'8E50', N'Database Administrator', '1976-01-06', N'M', N'M', '2009-01-22',66,1, '2014-06-30'),
    (N'525932996', N'adventure-works\janaina0', N'8E70', N'Application Specialist', '1985-01-30', N'M', N'F', '2008-12-23',71,1, '2014-06-30'),
    (N'112432117', N'adventure-works\brian3', N'94', N'Vice President of Sales', '1977-06-06', N'S', N'M', '2011-02-15',10,1, '2014-06-30'),
    (N'502097814', N'adventure-works\stephen0', N'9560', N'North American Sales Manager', '1951-10-17', N'M', N'M', '2011-01-04',14,1, '2014-06-30'),
    (N'841560125', N'adventure-works\michael9', N'956B', N'Sales Representative', '1968-12-25', N'S', N'M', '2011-05-31',38,1, '2014-06-30'),
    (N'191644724', N'adventure-works\linda3', N'956D', N'Sales Representative', '1980-02-27', N'M', N'F', '2011-05-31',27,1, '2014-06-30'),
    (N'615389812', N'adventure-works\jillian0', N'956F', N'Sales Representative', '1962-08-29', N'S', N'F', '2011-05-31',24,1, '2014-06-30'),
    (N'234474252', N'adventure-works\garrett1', N'957080', N'Sales Representative', '1975-02-04', N'M', N'M', '2011-05-31',33,1, '2014-06-30'),
    (N'716374314', N'adventure-works\tsvi0', N'957180', N'Sales Representative', '1974-01-18', N'M', N'M', '2011-05-31',29,1, '2014-06-30'),
    (N'61161660', N'adventure-works\pamela0', N'957280', N'Sales Representative', '1974-12-06', N'S', N'F', '2011-05-31',22,1, '2014-06-30'),
    (N'139397894', N'adventure-works\shu0', N'957380', N'Sales Representative', '1968-03-09', N'M', N'M', '2011-05-31',26,1, '2014-06-30'),
    (N'399771412', N'adventure-works\jos√©1', N'957440', N'Sales Representative', '1963-12-11', N'M', N'M', '2011-05-31',31,1, '2014-06-30'),
    (N'987554265', N'adventure-works\david8', N'9574C0', N'Sales Representative', '1974-02-11', N'S', N'M', '2011-05-31',23,1, '2014-06-30'),
    (N'90836195', N'adventure-works\tete0', N'957540', N'Sales Representative', '1978-01-05', N'M', N'M', '2012-09-30',39,1, '2014-06-30'),
    (N'481044938', N'adventure-works\syed0', N'95A0', N'Pacific Sales Manager', '1975-01-11', N'M', N'M', '2013-03-14',20,1, '2014-06-30'),
    (N'758596752', N'adventure-works\lynn0', N'95AB', N'Sales Representative', '1977-02-14', N'S', N'F', '2013-05-30',36,1, '2014-06-30'),
    (N'982310417', N'adventure-works\amy0', N'95E0', N'European Sales Manager', '1957-09-20', N'M', N'F', '2012-04-16',21,1, '2014-06-30'),
    (N'954276278', N'adventure-works\rachel0', N'95EB', N'Sales Representative', '1975-07-09', N'S', N'F', '2013-05-30',35,1, '2014-06-30'),
    (N'668991357', N'adventure-works\jae0', N'95ED', N'Sales Representative', '1968-03-17', N'M', N'F', '2012-05-30',37,1, '2014-06-30'),
    (N'134219713', N'adventure-works\ranjit0', N'95EF', N'Sales Representative', '1975-09-30', N'S', N'M', '2012-05-30',34,1, '2014-06-30');

SET DATEFORMAT ymd;
INSERT INTO [dbo].[Person] (
    [BusinessEntityID], [PersonType], [NameStyle], [Title], [FirstName],
    [MiddleName], [LastName], [Suffix], [EmailPromotion], [AdditionalContactInfo],
    [Demographics], [rowguid], [ModifiedDate]
) VALUES
    (1, N'EM', 0, NULL, N'Ken', N'J', N'Sánchez', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '92C4279F-1207-48A3-8448-4636514EB7E2', CONVERT(datetime2, '2009-01-07T00:00:00', 127)),
    (2, N'EM', 0, NULL, N'Terri', N'Lee', N'Duffy', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D8763459-8AA8-47CC-AFF7-C9079AF79033', CONVERT(datetime2, '2008-01-24T00:00:00', 127)),
    (3, N'EM', 0, NULL, N'Roberto', NULL, N'Tamburello', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E1A2555E-0828-434B-A33B-6F38136A37DE', CONVERT(datetime2, '2007-11-04T00:00:00', 127)),
    (4, N'EM', 0, NULL, N'Rob', NULL, N'Walters', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F2D7CE06-38B3-4357-805B-F4B6B71C01FF', CONVERT(datetime2, '2007-11-28T00:00:00', 127)),
    (5, N'EM', 0, N'Ms.', N'Gail', N'A', N'Erickson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F3A3F6B4-AE3B-430C-A754-9F2231BA6FEF', CONVERT(datetime2, '2007-12-30T00:00:00', 127)),
    (6, N'EM', 0, N'Mr.', N'Jossef', N'H', N'Goldberg', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0DEA28FD-EFFE-482A-AFD3-B7E8F199D56F', CONVERT(datetime2, '2013-12-16T00:00:00', 127)),
    (7, N'EM', 0, NULL, N'Dylan', N'A', N'Miller', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C45E8AB8-01BE-4B76-B215-820C8368181A', CONVERT(datetime2, '2009-02-01T00:00:00', 127)),
    (8, N'EM', 0, NULL, N'Diane', N'L', N'Margheim', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A948E590-4A56-45A9-BC9A-160A1CC9D990', CONVERT(datetime2, '2008-12-22T00:00:00', 127)),
    (9, N'EM', 0, NULL, N'Gigi', N'N', N'Matthew', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5FC28C0E-6D36-4252-9846-05CAA0B1F6C5', CONVERT(datetime2, '2009-01-09T00:00:00', 127)),
    (10, N'EM', 0, NULL, N'Michael', NULL, N'Raheem', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CA2C740E-75B2-420C-9D4B-E3CBC6609604', CONVERT(datetime2, '2009-04-26T00:00:00', 127)),
    (11, N'EM', 0, NULL, N'Ovidiu', N'V', N'Cracium', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D2CC2577-EF6B-4408-BD8C-747337FE5645', CONVERT(datetime2, '2010-11-28T00:00:00', 127)),
    (12, N'EM', 0, NULL, N'Thierry', N'B', N'D''Hers', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FA263C7F-600D-4E89-8DCD-0978F3530F5F', CONVERT(datetime2, '2007-12-04T00:00:00', 127)),
    (13, N'EM', 0, N'Ms.', N'Janice', N'M', N'Galvin', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '34EB99E0-7042-4DC1-A2FD-BDA290FF0E07', CONVERT(datetime2, '2010-12-16T00:00:00', 127)),
    (14, N'EM', 0, NULL, N'Michael', N'I', N'Sullivan', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9A7501DE-5CAF-4700-AB07-CC81102BB696', CONVERT(datetime2, '2010-12-23T00:00:00', 127)),
    (15, N'EM', 0, NULL, N'Sharon', N'B', N'Salavaria', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BEBA63CB-13F1-4B76-A3DE-FE9AC283A94C', CONVERT(datetime2, '2011-01-11T00:00:00', 127)),
    (16, N'EM', 0, NULL, N'David', N'M', N'Bradley', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '2CC8BA72-5DBB-497F-B941-775517993638', CONVERT(datetime2, '2007-12-13T00:00:00', 127)),
    (17, N'EM', 0, NULL, N'Kevin', N'F', N'Brown', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9EE4713E-B3D8-4409-BC5E-DEC62497F43A', CONVERT(datetime2, '2007-01-19T00:00:00', 127)),
    (18, N'EM', 0, NULL, N'John', N'L', N'Wood', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FE21BDA7-9327-4D13-89CC-8CA5CEE8F21E', CONVERT(datetime2, '2011-01-31T00:00:00', 127)),
    (19, N'EM', 0, NULL, N'Mary', N'A', N'Dempsey', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '36F04305-6769-4E61-9143-048B2B7AFA20', CONVERT(datetime2, '2011-02-07T00:00:00', 127)),
    (20, N'EM', 0, NULL, N'Wanida', N'M', N'Benshoof', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1E7E56F4-A583-4E3F-A3FE-BA8A71096D10', CONVERT(datetime2, '2010-12-31T00:00:00', 127)),
    (21, N'EM', 0, NULL, N'Terry', N'J', N'Eminhizer', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6A288FE0-D9CB-4DA2-BEEA-51370220490C', CONVERT(datetime2, '2009-02-23T00:00:00', 127)),
    (22, N'EM', 0, NULL, N'Sariya', N'E', N'Harnpadoungsataya', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '276E0EB5-0CE7-443F-987F-B7EF293C0930', CONVERT(datetime2, '2008-12-05T00:00:00', 127)),
    (23, N'EM', 0, NULL, N'Mary', N'E', N'Gibson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B10CF8EB-5589-4A81-B1BD-051C5759CE06', CONVERT(datetime2, '2009-01-05T00:00:00', 127)),
    (24, N'EM', 0, N'Ms.', N'Jill', N'A', N'Williams', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3F605060-96AD-4201-9BE2-BFE7A3C4E468', CONVERT(datetime2, '2009-01-11T00:00:00', 127)),
    (25, N'EM', 0, NULL, N'James', N'R', N'Hamilton', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '423FE59B-7975-4A7E-B8D2-87105C286D4C', CONVERT(datetime2, '2009-01-27T00:00:00', 127)),
    (26, N'EM', 0, NULL, N'Peter', N'J', N'Krebs', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6F32F507-290E-4A96-A045-F445EAE4337F', CONVERT(datetime2, '2008-11-24T00:00:00', 127)),
    (27, N'EM', 0, NULL, N'Jo', N'A', N'Brown', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'EDD8ADD6-F7FD-4086-BFDC-B4FD3E6AD8D7', CONVERT(datetime2, '2008-02-20T00:00:00', 127)),
    (28, N'EM', 0, NULL, N'Guy', N'R', N'Gilbert', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D366A33A-8EDE-42BD-BF79-3E7FB9713FE1', CONVERT(datetime2, '2006-06-23T00:00:00', 127)),
    (29, N'EM', 0, NULL, N'Mark', N'K', N'McArthur', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '667FA3BA-B877-4BCC-A752-E461040E2DDD', CONVERT(datetime2, '2009-01-16T00:00:00', 127)),
    (30, N'EM', 0, NULL, N'Britta', N'L', N'Simon', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5090C9A8-E39E-4A4D-9133-14E7CF07998B', CONVERT(datetime2, '2009-01-22T00:00:00', 127)),
    (31, N'EM', 0, NULL, N'Margie', N'W', N'Shoop', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5EA8A3DA-7BEA-4ADC-85F5-C934E2033825', CONVERT(datetime2, '2008-12-28T00:00:00', 127)),
    (32, N'EM', 0, NULL, N'Rebecca', N'A', N'Laszlo', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '686BD5AF-6F30-4633-944B-3C1916CB4CA2', CONVERT(datetime2, '2008-12-22T00:00:00', 127)),
    (33, N'EM', 0, NULL, N'Annik', N'O', N'Stahl', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BFA2BDC8-208A-447B-8A4E-72A215E9E134', CONVERT(datetime2, '2008-12-10T00:00:00', 127)),
    (34, N'EM', 0, NULL, N'Suchitra', N'O', N'Mohan', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '7E239D8B-00BF-42D6-AC6B-4EA718DBC2B3', CONVERT(datetime2, '2009-02-09T00:00:00', 127)),
    (35, N'EM', 0, NULL, N'Brandon', N'G', N'Heidepriem', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4CC8EDC2-5502-4550-8089-9B3CC6006490', CONVERT(datetime2, '2009-02-01T00:00:00', 127)),
    (36, N'EM', 0, NULL, N'Jose', N'R', N'Lugo', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '54655E98-B81A-4041-8FA4-9349C018521F', CONVERT(datetime2, '2009-02-03T00:00:00', 127)),
    (37, N'EM', 0, NULL, N'Chris', N'O', N'Okelberry', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5B16CA18-703B-48E8-AB3E-CA158C854808', CONVERT(datetime2, '2009-02-28T00:00:00', 127)),
    (38, N'EM', 0, NULL, N'Kim', N'B', N'Abercrombie', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9A2163B3-2F4D-4F9A-91BD-07D326140F9C', CONVERT(datetime2, '2010-01-09T00:00:00', 127)),
    (39, N'EM', 0, NULL, N'Ed', N'R', N'Dudenhoefer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '83C88383-9C3E-42C5-AE09-965B5F9EF5B1', CONVERT(datetime2, '2010-01-29T00:00:00', 127)),
    (40, N'EM', 0, NULL, N'JoLynn', N'M', N'Dobney', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E8E5C9DA-F808-4439-BF5C-B28236BB8467', CONVERT(datetime2, '2007-12-19T00:00:00', 127)),
    (41, N'EM', 0, NULL, N'Bryan', NULL, N'Baker', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A32E8274-08DC-4596-AB1F-BA6263CBC676', CONVERT(datetime2, '2009-01-14T00:00:00', 127)),
    (42, N'EM', 0, NULL, N'James', N'D', N'Kramer', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F4F040E0-02A0-4905-B964-2E894D0F0B94', CONVERT(datetime2, '2008-12-20T00:00:00', 127)),
    (43, N'EM', 0, NULL, N'Nancy', N'A', N'Anderson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0681C8F9-D2EC-47D9-9BD4-751B14EAED50', CONVERT(datetime2, '2008-12-26T00:00:00', 127)),
    (44, N'EM', 0, NULL, N'Simon', N'D', N'Rapier', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FF8EDBFC-A9A3-4C5D-8AFF-DF4D6F18DAFF', CONVERT(datetime2, '2008-12-01T00:00:00', 127)),
    (45, N'EM', 0, NULL, N'Thomas', N'R', N'Michaels', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '2CAF9ADC-7A33-48CF-B4B7-3C57D9DE39C4', CONVERT(datetime2, '2009-02-19T00:00:00', 127)),
    (46, N'EM', 0, NULL, N'Eugene', N'O', N'Kogan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FB0068EA-49D4-4E82-9895-2680CF59385E', CONVERT(datetime2, '2009-02-01T00:00:00', 127)),
    (47, N'EM', 0, NULL, N'Andrew', N'R', N'Hill', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B6439F4F-E57D-4293-B0C8-8919591055A3', CONVERT(datetime2, '2009-02-15T00:00:00', 127)),
    (48, N'EM', 0, NULL, N'Ruth', N'Ann', N'Ellerbrock', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9BE35B50-2024-4785-9393-7C7AB8D9634D', CONVERT(datetime2, '2007-12-30T00:00:00', 127)),
    (49, N'EM', 0, NULL, N'Barry', N'K', N'Johnson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CB28BE2E-DE93-4409-BACE-DF6042D04815', CONVERT(datetime2, '2013-11-29T00:00:00', 127)),
    (50, N'EM', 0, NULL, N'Sidney', N'M', N'Higa', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A20BB9F5-3BBB-4AB6-9BFA-6AED32C30204', CONVERT(datetime2, '2008-01-26T00:00:00', 127)),
    (51, N'EM', 0, NULL, N'Jeffrey', N'L', N'Ford', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '293DF609-2FA3-4C00-A891-ACE08661FE70', CONVERT(datetime2, '2008-02-13T00:00:00', 127)),
    (52, N'EM', 0, NULL, N'Doris', N'M', N'Hartwig', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '075AB67F-BFBD-4433-B33E-C65B2C045B6B', CONVERT(datetime2, '2014-01-31T00:00:00', 127)),
    (53, N'EM', 0, NULL, N'Diane', N'R', N'Glimp', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9EA27F41-36BB-4456-A506-2B87C416FB0C', CONVERT(datetime2, '2008-03-21T00:00:00', 127)),
    (54, N'EM', 0, NULL, N'Bonnie', N'N', N'Kearney', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'AB066D28-404A-436E-A17A-789D7F2638D3', CONVERT(datetime2, '2009-12-25T00:00:00', 127)),
    (55, N'EM', 0, NULL, N'Taylor', N'R', N'Maxwell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9F81714B-0C9C-43C3-8DF5-180CB0A42D40', CONVERT(datetime2, '2013-12-31T00:00:00', 127)),
    (56, N'EM', 0, NULL, N'Denise', N'H', N'Smith', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '77F4AC33-E165-49D7-BC6A-F3C2DC069C9B', CONVERT(datetime2, '2009-01-29T00:00:00', 127)),
    (57, N'EM', 0, NULL, N'Frank', N'T', N'Miller', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '05516FB2-586E-43F5-A87C-5E236F8DB8AD', CONVERT(datetime2, '2009-02-16T00:00:00', 127)),
    (58, N'EM', 0, NULL, N'Kendall', N'C', N'Keil', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CA8D648A-166B-4D61-9AE0-A0B1FFECFB1B', CONVERT(datetime2, '2008-11-28T00:00:00', 127)),
    (59, N'EM', 0, NULL, N'Bob', N'N', N'Hohman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6384295C-C932-47C0-8E97-71D94F83CD31', CONVERT(datetime2, '2008-12-17T00:00:00', 127)),
    (60, N'EM', 0, NULL, N'Pete', N'C', N'Male', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0AEB5713-8BB0-4B8D-BF81-777158E5FF09', CONVERT(datetime2, '2009-01-04T00:00:00', 127)),
    (61, N'EM', 0, NULL, N'Diane', N'H', N'Tibbott', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E054FFC1-258C-4DA4-B2C1-40E155F4C88B', CONVERT(datetime2, '2009-01-11T00:00:00', 127)),
    (62, N'EM', 0, NULL, N'John', N'T', N'Campbell', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F5A4CC1B-AEFA-46CA-8888-2CA1C6D4034B', CONVERT(datetime2, '2014-02-07T00:00:00', 127)),
    (63, N'EM', 0, NULL, N'Maciej', N'W', N'Dusza', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '611D56EB-C29D-4F90-A5C6-1B3B2D44E5F8', CONVERT(datetime2, '2010-01-22T00:00:00', 127)),
    (64, N'EM', 0, NULL, N'Michael', N'J', N'Zwilling', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DCC5AA7B-C09A-47A5-86A2-90D88191EBE6', CONVERT(datetime2, '2010-02-16T00:00:00', 127)),
    (65, N'EM', 0, NULL, N'Randy', N'T', N'Reeves', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D988D8E0-C335-4F4C-9ED9-199FEB7FD4A7', CONVERT(datetime2, '2010-02-16T00:00:00', 127)),
    (66, N'EM', 0, NULL, N'Karan', N'R', N'Khanna', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '58C97993-7491-49EE-BFDD-CD80CE2C922A', CONVERT(datetime2, '2009-12-15T00:00:00', 127)),
    (67, N'EM', 0, NULL, N'Jay', N'G', N'Adams', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '2FE289A7-CE57-49E5-BE61-3E6580D22EA6', CONVERT(datetime2, '2009-02-26T00:00:00', 127)),
    (68, N'EM', 0, NULL, N'Charles', N'B', N'Fitzgerald', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1E7CD457-A854-4D68-ACFB-3D752EEBD231', CONVERT(datetime2, '2009-11-26T00:00:00', 127)),
    (69, N'EM', 0, NULL, N'Steve', N'F', N'Masters', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '85F55758-54C4-4048-ACBE-BD9515669122', CONVERT(datetime2, '2009-02-08T00:00:00', 127)),
    (70, N'EM', 0, NULL, N'David', N'J', N'Ortiz', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '60245B9F-EABD-48CF-9FA9-C5DD48BCB3DF', CONVERT(datetime2, '2008-12-08T00:00:00', 127)),
    (71, N'EM', 0, NULL, N'Michael', N'Sean', N'Ray', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CA42A0B4-C068-4C75-A8A3-DE8953760927', CONVERT(datetime2, '2009-02-08T00:00:00', 127)),
    (72, N'EM', 0, NULL, N'Steven', N'T', N'Selikoff', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '7455FEFF-9238-4C29-819F-A4DF6508C0F8', CONVERT(datetime2, '2008-11-24T00:00:00', 127)),
    (73, N'EM', 0, NULL, N'Carole', N'M', N'Poland', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4EE4FAF4-9C85-4022-B07F-956F1C5595D5', CONVERT(datetime2, '2008-12-12T00:00:00', 127)),
    (74, N'EM', 0, NULL, N'Bjorn', N'M', N'Rettig', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '056617D9-E65F-41C7-98AF-9C14C4569652', CONVERT(datetime2, '2008-12-31T00:00:00', 127)),
    (75, N'EM', 0, NULL, N'Michiko', N'F', N'Osada', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '670CE5A5-2B86-4AD7-AEF5-DF2AAAC4A6AB', CONVERT(datetime2, '2009-01-19T00:00:00', 127)),
    (76, N'EM', 0, NULL, N'Carol', N'M', N'Philips', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '7AC771B3-347A-49FA-9298-487B02FA359D', CONVERT(datetime2, '2009-02-05T00:00:00', 127)),
    (77, N'EM', 0, NULL, N'Merav', N'A', N'Netz', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F9427582-FD1B-45DE-922D-2C2C58CEA0F7', CONVERT(datetime2, '2009-02-24T00:00:00', 127)),
    (78, N'EM', 0, NULL, N'Reuben', N'H', N'D''sa', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'AEBEAA5B-D861-4703-A959-6D0F6F4CDF74', CONVERT(datetime2, '2008-12-08T00:00:00', 127)),
    (79, N'EM', 0, NULL, N'Eric', N'L', N'Brown', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D0EFAF3A-04DC-442F-9AA1-926C38105241', CONVERT(datetime2, '2010-01-17T00:00:00', 127)),
    (80, N'EM', 0, NULL, N'Sandeep', N'P', N'Kaliyath', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '7E5B79F9-5B7A-41DC-850F-23B4764B5161', CONVERT(datetime2, '2010-01-10T00:00:00', 127)),
    (81, N'EM', 0, NULL, N'Mihail', N'U', N'Frintu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1062CA88-998C-472C-8BD5-9F3D4BC4D2FB', CONVERT(datetime2, '2009-12-22T00:00:00', 127)),
    (82, N'EM', 0, NULL, N'Jack', N'T', N'Creasey', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'ED8B39AF-5DA5-405F-8C38-A6C3AC25CA1B', CONVERT(datetime2, '2010-02-24T00:00:00', 127)),
    (83, N'EM', 0, NULL, N'Patrick', N'M', N'Cook', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F03E1512-0DC2-4329-8F21-6C5DBB9996F3', CONVERT(datetime2, '2010-02-05T00:00:00', 127)),
    (84, N'EM', 0, NULL, N'Frank', N'R', N'Martinez', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B66AB99E-691C-4CAC-B62D-579816800BFA', CONVERT(datetime2, '2010-01-29T00:00:00', 127)),
    (85, N'EM', 0, NULL, N'Brian', N'Richard', N'Goldstein', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '61CE0232-4144-48AE-BA3B-1D509505F106', CONVERT(datetime2, '2009-12-04T00:00:00', 127)),
    (86, N'EM', 0, NULL, N'Ryan', N'L', N'Cornelsen', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0E3F7512-87A6-4C0D-9E02-30919A75BF60', CONVERT(datetime2, '2008-12-29T00:00:00', 127)),
    (87, N'EM', 0, NULL, N'Cristian', N'K', N'Petculescu', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '45FE0378-ED23-4D24-A5F5-11B10B402980', CONVERT(datetime2, '2008-12-15T00:00:00', 127)),
    (88, N'EM', 0, NULL, N'Betsy', N'A', N'Stadick', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C214AB1E-A07E-4A62-9033-19DDE0DE1937', CONVERT(datetime2, '2009-12-11T00:00:00', 127)),
    (89, N'EM', 0, NULL, N'Patrick', N'C', N'Wedge', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CD334AC3-EE11-4999-BBA1-7F33DE161124', CONVERT(datetime2, '2010-01-25T00:00:00', 127)),
    (90, N'EM', 0, NULL, N'Danielle', N'C', N'Tiedt', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '852A07EE-1D24-4810-8121-365DAB884C8D', CONVERT(datetime2, '2010-02-13T00:00:00', 127)),
    (91, N'EM', 0, NULL, N'Kimberly', N'B', N'Zimmerman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '67647D8C-C685-4704-81B7-AE1087CC2C6D', CONVERT(datetime2, '2010-01-05T00:00:00', 127)),
    (92, N'EM', 0, NULL, N'Tom', N'M', N'Vande Velde', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A36DA26B-29BA-4627-9731-134D65A53143', CONVERT(datetime2, '2010-03-03T00:00:00', 127)),
    (93, N'EM', 0, NULL, N'Kok-Ho', N'T', N'Loh', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3E4AE757-8C3E-47CC-BC20-528CCF271E3D', CONVERT(datetime2, '2008-12-20T00:00:00', 127)),
    (94, N'EM', 0, NULL, N'Russell', NULL, N'Hunter', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '288EF549-203C-439C-A6B2-32FD822327FF', CONVERT(datetime2, '2008-12-05T00:00:00', 127)),
    (95, N'EM', 0, NULL, N'Jim', N'H', N'Scardelis', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A45F89BB-9748-41F8-9FAA-A5D76DF4B029', CONVERT(datetime2, '2008-12-12T00:00:00', 127)),
    (96, N'EM', 0, NULL, N'Elizabeth', N'I', N'Keyser', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CB9A2A0C-68E5-475B-9D6D-EBD548FE5B9C', CONVERT(datetime2, '2009-02-23T00:00:00', 127)),
    (97, N'EM', 0, NULL, N'Mandar', N'H', N'Samant', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '907AE318-9FF7-4B56-B52B-5FDCA7BF8A62', CONVERT(datetime2, '2009-02-03T00:00:00', 127)),
    (98, N'EM', 0, NULL, N'Sameer', N'A', N'Tejani', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F19BE575-4A2E-413D-9BE9-0AF4B75C6FCD', CONVERT(datetime2, '2009-02-04T00:00:00', 127)),
    (99, N'EM', 0, NULL, N'Nuan', NULL, N'Yu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '938FCD64-E06D-453C-9E70-BFE41753557B', CONVERT(datetime2, '2008-12-30T00:00:00', 127)),
    (100, N'EM', 0, NULL, N'Lolan', N'B', N'Song', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DCD41870-A67B-4D7F-B874-92C6854E1D0D', CONVERT(datetime2, '2009-01-05T00:00:00', 127)),
    (101, N'EM', 0, NULL, N'Houman', N'N', N'Pournasseh', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '14E39720-994D-42C0-A26D-7877BB264FF5', CONVERT(datetime2, '2009-01-18T00:00:00', 127)),
    (102, N'EM', 0, NULL, N'Zheng', N'W', N'Mu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '17D15AE1-FB50-4303-A509-24C6C553C17D', CONVERT(datetime2, '2008-11-26T00:00:00', 127)),
    (103, N'EM', 0, NULL, N'Ebru', N'N', N'Ersan', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '64A9674E-9625-4EF2-8F4D-357390735C08', CONVERT(datetime2, '2009-11-29T00:00:00', 127)),
    (104, N'EM', 0, NULL, N'Mary', N'R', N'Baker', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9E450675-B7F1-47C1-BB64-95E998B57ED4', CONVERT(datetime2, '2009-12-18T00:00:00', 127)),
    (105, N'EM', 0, NULL, N'Kevin', N'M', N'Homer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1D1C9D2A-EA15-409C-8E32-8F5C6F8F286A', CONVERT(datetime2, '2009-12-18T00:00:00', 127)),
    (106, N'EM', 0, NULL, N'John', N'T', N'Kane', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4C117C8F-07C6-4B4E-A7D0-B8AD58EED8AE', CONVERT(datetime2, '2010-02-20T00:00:00', 127)),
    (107, N'EM', 0, NULL, N'Christopher', N'E', N'Hill', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A066751C-B57F-4186-8219-6652AE1189B1', CONVERT(datetime2, '2010-02-01T00:00:00', 127)),
    (108, N'EM', 0, NULL, N'Jinghao', N'K', N'Liu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DD35EB13-6D4F-4C82-B33C-D693090722B6', CONVERT(datetime2, '2008-12-01T00:00:00', 127)),
    (109, N'EM', 0, NULL, N'Alice', N'O', N'Ciccu', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B5836E66-4251-4D4D-8F49-8E6D9E3C9D64', CONVERT(datetime2, '2008-11-30T00:00:00', 127)),
    (110, N'EM', 0, NULL, N'Jun', N'T', N'Cao', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '20BDFF3B-3561-482B-B02D-DA9B6183B6D9', CONVERT(datetime2, '2008-12-07T00:00:00', 127)),
    (111, N'EM', 0, NULL, N'Suroor', N'R', N'Fatima', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '62C97C5B-5CAB-42C6-813D-C0C75BBBFD7B', CONVERT(datetime2, '2008-12-10T00:00:00', 127)),
    (112, N'EM', 0, NULL, N'John', N'P', N'Evans', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FCFA001C-D5BC-455F-9124-21290B4A5304', CONVERT(datetime2, '2008-12-25T00:00:00', 127)),
    (113, N'EM', 0, NULL, N'Linda', N'K', N'Moschell', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1552D700-1E1C-4725-8074-D8D7D9A49B0E', CONVERT(datetime2, '2008-12-18T00:00:00', 127)),
    (114, N'EM', 0, NULL, N'Mindaugas', N'J', N'Krapauskas', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '7C623D80-0A62-42DE-A355-24715A7160C6', CONVERT(datetime2, '2009-01-06T00:00:00', 127)),
    (115, N'EM', 0, NULL, N'Angela', N'W', N'Barbariol', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8E6ADFD0-2FE0-4EB9-93F2-A04FD877F6DC', CONVERT(datetime2, '2009-01-13T00:00:00', 127)),
    (116, N'EM', 0, NULL, N'Michael', N'W', N'Patten', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C31D89BE-1DB6-495B-8699-4669E5FA4B9D', CONVERT(datetime2, '2009-01-24T00:00:00', 127)),
    (117, N'EM', 0, NULL, N'Chad', N'W', N'Niswonger', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1CAC3A75-34AB-45C1-B9F1-7CAA44C76A75', CONVERT(datetime2, '2009-02-11T00:00:00', 127)),
    (118, N'EM', 0, NULL, N'Don', N'L', N'Hall', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F369194E-2FF0-4E61-97A7-B821F8B2BAED', CONVERT(datetime2, '2009-02-06T00:00:00', 127)),
    (119, N'EM', 0, NULL, N'Michael', N'T', N'Entin', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '7533C47D-8E8F-40AC-8383-CF3DF57E3D84', CONVERT(datetime2, '2009-02-18T00:00:00', 127)),
    (120, N'EM', 0, NULL, N'Kitti', N'H', N'Lertpiriyasuwat', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BFADC42C-0D0E-4BDA-94B7-B14E89F5FC69', CONVERT(datetime2, '2009-02-25T00:00:00', 127)),
    (121, N'EM', 0, NULL, N'Pilar', N'G', N'Ackerman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '81F50324-D0B5-4EA5-8B20-F99D46572C76', CONVERT(datetime2, '2008-12-26T00:00:00', 127)),
    (122, N'EM', 0, NULL, N'Susan', N'W', N'Eaton', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '84C723AC-F07F-4299-89B1-FAABB9CA47FC', CONVERT(datetime2, '2008-11-30T00:00:00', 127)),
    (123, N'EM', 0, NULL, N'Vamsi', N'N', N'Kuppa', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '37154EFE-002F-4D85-A84B-A8B1FF714BC6', CONVERT(datetime2, '2008-11-30T00:00:00', 127)),
    (124, N'EM', 0, NULL, N'Kim', N'T', N'Ralls', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '7AB39816-FF5A-4F3D-A3DC-88D906EC054F', CONVERT(datetime2, '2008-12-19T00:00:00', 127)),
    (125, N'EM', 0, NULL, N'Matthias', N'T', N'Berndt', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '318BF718-6CCC-4F26-AC81-00EAF17B1BFE', CONVERT(datetime2, '2009-01-13T00:00:00', 127)),
    (126, N'EM', 0, NULL, N'Jimmy', N'T', N'Bischoff', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '52182904-CFAF-4B2D-8D77-268DE7AB5DA4', CONVERT(datetime2, '2009-02-19T00:00:00', 127)),
    (127, N'EM', 0, NULL, N'David', N'P', N'Hamilton', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D6532F6D-A832-4266-B0F0-2F874F5A85BA', CONVERT(datetime2, '2008-12-27T00:00:00', 127)),
    (128, N'EM', 0, NULL, N'Paul', N'B', N'Komosinski', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E4145F40-88D2-43F4-A018-9BFB38D6D9FF', CONVERT(datetime2, '2008-11-27T00:00:00', 127)),
    (129, N'EM', 0, NULL, N'Gary', N'W', N'Yukish', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '09EA8640-ED99-4205-9433-078F1D81D221', CONVERT(datetime2, '2008-12-15T00:00:00', 127)),
    (130, N'EM', 0, NULL, N'Rob', N'T', N'Caron', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '909F01EF-2F6D-44DD-AF8D-1553BED7EACF', CONVERT(datetime2, '2009-02-06T00:00:00', 127)),
    (131, N'EM', 0, NULL, N'Baris', N'F', N'Cetinok', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BA03C1A4-0577-44F5-BFBC-FC701C007F13', CONVERT(datetime2, '2009-02-08T00:00:00', 127)),
    (132, N'EM', 0, NULL, N'Nicole', N'B', N'Holliday', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F101C5EE-17B1-4FB0-9CAE-4A88694CEEAA', CONVERT(datetime2, '2009-02-15T00:00:00', 127)),
    (133, N'EM', 0, NULL, N'Michael', N'L', N'Rothkugel', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'EE04F4CB-977A-48D0-ACB4-D89172FD5AE2', CONVERT(datetime2, '2009-01-03T00:00:00', 127)),
    (134, N'EM', 0, NULL, N'Eric', NULL, N'Gubbels', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4A9AC912-9E22-47B7-B126-5F3B576A430E', CONVERT(datetime2, '2009-01-07T00:00:00', 127)),
    (135, N'EM', 0, NULL, N'Ivo', N'William', N'Salmre', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DFA4B008-D83F-4828-AAC6-449E683909F5', CONVERT(datetime2, '2008-11-27T00:00:00', 127)),
    (136, N'EM', 0, NULL, N'Sylvester', N'A', N'Valdez', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6152BEC3-BB11-43AA-B691-9881CE6DD3F3', CONVERT(datetime2, '2009-12-04T00:00:00', 127)),
    (137, N'EM', 0, NULL, N'Anibal', N'T', N'Sousa', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9B75A538-958C-4C91-987A-B9BCCE683E8A', CONVERT(datetime2, '2009-02-16T00:00:00', 127)),
    (138, N'EM', 0, NULL, N'Samantha', N'H', N'Smith', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '11EF6F0A-29B6-4541-82F0-2E7A35179E48', CONVERT(datetime2, '2009-01-28T00:00:00', 127)),
    (139, N'EM', 0, N'Mr.', N'Hung-Fu', N'T', N'Ting', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '739C7553-13E5-4F36-86F6-4775645547AE', CONVERT(datetime2, '2009-12-30T00:00:00', 127)),
    (140, N'EM', 0, NULL, N'Prasanna', N'E', N'Samarawickrama', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E8673E92-31F6-42F4-A65C-A595D3F899F6', CONVERT(datetime2, '2010-01-15T00:00:00', 127)),
    (141, N'EM', 0, NULL, N'Min', N'G', N'Su', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3BB0243E-11E3-4E5C-8DB5-6B927C71432E', CONVERT(datetime2, '2010-01-17T00:00:00', 127)),
    (142, N'EM', 0, NULL, N'Olinda', N'C', N'Turner', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BF741CEC-34E0-4B44-99C2-E0382FCD9E8E', CONVERT(datetime2, '2010-02-25T00:00:00', 127)),
    (143, N'EM', 0, NULL, N'Krishna', NULL, N'Sunkammurali', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '442C8270-1E84-47C5-A621-AF10CF1B3546', CONVERT(datetime2, '2010-02-06T00:00:00', 127)),
    (144, N'EM', 0, NULL, N'Paul', N'R', N'Singh', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BA4C0109-A2A4-4595-A357-38A64B643E3C', CONVERT(datetime2, '2009-01-10T00:00:00', 127)),
    (145, N'EM', 0, NULL, N'Cynthia', N'S', N'Randall', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F0C15AD6-4CFF-4931-A216-49B635596043', CONVERT(datetime2, '2009-01-20T00:00:00', 127)),
    (146, N'EM', 0, NULL, N'Jian Shuo', NULL, N'Wang', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '009EBB72-9173-4AA9-920E-B578F3C927DE', CONVERT(datetime2, '2008-11-30T00:00:00', 127)),
    (147, N'EM', 0, NULL, N'Sandra', NULL, N'Reátegui Alayo', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B07FA8A6-A056-48AB-91F8-318C6C87B502', CONVERT(datetime2, '2008-12-19T00:00:00', 127)),
    (148, N'EM', 0, NULL, N'Jason', N'M', N'Watters', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '00D61032-0D0E-41E3-8F37-B5987C8D5087', CONVERT(datetime2, '2009-01-07T00:00:00', 127)),
    (149, N'EM', 0, NULL, N'Andy', N'M', N'Ruth', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9CFDF60A-083A-44DC-AFE2-D578C76FD55A', CONVERT(datetime2, '2009-01-24T00:00:00', 127)),
    (150, N'EM', 0, NULL, N'Michael', N'T', N'Vanderhyde', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '944AE1F8-E154-4752-9A24-8E8FE85156F3', CONVERT(datetime2, '2009-02-19T00:00:00', 127)),
    (151, N'EM', 0, NULL, N'Rostislav', N'E', N'Shabalin', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FAA146E7-5C07-4C47-8B66-DCDA9BE58D54', CONVERT(datetime2, '2009-02-12T00:00:00', 127)),
    (152, N'EM', 0, NULL, N'Yuhong', N'L', N'Li', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '866472D1-65C3-4AA7-B15B-F75EF10CB7D8', CONVERT(datetime2, '2009-01-25T00:00:00', 127)),
    (153, N'EM', 0, NULL, N'Hanying', N'P', N'Feng', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DDAAE515-0A69-4750-B6EE-7AC6629F55B8', CONVERT(datetime2, '2008-12-09T00:00:00', 127)),
    (154, N'EM', 0, NULL, N'Raymond', N'K', N'Sam', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '02717918-B884-41B9-87B3-A0F8C2939EC1', CONVERT(datetime2, '2008-12-16T00:00:00', 127)),
    (155, N'EM', 0, NULL, N'Fadi', N'K', N'Fakhouri', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3EF14439-76B4-4086-BE8D-0A03A6694425', CONVERT(datetime2, '2008-12-28T00:00:00', 127)),
    (156, N'EM', 0, NULL, N'Lane', N'M', N'Sacksteder', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8591375E-8ED1-49C8-89F3-52AED00C77F1', CONVERT(datetime2, '2009-01-04T00:00:00', 127)),
    (157, N'EM', 0, NULL, N'Linda', N'A', N'Randall', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'EF38AC01-9C58-4CD0-AA39-D12DDA3C3E9C', CONVERT(datetime2, '2009-01-27T00:00:00', 127)),
    (158, N'EM', 0, NULL, N'Shelley', N'N', N'Dyck', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '77B7F905-5028-491C-A1DC-80087AAC7817', CONVERT(datetime2, '2009-02-28T00:00:00', 127)),
    (159, N'EM', 0, NULL, N'Terrence', N'W', N'Earls', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '15365ED2-53AC-4EC7-93BF-89ED5D903167', CONVERT(datetime2, '2009-02-09T00:00:00', 127)),
    (160, N'EM', 0, NULL, N'Jeff', N'V', N'Hay', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8C7296AB-73DA-4992-8200-833B2C625D50', CONVERT(datetime2, '2009-01-14T00:00:00', 127)),
    (161, N'EM', 0, NULL, N'Kirk', N'J', N'Koenigsbauer', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '35A9A423-C54F-48C5-9527-CF1E93089D92', CONVERT(datetime2, '2008-12-08T00:00:00', 127)),
    (162, N'EM', 0, NULL, N'Laura', N'C', N'Steele', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5FF2682D-D7E7-48A3-9DEF-B60F0B8434FE', CONVERT(datetime2, '2008-12-27T00:00:00', 127)),
    (163, N'EM', 0, NULL, N'Alex', N'M', N'Nayberg', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '08CA0103-CA0E-4B52-A20F-590BA3FECDA8', CONVERT(datetime2, '2009-02-01T00:00:00', 127)),
    (164, N'EM', 0, NULL, N'Andrew', N'M', N'Cencini', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B1D33171-636C-440C-B4F6-F5F6F1EBBC70', CONVERT(datetime2, '2009-02-27T00:00:00', 127)),
    (165, N'EM', 0, NULL, N'Chris', N'T', N'Preston', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9F2D6941-1DE5-4076-9817-7ECCF6C47D55', CONVERT(datetime2, '2009-01-15T00:00:00', 127)),
    (166, N'EM', 0, NULL, N'Jack', N'S', N'Richins', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8F9BCF96-29E1-4BED-9BF4-D8260AD0AF68', CONVERT(datetime2, '2009-02-14T00:00:00', 127)),
    (167, N'EM', 0, NULL, N'David', N'N', N'Johnson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C2C7021B-3209-45F7-8560-B3132595F9DA', CONVERT(datetime2, '2008-11-25T00:00:00', 127)),
    (168, N'EM', 0, NULL, N'Garrett', N'R', N'Young', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DA309209-A597-4D54-B717-D984B38975D1', CONVERT(datetime2, '2008-11-30T00:00:00', 127)),
    (169, N'EM', 0, NULL, N'Susan', N'A', N'Metters', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D8414490-CA5B-4517-BBE9-7FBF4D696771', CONVERT(datetime2, '2008-12-07T00:00:00', 127)),
    (170, N'EM', 0, NULL, N'George', N'Z', N'Li', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E9E65DEB-797B-4B49-B41C-81F1DC593BB9', CONVERT(datetime2, '2008-12-14T00:00:00', 127)),
    (171, N'EM', 0, NULL, N'David', N'A', N'Yalovsky', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4C1F3E85-AEED-4A7D-A4FE-EEDFB750E45D', CONVERT(datetime2, '2008-12-26T00:00:00', 127)),
    (172, N'EM', 0, NULL, N'Marc', N'J', N'Ingle', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8A6062D5-1C3A-46BF-A080-F6CF6090E5F0', CONVERT(datetime2, '2009-01-09T00:00:00', 127)),
    (173, N'EM', 0, NULL, N'Eugene', N'R', N'Zabokritski', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1284B1A5-D53C-4BF9-9777-05108B207103', CONVERT(datetime2, '2009-01-14T00:00:00', 127)),
    (174, N'EM', 0, NULL, N'Benjamin', N'R', N'Martin', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4430A64D-3D5D-423E-81B9-6C4F9ED88AB7', CONVERT(datetime2, '2009-01-20T00:00:00', 127)),
    (175, N'EM', 0, NULL, N'Reed', N'T', N'Koch', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '892F02CF-A623-4499-B850-1B46469B597E', CONVERT(datetime2, '2009-01-26T00:00:00', 127)),
    (176, N'EM', 0, NULL, N'David', N'Oliver', N'Lawrence', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0D4723B9-43E8-401D-BCBA-C7DCD8178B8C', CONVERT(datetime2, '2009-02-07T00:00:00', 127)),
    (177, N'EM', 0, NULL, N'Russell', N'M', N'King', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '2F67DE81-C40E-4234-8942-C77806FABFA3', CONVERT(datetime2, '2009-02-14T00:00:00', 127)),
    (178, N'EM', 0, NULL, N'John', N'N', N'Frum', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CE13FF53-4465-4E3A-90DA-2FA0E8F19378', CONVERT(datetime2, '2009-02-24T00:00:00', 127)),
    (179, N'EM', 0, NULL, N'Jan', N'S', N'Miksovsky', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A0719EB6-7FC8-4DB8-A5DB-EA45A5BEF0E2', CONVERT(datetime2, '2009-02-26T00:00:00', 127)),
    (180, N'EM', 0, NULL, N'Katie', N'L', N'McAskill-White', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FE4EC240-D091-4E90-8F70-E9467FAC3A18', CONVERT(datetime2, '2009-02-13T00:00:00', 127)),
    (181, N'EM', 0, NULL, N'Michael', N'T', N'Hines', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B7004365-FDD8-4FCD-85FB-53A759447180', CONVERT(datetime2, '2008-12-02T00:00:00', 127)),
    (182, N'EM', 0, NULL, N'Nitin', N'S', N'Mirchandani', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4BEF5E3D-5C17-4B16-AAC9-8E3611A4FFE5', CONVERT(datetime2, '2008-12-21T00:00:00', 127)),
    (183, N'EM', 0, NULL, N'Barbara', N'S', N'Decker', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C175EA91-0A68-49DB-9827-B4528F77D630', CONVERT(datetime2, '2009-01-15T00:00:00', 127)),
    (184, N'EM', 0, NULL, N'John', N'Y', N'Chen', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5E6EC398-C951-44A2-BEC7-DC1CD17FAACC', CONVERT(datetime2, '2009-02-02T00:00:00', 127)),
    (185, N'EM', 0, NULL, N'Stefen', N'A', N'Hesse', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '266FCE92-5534-4AC1-B68C-ECD8CEFFCEBC', CONVERT(datetime2, '2009-02-21T00:00:00', 127)),
    (186, N'EM', 0, NULL, N'Shane', N'S', N'Kim', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1432F3E8-7CCA-4C56-BDCB-02D5B0302BD9', CONVERT(datetime2, '2009-02-01T00:00:00', 127)),
    (187, N'EM', 0, NULL, N'Yvonne', N'S', N'McKay', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F3A901C1-4D9F-4666-8726-C2C51643A5D6', CONVERT(datetime2, '2008-12-02T00:00:00', 127)),
    (188, N'EM', 0, NULL, N'Douglas', N'B', N'Hite', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4CE168C6-28FF-454D-887E-8F96BB2FBA93', CONVERT(datetime2, '2008-12-20T00:00:00', 127)),
    (189, N'EM', 0, NULL, N'Janeth', N'M', N'Esteves', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A5A5A995-AD0A-4F50-847D-1CA048F11EE3', CONVERT(datetime2, '2009-01-08T00:00:00', 127)),
    (190, N'EM', 0, NULL, N'Robert', N'J', N'Rounthwaite', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FBF84B1A-3B42-40FE-8BF0-3094DCF25888', CONVERT(datetime2, '2009-01-26T00:00:00', 127)),
    (191, N'EM', 0, NULL, N'Lionel', N'C', N'Penuchot', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '55B6EB95-61ED-4458-9BFB-F0560F001970', CONVERT(datetime2, '2009-02-19T00:00:00', 127)),
    (192, N'EM', 0, NULL, N'Brenda', N'M', N'Diaz', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '056BBD40-CDF0-428D-B12D-9FFB0FF5FEAE', CONVERT(datetime2, '2009-02-26T00:00:00', 127)),
    (193, N'EM', 0, NULL, N'Alejandro', N'E', N'McGuel', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F38E101D-B360-4876-8E8F-07726E85AE5D', CONVERT(datetime2, '2008-11-29T00:00:00', 127)),
    (194, N'EM', 0, NULL, N'Fred', N'T', N'Northup', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E28C41D6-D9BA-455D-B91D-E3C9F7B535E2', CONVERT(datetime2, '2008-12-05T00:00:00', 127)),
    (195, N'EM', 0, NULL, N'Kevin', N'H', N'Liu', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '14AFAE63-9C86-4E23-B82E-5AAD4DBD1963', CONVERT(datetime2, '2008-12-10T00:00:00', 127)),
    (196, N'EM', 0, NULL, N'Shammi', N'G', N'Mohamed', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '923FDAEC-6DB1-42A7-9840-D9D335996596', CONVERT(datetime2, '2008-12-17T00:00:00', 127)),
    (197, N'EM', 0, NULL, N'Rajesh', N'M', N'Patel', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CCC3C607-1E9B-4E62-8064-0B89594472F3', CONVERT(datetime2, '2008-12-24T00:00:00', 127)),
    (198, N'EM', 0, NULL, N'Lorraine', N'O', N'Nay', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '35495D48-03AF-492F-BEE5-FDA7B3E61970', CONVERT(datetime2, '2008-12-28T00:00:00', 127)),
    (199, N'EM', 0, NULL, N'Paula', N'R', N'Nartker', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D6FF616B-A950-48D2-8CC0-0E7F5A9ECDA0', CONVERT(datetime2, '2009-01-05T00:00:00', 127)),
    (200, N'EM', 0, NULL, N'Frank', N'T', N'Lee', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8025020E-4FF8-488B-99CA-74640E1F8DAE', CONVERT(datetime2, '2009-01-10T00:00:00', 127)),
    (201, N'EM', 0, NULL, N'Brian', N'T', N'Lloyd', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A3A2494B-F92D-473E-BFDA-FC3FE406E134', CONVERT(datetime2, '2009-01-22T00:00:00', 127)),
    (202, N'EM', 0, NULL, N'Tawana', N'G', N'Nusbaum', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '63B57674-1205-41A1-A532-8E6C9F38E48D', CONVERT(datetime2, '2009-01-29T00:00:00', 127)),
    (203, N'EM', 0, NULL, N'Ken', N'L', N'Myer', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '981FCDB7-9792-463D-BF1F-F1B4CED52473', CONVERT(datetime2, '2009-02-17T00:00:00', 127)),
    (204, N'EM', 0, NULL, N'Gabe', N'B', N'Mares', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0F0FA05E-E101-47E4-9BF7-1E0790B68E00', CONVERT(datetime2, '2009-03-01T00:00:00', 127)),
    (205, N'EM', 0, NULL, N'Lori', N'A', N'Kane', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6F059DF8-BB13-4CD0-ABE1-D3E6688CEDD1', CONVERT(datetime2, '2009-02-19T00:00:00', 127)),
    (206, N'EM', 0, NULL, N'Stuart', N'V', N'Munson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CE75C8B6-EAC8-4B18-96FA-8182AD7233CD', CONVERT(datetime2, '2008-11-25T00:00:00', 127)),
    (207, N'EM', 0, NULL, N'Greg', N'F', N'Alderson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F3A921DD-55EB-46EB-A94D-68F4B080A388', CONVERT(datetime2, '2008-11-25T00:00:00', 127)),
    (208, N'EM', 0, NULL, N'Scott', N'R', N'Gode', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1BECEC23-19F8-4B73-A404-12CCBCFAD66E', CONVERT(datetime2, '2009-01-01T00:00:00', 127)),
    (209, N'EM', 0, NULL, N'Kathie', N'E', N'Flood', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3BA601AE-2918-4936-9380-FD4C4F15DEA4', CONVERT(datetime2, '2009-01-20T00:00:00', 127)),
    (210, N'EM', 0, NULL, N'Belinda', N'M', N'Newman', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FA3467EB-B6EE-43A7-8BDD-15AFE14F2738', CONVERT(datetime2, '2009-02-13T00:00:00', 127)),
    (211, N'EM', 0, NULL, N'Hazem', N'E', N'Abolrous', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C2637051-25A5-4461-B06A-523119259430', CONVERT(datetime2, '2009-02-21T00:00:00', 127)),
    (212, N'EM', 0, NULL, N'Peng', N'J', N'Wu', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '24B9C3C3-A06D-44FA-AA24-34DDD3ABF62C', CONVERT(datetime2, '2008-12-02T00:00:00', 127)),
    (213, N'EM', 0, NULL, N'Sootha', N'T', N'Charncherngkha', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C334506C-197B-4010-935E-AEE740818BC9', CONVERT(datetime2, '2010-02-16T00:00:00', 127)),
    (214, N'EM', 0, NULL, N'Andreas', N'T', N'Berglund', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4F896936-FBF8-4120-BC87-62CCF30D09DB', CONVERT(datetime2, '2009-01-26T00:00:00', 127)),
    (215, N'EM', 0, NULL, N'Mark', N'L', N'Harrington', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '494202A4-EE2D-406B-974F-85B70FE0889B', CONVERT(datetime2, '2009-01-08T00:00:00', 127)),
    (216, N'EM', 0, NULL, N'Sean', N'P', N'Alexander', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'AA6F6C92-C6B0-4C05-B3A3-90887A88DE0F', CONVERT(datetime2, '2008-12-21T00:00:00', 127)),
    (217, N'EM', 0, NULL, N'Zainal', N'T', N'Arifin', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '989DB0AC-1E7B-4CC8-B90F-1799172ED3AA', CONVERT(datetime2, '2008-12-28T00:00:00', 127)),
    (218, N'EM', 0, NULL, N'Tengiz', N'N', N'Kharatishvili', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '06CFED75-E8F0-4E81-871A-4368D9B9F73E', CONVERT(datetime2, '2008-12-09T00:00:00', 127)),
    (219, N'EM', 0, NULL, N'Sean', N'N', N'Chai', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '93D0906B-B2D7-486F-BD14-9A126BE82AB6', CONVERT(datetime2, '2009-01-15T00:00:00', 127)),
    (220, N'EM', 0, NULL, N'Karen', N'R', N'Berge', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '20B3CF90-BA00-4DBF-8026-A028742DC328', CONVERT(datetime2, '2009-02-02T00:00:00', 127)),
    (221, N'EM', 0, NULL, N'Chris', N'K', N'Norred', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B35192A9-15AC-4B64-9039-BB3F8566AF55', CONVERT(datetime2, '2009-02-27T00:00:00', 127)),
    (222, N'EM', 0, NULL, N'A. Scott', NULL, N'Wright', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '90CC98B7-6054-4AB5-844E-3ECCB8A73070', CONVERT(datetime2, '2008-12-05T00:00:00', 127)),
    (223, N'EM', 0, NULL, N'Sairaj', N'L', N'Uddin', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4F2F3EEF-B0A5-44B9-ADD9-424C26A77BF9', CONVERT(datetime2, '2009-01-19T00:00:00', 127)),
    (224, N'EM', 0, NULL, N'William', N'S', N'Vong', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B42F3DDD-01D6-4701-91C0-2F6D9E8847C6', CONVERT(datetime2, '2008-12-31T00:00:00', 127)),
    (225, N'EM', 0, NULL, N'Alan', N'J', N'Brewer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '72331C0F-994F-4494-ABFC-AA129DA85635', CONVERT(datetime2, '2009-02-06T00:00:00', 127)),
    (226, N'EM', 0, NULL, N'Brian', N'P', N'LaMee', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3F4225C1-7833-43D7-A485-BDCEE84553BF', CONVERT(datetime2, '2009-02-24T00:00:00', 127)),
    (227, N'EM', 0, NULL, N'Gary', N'E.', N'Altman', N'III', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A131E984-DD98-4D19-91FF-9A9BB57CDA99', CONVERT(datetime2, '2009-11-25T00:00:00', 127)),
    (228, N'EM', 0, NULL, N'Christian', N'E', N'Kleinerman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '32852137-66C8-4BE4-B6CF-3C094534DA9B', CONVERT(datetime2, '2008-12-07T00:00:00', 127)),
    (229, N'EM', 0, NULL, N'Lori', N'K', N'Penor', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '327A5C9F-3DFC-4775-8375-368A5A29FE16', CONVERT(datetime2, '2010-02-09T00:00:00', 127)),
    (230, N'EM', 0, NULL, N'Stuart', N'J', N'Macrae', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '991FB7CC-704B-4EDE-9C8E-2615EB3F56BF', CONVERT(datetime2, '2010-02-26T00:00:00', 127)),
    (231, N'EM', 0, NULL, N'Jo', N'L', N'Berry', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '196CBA72-3810-435A-9A52-57A5E276293B', CONVERT(datetime2, '2010-02-28T00:00:00', 127)),
    (232, N'EM', 0, NULL, N'Pat', N'H', N'Coleman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D703625A-CFB0-4B32-BCF6-F58185E71A7C', CONVERT(datetime2, '2010-01-20T00:00:00', 127)),
    (233, N'EM', 0, NULL, N'Magnus', N'E', N'Hedlund', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '615A2903-C6D2-4265-A8E6-471DEC449224', CONVERT(datetime2, '2009-12-14T00:00:00', 127)),
    (234, N'EM', 0, NULL, N'Laura', N'F', N'Norman', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4603A371-EE90-4D87-9C25-A13CE3F190A0', CONVERT(datetime2, '2009-01-24T00:00:00', 127)),
    (235, N'EM', 0, NULL, N'Paula', N'M', N'Barreto de Mattos', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8E952C09-C7C4-4D51-A8CB-36C44C949A0B', CONVERT(datetime2, '2008-11-29T00:00:00', 127)),
    (236, N'EM', 0, NULL, N'Grant', N'N', N'Culbertson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '242E654F-CFF1-495C-A408-1AAF2A8A0BA7', CONVERT(datetime2, '2009-02-18T00:00:00', 127)),
    (237, N'EM', 0, NULL, N'Hao', N'O', N'Chen', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '11C53AD9-2D74-480F-8D83-6AA37B1F52E1', CONVERT(datetime2, '2009-01-30T00:00:00', 127)),
    (238, N'EM', 0, NULL, N'Vidur', N'X', N'Luthra', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E10218F6-99DE-462D-BDBD-28BDDEA8693A', CONVERT(datetime2, '2008-12-25T00:00:00', 127)),
    (239, N'EM', 0, NULL, N'Mindy', N'C', N'Martin', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B903A253-A5CB-4C38-AA41-BC77CBA65BB3', CONVERT(datetime2, '2008-12-18T00:00:00', 127)),
    (240, N'EM', 0, NULL, N'Willis', N'T', N'Johnson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3B808CDB-0372-409A-AE40-C2177A7DB2B6', CONVERT(datetime2, '2008-12-06T00:00:00', 127)),
    (241, N'EM', 0, NULL, N'David', N'J', N'Liu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F766DCB5-CAE1-4164-B313-D597EFB41C96', CONVERT(datetime2, '2009-01-23T00:00:00', 127)),
    (242, N'EM', 0, NULL, N'Deborah', N'E', N'Poe', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9236BDD4-DE3E-43BE-BB3C-22E377A2FFA6', CONVERT(datetime2, '2008-12-11T00:00:00', 127)),
    (243, N'EM', 0, NULL, N'Candy', N'L', N'Spoon', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '303047DF-CC05-4C50-813F-BE67719AB00E', CONVERT(datetime2, '2008-12-30T00:00:00', 127)),
    (244, N'EM', 0, NULL, N'Bryan', N'A', N'Walton', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '014BC84D-E05A-4A97-BD3E-CB21A3522DA5', CONVERT(datetime2, '2009-01-17T00:00:00', 127)),
    (245, N'EM', 0, NULL, N'Barbara', N'C', N'Moreland', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F2CC060C-A8CB-4A93-99E3-5B9D47F00F9C', CONVERT(datetime2, '2009-02-11T00:00:00', 127)),
    (246, N'EM', 0, NULL, N'Dragan', N'K', N'Tomic', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6CBF1558-FAF0-4A54-94E9-6AF835225872', CONVERT(datetime2, '2009-02-04T00:00:00', 127)),
    (247, N'EM', 0, NULL, N'Janet', N'L', N'Sheperdigian', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D24AFF04-1200-43DF-A211-F28F2A8E753F', CONVERT(datetime2, '2009-02-22T00:00:00', 127)),
    (248, N'EM', 0, NULL, N'Mike', N'K', N'Seamans', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'AECB0EBA-B17A-41ED-AC2E-076EDA87F5E4', CONVERT(datetime2, '2009-03-01T00:00:00', 127)),
    (249, N'EM', 0, NULL, N'Wendy', N'Beth', N'Kahn', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A4F5869C-E86D-46BD-BAAC-1B4FAF4CD920', CONVERT(datetime2, '2008-12-18T00:00:00', 127)),
    (250, N'EM', 0, NULL, N'Sheela', N'H', N'Word', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '73BEA116-6235-48CB-94A5-58FA56994640', CONVERT(datetime2, '2011-02-18T00:00:00', 127)),
    (251, N'EM', 0, NULL, N'Mikael', N'Q', N'Sandberg', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D4285D37-ED4C-4A8B-AEDE-D6DBD37CC76F', CONVERT(datetime2, '2009-02-03T00:00:00', 127)),
    (252, N'EM', 0, NULL, N'Arvind', N'B', N'Rao', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E70B4E15-AB90-483F-A417-D0A8E66D35FB', CONVERT(datetime2, '2009-02-21T00:00:00', 127)),
    (253, N'EM', 0, NULL, N'Linda', N'P', N'Meisner', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F78F14D7-8E98-4925-821C-BE69B7A2C4BA', CONVERT(datetime2, '2009-12-10T00:00:00', 127)),
    (254, N'EM', 0, NULL, N'Fukiko', N'J', N'Ogisu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9F5E4E33-E097-483A-8B74-FA4D7E54EC56', CONVERT(datetime2, '2009-12-28T00:00:00', 127)),
    (255, N'EM', 0, NULL, N'Gordon', N'L', N'Hee', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '67925187-A9C0-4F6A-A377-653FFD3C50DF', CONVERT(datetime2, '2010-01-04T00:00:00', 127)),
    (256, N'EM', 0, NULL, N'Frank', N'S', N'Pellow', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1FE09286-926C-4528-8D83-C0816F5F8521', CONVERT(datetime2, '2010-01-16T00:00:00', 127)),
    (257, N'EM', 0, NULL, N'Eric', N'S', N'Kurjan', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BE6ED8D4-3A9D-46BF-9348-DE0753E8CE81', CONVERT(datetime2, '2010-01-20T00:00:00', 127)),
    (258, N'EM', 0, NULL, N'Erin', N'M', N'Hagens', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '09E5B805-A275-473F-8B8B-00673AEF252D', CONVERT(datetime2, '2010-01-24T00:00:00', 127)),
    (259, N'EM', 0, NULL, N'Ben', N'T', N'Miller', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'AB1DEFC1-5B4A-4032-A787-8F4147B2E363', CONVERT(datetime2, '2010-03-02T00:00:00', 127)),
    (260, N'EM', 0, NULL, N'Annette', N'L', N'Hill', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D69499B6-7467-4D5D-A89D-F116E5752444', CONVERT(datetime2, '2010-11-29T00:00:00', 127)),
    (261, N'EM', 0, NULL, N'Reinout', N'N', N'Hillmann', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BA53CB03-00ED-4650-82F6-3680524AAB3F', CONVERT(datetime2, '2010-12-18T00:00:00', 127)),
    (262, N'EM', 0, NULL, N'David', N'M', N'Barber', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '43BE0580-122B-4FDF-8B15-FD286A18E97A', CONVERT(datetime2, '2009-01-05T00:00:00', 127)),
    (263, N'EM', 0, NULL, N'Jean', N'E', N'Trenary', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0AA3878D-0F9F-4F2A-942F-8B46C9B57C0A', CONVERT(datetime2, '2008-12-04T00:00:00', 127)),
    (264, N'EM', 0, NULL, N'Stephanie', N'A', N'Conroy', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '24757DC7-8E05-4DE0-920D-A1F4A60B30BA', CONVERT(datetime2, '2009-01-28T00:00:00', 127)),
    (265, N'EM', 0, NULL, N'Ashvini', N'R', N'Sharma', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '252A5641-6B19-4347-830F-E659BFC76AEF', CONVERT(datetime2, '2008-11-27T00:00:00', 127)),
    (266, N'EM', 0, NULL, N'Peter', N'I', N'Connelly', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9D2546F7-7571-477D-AB76-532F0438EDDF', CONVERT(datetime2, '2009-02-16T00:00:00', 127)),
    (267, N'EM', 0, NULL, N'Karen', N'A', N'Berg', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F2D24899-80B9-44F7-9B3B-63ACDEF3B505', CONVERT(datetime2, '2009-02-09T00:00:00', 127)),
    (268, N'EM', 0, NULL, N'Ramesh', N'V', N'Meyyappan', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C69E8136-CBB5-48EB-84A0-59CE62920478', CONVERT(datetime2, '2009-01-27T00:00:00', 127)),
    (269, N'EM', 0, NULL, N'Dan', N'K', N'Bacon', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D9DA7900-703F-4CA6-92BD-5ED3A25C0455', CONVERT(datetime2, '2009-01-04T00:00:00', 127)),
    (270, N'EM', 0, NULL, N'François', N'P', N'Ajenstat', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '83605AF8-6496-4362-BE19-20F2EC7114BA', CONVERT(datetime2, '2009-01-10T00:00:00', 127)),
    (271, N'EM', 0, NULL, N'Dan', N'B', N'Wilson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9F637D98-7182-4F6A-B68B-1BA9F2ECCB4D', CONVERT(datetime2, '2009-01-15T00:00:00', 127)),
    (272, N'EM', 0, NULL, N'Janaina', N'Barreiro Gambaro', N'Bueno', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0204851C-3ECC-4968-8504-47706C350E3E', CONVERT(datetime2, '2008-12-16T00:00:00', 127)),
    (273, N'EM', 0, N'Mr.', N'Brian', N'S', N'Welcker', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0950A366-1EEE-4516-86E0-6F2F94CE1046', CONVERT(datetime2, '2011-02-08T00:00:00', 127)),
    (274, N'SP', 0, NULL, N'Stephen', N'Y', N'Jiang', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1F4062EC-0577-43DF-947A-EEE1E3D24C60', CONVERT(datetime2, '2010-12-28T00:00:00', 127)),
    (275, N'SP', 0, NULL, N'Michael', N'G', N'Blythe', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '250C3F67-B75F-4B0D-9BE2-DEB5825C8EEB', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (276, N'SP', 0, NULL, N'Linda', N'C', N'Mitchell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '519A296E-CA91-450F-95C9-153459604147', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (277, N'SP', 0, NULL, N'Jillian', NULL, N'Carson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3C407F7D-2EA1-4749-8856-8C8EECF23AEA', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (278, N'SP', 0, NULL, N'Garrett', N'R', N'Vargas', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6167A617-60E9-4212-9C40-BB471716776C', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (279, N'SP', 0, NULL, N'Tsvi', N'Michael', N'Reiter', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FCA35010-0144-4C4C-8245-D38E9FD4CCD2', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (280, N'SP', 0, NULL, N'Pamela', N'O', N'Ansman-Wolfe', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8C8F5D47-E3F4-4577-BE7E-BC224431CF2B', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (281, N'SP', 0, NULL, N'Shu', N'K', N'Ito', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CA87A053-6C5F-4F9B-8790-1D0AF332153A', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (282, N'SP', 0, NULL, N'José', N'Edvaldo', N'Saraiva', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '120C84EB-8A22-4E1F-8C2B-FF5F144FDB7D', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (283, N'SP', 0, NULL, N'David', N'R', N'Campbell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6F44C8D9-E3D0-482C-87DC-CCEB09060F31', CONVERT(datetime2, '2011-05-24T00:00:00', 127)),
    (284, N'SP', 0, N'Mr.', N'Tete', N'A', N'Mensa-Annan', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F5BED54C-DF64-402C-93BD-7CFD1F79DEEE', CONVERT(datetime2, '2012-09-23T00:00:00', 127)),
    (285, N'SP', 0, N'Mr.', N'Syed', N'E', N'Abbas', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FF284881-01C2-4C77-95A7-4DB96F59BB70', CONVERT(datetime2, '2013-03-07T00:00:00', 127)),
    (286, N'SP', 0, NULL, N'Lynn', N'N', N'Tsoflias', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'EED57FDF-01BE-4770-A2AD-10F3349CF3DB', CONVERT(datetime2, '2013-05-23T00:00:00', 127)),
    (287, N'SP', 0, NULL, N'Amy', N'E', N'Alberts', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0CCB2270-1C40-4E7D-9A09-0B29A94DA8B6', CONVERT(datetime2, '2012-04-09T00:00:00', 127)),
    (288, N'SP', 0, NULL, N'Rachel', N'B', N'Valdez', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F28093ED-C889-43EB-BE88-37C9E147148C', CONVERT(datetime2, '2013-05-23T00:00:00', 127)),
    (289, N'SP', 0, NULL, N'Jae', N'B', N'Pak', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '317764BC-A515-4C2B-9A82-8136B8EC538F', CONVERT(datetime2, '2012-05-23T00:00:00', 127)),
    (290, N'SP', 0, NULL, N'Ranjit', N'R', N'Varkey Chudukatil', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5F6547B3-A7A9-4B14-8895-4402150BD903', CONVERT(datetime2, '2012-05-23T00:00:00', 127)),
    (291, N'SC', 0, N'Mr.', N'Gustavo', NULL, N'Achong', NULL, 2, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes"><act:telephoneNumber><act:number>425-555-1112</act:number><act:SpecialInstructions>Call only after 5:00 p.m.</act:SpecialInstructions></act:telephoneNumber>Note that the customer has a secondary home address.<act:homePostalAddress><act:Street>123 Oak</act:Street><act:City>Seattle</act:City><act:StateProvince>WA</act:StateProvince><act:PostalCode>98001</act:PostalCode><act:CountryRegion>USA</act:CountryRegion><act:SpecialInstructions>If correspondence to the primary address fails, try this one.</act:SpecialInstructions></act:homePostalAddress>Customer provided additional email address.<act:eMail><act:eMailAddress>customer1@sample.com</act:eMailAddress><act:SpecialInstructions>For urgent issues, do not send e-mail. Instead use this emergency contact phone<act:telephoneNumber><act:number>425-555-1111</act:number></act:telephoneNumber>.</act:SpecialInstructions></act:eMail><crm:ContactRecord date="2001-06-02Z">This customer is interested in purchasing high-end bicycles for his family. The customer contacted Michael in sales.</crm:ContactRecord></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13909.13111</TotalPurchaseYTD></IndividualSurvey>', 'D4C132D3-FCB5-4231-9DD5-888A54BEC693', CONVERT(datetime2, '2015-04-15 16:33:33.060000000', 127)),
    (293, N'SC', 0, N'Ms.', N'Catherine', N'R.', N'Abel', NULL, 1, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes">These are additional phone and pager numbers for the customer.<act:telephoneNumber><act:number>206-555-2222</act:number><act:SpecialInstructions>On weekends, contact the manager at this number.</act:SpecialInstructions></act:telephoneNumber><act:telephoneNumber><act:number>206-555-1234</act:number></act:telephoneNumber><act:pager><act:number>206-555-1244</act:number><act:SpecialInstructions>Do not page between 9:00 a.m. and 5:00 p.m.</act:SpecialInstructions></act:pager>Customer provided this additional home address.<act:homePostalAddress><act:Street>P.O Box 5</act:Street><act:City>Edmonds</act:City><act:StateProvince>WA</act:StateProvince><act:PostalCode>98431</act:PostalCode><act:CountryRegion>USA</act:CountryRegion><act:SpecialInstructions>This is an alternative address for billing only.</act:SpecialInstructions></act:homePostalAddress><act:eMail><act:eMailAddress>Joe@sample.com</act:eMailAddress><act:SpecialInstructions>Do not send e-mail for urgent issues. Use telephone instead.</act:SpecialInstructions></act:eMail><crm:ContactRecord date="2001-07-02Z">Sales contacted this customer to explain new pricing.</crm:ContactRecord></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3943.7544</TotalPurchaseYTD></IndividualSurvey>', 'D54E0552-C226-4C22-AF3B-762CA854CDD3', CONVERT(datetime2, '2015-04-15 16:33:33.077000000', 127)),
    (295, N'SC', 0, N'Ms.', N'Kim', NULL, N'Abercrombie', NULL, 0, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes">Customer provided this telephone number for weekend access.<act:telephoneNumber><act:number>605-555-9877</act:number></act:telephoneNumber>secondary phone number.<act:pager><act:number>310-555-5432</act:number><act:SpecialInstructions>Page only if other contact attempts fail.</act:SpecialInstructions></act:pager>Additional home address.<act:homePostalAddress><act:Street>990 5th Avenue</act:Street><act:City>Redmond</act:City><act:StateProvince>WA</act:StateProvince><act:PostalCode>98052</act:PostalCode><act:CountryRegion>USA</act:CountryRegion><act:SpecialInstructions>Use this address for billing only.</act:SpecialInstructions></act:homePostalAddress><act:eMail><act:eMailAddress>Customer3@sample.com</act:eMailAddress><act:SpecialInstructions>Do not send e-mails for urgent issues. Use telephone instead.</act:SpecialInstructions></act:eMail><crm:ContactRecord date="2002-01-01Z">Customer first contacted the sales department. This customer is interested in opening a new bicycle store.</crm:ContactRecord><crm:ContactRecord date="2002-01-05Z">Linda in sales returned the call. Customer provided another telephone number for off-hour coverage.<act:telephoneNumber><act:number>302-555-7733</act:number></act:telephoneNumber>We are still negotiating possible long-term contract.</crm:ContactRecord></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7948.6029</TotalPurchaseYTD></IndividualSurvey>', 'F7CBDB48-0B44-470E-9F37-7060446FBFB9', CONVERT(datetime2, '2015-04-15 16:33:33.077000000', 127)),
    (297, N'SC', 0, N'Sr.', N'Humberto', NULL, N'Acevedo', NULL, 2, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes">Additional phone numbers.<act:mobile><act:number>750-555-8888</act:number><act:SpecialInstructions>If customer is not at work, call<act:telephoneNumber><act:number>222-555-4444</act:number></act:telephoneNumber></act:SpecialInstructions></act:mobile>Customer uses these two additional email addresses.<act:eMail><act:eMailAddress>Customer4@sample.com</act:eMailAddress></act:eMail><act:eMail><act:eMailAddress>Customer4@contoso.com</act:eMailAddress><act:SpecialInstructions>This is secondary email address.</act:SpecialInstructions></act:eMail><crm:ContactRecord date="2002-01-01Z">Customer is expanding his sports and recreation business. Sales is negotiating large sales order with this customer. We called Joe at<act:mobile><act:number>750-555-8888</act:number></act:mobile>We are still working on competitive pricing. Looks promising, but we need additional marketing support to wrap this up.</crm:ContactRecord></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-20021.0528</TotalPurchaseYTD></IndividualSurvey>', '5A41D336-84CF-44D7-B12B-83B64B511F7E', CONVERT(datetime2, '2015-04-15 16:33:33.090000000', 127)),
    (299, N'SC', 0, N'Sra.', N'Pilar', NULL, N'Ackerman', NULL, 0, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes"><crm:ContactRecord date="2002-01-01Z">Sales contacted this customer for the first time at<act:telephoneNumber><act:number>432-4444</act:number></act:telephoneNumber>We talked about the Road bike price drop and the new spring models. Customer provided us new mobile number<act:mobile><act:number>432-555-7809</act:number></act:mobile></crm:ContactRecord></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5158.224</TotalPurchaseYTD></IndividualSurvey>', 'DF1FB8AB-2323-4330-9AB8-54E13CE6D8F9', CONVERT(datetime2, '2015-04-15 16:33:33.090000000', 127)),
    (301, N'SC', 0, N'Ms.', N'Frances', N'B.', N'Adams', NULL, 1, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes">Additional email address.<act:eMail><act:eMailAddress>Customer6@sample.com</act:eMailAddress></act:eMail><crm:ContactRecord date="2002-07-01Z">Customer called us. Interested in the new Touring models.</crm:ContactRecord></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-14477.2375</TotalPurchaseYTD></IndividualSurvey>', '66980082-C186-40AE-972A-74560E78C1AF', CONVERT(datetime2, '2015-04-15 16:33:33.090000000', 127)),
    (303, N'SC', 0, N'Ms.', N'Margaret', N'J.', N'Smith', NULL, 0, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes">These are additional phone numbers the customer provided.<act:telephoneNumber><act:number>206-555-2222</act:number><act:SpecialInstructions>Best time to call is Monday-Wednesday mornings.</act:SpecialInstructions></act:telephoneNumber><act:mobile><act:number>209-555-2122</act:number></act:mobile></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2421.0199</TotalPurchaseYTD></IndividualSurvey>', '691818E1-FC19-445F-B161-E401867368E3', CONVERT(datetime2, '2015-04-15 16:33:33.107000000', 127)),
    (305, N'SC', 0, N'Ms.', N'Carla', N'J.', N'Adams', NULL, 0, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes"><crm:ContactRecord date="2002-09-02Z">The customer is interested in the Road-450 and Mountain-500 series bicycles. Customer called us. We need to follow up. Pager <act:pager><act:number>206-555-1234</act:number></act:pager> is the best way to reach.</crm:ContactRecord></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-22476.234</TotalPurchaseYTD></IndividualSurvey>', '5F183B92-1648-4948-8937-47C9C10F0583', CONVERT(datetime2, '2015-04-15 16:33:33.107000000', 127)),
    (307, N'SC', 0, N'Mr.', N'Jay', NULL, N'Adams', NULL, 1, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes">Additional home and mobile phone numbers:<act:telephoneNumber><act:number>253-555-4689</act:number></act:telephoneNumber><act:mobile><act:number>253-555-4878</act:number></act:mobile></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-23834.7669</TotalPurchaseYTD></IndividualSurvey>', '38256645-4B88-4169-B68A-5B0118F18445', CONVERT(datetime2, '2015-04-15 16:33:33.123000000', 127)),
    (309, N'SC', 0, N'Mr.', N'Ronald', N'L.', N'Adina', NULL, 0, '<AdditionalContactInfo xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactInfo" xmlns:crm="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactRecord" xmlns:act="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ContactTypes"><crm:ContactRecord date="2002-09-02Z">Marketing called this customer. Customer provided this phone number for follow-up questions.<act:telephoneNumber><act:number>305-555-8888</act:number><act:SpecialInstructions>If no answer, try this pager number.<act:pager><act:number>308-555-3678</act:number></act:pager></act:SpecialInstructions></act:telephoneNumber> and this mobile number<act:mobile><act:number>308-555-4888</act:number></act:mobile></crm:ContactRecord></AdditionalContactInfo>', '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7618.4208</TotalPurchaseYTD></IndividualSurvey>', 'DBB05A55-2256-42B1-901E-824D28158911', CONVERT(datetime2, '2015-04-15 16:33:33.123000000', 127)),
    (311, N'SC', 0, N'Mr.', N'Samuel', N'N.', N'Agcaoili', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12897.6219</TotalPurchaseYTD></IndividualSurvey>', '43C55826-2843-49AB-BBBD-41065965A20E', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (313, N'SC', 0, N'Mr.', N'James', N'T.', N'Aguilar', N'Jr.', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2569.9404</TotalPurchaseYTD></IndividualSurvey>', '35AD71E7-8B4E-4C65-A92F-D6829BB292A5', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (315, N'SC', 0, N'Mr.', N'Robert', N'E.', N'Ahlering', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5255.01</TotalPurchaseYTD></IndividualSurvey>', '61629167-3A73-4923-9A2C-AAF1ABE3672E', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (317, N'SC', 0, N'Mr.', N'François', NULL, N'Ferrier', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-48861.593876</TotalPurchaseYTD></IndividualSurvey>', 'B50C8DCD-EA86-43A5-AF69-84B7A6A7FAF9', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (319, N'SC', 0, N'Ms.', N'Kim', NULL, N'Akers', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5277.0496</TotalPurchaseYTD></IndividualSurvey>', '1EE0110F-77AE-4B1B-A05B-B03E0509BE95', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (321, N'SC', 0, N'Ms.', N'Lili', N'J.', N'Alameda', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B6D14D10-265F-4D72-ACE8-C1A42F62C82E', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (323, N'SC', 0, N'Ms.', N'Amy', N'E.', N'Alberts', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3698.450608</TotalPurchaseYTD></IndividualSurvey>', '367113B0-AB80-40D7-A0B7-B1D9D83CE06E', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (325, N'SC', 0, N'Ms.', N'Anna', N'A.', N'Albright', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2658.129</TotalPurchaseYTD></IndividualSurvey>', 'B6E43A72-8F5F-4525-B4C0-EE84D764E86F', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (327, N'SC', 0, N'Mr.', N'Milton', N'J.', N'Albury', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1712.49</TotalPurchaseYTD></IndividualSurvey>', '7385E725-3AD2-4D66-BC9E-1594362EF939', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (329, N'SC', 0, N'Mr.', N'Paul', N'L.', N'Alcorn', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1267.422</TotalPurchaseYTD></IndividualSurvey>', '45A435C5-5FE1-4194-AF2A-29B4C62E2A2A', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (331, N'SC', 0, N'Mr.', N'Gregory', N'F.', N'Alderson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4520.0576</TotalPurchaseYTD></IndividualSurvey>', '72479586-13A3-4F91-AB31-2AD2855FED2D', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (333, N'SC', 0, N'Mr.', N'J. Phillip', N'L.', N'Alexander', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-20478.6895</TotalPurchaseYTD></IndividualSurvey>', 'B5078EF5-206C-4FC2-A934-3D0EAB5E0C60', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (335, N'SC', 0, N'Ms.', N'Michelle', NULL, N'Alexander', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-298.062</TotalPurchaseYTD></IndividualSurvey>', 'CA1AB4EA-CBD9-4F27-9CC4-C6828FCD8216', CONVERT(datetime2, '2013-03-30T00:00:00', 127)),
    (337, N'SC', 0, N'Mr.', N'Sean', N'P.', N'Jacobson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4994.538</TotalPurchaseYTD></IndividualSurvey>', 'CEF1C975-E3D9-465D-91CF-FA0F3C7EBFA2', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (339, N'SC', 0, N'Ms.', N'Phyllis', N'A.', N'Allen', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8910.7562</TotalPurchaseYTD></IndividualSurvey>', '4865D7B7-5961-43CA-AD25-9F77D9D5E4A8', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (341, N'SC', 0, N'Mr.', N'Marvin', N'N.', N'Allen', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8171.7865</TotalPurchaseYTD></IndividualSurvey>', '99A1A11E-7E5D-4E61-B2C2-FF5F7F78CF59', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (343, N'SC', 0, N'Mr.', N'Michael', NULL, N'Allen', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-15764.8128</TotalPurchaseYTD></IndividualSurvey>', 'FA93C578-2A25-4820-AA5D-E2437CC24C59', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (345, N'SC', 0, N'Mr.', N'Cecil', N'J.', N'Allison', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4392.206864</TotalPurchaseYTD></IndividualSurvey>', '3FB94B42-B159-4D6E-BBF1-115659144884', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (347, N'SC', 0, N'Mr.', N'Oscar', N'L.', N'Alpuerto', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-23789.8396</TotalPurchaseYTD></IndividualSurvey>', 'C599201D-A7AE-4F0E-8CCC-C76B4F946BE8', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (349, N'SC', 0, N'Ms.', N'Sandra', N'J.', N'Altamirano', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4354.05</TotalPurchaseYTD></IndividualSurvey>', 'F2DC755A-C176-4274-955C-E10031732242', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (351, N'SC', 0, N'Ms.', N'Selena', N'R.', N'Alvarado', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8785.4535</TotalPurchaseYTD></IndividualSurvey>', 'AF662FC4-6DD0-46F8-A547-98C572D46DDF', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (353, N'SC', 0, N'Mr.', N'Emilio', N'R.', N'Alvaro', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3587.3244</TotalPurchaseYTD></IndividualSurvey>', '6CCBE1A7-4E4B-4242-9EB7-8830CB9EA77A', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (355, N'SC', 0, N'Mr.', N'Maxwell', N'J.', N'Amland', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-14020.3232</TotalPurchaseYTD></IndividualSurvey>', 'FD32C96B-D3B8-4B50-9C58-FA0AB92A217A', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (357, N'SC', 0, N'Ms.', N'Mae', N'N.', N'Anderson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3779.67975</TotalPurchaseYTD></IndividualSurvey>', 'FF2A1330-9211-4ECE-B355-4682EA36662D', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (359, N'SC', 0, N'Ms.', N'Ramona', N'J.', N'Antrim', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1657.548</TotalPurchaseYTD></IndividualSurvey>', '4782AD14-3F09-454C-B5BC-1F1AB922F86C', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (361, N'SC', 0, N'Ms.', N'Sabria', N'B.', N'Appelbaum', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6481.6109</TotalPurchaseYTD></IndividualSurvey>', '915EC73E-1AF5-41B0-857A-BBD3F6C6730B', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (363, N'SC', 0, N'Ms.', N'Hannah', N'R.', N'Arakawa', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1298.616</TotalPurchaseYTD></IndividualSurvey>', '92D8EDC9-4948-4348-8683-01CB62F79126', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (365, N'SC', 0, N'Ms.', N'Kyley', N'J.', N'Arbelaez', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8913.89967</TotalPurchaseYTD></IndividualSurvey>', '253A9028-5D6B-4FC4-A5A3-74560FCB7DA5', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (367, N'SC', 0, N'Mr.', N'Tom', N'H', N'Johnston', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-20697.5634</TotalPurchaseYTD></IndividualSurvey>', 'BC8FC7EC-3A2F-4436-8BEA-1A08AE3A6E39', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (369, N'SC', 0, N'Mr.', N'Thomas', N'B.', N'Armstrong', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4108.89595</TotalPurchaseYTD></IndividualSurvey>', '57D6E4EE-FBEE-424D-976C-4D15CA6991FD', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (371, N'SC', 0, N'Mr.', N'John', NULL, N'Arthur', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7693.8214</TotalPurchaseYTD></IndividualSurvey>', '902112B6-7FE7-4E85-8D77-FF49C5E6761A', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (373, N'SC', 0, N'Mr.', N'Chris', NULL, N'Ashton', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4110.372</TotalPurchaseYTD></IndividualSurvey>', '962918C8-1598-40EA-AE5E-458B4A6B6853', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (375, N'SC', 0, N'Ms.', N'Teresa', NULL, N'Atkinson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4240.1304</TotalPurchaseYTD></IndividualSurvey>', '6D3F3644-31EB-42B2-BCB1-4A970D2EFAF3', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (377, N'SC', 0, N'Mr.', N'John', N'P.', N'Ault', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8648.19</TotalPurchaseYTD></IndividualSurvey>', 'F0B67A75-4235-45D5-A5E0-2E938A975DE8', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (379, N'SC', 0, N'Mr.', N'Robert', N'A.', N'Avalos', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8582.652</TotalPurchaseYTD></IndividualSurvey>', '7B6F52CA-0379-417E-A66D-853183B39D76', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (381, N'SC', 0, N'Mr.', N'Stephen', N'M.', N'Ayers', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9256.6687</TotalPurchaseYTD></IndividualSurvey>', '5041E52C-1469-4579-AA66-3074605CFFA8', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (383, N'SC', 0, N'Mr.', N'Phillip', N'M.', N'Bacalzo', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1556.16</TotalPurchaseYTD></IndividualSurvey>', '115001F9-FF6F-4322-9EA0-BD2E275D6357', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (385, N'SC', 0, N'Mr.', N'Daniel', NULL, N'Blanco', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3615.5276</TotalPurchaseYTD></IndividualSurvey>', '10783772-2CA3-4744-ADBB-445045DA6208', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (387, N'SC', 0, N'Mr.', N'Cory', N'K.', N'Booth', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10112.904583</TotalPurchaseYTD></IndividualSurvey>', 'A80867C5-ED86-487B-B1B7-7C48126EB9F3', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (389, N'SC', 0, N'Mr.', N'James', N'B.', N'Bailey', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4971.4072</TotalPurchaseYTD></IndividualSurvey>', 'CFA4F9E4-A865-4A76-A0AC-0460D73E7EF7', CONVERT(datetime2, '2012-08-30T00:00:00', 127)),
    (391, N'SC', 0, N'Mr.', N'Douglas', N'A.', N'Baldwin', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12025.3101</TotalPurchaseYTD></IndividualSurvey>', 'BD35D341-DF60-4B41-AF27-28A179D3F3B9', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (393, N'SC', 0, N'Ms.', N'Jennifer', N'B.', N'Bales', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3179.724</TotalPurchaseYTD></IndividualSurvey>', '762CAC13-C911-425D-AEF7-BD6C6722DE3E', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (395, N'SC', 0, N'Mr.', N'Alberto', N'F.', N'Baltazar', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B3622F28-96D2-45C4-8310-F9688C6C631F', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (397, N'SC', 0, N'Mr.', N'Wayne', N'N.', N'Banack', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8975.0238</TotalPurchaseYTD></IndividualSurvey>', 'AA3425B9-0F7C-4F54-A037-D117DDC8BE1D', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (399, N'SC', 0, N'Mr.', N'Darrell', N'M.', N'Banks', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B4A92667-F303-4586-AD53-548E2254D90C', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (401, N'SC', 0, N'Ms.', N'Angela', NULL, N'Barbariol', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E25E7C6E-F378-42E1-8F0C-CC4E79E35F1C', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (403, N'SC', 0, N'Mr.', N'David', NULL, N'Barber', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '659DA8E8-C501-4F0E-A286-398FAF22D323', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (405, N'SC', 0, N'Mr.', N'Robert', N'L.', N'Barker', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-915.0252</TotalPurchaseYTD></IndividualSurvey>', 'B1B8F17D-64AE-4BD5-A626-9C43161FDD84', CONVERT(datetime2, '2012-10-30T00:00:00', 127)),
    (407, N'SC', 0, N'Ms.', N'Rebecca', N'R.', N'Barley', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2007.09</TotalPurchaseYTD></IndividualSurvey>', 'FEE35313-0747-4361-8259-C6D91B67EE57', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (409, N'SC', 0, N'Ms.', N'Brenda', N'L.', N'Barlow', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8172.606</TotalPurchaseYTD></IndividualSurvey>', '96970EB3-8B25-4E68-8BD4-5EAB2A96F60F', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (411, N'SC', 0, N'Mr.', N'Josh', NULL, N'Barnhill', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6635.3333</TotalPurchaseYTD></IndividualSurvey>', '37B54C12-D508-446B-8085-32B85790940E', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (413, N'SC', 0, N'Mr.', N'Adam', NULL, N'Barr', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8929.5598</TotalPurchaseYTD></IndividualSurvey>', 'E87B9431-227B-4AC7-860A-B764CFF0F492', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (415, N'SC', 0, N'Ms.', N'Norma', N'N.', N'Barrera', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0A4851DB-0028-4A91-9D57-DA5450272CAA', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (417, N'SC', 0, NULL, N'Gytis', N'M', N'Barzdukas', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3960.6052</TotalPurchaseYTD></IndividualSurvey>', '8D2FEEC9-F95E-4398-A1FA-1DFF561B8EB9', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (419, N'SC', 0, N'Mr.', N'David', N'M.', N'Bartness', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2172.078</TotalPurchaseYTD></IndividualSurvey>', 'DD87C1A3-1254-4D2D-9596-251B576B28CA', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (421, N'SC', 0, N'Ms.', N'Karel', N'E.', N'Bates', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10261.0519</TotalPurchaseYTD></IndividualSurvey>', 'CC35B476-85B0-40A0-B9F9-51B7C1C849F5', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (423, N'SC', 0, N'Sr.', N'Ciro', N'J.', N'Bauer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D26A8A72-F5EB-4BF6-B4C6-DB2D89606719', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (425, N'SC', 0, N'Ms.', N'Glenna', N'L.', N'Beanston', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '732E7E32-7EAF-48EB-A345-90D81EF8307B', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (427, N'SC', 0, N'Mr.', N'Shaun', NULL, N'Beasley', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5771.5534</TotalPurchaseYTD></IndividualSurvey>', '1649D376-3012-40A0-A821-C745FA34CA3F', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (429, N'SC', 0, N'Mr.', N'John', N'A.', N'Beaver', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-131.2036</TotalPurchaseYTD></IndividualSurvey>', '011DD8D5-1AC4-4FE8-B139-EE9D0EA172E6', CONVERT(datetime2, '2013-02-28T00:00:00', 127)),
    (431, N'SC', 0, N'Mr.', N'Christopher', N'R.', N'Beck', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-18088.887</TotalPurchaseYTD></IndividualSurvey>', '2DCA9FBE-92DF-4B43-B475-8BD20AA2DD1F', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (433, N'SC', 0, N'Mr.', N'Bradley', NULL, N'Beck', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D6A90F54-57E8-498C-8D84-19F07B6202FA', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (435, N'SC', 0, N'Mr.', N'Benjamin', N'J.', N'Becker', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3645.816</TotalPurchaseYTD></IndividualSurvey>', '806D1C84-0290-49E5-9CB7-DA12AA7766A0', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (437, N'SC', 0, N'Ms.', N'Ann', NULL, N'Beebe', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1000.1139</TotalPurchaseYTD></IndividualSurvey>', '3D4B934D-6559-431B-B471-1FD508407B19', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (439, N'SC', 0, N'Mr.', N'Shane', N'J.', N'Belli', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-20839.3703</TotalPurchaseYTD></IndividualSurvey>', 'A899E65B-74FD-42E5-A0FB-B2F72B3C0218', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (441, N'SC', 0, N'Mr.', N'Stanley', N'A.', N'Alan', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11853.9216</TotalPurchaseYTD></IndividualSurvey>', 'DD152AED-AA0A-4BF0-8435-71C075F646D7', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (443, N'SC', 0, N'Mr.', N'Mason', NULL, N'Bendixen', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2164.998</TotalPurchaseYTD></IndividualSurvey>', '9D353282-B8FD-4F15-AD44-3FF3D5D3ACDA', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (445, N'SC', 0, N'Mr.', N'John', N'M.', N'Bennetts', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-729.438</TotalPurchaseYTD></IndividualSurvey>', '694258A3-633C-42F9-98B8-08E50EDEDC68', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (447, N'SC', 0, N'Mr.', N'Ido', NULL, N'Ben-Sachar', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6F05729C-5386-459C-A638-ED8E41B19584', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (449, N'SC', 0, N'Ms.', N'Edna', N'J.', N'Benson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6487.11</TotalPurchaseYTD></IndividualSurvey>', '39E5E96C-C4AA-4A12-A8A8-B0C711115B4D', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (451, N'SC', 0, N'Mr.', N'Payton', N'P.', N'Benson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7018.7156</TotalPurchaseYTD></IndividualSurvey>', '912BFF37-4777-4899-A0B1-0FC112B2B79C', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (453, N'SC', 0, N'Mr.', N'Max', NULL, N'Benson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4314.7686</TotalPurchaseYTD></IndividualSurvey>', '7B630976-FD10-4DBF-97C3-DB840853145E', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (455, N'SC', 0, N'Mr.', N'Scot', N'A.', N'Bent', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3154.584</TotalPurchaseYTD></IndividualSurvey>', '0D80E5F2-2723-433A-826D-BCF7D860688D', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (457, N'SC', 0, N'Mr.', N'Richard', N'M.', N'Bentley', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '07B1C116-AE07-4B1A-AC53-69D6943C1D3C', CONVERT(datetime2, '2013-03-30T00:00:00', 127)),
    (459, N'SC', 0, N'Ms.', N'Marian', N'M.', N'Berch', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B9B136E9-A5A8-4207-90AC-AA896690975C', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (461, N'SC', 0, N'Ms.', N'Karen', NULL, N'Berge', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '2CAFBF86-4D0B-4864-9963-964413812403', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (463, N'SC', 0, N'Mr.', N'Alexander', N'J.', N'Berger', N'II', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1274.764216</TotalPurchaseYTD></IndividualSurvey>', 'B66BB606-03A8-44E8-831B-2B8278534CE8', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (465, N'SC', 0, N'Mr.', N'John', N'M.', N'Berger', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11626.0099</TotalPurchaseYTD></IndividualSurvey>', 'B901DD8D-087B-49BE-BF1F-3B11F6D95820', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (467, N'SC', 0, N'Ms.', N'Kris', N'R.', N'Bergin', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '32DCE686-0370-4E9F-8E4B-317D2278346A', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (469, N'SC', 0, NULL, N'Andreas', NULL, N'Berglund', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5DB095E5-82F2-4742-B010-C14BA56E7537', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (471, N'SC', 0, N'Mr.', N'Robert', N'M.', N'Bernacchi', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3545.613</TotalPurchaseYTD></IndividualSurvey>', '15155B90-85C9-4B09-8ECA-2C03696F3A3F', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (473, N'SC', 0, N'Mr.', N'Matthias', NULL, N'Berndt', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-20873.9571</TotalPurchaseYTD></IndividualSurvey>', '8CECFF02-4D31-4331-AEA4-33468D7032F6', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (475, N'SC', 0, NULL, N'John', NULL, N'Berry', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-16463.544</TotalPurchaseYTD></IndividualSurvey>', '8F36A32A-BC2C-4313-910C-E7A1E2D13045', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (477, N'SC', 0, N'Mr.', N'Steven', N'B.', N'Brown', N'IV', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11792.0114</TotalPurchaseYTD></IndividualSurvey>', '3DC2AAA7-145C-47FD-978E-0D33083C8360', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (479, N'SC', 0, N'Mr.', N'Chris', N'N.', N'Bidelman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4340.298</TotalPurchaseYTD></IndividualSurvey>', '9924416A-13DA-48FE-90DE-7EAAC04D6238', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (481, N'SC', 0, N'Ms.', N'Mary', N'B.', N'Billstrom', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7677.462</TotalPurchaseYTD></IndividualSurvey>', '98A83410-B6FE-4CED-BEB6-FE552CCAC92E', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (483, N'SC', 0, N'Mr.', N'Jimmy', NULL, N'Bischoff', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3996.7435</TotalPurchaseYTD></IndividualSurvey>', '68023A5B-740B-45A6-BC8F-F8EEAE4C6B7B', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (485, N'SC', 0, N'Mr.', N'Mary', NULL, N'Bishop', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6701B3D4-654F-452C-96F3-7AB7C7130A0E', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (487, N'SC', 0, N'Ms.', N'Mae', N'M.', N'Black', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4101.288</TotalPurchaseYTD></IndividualSurvey>', '9E1691E3-B0BF-40D2-9491-8073B2FBC95A', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (489, N'SC', 0, N'Ms.', N'Jackie', N'E.', N'Blackwell', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-218.85</TotalPurchaseYTD></IndividualSurvey>', '38CC4C3E-38F0-4169-AB2F-494B2B0F6ED1', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (491, N'SC', 0, N'Mr.', N'Donald', N'L.', N'Blanton', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2816.314</TotalPurchaseYTD></IndividualSurvey>', 'FF0E980C-B977-48A2-B390-DCCD57FC93F0', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (493, N'SC', 0, N'Ms.', N'Linda', N'E.', N'Burnett', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9128.7368</TotalPurchaseYTD></IndividualSurvey>', 'CA8E700A-A8FA-4CF3-B71E-F3F57B9DF5CB', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (495, N'SC', 0, N'Mr.', N'Michael', N'Greg', N'Blythe', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5174.4768</TotalPurchaseYTD></IndividualSurvey>', '4FF1B5B1-435E-4262-B48E-D6B9A87B0257', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (497, N'SC', 0, N'Ms.', N'Gabriel', N'L.', N'Bockenkamp', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-16774.4932</TotalPurchaseYTD></IndividualSurvey>', '325ED05B-148A-4A9F-A714-AF1D7412B1B4', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (499, N'SC', 0, N'Mr.', N'Michael', N'L.', N'Bohling', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '25F763CA-6461-48DA-8DFF-0FB14B6F47D1', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (501, N'SC', 0, N'Ms.', N'Corinna', NULL, N'Bolender', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-207.749816</TotalPurchaseYTD></IndividualSurvey>', '72B2C875-D28C-48D6-AC64-D8C4A5E0EE37', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (503, N'SC', 0, N'Sr.', N'Luis', NULL, N'Bonifaz', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6234.4455</TotalPurchaseYTD></IndividualSurvey>', 'EA724F36-2135-42A5-8D9C-33658584E7F4', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (505, N'SC', 0, N'Mr.', N'Randall', NULL, N'Boseman', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3134.513</TotalPurchaseYTD></IndividualSurvey>', 'AE08C9D1-7700-4BAD-BDD1-80ABEC1E1DC5', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (507, N'SC', 0, N'Ms.', N'Stephanie', NULL, N'Bourne', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-738.09</TotalPurchaseYTD></IndividualSurvey>', 'CDD08CF9-EE2E-44F6-A486-8E40B32F688D', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (509, N'SC', 0, N'Mr.', N'Eli', NULL, N'Bowen', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1764.0159</TotalPurchaseYTD></IndividualSurvey>', '46CD304E-AF55-4AF3-9602-F78182864BEA', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (511, N'SC', 0, N'Mr.', N'Lester', N'J.', N'Bowman', N'Sr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7842.8321</TotalPurchaseYTD></IndividualSurvey>', '6B2E1E9B-E298-4F97-91C3-F6026A5C3AA8', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (513, N'SC', 0, N'Mr.', N'David', N'M.', N'Bradley', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-647.988</TotalPurchaseYTD></IndividualSurvey>', '66F455D6-78A2-4105-B768-53264395BE94', CONVERT(datetime2, '2013-08-30T00:00:00', 127)),
    (515, N'SC', 0, N'Mr.', N'Cornelius', N'L.', N'Brandon', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1611.741</TotalPurchaseYTD></IndividualSurvey>', '79AEC05A-E153-4600-81E3-090097B60C8E', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (517, N'SC', 0, N'Mr.', N'Richard', NULL, N'Bready', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-32607.13905</TotalPurchaseYTD></IndividualSurvey>', '2B5DA487-220D-45DA-8E61-996BC090AD89', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (519, N'SC', 0, N'Ms.', N'Sara', N'N.', N'Breer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5744.6578</TotalPurchaseYTD></IndividualSurvey>', '498B64BB-D82B-46C0-AC38-5B93B760B847', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (521, N'SC', 0, N'Mr.', N'Ted', NULL, N'Bremer', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5008.2744</TotalPurchaseYTD></IndividualSurvey>', '4CD2E02D-3249-4DF7-A444-598A30B7BFB8', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (523, N'SC', 0, N'Mr.', N'Alan', NULL, N'Brewer', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3385.3678</TotalPurchaseYTD></IndividualSurvey>', 'E6164829-8FEC-4583-BFDD-8371C57926F2', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (525, N'SC', 0, N'Mr.', N'Walter', N'J.', N'Brian', N'IV', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2380.26</TotalPurchaseYTD></IndividualSurvey>', '881E3408-CE51-4665-B800-9A5ACF9377FD', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (527, N'SC', 0, N'Mr.', N'Christopher', N'M.', N'Bright', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-838.9178</TotalPurchaseYTD></IndividualSurvey>', '6B3718BA-54AB-46B9-A60C-76CE3A54880D', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (529, N'SC', 0, N'Ms.', N'Carol', N'J.', N'Brink', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '423D97B7-6FE7-46AE-BB48-C2F400F12173', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (531, N'SC', 0, N'Mr.', N'David', N'J.', N'Brink', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2296.5304</TotalPurchaseYTD></IndividualSurvey>', '7892935F-1314-4505-952A-C536E6EDFF66', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (533, N'SC', 0, N'Mr.', N'John', N'R.', N'Brooks', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3947.6869</TotalPurchaseYTD></IndividualSurvey>', '4B21D63C-181F-404A-A1AB-03EDF4E1D478', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (535, N'SC', 0, N'Mr.', N'Willie', N'P.', N'Brooks', N'Jr.', 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4605.3478</TotalPurchaseYTD></IndividualSurvey>', '0446E735-E6A5-4C76-9154-08C65D4C046E', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (537, N'SC', 0, N'Ms.', N'Carolee', N'J.', N'Brown', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4852.752</TotalPurchaseYTD></IndividualSurvey>', '2D327432-4BF3-4C2B-A731-FDC896EEFDAD', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (539, N'SC', 0, N'Mr.', N'Jo', NULL, N'Brown', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8177.3631</TotalPurchaseYTD></IndividualSurvey>', 'B531E652-FB76-496C-A4A9-DB3C0CD3B88D', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (541, N'SC', 0, N'Mr.', N'Robert', NULL, N'Brown', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6308.544</TotalPurchaseYTD></IndividualSurvey>', 'D7B162E3-A30F-4E31-9F2C-DEBD9F8766C8', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (543, N'SC', 0, N'Mr.', N'Kevin', N'F.', N'Browne', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10862.087624</TotalPurchaseYTD></IndividualSurvey>', 'A67CCA3B-E2CA-4001-ADFA-64A2B2F7452E', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (545, N'SC', 0, N'Ms.', N'Mary', N'K.', N'Browning', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2768.9177</TotalPurchaseYTD></IndividualSurvey>', '586B6ECC-96A6-4584-9D6C-A06AC6266791', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (547, N'SC', 0, N'Mr.', N'Dave', N'P.', N'Browning', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2855.215</TotalPurchaseYTD></IndividualSurvey>', '628DBA22-0EC1-4AE0-BCD2-8172ADCFA31F', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (549, N'SC', 0, N'Ms.', N'Bridget', N'E.', N'Browqett', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3438.6228</TotalPurchaseYTD></IndividualSurvey>', '1C3B3E89-6C89-4489-B305-0137CB8DC1D2', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (551, N'SC', 0, N'Mr.', N'Eric', N'J.', N'Brumfield', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4134.2279</TotalPurchaseYTD></IndividualSurvey>', '309505BC-2173-479A-9E49-AE93E07CBCC7', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (553, N'SC', 0, N'Mr.', N'Dick', N'R.', N'Brummer', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12969.3408</TotalPurchaseYTD></IndividualSurvey>', '87D8475F-8E44-4DE5-9217-BE305C56ECD2', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (555, N'SC', 0, N'Mr.', N'Michael', NULL, N'Brundage', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2769.0792</TotalPurchaseYTD></IndividualSurvey>', 'F754087B-0F9C-4317-AA7C-6CECA33EBB3E', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (557, N'SC', 0, N'Ms.', N'Shirley', N'R.', N'Bruner', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-652.937</TotalPurchaseYTD></IndividualSurvey>', 'BA56D00B-AB71-46B7-859E-455484A60C79', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (559, N'SC', 0, N'Ms.', N'June', N'B.', N'Brunner', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-17373.1394</TotalPurchaseYTD></IndividualSurvey>', '3F1AF50B-36D5-4080-91AD-91D3CAC823F9', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (561, N'SC', 0, N'Mr.', N'Dirk', N'J.', N'Bruno', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-17658.3144</TotalPurchaseYTD></IndividualSurvey>', 'B2F789C1-05D8-40B2-9105-B2A8FFCF9CC5', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (563, N'SC', 0, N'Ms.', N'Nancy', NULL, N'Buchanan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4138.8612</TotalPurchaseYTD></IndividualSurvey>', 'ED848E8C-5E2A-4FBE-88D7-8FC4F45EDF25', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (565, N'SC', 0, N'Sra.', N'Janaina Barreiro Gambaro', NULL, N'Bueno', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2324.55</TotalPurchaseYTD></IndividualSurvey>', '4CA3AB6C-CBBA-4AB3-8B8D-1B0E497274DF', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (567, N'SC', 0, N'Mr.', N'Edward', N'T.', N'Buensalido', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2761.32</TotalPurchaseYTD></IndividualSurvey>', '230D320F-6EFD-4B15-B110-3405FE2CE8D0', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (569, N'SC', 0, N'Ms.', N'Megan', N'E.', N'Burke', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-19859.289508</TotalPurchaseYTD></IndividualSurvey>', '3E2FB5D8-D299-4F33-B24A-5F70F42694A3', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (571, N'SC', 0, N'Ms.', N'Ingrid', N'K.', N'Burkhardt', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '7DDDFCD7-E203-4D0C-BB97-5B10A5CF9CC0', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (573, N'SC', 0, N'Ms.', N'Karren', N'K.', N'Burkhardt', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5056.3756</TotalPurchaseYTD></IndividualSurvey>', '8B6115C8-404E-4178-B784-87F833B5A095', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (575, N'SC', 0, N'Mr.', N'Ovidiu', NULL, N'Burlacu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B1BD6887-A5B7-4E1E-AEB8-A1F76A2F76F2', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (577, N'SC', 0, N'Ms.', N'Dana', N'H.', N'Burnell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-127.8</TotalPurchaseYTD></IndividualSurvey>', '6B8EEDA2-CD1C-4AA8-A77B-1CC089533E49', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (579, N'SC', 0, N'Mr.', N'Timothy', N'B.', N'Burnett', N'Jr.', 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5345.13258</TotalPurchaseYTD></IndividualSurvey>', 'B4A086B9-B9BA-40C1-B08F-AF78BED920EB', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (581, N'SC', 0, N'Mr.', N'Stephen', N'R.', N'Burton', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8868.93</TotalPurchaseYTD></IndividualSurvey>', 'FB56A714-9382-4417-9E89-AC61415D6C9A', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (583, N'SC', 0, N'Ms.', N'Deanna', N'R.', N'Buskirk', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3749.7846</TotalPurchaseYTD></IndividualSurvey>', 'C544B152-6E8C-49A2-BA7F-AD9F64A1AEDC', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (585, N'SC', 0, N'Mr.', N'Jared', N'L.', N'Bustamante', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4302.2176</TotalPurchaseYTD></IndividualSurvey>', '50DCEBC6-14DF-4ABE-8484-40408407D116', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (587, N'SC', 0, N'Mr.', N'Richard', N'A.', N'Byham', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-669</TotalPurchaseYTD></IndividualSurvey>', 'DEC2779C-1BC9-493B-B050-7F5E67BADE01', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (589, N'SC', 0, N'Mr.', N'David', N'J.', N'Byrnes', N'Sr.', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4883.9847</TotalPurchaseYTD></IndividualSurvey>', 'A8579A7F-2D79-41A2-979E-B2A1EE81DFA0', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (591, N'SC', 0, N'Mr.', N'Ryan', NULL, N'Calafato', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-20243.2341</TotalPurchaseYTD></IndividualSurvey>', 'C52CD03A-8653-472C-AE2E-5490A2590D75', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (593, N'SC', 0, N'Ms.', N'Sylvia', N'A.', N'Caldwell', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C65DCD1D-FBCA-4CBF-8FFB-2B2F50991691', CONVERT(datetime2, '2012-09-30T00:00:00', 127)),
    (595, N'SC', 0, N'Ms.', N'Mari', N'B.', N'Caldwell', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7830.498</TotalPurchaseYTD></IndividualSurvey>', 'CAD9AED3-9D15-4494-9AAC-AC66B8AD65A0', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (597, N'SC', 0, N'Ms.', N'Barbara', N'J.', N'Calone', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-30764.7584</TotalPurchaseYTD></IndividualSurvey>', '99D918E9-0DE7-4E70-8DD4-AC98C4297792', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (599, N'SC', 0, N'Ms.', N'Lindsey', N'R.', N'Camacho', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9999.2076</TotalPurchaseYTD></IndividualSurvey>', '32E0B800-B5E8-4AAB-9394-BE1523A19B62', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (601, N'SC', 0, N'Sr.', N'Gustavo', NULL, N'Camargo', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7333.5315</TotalPurchaseYTD></IndividualSurvey>', 'CD476813-1A70-4DB4-8A1D-B26C02A1340C', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (603, N'SC', 0, N'Ms.', N'DeeDee', N'J.', N'Cameron', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-352.9867</TotalPurchaseYTD></IndividualSurvey>', '788034C8-39DD-4DE2-9ACA-A522E731DBB1', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (605, N'SC', 0, N'Ms.', N'Deborah', N'M.', N'Lee', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3981DA23-757F-4715-8220-2E538DBAE568', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (607, N'SC', 0, N'Ms.', N'Joan', N'M.', N'Campbell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10060.0858</TotalPurchaseYTD></IndividualSurvey>', 'B0932973-C642-4B35-883B-03A5FE5AD44E', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (609, N'SC', 0, N'Mr.', N'David', NULL, N'Campbell', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12428.518</TotalPurchaseYTD></IndividualSurvey>', 'B493CF71-F031-4B25-BDFE-20B3681DD4D3', CONVERT(datetime2, '2012-09-30T00:00:00', 127)),
    (611, N'SC', 0, N'Mr.', N'Henry', N'L.', N'Campen', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-18217.76435</TotalPurchaseYTD></IndividualSurvey>', '6EFD9EDF-B06C-4530-9C5F-14054A8EEA90', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (613, N'SC', 0, N'Mr.', N'Chris', NULL, N'Cannon', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7514.436</TotalPurchaseYTD></IndividualSurvey>', '92ED161A-249A-4C74-B8F0-CC818FFBE12A', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (615, N'SC', 0, N'Mr.', N'Joseph', N'J.', N'Cantoni', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5646.8774</TotalPurchaseYTD></IndividualSurvey>', 'E0EF5905-7AD2-4510-8ED9-9A207176689F', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (617, N'SC', 0, N'Ms.', N'Suzana De Abreu', N'A.', N'Canuto', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6071.536125</TotalPurchaseYTD></IndividualSurvey>', 'CA9D7C75-44C1-4170-BFF8-E731974D2E3A', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (619, N'SC', 0, N'Mr.', N'Jun', NULL, N'Cao', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5525.13</TotalPurchaseYTD></IndividualSurvey>', '83877DF5-9A89-4A03-B1AA-0C78F9043E1B', CONVERT(datetime2, '2012-09-30T00:00:00', 127)),
    (621, N'SC', 0, N'Mr.', N'Johnny', N'A.', N'Caprio', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12330.318752</TotalPurchaseYTD></IndividualSurvey>', 'F0278BEE-A86C-43A7-920C-58F5FD5F5175', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (623, N'SC', 0, N'Mr.', N'Richard', NULL, N'Carey', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13653.726</TotalPurchaseYTD></IndividualSurvey>', 'F80F9900-7FCA-4A4A-A060-F180E0561679', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (625, N'SC', 0, N'Mr.', N'Carlton', N'M.', N'Carlisle', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11928.228</TotalPurchaseYTD></IndividualSurvey>', '77656232-BDDE-4CBE-9E6F-B4261E32505E', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (627, N'SC', 0, N'Mr.', N'Ty Loren', NULL, N'Carlson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4672.5218</TotalPurchaseYTD></IndividualSurvey>', '2957170D-8AE7-4555-8EF7-0D9BFC87AF91', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (629, N'SC', 0, N'Ms.', N'Jane', N'N.', N'Carmichael', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6357.9036</TotalPurchaseYTD></IndividualSurvey>', 'B5331048-01B4-448B-8F5D-6C0C6E9E3DB0', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (631, N'SC', 0, N'Ms.', N'Jovita', N'A.', N'Carmody', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8697.474375</TotalPurchaseYTD></IndividualSurvey>', 'D39915B3-9EB7-4048-AEA2-E421107ADE58', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (633, N'SC', 0, N'Mr.', N'Steve', N'J.', N'Carnes', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3532.986</TotalPurchaseYTD></IndividualSurvey>', 'C4D75D81-A251-400D-ACD1-058B35339FA7', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (635, N'SC', 0, N'Mr.', N'Fernando', NULL, N'Caro', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6357.384</TotalPurchaseYTD></IndividualSurvey>', 'F6BB29A1-54BA-4E7A-BB9C-470C780B35D8', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (637, N'SC', 0, N'Mr.', N'Rob', NULL, N'Caron', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5623.4144</TotalPurchaseYTD></IndividualSurvey>', '40D427EE-D647-4E23-96B2-727869A910B2', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (639, N'SC', 0, N'Mr.', N'Andy', NULL, N'Carothers', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5167.734</TotalPurchaseYTD></IndividualSurvey>', '12AAD41F-CB14-47B4-8D5D-8AB4B403FB3E', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (641, N'SC', 0, N'Ms.', N'Donna', N'F.', N'Carreras', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9616.6719</TotalPurchaseYTD></IndividualSurvey>', '2820F4B0-135D-4387-A507-1F70A3505FB1', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (643, N'SC', 0, N'Ms.', N'Rosmarie', N'J.', N'Carroll', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-955.104</TotalPurchaseYTD></IndividualSurvey>', 'E1D657EF-68CB-4378-A676-2FCF67EBF8AB', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (645, N'SC', 0, N'Mr.', N'Matthew', NULL, N'Carroll', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '60797E4A-461A-425F-8E11-02CCA751977C', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (647, N'SC', 0, N'Mr.', N'Joseph', N'N.', N'Castellucio', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-17110.6054</TotalPurchaseYTD></IndividualSurvey>', '9A23E182-5BC7-44E4-800A-A1744AB449A4', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (649, N'SC', 0, N'Ms.', N'Pamela', N'A.', N'Carlson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3435.6315</TotalPurchaseYTD></IndividualSurvey>', 'D4ABC852-2033-4C47-B3C0-32CDEB6F33D0', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (651, N'SC', 0, N'Mr.', N'Raul', N'E.', N'Casts', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-971.514</TotalPurchaseYTD></IndividualSurvey>', '431227C0-A31B-4D91-A43F-84F490715059', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (653, N'SC', 0, N'Ms.', N'Elizabeth', N'E.', N'Catalano', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-16461.01595</TotalPurchaseYTD></IndividualSurvey>', '5963C3DB-BE38-45BA-BDFD-DA1D61300206', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (655, N'SC', 0, N'Mr.', N'Matthew', N'J.', N'Cavallari', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2641.7628</TotalPurchaseYTD></IndividualSurvey>', 'D388A69D-A6AF-42AC-9597-7CFAF0501475', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (657, N'SC', 0, N'Ms.', N'Brigid', N'F.', N'Cavendish', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6513.8842</TotalPurchaseYTD></IndividualSurvey>', '56F7C46E-39F8-48BD-8708-0CB77475BCAB', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (659, N'SC', 0, N'Mr.', N'Andrew', NULL, N'Cencini', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5393.0088</TotalPurchaseYTD></IndividualSurvey>', '9B80DBEE-6A75-45F5-84DC-10C3EF6FF808', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (661, N'SC', 0, N'Ms.', N'Stacey', N'M.', N'Cereghino', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-27472.2599</TotalPurchaseYTD></IndividualSurvey>', '55F9D6D3-43D7-44C1-9B1A-1A398CF3BE4A', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (663, N'SC', 0, N'Mr.', N'Baris', NULL, N'Cetinok', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2930.831308</TotalPurchaseYTD></IndividualSurvey>', '46F7CCCA-DC1B-4AD4-8DAB-1212DB19F7F2', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (665, N'SC', 0, N'Mr.', N'Sean', NULL, N'Chai', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-479.898</TotalPurchaseYTD></IndividualSurvey>', 'EF57D74A-6847-447D-864B-740B8E241627', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (667, N'SC', 0, N'Mr.', N'Pat', N'A.', N'Chambers', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-15720.93635</TotalPurchaseYTD></IndividualSurvey>', 'F219DAEF-4372-4E5B-A9EF-60B56240EBD7', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (669, N'SC', 0, N'Mr.', N'Forrest', N'J.', N'Chandler', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1803.228</TotalPurchaseYTD></IndividualSurvey>', '486D5868-CEA6-470D-B41D-CBB2CC8046E8', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (671, N'SC', 0, N'Mr.', N'Lee', N'J.', N'Chapla', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8958.1152</TotalPurchaseYTD></IndividualSurvey>', 'FE1AB8DB-A700-4D29-AEC7-B555D38EA840', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (673, N'SC', 0, N'Mr.', N'Greg', NULL, N'Chapman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DF20285D-EC36-438A-B3B2-46D088FDFBC7', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (675, N'SC', 0, N'Mr.', N'Neil', NULL, N'Charney', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-490.446</TotalPurchaseYTD></IndividualSurvey>', '9A5AFDCA-413A-45E3-B8CD-5C3DB6BE90C6', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (677, N'SC', 0, N'Mr.', N'Hao', NULL, N'Chen', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5CA4A02B-3327-4BEE-B698-30DD5CDFB5BA', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (679, N'SC', 0, N'Mr.', N'John', N'Y.', N'Chen', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D6E4BD36-001D-4286-BEC4-634E1886CBDE', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (681, N'SC', 0, N'Mr.', N'Pei', NULL, N'Chow', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2235.186</TotalPurchaseYTD></IndividualSurvey>', '3989A851-C05D-4F53-98E0-B73650ACB2B0', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (683, N'SC', 0, N'Mr.', N'Yao-Qiang', NULL, N'Cheng', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10075.9737</TotalPurchaseYTD></IndividualSurvey>', '58FE206C-79F0-4F45-8394-6BF7BF5B2D4C', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (685, N'SC', 0, N'Ms.', N'Nicky', N'E.', N'Chesnut', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13362.06075</TotalPurchaseYTD></IndividualSurvey>', '8AC42F96-E03E-44CD-98F2-9CE548963D75', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (687, N'SC', 0, N'Ms.', N'Susan', N'B.', N'Chestnut', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6801.876</TotalPurchaseYTD></IndividualSurvey>', '3FE0425A-9682-4089-9986-62E5C16B403A', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (689, N'SC', 0, N'Mr.', N'Martin', NULL, N'Chisholm', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2447.622</TotalPurchaseYTD></IndividualSurvey>', '815C70E3-011E-4D90-9F32-0CC64609A472', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (691, N'SC', 0, N'Mr.', N'Mike', N'M.', N'Choi', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7881.469576</TotalPurchaseYTD></IndividualSurvey>', 'DBB26B94-07C5-435F-9FC2-6EC971F9B986', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (693, N'SC', 0, N'Ms.', N'Ruth', N'A.', N'Choin', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2566.6891</TotalPurchaseYTD></IndividualSurvey>', '8F1BE194-D9BE-45D3-BD00-53291893B45B', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (695, N'SC', 0, N'Mr.', N'Anthony', NULL, N'Chor', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5502.820875</TotalPurchaseYTD></IndividualSurvey>', 'C84289C0-F96E-445E-9912-293027DD5279', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (697, N'SC', 0, N'Mr.', N'Charles', N'M.', N'Christensen', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8018.9485</TotalPurchaseYTD></IndividualSurvey>', '3A2D4953-5D05-4949-9499-A68A8C63B478', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (699, N'SC', 0, N'Ms.', N'Jill', N'J.', N'Christie', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4951.6101</TotalPurchaseYTD></IndividualSurvey>', '99E89D47-4587-4834-9C2E-73CBA6B3BB5B', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (701, N'SC', 0, N'Ms.', N'Alice', NULL, N'Clark', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6973.188</TotalPurchaseYTD></IndividualSurvey>', '8C3BAD59-0A4B-4981-96E3-3AA82CA63DF7', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (703, N'SC', 0, N'Ms.', N'Gina', N'N.', N'Clark', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-18314.946</TotalPurchaseYTD></IndividualSurvey>', '2AFFCF46-56CF-4941-9FFA-C43C42F86AC0', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (705, N'SC', 0, N'Mr.', N'James', N'J.', N'Clark', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3389.37</TotalPurchaseYTD></IndividualSurvey>', '74FE7967-D8BF-4A1C-A685-A79A5242CB02', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (707, N'SC', 0, N'Ms.', N'Jane', NULL, N'Clayton', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3200.418</TotalPurchaseYTD></IndividualSurvey>', '22A626D6-0E68-4D1F-9388-436AA774DA0C', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (709, N'SC', 0, N'Ms.', N'Kristine', N'J.', N'Cleary', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7741.674</TotalPurchaseYTD></IndividualSurvey>', '4EE9E5D6-F899-4277-83DB-0C638875F277', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (711, N'SC', 0, N'Ms.', N'Teanna', N'M.', N'Cobb', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5E07B7BB-6EC5-480E-9392-D828D6651AE8', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (713, N'SC', 0, N'Ms.', N'Connie', N'L.', N'Coffman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-838.9178</TotalPurchaseYTD></IndividualSurvey>', 'B71DF301-3AE6-4544-B113-10A360A4C021', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (715, N'SC', 0, N'Ms.', N'Jeanette', N'R.', N'Cole', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9681.3596</TotalPurchaseYTD></IndividualSurvey>', '4A48A3F0-FE92-4490-988C-B1F95E3A8F2B', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (717, N'SC', 0, N'Mr.', N'Eric', N'E.', N'Coleman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3806.94975</TotalPurchaseYTD></IndividualSurvey>', '5254BEF7-B51A-4ABC-BEE2-6EE46FEFEE03', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (719, N'SC', 0, N'Mr.', N'Pat', NULL, N'Coleman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4712.440875</TotalPurchaseYTD></IndividualSurvey>', 'E9F5B558-CB09-4212-9A9D-6B87E1D82AEF', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (721, N'SC', 0, N'Mr.', N'Takiko', N'J.', N'Collins', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2572.122</TotalPurchaseYTD></IndividualSurvey>', '0D7BFDFC-F7CD-444B-95F1-9304059795A8', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (723, N'SC', 0, N'Mr.', N'John', N'L.', N'Colon', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3355.6712</TotalPurchaseYTD></IndividualSurvey>', '23030320-BA4C-47BA-A202-1D241D26EB83', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (725, N'SC', 0, N'Mr.', N'Scott', N'A.', N'Colvin', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1145.8902</TotalPurchaseYTD></IndividualSurvey>', '34E39986-605E-494B-8BFB-0126446141BF', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (727, N'SC', 0, N'Mr.', N'Aaron', NULL, N'Con', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7983.5739</TotalPurchaseYTD></IndividualSurvey>', '8FAED624-34E1-4E5E-88E6-D59320E67AC4', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (729, N'SC', 0, N'Mr.', N'Peter', NULL, N'Connelly', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1924.374</TotalPurchaseYTD></IndividualSurvey>', '9258DC85-D106-4833-B157-01F7002876EF', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (731, N'SC', 0, N'Mr.', N'William', N'J.', N'Conner', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6705.4794</TotalPurchaseYTD></IndividualSurvey>', 'A26C02DE-1268-44A3-8E34-3AFC37D7331A', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (733, N'SC', 0, N'Ms.', N'Stephanie', NULL, N'Conroy', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6044.7467</TotalPurchaseYTD></IndividualSurvey>', '99849527-310E-4F39-AF11-B95525F959CC', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (735, N'SC', 0, N'Ms.', N'Amy', N'R.', N'Consentino', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3294.282</TotalPurchaseYTD></IndividualSurvey>', '69772008-1512-4370-8F77-4C5D5C316DBA', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (737, N'SC', 0, N'Ms.', N'Dorothy', N'J.', N'Contreras', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12448.5143</TotalPurchaseYTD></IndividualSurvey>', '71F2130C-84FF-4DF5-B5A2-73C6A95D1848', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (739, N'SC', 0, N'Mr.', N'Patrick', N'M.', N'Cook', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4170.8765</TotalPurchaseYTD></IndividualSurvey>', '57D91698-A060-4F4D-99E0-22B015350C88', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (741, N'SC', 0, N'Mr.', N'Scott', NULL, N'Cooper', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3635.2066</TotalPurchaseYTD></IndividualSurvey>', '51B4D79C-C098-4422-B570-7BE3CA0C8FCF', CONVERT(datetime2, '2011-08-31T00:00:00', 127)),
    (743, N'SC', 0, N'Ms.', N'Eva', NULL, N'Corets', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3246.97657</TotalPurchaseYTD></IndividualSurvey>', 'FC24E5A2-5AA5-430E-B661-F4EE9D52029B', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (745, N'SC', 0, N'Mr.', N'Marlin', N'M.', N'Coriell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13675.891175</TotalPurchaseYTD></IndividualSurvey>', '0F2B7C68-4D2D-43E9-8A4A-D1B31108C6C1', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (747, N'SC', 0, N'Mr.', N'Ryan', NULL, N'Cornelsen', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3A076391-3EBB-4B38-BDFE-05DF6543081A', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (749, N'SC', 0, N'Mr.', N'Bruno', NULL, N'Costa Da Silva', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '06ADA335-52F1-4128-BA4B-F9920740E80A', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (751, N'SC', 0, N'Ms.', N'Pamela', N'R.', N'Cox', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-24303.19495</TotalPurchaseYTD></IndividualSurvey>', '9874D04F-EEED-4BBC-B78C-BBBC2482C677', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (753, N'SC', 0, N'Mr.', N'Jack', NULL, N'Creasey', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11372.322</TotalPurchaseYTD></IndividualSurvey>', '7403B4C0-AFF2-428E-8593-7E566E2472AA', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (755, N'SC', 0, N'Ms.', N'Sharon', N'A.', N'Crow', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DE44A5AC-CCFC-4A2C-B48C-9915CC450511', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (757, N'SC', 0, N'Ms.', N'Shelley', N'R.', N'Crow', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1221.036</TotalPurchaseYTD></IndividualSurvey>', '31E2DF54-57BB-4902-82B9-4C0C5D6FDB47', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (759, N'SC', 0, N'Mr.', N'Grant', NULL, N'Culbertson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5003.7408</TotalPurchaseYTD></IndividualSurvey>', 'D9025B48-F8A9-4D83-9837-C362D8D822D3', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (761, N'SC', 0, N'Mr.', N'Scott', NULL, N'Culp', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4751.7771</TotalPurchaseYTD></IndividualSurvey>', 'FDB8A700-3D5E-4D44-8DF4-EA4D5497BABB', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (763, N'SC', 0, N'Mr.', N'Conor', NULL, N'Cunningham', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3785.6497</TotalPurchaseYTD></IndividualSurvey>', '5B22508A-224A-424C-9C79-30A87895C6A1', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (765, N'SC', 0, N'Mr.', N'Jose', N'M.', N'Curry', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6587.5794</TotalPurchaseYTD></IndividualSurvey>', '820EC1B0-EFB7-4363-8E9E-AB4DF5387E0B', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (767, N'SC', 0, N'Mr.', N'Thierry', NULL, N'D''Hers', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3116.492825</TotalPurchaseYTD></IndividualSurvey>', '83C2BA9B-7197-4CE5-89D8-BFCAB7534672', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (769, N'SC', 0, N'Mr.', N'Ryan', NULL, N'Danner', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10586.106</TotalPurchaseYTD></IndividualSurvey>', '748C5ACA-D021-4B7D-BED0-905DB7497D89', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (771, N'SC', 0, N'Mr.', N'Mike', NULL, N'Danseglio', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5592.5316</TotalPurchaseYTD></IndividualSurvey>', '4DC82108-E043-418B-8A54-7F3D4E600C9B', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (773, N'SC', 0, N'Ms.', N'Megan', N'N.', N'Davis', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2170.458</TotalPurchaseYTD></IndividualSurvey>', '1FED9B9B-5398-4DEC-8454-90B2844BA2AA', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (775, N'SC', 0, N'Sr.', N'Alvaro', NULL, N'De Matos Miranda Filho', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10554.6266</TotalPurchaseYTD></IndividualSurvey>', 'F5C85E68-C674-4DDE-A7BF-583F08B1936A', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (777, N'SC', 0, N'Mr.', N'Jose', NULL, N'De Oliveira', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1445.1898</TotalPurchaseYTD></IndividualSurvey>', '3E7D3DD6-5027-4BA7-9A77-4DC413F24E86', CONVERT(datetime2, '2012-04-30T00:00:00', 127)),
    (779, N'SC', 0, N'Mr.', N'Jacob', N'N.', N'Dean', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6186.4826</TotalPurchaseYTD></IndividualSurvey>', '366627CC-57CD-4F6B-A248-468D11F59173', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (781, N'SC', 0, N'Mr.', N'Alexander', N'J.', N'Deborde', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-728.161</TotalPurchaseYTD></IndividualSurvey>', '49E1AFE9-FEFC-40A7-808C-214CBDD79EFB', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (783, N'SC', 0, N'Ms.', N'Barbara', N'S.', N'Decker', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1940.406</TotalPurchaseYTD></IndividualSurvey>', '027E39F9-5466-438E-8A90-02FA366D7C6C', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (785, N'SC', 0, N'Mr.', N'Kirk', NULL, N'DeGrasse', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8847.4712</TotalPurchaseYTD></IndividualSurvey>', '6BD6792C-8834-4725-B45D-A277C57958E2', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (787, N'SC', 0, N'Ms.', N'Aidan', NULL, N'Delaney', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6274.951775</TotalPurchaseYTD></IndividualSurvey>', '0DAF75FD-4C86-4E43-B6BA-FB9830C4135F', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (789, N'SC', 0, N'Mr.', N'Stefan', NULL, N'Delmarco', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-22297.0914</TotalPurchaseYTD></IndividualSurvey>', '14A72F97-3D24-42E4-AAD4-CD8B0B5EC9E1', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (791, N'SC', 0, N'Mr.', N'Shawn', N'R.', N'Demicell', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5350.0724</TotalPurchaseYTD></IndividualSurvey>', 'BE9FA09C-D5DF-49BB-A10C-AC989672E483', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (793, N'SC', 0, N'Ms.', N'Della', N'F.', N'Demott Jr', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6321.006292</TotalPurchaseYTD></IndividualSurvey>', '0335F445-4AFA-4DBF-88E7-98429A34540C', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (795, N'SC', 0, N'Mr.', N'Bruno', NULL, N'Deniut', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5938.434</TotalPurchaseYTD></IndividualSurvey>', '945A67E6-CAC2-47D8-8B7E-569DEFA040D5', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (797, N'SC', 0, N'Ms.', N'Helen', N'J.', N'Dennis', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8695.694676</TotalPurchaseYTD></IndividualSurvey>', '4409E074-05F8-43EC-97C0-58EDAF8E7B89', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (799, N'SC', 0, N'Mr.', N'Prashanth', NULL, N'Desai', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-205.321</TotalPurchaseYTD></IndividualSurvey>', '3A9C3CB6-1A91-4AB2-AB16-A65AA6EB9CAE', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (801, N'SC', 0, N'Ms.', N'Bev', N'L.', N'Desalvo', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6047.245</TotalPurchaseYTD></IndividualSurvey>', '3CBB4C2B-9BA6-4FE1-AB04-06F6195A571F', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (803, N'SC', 0, N'Ms.', N'Brenda', NULL, N'Diaz', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-17479.758</TotalPurchaseYTD></IndividualSurvey>', '3B346F3B-0706-4A79-964A-D33446EA693A', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (805, N'SC', 0, N'Ms.', N'Gabriele', NULL, N'Dickmann', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9402.006</TotalPurchaseYTD></IndividualSurvey>', '4D3BDBA2-A575-4520-B9CD-19C3240A1595', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (807, N'SC', 0, N'Ms.', N'Holly', NULL, N'Dickson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7275.1536</TotalPurchaseYTD></IndividualSurvey>', 'A9823A48-8715-4635-99C2-E5106983F0A4', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (809, N'SC', 0, N'Mr.', N'Dick', NULL, N'Dievendorff', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1462.386</TotalPurchaseYTD></IndividualSurvey>', '8ED8C1DA-9A33-48A8-8473-B47085EA6772', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (811, N'SC', 0, N'Mr.', N'Rudolph', N'J.', N'Dillon', N'Sr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11516.9692</TotalPurchaseYTD></IndividualSurvey>', '8D3BF552-B9DD-43D5-B4E3-727D7B2EF9FB', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (813, N'SC', 0, N'Mr.', N'Andrew', NULL, N'Dixon', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-44002.3023</TotalPurchaseYTD></IndividualSurvey>', '4B60B9E7-9275-486C-816C-563A5869B786', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (815, N'SC', 0, N'Mr.', N'Blaine', NULL, N'Dockter', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-28362.4553</TotalPurchaseYTD></IndividualSurvey>', 'C5A18908-0DA8-43D4-95A6-27DA0D22D5C9', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (817, N'SC', 0, N'Ms.', N'Cindy', N'M.', N'Dodd', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-14131.0484</TotalPurchaseYTD></IndividualSurvey>', '3E0ED174-68E8-4A5B-9C05-6C55F4849A37', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (819, N'SC', 0, N'Mr.', N'John', N'T.', N'Donovan', N'Jr.', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-14892.8367</TotalPurchaseYTD></IndividualSurvey>', 'EC1FB547-F0C4-457E-8998-D3ECBD5A9A2C', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (821, N'SC', 0, N'Ms.', N'Patricia', NULL, N'Doyle', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4795.2682</TotalPurchaseYTD></IndividualSurvey>', 'B27D4EAC-209C-4940-BFA8-57C90A938FC0', CONVERT(datetime2, '2012-09-30T00:00:00', 127)),
    (823, N'SC', 0, N'Mr.', N'Gerald', N'M.', N'Drury', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13619.052</TotalPurchaseYTD></IndividualSurvey>', '7F0C126C-FA81-456D-A2E2-9DB418BF2D37', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (825, N'SC', 0, N'Mr.', N'Gary', N'T.', N'Drury', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4621.7156</TotalPurchaseYTD></IndividualSurvey>', 'D5E4DFBD-81CA-47AA-A3CA-97E974C31334', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (827, N'SC', 0, N'Mr.', N'Reuben', NULL, N'D''sa', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9054.124126</TotalPurchaseYTD></IndividualSurvey>', 'E0BDA6CF-EFD4-440F-873A-ECA707F6D474', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (829, N'SC', 0, N'Mr.', N'Ed', NULL, N'Dudenhoefer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1951.0288</TotalPurchaseYTD></IndividualSurvey>', 'BB198792-853E-495F-A186-BB04EF68C566', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (831, N'SC', 0, N'Mr.', N'Bernard', NULL, N'Duerr', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-18730.5835</TotalPurchaseYTD></IndividualSurvey>', 'F22DD3D1-EC3A-479C-B5B1-7250E4668F34', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (833, N'SC', 0, N'Ms.', N'Tish', N'R.', N'Duff', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2289.8924</TotalPurchaseYTD></IndividualSurvey>', '2AB2C6FE-C649-4BB0-B861-9F3098060B2F', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (835, N'SC', 0, N'Sr.', N'Adrian', NULL, N'Dumitrascu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3214.836</TotalPurchaseYTD></IndividualSurvey>', '37109ABD-952F-435A-B5EA-C7A1CBD41801', CONVERT(datetime2, '2012-10-30T00:00:00', 127)),
    (837, N'SC', 0, N'Mr.', N'Bart', NULL, N'Duncan', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4223.364</TotalPurchaseYTD></IndividualSurvey>', '8606A98E-A480-412A-98AA-6CE99295D9B1', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (839, N'SC', 0, N'Mr.', N'Maciej', NULL, N'Dusza', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3528.0164</TotalPurchaseYTD></IndividualSurvey>', '06941E78-D1B9-4EEF-9BB5-E28A5AD6F269', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (841, N'SC', 0, N'Ms.', N'Shelley', NULL, N'Dyck', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-647.988</TotalPurchaseYTD></IndividualSurvey>', '9DFE5A5A-800C-43EB-A8D0-21D142F4D35B', CONVERT(datetime2, '2014-01-29T00:00:00', 127)),
    (843, N'SC', 0, N'Ms.', N'Linda', N'R.', N'Ecoffey', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8329.6964</TotalPurchaseYTD></IndividualSurvey>', '14376057-098B-413D-8980-BEEED85F022F', CONVERT(datetime2, '2012-10-30T00:00:00', 127)),
    (845, N'SC', 0, N'Ms.', N'Carla', N'L.', N'Eldridge', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12493.8451</TotalPurchaseYTD></IndividualSurvey>', '9F63941A-82F2-4254-857C-D69880AA99BC', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (847, N'SC', 0, N'Ms.', N'Carol', N'B.', N'Elliott', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5493.8109</TotalPurchaseYTD></IndividualSurvey>', '7A23FA52-B434-4AEC-A1CB-B098F163FCD0', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (849, N'SC', 0, N'Ms.', N'Shannon', N'P.', N'Elliott', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1452.2086</TotalPurchaseYTD></IndividualSurvey>', 'A99ABE00-EC27-4F0C-BAAE-C63456944B61', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (851, N'SC', 0, N'Ms.', N'Jauna', N'E.', N'Elson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12541.9712</TotalPurchaseYTD></IndividualSurvey>', '2884D7D6-747B-473E-9910-E44E3B4DB300', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (853, N'SC', 0, N'Mr.', N'Michael', NULL, N'Emanuel', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1876.518</TotalPurchaseYTD></IndividualSurvey>', '4270FF35-5CD9-43A2-BC56-C4862D34F32F', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (855, N'SC', 0, N'Mr.', N'Terry', NULL, N'Eminhizer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-17817.885792</TotalPurchaseYTD></IndividualSurvey>', 'BD0D5959-B717-4A95-A90F-F16E5FA118EC', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (857, N'SC', 0, N'Mr.', N'John', NULL, N'Emory', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10584.318</TotalPurchaseYTD></IndividualSurvey>', '46D8141F-65D3-40EA-BE3D-8A1747C27923', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (859, N'SC', 0, N'Ms.', N'Gail', NULL, N'Erickson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2787.78375</TotalPurchaseYTD></IndividualSurvey>', 'B66B678C-3E52-4105-817F-C9832F41B884', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (861, N'SC', 0, N'Mr.', N'Mark', N'B', N'Erickson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2126.6427</TotalPurchaseYTD></IndividualSurvey>', '6EC8593F-F21D-4ADF-AB22-5D0AA1710E27', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (863, N'SC', 0, N'Ms.', N'Martha', N'R.', N'Espinoza', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5934.4566</TotalPurchaseYTD></IndividualSurvey>', 'F8959DE4-34A5-47FC-A70F-4F2ED6C9283A', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (865, N'SC', 0, N'Ms.', N'Julie', N'P.', N'Estes', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1E1EC6E1-A1D5-4BCA-9C9E-208AECD6C313', CONVERT(datetime2, '2012-12-31T00:00:00', 127)),
    (867, N'SC', 0, N'Sra.', N'Janeth', NULL, N'Esteves', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2159.7348</TotalPurchaseYTD></IndividualSurvey>', 'F856D984-5AA3-4C73-9F58-B97E7E715371', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (869, N'SC', 0, N'Ms.', N'Twanna', N'R.', N'Evans', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-397.3725</TotalPurchaseYTD></IndividualSurvey>', 'AE3C7E11-A25E-40E2-83B3-C2CFA61C4398', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (871, N'SC', 0, N'Ms.', N'Ann', N'R.', N'Evans', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2539.0238</TotalPurchaseYTD></IndividualSurvey>', 'B52B85BD-7903-4A1E-B215-072A3DEAE7E1', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (873, N'SC', 0, N'Mr.', N'John', NULL, N'Evans', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3483.0418</TotalPurchaseYTD></IndividualSurvey>', 'C1C5773F-6FBC-4636-BF03-96E804CD6A7C', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (875, N'SC', 0, N'Mr.', N'Marc', NULL, N'Faeber', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-753.8058</TotalPurchaseYTD></IndividualSurvey>', 'EBF40090-1B25-40D2-A890-DDF37F814CAA', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (877, N'SC', 0, N'Mr.', N'Fadi', NULL, N'Fakhouri', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9456.0682</TotalPurchaseYTD></IndividualSurvey>', 'B2DF4B83-4EDA-4EBD-9924-8739A7BAE2C9', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (879, N'SC', 0, N'Ms.', N'Carolyn', NULL, N'Farino', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13307.7563</TotalPurchaseYTD></IndividualSurvey>', 'F46A3A00-AB39-4ED6-AD93-A0CB1C7C065C', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (881, N'SC', 0, N'Ms.', N'Geri', N'P.', N'Farrell', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11977.365225</TotalPurchaseYTD></IndividualSurvey>', '459895F2-25CA-4B0E-A404-5435E1263ECC', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (883, N'SC', 0, N'Ms.', N'Hanying', NULL, N'Feng', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3637.8778</TotalPurchaseYTD></IndividualSurvey>', '40C40BC4-B47C-42BA-A907-DE905100615F', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (885, N'SC', 0, N'Ms.', N'Rhoda', N'J.', N'Finley', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3470.4322</TotalPurchaseYTD></IndividualSurvey>', 'FF0C8B0F-9356-4D16-994D-2B1BEBB0CC80', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (887, N'SC', 0, N'Mr.', N'Duane', N'R.', N'Fitzgerald', N'Jr.', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2689.176</TotalPurchaseYTD></IndividualSurvey>', 'CCA34D47-2995-4E4C-BEC5-1E176758A79A', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (889, N'SC', 0, N'Mr.', N'James', NULL, N'Fine', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1882.7904</TotalPurchaseYTD></IndividualSurvey>', '638E90E1-5B29-4BC9-A4E6-BD65533A4B7A', CONVERT(datetime2, '2013-09-30T00:00:00', 127)),
    (891, N'SC', 0, N'Ms.', N'Kathie', NULL, N'Flood', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3288.2067</TotalPurchaseYTD></IndividualSurvey>', 'C28FB5E2-141D-48B0-8840-D35D984698F1', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (893, N'SC', 0, N'Mr.', N'Jay', NULL, N'Fluegel', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2860.884</TotalPurchaseYTD></IndividualSurvey>', '79F1FDF3-51A6-4665-A386-14BE6411DB03', CONVERT(datetime2, '2014-05-01T00:00:00', 127)),
    (895, N'SC', 0, N'Ms.', N'Kelly', NULL, N'Focht', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8697.474375</TotalPurchaseYTD></IndividualSurvey>', '25B7E2AF-7485-40FC-93E4-9A6C6816B40E', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (897, N'SC', 0, N'Mr.', N'Jeffrey', N'L.', N'Ford', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3399.396</TotalPurchaseYTD></IndividualSurvey>', 'D953F87F-5433-41F3-A7B0-32AA6E9AA01D', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (899, N'SC', 0, N'Mr.', N'Garth', NULL, N'Fort', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-958.101</TotalPurchaseYTD></IndividualSurvey>', 'BFA8E1B8-9ABB-4842-9710-7E17B9A28348', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (901, N'SC', 0, N'Ms.', N'Dorothy', N'J.', N'Fox', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2857.1869</TotalPurchaseYTD></IndividualSurvey>', 'E4CBB1EC-77DE-4708-80BF-0AAD4C944A72', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (903, N'SC', 0, N'Ms.', N'Judith', N'B.', N'Frazier', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3152.1667</TotalPurchaseYTD></IndividualSurvey>', 'AD1F11BC-63CC-4A61-BF3B-5B2D83515AE2', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (905, N'SC', 0, N'Mr.', N'John', NULL, N'Fredericksen', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2030.73375</TotalPurchaseYTD></IndividualSurvey>', 'AB39C90F-FB62-4C1E-B2BD-8378E5C306BB', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (907, N'SC', 0, N'Ms.', N'Susan', N'E.', N'French', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8681.7192</TotalPurchaseYTD></IndividualSurvey>', '7F837621-4620-49ED-9DC1-3B762EA3821B', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (909, N'SC', 0, N'Mr.', N'Liam', N'J.', N'Friedland', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6978.222</TotalPurchaseYTD></IndividualSurvey>', '3B97FBD6-B364-4EEC-9207-17DF8F387703', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (911, N'SC', 0, N'Mr.', N'Mihail', NULL, N'Frintu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-182.352</TotalPurchaseYTD></IndividualSurvey>', 'D90A4789-1000-493C-A0EF-6A9863393254', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (913, N'SC', 0, N'Mr.', N'John', NULL, N'Ford', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5505.6174</TotalPurchaseYTD></IndividualSurvey>', '7AB0363A-C2A5-46F6-8D54-49ED1237D8F2', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (915, N'SC', 0, N'Mr.', N'Paul', N'J.', N'Fulton', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-142.6553</TotalPurchaseYTD></IndividualSurvey>', 'CEC65603-F126-4952-A706-6CE95868CEC1', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (917, N'SC', 0, N'Mr.', N'Don', NULL, N'Funk', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '1E578891-1836-494D-B8E2-C4C85B62A556', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (919, N'SC', 0, N'Mr.', N'Bob', NULL, N'Gage', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2776.782</TotalPurchaseYTD></IndividualSurvey>', '5670D671-198E-44A7-A4D1-7488A4BA0952', CONVERT(datetime2, '2012-12-31T00:00:00', 127)),
    (921, N'SC', 0, N'Ms.', N'Aldeen', N'J.', N'Gallagher', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-48.588</TotalPurchaseYTD></IndividualSurvey>', 'FEA2C5D9-7D58-4643-B220-054952B2CB98', CONVERT(datetime2, '2013-08-30T00:00:00', 127)),
    (923, N'SC', 0, N'Mr.', N'Michael', NULL, N'Galos', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-838.9178</TotalPurchaseYTD></IndividualSurvey>', '209ADEE4-B82C-4628-A216-7C41A2C58CB9', CONVERT(datetime2, '2011-12-01T00:00:00', 127)),
    (925, N'SC', 0, N'Mr.', N'Jon', NULL, N'Ganio', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13299.96075</TotalPurchaseYTD></IndividualSurvey>', 'A6AE0A24-232F-4163-916B-5FDDDF25E75C', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (927, N'SC', 0, N'Ms.', N'Kathleen', N'M.', N'Garza', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-17024.192</TotalPurchaseYTD></IndividualSurvey>', '6EB95FDA-0E00-474B-9F8A-8AD24B1BF078', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (929, N'SC', 0, N'Mr.', N'Dominic', N'P.', N'Gash', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1210.6737</TotalPurchaseYTD></IndividualSurvey>', '195FCEF8-BBB7-4CD0-9D1D-E31B4E3AE084', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (931, N'SC', 0, N'Ms.', N'Janet', N'M.', N'Gates', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-14427.257592</TotalPurchaseYTD></IndividualSurvey>', '3D33B6AC-C73F-43CA-9059-A02A5F7C96D6', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (933, N'SC', 0, N'Mr.', N'Orlando', N'N.', N'Gee', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5212.8878</TotalPurchaseYTD></IndividualSurvey>', '2B396F04-D0F4-4175-9B34-49D9083C2E69', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (935, N'SC', 0, N'Mr.', N'Darren', NULL, N'Gehring', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-20439.1534</TotalPurchaseYTD></IndividualSurvey>', 'B695BC67-5988-489B-A5AB-FF84FC9FDE7E', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (937, N'SC', 0, N'Mr.', N'Jim', NULL, N'Geist', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6453.4763</TotalPurchaseYTD></IndividualSurvey>', '3F0AA68F-506F-4781-94B4-D32148244B3D', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (939, N'SC', 0, N'Ms.', N'Barbara', N'J.', N'German', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2236.288875</TotalPurchaseYTD></IndividualSurvey>', 'AF738035-38E8-455B-B5A0-0854681BC376', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (941, N'SC', 0, N'Mr.', N'Tom', NULL, N'Getzinger', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1572.696</TotalPurchaseYTD></IndividualSurvey>', '078F4C25-68B1-4957-BB80-05A71B86646E', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (943, N'SC', 0, N'Mr.', N'Leo', NULL, N'Giakoumakis', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8760.882</TotalPurchaseYTD></IndividualSurvey>', '4857E574-4200-46EE-B775-D7E3F5451C42', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (945, N'SC', 0, N'Mr.', N'Cornett', N'L.', N'Gibbens', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7624.998</TotalPurchaseYTD></IndividualSurvey>', 'E6013E35-645D-470D-BFF3-CDA1023BC28E', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (947, N'SC', 0, N'Mr.', N'Frances', N'J.', N'Giglio', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-14726.506</TotalPurchaseYTD></IndividualSurvey>', '64A4271E-5520-4AFF-A9F5-309D7EC139A9', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (949, N'SC', 0, N'Mr.', N'Guy', NULL, N'Gilbert', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7326.3973</TotalPurchaseYTD></IndividualSurvey>', 'AB835AC6-74E5-4FB0-9D83-DC2745F487E4', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (951, N'SC', 0, N'Ms.', N'Janet', N'J.', N'Gilliat', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-315.80328</TotalPurchaseYTD></IndividualSurvey>', 'C05CEE7C-89ED-42EA-8E4A-0C379D9757A3', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (953, N'SC', 0, N'Ms.', N'Mary', N'M.', N'Gimmi', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12751.4895</TotalPurchaseYTD></IndividualSurvey>', '7BD71F64-EC08-43F5-BD1C-E64E07465A43', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (955, N'SC', 0, N'Mr.', N'David', N'L.', N'Givens', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F339E919-19E8-4ADC-80BE-F5A865AE2B54', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (957, N'SC', 0, N'Ms.', N'Jeanie', N'R.', N'Glenn', N'PhD', 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13468.0398</TotalPurchaseYTD></IndividualSurvey>', '5A9C0503-7E40-4798-A676-65277C292387', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (959, N'SC', 0, N'Ms.', N'Diane', N'R.', N'Glimp', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2064.8257</TotalPurchaseYTD></IndividualSurvey>', '43D2D6E9-FBB7-40FC-BE4E-C067BEDEAA7D', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (961, N'SC', 0, N'Mr.', N'James', N'R.', N'Glynn', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2816.6334</TotalPurchaseYTD></IndividualSurvey>', 'E1892D16-DF5B-41D6-B128-C0A2B77EF5C9', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (963, N'SC', 0, N'Mr.', N'Scott', NULL, N'Gode', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12689.3078</TotalPurchaseYTD></IndividualSurvey>', '21D1A0D9-255F-4D3D-B023-001388BBC5B2', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (965, N'SC', 0, N'Mr.', N'Mete', NULL, N'Goktepe', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5576.8058</TotalPurchaseYTD></IndividualSurvey>', 'C4C51328-CC2D-451F-9471-19BBA515B52C', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (967, N'SC', 0, N'Mr.', N'Jossef', NULL, N'Goldberg', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '593813BE-C467-4B1A-B98B-B3763D04CAA7', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (969, N'SC', 0, N'Mr.', N'Brian', N'R', N'Goldstein', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-34326.2696</TotalPurchaseYTD></IndividualSurvey>', '33847C60-967D-4663-A586-89512FC6EEE6', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (971, N'SC', 0, N'Ms.', N'Lynn', N'A.', N'Gonzales', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-400.104</TotalPurchaseYTD></IndividualSurvey>', '60D55C4A-8BCF-4503-BD87-701B65A4C4F7', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (973, N'SC', 0, N'Ms.', N'Linda', N'L.', N'Gonzales', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4225.708</TotalPurchaseYTD></IndividualSurvey>', '2910102F-2584-430F-9F6A-1E4FAB50EFE9', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (975, N'SC', 0, N'Ms.', N'Abigail', N'J.', N'Gonzalez', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7930.8163</TotalPurchaseYTD></IndividualSurvey>', '209718D7-3541-4649-964B-B87F895C63B5', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (977, N'SC', 0, N'Mr.', N'Michael', NULL, N'Graff', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7925.681</TotalPurchaseYTD></IndividualSurvey>', '1ED42026-48AE-4D74-9277-55FB69466D70', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (979, N'SC', 0, N'Mr.', N'Derek', NULL, N'Graham', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10962.918</TotalPurchaseYTD></IndividualSurvey>', 'F722FF45-3AD4-4912-8EEB-7EB71C49C512', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (981, N'SC', 0, N'Mr.', N'Lowell', N'J.', N'Graham', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2864.8513</TotalPurchaseYTD></IndividualSurvey>', '2CA49C8F-9987-4456-B89F-2ACA7A827009', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (983, N'SC', 0, N'Mr.', N'Jon', NULL, N'Grande', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3075.312</TotalPurchaseYTD></IndividualSurvey>', '8C84C445-23C4-4E56-B4CF-8A37B5A8655F', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (985, N'SC', 0, N'Ms.', N'Jane', N'P.', N'Greer', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4884.6826</TotalPurchaseYTD></IndividualSurvey>', 'DCD73FC7-1479-418D-AB89-1390889F4F0A', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (987, N'SC', 0, N'Mr.', N'Geoff', NULL, N'Grisso', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7294.722</TotalPurchaseYTD></IndividualSurvey>', '15E9078D-60DB-4B81-84D8-9DBE82871E73', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (989, N'SC', 0, N'Mr.', N'Douglas', NULL, N'Groncki', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2675.07975</TotalPurchaseYTD></IndividualSurvey>', 'D51716AC-715C-4882-AF99-F942050E2EE5', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (991, N'SC', 0, N'Mr.', N'Brian', NULL, N'Groth', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1770.6198</TotalPurchaseYTD></IndividualSurvey>', '759505FB-BC7E-44D0-862C-AB020F5EE24B', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (993, N'SC', 0, N'Ms.', N'Faith', N'M.', N'Gustafson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6652.11</TotalPurchaseYTD></IndividualSurvey>', 'C53B0FEC-1904-4895-A816-85D73512A8FF', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (995, N'SC', 0, N'Mr.', N'Greg', NULL, N'Harrison', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-288</TotalPurchaseYTD></IndividualSurvey>', '273A8514-0196-4370-8360-34161F6F9569', CONVERT(datetime2, '2013-08-30T00:00:00', 127)),
    (997, N'SC', 0, N'Ms.', N'Hattie', N'J.', N'Haemon', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12187.599375</TotalPurchaseYTD></IndividualSurvey>', '078AD793-260A-44EA-9E48-6E1550B262D4', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (999, N'SC', 0, N'Mr.', N'Matthew', N'M.', N'Hagemann', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8637.408</TotalPurchaseYTD></IndividualSurvey>', '2B2BFE50-C11F-4F8D-A2B5-3BD07C7D69A0', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1001, N'SC', 0, N'Ms.', N'Erin', N'M.', N'Hagens', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-14594.3532</TotalPurchaseYTD></IndividualSurvey>', 'CFB11FCB-979F-42A8-97A7-445110D9368E', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1003, N'SC', 0, N'Ms.', N'Betty', N'M.', N'Haines', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4035.652875</TotalPurchaseYTD></IndividualSurvey>', '24D004A7-8161-4561-A81B-01B635321CB2', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1005, N'SC', 0, N'Ms.', N'Karen', N'E.', N'Hall', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F3206D91-ABBD-4249-89A1-7975DF4C5B36', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1007, N'SC', 0, N'Mr.', N'Don', NULL, N'Hall', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '352D1885-C1AD-4363-8605-3F3B871D5148', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1009, N'SC', 0, N'Mr.', N'Bryan', NULL, N'Hamilton', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-14201.358</TotalPurchaseYTD></IndividualSurvey>', 'E37C923E-517A-4C65-A330-0564B8D03A13', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1011, N'SC', 0, N'Mr.', N'James', N'R.', N'Hamilton', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6399.5121</TotalPurchaseYTD></IndividualSurvey>', '6F252FB9-ED64-410E-B4EE-2C4B7A50F8C6', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (1013, N'SC', 0, N'Mr.', N'Kerim', NULL, N'Hanif', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10583.3402</TotalPurchaseYTD></IndividualSurvey>', '27E64B39-5471-426E-8F36-F9E14567D4ED', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1015, N'SC', 0, N'Ms.', N'Jean', N'P.', N'Handley', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13817.791675</TotalPurchaseYTD></IndividualSurvey>', '6E0ACFFE-342D-4CF4-AF91-2E8D726EB0A6', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1017, N'SC', 0, N'Mr.', N'Mark', NULL, N'Hanson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-15363.837592</TotalPurchaseYTD></IndividualSurvey>', '5906DA28-7B28-4615-9BC3-359CB0A8BDF5', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1019, N'SC', 0, N'Mr.', N'William', N'J.', N'Hapke', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-939.6934</TotalPurchaseYTD></IndividualSurvey>', '2E3B8C3F-7C67-4A83-8E48-9FF1FE275FB7', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1021, N'SC', 0, N'Ms.', N'Katherine', NULL, N'Harding', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12199.164</TotalPurchaseYTD></IndividualSurvey>', 'DA05D5A5-F0F6-464D-A969-A3BD57F8AB0B', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1023, N'SC', 0, N'Ms.', N'Kimberly', N'Beth', N'Harrington', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3689.9268</TotalPurchaseYTD></IndividualSurvey>', '357CDE49-FC26-4768-BAF4-183904A6B2C9', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1025, N'SC', 0, N'Mr.', N'Lucy', NULL, N'Harrington', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4760.592</TotalPurchaseYTD></IndividualSurvey>', '62D0A71B-A907-49BC-80E3-9586D661945B', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1027, N'SC', 0, N'Mr.', N'Keith', NULL, N'Harris', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5886.937992</TotalPurchaseYTD></IndividualSurvey>', '64ACB192-766D-47E9-B7BA-6CCF3E6A8309', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1029, N'SC', 0, N'Ms.', N'Doris', NULL, N'Hartwig', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2415.48</TotalPurchaseYTD></IndividualSurvey>', '298A910A-2659-40E3-97A7-EAD2281F3FBB', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1031, N'SC', 0, N'Mr.', N'Roger', NULL, N'Harui', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-46721.4743</TotalPurchaseYTD></IndividualSurvey>', '0A6D3974-E188-4146-8928-02F4E5358C98', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1033, N'SC', 0, N'Ms.', N'Ann', N'T.', N'Hass', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3351.4347</TotalPurchaseYTD></IndividualSurvey>', 'BA9518A7-003B-4FB6-BAB2-A52BBE6756CE', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1035, N'SC', 0, N'Mr.', N'Mark', NULL, N'Hassall', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11660.202</TotalPurchaseYTD></IndividualSurvey>', 'C6F4C2AC-2C06-4453-AE82-C86CDC0A8185', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1037, N'SC', 0, N'Mr.', N'Neal', N'B.', N'Hasty', N'III', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5294.39775</TotalPurchaseYTD></IndividualSurvey>', 'E8BCFD08-EB0D-441F-BCD5-3ED3D059DC1A', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1039, N'SC', 0, N'Mr.', N'James', N'B.', N'Haugh', N'Jr.', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-25165.2171</TotalPurchaseYTD></IndividualSurvey>', '46D384C7-E46D-4396-B3EA-DA52B5391DDE', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1041, N'SC', 0, N'Mr.', N'Jeff', NULL, N'Hay', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2927.604</TotalPurchaseYTD></IndividualSurvey>', '0C3C732B-01A7-4639-9367-13DB26FAFAB3', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1043, N'SC', 0, N'Ms.', N'Brenda', N'F.', N'Heaney', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3044.6805</TotalPurchaseYTD></IndividualSurvey>', 'FF077FB8-4764-446A-B57C-492C6537A721', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1045, N'SC', 0, NULL, N'James', NULL, N'Hendergart', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7997.91356</TotalPurchaseYTD></IndividualSurvey>', '22770301-09BD-481A-B43E-F7F6119A2B79', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1047, N'SC', 0, N'Mr.', N'John', NULL, N'Hanson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3544.227</TotalPurchaseYTD></IndividualSurvey>', '5C96AB93-1919-4CC7-A0FA-9AC1D5FF26B0', CONVERT(datetime2, '2012-08-30T00:00:00', 127)),
    (1049, N'SC', 0, N'Ms.', N'Valerie', N'M.', N'Hendricks', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6954.4314</TotalPurchaseYTD></IndividualSurvey>', '58752170-C62C-402F-919B-D057289093EA', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1051, N'SC', 0, N'Mr.', N'Jay', NULL, N'Henningsen', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-15591.158125</TotalPurchaseYTD></IndividualSurvey>', '89DED201-A34A-48DA-B465-5D8D20277B46', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1053, N'SC', 0, N'Mr.', N'Jeff', N'D.', N'Henshaw', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2669.110875</TotalPurchaseYTD></IndividualSurvey>', 'DD3CB920-10E1-4664-9009-7327E30184DA', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1055, N'SC', 0, N'Mr.', N'Kari', NULL, N'Hensien', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6056.8247</TotalPurchaseYTD></IndividualSurvey>', 'B8202390-FE7D-4F2A-98D0-405579E28E09', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1057, N'SC', 0, N'Ms.', N'Deena', N'J.', N'Herman', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4E0C7AB4-4C3A-4F19-9D46-E0DB29F49A21', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1059, N'SC', 0, N'Ms.', N'Irene', N'J.', N'Hernandez', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-299.748</TotalPurchaseYTD></IndividualSurvey>', 'EF6ED666-FAB9-42F4-A3F2-FDB293F70FC6', CONVERT(datetime2, '2014-05-01T00:00:00', 127)),
    (1061, N'SC', 0, N'Mr.', N'Jésus', NULL, N'Hernandez', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-865.866</TotalPurchaseYTD></IndividualSurvey>', '2FDFA7BE-D20A-4A8F-85BB-BD3CC9200940', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1063, N'SC', 0, N'Ms.', N'Pam', N'L.', N'Herrick', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3963.3522</TotalPurchaseYTD></IndividualSurvey>', '19662316-60F9-483B-AF1D-681BB66344F6', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1065, N'SC', 0, N'Ms.', N'Cheryl', N'M.', N'Herring', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11211.8696</TotalPurchaseYTD></IndividualSurvey>', '6F163020-E65B-4B8C-831C-5B5733BB5E81', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1067, N'SC', 0, N'Mr.', N'Ronald', N'K.', N'Heymsfield', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3953.4113</TotalPurchaseYTD></IndividualSurvey>', '476DB19C-980A-476C-837A-B72F9CD73F3C', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1069, N'SC', 0, N'Mr.', N'Sidney', NULL, N'Higa', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1045.1786</TotalPurchaseYTD></IndividualSurvey>', '1A087930-B829-4A7E-853C-95571FFD495B', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1071, N'SC', 0, N'Ms.', N'Onetha', N'F.', N'Higgs', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2850.2448</TotalPurchaseYTD></IndividualSurvey>', '6683066A-1C9F-4FAF-A520-CFF102B846C8', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1073, N'SC', 0, N'Ms.', N'Fran', N'P.', N'Highfill', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2096.5438</TotalPurchaseYTD></IndividualSurvey>', 'A76AEFB5-9E9D-4F28-95BD-D196331C21DB', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1075, N'SC', 0, N'Ms.', N'Geneva', N'T.', N'Hill', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3792.282</TotalPurchaseYTD></IndividualSurvey>', '4A86A671-3CCA-49BE-8C73-78E514D66F2A', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1077, N'SC', 0, N'Mr.', N'Andrew', N'R.', N'Hill', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1560.024</TotalPurchaseYTD></IndividualSurvey>', '5DDF5918-AEA4-4983-9DCD-38C938D04FEA', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1079, N'SC', 0, N'Mr.', N'Reinout', NULL, N'Hillmann', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1964.156625</TotalPurchaseYTD></IndividualSurvey>', 'F23E361D-3D64-493F-BF1C-EE78CDF147D8', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1081, N'SC', 0, N'Mr.', N'Mike', NULL, N'Hines', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3706.0356</TotalPurchaseYTD></IndividualSurvey>', '05B6F0C3-AF29-4BEB-9295-EAC923B8E6E3', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1083, N'SC', 0, N'Mr.', N'Matthew', NULL, N'Hink', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6830.15175</TotalPurchaseYTD></IndividualSurvey>', 'EE805F29-0F3B-4FAC-AA71-A826B7D68B1E', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1085, N'SC', 0, N'Ms.', N'Nancy', N'E.', N'Hirota', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12357.8352</TotalPurchaseYTD></IndividualSurvey>', '6B7CC8CD-DAAE-407D-87FC-DB444F734072', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1087, N'SC', 0, N'Ms.', N'Rose', NULL, N'Hirsch', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1824.036</TotalPurchaseYTD></IndividualSurvey>', 'E80CE16D-63ED-41CB-BABF-FD360A7FA11D', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1089, N'SC', 0, N'Mr.', N'Douglas', NULL, N'Hite', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5444.0304</TotalPurchaseYTD></IndividualSurvey>', 'DBF9A4E5-098B-4A55-91CC-9223A317BC4E', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1091, N'SC', 0, N'Mr.', N'David', NULL, N'Hodgson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3025.5698</TotalPurchaseYTD></IndividualSurvey>', '4EFA4D8E-3679-43AC-95D0-5CA3896F92C0', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1093, N'SC', 0, N'Ms.', N'Helge', NULL, N'Hoeing', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2275.224</TotalPurchaseYTD></IndividualSurvey>', '1C6223D2-B1BB-4EF7-86CF-806439B20A67', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1095, N'SC', 0, N'Ms.', N'Barbara', NULL, N'Hoffman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5615.8963</TotalPurchaseYTD></IndividualSurvey>', '60F53D96-0721-484B-AE3E-B2C1EE7D0504', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1097, N'SC', 0, N'Ms.', N'Holly', N'J.', N'Holt', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BD3E13F7-91EA-4E42-A78A-D6850400F9CD', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1099, N'SC', 0, N'Mr.', N'Bob', NULL, N'Hodges', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2405.97</TotalPurchaseYTD></IndividualSurvey>', 'D532040E-3BC5-46FF-BF3E-6E894C79F5E0', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1101, N'SC', 0, N'Ms.', N'Jean', N'J.', N'Holloway', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1457.7952</TotalPurchaseYTD></IndividualSurvey>', 'DC05084C-49B4-446B-B1F0-DEF425669BFD', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1103, N'SC', 0, N'Mr.', N'Michael', NULL, N'Holm', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0A161B07-06DB-445F-BA6E-45679BC3C148', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1105, N'SC', 0, N'Mr.', N'Eddie', N'M.', N'Holmes', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-780.109</TotalPurchaseYTD></IndividualSurvey>', 'E1FA30EB-9098-4329-A354-965068A842C5', CONVERT(datetime2, '2012-10-30T00:00:00', 127)),
    (1107, N'SC', 0, N'Ms.', N'Juanita', N'J.', N'Holman', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11622.4263</TotalPurchaseYTD></IndividualSurvey>', '6A76BF58-73FC-4B64-957B-FC9AA165B74F', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1109, N'SC', 0, N'Mr.', N'Peter', NULL, N'Houston', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1541.358</TotalPurchaseYTD></IndividualSurvey>', '5E4C6CB6-06CC-414E-B38E-0D8152726DCE', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1111, N'SC', 0, N'Mr.', N'Curtis', N'P.', N'Howard', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5489.1688</TotalPurchaseYTD></IndividualSurvey>', '7DBF0770-9280-489B-B283-2144D707B6AB', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1113, N'SC', 0, N'Mr.', N'Joe', NULL, N'Howard', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4606.9217</TotalPurchaseYTD></IndividualSurvey>', '9A55AF97-D7B4-4168-B189-8CB905C30811', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1115, N'SC', 0, N'Ms.', N'Janice', N'K.', N'Hows', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1692.231</TotalPurchaseYTD></IndividualSurvey>', '448BE0C1-7B1B-452F-B4D0-D1AD3587ABD2', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1117, N'SC', 0, N'Mr.', N'George', N'M.', N'Huckaby', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1727.724</TotalPurchaseYTD></IndividualSurvey>', '1B35A756-D47E-4A75-9D7A-4B0871A25B7A', CONVERT(datetime2, '2011-12-01T00:00:00', 127)),
    (1119, N'SC', 0, N'Mr.', N'Joshua', N'J.', N'Huff', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3463.2998</TotalPurchaseYTD></IndividualSurvey>', '7522288A-1D85-4EBA-8027-23E59910D917', CONVERT(datetime2, '2012-01-29T00:00:00', 127)),
    (1121, N'SC', 0, N'Ms.', N'Arlene', NULL, N'Huff', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-400.104</TotalPurchaseYTD></IndividualSurvey>', 'A824044B-2BE8-4CDC-A9EF-CD45F462687B', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1123, N'SC', 0, N'Mr.', N'Matthew', N'M.', N'Hunter', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '3608CFE5-8EA2-49C9-A9AB-39F83941FAC9', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1125, N'SC', 0, N'Ms.', N'Phyllis', N'R.', N'Huntsman', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7084.5696</TotalPurchaseYTD></IndividualSurvey>', '64953C7E-623F-4FCB-B0BF-94DCC349639E', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1127, N'SC', 0, N'Mr.', N'Lawrence', N'E.', N'Hurkett', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-367.8764</TotalPurchaseYTD></IndividualSurvey>', '7CAB3D77-85D3-402F-9B38-AC033DEFD3FD', CONVERT(datetime2, '2012-08-30T00:00:00', 127)),
    (1129, N'SC', 0, N'Mr.', N'Ryan', NULL, N'Ihrig', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4102.602</TotalPurchaseYTD></IndividualSurvey>', 'EA77D6DD-6FE7-4102-A74A-746C4F675842', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1131, N'SC', 0, N'Ms.', N'Beth', NULL, N'Inghram', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-917.506</TotalPurchaseYTD></IndividualSurvey>', '28207EF8-348A-4EE5-B377-BE13953BAE3F', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1133, N'SC', 0, N'Mr.', N'Lucio', NULL, N'Iallo', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4334.5279</TotalPurchaseYTD></IndividualSurvey>', '64341963-8BEC-4D7D-ACBB-2CF7698ABCF2', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1135, N'SC', 0, N'Mr.', N'Richard', N'L.', N'Irwin', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6660.5168</TotalPurchaseYTD></IndividualSurvey>', 'D3F02054-A958-439F-B86D-94B4B20AFE81', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1137, N'SC', 0, N'Mr.', N'Erik', NULL, N'Ismert', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6167.824</TotalPurchaseYTD></IndividualSurvey>', '7A6F688B-83AF-489F-99DA-DE15A8335925', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1139, N'SC', 0, N'Ms.', N'Denean', N'J.', N'Ison', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-366.252</TotalPurchaseYTD></IndividualSurvey>', 'A8C4775E-E0AD-4C6A-809E-0E42C4D2C8F5', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1141, N'SC', 0, N'Mr.', N'Raman', NULL, N'Iyer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3078.4498</TotalPurchaseYTD></IndividualSurvey>', '7790503C-10F1-4E9F-8D30-C4B2486355C1', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1143, N'SC', 0, N'Mr.', N'Bronson', N'R.', N'Jacobs', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1344.588</TotalPurchaseYTD></IndividualSurvey>', 'E4991566-CB50-490F-88A0-970AFFBF973C', CONVERT(datetime2, '2014-05-01T00:00:00', 127)),
    (1145, N'SC', 0, N'Mr.', N'Eric', N'A.', N'Jacobsen', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9595.510875</TotalPurchaseYTD></IndividualSurvey>', '8273B73B-1BEF-4498-9295-AB6FEFCA957C', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1147, N'SC', 0, N'Ms.', N'Jodan', N'M.', N'Jacobson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3934.4716</TotalPurchaseYTD></IndividualSurvey>', '627F073A-FC32-4A7F-80AA-9556201083C7', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1149, N'SC', 0, N'Ms.', N'Mary', NULL, N'Alexander', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4049.8548</TotalPurchaseYTD></IndividualSurvey>', 'FF8E8B89-BCFF-46AC-A215-8D581A50E156', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1151, N'SC', 0, N'Mr.', N'David', NULL, N'Jaffe', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8838.63</TotalPurchaseYTD></IndividualSurvey>', '131DB561-4D70-4A3E-9110-0B169A8F940C', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1153, N'SC', 0, N'Mr.', N'Jay', NULL, N'Jamison', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2171.3296</TotalPurchaseYTD></IndividualSurvey>', '18A24D70-D8E9-45FB-B5AC-E307F7C7BA79', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1155, N'SC', 0, N'Ms.', N'Vance', N'P.', N'Johns', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5914.4176</TotalPurchaseYTD></IndividualSurvey>', '4B4936BA-62EB-4303-9A40-469535B738A3', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1157, N'SC', 0, N'Ms.', N'Joyce', NULL, N'Jarvis', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8840.028</TotalPurchaseYTD></IndividualSurvey>', '49812AEF-524C-40F2-9CC2-754C66053C62', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1159, N'SC', 0, N'Mr.', N'George', NULL, N'Jiang', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5054.272875</TotalPurchaseYTD></IndividualSurvey>', '231D39F9-3836-46C6-8BFE-AA6617548AD5', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1161, N'SC', 0, N'Mr.', N'Stephen', N'Yuan', N'Jiang', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10422.8841</TotalPurchaseYTD></IndividualSurvey>', '12CD778A-CEA4-4789-9EE9-255F04E1FEA7', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1163, N'SC', 0, N'Mr.', N'Samuel', N'A.', N'Johnson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-25101.045192</TotalPurchaseYTD></IndividualSurvey>', 'C12F4596-1FBD-4F99-B163-0AAB59832178', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1165, N'SC', 0, N'Ms.', N'Danielle', N'B.', N'Johnson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1164.408</TotalPurchaseYTD></IndividualSurvey>', '6CDB2B14-D87A-4265-908F-67B026617275', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1167, N'SC', 0, N'Mr.', N'Greg', N'M.', N'Johnson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-362.4763</TotalPurchaseYTD></IndividualSurvey>', '2622191E-64F5-4AF6-B7C1-3771E3E4737F', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1169, N'SC', 0, N'Mr.', N'Barry', NULL, N'Johnson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5191.6962</TotalPurchaseYTD></IndividualSurvey>', '783935F5-9A74-4A6C-8A52-97D577A8F6E5', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1171, N'SC', 0, N'Mr.', N'Brian', NULL, N'Johnson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-135.078</TotalPurchaseYTD></IndividualSurvey>', '0E30C955-7867-43DF-B10D-7585C91EF22D', CONVERT(datetime2, '2013-01-28T00:00:00', 127)),
    (1173, N'SC', 0, N'Mr.', N'David', NULL, N'Johnson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-18655.0422</TotalPurchaseYTD></IndividualSurvey>', '9A499C76-ECB6-4334-9FD7-1A5B9082ADE7', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1175, N'SC', 0, N'Mr.', N'Willis', NULL, N'Johnson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-115.3616</TotalPurchaseYTD></IndividualSurvey>', '7C796E8B-722A-4495-8687-0F749148919D', CONVERT(datetime2, '2011-08-31T00:00:00', 127)),
    (1177, N'SC', 0, N'Ms.', N'Tamara', NULL, N'Johnston', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-332.358</TotalPurchaseYTD></IndividualSurvey>', 'F391731B-15F6-4D6D-AF8E-EFA1525AB8D2', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1179, N'SC', 0, N'Mr.', N'Robert', N'E.', N'Jones', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '2C91835B-FA20-4C34-BE6D-66DFD46AADE2', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1181, N'SC', 0, N'Mr.', N'Brannon', NULL, N'Jones', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-29819.7779</TotalPurchaseYTD></IndividualSurvey>', 'CE3B88D7-3320-46F7-9FB2-2052BF75A152', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1183, N'SC', 0, N'Ms.', N'Jean', NULL, N'Jordan', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6239.1524</TotalPurchaseYTD></IndividualSurvey>', 'A61EB755-6EB6-4181-864A-FC413295E66C', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1185, N'SC', 0, N'Ms.', N'Peggy', N'J.', N'Justice', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1472.2185</TotalPurchaseYTD></IndividualSurvey>', '49385D62-755D-4B24-A7F1-79A8B0093D06', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1187, N'SC', 0, N'Ms.', N'Diane', N'F.', N'Krane', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11388.7887</TotalPurchaseYTD></IndividualSurvey>', '33488099-FB42-4382-AA74-6078B6DAE7F4', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1189, N'SC', 0, N'Mr.', N'Scott', N'B.', N'Kaffer', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '4A5F2A79-0334-43BE-8BA7-44031A3252DB', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1191, N'SC', 0, N'Ms.', N'Sandra', N'T.', N'Kitt', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5929.6869</TotalPurchaseYTD></IndividualSurvey>', '0DE74214-8D0B-435E-9D07-24A043EE2EE4', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1193, N'SC', 0, N'Ms.', N'Wendy', NULL, N'Kahn', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E4C0B90C-595F-49B8-9A4D-44D490E9227D', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1195, N'SC', 0, N'Mr.', N'Sandeep', NULL, N'Kaliyath', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2041.188</TotalPurchaseYTD></IndividualSurvey>', 'B3EE6BE4-D9D6-4B86-9492-E21B507128A1', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1197, N'SC', 0, N'Mr.', N'John', NULL, N'Kane', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6501.528</TotalPurchaseYTD></IndividualSurvey>', '38A0FE20-40AB-47B9-9EEB-929D7E782E21', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1199, N'SC', 0, N'Ms.', N'Lori', NULL, N'Kane', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9703.881</TotalPurchaseYTD></IndividualSurvey>', '891A215F-103E-49E2-9E8A-DB2EE90DBC32', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1201, N'SC', 0, N'Ms.', N'Judith', N'F.', N'Krane', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3805.8795</TotalPurchaseYTD></IndividualSurvey>', 'BAF2BB1F-FEF8-4F13-9F21-E8D92E15F790', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1203, N'SC', 0, N'Ms.', N'Kay', N'E.', N'Krane', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2501.407</TotalPurchaseYTD></IndividualSurvey>', '8D74437A-0149-4139-A535-2B16CA54D4D7', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1205, N'SC', 0, N'Mr.', N'Sandeep', NULL, N'Katyal', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-16724.4292</TotalPurchaseYTD></IndividualSurvey>', 'CEA347C5-A218-4DD3-9D90-31ADE771DE19', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1207, N'SC', 0, N'Ms.', N'Bonnie', NULL, N'Kearney', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D731D368-4F47-426C-AA82-8BFA5DB79131', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1209, N'SC', 0, N'Mr.', N'Kendall', NULL, N'Keil', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12967.926</TotalPurchaseYTD></IndividualSurvey>', 'F1E588E1-B459-4338-A4C5-5D7898922BC1', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1211, N'SC', 0, N'Mr.', N'Victor', N'A.', N'Kelley', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-781.7335</TotalPurchaseYTD></IndividualSurvey>', 'DA5943B7-815C-4574-A0A4-D3B8FF2CF19F', CONVERT(datetime2, '2012-10-30T00:00:00', 127)),
    (1213, N'SC', 0, N'Mr.', N'John', NULL, N'Kelly', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12267.621525</TotalPurchaseYTD></IndividualSurvey>', 'C5CF67E1-7134-4C5E-A994-02AACE0F81DD', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1215, N'SC', 0, N'Mr.', N'Robert', NULL, N'Kelly', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1751.9487</TotalPurchaseYTD></IndividualSurvey>', '76CB0633-1C1C-45B6-87DF-158754639CDF', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1217, N'SC', 0, N'Mr.', N'Kevin', NULL, N'Kennedy', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1771.8884</TotalPurchaseYTD></IndividualSurvey>', '2C53DB93-3437-418F-B313-0B754471668D', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1219, N'SC', 0, N'Ms.', N'Mary', N'R.', N'Kesslep', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11418.3659</TotalPurchaseYTD></IndividualSurvey>', '596D929A-88D6-4C08-8F41-8B9CAFB2CC36', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1221, N'SC', 0, N'Ms.', N'Alice', N'L.', N'Kesterson', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'C97264E8-BE7C-4302-B701-44D353B687CA', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1223, N'SC', 0, N'Ms.', N'Elizabeth', NULL, N'Keyser', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1258.3767</TotalPurchaseYTD></IndividualSurvey>', '00E85176-678F-4B08-9966-A0F5F16BEB3C', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1225, N'SC', 0, N'Ms.', N'Tammy', N'J.', N'Khan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B0A01341-E8C0-475B-A718-C0FCA1A9C046', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1227, N'SC', 0, N'Mr.', N'Imtiaz', NULL, N'Khan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6485.0558</TotalPurchaseYTD></IndividualSurvey>', '6DC8C11D-E4FE-43CD-BA3D-74B4A57288A7', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1229, N'SC', 0, N'Ms.', N'Karan', NULL, N'Khanna', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1357.536</TotalPurchaseYTD></IndividualSurvey>', 'B7166CB2-8A98-4B65-95B3-DB247F8C0054', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1231, N'SC', 0, N'Mr.', N'Joe', NULL, N'Kim', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-155.964</TotalPurchaseYTD></IndividualSurvey>', '346CB799-7A96-44DC-9AE7-0F317CFC78E3', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1233, N'SC', 0, N'Mr.', N'Jim', NULL, N'Kim', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4551.48148</TotalPurchaseYTD></IndividualSurvey>', 'C2406274-C746-4600-BFB7-0FB43624D7EB', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1235, N'SC', 0, N'Mr.', N'Shane', N'S.', N'Kim', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1286.282</TotalPurchaseYTD></IndividualSurvey>', '40C3B790-FC65-49ED-AACD-AC152E7BCBF5', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1237, N'SC', 0, N'Mr.', N'Tim', NULL, N'Kim', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1823.958</TotalPurchaseYTD></IndividualSurvey>', 'FD904563-FC9F-4044-A20B-A4C6A303A52F', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1239, N'SC', 0, N'Mr.', N'Russell', NULL, N'King', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12412.6456</TotalPurchaseYTD></IndividualSurvey>', '8902C8CA-94C6-4686-99C0-1FDA74760AD6', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1241, N'SC', 0, N'Mr.', N'Anton', NULL, N'Kirilov', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8164.5356</TotalPurchaseYTD></IndividualSurvey>', '6C36C7F2-F794-477E-B94B-B375EE86FBF3', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1243, N'SC', 0, N'Mr.', N'Christian', NULL, N'Kleinerman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1118.4311</TotalPurchaseYTD></IndividualSurvey>', 'B3631E6B-3E85-4136-921A-1DCACEBB2F4C', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1245, N'SC', 0, N'Mr.', N'Andrew', N'P.', N'Kobylinski', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8097.6664</TotalPurchaseYTD></IndividualSurvey>', '49F78DAA-9DAF-449C-80E5-AA33DAA62017', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (1247, N'SC', 0, N'Mr.', N'Reed', NULL, N'Koch', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1869.259584</TotalPurchaseYTD></IndividualSurvey>', 'AC327BE8-67EB-403A-9A8C-D60B67BB54BE', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1249, N'SC', 0, N'Mr.', N'Jim', NULL, N'Kennedy', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8249.076</TotalPurchaseYTD></IndividualSurvey>', '0EB10B35-6A95-4156-83BC-F0598EA856DC', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1251, N'SC', 0, N'Mr.', N'Kirk', N'T', N'King', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9975.246</TotalPurchaseYTD></IndividualSurvey>', 'A40D8239-5F34-4F2D-B4F0-F3C47CEFC6A8', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1253, N'SC', 0, N'Mr.', N'Eugene', NULL, N'Kogan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-25834.042</TotalPurchaseYTD></IndividualSurvey>', 'A3D52266-4E01-4892-904C-E31D6775BE68', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1255, N'SC', 0, N'Mr.', N'Scott', NULL, N'Konersmann', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2165.7328</TotalPurchaseYTD></IndividualSurvey>', '2AC3D1FC-31A4-469B-8116-499792BCC69A', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1257, N'SC', 0, N'Ms.', N'Joy', N'R.', N'Koski', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7283.2227</TotalPurchaseYTD></IndividualSurvey>', '191C04A8-8D77-4E34-BF26-99DFCC83A7DB', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1259, N'SC', 0, N'Ms.', N'Pamala', N'M.', N'Kotc', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2108.88</TotalPurchaseYTD></IndividualSurvey>', 'D4760B72-6EB5-46BB-81AD-51CB11AE7919', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1261, N'SC', 0, N'Mr.', N'Edward', N'J.', N'Kozlowski', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10287.846375</TotalPurchaseYTD></IndividualSurvey>', '237CFF8A-FC5F-43ED-8BDD-257A989F2FA0', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1263, N'SC', 0, N'Mr.', N'James', N'D.', N'Kramer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'FBA8126E-A78C-4D0A-AFFB-E78BC3017B12', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1265, N'SC', 0, N'Mr.', N'Mitch', NULL, N'Kennedy', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-17019.977325</TotalPurchaseYTD></IndividualSurvey>', '884C6553-2769-46FC-AE38-D219A6CDFA9B', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1267, N'SC', 0, N'Mr.', N'James', N'J.', N'Krow', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '797C0054-8145-4277-9E26-3C3EBD1B1864', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1269, N'SC', 0, N'Ms.', N'Margaret', N'T.', N'Krupka', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-18996.6408</TotalPurchaseYTD></IndividualSurvey>', '92592686-1A2D-4255-87A2-E5E43538090B', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1271, N'SC', 0, N'Mr.', N'Deepak', NULL, N'Kumar', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4754.928</TotalPurchaseYTD></IndividualSurvey>', '3D311A37-3355-491D-ACD2-07C91E4ADC14', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1273, N'SC', 0, N'Mr.', N'Jeffrey', N'B.', N'Kung', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5146.644</TotalPurchaseYTD></IndividualSurvey>', '0DEED92A-B15E-4CF6-932E-5E0E4C168A18', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1275, N'SC', 0, N'Mr.', N'Vamsi', NULL, N'Kuppa', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2239.296</TotalPurchaseYTD></IndividualSurvey>', '9F82648A-ED4B-4FD0-AC97-85B1ECE45594', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1277, N'SC', 0, N'Mr.', N'Jeffrey', NULL, N'Kurtz', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5619.3654</TotalPurchaseYTD></IndividualSurvey>', '141DBA98-A629-43F2-84E7-D9F7CB398EBF', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1279, N'SC', 0, N'Mr.', N'Peter', NULL, N'Kurniawan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1178.1911</TotalPurchaseYTD></IndividualSurvey>', '1398EAC2-C855-49FC-960C-2B6468803F8A', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (1281, N'SC', 0, N'Mr.', N'Eric', NULL, N'Lang', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2855.498625</TotalPurchaseYTD></IndividualSurvey>', '5DAFD177-9FBD-43AC-BB52-E391A4D667F3', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1283, N'SC', 0, N'Ms.', N'Rebecca', NULL, N'Laszlo', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8833.548</TotalPurchaseYTD></IndividualSurvey>', 'E36AE0A1-C262-4227-AD4F-2660090492F0', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1285, N'SC', 0, N'Ms.', N'Cecilia', N'M.', N'Laursen', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-156.024</TotalPurchaseYTD></IndividualSurvey>', '4086B1F4-3295-476B-BC44-8FE6EAC35D33', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1287, N'SC', 0, N'Mr.', N'David', N'O', N'Lawrence', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-404.664</TotalPurchaseYTD></IndividualSurvey>', 'A3F2188D-2A4F-4E53-9DA1-162739BB80B7', CONVERT(datetime2, '2012-10-30T00:00:00', 127)),
    (1289, N'SC', 0, N'Ms.', N'Elsa', NULL, N'Leavitt', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12476.144</TotalPurchaseYTD></IndividualSurvey>', 'F1A22FC4-BBDE-4B71-8216-CDF1D6D53506', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1291, N'SC', 0, N'Mr.', N'Michael', N'J.', N'Lee', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2916.088875</TotalPurchaseYTD></IndividualSurvey>', '56F48EE0-AE0E-4A09-B55F-66CFBCCC7486', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1293, N'SC', 0, N'Ms.', N'Marjorie', N'M.', N'Lee', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1132.859476</TotalPurchaseYTD></IndividualSurvey>', 'E9C645D5-A101-4C4E-968A-493AE907E098', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1295, N'SC', 0, N'Mr.', N'Frank', NULL, N'Campbell', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-16315.0115</TotalPurchaseYTD></IndividualSurvey>', 'FC564029-F29A-47ED-9F49-BFA7E8E1DFD6', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1297, N'SC', 0, N'Mr.', N'Mark', NULL, N'Lee', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2357.16</TotalPurchaseYTD></IndividualSurvey>', '1186CA57-C6B5-4893-8B7F-1F211772536D', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1299, N'SC', 0, N'Mr.', N'Robertson', NULL, N'Lee', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3719.412</TotalPurchaseYTD></IndividualSurvey>', '3A3D92B3-028C-42A5-84A5-FEDCDDA5D351', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1301, N'SC', 0, N'Ms.', N'Jolie', NULL, N'Lenehan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2258.4124</TotalPurchaseYTD></IndividualSurvey>', '4D714D1D-B458-4555-BA2C-0DC3C82F268E', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1303, N'SC', 0, N'Mr.', N'Roger', NULL, N'Lengel', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-17090.4</TotalPurchaseYTD></IndividualSurvey>', 'E462E2DD-8A56-4641-9605-21CAEA5C74CF', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1305, N'SC', 0, NULL, N'A.', N'Francesca', N'Leonetti', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4256.3016</TotalPurchaseYTD></IndividualSurvey>', '1E1403A8-6B43-4E6B-861C-1D49E6F23AC8', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1307, N'SC', 0, N'Ms.', N'Bonnie', N'B.', N'Lepro', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4165.5585</TotalPurchaseYTD></IndividualSurvey>', '3F69F157-009C-4FA1-8728-4F0FEBB30C09', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1309, N'SC', 0, N'Ms.', N'Gloria', N'J.', N'Lesko', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-15430.1657</TotalPurchaseYTD></IndividualSurvey>', 'D30ABE00-4F94-437F-966B-F61A5C2E7DFC', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1311, N'SC', 0, N'Ms.', N'Linda', NULL, N'Leste', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-16725.3924</TotalPurchaseYTD></IndividualSurvey>', 'C68C07A7-3C6C-42F2-AF9E-F16DBE26BAB8', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1313, N'SC', 0, N'Mr.', N'Steven', N'B.', N'Levy', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '77046421-3852-49B0-A0A1-2E8B8B378391', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1315, N'SC', 0, N'Ms.', N'Judy', NULL, N'Lew', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9084.1833</TotalPurchaseYTD></IndividualSurvey>', 'A80B439E-EEB3-4761-A0A6-7B8857CFC781', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1317, N'SC', 0, N'Ms.', N'Elsie', N'L.', N'Lewin', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5356.608</TotalPurchaseYTD></IndividualSurvey>', 'E93D81BE-EBE9-46D6-94E7-AC64F25516B8', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1319, N'SC', 0, N'Mr.', N'George', N'Z.', N'Li', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4925.3913</TotalPurchaseYTD></IndividualSurvey>', '7993BB35-4963-49C4-B677-A372D689BA64', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1321, N'SC', 0, N'Mr.', N'Yale', NULL, N'Li', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5389.533825</TotalPurchaseYTD></IndividualSurvey>', '96FB75A0-37C1-4138-9E14-E828D3648EAD', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1323, N'SC', 0, N'Ms.', N'Yan', NULL, N'Li', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7294.4246</TotalPurchaseYTD></IndividualSurvey>', 'CF457803-118A-4E24-B9F7-9151F104EBCF', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1325, N'SC', 0, N'Ms.', N'Yuhong', NULL, N'Li', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8020.1574</TotalPurchaseYTD></IndividualSurvey>', '39D3B8EE-A3CA-4DA5-BCF4-6D30F61BC6D1', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1327, N'SC', 0, N'Mr.', N'Joseph', N'M.', N'Lique', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6664.3908</TotalPurchaseYTD></IndividualSurvey>', '977EBB32-1CB8-4E96-A7E5-089EC3A4CE50', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1329, N'SC', 0, N'Mr.', N'Paulo', N'H.', N'Lisboa', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3638.814</TotalPurchaseYTD></IndividualSurvey>', '87D7B397-3FDC-4C07-846B-BEEDB6665BCA', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1331, N'SC', 0, N'Mr.', N'David', N'J.', N'Liu', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-35549.7461</TotalPurchaseYTD></IndividualSurvey>', 'E511D139-9556-4D36-97B6-C4ADA97DFDCD', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1333, N'SC', 0, NULL, N'Jinghao', NULL, N'Liu', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9708.2505</TotalPurchaseYTD></IndividualSurvey>', '3EF93BF0-BB51-4BB9-9718-BBE45CB21A35', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1335, N'SC', 0, N'Mr.', N'Kevin', NULL, N'Liu', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3522.09819</TotalPurchaseYTD></IndividualSurvey>', '6493BA77-F11F-4574-A026-24EEFC77AFAF', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1337, N'SC', 0, N'Mr.', N'Run', NULL, N'Liu', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-20972.4849</TotalPurchaseYTD></IndividualSurvey>', 'B8E6687A-9217-491B-B5AF-5E5009DC8434', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1339, N'SC', 0, N'Mr.', N'Todd', N'R.', N'Logan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2978.3265</TotalPurchaseYTD></IndividualSurvey>', '37C4E8BB-AECA-455A-91F0-A59C3F928D38', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1341, N'SC', 0, N'Mr.', N'Kok-Ho', NULL, N'Loh', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3024.8796</TotalPurchaseYTD></IndividualSurvey>', 'A5992E48-F0BB-4F18-80CF-017B1596FE76', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1343, N'SC', 0, N'Mr.', N'John', N'K.', N'Long', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9495.6099</TotalPurchaseYTD></IndividualSurvey>', 'FF38055E-D73F-4450-A98A-8A237423160A', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1345, N'SC', 0, N'Ms.', N'Sharon', N'J.', N'Looney', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3533.9869</TotalPurchaseYTD></IndividualSurvey>', '2337E86B-0D46-49CB-94FE-E80027511575', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1347, N'SC', 0, N'Mr.', N'Jeremy', NULL, N'Los', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '64F81ED9-A92B-43D8-8129-85F6D038C267', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1349, N'SC', 0, N'Mr.', N'Spencer', NULL, N'Low', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-21227.2898</TotalPurchaseYTD></IndividualSurvey>', '07F3859D-4B0D-46A7-8876-38DAB0B60FDA', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1351, N'SC', 0, N'Ms.', N'Anita', N'R.', N'Lucerne', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7494.35775</TotalPurchaseYTD></IndividualSurvey>', '5C28FCE8-DB52-4222-BD91-2AB9FED54696', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1353, N'SC', 0, N'Mr.', N'Jose', NULL, N'Lugo', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6878.16</TotalPurchaseYTD></IndividualSurvey>', 'A6627E59-66AB-4347-BDF7-B78956633EDE', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1355, N'SC', 0, N'Mr.', N'Richard', NULL, N'Lum', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4567.23766</TotalPurchaseYTD></IndividualSurvey>', 'AD9AD3EA-FC58-4F9A-B0EE-2293B87BAEAF', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1357, N'SC', 0, N'Ms.', N'Judy', N'R.', N'Lundahl', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-18027.5247</TotalPurchaseYTD></IndividualSurvey>', '79A79E46-4CEF-4DAD-BFAB-3C352C0B32DC', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1359, N'SC', 0, N'Mr.', N'Sean', N'J.', N'Lunt', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '62E81186-C871-4703-9671-E99295BCE668', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1361, N'SC', 0, N'Ms.', N'Helen', N'R.', N'Lutes', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-19717.022</TotalPurchaseYTD></IndividualSurvey>', '34698CFB-3401-4AB7-A4F2-36330167FDE8', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1363, N'SC', 0, N'Mr.', N'Robert', N'P.', N'Lyeba', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5434.7106</TotalPurchaseYTD></IndividualSurvey>', '924982EF-745A-4F0C-965E-B2B47ADAC6CD', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1365, N'SC', 0, N'Ms.', N'Sharon', N'F.', N'Lynn', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5776.524</TotalPurchaseYTD></IndividualSurvey>', 'A3A292D7-0601-4912-A06A-B5507230E68C', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1367, N'SC', 0, N'Mr.', N'Robert', NULL, N'Lyon', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4463.957</TotalPurchaseYTD></IndividualSurvey>', 'B894EE96-2BFA-41C8-B0D2-F819F5F2E4BE', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1369, N'SC', 0, N'Ms.', N'Jenny', NULL, N'Lysaker', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3699.894</TotalPurchaseYTD></IndividualSurvey>', '094C19E1-D85C-439D-86F4-552F1EE7A05F', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1371, N'SC', 0, N'Ms.', N'Denise', N'R.', N'Maccietto', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6857.3454</TotalPurchaseYTD></IndividualSurvey>', '22D1058E-C2B1-4D97-AB64-421874F719FB', CONVERT(datetime2, '2011-08-31T00:00:00', 127)),
    (1373, N'SC', 0, N'Mr.', N'Scott', NULL, N'MacDonald', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-16286.946</TotalPurchaseYTD></IndividualSurvey>', '77527F01-CF80-46AB-868A-75F891E7B81D', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1375, N'SC', 0, N'Mr.', N'Walter', N'J.', N'Mays', N'Sr.', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6291.4342</TotalPurchaseYTD></IndividualSurvey>', '45306013-4C42-40BE-969E-89CC1F40972C', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1377, N'SC', 0, N'Mr.', N'Patrick', N'J.', N'Magenheimer', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6219.6683</TotalPurchaseYTD></IndividualSurvey>', 'F498A20F-1759-4296-9CEE-C07519054063', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1379, N'SC', 0, N'Ms.', N'Kimberly', N'N.', N'Malmendier', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2065.866</TotalPurchaseYTD></IndividualSurvey>', '6FEF2768-5E92-4FE7-B94E-3AC5E492E2A4', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1381, N'SC', 0, N'Mr.', N'Ajay', NULL, N'Manchepalli', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-563.946</TotalPurchaseYTD></IndividualSurvey>', '5DD53673-7012-4821-9171-4D79880B751D', CONVERT(datetime2, '2013-12-31T00:00:00', 127)),
    (1383, N'SC', 0, N'Ms.', N'Parul', NULL, N'Manek', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2914.728</TotalPurchaseYTD></IndividualSurvey>', '2270A8B3-97FD-49A1-ABA3-C2F61FD5BCEA', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1385, N'SC', 0, N'Mr.', N'Tomas', N'M.', N'Manzanares', N'II', 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-364.704</TotalPurchaseYTD></IndividualSurvey>', 'A573668A-D115-4E03-9D9B-955BFD65D671', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1387, N'SC', 0, N'Ms.', N'Kathy', N'R.', N'Marcovecchio', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-9758.058</TotalPurchaseYTD></IndividualSurvey>', '71B83B01-15C3-4DB4-A1F9-46A8A8A055C8', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1389, N'SC', 0, N'Ms.', N'Jill', N'K.', N'Markwood', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '143C728D-7946-4796-91A6-41214EF4EBB2', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1391, N'SC', 0, N'Ms.', N'Melissa', N'R.', N'Marple', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7507.2519</TotalPurchaseYTD></IndividualSurvey>', '3AA82687-F265-4939-889F-275362B977AC', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (1393, N'SC', 0, N'Ms.', N'Cecelia', N'L.', N'Marshall', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-711.25</TotalPurchaseYTD></IndividualSurvey>', 'D5334C1B-5F15-4141-AADF-59EFCB72CE5D', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1395, N'SC', 0, N'Mr.', N'Benjamin', NULL, N'Martin', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-683.724</TotalPurchaseYTD></IndividualSurvey>', '9EFCDAA0-8DB7-4E22-9E9D-08E710EF62A4', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1397, N'SC', 0, N'Ms.', N'Linda', NULL, N'Martin', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12902.4311</TotalPurchaseYTD></IndividualSurvey>', '3BFF9302-E5DB-4F8D-983C-2BA0A7EB50B9', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1399, N'SC', 0, N'Ms.', N'Mindy', NULL, N'Martin', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3492.8728</TotalPurchaseYTD></IndividualSurvey>', '3CC90EA4-6133-4153-B54F-D7083D15821D', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1401, N'SC', 0, N'Ms.', N'Sandra', N'I.', N'Martinez', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'E700EC61-7F49-4DB3-B062-7E31093EDD02', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1403, N'SC', 0, N'Mr.', N'Frank', NULL, N'Mart¡nez', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-26077.157268</TotalPurchaseYTD></IndividualSurvey>', '6CCA7B0A-A6B5-499C-AC9C-739E1BD05ADE', CONVERT(datetime2, '2013-05-30T00:00:00', 127)),
    (1405, N'SC', 0, N'Mr.', N'Steve', NULL, N'Masters', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10443.8046</TotalPurchaseYTD></IndividualSurvey>', '416BDE0F-76A9-4D9B-AE96-B2E92D25A621', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1407, N'SC', 0, N'Mr.', N'Joseph', NULL, N'Matthews', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-824.73</TotalPurchaseYTD></IndividualSurvey>', 'D0EC82B9-52FC-4E84-8525-247F6200FF9D', CONVERT(datetime2, '2012-09-30T00:00:00', 127)),
    (1409, N'SC', 0, N'Ms.', N'Jennifer', N'J.', N'Maxham', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6131.9367</TotalPurchaseYTD></IndividualSurvey>', '87CB3177-81C4-4071-BB37-7726259D2046', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1411, N'SC', 0, N'Mr.', N'Chris', NULL, N'Maxwell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4288.0306</TotalPurchaseYTD></IndividualSurvey>', '8D0C2B71-009B-45B8-907E-2D90B1C53127', CONVERT(datetime2, '2012-09-30T00:00:00', 127)),
    (1413, N'SC', 0, N'Ms.', N'Sandra', N'B.', N'Maynard', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-12044.95685</TotalPurchaseYTD></IndividualSurvey>', '66423D78-B443-4AF8-954F-2EBA29F5F1C7', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1415, N'SC', 0, N'Mr.', N'John', N'J.', N'McClane', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1956.3698</TotalPurchaseYTD></IndividualSurvey>', '97175B3E-D8BF-47C7-8288-88F084632343', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1417, N'SC', 0, N'Ms.', N'Robin', N'M.', N'McGuigan', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8575.8129</TotalPurchaseYTD></IndividualSurvey>', '8C0A56CB-8E6F-4F54-8F41-903FB62A3051', CONVERT(datetime2, '2011-05-31T00:00:00', 127)),
    (1419, N'SC', 0, N'Ms.', N'Stacie', N'K.', N'Mcanich', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7974.804</TotalPurchaseYTD></IndividualSurvey>', '19DD2C32-77C6-4E41-BFE9-5E63B6D79DBC', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1421, N'SC', 0, N'Ms.', N'Katie', NULL, N'McAskill-White', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-10262.768999</TotalPurchaseYTD></IndividualSurvey>', '043E4B9E-A58E-478D-845D-AEF0BCA5C51D', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1423, N'SC', 0, N'Ms.', N'Lola', N'M.', N'McCarthy', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-16876.3516</TotalPurchaseYTD></IndividualSurvey>', 'CB93BF5D-5AFE-410A-A83D-D9669AD65131', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1425, N'SC', 0, N'Ms.', N'Jane', N'A.', N'McCarty', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13055.5736</TotalPurchaseYTD></IndividualSurvey>', '04BFF651-678A-491A-8060-D625DE99FA1B', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1427, N'SC', 0, N'Ms.', N'Nikki', NULL, N'McCormick', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2085.0114</TotalPurchaseYTD></IndividualSurvey>', '4881DC6E-51BB-482C-AA97-A97DA4DE14E9', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1429, N'SC', 0, N'Mr.', N'James', N'L.', N'McCoy', N'II', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-15390.6382</TotalPurchaseYTD></IndividualSurvey>', '6B4F444C-F70D-46BD-9844-BC4E0CBFBD18', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1431, N'SC', 0, N'Ms.', N'Christinia', N'A.', N'McDonald', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1610.334</TotalPurchaseYTD></IndividualSurvey>', '55270F25-C806-4C69-90A5-76F5FB9B3E30', CONVERT(datetime2, '2013-08-30T00:00:00', 127)),
    (1433, N'SC', 0, N'Mr.', N'Alejandro', NULL, N'McGuel', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1123.854</TotalPurchaseYTD></IndividualSurvey>', '87CD7A8E-C675-4E30-9D52-79B47C138FC7', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1435, N'SC', 0, N'Ms.', N'Yvonne', NULL, N'McKay', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8548.8998</TotalPurchaseYTD></IndividualSurvey>', 'C2287535-1A4D-4ABB-AACE-87C7EF315D6B', CONVERT(datetime2, '2011-08-01T00:00:00', 127)),
    (1437, N'SC', 0, N'Ms.', N'Nkenge', NULL, N'McLin', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-21623.3869</TotalPurchaseYTD></IndividualSurvey>', '488BC551-1F37-4203-B5B4-B1505C430D17', CONVERT(datetime2, '2011-07-01T00:00:00', 127)),
    (1439, N'SC', 0, N'Ms.', N'Nancy', N'A.', N'McPhearson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-5733.978</TotalPurchaseYTD></IndividualSurvey>', 'CA7F46A5-C071-49A4-8F3B-66CE13EF5D85', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1441, N'SC', 0, N'Ms.', N'Nellie', N'T.', N'Medina', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1019.29575</TotalPurchaseYTD></IndividualSurvey>', '6DFAC7EC-0A3A-432F-9CAF-691B7E1F3845', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1443, N'SC', 0, N'Ms.', N'Raquel', NULL, N'Mello', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-11744.8338</TotalPurchaseYTD></IndividualSurvey>', 'D6A548D6-676E-434C-9133-D4C0B3B90564', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1445, N'SC', 0, N'Ms.', N'Gladys', N'F.', N'Mendiola', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '29E7C318-AF58-4E1B-8F2F-C44E5CBD12A2', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1447, N'SC', 0, N'Mr.', N'R. Morgan', N'L.', N'Mendoza', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1202.22</TotalPurchaseYTD></IndividualSurvey>', 'D0E6394D-E7DB-4CC8-8D6E-92AB6951F426', CONVERT(datetime2, '2013-04-30T00:00:00', 127)),
    (1449, N'SC', 0, N'Mr.', N'Tosh', NULL, N'Meston', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7848.8242</TotalPurchaseYTD></IndividualSurvey>', 'D2EAF764-131F-4A73-924F-507AC5A14DC1', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1451, N'SC', 0, N'Ms.', N'Susan', NULL, N'Metters', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0B03A0AF-3897-45F7-AC4E-508771A1137B', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1453, N'SC', 0, N'Mr.', N'Stephen', N'A.', N'Mew', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1713.7118</TotalPurchaseYTD></IndividualSurvey>', '278FA142-89D8-42F1-80A1-E3000B642800', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (1455, N'SC', 0, N'Mr.', N'Eric', N'B.', N'Meyer', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-13356.4863</TotalPurchaseYTD></IndividualSurvey>', '63201227-20A4-497D-BC84-69517711C3BE', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (1457, N'SC', 0, N'Ms.', N'Helen', N'M.', N'Meyer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6316.4716</TotalPurchaseYTD></IndividualSurvey>', 'D1C4D14E-CC64-4FE8-AD40-373C0D426885', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (1459, N'SC', 0, N'Ms.', N'Deanna', NULL, N'Meyer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2574.4342</TotalPurchaseYTD></IndividualSurvey>', 'F9B3FE97-BC2F-4D22-B632-3678AB37AD75', CONVERT(datetime2, '2011-10-31T00:00:00', 127)),
    (1461, N'SC', 0, N'Mr.', N'Gary', N'P.', N'Meyerhoff', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3114.096</TotalPurchaseYTD></IndividualSurvey>', 'C1C82D81-8F35-4C73-9B10-F21C25B8080E', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1463, N'SC', 0, N'Mr.', N'Ramesh', NULL, N'Meyyappan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2206.122</TotalPurchaseYTD></IndividualSurvey>', '074E4861-0B71-4A94-89D0-94A58A2EDE44', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1465, N'SC', 0, N'Mr.', N'Thomas', N'R.', N'Michaels', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2458.9178</TotalPurchaseYTD></IndividualSurvey>', '7A43F075-EFA0-4617-AE12-DDBF8AF72B58', CONVERT(datetime2, '2012-09-30T00:00:00', 127)),
    (1467, N'SC', 0, N'Mr.', N'Jan', NULL, N'Miksovsky', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1366.2229</TotalPurchaseYTD></IndividualSurvey>', 'A56A8EB0-444C-40E7-8FC0-E11668D12375', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1469, N'SC', 0, N'Ms.', N'Virginia', N'L.', N'Miller', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1191.954192</TotalPurchaseYTD></IndividualSurvey>', 'D8B3540A-4A9E-4538-949B-C052DE95E0BD', CONVERT(datetime2, '2012-05-30T00:00:00', 127)),
    (1471, N'SC', 0, N'Mr.', N'Matthew', N'J.', N'Miller', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6207.012</TotalPurchaseYTD></IndividualSurvey>', '973E3B7D-6844-4F66-83DD-687E1A06178F', CONVERT(datetime2, '2012-07-31T00:00:00', 127)),
    (1473, N'SC', 0, N'Mr.', N'Emilo', N'R.', N'Miller', N'Jr.', 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-2115.03</TotalPurchaseYTD></IndividualSurvey>', 'E7D01A5E-348D-436F-A59B-6F234F8C35E0', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1475, N'SC', 0, N'Mr.', N'Ben', NULL, N'Miller', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-6000.798</TotalPurchaseYTD></IndividualSurvey>', '8CBF0F79-F979-495A-8044-4A8F0629751A', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1477, N'SC', 0, N'Mr.', N'Dylan', NULL, N'Miller', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-8558.2205</TotalPurchaseYTD></IndividualSurvey>', 'B6A6DFE5-5AF5-4052-A88A-0CA437E6036E', CONVERT(datetime2, '2011-10-01T00:00:00', 127)),
    (1479, N'SC', 0, N'Mr.', N'Frank', NULL, N'Miller', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-1871.1352</TotalPurchaseYTD></IndividualSurvey>', 'D3B77A11-4CA5-433D-A956-A1B34E18360D', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1481, N'SC', 0, N'Ms.', N'Neva', N'M.', N'Mitchell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-3755.180952</TotalPurchaseYTD></IndividualSurvey>', '5954BC0D-6F69-487A-B721-40F1D9857B24', CONVERT(datetime2, '2013-06-30T00:00:00', 127)),
    (1483, N'SC', 0, N'Ms.', N'Linda', NULL, N'Mitchell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-80.91</TotalPurchaseYTD></IndividualSurvey>', 'A24F2479-2DE8-464F-9BDA-FED6F4165AAC', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1485, N'SC', 0, N'Mr.', N'Scott', NULL, N'Mitchell', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-7925.9676</TotalPurchaseYTD></IndividualSurvey>', 'C6F97317-1E7C-4CB2-8475-91BD88BEF25A', CONVERT(datetime2, '2012-06-30T00:00:00', 127)),
    (1487, N'SC', 0, N'Mr.', N'Robert', NULL, N'Mitosinka', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5A5045B3-1F75-4181-A41B-32D418E175C4', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1489, N'SC', 0, N'Mr.', N'Joseph', N'P.', N'Mitzner', N'Jr.', 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>-4387.62</TotalPurchaseYTD></IndividualSurvey>', 'FA5EFC39-5704-48AF-9A59-4431A2E1770F', CONVERT(datetime2, '2013-07-31T00:00:00', 127)),
    (1491, N'VC', 0, N'Ms.', N'Paula', N'B.', N'Moberly', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9E368CD5-E5B6-43B4-A006-31D018D7C8E7', CONVERT(datetime2, '2011-12-23T00:00:00', 127)),
    (1493, N'VC', 0, N'Ms.', N'Suchitra', NULL, N'Mohan', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '43F4AFC3-86D7-4E2E-B585-1709DE5E0488', CONVERT(datetime2, '2011-04-25T00:00:00', 127)),
    (1495, N'VC', 0, N'Mr.', N'Jonathan', NULL, N'Moeller', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '5707FDA4-1564-4650-9068-643DAC5F3A45', CONVERT(datetime2, '2011-04-25T00:00:00', 127)),
    (1497, N'VC', 0, N'Mr.', N'William', N'J.', N'Monroe', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '08658234-8FB7-4CA8-A175-7D25BF2D279D', CONVERT(datetime2, '2012-02-03T00:00:00', 127)),
    (1499, N'VC', 0, N'Ms.', N'Alan', N'L.', N'Monitor', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'D78842CD-A022-4C78-9E05-0A8388B95AE3', CONVERT(datetime2, '2012-02-02T00:00:00', 127)),
    (1501, N'VC', 0, N'Ms.', N'Hillaine', N'N.', N'Montera', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '27440F5A-748B-4AC3-ABB8-5A1F14B55F23', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1503, N'VC', 0, N'Mr.', N'Bobby', NULL, N'Moore', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0DF34150-EF64-498C-B804-E7A773ED0BF5', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1505, N'VC', 0, N'Ms.', N'Barbara', NULL, N'Moreland', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DDAC3657-9C8B-4EBD-A800-5B0CEADD4E08', CONVERT(datetime2, '2012-01-25T00:00:00', 127)),
    (1507, N'VC', 0, N'Mr.', N'Jon', NULL, N'Morris', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'A691F19D-4968-47D0-8E19-0ECCAC1B23EB', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1509, N'VC', 0, N'Ms.', N'Julia', NULL, N'Moseley', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'AD2071C7-AF42-4015-AEFC-C8A7BBCBC39D', CONVERT(datetime2, '2012-01-25T00:00:00', 127)),
    (1511, N'VC', 0, N'Ms.', N'Marie', N'E.', N'Moya', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '978D673F-1EF2-4E58-9962-29F4B4503082', CONVERT(datetime2, '2011-12-23T00:00:00', 127)),
    (1513, N'VC', 0, N'Mr.', N'Zheng', NULL, N'Mu', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CFF47D41-05C5-4786-9A07-8E28BBDB29E4', CONVERT(datetime2, '2012-02-03T00:00:00', 127)),
    (1515, N'VC', 0, N'Mr.', N'Salman', NULL, N'Mughal', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F502F0F9-97DF-4E2A-B49E-D61EC2B37726', CONVERT(datetime2, '2012-01-25T00:00:00', 127)),
    (1517, N'VC', 0, N'Mr.', N'Albert', NULL, N'Mungin', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6CA86E4E-FDAB-4403-96D0-55750F18673B', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1519, N'VC', 0, N'Mr.', N'Stuart', NULL, N'Munson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8C23B46F-090B-49D9-B25C-A196DC6ED99F', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1521, N'VC', 0, N'Ms.', N'Billie Jo', NULL, N'Murray', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0D85BFF5-7900-4930-A093-253041883930', CONVERT(datetime2, '2012-01-25T00:00:00', 127)),
    (1523, N'VC', 0, N'Ms.', N'Dorothy', N'J.', N'Myer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DF46EA08-DFD7-45F9-9D33-1AC3F0678426', CONVERT(datetime2, '2012-02-02T00:00:00', 127)),
    (1525, N'VC', 0, N'Mr.', N'Ken', NULL, N'Myer', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '0D7E8D2A-CE67-431D-B66D-4D5A643544FD', CONVERT(datetime2, '2012-01-25T00:00:00', 127)),
    (1527, N'VC', 0, N'Ms.', N'Suzanne', NULL, N'Nelson', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F501A45F-B97F-42DE-BFE0-5A1D6A416239', CONVERT(datetime2, '2012-02-03T00:00:00', 127)),
    (1529, N'VC', 0, NULL, N'Mandar', NULL, N'Naik', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'B7C7EEDE-01C6-46EE-96A0-30DACEF5DFD4', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1531, N'VC', 0, N'Mr.', N'Greg', N'F.', N'Mohamed', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '79F148E6-A1F6-4541-9654-6E217F8E4C9C', CONVERT(datetime2, '2012-01-29T00:00:00', 127)),
    (1533, N'VC', 0, N'Mr.', N'Mike', NULL, N'Nash', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BF0B6804-90D3-4FF4-B91E-9744E00DDCC5', CONVERT(datetime2, '2012-02-02T00:00:00', 127)),
    (1535, N'VC', 0, N'Ms.', N'Lorraine', NULL, N'Nay', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '218FB7AA-8157-44FE-8FE4-B9FC65F859F8', CONVERT(datetime2, '2012-01-17T00:00:00', 127)),
    (1537, N'VC', 0, N'Mr.', N'Alex', NULL, N'Nayberg', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'CB6F7B14-9C7E-4927-87ED-CE52E8E9C89C', CONVERT(datetime2, '2012-02-18T00:00:00', 127)),
    (1539, N'VC', 0, N'Ms.', N'Jan', N'R.', N'Nelsen', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '96F356C6-BC33-408B-BCEF-AF61F651675F', CONVERT(datetime2, '2011-12-23T00:00:00', 127)),
    (1541, N'VC', 0, N'Mr.', N'Amir', N'T.', N'Netz', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '02039E2B-5F88-452A-BB2D-B3C4446DE3C8', CONVERT(datetime2, '2012-01-25T00:00:00', 127)),
    (1543, N'VC', 0, N'Mr.', N'Donals', N'E.', N'Nilson', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'EA2053A1-F88E-4C3A-8200-90F21BE83970', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1545, N'VC', 0, N'Mr.', N'Toby', NULL, N'Nixon', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '2D560D09-EFBF-44A0-B2F8-42D2427384F9', CONVERT(datetime2, '2012-01-29T00:00:00', 127)),
    (1547, N'VC', 0, N'Ms.', N'Laura', NULL, N'Norman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '07C69FA9-3E22-4312-B47C-C24E91C41D0D', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1549, N'VC', 0, N'Mr.', N'Fred', NULL, N'Northup', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '9FCF7E19-24C1-4B71-A5F0-A6BB130ECDA2', CONVERT(datetime2, '2012-02-03T00:00:00', 127)),
    (1551, N'VC', 0, N'Mr.', N'Michael', N'J', N'O''Connell', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'BDF392A4-39D7-4A5E-A04B-0D5C3CF0127E', CONVERT(datetime2, '2012-01-25T00:00:00', 127)),
    (1553, N'VC', 0, N'Mr.', N'Tim', NULL, N'O''Brien', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'DC80D2B4-9188-4300-AF2C-90CF548AB1B2', CONVERT(datetime2, '2012-02-18T00:00:00', 127)),
    (1555, N'VC', 0, NULL, N'Tina', N'Slone', N'O''Dell', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8C3FDB6D-C448-4E1C-BDD6-C764D7A950E8', CONVERT(datetime2, '2012-02-18T00:00:00', 127)),
    (1557, N'VC', 0, N'Mr.', N'Robert', NULL, N'O''Hara', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F1836FE1-F554-4868-A09E-44D1DEBFCB7E', CONVERT(datetime2, '2012-02-03T00:00:00', 127)),
    (1559, N'VC', 0, N'Mr.', N'Nino', N'E.', N'Olivotto', NULL, 2, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '8EBC4868-9A31-4ADC-8161-A2BAAE1EADAE', CONVERT(datetime2, '2011-12-24T00:00:00', 127)),
    (1561, N'VC', 0, N'Mr.', N'Lee', N'M.', N'Olguin', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', 'F5301349-F133-4190-B6D5-5902CF08442F', CONVERT(datetime2, '2012-02-02T00:00:00', 127)),
    (1563, N'VC', 0, N'Mr.', N'Tad', NULL, N'Orman', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '63BD791B-4A5F-4EA4-888E-327536B4F5A3', CONVERT(datetime2, '2011-12-23T00:00:00', 127)),
    (1565, N'VC', 0, N'Ms.', N'Gloria', N'B.', N'Orona', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '6890365D-9084-43A5-B784-610EA5A50E44', CONVERT(datetime2, '2011-12-23T00:00:00', 127)),
    (1567, N'VC', 0, N'Mr.', N'Fred', N'A.', N'Ortiz', NULL, 0, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '72A644D6-789A-4E39-9BA0-7955E9E923C8', CONVERT(datetime2, '2012-01-17T00:00:00', 127)),
    (1569, N'VC', 0, N'Mr.', N'John', N'E.', N'Ortiz', NULL, 1, NULL, '<IndividualSurvey xmlns="http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/IndividualSurvey"><TotalPurchaseYTD>0</TotalPurchaseYTD></IndividualSurvey>', '488B7FD6-4B8C-4E95-B3C4-0F54C0F2427C', CONVERT(datetime2, '2012-01-25T00:00:00', 127));

SET DATEFORMAT ymd;
INSERT INTO [dbo].[BillOfMaterials] (
    [ProductAssemblyID], [ComponentID], [StartDate], [EndDate], [UnitMeasureCode], [BOMLevel], [PerAssemblyQty], [ModifiedDate]
) VALUES
    (893, 749, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (271, 750, CONVERT(datetime, '2010-03-04 00:00:00', 120), CONVERT(datetime, '2010-05-03 00:00:00', 120), N'EA', 0, 1.00, CONVERT(datetime, '2010-05-03 00:00:00', 120)),
    (34, 750, CONVERT(datetime, '2010-05-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-04-20 00:00:00', 120)),
    (830, 751, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2074, 752, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1950, 753, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 0, 1.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1761, 753, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (3088, 754, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3351, 755, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (3246, 756, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2760, 757, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2395, 758, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3087, 759, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3350, 760, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2822, 761, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (3245, 762, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2759, 763, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2394, 764, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3341, 765, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2815, 766, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2449, 767, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2899, 768, CONVERT(datetime, '2010-09-15 00:00:00', 120), CONVERT(datetime, '2010-11-14 00:00:00', 120), N'EA', 0, 1.00, CONVERT(datetime, '2010-11-14 00:00:00', 120)),
    (2738, 768, CONVERT(datetime, '2010-11-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-11-01 00:00:00', 120)),
    (2363, 769, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1265, 770, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1195, 771, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (3017, 772, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3281, 773, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2783, 774, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (5, 775, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2737, 776, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2362, 777, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1264, 778, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1642, 779, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (393, 780, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (52, 781, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3217, 782, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (808, 783, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2049, 784, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (4, 785, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2736, 786, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2361, 787, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1263, 788, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (3247, 789, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2761, 790, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2396, 791, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3089, 792, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (2739, 793, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2364, 794, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1266, 795, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (482, 796, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1024, 797, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 0, 1.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (783, 797, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (2333, 798, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1240, 799, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (450, 800, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (92, 801, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1303, 953, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1699, 954, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3060, 955, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3326, 956, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2107, 957, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2800, 958, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1778, 959, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3102, 960, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (2027, 961, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1698, 962, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3059, 963, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (879, 964, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1789, 965, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3037, 966, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3304, 967, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2801, 968, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1779, 969, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (419, 970, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (73, 971, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1071, 972, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 0, 1.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (867, 972, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (2993, 973, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (33, 974, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (829, 975, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2073, 976, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (103, 977, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1668, 978, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3036, 979, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1641, 980, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (392, 981, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (51, 982, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (850, 983, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1194, 984, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (3016, 985, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3280, 986, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2782, 987, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2422, 988, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3, 989, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2735, 990, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2360, 991, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1262, 992, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (481, 993, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (809, 997, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2050, 998, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1938, 999, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 0, 1.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1731, 999, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 0, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (2000, 2, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 3, 10.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1657, 461, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (389, 504, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 3, 2.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (805, 505, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 3, 2.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2433, 486, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 4, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1290, 486, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (875, 483, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (3337, 482, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2806, 482, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2432, 482, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1289, 482, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 4, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (492, 486, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 4, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (876, 487, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (3338, 485, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (543, 523, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2162, 525, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (935, 524, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2487, 526, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2327, 497, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3138, 528, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1830, 530, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3135, 908, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1219, 497, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (3400, 528, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (3140, 530, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (2858, 909, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (386, 497, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2862, 528, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (3402, 530, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1357, 910, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (28, 497, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2489, 528, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2864, 530, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (348, 911, CONVERT(datetime, '2010-03-04 00:00:00', 120), CONVERT(datetime, '2010-05-03 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-03 00:00:00', 120)),
    (140, 911, CONVERT(datetime, '2010-05-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-04-20 00:00:00', 120)),
    (1032, 497, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 5.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (803, 497, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (1360, 528, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2491, 530, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2161, 912, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2022, 497, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (546, 528, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1362, 530, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (3136, 913, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1660, 497, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (144, 528, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (548, 530, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2859, 914, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (3012, 497, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (938, 528, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (146, 530, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1358, 915, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (3239, 497, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2165, 528, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1124, 530, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (940, 530, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (141, 916, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1288, 477, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2094, 487, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 4, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1956, 484, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 2, 2.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1769, 484, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (100, 478, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (491, 482, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 3, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1445, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1881, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3453, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2541, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3190, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1617, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'OZ', 1, 9.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1446, 492, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (626, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (987, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (220, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (219, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (776, 324, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1999, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3482, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2709, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3215, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (777, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (778, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1190, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (779, 534, CONVERT(datetime, '2010-05-06 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-04-22 00:00:00', 120)),
    (360, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1453, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1887, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3459, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2547, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3196, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (771, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'OZ', 1, 9.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (636, 493, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (637, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (993, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (232, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (231, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1449, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1883, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3455, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2689, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2543, 327, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (3192, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (628, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (629, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (989, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (224, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (223, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1450, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1884, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3456, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2544, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3193, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (770, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'OZ', 1, 9.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (630, 493, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (631, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (990, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (226, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (225, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1451, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1885, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3457, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2545, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3194, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (632, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (633, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1167, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (991, 533, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (228, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (227, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1452, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1886, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3458, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2546, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3195, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (634, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (635, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (992, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (230, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (229, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1409, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1860, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3432, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2520, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3169, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1411, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (585, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1151, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (966, 533, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (195, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1410, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1412, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1861, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3433, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2521, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3170, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1414, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (586, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (967, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (196, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1413, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1602, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 3.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1415, 324, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (1862, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3434, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2522, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3171, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1417, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (587, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (968, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (197, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1416, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1418, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1863, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3435, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2675, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2523, 327, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (3172, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (588, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (589, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (969, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (198, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1419, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1420, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1864, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3436, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2524, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3173, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (766, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'OZ', 1, 9.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (590, 493, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (591, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (970, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (199, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1421, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1422, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1865, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3437, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2525, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3174, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (592, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (593, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1152, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (971, 533, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (200, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1423, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1603, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 3.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1424, 324, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (1866, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3438, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2526, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3175, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (594, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (595, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (972, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (201, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1425, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1426, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1867, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3439, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2676, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2527, 327, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (3176, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (596, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (597, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (973, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (202, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1427, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1428, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1868, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3440, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2528, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3177, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (767, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'OZ', 1, 9.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (598, 493, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (599, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (974, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (203, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1429, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1430, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1869, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3441, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2682, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2529, 327, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (3178, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (601, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (602, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (975, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (204, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (600, 803, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1431, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1870, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3442, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2530, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3179, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (768, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'OZ', 1, 9.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (604, 493, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (605, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (976, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (205, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (603, 803, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1432, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1871, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3443, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2531, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3180, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (607, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (608, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1160, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (977, 533, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (206, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (606, 803, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1611, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 3.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1433, 324, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (1872, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3444, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2532, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3181, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (610, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (611, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (978, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (207, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (609, 803, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1434, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1873, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3445, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2683, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2533, 327, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (3182, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (613, 493, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (614, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (979, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (208, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (612, 803, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1400, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1857, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3429, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2517, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3166, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1402, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (582, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (963, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (192, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1401, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1600, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 3.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1403, 324, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (1858, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3430, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2518, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3167, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1405, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (583, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (964, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (193, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1404, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1406, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1859, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3431, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2674, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2519, 327, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (3168, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1408, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (584, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (965, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (194, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1407, 802, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1396, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1853, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3425, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2513, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3162, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (181, 494, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (578, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1145, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (959, 533, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (182, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (180, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1397, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1854, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3426, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2514, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3163, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (184, 494, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (579, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (960, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (185, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (183, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1596, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 3.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1398, 324, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (1855, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3427, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2515, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3164, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (187, 494, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (580, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (961, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (188, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (186, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1399, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1856, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3428, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2670, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2516, 327, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (3165, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (190, 494, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (581, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (962, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (191, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (189, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1388, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1849, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3421, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2509, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3158, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1389, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (574, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (955, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (173, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (172, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1592, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 3.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1390, 324, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (1850, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3422, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2510, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3159, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1391, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (575, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (956, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (175, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (174, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1392, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1851, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3423, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2667, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2511, 327, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (3160, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1393, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (576, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (957, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (177, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (176, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1394, 324, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1852, 325, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3424, 326, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2512, 327, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3161, 399, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1593, 492, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'OZ', 1, 9.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1395, 492, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (577, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (958, 533, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (179, 534, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (178, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2305, 324, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2720, 325, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2307, 326, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1202, 327, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (3236, 399, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (3011, 492, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (2653, 532, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 5.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2493, 532, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (1364, 533, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1838, 534, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (18, 804, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (368, 324, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1200, 325, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 2, 2.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (371, 326, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (262, 327, CONVERT(datetime, '2010-03-04 00:00:00', 120), CONVERT(datetime, '2010-05-03 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-03 00:00:00', 120)),
    (11, 327, CONVERT(datetime, '2010-05-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-04-20 00:00:00', 120)),
    (2323, 399, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2203, 494, CONVERT(datetime, '2010-07-08 00:00:00', 120), CONVERT(datetime, '2010-09-06 00:00:00', 120), N'OZ', 1, 9.00, CONVERT(datetime, '2010-09-06 00:00:00', 120)),
    (2020, 494, CONVERT(datetime, '2010-09-07 00:00:00', 120), NULL, N'OZ', 2, 8.00, CONVERT(datetime, '2010-08-24 00:00:00', 120)),
    (550, 532, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 2, 4.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (148, 533, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3409, 534, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2012, 804, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 2, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2989, 519, CONVERT(datetime, '2010-09-15 00:00:00', 120), CONVERT(datetime, '2010-11-14 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-11-14 00:00:00', 120)),
    (2857, 519, CONVERT(datetime, '2010-11-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-11-01 00:00:00', 120)),
    (1793, 717, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3108, 807, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3363, 813, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (511, 820, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (752, 828, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (531, 828, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (2464, 894, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3110, 907, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3364, 940, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1319, 945, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (111, 948, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1317, 951, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2823, 952, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1316, 996, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2980, 519, CONVERT(datetime, '2010-09-15 00:00:00', 120), CONVERT(datetime, '2010-11-14 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-11-14 00:00:00', 120)),
    (2852, 519, CONVERT(datetime, '2010-11-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-11-01 00:00:00', 120)),
    (2036, 718, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (411, 807, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (44, 813, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3025, 820, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (746, 828, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (526, 828, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (2456, 894, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (416, 907, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (46, 940, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1674, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3256, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1671, 951, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1037, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (811, 952, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (1667, 996, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1116, 519, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (928, 519, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (1710, 719, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (441, 807, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (64, 813, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3047, 820, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3124, 828, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (2125, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (447, 907, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (68, 940, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1707, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3290, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1932, 951, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1703, 951, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (832, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1697, 996, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2983, 519, CONVERT(datetime, '2010-09-15 00:00:00', 120), CONVERT(datetime, '2010-11-14 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-11-14 00:00:00', 120)),
    (2855, 519, CONVERT(datetime, '2010-11-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-11-01 00:00:00', 120)),
    (3067, 720, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (472, 807, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (85, 813, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3069, 820, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (748, 828, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (530, 828, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (2461, 894, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (703, 907, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (478, 907, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (291, 940, CONVERT(datetime, '2010-03-04 00:00:00', 120), CONVERT(datetime, '2010-05-03 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-03 00:00:00', 120)),
    (89, 940, CONVERT(datetime, '2010-05-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-04-20 00:00:00', 120)),
    (1738, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3313, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1734, 951, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (852, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1730, 996, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1119, 519, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (931, 519, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (3334, 721, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (490, 807, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (97, 813, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3080, 820, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3126, 828, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (2129, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (3086, 907, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3339, 940, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1765, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3331, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1763, 951, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (868, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1760, 996, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (139, 518, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3347, 734, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1298, 806, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (496, 812, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1776, 819, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1818, 827, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2131, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (499, 907, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (497, 939, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1774, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3345, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2098, 950, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (881, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2238, 995, CONVERT(datetime, '2010-07-08 00:00:00', 120), CONVERT(datetime, '2010-09-06 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-09-06 00:00:00', 120)),
    (2096, 995, CONVERT(datetime, '2010-09-07 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-24 00:00:00', 120)),
    (3395, 518, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2818, 735, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1311, 806, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (502, 812, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1785, 819, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1348, 827, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2463, 894, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (507, 907, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (505, 939, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1784, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3354, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2109, 950, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (887, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2106, 995, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (130, 518, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2750, 731, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1685, 806, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3026, 812, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1505, 819, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1231, 819, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (1808, 827, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2122, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (3032, 907, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3029, 939, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1227, 945, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (39, 948, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2338, 950, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2741, 952, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2331, 995, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3386, 518, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2379, 732, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1719, 806, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3048, 812, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1252, 819, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1340, 827, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2458, 894, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3055, 907, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3052, 939, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1246, 945, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (279, 948, CONVERT(datetime, '2010-03-04 00:00:00', 120), CONVERT(datetime, '2010-05-03 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-03 00:00:00', 120)),
    (58, 948, CONVERT(datetime, '2010-05-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-04-20 00:00:00', 120)),
    (2369, 950, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2763, 952, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2587, 995, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2358, 995, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (135, 518, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1273, 733, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1945, 806, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1748, 806, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (3070, 812, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1275, 819, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1814, 827, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2127, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (3076, 907, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3073, 939, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1270, 945, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (78, 948, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2401, 950, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2785, 952, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2392, 995, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (540, 517, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2438, 728, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1297, 806, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1296, 811, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2243, 818, CONVERT(datetime, '2010-07-08 00:00:00', 120), CONVERT(datetime, '2010-09-06 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-09-06 00:00:00', 120)),
    (2102, 818, CONVERT(datetime, '2010-09-07 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-24 00:00:00', 120)),
    (2146, 826, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2130, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (498, 907, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1300, 938, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1773, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3344, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2097, 950, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1079, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (880, 952, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (2809, 994, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (3133, 517, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1307, 729, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1310, 806, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1309, 811, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2112, 818, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2475, 826, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2462, 894, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (506, 907, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1312, 938, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1783, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3353, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2108, 950, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (886, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2813, 994, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (542, 517, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (510, 730, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1322, 806, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1321, 811, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2118, 818, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2149, 826, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2133, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (513, 907, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1323, 938, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1792, 945, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3360, 948, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2116, 950, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (894, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2821, 994, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (534, 517, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1230, 725, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1684, 806, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1682, 811, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2351, 818, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2138, 826, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2121, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (3031, 907, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1689, 938, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1226, 945, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (273, 948, CONVERT(datetime, '2010-03-04 00:00:00', 120), CONVERT(datetime, '2010-05-03 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-03 00:00:00', 120)),
    (38, 948, CONVERT(datetime, '2010-05-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-04-20 00:00:00', 120)),
    (2337, 950, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2740, 952, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (806, 994, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (3132, 517, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (432, 726, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1718, 806, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1716, 811, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2382, 818, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2471, 826, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2457, 894, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3054, 907, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1723, 938, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1245, 945, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (57, 948, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2368, 950, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2762, 952, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1047, 994, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (826, 994, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (538, 517, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (82, 727, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1747, 806, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1745, 811, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2601, 818, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2413, 818, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (2142, 826, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2126, 894, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (3075, 907, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1752, 938, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1269, 945, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (77, 948, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2400, 950, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2784, 952, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (848, 994, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1572, 517, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1355, 517, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (2437, 722, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2440, 806, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2439, 811, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (884, 818, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1109, 826, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (923, 826, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (911, 894, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1301, 907, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2441, 938, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2100, 945, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (3092, 948, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (1080, 950, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (883, 950, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (104, 952, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3340, 994, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1827, 517, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (1306, 723, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2447, 806, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2446, 811, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (889, 818, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2850, 826, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2839, 894, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1314, 907, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2448, 938, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2111, 945, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (3098, 948, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (888, 950, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (108, 952, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3348, 994, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1573, 517, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1356, 517, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (740, 724, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (509, 724, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (2453, 806, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2452, 811, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (896, 818, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1111, 826, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (924, 826, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (912, 894, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1324, 907, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2454, 938, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2117, 945, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (3105, 948, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (895, 950, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (110, 952, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3359, 994, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1570, 517, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1350, 517, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (1229, 736, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (2041, 806, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2038, 811, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2751, 818, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1105, 826, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (917, 826, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (902, 894, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1693, 907, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2043, 938, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2579, 945, CONVERT(datetime, '2010-08-05 00:00:00', 120), CONVERT(datetime, '2010-10-04 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-10-04 00:00:00', 120)),
    (2344, 945, CONVERT(datetime, '2010-10-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-21 00:00:00', 120)),
    (400, 948, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2744, 950, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (3251, 952, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (32, 994, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1822, 517, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (689, 737, CONVERT(datetime, '2010-03-18 00:00:00', 120), CONVERT(datetime, '2010-05-17 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-05-17 00:00:00', 120)),
    (431, 737, CONVERT(datetime, '2010-05-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-04 00:00:00', 120)),
    (2065, 806, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2062, 811, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2772, 818, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2845, 826, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2833, 894, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1728, 907, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2067, 938, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2376, 945, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (427, 948, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2907, 950, CONVERT(datetime, '2010-09-15 00:00:00', 120), CONVERT(datetime, '2010-11-14 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-11-14 00:00:00', 120)),
    (2766, 950, CONVERT(datetime, '2010-11-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-11-01 00:00:00', 120)),
    (3285, 952, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (49, 994, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1571, 517, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1353, 517, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (81, 738, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (2083, 806, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2080, 811, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2793, 818, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1107, 826, CONVERT(datetime, '2010-05-26 00:00:00', 120), CONVERT(datetime, '2010-07-25 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-07-25 00:00:00', 120)),
    (921, 826, CONVERT(datetime, '2010-07-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-12 00:00:00', 120)),
    (909, 894, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1947, 907, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1757, 907, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (2223, 938, CONVERT(datetime, '2010-07-08 00:00:00', 120), CONVERT(datetime, '2010-09-06 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-09-06 00:00:00', 120)),
    (2085, 938, CONVERT(datetime, '2010-09-07 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-24 00:00:00', 120)),
    (2408, 945, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (458, 948, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2788, 950, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (3308, 952, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (71, 994, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (3128, 516, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3229, 748, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (21, 807, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (1495, 810, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1212, 810, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (378, 817, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2468, 825, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1550, 894, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1326, 894, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (3242, 907, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (804, 937, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (3001, 945, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (2008, 948, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (2004, 951, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (786, 952, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1910, 996, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1639, 996, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (533, 516, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (1676, 739, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3273, 807, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1681, 810, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2350, 817, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2137, 825, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1799, 894, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3277, 907, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1688, 937, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (403, 945, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (814, 948, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (396, 951, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2335, 952, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3015, 996, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3131, 516, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3045, 740, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (3297, 807, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1936, 810, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1715, 810, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (2381, 817, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2470, 825, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1553, 894, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1330, 894, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (3301, 907, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1722, 937, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (430, 945, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (836, 948, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (423, 951, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2366, 952, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3035, 996, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (537, 516, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (3315, 741, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (3321, 807, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1744, 810, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (2412, 817, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2141, 825, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1803, 894, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (3324, 907, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (1751, 937, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (461, 945, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (855, 948, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (454, 951, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2398, 952, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3058, 996, CONVERT(datetime, '2010-12-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-01 00:00:00', 120)),
    (2151, 516, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (795, 747, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1496, 807, CONVERT(datetime, '2010-06-09 00:00:00', 120), CONVERT(datetime, '2010-08-08 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-08 00:00:00', 120)),
    (1214, 807, CONVERT(datetime, '2010-08-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-26 00:00:00', 120)),
    (2729, 810, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (2316, 817, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3377, 825, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2827, 894, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1664, 907, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (388, 937, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2011, 945, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (15, 948, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (8, 951, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (365, 952, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (782, 996, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2481, 516, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (2347, 743, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (1687, 807, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (821, 810, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (3270, 817, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (124, 825, CONVERT(datetime, '2010-03-04 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-02-18 00:00:00', 120)),
    (901, 894, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (1692, 907, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (825, 937, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2343, 945, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (399, 948, CONVERT(datetime, '2010-03-18 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-03-04 00:00:00', 120)),
    (2340, 951, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120)),
    (3250, 952, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2206, 996, CONVERT(datetime, '2010-07-08 00:00:00', 120), CONVERT(datetime, '2010-09-06 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-09-06 00:00:00', 120)),
    (2026, 996, CONVERT(datetime, '2010-09-07 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-24 00:00:00', 120)),
    (2155, 516, CONVERT(datetime, '2010-07-08 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-24 00:00:00', 120)),
    (1249, 744, CONVERT(datetime, '2010-06-09 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-26 00:00:00', 120)),
    (1937, 807, CONVERT(datetime, '2010-06-19 00:00:00', 120), CONVERT(datetime, '2010-08-18 00:00:00', 120), N'EA', 1, 2.00, CONVERT(datetime, '2010-08-18 00:00:00', 120)),
    (1721, 807, CONVERT(datetime, '2010-08-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-08-05 00:00:00', 120)),
    (842, 810, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (3294, 817, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (3380, 825, CONVERT(datetime, '2010-12-23 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-12-09 00:00:00', 120)),
    (2832, 894, CONVERT(datetime, '2010-09-15 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-09-01 00:00:00', 120)),
    (1727, 907, CONVERT(datetime, '2010-06-19 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-06-05 00:00:00', 120)),
    (846, 937, CONVERT(datetime, '2010-05-26 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-05-12 00:00:00', 120)),
    (2375, 945, CONVERT(datetime, '2010-08-05 00:00:00', 120), NULL, N'EA', 1, 1.00, CONVERT(datetime, '2010-07-22 00:00:00', 120));

SET IDENTITY_INSERT [dbo].[Product] ON;
SET DATEFORMAT ymd;
INSERT INTO [dbo].[Product] ([ProductID], [Name], [ProductNumber], [MakeFlag], [FinishedGoodsFlag], [Color], [SafetyStockLevel], [ReorderPoint], [StandardCost], [ListPrice], [Size], [SizeUnitMeasureCode], [WeightUnitMeasureCode], [Weight], [DaysToManufacture], [ProductLine], [Class], [Style], [ProductSubcategoryID], [ProductModelID], [SellStartDate], [SellEndDate], [DiscontinuedDate], [rowguid], [ModifiedDate])
VALUES
(1, N'Adjustable Race', N'AR-5381', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '694215b7-08f7-4c0d-acb1-d734ba44c0c8', GETDATE()),
(2, N'Bearing Ball', N'BA-8327', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '58ae3c20-4f3a-4749-a7d4-d568806cc537', GETDATE()),
(3, N'BB Ball Bearing', N'BE-2349', N'1', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9c21aed2-5bfa-4f18-bcb8-f11638dc2e4e', GETDATE()),
(4, N'Headset Ball Bearings', N'BE-2908', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ecfed6cb-51ff-49b5-b06c-7d8ac834db8b', GETDATE()),
(316, N'Blade', N'BL-2036', N'1', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'e73e9750-603b-4131-89f5-3dd15ed5ff80', GETDATE()),
(317, N'LL Crankarm', N'CA-5965', N'0', N'0', N'Black', 500, 375, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '3c9d10b7-a6b2-4774-9963-c19dcee72fea', GETDATE()),
(318, N'ML Crankarm', N'CA-6738', N'0', N'0', N'Black', 500, 375, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, N'M', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'eabb9a92-fa07-4eab-8955-f0517b4a4ca7', GETDATE()),
(319, N'HL Crankarm', N'CA-7457', N'0', N'0', N'Black', 500, 375, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7d3fd384-4f29-484b-86fa-4206e276fe58', GETDATE()),
(320, N'Chainring Bolts', N'CB-2903', N'0', N'0', N'Silver', 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7be38e48-b7d6-4486-888e-f53c26735101', GETDATE()),
(321, N'Chainring Nut', N'CN-6137', N'0', N'0', N'Silver', 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '3314b1d7-ef69-4431-b6dd-dc75268bd5df', GETDATE()),
(322, N'Chainring', N'CR-7833', N'0', N'0', N'Black', 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'f0ac2c4d-1a1f-4e3c-b4d9-68aea0ec1ce4', GETDATE()),
(323, N'Crown Race', N'CR-9981', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '51a32ca6-65a1-4c31-af2b-d9e4f5d631d4', GETDATE()),
(324, N'Chain Stays', N'CS-2812', N'1', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'fe0678ed-aef2-4c58-a450-8151cc24ddd8', GETDATE()),
(325, N'Decal 1', N'DC-8732', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '05ce123c-a402-478e-ae9b-75d7727aeaad', GETDATE()),
(326, N'Decal 2', N'DC-9824', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a56851f9-1cd7-4e2f-8779-2e773e1b5209', GETDATE()),
(327, N'Down Tube', N'DT-2377', N'1', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1dad47dd-e259-42b8-b8b4-15a0b7d21b2f', GETDATE()),
(328, N'Mountain End Caps', N'EC-M092', N'1', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '6070b1ea-59b7-4f8b-950f-2be07d00449d', GETDATE()),
(329, N'Road End Caps', N'EC-R098', N'1', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '88399d13-719e-4545-81d6-f0650f372fa2', GETDATE()),
(330, N'Touring End Caps', N'EC-T209', N'1', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '6903ce24-d0ce-4191-9198-4231de37a929', GETDATE()),
(331, N'Fork End', N'FE-3760', N'1', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'c91d602e-da52-43d2-bd7e-eb110a9392b9', GETDATE()),
(332, N'Freewheel', N'FH-2981', N'0', N'0', N'Silver', 500, 375, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'd864879a-e8b1-4f7b-bafa-1f136089c2c8', GETDATE()),
(341, N'Flat Washer 1', N'FW-1000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a3f2fa3a-22e1-43d8-a131-a9b89c32d8ea', GETDATE()),
(342, N'Flat Washer 6', N'FW-1200', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '331addec-e9b9-4a7e-9324-42069c2dcdc4', GETDATE()),
(343, N'Flat Washer 2', N'FW-1400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '84a3473e-ae26-4a21-81b9-60bb418a79b2', GETDATE()),
(344, N'Flat Washer 9', N'FW-3400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '0ae4ce60-5242-48f5-ada1-3013ff45f969', GETDATE()),
(345, N'Flat Washer 4', N'FW-3800', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '2c1c58b4-234c-4b3a-8c8e-84524ac05eea', GETDATE()),
(346, N'Flat Washer 3', N'FW-5160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '590c2c3f-a8b6-42b5-9412-d655e37f0eae', GETDATE()),
(347, N'Flat Washer 8', N'FW-5800', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1b73f5fe-ab85-49fc-99ad-0500cebda91d', GETDATE()),
(348, N'Flat Washer 5', N'FW-7160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'd182cf18-4ddf-429b-a0df-de1cfc92622d', GETDATE()),
(349, N'Flat Washer 7', N'FW-9160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7e55f64d-ea3c-45ff-be72-f7f7b9d61a79', GETDATE()),
(350, N'Fork Crown', N'FC-3654', N'1', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1cbfa85b-5c9b-4b58-9c17-95238215d926', GETDATE()),
(351, N'Front Derailleur Cage', N'FC-3982', N'0', N'0', N'Silver', 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '01c901e3-4323-48ed-ab9e-9bfda28bdef6', GETDATE()),
(352, N'Front Derailleur Linkage', N'FL-2301', N'0', N'0', N'Silver', 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '88ed2e08-e775-4915-b506-831600b773fd', GETDATE()),
(355, N'Guide Pulley', N'GP-0982', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '6a89205b-90c3-4997-8c63-bc6a5ebc752d', GETDATE()),
(356, N'LL Grip Tape', N'GT-0820', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '32c82181-1969-4660-ae04-a02d51994ec1', GETDATE()),
(357, N'ML Grip Tape', N'GT-1209', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, N'M', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '09343e22-2494-4279-9f32-5d961a0fbfea', GETDATE()),
(358, N'HL Grip Tape', N'GT-2908', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '8e5b2bf7-81dd-4454-b75e-d9ae2a6fbd26', GETDATE()),
(359, N'Thin-Jam Hex Nut 9', N'HJ-1213', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a63aff3c-4143-4016-bc99-d3f80ec1ade5', GETDATE()),
(360, N'Thin-Jam Hex Nut 10', N'HJ-1220', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a1ae0c6d-92a5-4fda-b33b-1301e83efe5b', GETDATE()),
(361, N'Thin-Jam Hex Nut 1', N'HJ-1420', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'e33e8c4c-282a-4d1f-91e7-e9969cf7134f', GETDATE()),
(362, N'Thin-Jam Hex Nut 2', N'HJ-1428', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a992684f-4642-4856-a817-2c0740ae8c55', GETDATE()),
(363, N'Thin-Jam Hex Nut 15', N'HJ-3410', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'b9d9a30d-cb07-422d-a312-f0535575337e', GETDATE()),
(364, N'Thin-Jam Hex Nut 16', N'HJ-3416', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '0ec8f653-24c9-41b6-86f5-39c1789db580', GETDATE()),
(365, N'Thin-Jam Hex Nut 5', N'HJ-3816', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '94506c9d-5991-46a7-82ea-00e05d8d9b9c', GETDATE()),
(366, N'Thin-Jam Hex Nut 6', N'HJ-3824', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '71e984c6-1d11-4cf2-baee-6c8df494bdad', GETDATE()),
(367, N'Thin-Jam Hex Nut 3', N'HJ-5161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'efc09cdb-ecd5-4db5-9e27-277dda6d7f50', GETDATE()),
(368, N'Thin-Jam Hex Nut 4', N'HJ-5162', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '0a0c93aa-d06c-48aa-99eb-20f2c8a6d6c4', GETDATE()),
(369, N'Thin-Jam Hex Nut 13', N'HJ-5811', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a2728648-9517-4dec-8606-d7d98ecd1e91', GETDATE()),
(370, N'Thin-Jam Hex Nut 14', N'HJ-5818', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '0a7ad37c-3696-4844-8633-9fddcd5fcefc', GETDATE()),
(371, N'Thin-Jam Hex Nut 7', N'HJ-7161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'acbb1de1-680c-4034-a8c5-3c6b01e57aa7', GETDATE()),
(372, N'Thin-Jam Hex Nut 8', N'HJ-7162', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a0da8f8f-45fb-4e62-ab14-aa229087de1e', GETDATE()),
(373, N'Thin-Jam Hex Nut 12', N'HJ-9080', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'db99cbcd-f18d-4979-8dcf-1012f3b1cb15', GETDATE()),
(374, N'Thin-Jam Hex Nut 11', N'HJ-9161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ec2e54f5-9d07-4c26-b969-40f835395be3', GETDATE()),
(375, N'Hex Nut 5', N'HN-1024', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'f2f3a14c-df15-4957-966d-55e5fcad1161', GETDATE()),
(376, N'Hex Nut 6', N'HN-1032', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'e73e44dd-f0b7-45d4-9066-e49f1b1fe7d0', GETDATE()),
(377, N'Hex Nut 16', N'HN-1213', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '2b2a5641-bffe-4079-b9f0-8bf355bc3996', GETDATE()),
(378, N'Hex Nut 17', N'HN-1220', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'f70bbecd-5be7-4ee9-a9e7-15786e622ba9', GETDATE()),
(379, N'Hex Nut 7', N'HN-1224', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ba3631d1-33d6-4a2f-8413-758bfda6f9c2', GETDATE()),
(380, N'Hex Nut 8', N'HN-1420', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'b478b33a-1fd5-4db6-b99a-eb3b2a7c1623', GETDATE()),
(381, N'Hex Nut 9', N'HN-1428', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'da46d979-59df-456d-b5ae-e7e89fc589df', GETDATE()),
(382, N'Hex Nut 22', N'HN-3410', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '2f457404-197d-4ddf-9868-a3aad1b32d6b', GETDATE()),
(383, N'Hex Nut 23', N'HN-3416', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '5f02449e-96e5-4fc8-ade0-8a9a7f533624', GETDATE()),
(384, N'Hex Nut 12', N'HN-3816', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '32c97696-7c3d-4793-a54b-3d73200badbc', GETDATE()),
(385, N'Hex Nut 13', N'HN-3824', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '8f0902d0-274d-4a4b-8fde-e37f53b2ab29', GETDATE()),
(386, N'Hex Nut 1', N'HN-4402', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '57456f8c-cb78-45ec-b9b8-21a9be5c12f5', GETDATE()),
(387, N'Hex Nut 10', N'HN-5161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '955811b4-2f17-48f0-a8b4-0c96cba4aa6d', GETDATE()),
(388, N'Hex Nut 11', N'HN-5162', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7d4bad17-374f-4281-9ae5-49abc3fe585d', GETDATE()),
(389, N'Hex Nut 2', N'HN-5400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '17c4b7ba-8574-4ec7-bd3b-7a51aba61f69', GETDATE()),
(390, N'Hex Nut 20', N'HN-5811', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'b4e990b7-b3f7-4f97-8f98-ce373833adb4', GETDATE()),
(391, N'Hex Nut 21', N'HN-5818', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '433ee702-e028-4630-895c-8cdbd0f1fd89', GETDATE()),
(392, N'Hex Nut 3', N'HN-6320', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ee76fae0-161e-409c-a6f5-837b5b8b344d', GETDATE()),
(393, N'Hex Nut 14', N'HN-7161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'eb10b88c-5351-4c06-af51-116baa48a2c8', GETDATE()),
(394, N'Hex Nut 15', N'HN-7162', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '35e28755-e8f0-47be-a8be-a20836dbe28d', GETDATE()),
(395, N'Hex Nut 4', N'HN-8320', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '69ab8d5e-6101-4203-81b1-794e923782ea', GETDATE()),
(396, N'Hex Nut 18', N'HN-9161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '39d42384-66f6-4ccd-b471-0589fc3fc576', GETDATE()),
(397, N'Hex Nut 19', N'HN-9168', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'b63f827e-9055-4678-9e90-4ffd8d06d4d4', GETDATE()),
(398, N'Handlebar Tube', N'HT-2981', N'1', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9f88c58e-5cfa-46c9-8994-da4f3ffe92ed', GETDATE()),
(399, N'Head Tube', N'HT-8019', N'1', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'b047c048-b4fb-4f80-94bc-c5fc00a6f77f', GETDATE()),
(400, N'LL Hub', N'HU-6280', N'1', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ab332dda-dda5-44ad-8c50-34ffaceffa8e', GETDATE()),
(401, N'HL Hub', N'HU-8998', N'1', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'd59403a3-d19e-4803-bda2-b436a6416fda', GETDATE()),
(402, N'Keyed Washer', N'KW-4091', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '43024784-2741-4cab-a6dc-8050ce507f2e', GETDATE()),
(403, N'External Lock Washer 3', N'LE-1000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '763412f0-6d53-43e2-b371-dcbed69f5e98', GETDATE()),
(404, N'External Lock Washer 4', N'LE-1200', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '89b6e84f-5c08-4cd9-b803-01f2ce24e417', GETDATE()),
(405, N'External Lock Washer 9', N'LE-1201', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '3330a721-e8cb-4e67-8d27-300db68c2395', GETDATE()),
(406, N'External Lock Washer 5', N'LE-1400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '39314098-768b-49f9-a777-af2e3bb06b17', GETDATE()),
(407, N'External Lock Washer 7', N'LE-3800', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '93f7bc73-bd22-45a0-9f2e-a11932342e6b', GETDATE()),
(408, N'External Lock Washer 6', N'LE-5160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'dc5f4cb0-a599-42cd-a96f-e9f01bc1dacc', GETDATE()),
(409, N'External Lock Washer 1', N'LE-6000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '802b7687-bc74-43f8-98ae-2112166faca7', GETDATE()),
(410, N'External Lock Washer 8', N'LE-7160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '02c48826-21ad-41f3-85a6-bc9a85a4dce4', GETDATE()),
(411, N'External Lock Washer 2', N'LE-8000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '502a2a3d-cd72-43ad-8fb6-77505187edf4', GETDATE()),
(412, N'Internal Lock Washer 3', N'LI-1000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'f1168c45-e4d2-4c37-adad-b76eaf402b71', GETDATE()),
(413, N'Internal Lock Washer 4', N'LI-1200', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7f7413bb-bad2-47e4-9bc4-d98b194be35d', GETDATE()),
(414, N'Internal Lock Washer 9', N'LI-1201', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '4f040109-8332-4fcf-8084-57e6d8c49283', GETDATE()),
(415, N'Internal Lock Washer 5', N'LI-1400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '0c02f2cc-bdb4-4794-a4f9-0eb33f7545c2', GETDATE()),
(416, N'Internal Lock Washer 7', N'LI-3800', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '3c2ac5bc-3f49-4fa4-8340-bc4cda983f46', GETDATE()),
(417, N'Internal Lock Washer 6', N'LI-5160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7f175dfe-1669-4ee9-8eeb-7b55fce9961c', GETDATE()),
(418, N'Internal Lock Washer 10', N'LI-5800', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'c8323eec-bdb2-4933-b3c6-24287638ad56', GETDATE()),
(419, N'Internal Lock Washer 1', N'LI-6000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '71f8232d-2b59-41ac-99a1-f5ea197671b5', GETDATE()),
(420, N'Internal Lock Washer 8', N'LI-7160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'e2f03586-02e8-4cd9-a342-1a8d65d393bd', GETDATE()),
(421, N'Internal Lock Washer 2', N'LI-8000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '97741a88-92a1-4e72-b0aa-bcb198a1c378', GETDATE()),
(422, N'Thin-Jam Lock Nut 9', N'LJ-1213', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7da2613b-3347-4072-a1dc-943ada161d7f', GETDATE()),
(423, N'Thin-Jam Lock Nut 10', N'LJ-1220', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a88f15be-2719-4741-93a4-2d96ef3712eb', GETDATE()),
(424, N'Thin-Jam Lock Nut 1', N'LJ-1420', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '48461e5d-d58a-47e5-8ba3-ce4430fca611', GETDATE()),
(425, N'Thin-Jam Lock Nut 2', N'LJ-1428', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '36187eb6-ad84-47b7-a55e-2941d3435fe2', GETDATE()),
(426, N'Thin-Jam Lock Nut 15', N'LJ-3410', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '99215648-afe8-4556-bc80-b6c62ae74fc8', GETDATE()),
(427, N'Thin-Jam Lock Nut 16', N'LJ-3416', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'b4fc4c32-049c-417f-bbb0-f19cdfd64252', GETDATE()),
(428, N'Thin-Jam Lock Nut 5', N'LJ-3816', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a57b7915-2e65-49de-87ba-acd007c55adb', GETDATE()),
(429, N'Thin-Jam Lock Nut 6', N'LJ-3824', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '5abd940c-d61f-4108-8312-0ea97e469613', GETDATE()),
(430, N'Thin-Jam Lock Nut 3', N'LJ-5161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9883496f-4785-4bfc-8af3-c357347b9f89', GETDATE()),
(431, N'Thin-Jam Lock Nut 4', N'LJ-5162', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '8c5ee683-d93c-4f25-9454-22faa4c31365', GETDATE()),
(432, N'Thin-Jam Lock Nut 13', N'LJ-5811', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '38e4a447-3d3c-4087-abad-97f006525b52', GETDATE()),
(433, N'Thin-Jam Lock Nut 14', N'LJ-5818', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'dce24b6c-76d8-4934-a4f6-c934364943ea', GETDATE()),
(434, N'Thin-Jam Lock Nut 7', N'LJ-7161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '344ad07c-cca5-4374-a3f3-8a7e0a1d9916', GETDATE()),
(435, N'Thin-Jam Lock Nut 8', N'LJ-7162', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'b2508cf2-c64f-493d-9db4-0d6601af1f73', GETDATE()),
(436, N'Thin-Jam Lock Nut 12', N'LJ-9080', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '5d3e589f-4584-406b-b9cc-3c8c060cb9a5', GETDATE()),
(437, N'Thin-Jam Lock Nut 11', N'LJ-9161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '64169a28-161c-4737-b724-f42ffd99de80', GETDATE()),
(438, N'Lock Nut 5', N'LN-1024', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'eb4c1d34-4816-4130-bb30-07b4de4072b6', GETDATE()),
(439, N'Lock Nut 6', N'LN-1032', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '98ccbb38-4683-486e-bbfe-cbbe4ea63c03', GETDATE()),
(440, N'Lock Nut 16', N'LN-1213', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'bbfd88f8-28c5-44ee-b625-52e882393dfc', GETDATE()),
(441, N'Lock Nut 17', N'LN-1220', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '92dc4ba8-a052-45df-83ec-226f050f876b', GETDATE()),
(442, N'Lock Nut 7', N'LN-1224', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'd583d825-c707-4529-b6f2-abffa21b81ec', GETDATE()),
(443, N'Lock Nut 8', N'LN-1420', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'e91c3dc2-c99b-4d01-8108-5dde3c87830a', GETDATE()),
(444, N'Lock Nut 9', N'LN-1428', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '06534a20-4c00-4824-8bba-b4e3a5724d93', GETDATE()),
(445, N'Lock Nut 22', N'LN-3410', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1e4fa4ec-367e-4d8d-88b4-6cd34d1cfb89', GETDATE()),
(446, N'Lock Nut 23', N'LN-3416', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'afa814c8-8ec8-49db-9fee-a291197a8fe9', GETDATE()),
(447, N'Lock Nut 12', N'LN-3816', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9a751f85-7130-4562-9f22-db9cab6e5bbc', GETDATE()),
(448, N'Lock Nut 13', N'LN-3824', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '06be8347-45c1-4c40-afcb-6ab34ad135fb', GETDATE()),
(449, N'Lock Nut 1', N'LN-4400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1dc29704-e0e0-4ef5-b581-4a524730c448', GETDATE()),
(450, N'Lock Nut 10', N'LN-5161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '612c26c7-6018-4050-b628-8b2d2e6841fa', GETDATE()),
(451, N'Lock Nut 11', N'LN-5162', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '5bcc4558-6c16-48f1-92f0-fd2eefb17306', GETDATE()),
(452, N'Lock Nut 2', N'LN-5400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '53ad147d-c16d-4a8c-b086-625a31405574', GETDATE()),
(453, N'Lock Nut 20', N'LN-5811', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '2749030e-49b7-4b24-8d47-fbcf194aba38', GETDATE()),
(454, N'Lock Nut 21', N'LN-5818', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'e10a7b34-87f5-42cd-88b3-27a3d8e16b18', GETDATE()),
(455, N'Lock Nut 3', N'LN-6320', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'aa5071eb-2145-4d08-9d33-b9d2ba9e1493', GETDATE()),
(456, N'Lock Nut 14', N'LN-7161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1e96b03d-dc07-4a98-bc24-bf5b24c393e5', GETDATE()),
(457, N'Lock Nut 15', N'LN-7162', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ce04de2b-4eca-4203-a108-b7d92ff0e96e', GETDATE()),
(458, N'Lock Nut 4', N'LN-8320', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '41bd9389-8b22-4a35-9a2c-233d39ada7ea', GETDATE()),
(459, N'Lock Nut 19', N'LN-9080', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '5986504b-22a0-4e74-a137-c7cf99a8216f', GETDATE()),
(460, N'Lock Nut 18', N'LN-9161', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a420963f-92fd-4cd4-8531-6926e4162c41', GETDATE()),
(461, N'Lock Ring', N'LR-2398', N'0', N'0', N'Silver', 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'aeca59da-b61c-4976-8316-97e14cd4eff1', GETDATE()),
(462, N'Lower Head Race', N'LR-8520', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'dbb962bf-0603-4095-959b-5ba9c74fbd32', GETDATE()),
(463, N'Lock Washer 4', N'LW-1000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a3ee3bc5-73c5-45f3-a952-9991d038dd9c', GETDATE()),
(464, N'Lock Washer 5', N'LW-1200', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ecaed08d-2cf5-4d81-a4ed-8369e25af227', GETDATE()),
(465, N'Lock Washer 10', N'LW-1201', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a2212bab-af58-41a5-a659-a6141c8967ca', GETDATE()),
(466, N'Lock Washer 6', N'LW-1400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9092f2e1-3637-4669-8565-55404a55750e', GETDATE()),
(467, N'Lock Washer 13', N'LW-3400', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '3cb31f4a-c61c-408c-be1e-48bee412e511', GETDATE()),
(468, N'Lock Washer 8', N'LW-3800', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '900d26e6-21a0-427d-b43c-173f6dcb2045', GETDATE()),
(469, N'Lock Washer 1', N'LW-4000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '5402ea37-29df-47ff-9fc7-867d60594c48', GETDATE()),
(470, N'Lock Washer 7', N'LW-5160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '99357255-e66b-458c-ab2e-6f68ef5452d2', GETDATE()),
(471, N'Lock Washer 12', N'LW-5800', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7bc9d58e-3e62-481f-8343-beb0883b3ecf', GETDATE()),
(472, N'Lock Washer 2', N'LW-6000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '5f201424-9e6a-4f8d-9c2c-30777e27d64f', GETDATE()),
(473, N'Lock Washer 9', N'LW-7160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'f9426fb2-1e68-464e-bf32-615026e0249e', GETDATE()),
(474, N'Lock Washer 3', N'LW-8000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ac007b7f-73b7-4623-8150-02444c5ec023', GETDATE()),
(475, N'Lock Washer 11', N'LW-9160', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '639d8448-b427-47b1-9e5b-0e5090a27632', GETDATE()),
(476, N'Metal Angle', N'MA-7075', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'e876e239-7ec2-45c8-ba4b-b9ceacb379a6', GETDATE()),
(477, N'Metal Bar 1', N'MB-2024', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '8b5429ce-7876-44b3-9332-baf78a238b36', GETDATE()),
(478, N'Metal Bar 2', N'MB-6061', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '2a14f60e-3827-49ba-af13-466dbc30c5ba', GETDATE()),
(479, N'Metal Plate 2', N'MP-2066', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '0773a2c9-f47f-429e-814a-25b2e08c128a', GETDATE()),
(480, N'Metal Plate 1', N'MP-2503', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '242389be-dde0-42a1-85d9-f99fdc981336', GETDATE()),
(481, N'Metal Plate 3', N'MP-4960', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '8b7e90e5-7785-455e-bc7c-e962f18c6848', GETDATE()),
(482, N'Metal Sheet 2', N'MS-0253', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '8bb96dfb-23aa-4877-9c5e-866bb18facc7', GETDATE()),
(483, N'Metal Sheet 3', N'MS-1256', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9074e00d-005b-450e-9c92-6667782e8108', GETDATE()),
(484, N'Metal Sheet 7', N'MS-1981', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'a9539885-0cee-4aa0-9072-8db1d34a16db', GETDATE()),
(485, N'Metal Sheet 4', N'MS-2259', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '3cb3cf7d-ab8e-44a3-b7e9-73149f5ec29f', GETDATE()),
(486, N'Metal Sheet 5', N'MS-2341', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '2a2c555d-328d-4299-bd83-591d0762df62', GETDATE()),
(487, N'Metal Sheet 6', N'MS-2348', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '64844011-a1c3-4f8f-9caa-9c8d214ecc12', GETDATE()),
(488, N'Metal Sheet 1', N'MS-6061', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '3b2febc6-c76c-4a56-9cf7-8af5b76e24ee', GETDATE()),
(489, N'Metal Tread Plate', N'MT-1000', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'd2177b6c-3352-43f0-9a41-719754dd280c', GETDATE()),
(490, N'LL Nipple', N'NI-4127', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '88310f73-ab0a-41a2-8597-936f192b7d12', GETDATE()),
(491, N'HL Nipple', N'NI-9522', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '88a7b897-6ff5-4ca2-b68a-6ea0e86f92b9', GETDATE()),
(492, N'Paint - Black', N'PA-187B', N'0', N'0', NULL, 60, 45, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'df20e514-3d47-491b-9454-0911ec3f7d29', GETDATE()),
(493, N'Paint - Red', N'PA-361R', N'0', N'0', NULL, 60, 45, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '4c568357-5d21-4ad4-bb85-bb5519b3b50c', GETDATE()),
(494, N'Paint - Silver', N'PA-529S', N'0', N'0', NULL, 60, 45, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'fa81e47d-7333-49c2-809b-308171ca2fb1', GETDATE()),
(495, N'Paint - Blue', N'PA-632U', N'0', N'0', NULL, 60, 45, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '25a73761-ae90-49d3-8d1d-dd7858db4704', GETDATE()),
(496, N'Paint - Yellow', N'PA-823Y', N'0', N'0', NULL, 60, 45, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1c8adb43-9fe8-44a6-b949-8af33ce9486e', GETDATE()),
(497, N'Pinch Bolt', N'PB-6109', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'f1694c24-dfab-4c92-bc66-6e717db24ea8', GETDATE()),
(504, N'Cup-Shaped Race', N'RA-2345', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '874c800e-334d-4a3c-8d3a-1e872d5b2a1b', GETDATE()),
(505, N'Cone-Shaped Race', N'RA-7490', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '35ce3995-9dd2-40e2-98b8-275931ac2d76', GETDATE()),
(506, N'Reflector', N'RF-9198', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1c850499-38ed-4c2d-8665-7edb6a7ce93d', GETDATE()),
(507, N'LL Mountain Rim', N'RM-M464', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, N'G', 435.00, 0, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'b2cc7dfb-783d-4587-88c0-2712a538a5b2', GETDATE()),
(508, N'ML Mountain Rim', N'RM-M692', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, N'G', 450.00, 0, NULL, N'M', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '733fd04d-322f-44f5-beec-f326189d1ce6', GETDATE()),
(509, N'HL Mountain Rim', N'RM-M823', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, N'G', 400.00, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9fa4a3b5-d396-48d4-adfc-b573bc4a800a', GETDATE()),
(510, N'LL Road Rim', N'RM-R436', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, N'G', 445.00, 0, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'c2770757-b258-4eec-a811-6856faf87437', GETDATE()),
(511, N'ML Road Rim', N'RM-R600', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, N'G', 450.00, 0, NULL, N'M', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '80108059-0002-4253-a805-53a2324c33a4', GETDATE()),
(512, N'HL Road Rim', N'RM-R800', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, N'G', 400.00, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'cd9b5c44-fb31-4e0f-9905-3b2086966cc5', GETDATE()),
(513, N'Touring Rim', N'RM-T801', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, N'G', 460.00, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '4852db13-308a-4893-aafa-390a0dfe9f12', GETDATE()),
(514, N'LL Mountain Seat Assembly', N'SA-M198', N'1', N'0', NULL, 500, 375, 98.77, 133.34, NULL, NULL, NULL, NULL, 1, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'fcfc0a4f-4563-4e0b-bff4-5ddcfe3a9273', GETDATE()),
(515, N'ML Mountain Seat Assembly', N'SA-M237', N'1', N'0', NULL, 500, 375, 108.99, 147.14, NULL, NULL, NULL, NULL, 1, NULL, N'M', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'd3c8ae4c-a1be-448d-bf58-6ecbf36afa0b', GETDATE()),
(516, N'HL Mountain Seat Assembly', N'SA-M687', N'1', N'0', NULL, 500, 375, 145.87, 196.92, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9e18adab-b9c7-45b1-bd95-1805ec4f297d', GETDATE()),
(517, N'LL Road Seat Assembly', N'SA-R127', N'1', N'0', NULL, 500, 375, 98.77, 133.34, NULL, NULL, NULL, NULL, 1, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'f5a30b8d-f35b-43f2-83a0-f7f6b51f6241', GETDATE()),
(518, N'ML Road Seat Assembly', N'SA-R430', N'1', N'0', NULL, 500, 375, 108.99, 147.14, NULL, NULL, NULL, NULL, 1, NULL, N'M', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ad109395-fda9-4c2a-96f1-515ccde3d9f4', GETDATE()),
(519, N'HL Road Seat Assembly', N'SA-R522', N'1', N'0', NULL, 500, 375, 145.87, 196.92, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '7b52ee2a-7100-4a39-a0af-c89012da6ef8', GETDATE()),
(520, N'LL Touring Seat Assembly', N'SA-T467', N'1', N'0', NULL, 500, 375, 98.77, 133.34, NULL, NULL, NULL, NULL, 1, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'af3d83ba-4b8e-4072-817f-e6b095a1c879', GETDATE()),
(521, N'ML Touring Seat Assembly', N'SA-T612', N'1', N'0', NULL, 500, 375, 108.99, 147.14, NULL, NULL, NULL, NULL, 1, NULL, N'M', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '85b9a3de-000c-4351-9494-05796689c216', GETDATE()),
(522, N'HL Touring Seat Assembly', N'SA-T872', N'1', N'0', NULL, 500, 375, 145.87, 196.92, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '8c471bca-a735-4087-ad50-90ede0ac1a1b', GETDATE()),
(523, N'LL Spindle/Axle', N'SD-2342', N'0', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'd2bd1f55-2cd4-4998-89fa-28ff2e28de2c', GETDATE()),
(524, N'HL Spindle/Axle', N'SD-9872', N'0', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '6ce0661d-ba1f-4012-b785-55165b3b241a', GETDATE()),
(525, N'LL Shell', N'SH-4562', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, N'L', NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'ae7bcda7-e836-4f68-9e61-745f27f9aa3e', GETDATE()),
(526, N'HL Shell', N'SH-9312', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'd215a3ae-aaf2-4cb0-9d20-3758aad078e2', GETDATE()),
(527, N'Spokes', N'SK-9283', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '5aabb729-343b-4084-a235-ccb3da9f29e7', GETDATE()),
(528, N'Seat Lug', N'SL-0931', N'0', N'0', NULL, 1000, 750, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '4a898b1e-9a3b-4beb-9873-a7465934051a', GETDATE()),
(529, N'Stem', N'SM-9087', N'1', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '1173306e-b616-4c4a-b715-4e0a483ba2b5', GETDATE()),
(530, N'Seat Post', N'SP-2981', N'0', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9b4ceb84-4e84-43f3-b326-9b7f22905363', GETDATE()),
(531, N'Steerer', N'SR-2098', N'1', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, 'f3b140a1-b139-4bb5-b144-1b7cbbee6c9a', GETDATE()),
(532, N'Seat Stays', N'SS-2985', N'1', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '20c2c611-dffc-49b5-99cf-d89bdd3a91ce', GETDATE()),
(533, N'Seat Tube', N'ST-9828', N'1', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '41f5388b-7253-4002-bcc6-b2a50920d11f', GETDATE()),
(534, N'Top Tube', N'TO-2301', N'1', N'0', NULL, 500, 375, 0, 0, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '4c0bad8e-066b-46b8-bfe9-da61539606e8', GETDATE()),
(535, N'Tension Pulley', N'TP-0923', N'0', N'0', NULL, 800, 600, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '13df62b2-8a7b-47d5-9084-f1172c4779e4', GETDATE()),
(679, N'Rear Derailleur Cage', N'RC-0291', N'0', N'0', N'Silver', 500, 375, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '912b03ea-4447-48c8-85da-09b80ab26340', GETDATE()),
(680, N'HL Road Frame - Black, 58', N'FR-R92B-58', N'1', N'1', N'Black', 500, 375, 1059.31, 1431.5, N'58', N'CM', N'LB', 2.24, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '43dd68d6-14a4-461f-9069-55309d90ea7e', GETDATE()),
(706, N'HL Road Frame - Red, 58', N'FR-R92R-58', N'1', N'1', N'Red', 500, 375, 1059.31, 1431.5, N'58', N'CM', N'LB', 2.24, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2008-04-30 00:00:00', 120), NULL, NULL, '9540ff17-2712-4c90-a3d1-8ce5568b2462', GETDATE()),
(707, N'Sport-100 Helmet, Red', N'HL-U509-R', N'0', N'1', N'Red', 4, 3, 13.0863, 34.99, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 31, 33, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '2e1ef41a-c08a-4ff6-8ada-bde58b64a712', GETDATE()),
(708, N'Sport-100 Helmet, Black', N'HL-U509', N'0', N'1', N'Black', 4, 3, 13.0863, 34.99, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 31, 33, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'a25a44fb-c2de-4268-958f-110b8d7621e2', GETDATE()),
(709, N'Mountain Bike Socks, M', N'SO-B909-M', N'0', N'1', N'White', 4, 3, 3.3963, 9.5, N'M', NULL, NULL, NULL, 0, N'M', NULL, N'U', 23, 18, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '18f95f47-1540-4e02-8f1f-cc1bcb6828d0', GETDATE()),
(710, N'Mountain Bike Socks, L', N'SO-B909-L', N'0', N'1', N'White', 4, 3, 3.3963, 9.5, N'L', NULL, NULL, NULL, 0, N'M', NULL, N'U', 23, 18, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '161c035e-21b3-4e14-8e44-af508f35d80a', GETDATE()),
(711, N'Sport-100 Helmet, Blue', N'HL-U509-B', N'0', N'1', N'Blue', 4, 3, 13.0863, 34.99, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 31, 33, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'fd7c0858-4179-48c2-865b-abd5dfc7bc1d', GETDATE()),
(712, N'AWC Logo Cap', N'CA-1098', N'0', N'1', N'Multi', 4, 3, 6.9223, 8.99, NULL, NULL, NULL, NULL, 0, N'S', NULL, N'U', 19, 2, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'b9ede243-a6f4-4629-b1d4-ffe1aedc6de7', GETDATE()),
(713, N'Long-Sleeve Logo Jersey, S', N'LJ-0192-S', N'0', N'1', N'Multi', 4, 3, 38.4923, 49.99, N'S', NULL, NULL, NULL, 0, N'S', NULL, N'U', 21, 11, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'fd449c82-a259-4fae-8584-6ca0255faf68', GETDATE()),
(714, N'Long-Sleeve Logo Jersey, M', N'LJ-0192-M', N'0', N'1', N'Multi', 4, 3, 38.4923, 49.99, N'M', NULL, NULL, NULL, 0, N'S', NULL, N'U', 21, 11, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '6a290063-a0cf-432a-8110-2ea0fda14308', GETDATE()),
(715, N'Long-Sleeve Logo Jersey, L', N'LJ-0192-L', N'0', N'1', N'Multi', 4, 3, 38.4923, 49.99, N'L', NULL, NULL, NULL, 0, N'S', NULL, N'U', 21, 11, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '34cf5ef5-c077-4ea0-914a-084814d5cbd5', GETDATE()),
(716, N'Long-Sleeve Logo Jersey, XL', N'LJ-0192-X', N'0', N'1', N'Multi', 4, 3, 38.4923, 49.99, N'XL', NULL, NULL, NULL, 0, N'S', NULL, N'U', 21, 11, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '6ec47ec9-c041-4dda-b686-2125d539ce9b', GETDATE()),
(717, N'HL Road Frame - Red, 62', N'FR-R92R-62', N'1', N'1', N'Red', 500, 375, 868.6342, 1431.5, N'62', N'CM', N'LB', 2.30, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '052e4f8b-0a2a-46b2-9f42-10febcfae416', GETDATE()),
(718, N'HL Road Frame - Red, 44', N'FR-R92R-44', N'1', N'1', N'Red', 500, 375, 868.6342, 1431.5, N'44', N'CM', N'LB', 2.12, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'a88d3b54-2cae-43f2-8c6e-ea1d97b46a7c', GETDATE()),
(719, N'HL Road Frame - Red, 48', N'FR-R92R-48', N'1', N'1', N'Red', 500, 375, 868.6342, 1431.5, N'48', N'CM', N'LB', 2.16, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '07befc9a-7634-402b-b234-d7797733baaf', GETDATE()),
(720, N'HL Road Frame - Red, 52', N'FR-R92R-52', N'1', N'1', N'Red', 500, 375, 868.6342, 1431.5, N'52', N'CM', N'LB', 2.20, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'fcfea68f-310e-4e6e-9f99-bb17d011ebae', GETDATE()),
(721, N'HL Road Frame - Red, 56', N'FR-R92R-56', N'1', N'1', N'Red', 500, 375, 868.6342, 1431.5, N'56', N'CM', N'LB', 2.24, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '56c85873-4993-41b4-8096-1067cfd7e4bd', GETDATE()),
(722, N'LL Road Frame - Black, 58', N'FR-R38B-58', N'1', N'1', N'Black', 500, 375, 204.6251, 337.22, N'58', N'CM', N'LB', 2.46, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '2140f256-f705-4d67-975d-32de03265838', GETDATE()),
(723, N'LL Road Frame - Black, 60', N'FR-R38B-60', N'1', N'1', N'Black', 500, 375, 204.6251, 337.22, N'60', N'CM', N'LB', 2.48, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'aa95e2a5-e7c4-4b74-b1ea-b52ee3b51537', GETDATE()),
(724, N'LL Road Frame - Black, 62', N'FR-R38B-62', N'1', N'1', N'Black', 500, 375, 204.6251, 337.22, N'62', N'CM', N'LB', 2.50, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '5247be33-50bf-4527-8a30-a39aae500a8e', GETDATE()),
(725, N'LL Road Frame - Red, 44', N'FR-R38R-44', N'1', N'1', N'Red', 500, 375, 187.1571, 337.22, N'44', N'CM', N'LB', 2.32, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '137d319d-44ad-42b2-ab61-60b9ce52b5f2', GETDATE()),
(726, N'LL Road Frame - Red, 48', N'FR-R38R-48', N'1', N'1', N'Red', 500, 375, 187.1571, 337.22, N'48', N'CM', N'LB', 2.36, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '35213547-275f-4767-805d-c8a4b8e13745', GETDATE()),
(727, N'LL Road Frame - Red, 52', N'FR-R38R-52', N'1', N'1', N'Red', 500, 375, 187.1571, 337.22, N'52', N'CM', N'LB', 2.40, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'c455e0b3-d716-419d-abf0-7e03efdd2e26', GETDATE()),
(728, N'LL Road Frame - Red, 58', N'FR-R38R-58', N'1', N'1', N'Red', 500, 375, 187.1571, 337.22, N'58', N'CM', N'LB', 2.46, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '799a56ff-5ad2-41b3-bfac-528b477ad129', GETDATE()),
(729, N'LL Road Frame - Red, 60', N'FR-R38R-60', N'1', N'1', N'Red', 500, 375, 187.1571, 337.22, N'60', N'CM', N'LB', 2.48, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '1784bb14-d1f5-4b24-92da-9127ad179302', GETDATE()),
(730, N'LL Road Frame - Red, 62', N'FR-R38R-62', N'1', N'1', N'Red', 500, 375, 187.1571, 337.22, N'62', N'CM', N'LB', 2.50, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '7e73aa1f-8569-4d87-9f80-ac2e513e0803', GETDATE()),
(731, N'ML Road Frame - Red, 44', N'FR-R72R-44', N'1', N'1', N'Red', 500, 375, 352.1394, 594.83, N'44', N'CM', N'LB', 2.22, 1, N'R', N'M', N'U', 14, 16, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '459e041c-3234-409e-b4cd-81728f8a2398', GETDATE()),
(732, N'ML Road Frame - Red, 48', N'FR-R72R-48', N'1', N'1', N'Red', 500, 375, 352.1394, 594.83, N'48', N'CM', N'LB', 2.26, 1, N'R', N'M', N'U', 14, 16, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'b673189c-c042-413b-8194-73bc44b0492c', GETDATE()),
(733, N'ML Road Frame - Red, 52', N'FR-R72R-52', N'1', N'1', N'Red', 500, 375, 352.1394, 594.83, N'52', N'CM', N'LB', 2.30, 1, N'R', N'M', N'U', 14, 16, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '55ea276b-82d8-4ccb-9ab1-9b1b75b15a83', GETDATE()),
(734, N'ML Road Frame - Red, 58', N'FR-R72R-58', N'1', N'1', N'Red', 500, 375, 352.1394, 594.83, N'58', N'CM', N'LB', 2.36, 1, N'R', N'M', N'U', 14, 16, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'df4ce1e2-ba9a-4657-b999-ccfa6c55d9c1', GETDATE()),
(735, N'ML Road Frame - Red, 60', N'FR-R72R-60', N'1', N'1', N'Red', 500, 375, 352.1394, 594.83, N'60', N'CM', N'LB', 2.38, 1, N'R', N'M', N'U', 14, 16, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'b2e48e8c-63a5-469a-ba4c-4f5ebb1104a4', GETDATE()),
(736, N'LL Road Frame - Black, 44', N'FR-R38B-44', N'1', N'1', N'Black', 500, 375, 204.6251, 337.22, N'44', N'CM', N'LB', 2.32, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'c9967889-f490-4a66-943a-bce432e938d8', GETDATE()),
(737, N'LL Road Frame - Black, 48', N'FR-R38B-48', N'1', N'1', N'Black', 500, 375, 204.6251, 337.22, N'48', N'CM', N'LB', 2.36, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '3b5f29b6-a441-4ff7-a0fa-fad10e2ceb4c', GETDATE()),
(738, N'LL Road Frame - Black, 52', N'FR-R38B-52', N'1', N'1', N'Black', 500, 375, 204.6251, 337.22, N'52', N'CM', N'LB', 2.40, 1, N'R', N'L', N'U', 14, 9, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '18fc5d72-a012-4dc7-bb35-0d01a84d0219', GETDATE()),
(739, N'HL Mountain Frame - Silver, 42', N'FR-M94S-42', N'1', N'1', N'Silver', 500, 375, 747.2002, 1364.5, N'42', N'CM', N'LB', 2.72, 1, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '8ae32663-8d6f-457d-8343-5b181fec43a7', GETDATE()),
(740, N'HL Mountain Frame - Silver, 44', N'FR-M94S-44', N'1', N'1', N'Silver', 500, 375, 706.811, 1364.5, N'44', N'CM', N'LB', 2.76, 1, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '1909c60c-c490-411d-b3e6-12ddd7832482', GETDATE()),
(741, N'HL Mountain Frame - Silver, 48', N'FR-M94S-52', N'1', N'1', N'Silver', 500, 375, 706.811, 1364.5, N'48', N'CM', N'LB', 2.80, 1, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'b181ec1f-ca20-4724-b2eb-15f3e455142e', GETDATE()),
(742, N'HL Mountain Frame - Silver, 46', N'FR-M94S-46', N'1', N'1', N'Silver', 500, 375, 747.2002, 1364.5, N'46', N'CM', N'LB', 2.84, 1, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'a189d86e-d923-4336-b13d-a5db6f426540', GETDATE()),
(743, N'HL Mountain Frame - Black, 42', N'FR-M94B-42', N'1', N'1', N'Black', 500, 375, 739.041, 1349.6, N'42', N'CM', N'LB', 2.72, 1, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '27db28f8-5ab8-4091-b94e-6f1b2d8e7ab0', GETDATE()),
(744, N'HL Mountain Frame - Black, 44', N'FR-M94B-44', N'1', N'1', N'Black', 500, 375, 699.0928, 1349.6, N'44', N'CM', N'LB', 2.76, 1, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'cb443286-6b25-409f-a10b-1ad4eeb4bd4e', GETDATE()),
(745, N'HL Mountain Frame - Black, 48', N'FR-M94B-48', N'1', N'1', N'Black', 500, 375, 699.0928, 1349.6, N'48', N'CM', N'LB', 2.80, 1, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '1fee0573-6676-432d-8d6d-41ba9faa5865', GETDATE()),
(746, N'HL Mountain Frame - Black, 46', N'FR-M94B-46', N'1', N'1', N'Black', 500, 375, 739.041, 1349.6, N'46', N'CM', N'LB', 2.84, 1, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '50abebcb-451e-42b9-8dbb-e5c4a34470e9', GETDATE()),
(747, N'HL Mountain Frame - Black, 38', N'FR-M94B-38', N'1', N'1', N'Black', 500, 375, 739.041, 1349.6, N'38', N'CM', N'LB', 2.68, 2, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, '0c548577-3171-4ce2-b9a0-1ed526849de8', GETDATE()),
(748, N'HL Mountain Frame - Silver, 38', N'FR-M94S-38', N'1', N'1', N'Silver', 500, 375, 747.2002, 1364.5, N'38', N'CM', N'LB', 2.68, 2, N'M', N'H', N'U', 12, 5, CONVERT(datetime, '2011-05-31 00:00:00', 120), NULL, NULL, 'f246acaa-a80b-40ec-9208-02edef885129', GETDATE()),
(749, N'Road-150 Red, 62', N'BK-R93R-62', N'1', N'1', N'Red', 100, 75, 2171.2942, 3578.27, N'62', N'CM', N'LB', 15.00, 4, N'R', N'H', N'U', 2, 25, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'bc621e1f-2553-4fdc-b22e-5e44a9003569', GETDATE()),
(750, N'Road-150 Red, 44', N'BK-R93R-44', N'1', N'1', N'Red', 100, 75, 2171.2942, 3578.27, N'44', N'CM', N'LB', 13.77, 4, N'R', N'H', N'U', 2, 25, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'c19e1136-5da4-4b40-8758-54a85d7ea494', GETDATE()),
(751, N'Road-150 Red, 48', N'BK-R93R-48', N'1', N'1', N'Red', 100, 75, 2171.2942, 3578.27, N'48', N'CM', N'LB', 14.13, 4, N'R', N'H', N'U', 2, 25, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'd10b7cc1-455e-435b-a08f-ec5b1c5776e9', GETDATE()),
(752, N'Road-150 Red, 52', N'BK-R93R-52', N'1', N'1', N'Red', 100, 75, 2171.2942, 3578.27, N'52', N'CM', N'LB', 14.42, 4, N'R', N'H', N'U', 2, 25, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '5e085ba0-3cd5-487f-85bb-79ed1c701f23', GETDATE()),
(753, N'Road-150 Red, 56', N'BK-R93R-56', N'1', N'1', N'Red', 100, 75, 2171.2942, 3578.27, N'56', N'CM', N'LB', 14.68, 4, N'R', N'H', N'U', 2, 25, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '30819b88-f0d3-4e7a-8105-19f6fac2cefb', GETDATE()),
(754, N'Road-450 Red, 58', N'BK-R68R-58', N'1', N'1', N'Red', 100, 75, 884.7083, 1457.99, N'58', N'CM', N'LB', 17.79, 4, N'R', N'M', N'U', 2, 28, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '40d5effa-c0c4-479f-af66-5f1bf8ed3bfb', GETDATE()),
(755, N'Road-450 Red, 60', N'BK-R68R-60', N'1', N'1', N'Red', 100, 75, 884.7083, 1457.99, N'60', N'CM', N'LB', 17.90, 4, N'R', N'M', N'U', 2, 28, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '181a90cb-3678-490e-8418-78f73fb5343d', GETDATE()),
(756, N'Road-450 Red, 44', N'BK-R68R-44', N'1', N'1', N'Red', 100, 75, 884.7083, 1457.99, N'44', N'CM', N'LB', 16.77, 4, N'R', N'M', N'U', 2, 28, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'f8b5e26a-3d33-4e39-b500-cc21a133062e', GETDATE()),
(757, N'Road-450 Red, 48', N'BK-R68R-48', N'1', N'1', N'Red', 100, 75, 884.7083, 1457.99, N'48', N'CM', N'LB', 17.13, 4, N'R', N'M', N'U', 2, 28, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'c72c9978-0b04-46b3-9de6-948feca1c86e', GETDATE()),
(758, N'Road-450 Red, 52', N'BK-R68R-52', N'1', N'1', N'Red', 100, 75, 884.7083, 1457.99, N'52', N'CM', N'LB', 17.42, 4, N'R', N'M', N'U', 2, 28, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '040a4b7d-4060-4507-aa92-7508b434797e', GETDATE()),
(759, N'Road-650 Red, 58', N'BK-R50R-58', N'1', N'1', N'Red', 100, 75, 486.7066, 782.99, N'58', N'CM', N'LB', 19.79, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '6711d6bc-664f-4890-9f69-af1de321d055', GETDATE()),
(760, N'Road-650 Red, 60', N'BK-R50R-60', N'1', N'1', N'Red', 100, 75, 486.7066, 782.99, N'60', N'CM', N'LB', 19.90, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '664867e5-4ab3-4783-96f9-42efde92f49b', GETDATE()),
(761, N'Road-650 Red, 62', N'BK-R50R-62', N'1', N'1', N'Red', 100, 75, 486.7066, 782.99, N'62', N'CM', N'LB', 20.00, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '1da14e09-6d71-4e2a-9ee9-1bdfdfd8a109', GETDATE()),
(762, N'Road-650 Red, 44', N'BK-R50R-44', N'1', N'1', N'Red', 100, 75, 486.7066, 782.99, N'44', N'CM', N'LB', 18.77, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'f247aaae-12e3-4048-a37b-cce4a8999e81', GETDATE()),
(763, N'Road-650 Red, 48', N'BK-R50R-48', N'1', N'1', N'Red', 100, 75, 486.7066, 782.99, N'48', N'CM', N'LB', 19.13, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '3da5fa6e-9e0f-4896-ac10-948c27f4eb79', GETDATE()),
(764, N'Road-650 Red, 52', N'BK-R50R-52', N'1', N'1', N'Red', 100, 75, 486.7066, 782.99, N'52', N'CM', N'LB', 19.42, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '07cfe1ea-8a37-4d2a-835f-bc8d37e564af', GETDATE()),
(765, N'Road-650 Black, 58', N'BK-R50B-58', N'1', N'1', N'Black', 100, 75, 486.7066, 782.99, N'58', N'CM', N'LB', 19.79, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '74645b12-3ce9-49a6-bd96-2cd814b37420', GETDATE()),
(766, N'Road-650 Black, 60', N'BK-R50B-60', N'1', N'1', N'Black', 100, 75, 486.7066, 782.99, N'60', N'CM', N'LB', 19.90, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'a2db196d-6640-49ea-a84f-2e87ca6f50c6', GETDATE()),
(767, N'Road-650 Black, 62', N'BK-R50B-62', N'1', N'1', N'Black', 100, 75, 486.7066, 782.99, N'62', N'CM', N'LB', 20.00, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'c82a8309-63d3-4c0c-ad71-e91272397095', GETDATE()),
(768, N'Road-650 Black, 44', N'BK-R50B-44', N'1', N'1', N'Black', 100, 75, 486.7066, 782.99, N'44', N'CM', N'LB', 18.77, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '11d563ac-115c-4f0d-a1e5-e946eee8b38b', GETDATE()),
(769, N'Road-650 Black, 48', N'BK-R50B-48', N'1', N'1', N'Black', 100, 75, 486.7066, 782.99, N'48', N'CM', N'LB', 19.13, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'a7e2179e-137c-497e-a5e6-c9ea64935fb0', GETDATE()),
(770, N'Road-650 Black, 52', N'BK-R50B-52', N'1', N'1', N'Black', 100, 75, 486.7066, 782.99, N'52', N'CM', N'LB', 19.42, 4, N'R', N'L', N'U', 2, 30, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '136e2865-e0da-4624-963a-31349279ab1a', GETDATE()),
(771, N'Mountain-100 Silver, 38', N'BK-M82S-38', N'1', N'1', N'Silver', 100, 75, 1912.1544, 3399.99, N'38', N'CM', N'LB', 20.35, 4, N'M', N'H', N'U', 1, 19, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'ca74b54e-fc30-4464-8b83-019bfd1b2dbb', GETDATE()),
(772, N'Mountain-100 Silver, 42', N'BK-M82S-42', N'1', N'1', N'Silver', 100, 75, 1912.1544, 3399.99, N'42', N'CM', N'LB', 20.77, 4, N'M', N'H', N'U', 1, 19, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'bbfff5a5-4bdc-49a9-a5ad-7584adffe808', GETDATE()),
(773, N'Mountain-100 Silver, 44', N'BK-M82S-44', N'1', N'1', N'Silver', 100, 75, 1912.1544, 3399.99, N'44', N'CM', N'LB', 21.13, 4, N'M', N'H', N'U', 1, 19, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '155fd77e-d6d6-43f0-8a5b-4a8305eb45cd', GETDATE()),
(774, N'Mountain-100 Silver, 48', N'BK-M82S-48', N'1', N'1', N'Silver', 100, 75, 1912.1544, 3399.99, N'48', N'CM', N'LB', 21.42, 4, N'M', N'H', N'U', 1, 19, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'ba5551df-c9ee-4b43-b3ca-8c19d0f9384d', GETDATE()),
(775, N'Mountain-100 Black, 38', N'BK-M82B-38', N'1', N'1', N'Black', 100, 75, 1898.0944, 3374.99, N'38', N'CM', N'LB', 20.35, 4, N'M', N'H', N'U', 1, 19, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, 'dea33347-fcd3-4346-aa0f-068cd6b3ad06', GETDATE()),
(776, N'Mountain-100 Black, 42', N'BK-M82B-42', N'1', N'1', N'Black', 100, 75, 1898.0944, 3374.99, N'42', N'CM', N'LB', 20.77, 4, N'M', N'H', N'U', 1, 19, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '02935111-a546-4c6d-941f-be12d42c158e', GETDATE()),
(777, N'Mountain-100 Black, 44', N'BK-M82B-44', N'1', N'1', N'Black', 100, 75, 1898.0944, 3374.99, N'44', N'CM', N'LB', 21.13, 4, N'M', N'H', N'U', 1, 19, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '7920bc3b-8fd4-4610-93d2-e693a66b6474', GETDATE()),
(778, N'Mountain-100 Black, 48', N'BK-M82B-48', N'1', N'1', N'Black', 100, 75, 1898.0944, 3374.99, N'48', N'CM', N'LB', 21.42, 4, N'M', N'H', N'U', 1, 19, CONVERT(datetime, '2011-05-31 00:00:00', 120), CONVERT(datetime, '2012-05-29 00:00:00', 120), NULL, '1b486300-7e64-4c5d-a9ba-a8368e20c5a0', GETDATE()),
(779, N'Mountain-200 Silver, 38', N'BK-M68S-38', N'1', N'1', N'Silver', 100, 75, 1265.6195, 2319.99, N'38', N'CM', N'LB', 23.35, 4, N'M', N'H', N'U', 1, 20, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '3a45d835-abae-4806-ac03-c53abcd3b974', GETDATE()),
(780, N'Mountain-200 Silver, 42', N'BK-M68S-42', N'1', N'1', N'Silver', 100, 75, 1265.6195, 2319.99, N'42', N'CM', N'LB', 23.77, 4, N'M', N'H', N'U', 1, 20, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'ce4849b4-56e6-4b50-808b-9bde67cc4704', GETDATE()),
(781, N'Mountain-200 Silver, 46', N'BK-M68S-46', N'1', N'1', N'Silver', 100, 75, 1265.6195, 2319.99, N'46', N'CM', N'LB', 24.13, 4, N'M', N'H', N'U', 1, 20, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '20799030-420c-496a-9922-09189c2b457e', GETDATE()),
(782, N'Mountain-200 Black, 38', N'BK-M68B-38', N'1', N'1', N'Black', 100, 75, 1251.9813, 2294.99, N'38', N'CM', N'LB', 23.35, 4, N'M', N'H', N'U', 1, 20, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '82cb8f9b-b8bb-4841-98d3-bcdb807c4dd8', GETDATE()),
(783, N'Mountain-200 Black, 42', N'BK-M68B-42', N'1', N'1', N'Black', 100, 75, 1251.9813, 2294.99, N'42', N'CM', N'LB', 23.77, 4, N'M', N'H', N'U', 1, 20, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '2b0af5b9-7571-4621-b760-47df599f9650', GETDATE()),
(784, N'Mountain-200 Black, 46', N'BK-M68B-46', N'1', N'1', N'Black', 100, 75, 1251.9813, 2294.99, N'46', N'CM', N'LB', 24.13, 4, N'M', N'H', N'U', 1, 20, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '33f42284-e216-4b98-ba48-b4ee18a01fa0', GETDATE()),
(785, N'Mountain-300 Black, 38', N'BK-M47B-38', N'1', N'1', N'Black', 100, 75, 598.4354, 1079.99, N'38', N'CM', N'LB', 25.35, 4, N'M', N'M', N'U', 1, 21, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'f06c2cbf-0901-4c08-80ed-fb4e87171b3b', GETDATE()),
(786, N'Mountain-300 Black, 40', N'BK-M47B-40', N'1', N'1', N'Black', 100, 75, 598.4354, 1079.99, N'40', N'CM', N'LB', 25.77, 4, N'M', N'M', N'U', 1, 21, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '580d4322-be03-4c91-83d2-ee33ec6008ab', GETDATE()),
(787, N'Mountain-300 Black, 44', N'BK-M47B-44', N'1', N'1', N'Black', 100, 75, 598.4354, 1079.99, N'44', N'CM', N'LB', 26.13, 4, N'M', N'M', N'U', 1, 21, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '07c2a548-0452-47b4-9dce-6edb0a30c85e', GETDATE()),
(788, N'Mountain-300 Black, 48', N'BK-M47B-48', N'1', N'1', N'Black', 100, 75, 598.4354, 1079.99, N'48', N'CM', N'LB', 26.42, 4, N'M', N'M', N'U', 1, 21, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '16078dbe-388d-4c18-aa8f-0c8f48035468', GETDATE()),
(789, N'Road-250 Red, 44', N'BK-R89R-44', N'1', N'1', N'Red', 100, 75, 1518.7864, 2443.35, N'44', N'CM', N'LB', 14.77, 4, N'R', N'H', N'U', 2, 26, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '0aa71ad6-afaf-43c6-9745-35d815b50a5b', GETDATE()),
(790, N'Road-250 Red, 48', N'BK-R89R-48', N'1', N'1', N'Red', 100, 75, 1518.7864, 2443.35, N'48', N'CM', N'LB', 15.13, 4, N'R', N'H', N'U', 2, 26, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '115ddade-70e3-43f9-80dc-638daea271c4', GETDATE()),
(791, N'Road-250 Red, 52', N'BK-R89R-52', N'1', N'1', N'Red', 100, 75, 1518.7864, 2443.35, N'52', N'CM', N'LB', 15.42, 4, N'R', N'H', N'U', 2, 26, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'c9fd1df4-9512-420a-b379-067108033b75', GETDATE()),
(792, N'Road-250 Red, 58', N'BK-R89R-58', N'1', N'1', N'Red', 100, 75, 1554.9479, 2443.35, N'58', N'CM', N'LB', 15.79, 4, N'R', N'H', N'U', 2, 26, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'df629c11-8d8b-4774-9d52-ecb64dc95212', GETDATE()),
(793, N'Road-250 Black, 44', N'BK-R89B-44', N'1', N'1', N'Black', 100, 75, 1554.9479, 2443.35, N'44', N'CM', N'LB', 14.77, 4, N'R', N'H', N'U', 2, 26, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '1ff419b5-52af-4f7e-aeae-4fec5e99de35', GETDATE()),
(794, N'Road-250 Black, 48', N'BK-R89B-48', N'1', N'1', N'Black', 100, 75, 1554.9479, 2443.35, N'48', N'CM', N'LB', 15.13, 4, N'R', N'H', N'U', 2, 26, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '9d165ddf-8f5d-41c7-9bb8-13f41a3d1f62', GETDATE()),
(795, N'Road-250 Black, 52', N'BK-R89B-52', N'1', N'1', N'Black', 100, 75, 1554.9479, 2443.35, N'52', N'CM', N'LB', 15.42, 4, N'R', N'H', N'U', 2, 26, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '74fe3957-cbc3-450a-ab92-849bd13530e2', GETDATE()),
(796, N'Road-250 Black, 58', N'BK-R89B-58', N'1', N'1', N'Black', 100, 75, 1554.9479, 2443.35, N'58', N'CM', N'LB', 15.68, 4, N'R', N'H', N'U', 2, 26, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '1c530fe8-a169-4adc-b3dc-b11a48245e63', GETDATE()),
(797, N'Road-550-W Yellow, 38', N'BK-R64Y-38', N'1', N'1', N'Yellow', 100, 75, 713.0798, 1120.49, N'38', N'CM', N'LB', 17.35, 4, N'R', N'M', N'W', 2, 29, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'aad81532-a572-49a5-83c3-dfa9e3b4fea6', GETDATE()),
(798, N'Road-550-W Yellow, 40', N'BK-R64Y-40', N'1', N'1', N'Yellow', 100, 75, 713.0798, 1120.49, N'40', N'CM', N'LB', 17.77, 4, N'R', N'M', N'W', 2, 29, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'a35a1c35-c128-4697-951e-4199062e78f3', GETDATE()),
(799, N'Road-550-W Yellow, 42', N'BK-R64Y-42', N'1', N'1', N'Yellow', 100, 75, 713.0798, 1120.49, N'42', N'CM', N'LB', 18.13, 4, N'R', N'M', N'W', 2, 29, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'a359ab09-16f2-4726-a60f-0dfdb1c9527e', GETDATE()),
(800, N'Road-550-W Yellow, 44', N'BK-R64Y-44', N'1', N'1', N'Yellow', 100, 75, 713.0798, 1120.49, N'44', N'CM', N'LB', 18.42, 4, N'R', N'M', N'W', 2, 29, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '0a7028fb-ff06-4d38-aaa1-b64816278165', GETDATE()),
(801, N'Road-550-W Yellow, 48', N'BK-R64Y-48', N'1', N'1', N'Yellow', 100, 75, 713.0798, 1120.49, N'48', N'CM', N'LB', 18.68, 4, N'R', N'M', N'W', 2, 29, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'c90cc877-804c-4ce7-afc3-4c8791a13dfb', GETDATE()),
(802, N'LL Fork', N'FK-1639', N'1', N'1', NULL, 500, 375, 65.8097, 148.22, NULL, NULL, NULL, NULL, 1, NULL, N'L', NULL, 10, 104, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'fb8502be-07eb-4134-ab06-c3a9959a52ae', GETDATE()),
(803, N'ML Fork', N'FK-5136', N'1', N'1', NULL, 500, 375, 77.9176, 175.49, NULL, NULL, NULL, NULL, 1, NULL, N'M', NULL, 10, 105, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'f5fa4e2f-b976-48a4-bf79-85632f697d2e', GETDATE()),
(804, N'HL Fork', N'FK-9939', N'1', N'1', NULL, 500, 375, 101.8936, 229.49, NULL, NULL, NULL, NULL, 1, NULL, N'H', NULL, 10, 106, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '553229b3-1ad9-4a71-a21c-2af4332cfce9', GETDATE()),
(805, N'LL Headset', N'HS-0296', N'1', N'1', NULL, 500, 375, 15.1848, 34.2, NULL, NULL, NULL, NULL, 1, NULL, N'L', NULL, 11, 59, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'bb6bd7b3-a34d-4d64-822e-781fa6838e19', GETDATE()),
(806, N'ML Headset', N'HS-2451', N'1', N'1', NULL, 500, 375, 45.4168, 102.29, NULL, NULL, NULL, NULL, 1, NULL, N'M', NULL, 11, 60, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '23b5d52b-8c29-4059-b899-75c53b5ee2e6', GETDATE()),
(807, N'HL Headset', N'HS-3479', N'1', N'1', NULL, 500, 375, 55.3801, 124.73, NULL, NULL, NULL, NULL, 1, NULL, N'H', NULL, 11, 61, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '12e4d5e8-79ed-4bcb-a532-6275d1a93417', GETDATE()),
(808, N'LL Mountain Handlebars', N'HB-M243', N'1', N'1', NULL, 500, 375, 19.7758, 44.54, NULL, NULL, NULL, NULL, 1, N'M', N'L', NULL, 4, 52, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'b59b7bf2-7afc-4a74-b063-f942f1e0da19', GETDATE()),
(809, N'ML Mountain Handlebars', N'HB-M763', N'1', N'1', NULL, 500, 375, 27.4925, 61.92, NULL, NULL, NULL, NULL, 1, N'M', N'M', NULL, 4, 54, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'ae6020df-d9c9-4d34-9795-1f80e6bbf5a5', GETDATE()),
(810, N'HL Mountain Handlebars', N'HB-M918', N'1', N'1', NULL, 500, 375, 53.3999, 120.27, NULL, NULL, NULL, NULL, 1, N'M', N'H', NULL, 4, 55, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '6aa0f921-0f09-4f99-8d3c-335946873553', GETDATE()),
(811, N'LL Road Handlebars', N'HB-R504', N'1', N'1', NULL, 500, 375, 19.7758, 44.54, NULL, NULL, NULL, NULL, 1, N'R', N'L', NULL, 4, 56, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'acdae407-b40e-435e-8e84-1bfc33013e81', GETDATE()),
(812, N'ML Road Handlebars', N'HB-R720', N'1', N'1', NULL, 500, 375, 27.4925, 61.92, NULL, NULL, NULL, NULL, 1, N'R', N'M', NULL, 4, 57, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '33fa1a03-38d6-405e-be9b-eea92f3d204f', GETDATE()),
(813, N'HL Road Handlebars', N'HB-R956', N'1', N'1', NULL, 500, 375, 53.3999, 120.27, NULL, NULL, NULL, NULL, 1, N'R', N'H', NULL, 4, 58, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '5c5327b9-ad2e-4c33-8d74-edf49e0c2dd8', GETDATE()),
(814, N'ML Mountain Frame - Black, 38', N'FR-M63B-38', N'1', N'1', N'Black', 500, 375, 185.8193, 348.76, N'38', N'CM', N'LB', 2.73, 2, N'M', N'M', N'U', 12, 15, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '0d879312-a7d3-441d-9d23-b6550bab3814', GETDATE()),
(815, N'LL Mountain Front Wheel', N'FW-M423', N'1', N'1', N'Black', 500, 375, 26.9708, 60.745, NULL, NULL, NULL, NULL, 1, N'M', N'L', NULL, 17, 42, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'd7b1d161-182e-44f6-a9ff-9c1eba76613b', GETDATE()),
(816, N'ML Mountain Front Wheel', N'FW-M762', N'1', N'1', N'Black', 500, 375, 92.8071, 209.025, NULL, NULL, NULL, NULL, 1, N'M', N'M', NULL, 17, 45, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '5e3e5033-9a77-4dca-8b7f-dfed78efa08a', GETDATE()),
(817, N'HL Mountain Front Wheel', N'FW-M928', N'1', N'1', N'Black', 500, 375, 133.2955, 300.215, NULL, NULL, NULL, NULL, 1, N'M', N'H', NULL, 17, 46, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '3c8ffff6-a8dc-45a3-963b-a6284ced7596', GETDATE()),
(818, N'LL Road Front Wheel', N'FW-R623', N'1', N'1', N'Black', 500, 375, 37.9909, 85.565, NULL, NULL, N'G', 900.00, 1, N'R', N'L', NULL, 17, 49, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '9e66de78-decb-438a-b9a9-023c773c60a2', GETDATE()),
(819, N'ML Road Front Wheel', N'FW-R762', N'1', N'1', N'Black', 500, 375, 110.2829, 248.385, NULL, NULL, N'G', 850.00, 1, N'R', N'M', NULL, 17, 50, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '6ea94fbf-b9aa-43fc-84e8-91d508dde751', GETDATE()),
(820, N'HL Road Front Wheel', N'FW-R820', N'1', N'1', N'Black', 500, 375, 146.5466, 330.06, NULL, NULL, N'G', 650.00, 1, N'R', N'H', NULL, 17, 51, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '727a3cd5-8d40-4884-a7e4-dfd3ffdeb164', GETDATE()),
(821, N'Touring Front Wheel', N'FW-T905', N'1', N'1', N'Black', 500, 375, 96.7964, 218.01, NULL, NULL, NULL, NULL, 1, N'T', NULL, NULL, 17, 44, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '8d1cdb07-4fa1-4ac1-840f-a16c3dca5009', GETDATE()),
(822, N'ML Road Frame-W - Yellow, 38', N'FR-R72Y-38', N'1', N'1', N'Yellow', 500, 375, 360.9428, 594.83, N'38', N'CM', N'LB', 2.18, 2, N'R', N'M', N'W', 14, 17, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '22976fa7-0ad0-40f9-b4f9-ba10279ea1a3', GETDATE()),
(823, N'LL Mountain Rear Wheel', N'RW-M423', N'1', N'1', N'Black', 500, 375, 38.9588, 87.745, NULL, NULL, NULL, NULL, 1, N'M', N'L', NULL, 17, 123, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'e6c39f58-995f-4786-a309-df3812d8b30f', GETDATE()),
(824, N'ML Mountain Rear Wheel', N'RW-M762', N'1', N'1', N'Black', 500, 375, 104.7951, 236.025, NULL, NULL, NULL, NULL, 1, N'M', N'M', NULL, 17, 124, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'b2f7cf9b-a7bf-49ab-9cca-9f6e791cd693', GETDATE()),
(825, N'HL Mountain Rear Wheel', N'RW-M928', N'1', N'1', N'Black', 500, 375, 145.2835, 327.215, NULL, NULL, NULL, NULL, 1, N'M', N'H', NULL, 17, 125, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '35d02edc-1120-41fb-b5df-8655a93b3353', GETDATE()),
(826, N'LL Road Rear Wheel', N'RW-R623', N'1', N'1', N'Black', 500, 375, 49.9789, 112.565, NULL, NULL, N'G', 1050.00, 1, N'R', N'L', NULL, 17, 126, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '78d01117-8dcd-465f-9dc7-94a2cdc35582', GETDATE()),
(827, N'ML Road Rear Wheel', N'RW-R762', N'1', N'1', N'Black', 500, 375, 122.2709, 275.385, NULL, NULL, N'G', 1000.00, 1, N'R', N'M', NULL, 17, 77, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'cf739f9a-0af0-4810-b229-c431a31d7779', GETDATE()),
(828, N'HL Road Rear Wheel', N'RW-R820', N'1', N'1', N'Black', 500, 375, 158.5346, 357.06, NULL, NULL, N'G', 890.00, 1, N'R', N'H', NULL, 17, 78, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '8f262a37-43aa-4ad5-ae1f-8bd6967d8e9b', GETDATE()),
(829, N'Touring Rear Wheel', N'RW-T905', N'1', N'1', N'Black', 500, 375, 108.7844, 245.01, NULL, NULL, NULL, NULL, 1, N'T', NULL, NULL, 17, 43, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '30d2254d-e26d-4586-b4c5-e8ccb8a059b8', GETDATE()),
(830, N'ML Mountain Frame - Black, 40', N'FR-M63B-40', N'1', N'1', N'Black', 500, 375, 185.8193, 348.76, N'40', N'CM', N'LB', 2.77, 1, N'M', N'M', N'U', 12, 14, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'aed1c9c4-c139-45d2-b38e-0a0a9f518665', GETDATE()),
(831, N'ML Mountain Frame - Black, 44', N'FR-M63B-44', N'1', N'1', N'Black', 500, 375, 185.8193, 348.76, N'44', N'CM', N'LB', 2.81, 1, N'M', N'M', N'U', 12, 14, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '3f2135d4-ec5f-4e30-bde8-5444518f0637', GETDATE()),
(832, N'ML Mountain Frame - Black, 48', N'FR-M63B-48', N'1', N'1', N'Black', 500, 375, 185.8193, 348.76, N'48', N'CM', N'LB', 2.85, 1, N'M', N'M', N'U', 12, 14, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'c2009b69-f15a-47ec-b710-1809d299318a', GETDATE()),
(833, N'ML Road Frame-W - Yellow, 40', N'FR-R72Y-40', N'1', N'1', N'Yellow', 500, 375, 360.9428, 594.83, N'40', N'CM', N'LB', 2.22, 1, N'R', N'M', N'W', 14, 17, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '22df26f2-60bc-493e-a14a-5500633e9f7e', GETDATE()),
(834, N'ML Road Frame-W - Yellow, 42', N'FR-R72Y-42', N'1', N'1', N'Yellow', 500, 375, 360.9428, 594.83, N'42', N'CM', N'LB', 2.26, 1, N'R', N'M', N'W', 14, 17, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '207b54da-5404-415d-8578-9a45082e3bf1', GETDATE()),
(835, N'ML Road Frame-W - Yellow, 44', N'FR-R72Y-44', N'1', N'1', N'Yellow', 500, 375, 360.9428, 594.83, N'44', N'CM', N'LB', 2.30, 1, N'R', N'M', N'W', 14, 17, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'a0fad492-ac24-4fcf-8d2a-d21d06386ae1', GETDATE()),
(836, N'ML Road Frame-W - Yellow, 48', N'FR-R72Y-48', N'1', N'1', N'Yellow', 500, 375, 360.9428, 594.83, N'48', N'CM', N'LB', 2.34, 1, N'R', N'M', N'W', 14, 17, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '8487bfe0-2138-471e-9c6d-fdb3a67e7d86', GETDATE()),
(837, N'HL Road Frame - Black, 62', N'FR-R92B-62', N'1', N'1', N'Black', 500, 375, 868.6342, 1431.5, N'62', N'CM', N'LB', 2.30, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '5dce9c8c-fb94-46f8-b826-11658f6a3682', GETDATE()),
(838, N'HL Road Frame - Black, 44', N'FR-R92B-44', N'1', N'1', N'Black', 500, 375, 868.6342, 1431.5, N'44', N'CM', N'LB', 2.12, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'e4902656-a4bc-4b08-9d47-4f3da0fd76c3', GETDATE()),
(839, N'HL Road Frame - Black, 48', N'FR-R92B-48', N'1', N'1', N'Black', 500, 375, 868.6342, 1431.5, N'48', N'CM', N'LB', 2.16, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '557b603b-407b-41a4-ae34-4f7747866dba', GETDATE()),
(840, N'HL Road Frame - Black, 52', N'FR-R92B-52', N'1', N'1', N'Black', 500, 375, 868.6342, 1431.5, N'52', N'CM', N'LB', 2.20, 1, N'R', N'H', N'U', 14, 6, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '0ed082b3-fbba-43af-9149-8741b9fc78c8', GETDATE()),
(841, N'Men''s Sports Shorts, S', N'SH-M897-S', N'0', N'1', N'Black', 4, 3, 24.7459, 59.99, N'S', NULL, NULL, NULL, 0, N'S', NULL, N'M', 22, 13, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '34b08c1f-99d1-43c4-8ef7-2cd754b6665d', GETDATE()),
(842, N'Touring-Panniers, Large', N'PA-T100', N'0', N'1', N'Grey', 4, 3, 51.5625, 125, NULL, NULL, NULL, NULL, 0, N'T', NULL, NULL, 35, 120, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '56334fff-91d4-495e-bf98-933bc1010f23', GETDATE()),
(843, N'Cable Lock', N'LO-C100', N'0', N'1', NULL, 4, 3, 10.3125, 25, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 34, 115, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '56ffd7b9-1014-4640-b1bd-b2649589b4d7', GETDATE()),
(844, N'Minipump', N'PU-0452', N'0', N'1', NULL, 4, 3, 8.2459, 19.99, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 36, 116, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'aaf7a076-9ee3-46bf-a69f-414d847e858b', GETDATE()),
(845, N'Mountain Pump', N'PU-M044', N'0', N'1', NULL, 4, 3, 10.3084, 24.99, NULL, NULL, NULL, NULL, 0, N'M', NULL, NULL, 36, 117, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '57169f80-fafb-4678-8f51-fe1f131d0c83', GETDATE()),
(846, N'Taillights - Battery-Powered', N'LT-T990', N'0', N'1', NULL, 4, 3, 5.7709, 13.99, NULL, NULL, NULL, NULL, 0, N'R', NULL, NULL, 33, 108, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '3c617b87-50a5-434c-a0d3-22314b7027ee', GETDATE()),
(847, N'Headlights - Dual-Beam', N'LT-H902', N'0', N'1', NULL, 4, 3, 14.4334, 34.99, NULL, NULL, NULL, NULL, 0, N'R', NULL, NULL, 33, 109, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '417db6cb-f38f-4b0d-87e7-e1ebf7456c3a', GETDATE()),
(848, N'Headlights - Weatherproof', N'LT-H903', N'0', N'1', NULL, 4, 3, 18.5584, 44.99, NULL, NULL, NULL, NULL, 0, N'R', NULL, NULL, 33, 110, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'fc362c1a-4b9c-4d5f-a6d3-0775846c61f0', GETDATE()),
(849, N'Men''s Sports Shorts, M', N'SH-M897-M', N'0', N'1', N'Black', 4, 3, 24.7459, 59.99, N'M', NULL, NULL, NULL, 0, N'S', NULL, N'M', 22, 13, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'db37b435-74b9-43d3-b363-abbead107bc4', GETDATE()),
(850, N'Men''s Sports Shorts, L', N'SH-M897-L', N'0', N'1', N'Black', 4, 3, 24.7459, 59.99, N'L', NULL, NULL, NULL, 0, N'S', NULL, N'M', 22, 13, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '714184c5-669b-4cf1-a802-30e7b1ea7722', GETDATE()),
(851, N'Men''s Sports Shorts, XL', N'SH-M897-X', N'0', N'1', N'Black', 4, 3, 24.7459, 59.99, N'XL', NULL, NULL, NULL, 0, N'S', NULL, N'M', 22, 13, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'bd3ffe40-fe2e-44cb-a4e0-81786c3a751f', GETDATE()),
(852, N'Women''s Tights, S', N'TG-W091-S', N'0', N'1', N'Black', 4, 3, 30.9334, 74.99, N'S', NULL, NULL, NULL, 0, N'S', NULL, N'W', 24, 38, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '7de86c98-4f5b-4155-8572-c977f14ebaeb', GETDATE()),
(853, N'Women''s Tights, M', N'TG-W091-M', N'0', N'1', N'Black', 4, 3, 30.9334, 74.99, N'M', NULL, NULL, NULL, 0, N'S', NULL, N'W', 24, 38, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '4d8e186c-b8c9-4c64-b411-4995dd87e316', GETDATE()),
(854, N'Women''s Tights, L', N'TG-W091-L', N'0', N'1', N'Black', 4, 3, 30.9334, 74.99, N'L', NULL, NULL, NULL, 0, N'S', NULL, N'W', 24, 38, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'e378c2f3-54f6-4ea9-b049-e8bb32b5bdfd', GETDATE()),
(855, N'Men''s Bib-Shorts, S', N'SB-M891-S', N'0', N'1', N'Multi', 4, 3, 37.1209, 89.99, N'S', NULL, NULL, NULL, 0, N'S', NULL, N'M', 18, 12, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '9f60af1e-4c11-4e90-baea-48e834e8b4c2', GETDATE()),
(856, N'Men''s Bib-Shorts, M', N'SB-M891-M', N'0', N'1', N'Multi', 4, 3, 37.1209, 89.99, N'M', NULL, NULL, NULL, 0, N'S', NULL, N'M', 18, 12, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'e0cbec04-f4fc-450f-9780-f8ea7691febd', GETDATE()),
(857, N'Men''s Bib-Shorts, L', N'SB-M891-L', N'0', N'1', N'Multi', 4, 3, 37.1209, 89.99, N'L', NULL, NULL, NULL, 0, N'S', NULL, N'M', 18, 12, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, 'e1df75a4-9986-489e-a5de-ad3da824eb5e', GETDATE()),
(858, N'Half-Finger Gloves, S', N'GL-H102-S', N'0', N'1', N'Black', 4, 3, 9.1593, 24.49, N'S', NULL, NULL, NULL, 0, N'S', NULL, N'U', 20, 4, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '9e1db5c3-539d-4061-9433-d762dc195cd8', GETDATE()),
(859, N'Half-Finger Gloves, M', N'GL-H102-M', N'0', N'1', N'Black', 4, 3, 9.1593, 24.49, N'M', NULL, NULL, NULL, 0, N'S', NULL, N'U', 20, 4, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, '9d458fd5-392d-4ab1-afef-6a5548e48858', GETDATE()),
(860, N'Half-Finger Gloves, L', N'GL-H102-L', N'0', N'1', N'Black', 4, 3, 9.1593, 24.49, N'L', NULL, NULL, NULL, 0, N'S', NULL, N'U', 20, 4, CONVERT(datetime, '2012-05-30 00:00:00', 120), NULL, NULL, 'fa710215-925f-4959-81df-538e72a6a255', GETDATE()),
(861, N'Full-Finger Gloves, S', N'GL-F110-S', N'0', N'1', N'Black', 4, 3, 15.6709, 37.99, N'S', NULL, NULL, NULL, 0, N'M', NULL, N'U', 20, 3, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '76fac097-1fb3-456b-8fb9-2c7a613771b4', GETDATE()),
(862, N'Full-Finger Gloves, M', N'GL-F110-M', N'0', N'1', N'Black', 4, 3, 15.6709, 37.99, N'M', NULL, NULL, NULL, 0, N'M', NULL, N'U', 20, 3, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '1084221e-1890-443e-9d87-afcad6358355', GETDATE()),
(863, N'Full-Finger Gloves, L', N'GL-F110-L', N'0', N'1', N'Black', 4, 3, 15.6709, 37.99, N'L', NULL, NULL, NULL, 0, N'M', NULL, N'U', 20, 3, CONVERT(datetime, '2012-05-30 00:00:00', 120), CONVERT(datetime, '2013-05-29 00:00:00', 120), NULL, '6116f9d4-8a1d-4022-a642-9c445c197203', GETDATE()),
(864, N'Classic Vest, S', N'VE-C304-S', N'0', N'1', N'Blue', 4, 3, 23.749, 63.5, N'S', NULL, NULL, NULL, 0, N'S', NULL, N'U', 25, 1, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'eb423ef3-409d-46fe-b35b-d69970820314', GETDATE()),
(865, N'Classic Vest, M', N'VE-C304-M', N'0', N'1', N'Blue', 4, 3, 23.749, 63.5, N'M', NULL, NULL, NULL, 0, N'S', NULL, N'U', 25, 1, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '2e52f96e-64a1-4069-911c-e3fd6e094a1e', GETDATE()),
(866, N'Classic Vest, L', N'VE-C304-L', N'0', N'1', N'Blue', 4, 3, 23.749, 63.5, N'L', NULL, NULL, NULL, 0, N'S', NULL, N'U', 25, 1, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '3211f5a8-b6c4-48bd-9aa4-d69cb40d97dd', GETDATE()),
(867, N'Women''s Mountain Shorts, S', N'SH-W890-S', N'0', N'1', N'Black', 4, 3, 26.1763, 69.99, N'S', NULL, NULL, NULL, 0, N'M', NULL, N'W', 22, 37, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '22616fd2-b99f-4f7d-acf6-33dff66d42d2', GETDATE()),
(868, N'Women''s Mountain Shorts, M', N'SH-W890-M', N'0', N'1', N'Black', 4, 3, 26.1763, 69.99, N'M', NULL, NULL, NULL, 0, N'M', NULL, N'W', 22, 37, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '968e3610-e583-42e8-8ab6-484a799b1774', GETDATE()),
(869, N'Women''s Mountain Shorts, L', N'SH-W890-L', N'0', N'1', N'Black', 4, 3, 26.1763, 69.99, N'L', NULL, NULL, NULL, 0, N'M', NULL, N'W', 22, 37, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '1a66b244-5ca0-4153-b539-ae048d14faec', GETDATE()),
(870, N'Water Bottle - 30 oz.', N'WB-H098', N'0', N'1', NULL, 4, 3, 1.8663, 4.99, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 28, 111, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '834e8d1a-69a7-4c42-8b68-fa08d9ec9e5b', GETDATE()),
(871, N'Mountain Bottle Cage', N'BC-M005', N'0', N'1', NULL, 4, 3, 3.7363, 9.99, NULL, NULL, NULL, NULL, 0, N'M', NULL, NULL, 28, 112, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '684491de-63f8-4632-90e3-36773c4e63bd', GETDATE()),
(872, N'Road Bottle Cage', N'BC-R205', N'0', N'1', NULL, 4, 3, 3.3623, 8.99, NULL, NULL, NULL, NULL, 0, N'R', NULL, NULL, 28, 113, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '601b1fe8-d4d0-4cfb-9379-29481cc05291', GETDATE()),
(873, N'Patch Kit/8 Patches', N'PK-7098', N'0', N'1', NULL, 4, 3, 0.8565, 2.29, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 37, 114, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '36e638e4-68df-411b-930a-daad57221aa6', GETDATE()),
(874, N'Racing Socks, M', N'SO-R809-M', N'0', N'1', N'White', 4, 3, 3.3623, 8.99, N'M', NULL, NULL, NULL, 0, N'R', NULL, N'U', 23, 24, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'b9c7eb0a-1dd1-4a1d-b4c3-1dad83a8ea7e', GETDATE()),
(875, N'Racing Socks, L', N'SO-R809-L', N'0', N'1', N'White', 4, 3, 3.3623, 8.99, N'L', NULL, NULL, NULL, 0, N'R', NULL, N'U', 23, 24, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'c0a16305-74b7-4fae-b5c9-3e8bd0e44762', GETDATE()),
(876, N'Hitch Rack - 4-Bike', N'RA-H123', N'0', N'1', NULL, 4, 3, 44.88, 120, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 26, 118, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '7a0c4bbd-9679-4f59-9ebc-9daf3439a38a', GETDATE()),
(877, N'Bike Wash - Dissolver', N'CL-9009', N'0', N'1', NULL, 4, 3, 2.9733, 7.95, NULL, NULL, NULL, NULL, 0, N'S', NULL, NULL, 29, 119, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '3c40b5ad-e328-4715-88a7-ec3220f02acf', GETDATE()),
(878, N'Fender Set - Mountain', N'FE-6654', N'0', N'1', NULL, 4, 3, 8.2205, 21.98, NULL, NULL, NULL, NULL, 0, N'M', NULL, NULL, 30, 121, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'e6e76c7f-c145-4cad-a9e8-b1e4e845a2c0', GETDATE()),
(879, N'All-Purpose Bike Stand', N'ST-1401', N'0', N'1', NULL, 4, 3, 59.466, 159, NULL, NULL, NULL, NULL, 0, N'M', NULL, NULL, 27, 122, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'c7bb564b-a637-40f5-b21b-cbf7e4f713be', GETDATE()),
(880, N'Hydration Pack - 70 oz.', N'HY-1023-70', N'0', N'1', N'Silver', 4, 3, 20.5663, 54.99, N'70', NULL, NULL, NULL, 0, N'S', NULL, NULL, 32, 107, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'a99d90c0-05e2-4e44-ad90-c55c9f0784de', GETDATE()),
(881, N'Short-Sleeve Classic Jersey, S', N'SJ-0194-S', N'0', N'1', N'Yellow', 4, 3, 41.5723, 53.99, N'S', NULL, NULL, NULL, 0, N'S', NULL, N'U', 21, 32, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '05b2e20f-2399-4cb3-9b49-b28d6649c104', GETDATE()),
(882, N'Short-Sleeve Classic Jersey, M', N'SJ-0194-M', N'0', N'1', N'Yellow', 4, 3, 41.5723, 53.99, N'M', NULL, NULL, NULL, 0, N'S', NULL, N'U', 21, 32, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'bbbf003b-367d-4d71-af71-10f50b6234a0', GETDATE()),
(883, N'Short-Sleeve Classic Jersey, L', N'SJ-0194-L', N'0', N'1', N'Yellow', 4, 3, 41.5723, 53.99, N'L', NULL, NULL, NULL, 0, N'S', NULL, N'U', 21, 32, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '2d9f59b8-9f24-46eb-98ad-553e48bb9db9', GETDATE()),
(884, N'Short-Sleeve Classic Jersey, XL', N'SJ-0194-X', N'0', N'1', N'Yellow', 4, 3, 41.5723, 53.99, N'XL', NULL, NULL, NULL, 0, N'S', NULL, N'U', 21, 32, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '906d42f6-c21f-4d20-b528-02ffdb55fd1e', GETDATE()),
(885, N'HL Touring Frame - Yellow, 60', N'FR-T98Y-60', N'1', N'1', N'Yellow', 500, 375, 601.7437, 1003.91, N'60', N'CM', N'LB', 3.08, 1, N'T', N'H', N'U', 16, 7, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'c49679bd-96a9-4176-a7ed-5bc6d6444647', GETDATE()),
(886, N'LL Touring Frame - Yellow, 62', N'FR-T67Y-62', N'1', N'1', N'Yellow', 500, 375, 199.8519, 333.42, N'62', N'CM', N'LB', 3.20, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '8d4d52a6-8abc-4c05-be4d-c067faf1a64e', GETDATE()),
(887, N'HL Touring Frame - Yellow, 46', N'FR-T98Y-46', N'1', N'1', N'Yellow', 500, 375, 601.7437, 1003.91, N'46', N'CM', N'LB', 2.96, 1, N'T', N'H', N'U', 16, 7, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '137675a7-34cd-4b7a-abe1-4e0eeb79b65d', GETDATE()),
(888, N'HL Touring Frame - Yellow, 50', N'FR-T98Y-50', N'1', N'1', N'Yellow', 500, 375, 601.7437, 1003.91, N'50', N'CM', N'LB', 3.00, 1, N'T', N'H', N'U', 16, 7, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '105ec6e5-30c5-4fe3-a08b-cb324c85323d', GETDATE()),
(889, N'HL Touring Frame - Yellow, 54', N'FR-T98Y-54', N'1', N'1', N'Yellow', 500, 375, 601.7437, 1003.91, N'54', N'CM', N'LB', 3.04, 1, N'T', N'H', N'U', 16, 7, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '12b1f317-c39b-48d0-b1a7-8018c60aea53', GETDATE()),
(890, N'HL Touring Frame - Blue, 46', N'FR-T98U-46', N'1', N'1', N'Blue', 500, 375, 601.7437, 1003.91, N'46', N'CM', N'LB', 2.96, 1, N'T', N'H', N'U', 16, 7, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '8bbd3437-a58b-41a0-9503-fc14b23e7678', GETDATE()),
(891, N'HL Touring Frame - Blue, 50', N'FR-T98U-50', N'1', N'1', N'Blue', 500, 375, 601.7437, 1003.91, N'50', N'CM', N'LB', 3.00, 1, N'T', N'H', N'U', 16, 7, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'c4244f0c-abce-451b-a895-83c0e6d1f448', GETDATE()),
(892, N'HL Touring Frame - Blue, 54', N'FR-T98U-54', N'1', N'1', N'Blue', 500, 375, 601.7437, 1003.91, N'54', N'CM', N'LB', 3.04, 1, N'T', N'H', N'U', 16, 7, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'e9aae947-6bc3-4909-8937-3e1cdcec8a8f', GETDATE()),
(893, N'HL Touring Frame - Blue, 60', N'FR-T98U-60', N'1', N'1', N'Blue', 500, 375, 601.7437, 1003.91, N'60', N'CM', N'LB', 3.08, 1, N'T', N'H', N'U', 16, 7, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'b01951a4-a581-4d10-9dc2-515da180f1b8', GETDATE()),
(894, N'Rear Derailleur', N'RD-2308', N'1', N'1', N'Silver', 500, 375, 53.9282, 121.46, NULL, NULL, N'G', 215.00, 1, NULL, NULL, NULL, 9, 127, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '5ebfcf02-4e3e-443a-ad60-5aeef28dac76', GETDATE()),
(895, N'LL Touring Frame - Blue, 50', N'FR-T67U-50', N'1', N'1', N'Blue', 500, 375, 199.8519, 333.42, N'50', N'CM', N'LB', 3.10, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'b78eccca-fa88-4071-9110-410585127e46', GETDATE()),
(896, N'LL Touring Frame - Blue, 54', N'FR-T67U-54', N'1', N'1', N'Blue', 500, 375, 199.8519, 333.42, N'54', N'CM', N'LB', 3.14, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '0ff799c9-dd11-4b81-aaf7-65410017405b', GETDATE()),
(897, N'LL Touring Frame - Blue, 58', N'FR-T67U-58', N'1', N'1', N'Blue', 500, 375, 199.8519, 333.42, N'58', N'CM', N'LB', 3.16, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'ccd4fa47-7194-4bd0-909b-1fa4bd7916a7', GETDATE()),
(898, N'LL Touring Frame - Blue, 62', N'FR-T67U-62', N'1', N'1', N'Blue', 500, 375, 199.8519, 333.42, N'62', N'CM', N'LB', 3.20, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '08a211a5-dcd2-42d0-9276-64d92d4890a6', GETDATE()),
(899, N'LL Touring Frame - Yellow, 44', N'FR-T67Y-44', N'1', N'1', N'Yellow', 500, 375, 199.8519, 333.42, N'44', N'CM', N'LB', 3.02, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '109cb7bc-6ec6-4a36-85c8-55b843b2ab12', GETDATE()),
(900, N'LL Touring Frame - Yellow, 50', N'FR-T67Y-50', N'1', N'1', N'Yellow', 500, 375, 199.8519, 333.42, N'50', N'CM', N'LB', 3.10, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '285fd682-c647-49d1-b8f3-368a43c9cda0', GETDATE()),
(901, N'LL Touring Frame - Yellow, 54', N'FR-T67Y-54', N'1', N'1', N'Yellow', 500, 375, 199.8519, 333.42, N'54', N'CM', N'LB', 3.14, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '2536e3b2-d4da-49e6-965a-f612c2c8914f', GETDATE()),
(902, N'LL Touring Frame - Yellow, 58', N'FR-T67Y-58', N'1', N'1', N'Yellow', 500, 375, 199.8519, 333.42, N'58', N'CM', N'LB', 3.16, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '5d17ff1c-f50e-438f-a4e9-7c400fb762e7', GETDATE()),
(903, N'LL Touring Frame - Blue, 44', N'FR-T67U-44', N'1', N'1', N'Blue', 500, 375, 199.8519, 333.42, N'44', N'CM', N'LB', 3.02, 1, N'T', N'L', N'U', 16, 10, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'e9c17be7-f4dc-465e-ab73-c0198f37bfdd', GETDATE()),
(904, N'ML Mountain Frame-W - Silver, 40', N'FR-M63S-40', N'1', N'1', N'Silver', 500, 375, 199.3757, 364.09, N'40', N'CM', N'LB', 2.77, 1, N'M', N'M', N'W', 12, 15, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'a7dde43e-f7d5-4075-a0c1-c866ad7aa154', GETDATE()),
(905, N'ML Mountain Frame-W - Silver, 42', N'FR-M63S-42', N'1', N'1', N'Silver', 500, 375, 199.3757, 364.09, N'42', N'CM', N'LB', 2.81, 1, N'M', N'M', N'W', 12, 15, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'd4a2fcad-1e63-4ebd-863c-5a7c48d5b8d9', GETDATE()),
(906, N'ML Mountain Frame-W - Silver, 46', N'FR-M63S-46', N'1', N'1', N'Silver', 500, 375, 199.3757, 364.09, N'46', N'CM', N'LB', 2.85, 1, N'M', N'M', N'W', 12, 15, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '82025c63-7b28-412d-92c1-408e0e6ae646', GETDATE()),
(907, N'Rear Brakes', N'RB-9231', N'0', N'1', N'Silver', 500, 375, 47.286, 106.5, NULL, NULL, N'G', 317.00, 1, NULL, NULL, NULL, 6, 128, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '5946f163-93f0-4141-b17e-55d9778cc274', GETDATE()),
(908, N'LL Mountain Seat/Saddle', N'SE-M236', N'0', N'1', NULL, 500, 375, 12.0413, 27.12, NULL, NULL, NULL, NULL, 1, N'M', N'L', NULL, 15, 79, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '4dab53c5-31e7-47d6-a5a0-940f8d4dad22', GETDATE()),
(909, N'ML Mountain Seat/Saddle', N'SE-M798', N'0', N'1', NULL, 500, 375, 17.3782, 39.14, NULL, NULL, NULL, NULL, 1, N'M', N'M', NULL, 15, 80, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '30222244-0fd8-4d28-8448-f2e658bf52bd', GETDATE()),
(910, N'HL Mountain Seat/Saddle', N'SE-M940', N'0', N'1', NULL, 500, 375, 23.3722, 52.64, NULL, NULL, NULL, NULL, 1, N'M', N'H', NULL, 15, 81, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'a96a5024-87de-488a-bf81-bc0c81f6cd18', GETDATE()),
(911, N'LL Road Seat/Saddle', N'SE-R581', N'0', N'1', NULL, 500, 375, 12.0413, 27.12, NULL, NULL, NULL, NULL, 1, N'R', N'L', NULL, 15, 82, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '31b9bc62-792b-4e7a-a24d-411dc76e0271', GETDATE()),
(912, N'ML Road Seat/Saddle', N'SE-R908', N'0', N'1', NULL, 500, 375, 17.3782, 39.14, NULL, NULL, NULL, NULL, 1, N'T', N'M', NULL, 15, 83, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '49845afe-a8cc-4354-a5d4-09d35bf7fb9e', GETDATE()),
(913, N'HL Road Seat/Saddle', N'SE-R995', N'0', N'1', NULL, 500, 375, 23.3722, 52.64, NULL, NULL, NULL, NULL, 1, N'R', N'H', NULL, 15, 84, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '2befe629-4b2a-41a1-a009-df13ead69105', GETDATE()),
(914, N'LL Touring Seat/Saddle', N'SE-T312', N'0', N'1', NULL, 500, 375, 12.0413, 27.12, NULL, NULL, NULL, NULL, 1, N'T', N'L', NULL, 15, 66, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '7874a1d6-7a5b-412f-a2eb-c2f457b9603d', GETDATE()),
(915, N'ML Touring Seat/Saddle', N'SE-T762', N'0', N'1', NULL, 500, 375, 17.3782, 39.14, NULL, NULL, NULL, NULL, 1, N'T', N'M', NULL, 15, 65, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '072acb72-7796-4bd0-9bbb-6efc29ac336c', GETDATE()),
(916, N'HL Touring Seat/Saddle', N'SE-T924', N'0', N'1', NULL, 500, 375, 23.3722, 52.64, NULL, NULL, NULL, NULL, 1, N'T', N'H', NULL, 15, 67, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '0e158724-934d-4a64-991f-94fec00bdea8', GETDATE()),
(917, N'LL Mountain Frame - Silver, 42', N'FR-M21S-42', N'1', N'1', N'Silver', 500, 375, 144.5938, 264.05, N'42', N'CM', N'LB', 2.92, 1, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '37bd4d2b-346b-4c6b-8f3f-85c084282529', GETDATE()),
(918, N'LL Mountain Frame - Silver, 44', N'FR-M21S-44', N'1', N'1', N'Silver', 500, 375, 144.5938, 264.05, N'44', N'CM', N'LB', 2.96, 1, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '393a4d00-7428-41f0-a48a-af26b00e9a9c', GETDATE()),
(919, N'LL Mountain Frame - Silver, 48', N'FR-M21S-48', N'1', N'1', N'Silver', 500, 375, 144.5938, 264.05, N'48', N'CM', N'LB', 3.00, 1, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '8dfef6f2-91a8-4884-8949-b2551218b37a', GETDATE()),
(920, N'LL Mountain Frame - Silver, 52', N'FR-M21S-52', N'1', N'1', N'Silver', 500, 375, 144.5938, 264.05, N'52', N'CM', N'LB', 3.04, 1, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'f230baac-5951-4eb1-b1e8-94c2ca2b37fa', GETDATE()),
(921, N'Mountain Tire Tube', N'TT-M928', N'0', N'1', NULL, 500, 375, 1.8663, 4.99, NULL, NULL, NULL, NULL, 0, N'M', NULL, NULL, 37, 92, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '01a8c3fc-ed52-458e-a634-d5b6e2accfed', GETDATE()),
(922, N'Road Tire Tube', N'TT-R982', N'0', N'1', NULL, 500, 375, 1.4923, 3.99, NULL, NULL, NULL, NULL, 0, N'R', NULL, NULL, 37, 93, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'ea442bd7-f69b-42d9-aa71-95e10b648f52', GETDATE()),
(923, N'Touring Tire Tube', N'TT-T092', N'0', N'1', NULL, 500, 375, 1.8663, 4.99, NULL, NULL, NULL, NULL, 0, N'T', NULL, NULL, 37, 94, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '179fec38-cab5-4a47-bcff-31cfc9e43a3c', GETDATE()),
(924, N'LL Mountain Frame - Black, 42', N'FR-M21B-42', N'1', N'1', N'Black', 500, 375, 136.785, 249.79, N'42', N'CM', N'LB', 2.92, 1, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'faabd7fb-cb35-4bad-8857-ec71468686ad', GETDATE()),
(925, N'LL Mountain Frame - Black, 44', N'FR-M21B-44', N'1', N'1', N'Black', 500, 375, 136.785, 249.79, N'44', N'CM', N'LB', 2.96, 1, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '47ab0300-7b55-4d35-a786-80190976b9b5', GETDATE()),
(926, N'LL Mountain Frame - Black, 48', N'FR-M21B-48', N'1', N'1', N'Black', 500, 375, 136.785, 249.79, N'48', N'CM', N'LB', 3.00, 1, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '408435aa-15c0-41e5-981f-32a8226af15f', GETDATE()),
(927, N'LL Mountain Frame - Black, 52', N'FR-M21B-52', N'1', N'1', N'Black', 500, 375, 136.785, 249.79, N'52', N'CM', N'LB', 3.04, 1, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '4800f4e6-99ea-4afd-a392-2cb05265d0d4', GETDATE()),
(928, N'LL Mountain Tire', N'TI-M267', N'0', N'1', NULL, 500, 375, 9.3463, 24.99, NULL, NULL, NULL, NULL, 0, N'M', N'L', NULL, 37, 85, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '76060a93-949c-48ea-9b31-a593d6c14983', GETDATE()),
(929, N'ML Mountain Tire', N'TI-M602', N'0', N'1', NULL, 500, 375, 11.2163, 29.99, NULL, NULL, NULL, NULL, 0, N'M', N'M', NULL, 37, 86, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'daff9e11-6254-432d-8c4f-f06e52687184', GETDATE()),
(930, N'HL Mountain Tire', N'TI-M823', N'0', N'1', NULL, 500, 375, 13.09, 35, NULL, NULL, NULL, NULL, 0, N'M', N'H', NULL, 37, 87, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'ddad25f5-0445-4b5c-8466-6446930ad8b8', GETDATE()),
(931, N'LL Road Tire', N'TI-R092', N'0', N'1', NULL, 500, 375, 8.0373, 21.49, NULL, NULL, NULL, NULL, 0, N'R', N'L', NULL, 37, 88, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '15b569a6-d172-42c2-a420-62ab5946cc80', GETDATE()),
(932, N'ML Road Tire', N'TI-R628', N'0', N'1', NULL, 500, 375, 9.3463, 24.99, NULL, NULL, NULL, NULL, 0, N'R', N'M', NULL, 37, 89, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'd1016cc5-c12b-4f05-915c-70fa062e4a64', GETDATE()),
(933, N'HL Road Tire', N'TI-R982', N'0', N'1', NULL, 500, 375, 12.1924, 32.6, NULL, NULL, NULL, NULL, 0, N'R', N'H', NULL, 37, 90, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'c86de56a-5048-4b89-b7c7-56bc75c9bcee', GETDATE()),
(934, N'Touring Tire', N'TI-T723', N'0', N'1', NULL, 500, 375, 10.8423, 28.99, NULL, NULL, NULL, NULL, 0, N'T', NULL, NULL, 37, 91, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '9d5ca300-5302-41e1-bca5-8ce5b93f26a5', GETDATE()),
(935, N'LL Mountain Pedal', N'PD-M282', N'0', N'1', N'Silver/Black', 500, 375, 17.9776, 40.49, NULL, NULL, N'G', 218.00, 1, N'M', N'L', NULL, 13, 62, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '9fdd0c65-b2b0-4c6c-8704-3a9747be5174', GETDATE()),
(936, N'ML Mountain Pedal', N'PD-M340', N'0', N'1', N'Silver/Black', 500, 375, 27.568, 62.09, NULL, NULL, N'G', 215.00, 1, N'M', N'M', NULL, 13, 63, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '274c86dc-439e-4469-9de8-7e9bd6455d0d', GETDATE()),
(937, N'HL Mountain Pedal', N'PD-M562', N'0', N'1', N'Silver/Black', 500, 375, 35.9596, 80.99, NULL, NULL, N'G', 185.00, 1, N'M', N'H', NULL, 13, 64, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'a05464e8-6b4d-42b3-a4d6-8683136f4b66', GETDATE()),
(938, N'LL Road Pedal', N'PD-R347', N'0', N'1', N'Silver/Black', 500, 375, 17.9776, 40.49, NULL, NULL, N'G', 189.00, 1, N'R', N'L', NULL, 13, 68, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '2c7dd8c3-4c55-475f-ba58-f4baca520d72', GETDATE()),
(939, N'ML Road Pedal', N'PD-R563', N'0', N'1', N'Silver/Black', 500, 375, 27.568, 62.09, NULL, NULL, N'G', 168.00, 1, N'R', N'M', NULL, 13, 69, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '216ad46f-bc3f-4862-b0e9-2e261e5a6059', GETDATE()),
(940, N'HL Road Pedal', N'PD-R853', N'0', N'1', N'Silver/Black', 500, 375, 35.9596, 80.99, NULL, NULL, N'G', 149.00, 1, N'R', N'H', NULL, 13, 70, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '44e96967-ab99-41ed-8b41-5bc70a5ca1a9', GETDATE()),
(941, N'Touring Pedal', N'PD-T852', N'0', N'1', N'Silver/Black', 500, 375, 35.9596, 80.99, NULL, NULL, NULL, NULL, 1, N'T', NULL, NULL, 13, 53, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '6967c816-3ebb-45fa-8547-cef00e08573e', GETDATE()),
(942, N'ML Mountain Frame-W - Silver, 38', N'FR-M63S-38', N'1', N'1', N'Silver', 500, 375, 199.3757, 364.09, N'38', N'CM', N'LB', 2.73, 2, N'M', N'M', N'W', 12, 15, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'ba3646b0-1487-426e-ab4e-57d42e6f9233', GETDATE()),
(943, N'LL Mountain Frame - Black, 40', N'FR-M21B-40', N'1', N'1', N'Black', 500, 375, 136.785, 249.79, N'40', N'CM', N'LB', 2.88, 2, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '3050e4df-2bba-4c2b-bdcc-b4c89972db1c', GETDATE()),
(944, N'LL Mountain Frame - Silver, 40', N'FR-M21S-40', N'1', N'1', N'Silver', 500, 375, 144.5938, 264.05, N'40', N'CM', N'LB', 2.88, 2, N'M', N'L', N'U', 12, 8, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '756f862e-40cc-4dfc-b189-716d0dda5ff9', GETDATE()),
(945, N'Front Derailleur', N'FD-2342', N'1', N'1', N'Silver', 500, 375, 40.6216, 91.49, NULL, NULL, N'G', 88.00, 1, NULL, NULL, NULL, 9, 103, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '448e9e7b-9548-4a4c-abb3-853686aa7517', GETDATE()),
(946, N'LL Touring Handlebars', N'HB-T721', N'1', N'1', NULL, 500, 375, 20.464, 46.09, NULL, NULL, NULL, NULL, 1, N'T', N'L', NULL, 4, 47, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'a2f1352e-45d0-42c4-aef3-60033073bb66', GETDATE()),
(947, N'HL Touring Handlebars', N'HB-T928', N'1', N'1', NULL, 500, 375, 40.6571, 91.57, NULL, NULL, NULL, NULL, 1, N'T', N'H', NULL, 4, 48, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'cb524e92-4fa8-4f6c-9993-60796856c654', GETDATE()),
(948, N'Front Brakes', N'FB-9873', N'0', N'1', N'Silver', 500, 375, 47.286, 106.5, NULL, NULL, N'G', 317.00, 1, NULL, NULL, NULL, 6, 102, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'c1813164-1b4b-42d1-9007-4e5f9aee0e19', GETDATE()),
(949, N'LL Crankset', N'CS-4759', N'1', N'1', N'Black', 500, 375, 77.9176, 175.49, NULL, NULL, N'G', 600.00, 1, NULL, N'L', NULL, 8, 99, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '41d47371-4374-46d3-8d61-b07616ec54f0', GETDATE()),
(950, N'ML Crankset', N'CS-6583', N'1', N'1', N'Black', 500, 375, 113.8816, 256.49, NULL, NULL, N'G', 635.00, 1, NULL, N'M', NULL, 8, 100, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'd3a7a02c-a3d5-4a04-9454-0c4e43772b78', GETDATE()),
(951, N'HL Crankset', N'CS-9183', N'1', N'1', N'Black', 500, 375, 179.8156, 404.99, NULL, NULL, N'G', 575.00, 1, NULL, N'H', NULL, 8, 101, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '2c4a8956-7b72-48fe-b028-699e117b1daa', GETDATE()),
(952, N'Chain', N'CH-0234', N'0', N'1', N'Silver', 500, 375, 8.9866, 20.24, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, 7, 98, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '5d27e2a5-27ec-4ccb-ba2c-fc980ffe6708', GETDATE()),
(953, N'Touring-2000 Blue, 60', N'BK-T44U-60', N'1', N'1', N'Blue', 100, 75, 755.1508, 1214.85, N'60', N'CM', N'LB', 27.90, 4, N'T', N'M', N'U', 3, 35, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'f1bb3957-8d27-47f3-91ec-c71822d11436', GETDATE()),
(954, N'Touring-1000 Yellow, 46', N'BK-T79Y-46', N'1', N'1', N'Yellow', 100, 75, 1481.9379, 2384.07, N'46', N'CM', N'LB', 25.13, 4, N'T', N'H', N'U', 3, 34, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '83b77413-8c8a-4af1-93e4-136edb7ff15f', GETDATE()),
(955, N'Touring-1000 Yellow, 50', N'BK-T79Y-50', N'1', N'1', N'Yellow', 100, 75, 1481.9379, 2384.07, N'50', N'CM', N'LB', 25.42, 4, N'T', N'H', N'U', 3, 34, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '5ec991ad-8761-4a61-a318-312df3a78604', GETDATE()),
(956, N'Touring-1000 Yellow, 54', N'BK-T79Y-54', N'1', N'1', N'Yellow', 100, 75, 1481.9379, 2384.07, N'54', N'CM', N'LB', 25.68, 4, N'T', N'H', N'U', 3, 34, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '1220b0f0-064d-46b7-8507-1fa758b77b9c', GETDATE()),
(957, N'Touring-1000 Yellow, 60', N'BK-T79Y-60', N'1', N'1', N'Yellow', 100, 75, 1481.9379, 2384.07, N'60', N'CM', N'LB', 25.90, 4, N'T', N'H', N'U', 3, 34, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'bcd1a5a9-6140-4dc9-9620-689dc7e4c155', GETDATE()),
(958, N'Touring-3000 Blue, 54', N'BK-T18U-54', N'1', N'1', N'Blue', 100, 75, 461.4448, 742.35, N'54', N'CM', N'LB', 29.68, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'a3ee6897-52fe-42e4-92ec-7a91e7bb905a', GETDATE()),
(959, N'Touring-3000 Blue, 58', N'BK-T18U-58', N'1', N'1', N'Blue', 100, 75, 461.4448, 742.35, N'58', N'CM', N'LB', 29.90, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '23d89cee-9f44-4f3e-b289-63de6ba2b737', GETDATE()),
(960, N'Touring-3000 Blue, 62', N'BK-T18U-62', N'1', N'1', N'Blue', 100, 75, 461.4448, 742.35, N'62', N'CM', N'LB', 30.00, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '060192c9-bcd9-4260-b729-d6bcfadfb08e', GETDATE()),
(961, N'Touring-3000 Yellow, 44', N'BK-T18Y-44', N'1', N'1', N'Yellow', 100, 75, 461.4448, 742.35, N'44', N'CM', N'LB', 28.77, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '5646c15a-68ad-4234-b328-254706cbccc5', GETDATE()),
(962, N'Touring-3000 Yellow, 50', N'BK-T18Y-50', N'1', N'1', N'Yellow', 100, 75, 461.4448, 742.35, N'50', N'CM', N'LB', 29.13, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'df85e805-af87-4fab-a668-c80f2a5b8a69', GETDATE()),
(963, N'Touring-3000 Yellow, 54', N'BK-T18Y-54', N'1', N'1', N'Yellow', 100, 75, 461.4448, 742.35, N'54', N'CM', N'LB', 29.42, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '192becd1-f465-4194-88a2-ee57fed3a3c5', GETDATE()),
(964, N'Touring-3000 Yellow, 58', N'BK-T18Y-58', N'1', N'1', N'Yellow', 100, 75, 461.4448, 742.35, N'58', N'CM', N'LB', 29.79, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'bed79f64-a53d-44a3-ace8-2baa425a5a54', GETDATE()),
(965, N'Touring-3000 Yellow, 62', N'BK-T18Y-62', N'1', N'1', N'Yellow', 100, 75, 461.4448, 742.35, N'62', N'CM', N'LB', 30.00, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'd28b3872-5173-40a4-b12f-655524386cc7', GETDATE()),
(966, N'Touring-1000 Blue, 46', N'BK-T79U-46', N'1', N'1', N'Blue', 100, 75, 1481.9379, 2384.07, N'46', N'CM', N'LB', 25.13, 4, N'T', N'H', N'U', 3, 34, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '86990d54-6efb-4c53-9974-6c3b0297e222', GETDATE()),
(967, N'Touring-1000 Blue, 50', N'BK-T79U-50', N'1', N'1', N'Blue', 100, 75, 1481.9379, 2384.07, N'50', N'CM', N'LB', 25.42, 4, N'T', N'H', N'U', 3, 34, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '68c0a818-2985-46eb-8255-0fb70919fa24', GETDATE()),
(968, N'Touring-1000 Blue, 54', N'BK-T79U-54', N'1', N'1', N'Blue', 100, 75, 1481.9379, 2384.07, N'54', N'CM', N'LB', 25.68, 4, N'T', N'H', N'U', 3, 34, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '12280a8c-7578-4367-ba71-214bcc1e4792', GETDATE()),
(969, N'Touring-1000 Blue, 60', N'BK-T79U-60', N'1', N'1', N'Blue', 100, 75, 1481.9379, 2384.07, N'60', N'CM', N'LB', 25.90, 4, N'T', N'H', N'U', 3, 34, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'dd70cf36-449a-43fd-839d-a84ee14c849a', GETDATE()),
(970, N'Touring-2000 Blue, 46', N'BK-T44U-46', N'1', N'1', N'Blue', 100, 75, 755.1508, 1214.85, N'46', N'CM', N'LB', 27.13, 4, N'T', N'M', N'U', 3, 35, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'c0009006-715f-4b76-a05a-1a0d3adfb49a', GETDATE()),
(971, N'Touring-2000 Blue, 50', N'BK-T44U-50', N'1', N'1', N'Blue', 100, 75, 755.1508, 1214.85, N'50', N'CM', N'LB', 27.42, 4, N'T', N'M', N'U', 3, 35, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '84abda8f-0007-4bca-9a61-b2dea58866c3', GETDATE()),
(972, N'Touring-2000 Blue, 54', N'BK-T44U-54', N'1', N'1', N'Blue', 100, 75, 755.1508, 1214.85, N'54', N'CM', N'LB', 27.68, 4, N'T', N'M', N'U', 3, 35, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '6dcfe2a3-3555-44e4-9116-6f6dbe448b8b', GETDATE()),
(973, N'Road-350-W Yellow, 40', N'BK-R79Y-40', N'1', N'1', N'Yellow', 100, 75, 1082.51, 1700.99, N'40', N'CM', N'LB', 15.35, 4, N'R', N'M', N'W', 2, 27, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '237b16d9-53f2-4fd4-befe-48209e57aec3', GETDATE()),
(974, N'Road-350-W Yellow, 42', N'BK-R79Y-42', N'1', N'1', N'Yellow', 100, 75, 1082.51, 1700.99, N'42', N'CM', N'LB', 15.77, 4, N'R', N'M', N'W', 2, 27, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '80bd3f8b-42c7-43d8-91f5-9fb6175287af', GETDATE()),
(975, N'Road-350-W Yellow, 44', N'BK-R79Y-44', N'1', N'1', N'Yellow', 100, 75, 1082.51, 1700.99, N'44', N'CM', N'LB', 16.13, 4, N'R', N'M', N'W', 2, 27, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '0c61e8af-003d-4e4b-b5b7-02f01a26be26', GETDATE()),
(976, N'Road-350-W Yellow, 48', N'BK-R79Y-48', N'1', N'1', N'Yellow', 100, 75, 1082.51, 1700.99, N'48', N'CM', N'LB', 16.42, 4, N'R', N'M', N'W', 2, 27, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'ec4284dc-85fa-44a8-89ec-77fc9b71720a', GETDATE()),
(977, N'Road-750 Black, 58', N'BK-R19B-58', N'1', N'1', N'Black', 100, 75, 343.6496, 539.99, N'58', N'CM', N'LB', 20.79, 4, N'R', N'L', N'U', 2, 31, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '87b81a1a-a5b5-43d2-a20d-0230800490b9', GETDATE()),
(978, N'Touring-3000 Blue, 44', N'BK-T18U-44', N'1', N'1', N'Blue', 100, 75, 461.4448, 742.35, N'44', N'CM', N'LB', 28.77, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '4f638e15-2ed0-4193-90ce-47da580e01dd', GETDATE()),
(979, N'Touring-3000 Blue, 50', N'BK-T18U-50', N'1', N'1', N'Blue', 100, 75, 461.4448, 742.35, N'50', N'CM', N'LB', 29.13, 4, N'T', N'L', N'U', 3, 36, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'de305b62-88fc-465b-b23d-d8c0f7e6d361', GETDATE()),
(980, N'Mountain-400-W Silver, 38', N'BK-M38S-38', N'1', N'1', N'Silver', 100, 75, 419.7784, 769.49, N'38', N'CM', N'LB', 26.35, 4, N'M', N'M', N'W', 1, 22, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '7a927632-99a4-4f24-adce-0062d2d113d9', GETDATE()),
(981, N'Mountain-400-W Silver, 40', N'BK-M38S-40', N'1', N'1', N'Silver', 100, 75, 419.7784, 769.49, N'40', N'CM', N'LB', 26.77, 4, N'M', N'M', N'W', 1, 22, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '0a72791c-a984-4733-ae4e-2b4373cfd7cd', GETDATE()),
(982, N'Mountain-400-W Silver, 42', N'BK-M38S-42', N'1', N'1', N'Silver', 100, 75, 419.7784, 769.49, N'42', N'CM', N'LB', 27.13, 4, N'M', N'M', N'W', 1, 22, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '4ea4fe06-aaea-42d4-a9d9-69f6a9a4a042', GETDATE()),
(983, N'Mountain-400-W Silver, 46', N'BK-M38S-46', N'1', N'1', N'Silver', 100, 75, 419.7784, 769.49, N'46', N'CM', N'LB', 27.42, 4, N'M', N'M', N'W', 1, 22, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '0b0c8ee4-ff2d-438e-9cac-464783b86191', GETDATE()),
(984, N'Mountain-500 Silver, 40', N'BK-M18S-40', N'1', N'1', N'Silver', 100, 75, 308.2179, 564.99, N'40', N'CM', N'LB', 27.35, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'b96c057b-6416-4851-8d59-bcb37c8e6e51', GETDATE()),
(985, N'Mountain-500 Silver, 42', N'BK-M18S-42', N'1', N'1', N'Silver', 100, 75, 308.2179, 564.99, N'42', N'CM', N'LB', 27.77, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'b8d1b5d9-8a39-4097-a04a-56e95559b534', GETDATE()),
(986, N'Mountain-500 Silver, 44', N'BK-M18S-44', N'1', N'1', N'Silver', 100, 75, 308.2179, 564.99, N'44', N'CM', N'LB', 28.13, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'e8cec794-e8e2-4312-96a7-4832e573d3fc', GETDATE()),
(987, N'Mountain-500 Silver, 48', N'BK-M18S-48', N'1', N'1', N'Silver', 100, 75, 308.2179, 564.99, N'48', N'CM', N'LB', 28.42, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '77ef419f-481f-40b9-bdb9-7e6678e550e3', GETDATE()),
(988, N'Mountain-500 Silver, 52', N'BK-M18S-52', N'1', N'1', N'Silver', 100, 75, 308.2179, 564.99, N'52', N'CM', N'LB', 28.68, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'cad60945-183e-4ab3-a70c-3f5bac6bf134', GETDATE()),
(989, N'Mountain-500 Black, 40', N'BK-M18B-40', N'1', N'1', N'Black', 100, 75, 294.5797, 539.99, N'40', N'CM', N'LB', 27.35, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '28287157-3f06-487b-8531-bee8a37329e4', GETDATE()),
(990, N'Mountain-500 Black, 42', N'BK-M18B-42', N'1', N'1', N'Black', 100, 75, 294.5797, 539.99, N'42', N'CM', N'LB', 27.77, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'f5752c9c-56ba-41a7-83fd-139da28c15fa', GETDATE()),
(991, N'Mountain-500 Black, 44', N'BK-M18B-44', N'1', N'1', N'Black', 100, 75, 294.5797, 539.99, N'44', N'CM', N'LB', 28.13, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'c7852127-2fb8-4959-b5a3-de5a8d6445e4', GETDATE()),
(992, N'Mountain-500 Black, 48', N'BK-M18B-48', N'1', N'1', N'Black', 100, 75, 294.5797, 539.99, N'48', N'CM', N'LB', 28.42, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '75752e26-a3b6-4264-9b06-f23a4fbdc5a7', GETDATE()),
(993, N'Mountain-500 Black, 52', N'BK-M18B-52', N'1', N'1', N'Black', 100, 75, 294.5797, 539.99, N'52', N'CM', N'LB', 28.68, 4, N'M', N'L', N'U', 1, 23, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '69ee3b55-e142-4e4f-aed8-af02978fbe87', GETDATE()),
(994, N'LL Bottom Bracket', N'BB-7421', N'1', N'1', NULL, 500, 375, 23.9716, 53.99, NULL, NULL, N'G', 223.00, 1, NULL, N'L', NULL, 5, 95, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'fa3c65cd-0a22-47e3-bdf6-53f1dc138c43', GETDATE()),
(995, N'ML Bottom Bracket', N'BB-8107', N'1', N'1', NULL, 500, 375, 44.9506, 101.24, NULL, NULL, N'G', 168.00, 1, NULL, N'M', NULL, 5, 96, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '71ab847f-d091-42d6-b735-7b0c2d82fc84', GETDATE()),
(996, N'HL Bottom Bracket', N'BB-9108', N'1', N'1', NULL, 500, 375, 53.9416, 121.49, NULL, NULL, N'G', 170.00, 1, NULL, N'H', NULL, 5, 97, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '230c47c5-08b2-4ce3-b706-69c0bdd62965', GETDATE()),
(997, N'Road-750 Black, 44', N'BK-R19B-44', N'1', N'1', N'Black', 100, 75, 343.6496, 539.99, N'44', N'CM', N'LB', 19.77, 4, N'R', N'L', N'U', 2, 31, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '44ce4802-409f-43ab-9b27-ca53421805be', GETDATE()),
(998, N'Road-750 Black, 48', N'BK-R19B-48', N'1', N'1', N'Black', 100, 75, 343.6496, 539.99, N'48', N'CM', N'LB', 20.13, 4, N'R', N'L', N'U', 2, 31, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, '3de9a212-1d49-40b6-b10a-f564d981dbde', GETDATE()),
(999, N'Road-750 Black, 52', N'BK-R19B-52', N'1', N'1', N'Black', 100, 75, 343.6496, 539.99, N'52', N'CM', N'LB', 20.42, 4, N'R', N'L', N'U', 2, 31, CONVERT(datetime, '2013-05-30 00:00:00', 120), NULL, NULL, 'ae638923-2b67-4679-b90e-abbab17dca31', GETDATE());
SET IDENTITY_INSERT [dbo].[Product] OFF;

INSERT INTO [dbo].[Members] (
    [CustomerID], [FirstName], [LastName], [Email], [Phone], [RegistrationDate]
) VALUES
(1, 'Alice', 'Cooper', 'alice.cooper@example.com', '1112223333', '2022-06-01'),
(2, 'Bob', 'Marley', 'bob.marley@example.com', '4445556666', '2022-06-02'),
(3, 'Charlie', 'Brown', 'charlie.brown@example.com', '7778889999', '2022-06-03'),
(4, 'David', 'Bowie', 'david.bowie@example.com', '1234567890', '2022-06-04'),
(5, 'Eve', 'Jackson', 'eve.jackson@example.com', '9876543210', '2022-06-05'),
(6, 'Frank', 'Sinatra', 'frank.sinatra@example.com', '5555555555', '2022-06-06'),
(7, 'Grace', 'Kelly', 'grace.kelly@example.com', '1112223333', '2022-06-07'),
(8, 'Harry', 'Potter', 'harry.potter@example.com', '4445556666', '2022-06-08'),
(9, 'Ivy', 'Smith', 'ivy.smith@example.com', '7778889999', '2022-06-09'),
(10, 'Jack', 'Nicholson', 'jack.nicholson@example.com', '1234567890', '2022-06-10'),
(11, 'Kate', 'Winslet', 'kate.winslet@example.com', '9876543210', '2022-06-11'),
(12, 'Leo', 'DiCaprio', 'leo.dicaprio@example.com', '5555555555', '2022-06-12'),
(13, 'Meryl', 'Streep', 'meryl.streep@example.com', '1112223333', '2022-06-13'),
(14, 'Natalie', 'Portman', 'natalie.portman@example.com', '4445556666', '2022-06-14'),
(15, 'Oprah', 'Winfrey', 'oprah.winfrey@example.com', '7778889999', '2022-06-15'),
(16, 'Paul', 'Newman', 'paul.newman@example.com', '1234567890', '2022-06-16'),
(17, 'Queen', 'Elizabeth', 'queen.elizabeth@example.com', '9876543210', '2022-06-17'),
(18, 'Robert', 'DeNiro', 'robert.deniro@example.com', '5555555555', '2022-06-18'),
(19, 'Scarlett', 'Johansson', 'scarlett.johansson@example.com', '1112223333', '2022-06-19'),
(20, 'Tom', 'Hanks', 'tom.hanks@example.com', '4445556666', '2022-06-20');


INSERT INTO [dbo].[Shopping] (
    [OrderID], [CustomerID], [OrderDate], [TotalAmount], [Status]
) VALUES
    (1, 1, '2022-01-10', 100.00, 'Delivered'),
    (2, 2, '2022-02-20', 75.50, 'Shipped'),
    (3, 3, '2022-03-25', 200.75, 'Processing'),
    (4, 4, '2022-04-15', 50.25, 'Pending'),
    (5, 5, '2022-05-30', 125.00, 'Cancelled'),
    (6, 6, '2022-06-05', 80.00, 'Delivered'),
    (7, 7, '2022-06-10', 90.00, 'Refund Requested'),
    (8, 8, '2022-07-01', 120.50, 'Delivered'),
    (9, 9, '2022-07-15', 65.75, 'Shipped'),
    (10, 10, '2022-08-10', 180.25, 'Processing'),
    (11, 11, '2022-08-20', 95.00, 'Pending'),
    (12, 12, '2022-09-05', 110.50, 'Cancelled'),
    (13, 13, '2022-09-15', 75.25, 'Delivered'),
    (14, 14, '2022-10-01', 85.00, 'Shipped'),
    (15, 15, '2022-10-10', 95.75, 'Processing'),
    (16, 16, '2022-11-01', 60.50, 'Pending'),
    (17, 17, '2022-11-15', 125.25, 'Cancelled'),
    (18, 18, '2022-12-01', 90.00, 'Delivered'),
    (19, 19, '2022-12-10', 105.50, 'Shipped'),
    (20, 1, '2022-12-20', 80.75, 'Processing'),
    (21, 2, '2023-01-05', 65.25, 'Pending'),
    (22, 3, '2023-01-15', 110.00, 'Cancelled'),
    (23, 4, '2023-02-01', 95.50, 'Refund Requested'),
    (24, 5, '2023-02-10', 85.75, 'Shipped'),
    (25, 6, '2023-03-01', 120.25, 'Processing'),
    (26, 7, '2023-03-15', 75.00, 'Pending'),
    (27, 8, '2023-04-01', 105.50, 'Cancelled'),
    (28, 9, '2023-04-10', 90.25, 'Delivered'),
    (29, 10, '2023-05-01', 80.00, 'Shipped'),
    (30, 11, '2023-05-15', 95.75, 'Processing'),
    (31, 12, '2023-06-01', 65.50, 'Pending'),
    (32, 13, '2023-06-10', 110.25, 'Cancelled'),
    (33, 14, '2023-07-01', 85.00, 'Delivered'),
    (34, 15, '2023-07-15', 75.50, 'Shipped'),
    (35, 16, '2023-08-01', 100.75, 'Processing'),
    (36, 17, '2023-08-10', 60.25, 'Pending'),
    (37, 18, '2023-09-01', 125.00, 'Cancelled'),
    (38, 19, '2023-09-15', 90.50, 'Refund Requested'),
    (39, 1, '2023-10-01', 105.75, 'Shipped'),
    (40, 2, '2023-10-10', 80.25, 'Processing'),
    (41, 3, '2023-11-01', 95.00, 'Pending'),
    (42, 4, '2023-11-15', 110.50, 'Cancelled'),
    (43, 5, '2023-12-01', 85.25, 'Delivered'),
    (44, 6, '2023-12-10', 75.00, 'Shipped'),
    (45, 7, '2024-01-01', 100.75, 'Processing'),
    (46, 8, '2024-01-15', 65.50, 'Pending'),
    (47, 9, '2024-02-01', 120.25, 'Cancelled'),
    (48, 10, '2024-02-10', 90.00, 'Delivered'),
    (49, 11, '2024-03-01', 105.50, 'Shipped'),
    (50, 12, '2024-03-15', 80.75, 'Processing'),
    (51, 13, '2024-04-01', 95.25, 'Pending'),
    (52, 14, '2024-04-10', 110.00, 'Cancelled'),
    (53, 15, '2024-05-01', 85.50, 'Delivered'),
    (54, 16, '2024-05-15', 75.75, 'Shipped'),
    (55, 17, '2024-06-01', 100.25, 'Processing'),
    (56, 18, '2024-06-10', 60.00, 'Pending'),
    (57, 19, '2024-07-01', 125.50, 'Cancelled'),
    (58, 1, '2024-07-15', 90.25, 'Delivered'),
    (59, 2, '2024-08-01', 105.00, 'Shipped'),
    (60, 3, '2024-08-10', 80.75, 'Processing'),
    (61, 4, '2024-09-01', 95.50, 'Pending'),
    (62, 5, '2024-09-15', 110.25, 'Cancelled'),
    (63, 6, '2024-10-01', 85.00, 'Delivered'),
    (64, 7, '2024-10-10', 75.50, 'Shipped'),
    (65, 8, '2024-11-01', 100.75, 'Processing'),
    (66, 9, '2024-11-15', 65.25, 'Pending'),
    (67, 10, '2024-12-01', 120.00, 'Cancelled'),
    (68, 11, '2024-12-10', 90.50, 'Delivered'),
    (69, 12, '2025-01-01', 105.75, 'Shipped'),
    (70, 13, '2025-01-15', 80.25, 'Processing'),
    (71, 14, '2025-02-01', 95.00, 'Pending'),
    (72, 15, '2025-02-10', 110.50, 'Cancelled'),
    (73, 16, '2025-03-01', 85.25, 'Delivered'),
    (74, 17, '2025-03-15', 75.00, 'Shipped'),
    (75, 18, '2025-04-01', 100.75, 'Processing'),
    (76, 19, '2025-04-10', 65.50, 'Pending'),
    (77, 1, '2025-05-01', 120.25, 'Cancelled'),
    (78, 2, '2025-05-15', 90.00, 'Delivered'),
    (79, 3, '2025-06-01', 105.50, 'Shipped'),
    (80, 4, '2025-06-10', 80.75, 'Processing'),
    (81, 5, '2025-07-01', 95.25, 'Pending'),
    (82, 6, '2025-07-15', 110.00, 'Cancelled'),
    (83, 7, '2025-08-01', 85.50, 'Delivered'),
    (84, 8, '2025-08-10', 75.75, 'Shipped'),
    (85, 9, '2025-09-01', 100.25, 'Processing'),
    (86, 10, '2025-09-15', 60.00, 'Pending'),
    (87, 11, '2025-10-01', 125.50, 'Cancelled'),
    (88, 12, '2025-10-10', 90.25, 'Delivered'),
    (89, 13, '2025-11-01', 105.00, 'Shipped'),
    (90, 14, '2025-11-15', 80.75, 'Processing'),
    (91, 15, '2025-12-01', 95.50, 'Pending'),
    (92, 16, '2025-12-10', 110.25, 'Cancelled'),
    (93, 17, '2026-01-01', 85.00, 'Delivered'),
    (94, 18, '2026-01-15', 75.50, 'Shipped'),
    (95, 19, '2026-02-01', 100.75, 'Processing');

INSERT INTO [dbo].[Coupons] (
    [TicketID], [CustomerID], [OrderID], [IssueDescription], [CreatedDate], [ResolvedDate], [Status], [OSUser]
) VALUES 
(1, 1, 1, 'Item was damaged during shipping', '2022-01-15', '2022-01-20', 'Closed', CURRENT_USER),
(2, 2, 2, 'Order not received yet', '2022-02-25', NULL, 'Open', CURRENT_USER),
(3, 3, 3, 'Wrong item shipped', '2022-03-28', '2022-04-01', 'Closed', CURRENT_USER),
(4, 4, 4, 'Order placed by mistake', '2022-04-16', NULL, 'Open', CURRENT_USER),
(5, 5, 5, 'Change of mind', '2022-05-31', '2022-06-02', 'Closed', CURRENT_USER),
(6, 6, 6, 'Missing parts', '2022-06-10', NULL, 'Open', CURRENT_USER),
(7, 7, 7, 'Defective product', '2022-07-05', '2022-07-12', 'Closed', CURRENT_USER),
(8, 8, 8, 'Late delivery', '2022-08-22', NULL, 'Open', CURRENT_USER),
(9, 9, 9, 'Incorrect order', '2022-09-03', '2022-09-08', 'Closed', CURRENT_USER),
(10, 10, 10, 'Damaged packaging', '2022-10-18', NULL, 'Open', CURRENT_USER),
(11, 11, 11, 'Missing accessories', '2022-11-25', '2022-12-02', 'Closed', CURRENT_USER),
(12, 12, 12, 'Delivery address issue', '2022-12-15', NULL, 'Open', CURRENT_USER),
(13, 13, 13, 'Product not as described', '2023-01-10', NULL, 'Open', CURRENT_USER),
(14, 14, 14, 'Item out of stock', '2023-02-03', '2023-02-08', 'Closed', CURRENT_USER),
(15, 15, 15, 'Billing error', '2023-03-20', NULL, 'Open', CURRENT_USER),
(16, 16, 16, 'Warranty claim', '2023-04-12', NULL, 'Open', CURRENT_USER),
(17, 17, 17, 'Refund request', '2023-05-05', '2023-05-10', 'Closed', CURRENT_USER),
(18, 18, 18, 'Exchange request', '2023-06-22', NULL, 'Open', CURRENT_USER),
(19, 19, 19, 'Delivery delay', '2023-07-15', NULL, 'Open', CURRENT_USER),
(20, 10, 20, 'Technical support needed', '2023-08-08', '2023-08-12', 'Closed', CURRENT_USER),
(21, 11, 21, 'Product compatibility issue', '2023-09-25', NULL, 'Open', CURRENT_USER),
(22, 12, 22, 'Incorrect pricing', '2023-10-18', '2023-10-22', 'Closed', CURRENT_USER),
(23, 13, 23, 'Defective software', '2023-11-10', NULL, 'Open', CURRENT_USER),
(24, 14, 24, 'Shipping cost dispute', '2023-12-03', '2023-12-08', 'Closed', CURRENT_USER),
(25, 15, 25, 'Return policy issue', '2024-01-20', NULL, 'Open', CURRENT_USER),
(26, 16, 26, 'Damaged during transit', '2024-02-12', '2024-02-18', 'Closed', CURRENT_USER),
(27, 17, 27, 'Incorrect quantity received', '2024-03-05', NULL, 'Open', CURRENT_USER),
(28, 18, 28, 'Unauthorized purchase', '2024-04-22', '2024-04-27', 'Closed', CURRENT_USER),
(29, 19, 29, 'Subscription cancellation', '2024-05-15', NULL, 'Open', CURRENT_USER),
(30, 10, 30, 'Installation assistance needed', '2024-06-08', '2024-06-13', 'Closed', CURRENT_USER),
(31, 1, 31, 'Warranty expired', '2024-07-25', NULL, 'Open', CURRENT_USER),
(32, 2, 32, 'Product recall', '2024-08-18', '2024-08-23', 'Closed', CURRENT_USER),
(33, 3, 33, 'Delivery to wrong address', '2024-09-10', NULL, 'Open', CURRENT_USER),
(34, 4, 34, 'Duplicate order', '2024-10-03', '2024-10-08', 'Closed', CURRENT_USER),
(35, 5, 35, 'Missing user manual', '2024-11-20', NULL, 'Open', CURRENT_USER),
(36, 6, 36, 'Damaged during assembly', '2024-12-13', '2024-12-18', 'Closed', CURRENT_USER),
(37, 7, 37, 'Incorrect model received', '2025-01-05', NULL, 'Open', CURRENT_USER),
(38, 8, 38, 'Payment issue', '2025-02-22', '2025-02-27', 'Closed', CURRENT_USER),
(39, 9, 39, 'Missing parts for installation', '2025-03-17', NULL, 'Open', CURRENT_USER),
(40, 10, 40, 'Product defect', '2025-04-09', '2025-04-14', 'Closed', CURRENT_USER),
(41, 11, 41, 'Delayed shipping', '2025-05-26', NULL, 'Open', CURRENT_USER),
(42, 12, 42, 'Accessory not included', '2025-06-19', '2025-06-24', 'Closed', CURRENT_USER),
(43, 13, 43, 'Incorrect color received', '2025-07-12', NULL, 'Open', CURRENT_USER),
(44, 14, 44, 'Damaged during installation', '2025-08-04', '2025-08-09', 'Closed', CURRENT_USER),
(45, 15, 45, 'Refund processing delay', '2025-09-21', NULL, 'Open', CURRENT_USER),
(46, 16, 46, 'Product compatibility question', '2025-10-14', '2025-10-19', 'Closed', CURRENT_USER),
(47, 17, 47, 'Incorrect size received', '2025-11-06', NULL, 'Open', CURRENT_USER),
(48, 18, 48, 'Unauthorized charge', '2025-12-23', '2025-12-28', 'Closed', CURRENT_USER),
(49, 19, 49, 'Delivery address change', '2026-01-16', NULL, 'Open', CURRENT_USER),
(50, 10, 50, 'Missing promotional item', '2026-02-08', '2026-02-13', 'Closed', CURRENT_USER),
(51, 1, 51, 'Incorrect product description', '2026-03-27', NULL, 'Open', CURRENT_USER),
(52, 2, 52, 'Damaged during return', '2026-04-19', '2026-04-24', 'Closed', CURRENT_USER),
(53, 3, 53, 'Unsatisfactory customer service', '2026-05-12', NULL, 'Open', CURRENT_USER),
(54, 4, 54, 'Product discontinued', '2026-06-04', '2026-06-09', 'Closed', CURRENT_USER);

INSERT INTO [dbo].[ProductSaleRegions] (
    [RegionID], [RegionName]
) VALUES
    ('1', 'North America'),
    ('2', 'Europe'),
    ('3', 'Asia'),
    ('4', 'South America'),
    ('5', 'Africa'),
    ('6', 'Australia'),
    ('7', 'Middle East'),
    ('8', 'Central America'),
    ('9', 'Eastern Europe'),
    ('10', 'Western Europe');
GO

INSERT INTO [dbo].[ProductSales] (
    [SaleID], [ProductID], [SaleDate], [Quantity], [UnitPrice], [TotalAmount], [CustomerID], [SalePrice], [RegionID]
) VALUES
    ('334', '229', '2004-07-24', 3, 399.99, 3142.85, 385, 416.03, 8),
    ('335', '219', '2023-08-24', 1, 1299.73, 1267.45, 336, 1425.85, 5),
    ('336', '247', '2024-09-01', 3, 339.99, 336.62, 362, 361.74, 8),
    ('337', '257', '2024-06-20', 2, 84.95, 733.41, 233, 83.68, 4),
    ('338', '218', '2024-09-26', 9, 1388.89, 8497.62, 748, 1459.98, 1),
    ('339', '220', '2023-05-15', 4, 899.99, 3599.96, 400, 912.00, 2),
    ('340', '221', '2024-02-28', 2, 1199.99, 2399.98, 401, 1224.00, 3),
    ('341', '222', '2022-11-30', 6, 499.99, 2999.94, 402, 528.00, 4),
    ('342', '223', '2023-09-01', 3, 699.99, 2099.97, 403, 720.00, 5),
    ('343', '224', '2024-04-15', 5, 399.99, 1999.95, 404, 416.00, 6),
    ('344', '225', '2022-12-25', 8, 249.99, 1999.92, 405, 264.00, 7),
    ('345', '226', '2023-07-10', 7, 349.99, 2449.93, 406, 368.00, 8),
    ('346', '227', '2024-03-20', 4, 599.99, 2399.96, 407, 624.00, 9),
    ('347', '228', '2022-10-05', 9, 199.99, 1799.91, 408, 216.00, 10),
    ('348', '229', '2023-06-18', 6, 399.99, 2399.94, 409, 416.00, 1);


