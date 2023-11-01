# Timesheets Application

## About This Project:

The Timesheets Application is a tool designed to help your buisness efficiently manage and keep track of work hours for multiple employees accross multiple applications, similtanously. This is done with a windows application that creates and updates a local SQLite database with timesheet information. Then, while the user is connected to the internet, the application will upload that timehsheet information to a secure remote MS SQL server 2014 database that is located on your domain, using APIs. Furthermore, the program is structured to eliminate the chance of overwriting data or merge conflicts by using GUID's and properly structuring the merge and syncing functions.

![Timesheet](https://github.com/zkonings/Timesheets-Application/assets/148987384/acb9d9cc-9159-43b9-aa1e-373c91224bef)

## Built With:

- **Language:** VB.NET, ASP.NET (API's hosted on serverside, not included in repo)
- **Framework:** .NET Framework 4.7.2
- **Database:** local SQLite, MS SQL Server 2014 (remote secure, accessed by API)
- **Other Technologies:** Windows userform application

## Basic Workflow:
![Timesheet API Database Workflow Graph](https://github.com/zkonings/Timesheets-Application/assets/148987384/860bc5ec-1027-4482-b7ee-3c321b73561e)

## Database:
The application makes use of SQLite database that is packaged with the program. The application will check to see if a local database has been created and if not, creates one. All changes to data are pushed to the local database as to prevent data loss during wavering connectivity to the internet. From there, a connection to a remote secure MS SQL 2014 database is created using API's and a synchronization process is performed to keep all records up to date.

## Synchronization:
The local SQLite database stores data with a unique identifier number [GUID] and a timestamp. Depending on the time of the timestamp will depend on if the data is pushed to the remote database or the other way around.

The program checks the GUID and Timestamp for all records on both databases. If a GUID is missing, it gets added to the database. If the GUID exists and has a different timestamp, the databases are updated to have the newest information. 

##Deleting Records:
A "Deleted" column is added to each record of type Boolean, as to keep all records available to view if needed. By default, only records with the value = 0 is shown. However, a check box is available to show previously deleted records.

##Backing Up Databases:
Since our remote server uses a third party back-up service for disaster recovery and since all local machines should have a copy of the database that is updated each time they are online, I decided to not include any code to back-up the databases any further.


## Getting Started:



## Prerequisites:



## Usage:



## Contact:

If you have any questions or suggestions, feel free to reach out:

- **Email:** zachkonings@gmail.com



