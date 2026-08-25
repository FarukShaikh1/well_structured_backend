-- ============================================================
-- V006 — Family Relationship Graph Module
-- Tables: FamilyPersons, FamilyRelationships
-- SPs:    FamilyPerson_Search, FamilyPerson_Add, FamilyPerson_Update,
--         FamilyRelationship_Add, FamilyRelationship_Delete,
--         FamilyPerson_GetGraph
-- ============================================================

-- ── Tables ────────────────────────────────────────────────────────────────────

CREATE TABLE FamilyPersons (
    PersonId         UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    FirstName        NVARCHAR(100)    NOT NULL,
    LastName         NVARCHAR(100)    NOT NULL,
    -- Persisted computed column lets us index and search FullName efficiently
    FullName         AS (FirstName + ' ' + LastName) PERSISTED,
    DateOfBirth      DATE             NULL,
    Gender           CHAR(1)          NULL CHECK (Gender IN ('M','F','O')),
    Notes            NVARCHAR(1000)   NULL,
    ProfileImagePath NVARCHAR(500)    NULL,
    -- Optional soft-link to an app user account
    LinkedUserId     UNIQUEIDENTIFIER NULL,
    CreatedBy        UNIQUEIDENTIFIER NOT NULL,
    CreatedOn        DATETIME2(0)     NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy        UNIQUEIDENTIFIER NULL,
    UpdatedOn        DATETIME2(0)     NULL,
    IsActive         BIT              NOT NULL DEFAULT 1,
    CONSTRAINT PK_FamilyPersons PRIMARY KEY (PersonId)
);

CREATE INDEX IX_FamilyPersons_FullName  ON FamilyPersons (FullName)    WHERE IsActive = 1;
CREATE INDEX IX_FamilyPersons_FirstName ON FamilyPersons (FirstName)   WHERE IsActive = 1;
CREATE INDEX IX_FamilyPersons_LastName  ON FamilyPersons (LastName)    WHERE IsActive = 1;

CREATE TABLE FamilyRelationships (
    RelationshipId   UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    PersonId         UNIQUEIDENTIFIER NOT NULL,  -- "from" person
    RelatedPersonId  UNIQUEIDENTIFIER NOT NULL,  -- "to" person
    -- Allowed values: Father | Mother | Child | Sibling | Spouse
    RelationshipType NVARCHAR(50)     NOT NULL,
    CreatedBy        UNIQUEIDENTIFIER NOT NULL,
    CreatedOn        DATETIME2(0)     NOT NULL DEFAULT GETUTCDATE(),
    IsActive         BIT              NOT NULL DEFAULT 1,
    CONSTRAINT PK_FamilyRelationships    PRIMARY KEY (RelationshipId),
    CONSTRAINT FK_FR_PersonId            FOREIGN KEY (PersonId)        REFERENCES FamilyPersons (PersonId),
    CONSTRAINT FK_FR_RelatedPersonId     FOREIGN KEY (RelatedPersonId) REFERENCES FamilyPersons (PersonId),
    CONSTRAINT CK_FR_NoSelfRelationship  CHECK (PersonId <> RelatedPersonId),
    -- Prevent exact duplicate edges
    CONSTRAINT UQ_FR_Pair_Type           UNIQUE (PersonId, RelatedPersonId, RelationshipType)
);

CREATE INDEX IX_FR_PersonId        ON FamilyRelationships (PersonId)        WHERE IsActive = 1;
CREATE INDEX IX_FR_RelatedPersonId ON FamilyRelationships (RelatedPersonId) WHERE IsActive = 1;

GO

-- ── Stored Procedures ─────────────────────────────────────────────────────────

CREATE OR ALTER PROCEDURE FamilyPerson_Search
    @Name NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 20
        PersonId,
        FirstName,
        LastName,
        FullName,
        DateOfBirth,
        Gender,
        ProfileImagePath
    FROM  FamilyPersons
    WHERE IsActive = 1
      AND (FirstName LIKE '%' + @Name + '%'
        OR LastName  LIKE '%' + @Name + '%')
    ORDER BY FullName;
END;
GO

