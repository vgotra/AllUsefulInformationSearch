# README for Database Migration Scripts

## How to create a new migration script

Just create 2 files in this folder: one for migration and one for rollback

## Naming convention

- Migration Apply: DDDD_Migration_MigrationName.sql
- Migration Rollback: DDDD_Migration_MigrationName.Rollback.sql

**Notes**: DDDD just digits. Please reuse useful numeration with step 5 or 10 to have a possibility to include new scripts between existing ones (in case of fixes).

## Notes

Please don't forget to reuse **IF NOT EXISTS** for all objects (tables, indexes, etc.) to avoid errors during the rollback process.
Also don't forget to use SCHEMA NAME for every database object.