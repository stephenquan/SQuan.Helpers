// ST.shared.cs

using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace SQuan.Helpers.SQLite.Spatial;

/// <summary>
/// Provides extension methods for working with spatial data in SQLite using NetTopologySuite.
/// </summary>
public static class ST
{
	/// <summary>
	/// Creates a Well-Known Binary (WKB) representation of the specified geometry in Extended Well-Known Binary (EWKB) format.
	/// </summary>
	/// <param name="geometry">The geometry to convert to EWKB format.</param>
	/// <param name="srid">The Spatial Reference System Identifier (SRID) of the geometry. Default is 4326.</param>
	/// <returns>A byte array containing the EWKB representation of the geometry.</returns>
	public static byte[] ToEWKB(this Geometry geometry, int srid = 4326)
	{
		geometry.SRID = srid;
		return new WKBWriter(ByteOrder.LittleEndian, handleSRID: true).Write(geometry);
	}

	/// <summary>
	/// Sets the Spatial Reference System Identifier (SRID) of the specified geometry and returns the modified geometry.
	/// </summary>
	/// <param name="geometry">The geometry for which to set the SRID.</param>
	/// <param name="srid">The Spatial Reference System Identifier (SRID) to set.</param>
	/// <returns>The geometry with the updated SRID.</returns>
	public static Geometry SetSRID(this Geometry geometry, int srid)
	{
		geometry.SRID = srid;
		return geometry;
	}

	/// <summary>
	/// Creates a geometry object from the specified Extended Well-Known Binary (EWKB) representation.
	/// </summary>
	/// <param name="ewkb">A byte array containing the EWKB representation of the geometry.</param>
	/// <returns>A <see cref="Geometry"/> object created from the EWKB representation.</returns>
	public static Geometry FromEWKB(byte[] ewkb) => new WKBReader().Read(ewkb);

	/// <summary>
	/// Calculates the area of a geometry represented in Extended Well-Known Binary (EWKB) format.
	/// </summary>
	/// <param name="ewkb">A byte array containing the EWKB representation of the geometry.</param>
	/// <returns>The area of the geometry, or null if the geometry is null.</returns>
	public static double? ST_Area(this byte[] ewkb) => FromEWKB(ewkb)?.Area;

	/// <summary>
	/// Returns the Well-Known Text (WKT) representation of a geometry represented in Extended Well-Known Binary (EWKB) format.
	/// </summary>
	/// <param name="ewkb">A byte array containing the EWKB representation of the geometry.</param>
	/// <returns>The WKT representation of the geometry.</returns>
	public static string ST_AsText(this byte[] ewkb)
	{
		if (FromEWKB(ewkb) is Geometry geometry)
		{
			string wkt = geometry.AsText();
			if (geometry.SRID is int srid && srid != 0)
			{
				return $"SRID={srid};{wkt}";
			}
			return wkt;
		}
		return string.Empty;
	}


	/// <summary>
	/// Creates a geometry object from the specified Well-Known Text (WKT) representation and returns its Extended Well-Known Binary (EWKB) representation.
	/// </summary>
	/// <param name="text">A string containing the WKT representation of the geometry.</param>
	/// <param name="srid">The Spatial Reference System Identifier (SRID) of the geometry. Default is 4326.</param>
	/// <returns>A byte array containing the EWKB representation of the geometry.</returns>
	public static byte[] ST_GeomFromText(string text, int srid = 4326) => ToEWKB(new WKTReader().Read(text).SetSRID(srid));

	/// <summary>
	/// Creates a Well-Known Binary (WKB) representation of a point geometry with the specified coordinates and SRID.
	/// </summary>
	/// <param name="x">The X coordinate of the point.</param>
	/// <param name="y">The Y coordinate of the point.</param>
	/// <param name="srid">The Spatial Reference System Identifier (SRID) of the point. Default is 4326.</param>
	/// <returns>A byte array containing the WKB representation of the point.</returns>
	public static byte[] ST_Point(double x, double y, int srid = 4326) => ToEWKB(new Point(x, y).SetSRID(srid));

	/// <summary>
	/// Retrieves the Spatial Reference System Identifier (SRID) of a geometry represented in Extended Well-Known Binary (EWKB) format.
	/// </summary>
	/// <param name="ewkb">A byte array containing the EWKB representation of the geometry.</param>
	/// <returns>The SRID of the geometry.</returns>
	public static int ST_SRID(this byte[] ewkb) => FromEWKB(ewkb).SRID;

