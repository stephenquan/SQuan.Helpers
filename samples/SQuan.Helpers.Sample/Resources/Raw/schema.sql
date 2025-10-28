-- schema.sql

-- Drop existing tables and triggers if they exist
DROP TABLE IF EXISTS UsaStates;
DROP TABLE IF EXISTS UsaCities;
DROP TABLE IF EXISTS UsaStates_rtree;
DROP TABLE IF EXISTS UsaCities_rtree;
DROP TRIGGER IF EXISTS UsaStates_insert;
DROP TRIGGER IF EXISTS UsaStates_update;
DROP TRIGGER IF EXISTS UsaStates_delete;
DROP TRIGGER IF EXISTS UsaCities_insert;
DROP TRIGGER IF EXISTS UsaCities_update;
DROP TRIGGER IF EXISTS UsaCities_delete;

-- Create the tables.
CREATE TABLE UsaStates
(
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Color TEXT NOT NULL,
    WKT TEXT NULL
);

CREATE INDEX idx_UsaStates_Name ON UsaStates(Name);

CREATE TABLE UsaCities
(
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Color TEXT NOT NULL,
    WKT TEXT NULL
);

CREATE INDEX idx_UsaCities ON UsaCities(Name);

-- Create the R*Tree index table.
CREATE VIRTUAL TABLE UsaStates_rtree USING rtree(Id, MinX, MaxX, MinY, MaxY);
CREATE VIRTUAL TABLE UsaCities_rtree USING rtree(Id, MinX, MaxX, MinY, MaxY);

-- Create the triggers needed to keep the R*Tree indexes in sync.
CREATE TRIGGER UsaStates_insert AFTER INSERT ON UsaStates
BEGIN
    INSERT INTO UsaStates_rtree (Id, MinX, MaxX, MinY, MaxY)
    VALUES (
        NEW.Id,
        ST_MinX(NEW.WKT),
        ST_MaxX(NEW.WKT),
        ST_MinY(NEW.WKT),
        ST_MaxY(NEW.WKT));
END;

CREATE TRIGGER UsaStates_update AFTER UPDATE OF WKT ON UsaStates
BEGIN
    UPDATE UsaStates_rtree
    SET MinX = ST_MinX(NEW.WKT),
        MaxX = ST_MaxX(NEW.WKT),
        MinY = ST_MinY(NEW.WKT),
        MaxY = ST_MaxY(NEW.WKT)
    WHERE Id = NEW.Id;
END;

CREATE TRIGGER UsaStates_delete AFTER DELETE ON UsaStates
BEGIN
    DELETE FROM UsaStates_rtree WHERE Id = OLD.Id;
END;

CREATE TRIGGER UsaCities_insert AFTER INSERT ON UsaCities
BEGIN
    INSERT INTO UsaCities_rtree (Id, MinX, MaxX, MinY, MaxY)
    VALUES (
        NEW.Id,
        ST_MinX(NEW.WKT),
        ST_MaxX(NEW.WKT),
        ST_MinY(NEW.WKT),
        ST_MaxY(NEW.WKT));
END;

CREATE TRIGGER UsaCities_update AFTER UPDATE OF WKT ON UsaCities
BEGIN
    UPDATE UsaCities_rtree
    SET MinX = ST_MinX(NEW.WKT),
        MaxX = ST_MaxX(NEW.WKT),
        MinY = ST_MinY(NEW.WKT),
        MaxY = ST_MaxY(NEW.WKT)
    WHERE Id = NEW.Id;
END;

CREATE TRIGGER UsaCities_delete AFTER DELETE ON UsaCities
BEGIN
    DELETE FROM UsaCities_rtree WHERE Id = OLD.Id;
END;
