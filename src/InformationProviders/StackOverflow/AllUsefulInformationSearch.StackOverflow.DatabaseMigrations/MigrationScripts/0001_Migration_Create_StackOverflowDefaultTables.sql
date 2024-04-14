CREATE SCHEMA IF NOT EXISTS StackOverflow;

-- create table for storing settings
CREATE TABLE IF NOT EXISTS StackOverflow.Settings
(
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Value TEXT NOT NULL,
    LastUpdated TIMESTAMP WITH TIME ZONE NOT NULL
);

-- create table for storing web data files information
CREATE TABLE IF NOT EXISTS StackOverflow.WebDataFiles
(
    Id UUID PRIMARY KEY,
    Name TEXT NOT NULL,
    Link TEXT NOT NULL,
    Size BIGINT NOT NULL,
    LastModified TIMESTAMP WITH TIME ZONE NOT NULL,
    ProcessingStatus INT NOT NULL,
    LastUpdated TIMESTAMP WITH TIME ZONE NOT NULL
);

COMMENT ON COLUMN StackOverflow.WebDataFiles.LastModified IS 'Datetime of last modified of the web data file at StackOverflow archive.';
COMMENT ON COLUMN StackOverflow.WebDataFiles.ProcessingStatus IS 'Processing status of file based on enumeration in source code.';
COMMENT ON COLUMN StackOverflow.WebDataFiles.LastUpdated IS 'Timestamp of when the data file entity was last updated in the database.';