/*============================================================================
  File:     mnsg-adven.sql

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
  
  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
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