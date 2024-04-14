# Prototype for Parsing and Importing StackOverflow Offline Data

## Components

- `WebParser` - parses StackOverflow offline data html page and getting items to download and parse.
- `Workflows` - high level abraction and implementations for workflows
- `DataBaseMigrations` - database migrations for creating tables and indexes
- `DataAccess` - data access layer for working with database

## Libraries

- Reused Dapper for prototype, later it should be replaced with pure Ado.Net for performance reasons (also AOT).