	/// <summary>
	/// Calculates the X coordinate of the centroid of a geometry represented in Extended Well-Known Binary (EWKB) format.
	/// </summary>
	/// <param name="ewkb">A byte array containing the EWKB representation of the geometry.</param>
	/// <returns>The X coordinate of the centroid, or null if the geometry is null.</returns>
	public static double? ST_X(this byte[] ewkb) => FromEWKB(ewkb)?.Centroid.X;
	/// <summary>
	/// Calculates the Y coordinate of the centroid of a geometry represented in Extended Well-Known Binary (EWKB) format.
	/// </summary>
	/// <param name="ewkb">A byte array containing the EWKB representation of the geometry.</param>
	/// <returns>The Y coordinate of the centroid, or null if the geometry is null.</returns>
	public static double? ST_Y(this byte[] ewkb) => FromEWKB(ewkb)?.Centroid.Y;

	/// <summary>
	/// Returns the EWKB representation unchanged.
	/// </summary>
	public static byte[] ST_AsBinary(this byte[] ewkb) => ewkb;

	/// <summary>
	/// Returns the Extended Well-Known Text (EWKT) representation of a geometry.
	/// </summary>
	public static string ST_AsEWKT(this byte[] ewkb) => ST_AsText(ewkb);

	/// <summary>
	/// Returns the boundary of a geometry.
	/// </summary>
	public static byte[] ST_Boundary(this byte[] ewkb) => ToEWKB(FromEWKB(ewkb).Boundary, ST_SRID(ewkb));

	/// <summary>
	/// Returns a geometry covering all points within the specified distance.
	/// </summary>
	public static byte[] ST_Buffer(this byte[] ewkb, double distance) => ToEWKB(FromEWKB(ewkb).Buffer(distance), ST_SRID(ewkb));

	/// <summary>
	/// Returns the centroid of a geometry.
	/// </summary>
	public static byte[] ST_Centroid(this byte[] ewkb) => ToEWKB(FromEWKB(ewkb).Centroid, ST_SRID(ewkb));

