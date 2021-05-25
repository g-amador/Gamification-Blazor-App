# SETUP DATABASE

## Setup Migration

Before migration you have to verify the database connection, this will be found in appsettings.json file as shown below,

```
"ConnectionStrings": {
	"GamificationAPI": "Server=(localdb)\\mssqllocaldb;Database=GamificationAPI-79145ad7-6d46-4195-b012-1f5e06acb6c2;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

The above connection string was created when we create the Entity Framework Database Context. You can also use your own SQL database instead of LocalDb. 

To enable migrations, Click Tools -> NuGet Package Manager -> Package Manager Console,

Run this command, 

```Add-Migration Initial```


## Create a Database

You have to execute below command in Package Manager Console to create a database,

```Update-Database```

To Open SQL Server Object Explorer, Click View -> SQL Server Object Explorer, You can now see the GamificationAPI-79145ad7-6d46-4195-b012-1f5e06acb6c2 
database and Model corresponding tables.

Any changes to the data model, you should use the 

```Add-Migration MigrationName```

and 

```Update-Database```

commands to push changes to the database.


# RESET IDENTITY SEED AFTER DELETING RECORDS IN SQL SERVER

```sql
USE [GamificationAPIContext-5251a309-b240-48e2-917b-99c922ac67d1]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DBCC CHECKIDENT ('[dbo].[Application]', RESEED, 0);
GO
```
