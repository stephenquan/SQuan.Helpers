# SQLiteNetSpatialConnection Class

The SQLiteNetSpatialConnection class is a wrapper for [sqlite-pcl-net](https://www.nuget.org/packages/sqlite-net-pcl) SQLite.SQLiteConnection with spatial functions added.

## C# Namespace

```c#
using SQuan.Helpers.SQLite.Spatial;
```
## Constructor

```c#
 SQLiteNetSpatialConnection(
    string databasePath,
    SQLiteOpenFlags openFlags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | FullMutex,
    bool storeDataTimeAsTicks = true)
```
