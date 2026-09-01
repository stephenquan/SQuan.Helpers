# SQuan.Helpers.SQLite.Spatial

## Overview

SQuan.Helpers.SQLite.Spatial is a library that simplifies working with spatial data in SQLite databases.

It integrates popular SQLite libraries with NetTopologySuite, making it easier to store, retrieve, and query spatial information. The library handles the underlying mapping and database integration so developers can focus on their application's data and workflows.

Whether you're working with points, lines, polygons, or more complex geometries, SQuan.Helpers.SQLite.Spatial provides a consistent and convenient way to persist and query spatial data in SQLite.

## Add NuGet package

Use the NuGet Package Manager in Visual Studio to install the [SQuan.Helpers.SQLite.Spatial](https://www.nuget.org/packages/SQuan.Helpers.SQLite.Spatial) package:

1. Select Project > Manage NuGet Packages
2. On the NuGet Package Manager page, next to Package source, select nuget.org
3. Go to the Browse tab and search for [SQuan.Helpers.SQLite.Spatial](https://www.nuget.org/packages/SQuan.Helpers.SQLite.Spatial). In the list, select [SQuan.Helpers.SQLite.Spatial](https://www.nuget.org/packages/SQuan.Helpers.SQLite.Spatial), and then select Install.

## C# Namespace

```c#
using SQuan.Helpers.SQLite.Spatial;
```

## C# Classes

| Name                                 | Description |
| ------------------------------------ | ----------- |
| [SQLiteNetSpatialConnection](SQLiteNetSpatialConnection/index.md) | SQLite.SQLiteConnection with spatial functions
| [MicrosoftDataSqliteSpatialConnection](MicrosoftDataSqliteSpatialConnection/index.md) | Microsoft.Data.Sqlite.SqliteConnection with spatial functions |
| [ST](ST/index.md) | Provides C# spatial function |
