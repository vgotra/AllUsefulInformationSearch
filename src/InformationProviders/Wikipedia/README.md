# Prototype for Parsing and Importing Wikipedia Archive Data

## Components

- **Auis.Wikipedia.App** - executing app for parsing and importing Wikipedia archive data into a database

- **Auis.Wikipedia.BusinessLogic** - business logic layer

- **Auis.Wikipedia.Common** - common classes, interfaces, constants, defaults, anything that can be shared between different layers

- **Auis.Wikipedia.DataAccess** - data access layer for Wikipedia archive data

- **Auis.Wikipedia.DatabaseMigrations** - database migrations

- **Auis.Wikipedia.Models** - models (request/response, entities, xml, etc)

- **Auis.Wikipedia.Tests** - tests (unit/integration/e2e)

- **Auis.Wikipedia.WebApi** - REST (at current moment, but can be changed) API

## How to run

- Execute Auis.Wikipedia.App.exe file
  - If arguments are not provided, the app will process all files
  ```shell
  Auis.Wikipedia.App.exe
  ```
  
## Notes

- the app is not optimized for performance, but it can be done later

- the app is not optimized for memory usage, but it can be done later
  
- mostly all options are specified in **appsettings.json** (some hardcodes will be refactored later)