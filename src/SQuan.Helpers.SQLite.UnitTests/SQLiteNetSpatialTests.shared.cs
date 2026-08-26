// SQLiteNetSpatialTests.shared.cs

using SQuan.Helpers.SQLite.Spatial;

namespace SQuan.Helpers.SQLite.UnitTests;

public class SQLiteNetSpatialTests
{
	[Theory]
	[InlineData("SELECT ST_AsText(ST_Point(3,4))", "SRID=4326;POINT (3 4)")]
	[InlineData("SELECT ST_AsText(ST_Envelope(ST_Buffer(ST_GeomFromText('POINT (5 5)'), 5)))", "SRID=4326;POLYGON ((0 0, 0 10, 10 10, 10 0, 0 0))")]
	[InlineData("SELECT ST_AsText(ST_Intersection(ST_GeomFromText('LINESTRING(0 0,10 10)'),ST_GeomFromText('LINESTRING(0 10,10 0)')))", "SRID=4326;POINT (5 5)")]
	[InlineData("SELECT ST_AsText(ST_Centroid(ST_GeomFromText('POLYGON ((0 0, 0 10, 10 10, 10 0, 0 0))')))", "SRID=4326;POINT (5 5)")]
	public void SQLiteSpatial_GeometryQuery_ReturnsExpectedGeometry(string sqlQuery, string expectedWkt)
	{
		SQLiteNetSpatialConnection db = new(":memory:");

		string? actualWkt = db.ExecuteScalar<string?>(sqlQuery);

		Assert.NotNull(actualWkt);
		Assert.Equal(expectedWkt, actualWkt);
	}

	[Theory]
	[InlineData("SELECT ST_Distance(ST_GeomFromText('POINT(0 0)'),ST_GeomFromText('POINT(3 4)'))", 5)]
	[InlineData("SELECT ST_Area(ST_Envelope(ST_Buffer(ST_GeomFromText('POINT (5 5)'), 5)))", 100)]
	[InlineData("SELECT ST_Length(ST_Envelope(ST_Buffer(ST_GeomFromText('POINT (5 5)'), 5)))", 40)]
	[InlineData("SELECT ST_Width(ST_Buffer(ST_GeomFromText('POINT (40 30)'), 5))", 10)]
	[InlineData("SELECT ST_Height(ST_Buffer(ST_GeomFromText('POINT (40 30)'), 5))", 10)]
	[InlineData("SELECT ST_X(ST_GeomFromText('POINT (40 30)'))", 40)]
	[InlineData("SELECT ST_Y(ST_GeomFromText('POINT (40 30)'))", 30)]
	[InlineData("SELECT ST_MinX(ST_Buffer(ST_GeomFromText('POINT (40 30)'), 5))", 35)]
	[InlineData("SELECT ST_MaxX(ST_Buffer(ST_GeomFromText('POINT (40 30)'), 5))", 45)]
	[InlineData("SELECT ST_MinY(ST_Buffer(ST_GeomFromText('POINT (40 30)'), 5))", 25)]
	[InlineData("SELECT ST_MaxY(ST_Buffer(ST_GeomFromText('POINT (40 30)'), 5))", 35)]
	public void SQLiteSpatial_NumericQuery_ReturnsExpectedNumber(string sqlQuery, double expectedResult)
	{
		SQLiteNetSpatialConnection db = new(":memory:");

		double? actualResult = db.ExecuteScalar<double?>(sqlQuery);

		Assert.NotNull(actualResult);
		Assert.Equal(expectedResult, actualResult);
	}

	[Theory]
	[InlineData("SELECT ST_IsValid(ST_GeomFromText('POINT (40 30)'))", 1)]
	[InlineData("SELECT ST_IsRectangle(ST_Buffer(ST_GeomFromText('POINT (40 30)'), 5))", 0)]
	[InlineData("SELECT ST_IsRectangle(ST_Envelope(ST_Buffer(ST_GeomFromText('POINT (40 30)'), 5)))", 1)]
	[InlineData("SELECT ST_DWithin(ST_GeomFromText('POINT (0 0)'),ST_GeomFromText('POINT (3 4)'),5)", 1)]
	[InlineData("SELECT ST_Contains(ST_GeomFromText('POLYGON ((0 0, 0 10, 10 10, 10 0, 0 0))'),ST_Point(5,5))", 1)]
	public void SQLiteSpatial_BooleanQuery_ReturnsExpectedBoolean(string sqlQuery, int expectedResult)
	{
		SQLiteNetSpatialConnection db = new(":memory:");

		int? actualResult = db.ExecuteScalar<int?>(sqlQuery);

		Assert.NotNull(actualResult);
		Assert.Equal(expectedResult, actualResult);
	}

	[Theory]
	[InlineData("SELECT ST_SRID(ST_SetSRID(ST_GeomFromText('POINT (40 30)', 0), 4326))", 4326)]
	public void SQLiteSpatial_IntegerQuery_ReturnsExpectedInteger(string sqlQuery, int expectedResult)
	{
		SQLiteNetSpatialConnection db = new(":memory:");

		int? actualResult = db.ExecuteScalar<int?>(sqlQuery);

		Assert.NotNull(actualResult);
		Assert.Equal(expectedResult, actualResult);
	}

	[Fact]
	public void SQLiteSpatial_GeometryResult_ReturnsBlob()
	{
		SQLiteNetSpatialConnection db = new(":memory:");

		string? storageType = db.ExecuteScalar<string?>("SELECT typeof(ST_Point(3,4))");

		Assert.Equal("blob", storageType);
	}
}
