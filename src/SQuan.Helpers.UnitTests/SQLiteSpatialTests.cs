using SQuan.Helpers.SQLiteSpatial;

namespace SQuan.Helpers.Maui.UnitTests;

public class SQLiteSpatialTests
{
	[Theory]
	[InlineData("SELECT ST_Point(3,4)", "POINT (3 4)")]
	[InlineData("SELECT ST_Envelope(ST_Buffer('POINT (5 5)', 5))", "POLYGON ((0 0, 0 10, 10 10, 10 0, 0 0))")]
	[InlineData("SELECT ST_Intersection('LINESTRING(0 0,10 10)','LINESTRING(0 10,10 0)')", "POINT (5 5)")]
	public void SQLiteSpatial_SpatialQuery_ReturnsExpectedGeometry(string sqlQuery, string expectedWkt)
	{
		SQLiteSpatialConnection db = new(":memory:");
		string? actualWkt = db.ExecuteScalar<string?>(sqlQuery);
		Assert.NotNull(actualWkt);
		Assert.Equal(expectedWkt, actualWkt);
	}

	[Theory]
	[InlineData("SELECT ST_Distance('POINT(0 0)','POINT(3 4)')", 5)]
	[InlineData("SELECT ST_Area(ST_Envelope(ST_Buffer('POINT (5 5)', 5)))", 100)]
	public void SQLiteSpatial_SpatialQuery_ReturnsExpectedNumber(string sqlQuery, double expectedResult)
	{
		SQLiteSpatialConnection db = new(":memory:");
		double? actualResult = db.ExecuteScalar<double?>(sqlQuery);
		Assert.NotNull(actualResult);
		Assert.Equal(expectedResult, actualResult);
	}

}
