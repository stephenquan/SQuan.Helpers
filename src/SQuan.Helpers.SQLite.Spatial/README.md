# SQuan.Helpers.SQLite.Spatial

The [SQuan.Helpers.SQLite.Spatial](https://www.nuget.org/packages/SQuan.Helpers.SQLite.Spatial) brings together [sqlite-net-pcl](https://www.nuget.org/packages/sqlite-net-pcl) and [NetTopologySuite](https://www.nuget.org/packages/NetTopologySuite) to enable spatial capabilities in SQLite for .NET applications. It uses EWKB (Extended Well-Known Binary) for geometry values. Use `ST_GeomFromText` and `ST_AsText` at WKT boundaries. Its spatial functions are loosely inspired by those in [PostGIS](https://postgis.net/docs/manual-1.5/ch08.html).

## Namespace

```c#
using SQuan.Helpers.SQLite.Spatial;
```

## Example

```c#
// Create an in-memory SQLite database with spatial support.
SQLiteSpatialConnection db = new(":memory:");

// Example spatial queries
double area_50_units = db.ExecuteScalar<double>("SELECT ST_Area(ST_GeomFromText('POLYGON((10 10,20 10,20 20,10 10))'))");
string? centroid_at_5_5 = db.ExecuteScalar<string>("SELECT ST_AsText(ST_Centroid(ST_GeomFromText('POLYGON ((0 0, 0 10, 10 10, 10 0, 0 0))')))");
string? circle_buffer = db.ExecuteScalar<string>("SELECT ST_AsText(ST_Buffer(ST_GeomFromText('POINT(10 10)'), 5))");
double? distance_5_units = db.ExecuteScalar<double?>("SELECT ST_Distance(ST_GeomFromText('POINT(0 0)'), ST_GeomFromText('POINT(3 4)'))");
double? area_100_units = db.ExecuteScalar<double?>("SELECT ST_Area(ST_Envelope(ST_Buffer(ST_GeomFromText('POINT(10 10)'), 5)))");

// Retrieve cities in order of distance, starting with those nearest to Los Angeles.
var results = db.Query<SpatialData>("SELECT * FROM UsaCities ORDER BY ST_Distance(Geometry, ST_GeomFromText('POINT(-118.243683 34.052235)'))");
foreach (var result in results)
{
    System.Diagnostics.Trace.WriteLine("City: " + result.Name);
}
```

## Further information

For more information please visit:

 - Documentation: https://github.com/stephenquan/SQuan.Helpers/wiki/SQLite.Spatial
 - GitHub repository: https://github.com/stephenquan/SQuan.Helpers