-- ─────────────────────────────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE FamilyPerson_Add
    @PersonId        UNIQUEIDENTIFIER,
    @FirstName       NVARCHAR(100),
    @LastName        NVARCHAR(100),
    @DateOfBirth     DATE,
    @Gender          CHAR(1),
    @Notes           NVARCHAR(1000),
    @ProfileImagePath NVARCHAR(500),
    @LinkedUserId    UNIQUEIDENTIFIER,
    @CreatedBy       UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO FamilyPersons
        (PersonId, FirstName, LastName, DateOfBirth, Gender, Notes, ProfileImagePath, LinkedUserId, CreatedBy)
    VALUES
        (@PersonId, @FirstName, @LastName, @DateOfBirth, @Gender, @Notes, @ProfileImagePath, @LinkedUserId, @CreatedBy);
END;
GO

-- ─────────────────────────────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE FamilyPerson_Update
    @PersonId        UNIQUEIDENTIFIER,
    @FirstName       NVARCHAR(100),
    @LastName        NVARCHAR(100),
    @DateOfBirth     DATE,
    @Gender          CHAR(1),
    @Notes           NVARCHAR(1000),
    @ProfileImagePath NVARCHAR(500),
    @UpdatedBy       UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE FamilyPersons
    SET  FirstName        = @FirstName,
         LastName         = @LastName,
         DateOfBirth      = @DateOfBirth,
         Gender           = @Gender,
         Notes            = @Notes,
         ProfileImagePath = @ProfileImagePath,
         UpdatedBy        = @UpdatedBy,
         UpdatedOn        = GETUTCDATE()
    WHERE PersonId = @PersonId AND IsActive = 1;
END;
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Adds a directed relationship and its automatic reverse (where applicable).
-- Bidirectional rules:
--   Father  → reverse is Child  (of the related person)
--   Mother  → reverse is Child
--   Spouse  → reverse is Spouse
--   Sibling → reverse is Sibling
--   Child   → no auto-reverse (parent gender unknown)
-- ─────────────────────────────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE FamilyRelationship_Add
    @PersonId        UNIQUEIDENTIFIER,
    @RelatedPersonId UNIQUEIDENTIFIER,
    @RelationshipType NVARCHAR(50),
    @CreatedBy       UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Forward edge
        IF NOT EXISTS (
            SELECT 1 FROM FamilyRelationships
            WHERE  PersonId         = @PersonId
              AND  RelatedPersonId  = @RelatedPersonId
              AND  RelationshipType = @RelationshipType
              AND  IsActive         = 1
        )
        BEGIN
            INSERT INTO FamilyRelationships (PersonId, RelatedPersonId, RelationshipType, CreatedBy)
            VALUES (@PersonId, @RelatedPersonId, @RelationshipType, @CreatedBy);
        END

        -- Auto reverse edge
        DECLARE @ReverseType NVARCHAR(50) = CASE @RelationshipType
            WHEN 'Father'  THEN 'Child'
            WHEN 'Mother'  THEN 'Child'
            WHEN 'Sibling' THEN 'Sibling'
            WHEN 'Spouse'  THEN 'Spouse'
            ELSE NULL
        END;

        IF @ReverseType IS NOT NULL
           AND NOT EXISTS (
               SELECT 1 FROM FamilyRelationships
               WHERE  PersonId         = @RelatedPersonId
                 AND  RelatedPersonId  = @PersonId
                 AND  RelationshipType = @ReverseType
                 AND  IsActive         = 1
           )
        BEGIN
            INSERT INTO FamilyRelationships (PersonId, RelatedPersonId, RelationshipType, CreatedBy)
            VALUES (@RelatedPersonId, @PersonId, @ReverseType, @CreatedBy);
        END

        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ─────────────────────────────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE FamilyRelationship_Delete
    @RelationshipId UNIQUEIDENTIFIER,
    @UpdatedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE FamilyRelationships
    SET IsActive = 0
    WHERE RelationshipId = @RelationshipId;
END;
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Breadth-first traversal of the relationship graph starting from @RootPersonId.
-- Uses temp tables to track visited nodes and prevent infinite loops.
-- Returns two result sets: (1) nodes (persons), (2) edges (relationships).
-- ─────────────────────────────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE FamilyPerson_GetGraph
    @RootPersonId UNIQUEIDENTIFIER,
    @MaxDepth     INT = 5
