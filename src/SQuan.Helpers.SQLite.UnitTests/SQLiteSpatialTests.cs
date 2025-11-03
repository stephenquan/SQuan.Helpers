// SQLiteSpatialTests.cs

using SQuan.Helpers.SQLite.Spatial;

namespace SQuan.Helpers.SQLite.UnitTests;

public class SQLiteSpatialTests
{
	[Theory]
	[InlineData("SELECT ST_Point(3,4)", "POINT (3 4)")]
	[InlineData("SELECT ST_Envelope(ST_Buffer('POINT (5 5)', 5))", "POLYGON ((0 0, 0 10, 10 10, 10 0, 0 0))")]
	[InlineData("SELECT ST_Intersection('LINESTRING(0 0,10 10)','LINESTRING(0 10,10 0)')", "POINT (5 5)")]
	public void SQLiteSpatial_GeometryQuery_ReturnsExpectedGeometry(string sqlQuery, string expectedWkt)
	{
		SQLiteSpatialConnection db = new(":memory:");
		string? actualWkt = db.ExecuteScalar<string?>(sqlQuery);
		Assert.NotNull(actualWkt);
		Assert.Equal(expectedWkt, actualWkt);
	}

	[Theory]
	[InlineData("SELECT ST_Distance('POINT(0 0)','POINT(3 4)')", 5)]
	[InlineData("SELECT ST_Area(ST_Envelope(ST_Buffer('POINT (5 5)', 5)))", 100)]
	[InlineData("SELECT ST_Length(ST_Envelope(ST_Buffer('POINT (5 5)', 5)))", 40)]
	[InlineData("SELECT ST_Width(ST_Buffer('POINT (40 30)', 5))", 10)]
	[InlineData("SELECT ST_Height(ST_Buffer('POINT (40 30)', 5))", 10)]
	[InlineData("SELECT ST_X('POINT (40 30)')", 40)]
	[InlineData("SELECT ST_Y('POINT (40 30)')", 30)]
	[InlineData("SELECT ST_MinX(ST_Buffer('POINT (40 30)', 5))", 35)]
	[InlineData("SELECT ST_MaxX(ST_Buffer('POINT (40 30)', 5))", 45)]
	[InlineData("SELECT ST_MinY(ST_Buffer('POINT (40 30)', 5))", 25)]
	[InlineData("SELECT ST_MaxY(ST_Buffer('POINT (40 30)', 5))", 35)]
	public void SQLiteSpatial_NumericQuery_ReturnsExpectedNumber(string sqlQuery, double expectedResult)
	{
		SQLiteSpatialConnection db = new(":memory:");
		double? actualResult = db.ExecuteScalar<double?>(sqlQuery);
		Assert.NotNull(actualResult);
		Assert.Equal(expectedResult, actualResult);
	}

	[Theory]
	[InlineData("SELECT ST_IsValid('POINT (40 30)')", 1)]
	[InlineData("SELECT ST_IsRectangle(ST_Buffer('POINT (40 30)', 5))", 0)]
	[InlineData("SELECT ST_IsRectangle(ST_Envelope(ST_Buffer('POINT (40 30)', 5)))", 1)]
	[InlineData("SELECT ST_IsGeometry('POINT (40 30)')", 1)]
	[InlineData("SELECT ST_IsGeometry('I hate Mondays!')", 0)]
	public void SQLiteSpatial_BooleanQuery_ReturnsExpectedBoolean(string sqlQuery, int expectedResult)
	{
		SQLiteSpatialConnection db = new(":memory:");
		int? actualResult = db.ExecuteScalar<int?>(sqlQuery);
		Assert.NotNull(actualResult);
		Assert.Equal(expectedResult, actualResult);
	}

	[Theory]
	[InlineData("SELECT ST_SRID(ST_SetSRID('POINT (40 30)', 4326))", 4326)]
	public void SQLiteSpatial_IntegerQuery_ReturnsExpectedInteger(string sqlQuery, int expectedResult)
	{
		SQLiteSpatialConnection db = new(":memory:");
		int? actualResult = db.ExecuteScalar<int?>(sqlQuery);
		Assert.NotNull(actualResult);
		Assert.Equal(expectedResult, actualResult);
	}
}
