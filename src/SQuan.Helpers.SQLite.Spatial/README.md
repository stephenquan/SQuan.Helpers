# SQuan.Helpers.SQLite.Spatial

The [SQuan.Helpers.SQLite.Spatial](https://www.nuget.org/packages/SQuan.Helpers.SQLite.Spatial) brings together [sqlite-net-pcl](https://www.nuget.org/packages/sqlite-net-pcl)/[Microsoft.Data.SqLite](https://www.nuget.org/packages/Microsoft.Data.Sqlite) and [NetTopologySuite](https://www.nuget.org/packages/NetTopologySuite) to enable spatial capabilities in SQLite for .NET applications. It uses EWKB (Extended Well-Known Binary) for geometry values. Use `ST_GeomFromText` and `ST_AsText` at WKT boundaries. Its spatial functions are loosely inspired by those in [PostGIS](https://postgis.net/docs/manual-1.5/ch08.html).

## Namespace

```c#
using SQuan.Helpers.SQLite.Spatial;
```

## ST functions in C#

There is a C# static class called ST which provide static functions as well as extension methods for working with geometry stored in Extended Well-Known Binary blob (EWKB). There are marshalling functions that convert between EWKB to NetTopologySuite's Geometry class.

```c#
public static class ST
{
    static byte[] ToEWKB(this Geometry geometry, int srid = 4326);
    public static Geometry SetSRID(this Geometry geometry, int srid);
    public static Geometry ToGeometry(byte[] ewkb);
    public static double? ST_Area(this byte[] ewkb);
    public static string ST_AsText(this byte[] ewkb);
    public static byte[] ST_GeomFromText(string text, int srid = 4326);
    public static byte[] ST_Point(double x, double y, int srid = 4326);
    public static int ST_SRID(this byte[] ewkb);
    public static double? ST_X(this byte[] ewkb);
    public static double? ST_Y(this byte[] ewkb);
    public static byte[] ST_AsBinary(this byte[] ewkb);
    public static string ST_AsEWKT(this byte[] ewkb);
    public static byte[] ST_Boundary(this byte[] ewkb);
    public static byte[] ST_Buffer(this byte[] ewkb, double distance);
    public static byte[] ST_Centroid(this byte[] ewkb);
    public static bool ST_Contains(this byte[] ewkb, byte[] other);
    public static byte[] ST_ConvexHull(this byte[] ewkb);
    public static bool ST_CoveredBy(this byte[] ewkb, byte[] other);
    public static bool ST_Covers(this byte[] ewkb, byte[] other);
    public static bool ST_Crosses(this byte[] ewkb, byte[] other);
    public static int ST_Dimension(this byte[] ewkb);
    public static byte[] ST_Difference(this byte[] ewkb, byte[] other);
    public static bool ST_Disjoint(this byte[] ewkb, byte[] other);
    public static double ST_Distance(this byte[] ewkb, byte[] other);
    public static bool ST_DWithin(this byte[] ewkb, byte[] other, double distance);
    public static bool ST_Equals(this byte[] ewkb, byte[] other);
    public static byte[] ST_Envelope(this byte[] ewkb);
    public static byte[]? ST_EndPoint(this byte[] ewkb);
    public static byte[]? ST_GeometryN(this byte[] ewkb, int index);
    public static string ST_GeometryType(this byte[] ewkb);
    public static double ST_Height(this byte[] ewkb);
    public static byte[] ST_InteriorPoint(this byte[] ewkb);
    public static byte[] ST_Intersection(this byte[] ewkb, byte[] other);
    public static bool ST_Intersects(this byte[] ewkb, byte[] other);
    public static bool ST_IsClosed(this byte[] ewkb);
    public static bool ST_IsEmpty(this byte[] ewkb);
    public static bool ST_IsRectangle(this byte[] ewkb);
    public static bool ST_IsRing(this byte[] ewkb);
    public static bool ST_IsSimple(this byte[] ewkb);
    public static bool ST_IsValid(this byte[] ewkb);
    public static double ST_Length(this byte[] ewkb);
    public static double ST_MaxX(this byte[] ewkb);
    public static double ST_MaxY(this byte[] ewkb);
    public static double ST_MinX(this byte[] ewkb);
    public static double ST_MinY(this byte[] ewkb);
    public static int ST_NumGeometries(this byte[] ewkb);
    public static int ST_NumInteriorRings(this byte[] ewkb);
    public static int ST_NumPoints(this byte[] ewkb);
    public static bool ST_Overlaps(this byte[] ewkb, byte[] other);
    public static double ST_Perimeter(this byte[] ewkb);
    public static byte[]? ST_PointN(this byte[] ewkb, int index)
    public static byte[] ST_PointOnSurface(this byte[] ewkb);
    public static bool ST_Relate(this byte[] ewkb, byte[] other, string intersectionPattern);
    public static byte[] ST_Reverse(this byte[] ewkb);
    public static byte[] ST_SetSRID(this byte[] ewkb, int srid);
    public static byte[]? ST_StartPoint(this byte[] ewkb);
    public static byte[] ST_SymDifference(this byte[] ewkb, byte[] other);
    public static byte[] ST_SymmetricDifference(this byte[] ewkb, byte[] other);
    public static bool ST_Touches(this byte[] ewkb, byte[] other);
    public static byte[] ST_Union(this byte[] ewkb, byte[] other);
    public static double ST_Width(this byte[] ewkb);
    public static bool ST_Within(this byte[] ewkb, byte[] other);
};
```

## SQLiteNetSpatialConnection

SQLiteNetSpatialConnection is a spatial wrapper for sqlite-net-pcl's SQLiteConnection with the ST functions added to the SQLiteConnection.

## MicrosoftDataSqliteSpatialConnection

MicrosoftDataSqliteSpatialConnection is a spatial wrapper for Microsoft.Data.SqLite's SqliteConnection with the ST functions added to the SqliteConnection.

## Example - Using ST functions in SQLite

```c#
// Create an in-memory SQLite database with spatial support.
SQLiteNetSpatialConnection db = new(":memory:");

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

 - Documentation: https://stephenquan.github.io/SQuan.Helpers/SQLite.Spatial/
 - GitHub repository: https://github.com/stephenquan/SQuan.Helpers
