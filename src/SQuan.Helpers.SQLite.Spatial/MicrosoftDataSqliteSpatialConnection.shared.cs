// MicrosoftDataSqliteSpatialConnection.shared.cs

using Microsoft.Data.Sqlite;

namespace SQuan.Helpers.SQLite.Spatial;

/// <summary>
/// Represents a Microsoft.Data.Sqlite connection with spatial extensions enabled.
/// </summary>
public class MicrosoftDataSqliteSpatialConnection : SqliteConnection, ISpatialConnection
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MicrosoftDataSqliteSpatialConnection"/> class with the specified connection string.
	/// </summary>
	/// <param name="connectionString">The connection string to the SQLite database.</param>
	public MicrosoftDataSqliteSpatialConnection(string? connectionString = null) : base(connectionString)
	{
	}

	/// <summary>
	/// Opens the connection to the database and enables spatial extensions.
	/// </summary>
	public override void Open()
	{
		base.Open();
		EnableSpatialExtensions();
	}

	/// <summary>
	/// Enables spatial extensions for the specified <see cref="Microsoft.Data.Sqlite.SqliteConnection"/>.
	/// </summary>
	void EnableSpatialExtensions()
	{
		CreateFunction("ST_Area", (byte[] ewkb) => ewkb.ST_Area(), isDeterministic: true);
		CreateFunction("ST_AsText", (byte[] ewkb) => ewkb.ST_AsText(), isDeterministic: true);
		CreateFunction("ST_GeomFromText", (string text) => ST.ST_GeomFromText(text), isDeterministic: true);
		CreateFunction("ST_GeomFromText", (string text, int srid) => ST.ST_GeomFromText(text, srid), isDeterministic: true);
		CreateFunction("ST_Point", (double x, double y) => ST.ST_Point(x, y), isDeterministic: true);
		CreateFunction("ST_Point", (double x, double y, int srid) => ST.ST_Point(x, y, srid), isDeterministic: true);
		CreateFunction("ST_SRID", (byte[] ewkb) => ewkb.ST_SRID(), isDeterministic: true);
		CreateFunction("ST_X", (byte[] ewkb) => ewkb.ST_X(), isDeterministic: true);
		CreateFunction("ST_Y", (byte[] ewkb) => ewkb.ST_Y(), isDeterministic: true);
		CreateFunction("ST_AsBinary", (byte[] ewkb) => ewkb.ST_AsBinary(), isDeterministic: true);
		CreateFunction("ST_AsEWKT", (byte[] ewkb) => ewkb.ST_AsEWKT(), isDeterministic: true);
		CreateFunction("ST_Boundary", (byte[] ewkb) => ewkb.ST_Boundary(), isDeterministic: true);
		CreateFunction("ST_Buffer", (byte[] ewkb, double distance) => ewkb.ST_Buffer(distance), isDeterministic: true);
		CreateFunction("ST_Centroid", (byte[] ewkb) => ewkb.ST_Centroid(), isDeterministic: true);
		CreateFunction("ST_Contains", (byte[] ewkb, byte[] other) => ewkb.ST_Contains(other), isDeterministic: true);
		CreateFunction("ST_ConvexHull", (byte[] ewkb) => ewkb.ST_ConvexHull(), isDeterministic: true);
		CreateFunction("ST_CoveredBy", (byte[] ewkb, byte[] other) => ewkb.ST_CoveredBy(other), isDeterministic: true);
		CreateFunction("ST_Covers", (byte[] ewkb, byte[] other) => ewkb.ST_Covers(other), isDeterministic: true);
		CreateFunction("ST_Crosses", (byte[] ewkb, byte[] other) => ewkb.ST_Crosses(other), isDeterministic: true);
		CreateFunction("ST_Dimension", (byte[] ewkb) => ewkb.ST_Dimension(), isDeterministic: true);
		CreateFunction("ST_Difference", (byte[] ewkb, byte[] other) => ewkb.ST_Difference(other), isDeterministic: true);
		CreateFunction("ST_Disjoint", (byte[] ewkb, byte[] other) => ewkb.ST_Disjoint(other), isDeterministic: true);
		CreateFunction("ST_Distance", (byte[] ewkb, byte[] other) => ewkb.ST_Distance(other), isDeterministic: true);
		CreateFunction("ST_DWithin", (byte[] ewkb, byte[] other, double distance) => ewkb.ST_DWithin(other, distance), isDeterministic: true);
		CreateFunction("ST_Equals", (byte[] ewkb, byte[] other) => ewkb.ST_Equals(other), isDeterministic: true);
		CreateFunction("ST_EndPoint", (byte[] ewkb) => ewkb.ST_EndPoint(), isDeterministic: true);
		CreateFunction("ST_Envelope", (byte[] ewkb) => ewkb.ST_Envelope(), isDeterministic: true);
		CreateFunction("ST_GeometryN", (byte[] ewkb, int index) => ewkb.ST_GeometryN(index), isDeterministic: true);
		CreateFunction("ST_GeometryType", (byte[] ewkb) => ewkb.ST_GeometryType(), isDeterministic: true);
		CreateFunction("ST_Height", (byte[] ewkb) => ewkb.ST_Height(), isDeterministic: true);
		CreateFunction("ST_InteriorPoint", (byte[] ewkb) => ewkb.ST_InteriorPoint(), isDeterministic: true);
		CreateFunction("ST_Intersection", (byte[] ewkb, byte[] other) => ewkb.ST_Intersection(other), isDeterministic: true);
		CreateFunction("ST_Intersects", (byte[] ewkb, byte[] other) => ewkb.ST_Intersects(other), isDeterministic: true);
		CreateFunction("ST_IsClosed", (byte[] ewkb) => ewkb.ST_IsClosed(), isDeterministic: true);
		CreateFunction("ST_IsEmpty", (byte[] ewkb) => ewkb.ST_IsEmpty(), isDeterministic: true);
		CreateFunction("ST_IsRectangle", (byte[] ewkb) => ewkb.ST_IsRectangle(), isDeterministic: true);
		CreateFunction("ST_IsRing", (byte[] ewkb) => ewkb.ST_IsRing(), isDeterministic: true);
		CreateFunction("ST_IsSimple", (byte[] ewkb) => ewkb.ST_IsSimple(), isDeterministic: true);
		CreateFunction("ST_IsValid", (byte[] ewkb) => ewkb.ST_IsValid(), isDeterministic: true);
		CreateFunction("ST_Length", (byte[] ewkb) => ewkb.ST_Length(), isDeterministic: true);
		CreateFunction("ST_MaxX", (byte[] ewkb) => ewkb.ST_MaxX(), isDeterministic: true);
		CreateFunction("ST_MaxY", (byte[] ewkb) => ewkb.ST_MaxY(), isDeterministic: true);
		CreateFunction("ST_MinX", (byte[] ewkb) => ewkb.ST_MinX(), isDeterministic: true);
		CreateFunction("ST_MinY", (byte[] ewkb) => ewkb.ST_MinY(), isDeterministic: true);
		CreateFunction("ST_NumGeometries", (byte[] ewkb) => ewkb.ST_NumGeometries(), isDeterministic: true);
		CreateFunction("ST_NumInteriorRings", (byte[] ewkb) => ewkb.ST_NumInteriorRings(), isDeterministic: true);
		CreateFunction("ST_NumPoints", (byte[] ewkb) => ewkb.ST_NumPoints(), isDeterministic: true);
		CreateFunction("ST_Overlaps", (byte[] ewkb, byte[] other) => ewkb.ST_Overlaps(other), isDeterministic: true);
		CreateFunction("ST_Perimeter", (byte[] ewkb) => ewkb.ST_Perimeter(), isDeterministic: true);
		CreateFunction("ST_PointN", (byte[] ewkb, int index) => ewkb.ST_PointN(index), isDeterministic: true);
		CreateFunction("ST_PointOnSurface", (byte[] ewkb) => ewkb.ST_PointOnSurface(), isDeterministic: true);
		CreateFunction("ST_Relate", (byte[] ewkb, byte[] other, string intersectionPattern) => ewkb.ST_Relate(other, intersectionPattern), isDeterministic: true);
		CreateFunction("ST_Reverse", (byte[] ewkb) => ewkb.ST_Reverse(), isDeterministic: true);
		CreateFunction("ST_SetSRID", (byte[] ewkb, int srid) => ewkb.ST_SetSRID(srid), isDeterministic: true);
		CreateFunction("ST_StartPoint", (byte[] ewkb) => ewkb.ST_StartPoint(), isDeterministic: true);
		CreateFunction("ST_SymDifference", (byte[] ewkb, byte[] other) => ewkb.ST_SymDifference(other), isDeterministic: true);
		CreateFunction("ST_SymmetricDifference", (byte[] ewkb, byte[] other) => ewkb.ST_SymmetricDifference(other), isDeterministic: true);
		CreateFunction("ST_Touches", (byte[] ewkb, byte[] other) => ewkb.ST_Touches(other), isDeterministic: true);
		CreateFunction("ST_Union", (byte[] ewkb, byte[] other) => ewkb.ST_Union(other), isDeterministic: true);
		CreateFunction("ST_Width", (byte[] ewkb) => ewkb.ST_Width(), isDeterministic: true);
		CreateFunction("ST_Within", (byte[] ewkb, byte[] other) => ewkb.ST_Within(other), isDeterministic: true);
	}
}
