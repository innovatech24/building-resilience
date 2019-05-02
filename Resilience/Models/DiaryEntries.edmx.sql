
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/02/2019 13:17:21
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
IF OBJECT_ID(N'[dbo].[FK_TaskTaskAssign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskAssigns] DROP CONSTRAINT [FK_TaskTaskAssign];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersTaskAssign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskAssigns] DROP CONSTRAINT [FK_UsersTaskAssign];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[DiaryEntries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiaryEntries];
GO
IF OBJECT_ID(N'[dbo].[TaskAssigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskAssigns];
GO
IF OBJECT_ID(N'[dbo].[Tasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tasks];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int  NOT NULL,
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

-- Creating table 'ExerciseAssigns'
CREATE TABLE [dbo].[ExerciseAssigns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MentorId] int  NOT NULL,
    [DueDate] datetime  NOT NULL,
    [CompletionDate] datetime  NULL,
    [MenteeRating] int  NULL,
    [MentorFeedback] nvarchar(max)  NULL,
    [MenteeComments] nvarchar(max)  NULL,
    [TaskId] int  NOT NULL,
    [UsersId] int  NOT NULL
);
GO

-- Creating table 'Exercises'
CREATE TABLE [dbo].[Exercises] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TaskName] nvarchar(max)  NOT NULL,
    [TaskDescription] nvarchar(max)  NOT NULL,
    [MentorId] int  NULL
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

-- Creating primary key on [Id] in table 'ExerciseAssigns'
ALTER TABLE [dbo].[ExerciseAssigns]
ADD CONSTRAINT [PK_ExerciseAssigns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Exercises'
ALTER TABLE [dbo].[Exercises]
ADD CONSTRAINT [PK_Exercises]
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

-- Creating foreign key on [TaskId] in table 'ExerciseAssigns'
ALTER TABLE [dbo].[ExerciseAssigns]
ADD CONSTRAINT [FK_TaskTaskAssign]
    FOREIGN KEY ([TaskId])
    REFERENCES [dbo].[Exercises]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskTaskAssign'
CREATE INDEX [IX_FK_TaskTaskAssign]
ON [dbo].[ExerciseAssigns]
    ([TaskId]);
GO

-- Creating foreign key on [UsersId] in table 'ExerciseAssigns'
ALTER TABLE [dbo].[ExerciseAssigns]
ADD CONSTRAINT [FK_UsersTaskAssign]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersTaskAssign'
CREATE INDEX [IX_FK_UsersTaskAssign]
ON [dbo].[ExerciseAssigns]
    ([UsersId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------