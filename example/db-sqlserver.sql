 

-- ----------------------------
-- Table structure for school
-- ----------------------------
DROP TABLE [dbo].[school]
GO
CREATE TABLE [dbo].[school] (
[Id] int NULL ,
[SchoolId] nvarchar(MAX) NULL ,
[Name] nvarchar(MAX) NULL 
)


GO

-- ----------------------------
-- Table structure for student
-- ----------------------------
DROP TABLE [dbo].[student]
GO
CREATE TABLE [dbo].[student] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Type] varchar(255) NULL ,
[IsDel] bit NULL ,
[Status] int NULL ,
[LongValue] bigint NULL ,
[FloatValue] varchar(255) NULL ,
[DecimalValue] varchar(255) NULL ,
[Body] varchar(255) NULL ,
[DateCreated] varchar(255) NULL ,
[SchoolId] varchar(255) NULL 
)


GO
ALTER TABLE [dbo].[student] SET (LOCK_ESCALATION = AUTO)
GO

-- ----------------------------
-- Indexes structure for table student
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table student
-- ----------------------------
ALTER TABLE [dbo].[student] ADD PRIMARY KEY ([Id])
GO
