# SQuan.Helpers.SQLiteSpatial

The `SQuan.Helpers.SQLiteSpatial` package combines sqlite-net-pcl with NetTopologySuite to enable SQLite spatial support in .NET applications.

## SQLiteSpatial Example

```c#
SQLiteSpatialConnection db = new(":memory:");
double area_50_units = db.ExecuteScalar<double>("SELECT ST_Area('POLYGON((10 10,20 10,20 20,10 10))')");
string? centroid_at_50_50 = db.ExecuteScalar<string>("SELECT ST_Centroid('POLYGON((10 10,20 10,20 20,10 10))')");
string? circle_buffer = db.ExecuteScalar<string>("SELECT ST_Buffer('POINT(10 10)', 5)");
```

## Further information

For more information please visit:

 - Documentation: https://github.com/stephenquan/SQuan.Helpers.Maui/wiki
 - GitHub repository: https://github.com/stephenquan/SQuan.Helpers.Maui
