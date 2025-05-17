# Prototype for Parsing and Importing StackOverflow Archive Data

## Components

- **Auis.StackOverflow.App** - executing app for parsing and importing StackOverflow archive data into a database
- **Auis.StackOverflow.BusinessLogic** - business logic layer
- **Auis.StackOverflow.Common** - common classes, interfaces, constants, defaults, anything that can be shared between different layers
- **Auis.StackOverflow.DataAccess** - data access layer for StackOverflow archive data
- **Auis.StackOverflow.DatabaseMigrations** - database migrations
- **Auis.StackOverflow.Models** - models (request/response, entities, xml, etc)
- **Auis.StackOverflow.Tests** - tests (unit/integration/e2e)

## How to run

- Execute Auis.StackOverflow.App.exe with/without specifying a file to process
  ```shell
  Auis.StackOverflow.App.exe filename.7z 
  ```
  
## Notes

- the app is not optimized for performance, but it can be done later
- the app is not optimized for memory usage, but it can be done later
- mostly all options are specified in **appsettings.json** (some hardcodes will be refactored later)
- archives of StackOverflow, even archived, take over 90 Gb of harddisk space, but after processing, in db, it will take ~20 Gb (it can be changed later)