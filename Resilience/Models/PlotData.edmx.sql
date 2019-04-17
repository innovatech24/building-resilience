
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/18/2019 00:00:47
-- Generated from EDMX file: C:\Users\kiran\Desktop\IE\repos\Resilience\Resilience\Models\PlotData.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PlotData];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Regions'
CREATE TABLE [dbo].[Regions] (
    [Region_Id] int IDENTITY(1,1) NOT NULL,
    [Region_Name] nvarchar(max)  NOT NULL,
    [Area] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Bullyings'
CREATE TABLE [dbo].[Bullyings] (
    [Bullying_Id] int IDENTITY(1,1) NOT NULL,
    [Year] nvarchar(max)  NOT NULL,
    [Numerator] nvarchar(max)  NOT NULL,
    [Denominator] nvarchar(max)  NOT NULL,
    [Indicator] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Behaviours'
CREATE TABLE [dbo].[Behaviours] (
    [Behaviour_Id] int IDENTITY(1,1) NOT NULL,
    [Year] nvarchar(max)  NOT NULL,
    [Numerator] nvarchar(max)  NOT NULL,
    [Denominator] nvarchar(max)  NOT NULL,
    [Indicator] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FamilyViolences'
CREATE TABLE [dbo].[FamilyViolences] (
    [Violence_Id] int IDENTITY(1,1) NOT NULL,
    [Year] nvarchar(max)  NOT NULL,
    [Numerator] nvarchar(max)  NOT NULL,
    [Denominator] nvarchar(max)  NOT NULL,
    [Indicator] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FamilyStresses'
CREATE TABLE [dbo].[FamilyStresses] (
    [Stress_Id] int IDENTITY(1,1) NOT NULL,
    [Year] nvarchar(max)  NOT NULL,
    [Numerator] nvarchar(max)  NOT NULL,
    [Denominator] nvarchar(max)  NOT NULL,
    [Indicator] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RegionBullying'
CREATE TABLE [dbo].[RegionBullying] (
    [Regions_Region_Id] int  NOT NULL,
    [Bullyings_Bullying_Id] int  NOT NULL
);
GO

-- Creating table 'RegionBehaviour'
CREATE TABLE [dbo].[RegionBehaviour] (
    [Regions_Region_Id] int  NOT NULL,
    [Behaviours_Behaviour_Id] int  NOT NULL
);
GO

-- Creating table 'RegionFamilyStress'
CREATE TABLE [dbo].[RegionFamilyStress] (
    [Regions_Region_Id] int  NOT NULL,
    [FamilyStresses_Stress_Id] int  NOT NULL
);
GO

-- Creating table 'RegionFamilyViolence'
CREATE TABLE [dbo].[RegionFamilyViolence] (
    [Regions_Region_Id] int  NOT NULL,
    [FamilyViolences_Violence_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Region_Id] in table 'Regions'
ALTER TABLE [dbo].[Regions]
ADD CONSTRAINT [PK_Regions]
    PRIMARY KEY CLUSTERED ([Region_Id] ASC);
GO

-- Creating primary key on [Bullying_Id] in table 'Bullyings'
ALTER TABLE [dbo].[Bullyings]
ADD CONSTRAINT [PK_Bullyings]
    PRIMARY KEY CLUSTERED ([Bullying_Id] ASC);
GO

-- Creating primary key on [Behaviour_Id] in table 'Behaviours'
ALTER TABLE [dbo].[Behaviours]
ADD CONSTRAINT [PK_Behaviours]
    PRIMARY KEY CLUSTERED ([Behaviour_Id] ASC);
GO

-- Creating primary key on [Violence_Id] in table 'FamilyViolences'
ALTER TABLE [dbo].[FamilyViolences]
ADD CONSTRAINT [PK_FamilyViolences]
    PRIMARY KEY CLUSTERED ([Violence_Id] ASC);
GO

-- Creating primary key on [Stress_Id] in table 'FamilyStresses'
ALTER TABLE [dbo].[FamilyStresses]
ADD CONSTRAINT [PK_FamilyStresses]
    PRIMARY KEY CLUSTERED ([Stress_Id] ASC);
GO

-- Creating primary key on [Regions_Region_Id], [Bullyings_Bullying_Id] in table 'RegionBullying'
ALTER TABLE [dbo].[RegionBullying]
ADD CONSTRAINT [PK_RegionBullying]
    PRIMARY KEY CLUSTERED ([Regions_Region_Id], [Bullyings_Bullying_Id] ASC);
GO

-- Creating primary key on [Regions_Region_Id], [Behaviours_Behaviour_Id] in table 'RegionBehaviour'
ALTER TABLE [dbo].[RegionBehaviour]
ADD CONSTRAINT [PK_RegionBehaviour]
    PRIMARY KEY CLUSTERED ([Regions_Region_Id], [Behaviours_Behaviour_Id] ASC);
GO

-- Creating primary key on [Regions_Region_Id], [FamilyStresses_Stress_Id] in table 'RegionFamilyStress'
ALTER TABLE [dbo].[RegionFamilyStress]
ADD CONSTRAINT [PK_RegionFamilyStress]
    PRIMARY KEY CLUSTERED ([Regions_Region_Id], [FamilyStresses_Stress_Id] ASC);
GO

-- Creating primary key on [Regions_Region_Id], [FamilyViolences_Violence_Id] in table 'RegionFamilyViolence'
ALTER TABLE [dbo].[RegionFamilyViolence]
ADD CONSTRAINT [PK_RegionFamilyViolence]
    PRIMARY KEY CLUSTERED ([Regions_Region_Id], [FamilyViolences_Violence_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Regions_Region_Id] in table 'RegionBullying'
ALTER TABLE [dbo].[RegionBullying]
ADD CONSTRAINT [FK_RegionBullying_Region]
    FOREIGN KEY ([Regions_Region_Id])
    REFERENCES [dbo].[Regions]
        ([Region_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Bullyings_Bullying_Id] in table 'RegionBullying'
ALTER TABLE [dbo].[RegionBullying]
ADD CONSTRAINT [FK_RegionBullying_Bullying]
    FOREIGN KEY ([Bullyings_Bullying_Id])
    REFERENCES [dbo].[Bullyings]
        ([Bullying_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RegionBullying_Bullying'
CREATE INDEX [IX_FK_RegionBullying_Bullying]
ON [dbo].[RegionBullying]
    ([Bullyings_Bullying_Id]);
GO

-- Creating foreign key on [Regions_Region_Id] in table 'RegionBehaviour'
ALTER TABLE [dbo].[RegionBehaviour]
ADD CONSTRAINT [FK_RegionBehaviour_Region]
    FOREIGN KEY ([Regions_Region_Id])
    REFERENCES [dbo].[Regions]
        ([Region_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Behaviours_Behaviour_Id] in table 'RegionBehaviour'
ALTER TABLE [dbo].[RegionBehaviour]
ADD CONSTRAINT [FK_RegionBehaviour_Behaviour]
    FOREIGN KEY ([Behaviours_Behaviour_Id])
    REFERENCES [dbo].[Behaviours]
        ([Behaviour_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RegionBehaviour_Behaviour'
CREATE INDEX [IX_FK_RegionBehaviour_Behaviour]
ON [dbo].[RegionBehaviour]
    ([Behaviours_Behaviour_Id]);
GO

-- Creating foreign key on [Regions_Region_Id] in table 'RegionFamilyStress'
ALTER TABLE [dbo].[RegionFamilyStress]
ADD CONSTRAINT [FK_RegionFamilyStress_Region]
    FOREIGN KEY ([Regions_Region_Id])
    REFERENCES [dbo].[Regions]
        ([Region_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [FamilyStresses_Stress_Id] in table 'RegionFamilyStress'
ALTER TABLE [dbo].[RegionFamilyStress]
ADD CONSTRAINT [FK_RegionFamilyStress_FamilyStress]
    FOREIGN KEY ([FamilyStresses_Stress_Id])
    REFERENCES [dbo].[FamilyStresses]
        ([Stress_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RegionFamilyStress_FamilyStress'
CREATE INDEX [IX_FK_RegionFamilyStress_FamilyStress]
ON [dbo].[RegionFamilyStress]
    ([FamilyStresses_Stress_Id]);
GO

-- Creating foreign key on [Regions_Region_Id] in table 'RegionFamilyViolence'
ALTER TABLE [dbo].[RegionFamilyViolence]
ADD CONSTRAINT [FK_RegionFamilyViolence_Region]
    FOREIGN KEY ([Regions_Region_Id])
    REFERENCES [dbo].[Regions]
        ([Region_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [FamilyViolences_Violence_Id] in table 'RegionFamilyViolence'
ALTER TABLE [dbo].[RegionFamilyViolence]
ADD CONSTRAINT [FK_RegionFamilyViolence_FamilyViolence]
    FOREIGN KEY ([FamilyViolences_Violence_Id])
    REFERENCES [dbo].[FamilyViolences]
        ([Violence_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RegionFamilyViolence_FamilyViolence'
CREATE INDEX [IX_FK_RegionFamilyViolence_FamilyViolence]
ON [dbo].[RegionFamilyViolence]
    ([FamilyViolences_Violence_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------