AS
BEGIN
    SET NOCOUNT ON;

    -- Visited set: each person appears only once
    CREATE TABLE #Visited (
        PersonId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Depth    INT              NOT NULL
    );
    -- BFS queue
    CREATE TABLE #Queue (
        PersonId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Depth    INT              NOT NULL
    );
    -- Staging table for newly discovered neighbours in one iteration
    DECLARE @NewNodes TABLE (PersonId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, Depth INT NOT NULL);

    INSERT INTO #Visited VALUES (@RootPersonId, 0);
    INSERT INTO #Queue   VALUES (@RootPersonId, 0);

    WHILE EXISTS (SELECT 1 FROM #Queue)
    BEGIN
        DECLARE @CurrId    UNIQUEIDENTIFIER;
        DECLARE @CurrDepth INT;

        -- Pick shallowest item first (true BFS)
        SELECT TOP 1 @CurrId = PersonId, @CurrDepth = Depth
        FROM  #Queue
        ORDER BY Depth;

        DELETE FROM #Queue WHERE PersonId = @CurrId;

        IF @CurrDepth < @MaxDepth
        BEGIN
            DELETE FROM @NewNodes;

            -- Neighbours via forward edges (A → B)
            INSERT INTO @NewNodes (PersonId, Depth)
            SELECT DISTINCT r.RelatedPersonId, @CurrDepth + 1
            FROM  FamilyRelationships r
            WHERE r.PersonId = @CurrId AND r.IsActive = 1
              AND NOT EXISTS (SELECT 1 FROM #Visited v WHERE v.PersonId = r.RelatedPersonId);

            -- Neighbours via reverse edges (B → A), not already staged
            INSERT INTO @NewNodes (PersonId, Depth)
            SELECT DISTINCT r.PersonId, @CurrDepth + 1
            FROM  FamilyRelationships r
            WHERE r.RelatedPersonId = @CurrId AND r.IsActive = 1
              AND NOT EXISTS (SELECT 1 FROM #Visited  v WHERE v.PersonId = r.PersonId)
              AND NOT EXISTS (SELECT 1 FROM @NewNodes n WHERE n.PersonId = r.PersonId);

            -- Commit newly discovered nodes
            INSERT INTO #Visited (PersonId, Depth) SELECT PersonId, Depth FROM @NewNodes;
            INSERT INTO #Queue   (PersonId, Depth) SELECT PersonId, Depth FROM @NewNodes;
        END
    END

    -- ── Result set 1: nodes ──────────────────────────────────────────────────
    SELECT
        p.PersonId,
        p.FullName           AS Label,
        p.FirstName,
        p.LastName,
        p.DateOfBirth,
        p.Gender,
        p.ProfileImagePath,
        v.Depth,
        CASE WHEN p.PersonId = @RootPersonId THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsRoot
    FROM  FamilyPersons p
    INNER JOIN #Visited v ON p.PersonId = v.PersonId
    WHERE p.IsActive = 1;

    -- ── Result set 2: edges (only between visited nodes) ────────────────────
    SELECT
        r.RelationshipId,
        r.PersonId          AS FromId,
        r.RelatedPersonId   AS ToId,
        r.RelationshipType  AS Label
    FROM  FamilyRelationships r
    INNER JOIN #Visited v1 ON r.PersonId        = v1.PersonId
    INNER JOIN #Visited v2 ON r.RelatedPersonId = v2.PersonId
    WHERE r.IsActive = 1;

    DROP TABLE #Visited;
    DROP TABLE #Queue;
END;
GO

SELECT * FROM Permissions
SELECT * FROM Modules
SELECT * FROM UserModulePermissions
SELECT * FROM [dbo].[RolePermissions]

INSERT INTO Permissions
(PermissionName, Module, Description, IsActive, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, ModuleId)
VALUES
('Family.View',		'Family','View Family',1,GETDATE(), NULL, NULL, NULL, '33C6AC71-1977-4C90-8712-B6326E039001'),
('Family.Create',	'Family','Create Family',1,GETDATE(), NULL, NULL, NULL, '33C6AC71-1977-4C90-8712-B6326E039001'),
('Family.Delete',	'Family','Delete Family',1,GETDATE(), NULL, NULL, NULL, '33C6AC71-1977-4C90-8712-B6326E039001'),
('Family.Update',	'Family','Update Family',1,GETDATE(), NULL, NULL, NULL, '33C6AC71-1977-4C90-8712-B6326E039001')

INSERT INTO [RolePermissions]
(RoleId, PermissionId, AssignedOn, AssignedBy)
SELECT '949131B8-BF7D-40CE-A02D-3D6434B78166',PermissionId,GETDATE(),'C3D0A1D1-78F3-4128-8C22-C394AD7F55E5' FROM Permissions WHERE Module = 'Family';

-- ── Sample Seed Data ──────────────────────────────────────────────────────────
DECLARE @SysUser UNIQUEIDENTIFIER = 'C3D0A1D1-78F3-4128-8C22-C394AD7F55E5'

--INSERT INTO FamilyPersons (PersonId, FirstName, LastName, DateOfBirth, Gender, CreatedBy)
--VALUES
--    ('A1000001-0000-0000-0000-000000000001', 'Faruk',   'Shaikh', '1990-07-01', 'M', @SysUser),
--    ('A1000001-0000-0000-0000-000000000002', 'Abdul Majid',   'Shaikh', '1960-03-15', 'M', @SysUser),
--    ('A1000001-0000-0000-0000-000000000003', 'Shahenaz Parveen',  'Shaikh', '1965-06-20', 'F', @SysUser),
--    ('A1000001-0000-0000-0000-000000000004', 'Arshiya Anjum',    'Khan',   '1992-11-05', 'F', @SysUser),
--    ('A1000001-0000-0000-0000-000000000005', 'Tauhid',     'Shaikh', '1988-01-22', 'M', @SysUser),
--    ('A1000001-0000-0000-0000-000000000006', 'Rasul', 'Shaikh', '1935-08-10', 'M', @SysUser),
--    ('A1000001-0000-0000-0000-000000000007', 'Amina Bi',  'Shaikh', '1940-04-30', 'F', @SysUser),
--    ('A1000001-0000-0000-0000-000000000008', 'Taimoor',   'Shaikh', '1993-03-12', 'M', @SysUser),
--    ('A1000001-0000-0000-0000-000000000009', 'Sharfana Anjum',   'Shaikh', '1993-03-12', 'F', @SysUser),
--    ('A1000001-0000-0000-0000-000000000010', 'Taslima Nasreen',   'Shaikh', '1993-03-12', 'F', @SysUser);

---- Abdul Majid is Father of Faruk  →  auto-reverse: Faruk is Child of Abdul Majid
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000002', 'A1000001-0000-0000-0000-000000000001', 'Father', @SysUser;

---- Shahenaz Parveen is Mother of Faruk  →  auto-reverse: Faruk is Child of Shahenaz Parveen
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000003', 'A1000001-0000-0000-0000-000000000001', 'Mother', @SysUser;

---- Faruk is Spouse of Arshiya Anjum   →  auto-reverse: Arshiya Anjum is Spouse of Faruk
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000001', 'A1000001-0000-0000-0000-000000000004', 'Spouse', @SysUser;

---- Faruk is Sibling of Tauhid   →  auto-reverse: Tauhid is Sibling of Faruk
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000001', 'A1000001-0000-0000-0000-000000000005', 'Sibling', @SysUser;

-- Faruk is Sibling of Tauhid   →  auto-reverse: Tauhid is Sibling of Faruk
EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000001', 'A1000001-0000-0000-0000-0000000000010', 'Sibling', @SysUser;

---- Faruk is Sibling of Tauhid   →  auto-reverse: Tauhid is Sibling of Faruk
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000005', 'A1000001-0000-0000-0000-0000000000010', 'Sibling', @SysUser;

---- Rasul is Father of Abdul Majid  →  Faruk's paternal grandfather
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000006', 'A1000001-0000-0000-0000-000000000002', 'Father', @SysUser;

---- Amina Bi is Mother of Abdul Majid  →  Faruk's paternal grandmother
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000007', 'A1000001-0000-0000-0000-000000000002', 'Mother', @SysUser;

---- Abdul Majid is Father of Tauhid (siblings share parents)
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000002', 'A1000001-0000-0000-0000-000000000005', 'Father', @SysUser;

---- Tauhid is Father of Taimoor (child)
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000005', 'A1000001-0000-0000-0000-000000000008', 'Father', @SysUser;

---- Tauhid is Father of Taimoor (child)
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000005', 'A1000001-0000-0000-0000-000000000008', 'Father', @SysUser;

---- Faruk is Spouse of Arshiya Anjum   →  auto-reverse: Arshiya Anjum is Spouse of Faruk
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000009', 'A1000001-0000-0000-0000-000000000005', 'Spouse', @SysUser;

-- Rasul is Spouse of Aminabi →  auto-reverse: Arshiya Anjum is Spouse of Faruk
--EXEC FamilyRelationship_Add 'A1000001-0000-0000-0000-000000000006', 'A1000001-0000-0000-0000-000000000007', 'Spouse', @SysUser;
