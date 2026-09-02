# ST Class

## Overview

The ST static class provides a common set of spatial operations that can be used consistently in both SQLite and C#. Rather than maintaining separate implementations for application code and database queries, developers can work with a familiar set of spatial concepts across both environments.

This unified model makes it easier to move spatial logic between application code and the database. Operations such as creating geometries, calculating distances, testing spatial relationships, and performing geometric transformations can be expressed using the same conceptual API, whether they are executed in-memory through C# or within SQLite queries.

## C# Namespace

```c#
using SQuan.Helpers.SQLite.Spatial;
```

## Constructors

| Name            | Description                             |
| --------------- | --------------------------------------- |
| [ST_Point](#st_point) | Creates a point geometry. |
| [ST_GeomFromText](#st_geomfromtext) | Creates a geometry using WKT definition. |

## Methods

| Name            | Description                             |
| --------------- | --------------------------------------- |
| [ST_Area](#st_area) | Calculates the area of a polygon geometry. |
| [ST_AsText](#st_astext) | Returns the WKT representation of the geometry. |
| [ST_Boundary](#st_boundary) | Computes the boundary of a geometry. |
| [ST_Buffer](#st_buffer) | Computes a buffer region around a geometry. |
| [ST_Centroid](#st_centroid) | Calculates the geometric center (centroid) of a geometry. |
| [ST_Contains](#st_contains) | Determines whether one geometry contains another. |
| [ST_ConvexHull](#st_convexhull) | Computes the convex hull of a geometry. |
| [ST_CoveredBy](#st_coveredby) | Determines whether one geometry is spatially covered by another. |
| [ST_Covers](#st_covers) | Determines whether one geometry spatially covers another. |
| [ST_Crosses](#st_crosses) | Determines whether two geometries cross each other. |
| [ST_Difference](#st_difference) | Computes the geometric difference between two geometries. |
| [ST_Disjoint](#st_disjoint) | Determines whether two geometries are spatially disjoint. |
| [ST_Distance](#st_distance) | Calculates the shortest distance between two geometries. |
| [ST_Equals](#st_equals) | Determines whether two geometries are topologically equal. |
| [ST_EqualsExact](#st_equalsexact) | Determines whether two geometries are exactly equal in both shape and coordinate order. |
| [ST_EqualsNormalized](#st_equalsnormalized) | Determines whether two geometries are equal after applying a normalization process. |
| [ST_EqualsTopologically](#st_equalstopologically) | Determines whether two geometries are topologically equal. |
| [ST_Envelope](#st_envelope) | Computes the minimum bounding rectangle (MBR), also known as the envelope, of a geometry. |
| [ST_GeometryType](#st_geometrytype) | Returns the type of geometry. |
| [ST_Height](#st_height) | Calculates the height of the minimum bounding rectangle (MBR), also known as the envelope, for a given geometry. |
| [ST_InteriorPoint](#st_interiorpoint) | Returns a point guaranteed to lie in the interior of a given geometry. |
| [ST_Intersection](#st_intersection) | Computes the geometric intersection of two geometries. |
| [ST_Intersects](#st_intersects) | Determines whether two geometries intersect. |
| [ST_IsEmpty](#st_isempty) | Tests whether the set of points covered in this Geometry is empty. |
| [ST_IsGeometry](#st_isgeometry) | Tests for a valid geometry. |
| [ST_IsRectangle](#st_isrectangle) | Test whether the a given geometry represents a valid axis-aligned rectangle. |
| [ST_IsSimple](#st_issimple) | Tests whether a given geometry is simple. |
| [ST_IsValid](#st_isvalid) | Tests whether a given  is topologically valid. |
| [ST_Length](#st_length) | Calculates the total length of a geometry. |
| [ST_Reverse](#st_reverse) | Reverses the order of vertices in a linear geometry. |
| [ST_SetSRID](#st_setsrid) | Assigns a SRID to a geometry. |
| [ST_SRID](#st_srid) | Retrieves the SRID from a geometry. |
| [ST_SymmetricDifference](#st_symmetricdifference) | Computes the symmetric difference between two geometries. |
| [ST_Touches](#st_touches) | Determines whether two geometries touch at their boundaries. |
| [ST_Union](#st_union) | Computes the spatial union of two geometries. |
| [ST_Width](#st_width) | Calculates the width of the minimum bounding rectangle (MBR), also known as the envelope, for a given geometry. |
| [ST_Within](#st_within) | Determines whether the first geometry is completely contained within the second geometry. |
| [ST_X](#st_x) | Extracts the X-coordinate of the centroid of a geometry. |
| [ST_XMax](#st_xmax) | Returns the Y maximum of a bounding box or a geometry. |
| [ST_XMin](#st_xmin) | Returns the X minimum of a bounding box or a geometry. |
| [ST_Y](#st_y) | Extracts the Y-coordinate of the centroid of a geometry. |
| [ST_YMax](#st_ymax) | Returns the Y maximum of a bounding box or a geometry. |
| [ST_YMin](#st_ymin) | Returns the Y minimum of a bounding box or a geometry. |

## ST_Area

Calculates the area of a polygon geometry provided in EWKB format. If the input is null or invalid, the method returns null. The area is computed assuming planar geometry and is expressed in the same units as the coordinate system of the input.

```sql
double ST_Area(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Area(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    double area = geometry.ST_Area();
    ```

## ST_AsText

Returns the OGC Well-Known Text (WKT) representation of the geometry. 

```sql
text ST_AsText(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_AsText(ST_Point(40, 30))
    ```

=== "C#"

    ```c#
    byte[] point = ST.ST_Point(40, 30);
    string ewkt = point.ST_AsText();
    ```

## ST_Boundary

Computes the boundary of a geometry from its EWKB representation. For a polygon, the boundary consists of its exterior and interior rings. For a linestring, the boundary includes its endpoints. Returns a EWKB representing the boundary geometry.

```sql
blob ST_Boundary(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Boundary('POLYGON((0 0, 40 30, 40 0, 0 0))')
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] boundary = geometry.ST_Boundary();
    ```

## ST_Buffer

Computes a buffer region around a geometry defined by its EWKB representation. The buffer is a polygon that encloses all points within the specified distance from the original geometry.

```sql
blob ST_Buffer(blob ewkb, real distance)
```

=== "SQLite"

    ```sql
    SELECT ST_Buffer(ST_Point(40, 30), 50)
    ```

=== "C#"

    ```c#
    byte[] point = ST.ST_Point(40, 30);
    byte[] buffer = point.ST_Buffer(50);
    ```

## ST_Centroid

Calculates the geometric center (centroid) of a geometry provided in EWKB format. The centroid is the arithmetic mean position of all the points in the shape and may not necessarily lie within the geometry itself (e.g., for concave polygons).

```sql
blob ST_Centroid(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Centroid('POLYGON((0 0, 40 30, 40 0, 0 0))')
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] centroid = geometry.ST_Centroid();
    ```

## ST_Contains

Determines whether one geometry contains another, based on their EWKB representations. The method returns 1 (true) if the first geometry completely contains the second, and 0 (false) otherwise.

```sql
integer ST_Contains(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Contains(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_Point(30, 10))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] point = ST.ST_Point(30, 10);
    int contains = geometry.ST_Contains(point);
    ```

## ST_ConvexHull

Computes the convex hull of a geometry provided in EWKB format. The convex hull is the smallest convex polygon that fully encloses all points of the input geometry.

```sql
blob ST_ConvexHull(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_ConvexHull(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] convexHull = geometry.ST_ConvexHull();
    ```

## ST_CoveredBy

Determines whether one geometry is spatially covered by another, based on their EWKB representations. A geometry is considered "covered by" another if every point of the first geometry lies within or on the boundary of the second geometry. The method returns 1 (true) if the first geometry is covered by the second, and 0 (false) otherwise.

```sql
integer ST_CoveredBy(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_CoveredBy(
                ST_Point(30, 10),
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] point = ST.ST_Point(30, 10);
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    int coveredby = point.ST_CoveredBy(geometry);
    ```

## ST_Covers

Determines whether one geometry spatially covers another, based on their EWKB representations. A geometry covers another if every point of the second geometry lies within or on the boundary of the first. The method returns 1 (true) if the first geometry covers the second, and 0 (false) otherwise.

```sql
integer ST_Covers(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Covers(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_Point(30, 10))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] point = ST.ST_Point(30, 10);
    int covers = geometry.ST_Covers(point);
    ```

## ST_Crosses

Determines whether two geometries cross each other, based on their EWKB representations. Two geometries cross if they share some but not all interior points, and the result of their intersection has a lower dimension than the maximum of the input geometries. The method returns 1 (true) if the first geometry crosses the second, and 0 (false) otherwise.

```sql
integer ST_Crosses(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Crosses(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    int crosses = geometry.ST_Crosses(other);
    ```

## ST_Difference

Computes the geometric difference between two geometries provided in EWKB format. The result is a new geometry representing the portion of the first geometry that does not intersect with the second geometry.

```sql
blob ST_Difference(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Difference(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    byte[] difference = geometry.ST_Difference(other);
    ```

## ST_Disjoint

Determines whether two geometries are spatially disjoint, based on their EWKB representations. Two geometries are disjoint if they do not share any points - meaning their intersection is empty. The method returns 1 (true) if the two geometries are disjoint, and 0 (false) otherwise.

```sql
integer ST_Disjoint(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Disjoint(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    int disjoint = geometry.ST_Disjoint(other);
    ```

## ST_Distance

Calculates the shortest distance between two geometries provided in EWKB format. The result is a non-negative value representing the minimum distance between any two points from the input geometries. If either input is null or invalid, the method returns null.

```sql
real ST_Distance(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Distance(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_Point(0, 30))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_Point(0, 30);
    double distance = geometry.ST_Distance(other);
    ```

## ST_Equals

Determines whether two geometries are topologically equal, based on their EWKB representations. Two geometries are considered topologically equal if they represent the same spatial structure, regardless of differences in coordinate order or representation. The method returns 1 (true) if the first geometry is topologically equal to the second, and 0 (false) otherwise.

```sql
integer ST_Equals(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Equals(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    int equals = geometry.ST_Equals(other);
    ```

## ST_EqualsExact

Determines whether two geometries are exactly equal in both shape and coordinate order, based on their EWKB representations. This method performs a strict comparison, requiring that both geometries have identical types, vertex sequences, and structure. The method returns 1 (true) if the first geometry is exactly equal to the second, and 0 (false) otherwise.

```sql
integer ST_EqualsExact(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_EqualsExact(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    int equalsExact = geometry.ST_EqualsExact(other);
    ```

## ST_EqualsNormalized

Determines whether two geometries are equal after applying a normalization process, based on their EWKB representations. Normalized equality means the geometries are structurally and spatially identical after standardizing their internal representation (e.g., ordering of coordinates, ring orientation). The method returns 1 (true) if the first geometry is equal to the second, and 0 (false) otherwise.

```sql
integer ST_EqualsNormalized(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_EqualsNormalized(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 40 0, 40 30, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 40 0, 40 30, 0 0))");
    int equalsNormalized = geometry.ST_EqualsNormalized(other);
    ```

## ST_EqualsTopologically

Determines whether two geometries are topologically equal, based on their EWKB representations. Two geometries are considered topologically equal if they represent the same spatial structure, regardless of differences in coordinate order or representation. The method returns 1 (true) if the first geometry is topologically equal to the second, and 0 (false) otherwise.

```sql
integer ST_EqualsTopologically(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_EqualsTopologically(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 40 0, 40 30, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 40 0, 40 30, 0 0))");
    int equalsTopologically = geometry.ST_EqualsTopologically(other);
    ```

## ST_Envelope

Computes the minimum bounding rectangle (MBR), also known as the envelope, of a geometry provided in EWKB format. The envelope is the smallest axis-aligned rectangle that fully contains the input geometry.

```sql
blob ST_Envelope(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Envelope(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))')
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] envelope = geometry.ST_Envelope();
    ```

## ST_GeometryType

Returns the type of geometry represented by a EWKB representation. This method identifies the geometry class and returns it as a string.

```sql
text ST_GeometryType(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_GeometryType(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))')
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    string geometryType = geometry.ST_GeometryType();
    ```

## ST_GeomFromText

Constructs a geometry object from the OGC Well-Known text representation.

```sql
blob ST_GeomFromText(blob wkt)
```

=== "SQLite"

    ```sql
    SELECT ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))')
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    ```

## ST_Height

Calculates the height of the minimum bounding rectangle (MBR), also known as the envelope, for a given geometry in EWKB format. The height is the difference between the maximum and minimum Y-coordinates of the geometry's envelope.

```sql
real ST_Height(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Height(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    double height = geometry.ST_Height();
    ```

## ST_InteriorPoint

Returns a point guaranteed to lie in the interior of a given geometry, based on its EWKB representation. This method is especially useful for labeling or anchoring geometries such as polygons or multipolygons.

```sql
blob ST_InteriorPoint(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_InteriorPoint(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] interiorPoint = geometry.ST_InteriorPoint();
    ```

## ST_Intersection

Computes the geometric intersection of two geometries provided in EWKB format. The result is a new geometry representing the shared spatial area between the two inputs - where they overlap or touch.

```sql
blob ST_Intersection(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Intersection(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    byte[] intersection = geometry.ST_Intersection(other);
    ```

## ST_Intersects

Determines whether two geometries intersect, based on their EWKB representations. Two geometries intersect if they share at least one point in space, including touching at boundaries or overlapping interiors. The method returns 1 (true) if the first geometry intersects the second, and 0 (false) otherwise.

```sql
integer ST_Intersects(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Intersects(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    int intersects = geometry.ST_Intersects(other);
    ```

## ST_IsEmpty

Tests whether the set of points covered in this Geometry is empty. Note this test is for topological emptiness, not structural emptiness. A collection containing only empty elements is reported as empty. The method returns 1 (true) if the geometry is empty, and 0 (false) otherwise.

```sql
integer ST_IsEmpty(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_IsEmpty(ST_GeomFromText('POLYGON((0 0, 0 0, 0 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 0 0, 0 0, 0 0))");
    int isEmpty = geometry.ST_IsEmpty();
    ```

## ST_IsGeometry

Tests whether the data supplied is a valid EWKB representation of a geometry. The method returns 1 (true) if the text is a valid WKT representation of geometry, and 0 (false) otherwise.

```sql
integer ST_IsGeometry(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_IsGeometry(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    int isGeometry = geometry.ST_IsGeometry();
    ```

## ST_IsRectangle

Determines whether a given geometry, provided in EWKB format, represents a valid axis-aligned rectangle. This method checks for rectangular shape, closed polygon structure, and right-angle alignment. The method returns 1 (true) if the geometry is a rectangle, and 0 (false) otherwise.

```sql
integer ST_IsRectangle(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_IsRectangle(ST_GeomFromText('POLYGON((0 0, 0 30, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 30, 40 0, 0 0))");
    int isRectangle = geometry.ST_IsRectangle();
    ```

## ST_IsSimple

Tests whether a given geometry, provided in EWKB format, is simple. Geometry is simple if it has no points of self-tangency, self-intersection, or other anomalous points. The method returns 1 (true) if the geometry is simple, and 0 (false) otherwise.

```sql
integer ST_IsSimple(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_IsSimple(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    int isSimple = geometry.ST_IsSimple();
    ```

## ST_IsValid

Tests whether a given geometry, provided in EWKB format, is topologically valid according to the OGC SFS specification. The method returns 1 (true) if the geometry is valid, and 0 (false) otherwise.

```sql
integer ST_IsValid(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_IsValid(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    int isValid = geometry.ST_IsValid();
    ```

## ST_Length

Calculates the total length of a geometry provided in EWKB format. This method is typically used for linear geometries such as LINESTRING or MULTILINESTRING, and returns the sum of all segment lengths. If the input is null or invalid, the method returns null.

```sql
real ST_Length(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Length(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    double length = geometry.ST_Length();
    ```

## ST_Point

```sql
blob ST_Point(real x, real y, [integer srid = 4326])
```

Returns a Point with the given X and Y coordinate values.

=== "SQLite"

    ```sql
    SELECT ST_Point(40, 30)
    ```

=== "C#"

    ```c#
    byte[] point = ST.ST_Point(40, 30);
    ```

## ST_Reverse

Reverses the order of vertices in a linear geometry provided in EWKB format. This method is typically used with LINESTRING or MULTILINESTRING geometries to invert the direction of traversal.

```sql
blob ST_Reverse(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Reverse(ST_GeomFromText('LINESTRING(0 0, 40 30, 40 0)'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST_GeomFromText('LINESTRING(0 0, 40 30, 40 0)');
    byte[] reverse = geometry.ST_Reverse();
    ```

## ST_SetSRID

Assigns a Spatial Reference System Identifier (SRID) to a geometry represented in EWKB format. The SRID defines the coordinate system and projection used for interpreting the geometry's coordinates. This method does not alter the geometry itself, only its spatial reference metadata.

```sql
blob ST_SetSRID(blob ewkb, integer srid)
```

=== "SQLite"

    ```sql
    SELECT ST_SetSRID(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'), 4326)
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))');
    byte[] modified = geometry.ST_SetSRID(4326);
    ```

## ST_SRID

Retrieves the Spatial Reference System Identifier (SRID) from a geometry represented in EWKB format. The SRID defines the coordinate system and projection used to interpret the geometry's coordinates.

```sql
integer ST_SRID(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_SRID(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))');
    int srid = geometry.ST_SRID();
    ```

## ST_SymmetricDifference

Computes the symmetric difference between two geometries provided in EWKB format. The symmetric difference is the set of points that belong to either geometry but not to both - essentially the non-overlapping portions of the two inputs.

```sql
blob ST_SymmetricDifference(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_SymmetricDifference(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    byte[] symmetricDifference = geometry.ST_SymmetricDifference(other);
    ```

## ST_Touches

Determines whether two geometries touch at their boundaries but do not overlap in their interiors, based on their EWKB representations. This method returns 1 (true) if the geometries share at least one boundary point and their interiors do not intersect, and 0 (false) otherwise.

```sql
integer ST_Touches(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Touches(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 30, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    int touches = geometry.ST_Touches(other);
    ```

## ST_Union

Computes the spatial union of two geometries provided in Well-Known Text (WKT) format. The union operation returns a geometry that represents all points from both input geometries, merging overlapping areas and combining disjoint parts into a single result.

```sql
blob ST_Union(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Union(
                ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 30, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    byte[] union = geometry.ST_Union(other);
    ```

## ST_Width

Calculates the width of the minimum bounding rectangle (MBR), also known as the envelope, for a given geometry in EWKB format. The width is the difference between the maximum and minimum X-coordinates of the geometry's envelope.

```sql
real ST_Width(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Width(ST_GeomFromText('POLYGON((0 0, 40 30, 40 0, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((0 0, 40 30, 40 0, 0 0))");
    double width = geometry.ST_Width();
    ```

## ST_Within

Determines whether the first geometry is completely contained within the second geometry, based on their EWKB representations. This method returns 1 (true) if every point of the first geometry lies inside the second geometry, including its boundary, and 0 (false) otherwise.

```sql
integer ST_Within(blob ewkb, blob other)
```

=== "SQLite"

    ```sql
    SELECT ST_Within(
                ST_Point(30, 10),
                ST_GeomFromText('POLYGON((0 0, 0 30, 40 30, 0 0))'))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_Point(30, 10);
    byte[] other = ST.ST_GeomFromText("POLYGON((0 0, 0 30, 40 0, 0 0))");
    int within = geometry.ST_Within(other);
    ```

## ST_X

Extracts the X-coordinate of the centroid (geometric center) of a geometry provided in EWKB format. The centroid represents the average position of all points in the geometry and is useful for labeling, spatial indexing, and geometric analysis.

```sql
real ST_X(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_X(ST_Point(40, 30))
    ```

=== "C#"

    ```c#
    byte[] point = ST.ST_Point(40, 30);
    double x_40 = point.ST_X();
    ```

## ST_XMax

Returns the X maximum of a bounding box or a geometry.

```sql
real ST_XMax(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_XMax(ST_GeomFromText("POLYGON((30 40, 50 40, 50 60, 30 60, 30 40))"))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((30 40, 50 40, 50 60, 30 60, 30 40))");
    double xmax_50 = geometry.ST_XMax();
    ```

## ST_XMin

Returns the X minimum of a bounding box or a geometry.

```sql
real ST_XMin(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_XMin(ST_GeomFromText("POLYGON((30 40, 50 40, 50 60, 30 60, 30 40))"))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((30 40, 50 40, 50 60, 30 60, 30 40))");
    double xmin_30 = geometry.ST_XMin();
    ```

## ST_Y

Extracts the Y-coordinate of the centroid (geometric center) of a geometry provided in EWKB format. The centroid represents the average position of all points in the geometry and is useful for labeling, spatial indexing, and geometric analysis.

```sql
real ST_Y(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_Y(ST_Point(40, 30))
    ```

=== "C#"

    ```c#
    byte[] point = ST.ST_Point(40, 30);
    double y_30 = point.ST_Y();
    ```

## ST_YMax

Returns the Y maximum of a bounding box or a geometry.

real ST_YMax(blob ewkb)

=== "SQLite"

    ```sql
    SELECT ST_YMax(ST_GeomFromText("POLYGON((30 40, 50 40, 50 60, 30 60, 30 40))"))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((30 40, 50 40, 50 60, 30 60, 30 40))");
    double ymax_60 = geometry.ST_YMax();
    ```

## ST_YMin

Returns the Y minimum of a bounding box or a geometry.

```sql
real ST_YMin(blob ewkb)
```

=== "SQLite"

    ```sql
    SELECT ST_YMin(ST_GeomFromText("POLYGON((30 40, 50 40, 50 60, 30 60, 30 40))"))
    ```

=== "C#"

    ```c#
    byte[] geometry = ST.ST_GeomFromText("POLYGON((30 40, 50 40, 50 60, 30 60, 30 40))");
    double ymin_40 = geometry.ST_YMin();
    ```
