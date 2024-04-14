CREATE SCHEMA IF NOT EXISTS StackOverflow;

-- create table for storing settings
CREATE TABLE IF NOT EXISTS StackOverflow.SettingEntity
(
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Value TEXT NOT NULL,
    LastUpdated TIMESTAMP WITH TIME ZONE NOT NULL
);