
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/25/2019 19:02:17
-- Generated from EDMX file: C:\Users\kiran\Desktop\IE\repos\Resilience\Resilience\Models\DiaryEntries.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DiaryEntries];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UsersDiaryEntries]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DiaryEntries] DROP CONSTRAINT [FK_UsersDiaryEntries];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DiaryEntries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiaryEntries];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [EmailAddress] nvarchar(max)  NOT NULL,
    [MentorId] int  NULL
);
GO

-- Creating table 'DiaryEntries'
CREATE TABLE [dbo].[DiaryEntries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Entry] nvarchar(max)  NOT NULL,
    [UsersId] int  NOT NULL,
    [MentorId] int  NOT NULL,
    [SentimentScore] float  NULL,
    [MentorFeedback] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [MenteeFeedback] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DiaryEntries'
ALTER TABLE [dbo].[DiaryEntries]
ADD CONSTRAINT [PK_DiaryEntries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UsersId] in table 'DiaryEntries'
ALTER TABLE [dbo].[DiaryEntries]
ADD CONSTRAINT [FK_UsersDiaryEntries]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersDiaryEntries'
CREATE INDEX [IX_FK_UsersDiaryEntries]
ON [dbo].[DiaryEntries]
    ([UsersId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------