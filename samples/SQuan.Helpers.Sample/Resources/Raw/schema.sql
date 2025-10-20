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
    Geometry TEXT NULL
);

CREATE INDEX idx_UsaStates_Name ON UsaStates(Name);

CREATE TABLE UsaCities
(
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Color TEXT NOT NULL,
    Geometry TEXT NULL
);

CREATE INDEX idx_UsaCities ON UsaCities(Name);

-- Create the R*Tree index table.
CREATE VIRTUAL TABLE UsaStates_rtree USING rtree(Id, XMin, YMin, XMax, YMax);
CREATE VIRTUAL TABLE UsaCities_rtree USING rtree(Id, XMin, YMin, XMax, YMax);

-- Create the triggers needed to keep the R*Tree indexes in sync.
CREATE TRIGGER UsaStates_insert AFTER INSERT ON UsaStates
BEGIN
    INSERT INTO UsaStates_rtree (Id, XMin, YMin, XMax, YMax)
    VALUES (
        NEW.Id,
        ST_XMin(NEW.Geometry),
        ST_YMin(NEW.Geometry),
        ST_XMax(NEW.Geometry),
        ST_YMax(NEW.Geometry));
END;

CREATE TRIGGER UsaStates_update AFTER UPDATE OF geometry ON UsaStates
BEGIN
    UPDATE UsaStates_rtree
    SET XMin = ST_XMin(NEW.Geometry),
        XMax = ST_XMax(NEW.Geometry),
        YMin = ST_YMin(NEW.Geometry),
        YMax = ST_YMax(NEW.Geometry)
    WHERE Id = NEW.Id;
END;

CREATE TRIGGER UsaStates_delete AFTER DELETE ON UsaStates
BEGIN
    DELETE FROM UsaStates_rtree WHERE Id = OLD.Id;
END;

CREATE TRIGGER UsaCities_insert AFTER INSERT ON UsaCities
BEGIN
    INSERT INTO UsaCities_rtree (Id, XMin, YMin, XMax, YMax)
    VALUES (
        NEW.Id,
        ST_XMin(NEW.Geometry),
        ST_YMin(NEW.Geometry),
        ST_XMax(NEW.Geometry),
        ST_YMax(NEW.Geometry));
END;

CREATE TRIGGER UsaCities_update AFTER UPDATE OF geometry ON UsaCities
BEGIN
    UPDATE UsaCities_rtree
    SET XMin = ST_XMin(NEW.Geometry),
        XMax = ST_XMax(NEW.Geometry),
        YMin = ST_YMin(NEW.Geometry),
        YMax = ST_YMax(NEW.Geometry)
    WHERE Id = NEW.Id;
END;

CREATE TRIGGER UsaCities_delete AFTER DELETE ON UsaCities
BEGIN
    DELETE FROM UsaCities_rtree WHERE Id = OLD.Id;
END;