	/// <summary>
	/// Determines whether the first geometry contains the second geometry.
	/// </summary>
	public static bool ST_Contains(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Contains(FromEWKB(other));

	/// <summary>
	/// Returns the convex hull of a geometry.
	/// </summary>
	public static byte[] ST_ConvexHull(this byte[] ewkb) => ToEWKB(FromEWKB(ewkb).ConvexHull(), ST_SRID(ewkb));

	/// <summary>
	/// Determines whether the first geometry is covered by the second geometry.
	/// </summary>
	public static bool ST_CoveredBy(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).CoveredBy(FromEWKB(other));

	/// <summary>
	/// Determines whether the first geometry covers the second geometry.
	/// </summary>
	public static bool ST_Covers(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Covers(FromEWKB(other));

	/// <summary>
	/// Determines whether two geometries cross.
	/// </summary>
	public static bool ST_Crosses(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Crosses(FromEWKB(other));

	/// <summary>
	/// Returns the topological dimension of a geometry.
	/// </summary>
	public static int ST_Dimension(this byte[] ewkb) => (int)FromEWKB(ewkb).Dimension;

	/// <summary>
	/// Returns the portion of the first geometry that does not intersect the second geometry.
	/// </summary>
	public static byte[] ST_Difference(this byte[] ewkb, byte[] other) => ToEWKB(FromEWKB(ewkb).Difference(FromEWKB(other)), ST_SRID(ewkb));

	/// <summary>
	/// Determines whether two geometries are disjoint.
	/// </summary>
	public static bool ST_Disjoint(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Disjoint(FromEWKB(other));

	/// <summary>
	/// Returns the minimum Cartesian distance between two geometries.
	/// </summary>
	public static double ST_Distance(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Distance(FromEWKB(other));

	/// <summary>
	/// Determines whether two geometries are within the specified Cartesian distance.
	/// </summary>
	public static bool ST_DWithin(this byte[] ewkb, byte[] other, double distance) => FromEWKB(ewkb).IsWithinDistance(FromEWKB(other), distance);

	/// <summary>
	/// Determines whether two geometries are topologically equal.
	/// </summary>
	public static bool ST_Equals(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).EqualsTopologically(FromEWKB(other));

	/// <summary>
	/// Returns the bounding box of a geometry.
	/// </summary>
	public static byte[] ST_Envelope(this byte[] ewkb) => ToEWKB(FromEWKB(ewkb).Envelope, ST_SRID(ewkb));

	/// <summary>
	/// Returns the last point of a line string, or null for another geometry type.
	/// </summary>
	public static byte[]? ST_EndPoint(this byte[] ewkb)
	{
		Geometry geometry = FromEWKB(ewkb);
		return geometry is LineString lineString && !lineString.IsEmpty
			? ToEWKB(lineString.GetPointN(lineString.NumPoints - 1), geometry.SRID)
			: null;
	}

	/// <summary>
	/// Returns the nth component of a geometry, using one-based indexing.
	/// </summary>
	public static byte[]? ST_GeometryN(this byte[] ewkb, int index)
	{
		Geometry geometry = FromEWKB(ewkb);
		return index >= 1 && index <= geometry.NumGeometries
			? ToEWKB(geometry.GetGeometryN(index - 1), geometry.SRID)
			: null;
	}

	/// <summary>
	/// Returns the geometry type name.
	/// </summary>
	public static string ST_GeometryType(this byte[] ewkb) => FromEWKB(ewkb).GeometryType;

	/// <summary>
	/// Returns the height of a geometry's bounding box.
	/// </summary>
	public static double ST_Height(this byte[] ewkb) => FromEWKB(ewkb).EnvelopeInternal.Height;

	/// <summary>
	/// Returns a point guaranteed to lie on the geometry.
	/// </summary>
	public static byte[] ST_InteriorPoint(this byte[] ewkb) => ToEWKB(FromEWKB(ewkb).InteriorPoint, ST_SRID(ewkb));

	/// <summary>
	/// Returns the shared portion of two geometries.
	/// </summary>
	public static byte[] ST_Intersection(this byte[] ewkb, byte[] other) => ToEWKB(FromEWKB(ewkb).Intersection(FromEWKB(other)), ST_SRID(ewkb));

	/// <summary>
	/// Determines whether two geometries intersect.
	/// </summary>
	public static bool ST_Intersects(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Intersects(FromEWKB(other));

	/// <summary>
	/// Determines whether a geometry is closed.
	/// </summary>
	public static bool ST_IsClosed(this byte[] ewkb)
	{
		Geometry geometry = FromEWKB(ewkb);
		if (geometry is LineString lineString)
		{
			return lineString.IsClosed;
		}
		if (geometry is MultiLineString multiLineString)
		{
			for (int index = 0; index < multiLineString.NumGeometries; index++)
			{
				if (multiLineString.GetGeometryN(index) is LineString part && !part.IsClosed)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	/// <summary>
	/// Determines whether a geometry is empty.
	/// </summary>
	public static bool ST_IsEmpty(this byte[] ewkb) => FromEWKB(ewkb).IsEmpty;

	/// <summary>
	/// Determines whether a geometry is rectangular.
	/// </summary>
	public static bool ST_IsRectangle(this byte[] ewkb) => FromEWKB(ewkb).IsRectangle;

	/// <summary>
	/// Determines whether a line string is both closed and simple.
	/// </summary>
	public static bool ST_IsRing(this byte[] ewkb) => FromEWKB(ewkb) is LineString lineString && lineString.IsRing;

	/// <summary>
	/// Determines whether a geometry is simple.
	/// </summary>
	public static bool ST_IsSimple(this byte[] ewkb) => FromEWKB(ewkb).IsSimple;

	/// <summary>
	/// Determines whether a geometry is topologically valid.
	/// </summary>
	public static bool ST_IsValid(this byte[] ewkb) => FromEWKB(ewkb).IsValid;

	/// <summary>
	/// Returns the Cartesian length of a geometry.
	/// </summary>
	public static double ST_Length(this byte[] ewkb) => FromEWKB(ewkb).Length;

	/// <summary>
	/// Returns the maximum X coordinate of a geometry's bounding box.
	/// </summary>
	public static double ST_MaxX(this byte[] ewkb) => FromEWKB(ewkb).EnvelopeInternal.MaxX;

	/// <summary>
	/// Returns the maximum Y coordinate of a geometry's bounding box.
	/// </summary>
	public static double ST_MaxY(this byte[] ewkb) => FromEWKB(ewkb).EnvelopeInternal.MaxY;

	/// <summary>
	/// Returns the minimum X coordinate of a geometry's bounding box.
	/// </summary>
	public static double ST_MinX(this byte[] ewkb) => FromEWKB(ewkb).EnvelopeInternal.MinX;

	/// <summary>
	/// Returns the minimum Y coordinate of a geometry's bounding box.
	/// </summary>
	public static double ST_MinY(this byte[] ewkb) => FromEWKB(ewkb).EnvelopeInternal.MinY;

	/// <summary>
	/// Returns the number of component geometries.
	/// </summary>
	public static int ST_NumGeometries(this byte[] ewkb) => FromEWKB(ewkb).NumGeometries;

	/// <summary>
	/// Returns the number of interior rings in a polygon, or zero for another geometry type.
	/// </summary>
	public static int ST_NumInteriorRings(this byte[] ewkb) => FromEWKB(ewkb) is Polygon polygon ? polygon.NumInteriorRings : 0;

	/// <summary>
	/// Returns the number of points in a geometry.
	/// </summary>
	public static int ST_NumPoints(this byte[] ewkb) => FromEWKB(ewkb).NumPoints;

	/// <summary>
	/// Determines whether two geometries overlap.
	/// </summary>
	public static bool ST_Overlaps(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Overlaps(FromEWKB(other));

	/// <summary>
	/// Returns the Cartesian perimeter of a geometry.
	/// </summary>
	public static double ST_Perimeter(this byte[] ewkb) => FromEWKB(ewkb).Boundary.Length;

	/// <summary>
	/// Returns the nth point of a line string, using one-based indexing.
	/// </summary>
	public static byte[]? ST_PointN(this byte[] ewkb, int index)
	{
		Geometry geometry = FromEWKB(ewkb);
		return geometry is LineString lineString && index >= 1 && index <= lineString.NumPoints
			? ToEWKB(lineString.GetPointN(index - 1), geometry.SRID)
			: null;
	}

	/// <summary>
	/// Returns a point guaranteed to lie on the geometry.
	/// </summary>
	public static byte[] ST_PointOnSurface(this byte[] ewkb) => ST_InteriorPoint(ewkb);

	/// <summary>
	/// Determines whether the DE-9IM relationship matches an intersection pattern.
	/// </summary>
	public static bool ST_Relate(this byte[] ewkb, byte[] other, string intersectionPattern) => FromEWKB(ewkb).Relate(FromEWKB(other), intersectionPattern);

	/// <summary>
	/// Reverses the order of a geometry's vertices.
	/// </summary>
	public static byte[] ST_Reverse(this byte[] ewkb) => ToEWKB(FromEWKB(ewkb).Reverse(), ST_SRID(ewkb));

	/// <summary>
	/// Returns EWKB with the specified SRID.
	/// </summary>
	public static byte[] ST_SetSRID(this byte[] ewkb, int srid) => ToEWKB(FromEWKB(ewkb), srid);

	/// <summary>
	/// Returns the first point of a line string, or null for another geometry type.
	/// </summary>
	public static byte[]? ST_StartPoint(this byte[] ewkb)
	{
		Geometry geometry = FromEWKB(ewkb);
		return geometry is LineString lineString && !lineString.IsEmpty
			? ToEWKB(lineString.GetPointN(0), geometry.SRID)
			: null;
	}

	/// <summary>
	/// Returns the portions of two geometries that do not intersect.
	/// </summary>
	public static byte[] ST_SymDifference(this byte[] ewkb, byte[] other) => ToEWKB(FromEWKB(ewkb).SymmetricDifference(FromEWKB(other)), ST_SRID(ewkb));

	/// <summary>
	/// Returns the portions of two geometries that do not intersect.
	/// </summary>
	public static byte[] ST_SymmetricDifference(this byte[] ewkb, byte[] other) => ST_SymDifference(ewkb, other);

	/// <summary>
	/// Determines whether two geometries touch.
	/// </summary>
	public static bool ST_Touches(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Touches(FromEWKB(other));

	/// <summary>
	/// Returns the combined point set of two geometries.
	/// </summary>
	public static byte[] ST_Union(this byte[] ewkb, byte[] other) => ToEWKB(FromEWKB(ewkb).Union(FromEWKB(other)), ST_SRID(ewkb));

	/// <summary>
	/// Returns the width of a geometry's bounding box.
	/// </summary>
	public static double ST_Width(this byte[] ewkb) => FromEWKB(ewkb).EnvelopeInternal.Width;

	/// <summary>
	/// Determines whether the first geometry is within the second geometry.
	/// </summary>
	public static bool ST_Within(this byte[] ewkb, byte[] other) => FromEWKB(ewkb).Within(FromEWKB(other));
}
