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
		CreateFunction("ST_Area", (byte[] ewkb) => ST.ST_Area(ewkb), isDeterministic: true);
		CreateFunction("ST_AsText", (byte[] ewkb) => ST.ST_AsText(ewkb), isDeterministic: true);
		CreateFunction("ST_GeomFromText", (string text) => ST.ST_GeomFromText(text), isDeterministic: true);
		CreateFunction("ST_GeomFromText", (string text, int srid) => ST.ST_GeomFromText(text, srid), isDeterministic: true);
		CreateFunction("ST_Point", (double x, double y) => ST.ST_Point(x, y), isDeterministic: true);
		CreateFunction("ST_Point", (double x, double y, int srid) => ST.ST_Point(x, y, srid), isDeterministic: true);
		CreateFunction("ST_SRID", (byte[] ewkb) => ST.ST_SRID(ewkb), isDeterministic: true);
		CreateFunction("ST_X", (byte[] ewkb) => ST.ST_X(ewkb), isDeterministic: true);
		CreateFunction("ST_Y", (byte[] ewkb) => ST.ST_Y(ewkb), isDeterministic: true);
		CreateFunction("ST_AsBinary", (byte[] ewkb) => ST.ST_AsBinary(ewkb), isDeterministic: true);
		CreateFunction("ST_AsEWKT", (byte[] ewkb) => ST.ST_AsEWKT(ewkb), isDeterministic: true);
		CreateFunction("ST_Boundary", (byte[] ewkb) => ST.ST_Boundary(ewkb), isDeterministic: true);
		CreateFunction("ST_Buffer", (byte[] ewkb, double distance) => ST.ST_Buffer(ewkb, distance), isDeterministic: true);
		CreateFunction("ST_Centroid", (byte[] ewkb) => ST.ST_Centroid(ewkb), isDeterministic: true);
		CreateFunction("ST_Contains", (byte[] ewkb, byte[] other) => ST.ST_Contains(ewkb, other), isDeterministic: true);
		CreateFunction("ST_ConvexHull", (byte[] ewkb) => ST.ST_ConvexHull(ewkb), isDeterministic: true);
		CreateFunction("ST_CoveredBy", (byte[] ewkb, byte[] other) => ST.ST_CoveredBy(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Covers", (byte[] ewkb, byte[] other) => ST.ST_Covers(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Crosses", (byte[] ewkb, byte[] other) => ST.ST_Crosses(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Dimension", (byte[] ewkb) => ST.ST_Dimension(ewkb), isDeterministic: true);
		CreateFunction("ST_Difference", (byte[] ewkb, byte[] other) => ST.ST_Difference(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Disjoint", (byte[] ewkb, byte[] other) => ST.ST_Disjoint(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Distance", (byte[] ewkb, byte[] other) => ST.ST_Distance(ewkb, other), isDeterministic: true);
		CreateFunction("ST_DWithin", (byte[] ewkb, byte[] other, double distance) => ST.ST_DWithin(ewkb, other, distance), isDeterministic: true);
		CreateFunction("ST_Equals", (byte[] ewkb, byte[] other) => ST.ST_Equals(ewkb, other), isDeterministic: true);
		CreateFunction("ST_EndPoint", (byte[] ewkb) => ST.ST_EndPoint(ewkb), isDeterministic: true);
		CreateFunction("ST_Envelope", (byte[] ewkb) => ST.ST_Envelope(ewkb), isDeterministic: true);
		CreateFunction("ST_GeometryN", (byte[] ewkb, int index) => ST.ST_GeometryN(ewkb, index), isDeterministic: true);
		CreateFunction("ST_GeometryType", (byte[] ewkb) => ST.ST_GeometryType(ewkb), isDeterministic: true);
		CreateFunction("ST_Height", (byte[] ewkb) => ST.ST_Height(ewkb), isDeterministic: true);
		CreateFunction("ST_InteriorPoint", (byte[] ewkb) => ST.ST_InteriorPoint(ewkb), isDeterministic: true);
		CreateFunction("ST_Intersection", (byte[] ewkb, byte[] other) => ST.ST_Intersection(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Intersects", (byte[] ewkb, byte[] other) => ST.ST_Intersects(ewkb, other), isDeterministic: true);
		CreateFunction("ST_IsClosed", (byte[] ewkb) => ST.ST_IsClosed(ewkb), isDeterministic: true);
		CreateFunction("ST_IsEmpty", (byte[] ewkb) => ST.ST_IsEmpty(ewkb), isDeterministic: true);
		CreateFunction("ST_IsRectangle", (byte[] ewkb) => ST.ST_IsRectangle(ewkb), isDeterministic: true);
		CreateFunction("ST_IsRing", (byte[] ewkb) => ST.ST_IsRing(ewkb), isDeterministic: true);
		CreateFunction("ST_IsSimple", (byte[] ewkb) => ST.ST_IsSimple(ewkb), isDeterministic: true);
		CreateFunction("ST_IsValid", (byte[] ewkb) => ST.ST_IsValid(ewkb), isDeterministic: true);
		CreateFunction("ST_Length", (byte[] ewkb) => ST.ST_Length(ewkb), isDeterministic: true);
		CreateFunction("ST_MaxX", (byte[] ewkb) => ST.ST_MaxX(ewkb), isDeterministic: true);
		CreateFunction("ST_MaxY", (byte[] ewkb) => ST.ST_MaxY(ewkb), isDeterministic: true);
		CreateFunction("ST_MinX", (byte[] ewkb) => ST.ST_MinX(ewkb), isDeterministic: true);
		CreateFunction("ST_MinY", (byte[] ewkb) => ST.ST_MinY(ewkb), isDeterministic: true);
		CreateFunction("ST_NumGeometries", (byte[] ewkb) => ST.ST_NumGeometries(ewkb), isDeterministic: true);
		CreateFunction("ST_NumInteriorRings", (byte[] ewkb) => ST.ST_NumInteriorRings(ewkb), isDeterministic: true);
		CreateFunction("ST_NumPoints", (byte[] ewkb) => ST.ST_NumPoints(ewkb), isDeterministic: true);
		CreateFunction("ST_Overlaps", (byte[] ewkb, byte[] other) => ST.ST_Overlaps(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Perimeter", (byte[] ewkb) => ST.ST_Perimeter(ewkb), isDeterministic: true);
		CreateFunction("ST_PointN", (byte[] ewkb, int index) => ST.ST_PointN(ewkb, index), isDeterministic: true);
		CreateFunction("ST_PointOnSurface", (byte[] ewkb) => ST.ST_PointOnSurface(ewkb), isDeterministic: true);
		CreateFunction("ST_Relate", (byte[] ewkb, byte[] other, string intersectionPattern) => ST.ST_Relate(ewkb, other, intersectionPattern), isDeterministic: true);
		CreateFunction("ST_Reverse", (byte[] ewkb) => ST.ST_Reverse(ewkb), isDeterministic: true);
		CreateFunction("ST_SetSRID", (byte[] ewkb, int srid) => ST.ST_SetSRID(ewkb, srid), isDeterministic: true);
		CreateFunction("ST_StartPoint", (byte[] ewkb) => ST.ST_StartPoint(ewkb), isDeterministic: true);
		CreateFunction("ST_SymDifference", (byte[] ewkb, byte[] other) => ST.ST_SymDifference(ewkb, other), isDeterministic: true);
		CreateFunction("ST_SymmetricDifference", (byte[] ewkb, byte[] other) => ST.ST_SymmetricDifference(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Touches", (byte[] ewkb, byte[] other) => ST.ST_Touches(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Union", (byte[] ewkb, byte[] other) => ST.ST_Union(ewkb, other), isDeterministic: true);
		CreateFunction("ST_Width", (byte[] ewkb) => ST.ST_Width(ewkb), isDeterministic: true);
		CreateFunction("ST_Within", (byte[] ewkb, byte[] other) => ST.ST_Within(ewkb, other), isDeterministic: true);
	}
}
