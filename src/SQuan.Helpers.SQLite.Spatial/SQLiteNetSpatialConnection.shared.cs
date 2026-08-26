// SQLiteNetSpatialConnection.shared.cs

using SQLite;
using SQLitePCL;

namespace SQuan.Helpers.SQLite.Spatial;

/// <summary>
/// Extension methods for enabling EWKB spatial functions on SQLite connections.
/// </summary>
public partial class SQLiteNetSpatialConnection : SQLiteConnection, ISpatialConnection
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SQLiteNetSpatialConnection"/> class with the specified database path and an optional flag to store DateTime values as ticks.
	/// </summary>
	/// <param name="databasePath">The path to the SQLite database file.</param>
	/// <param name="openFlags">The flags to use when opening the SQLite database. Defaults to ReadWrite | Create | FullMutex.</param>
	/// <param name="storeDataTimeAsTicks">A value indicating whether to store DateTime values as ticks. Defaults to true.</param>
	public SQLiteNetSpatialConnection(
		string databasePath,
		SQLiteOpenFlags openFlags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex,
		bool storeDataTimeAsTicks = true)
		: base(databasePath, openFlags, storeDataTimeAsTicks)
	{
		EnableSpatialExtensions();
	}

	/// <summary>
	/// Applies spatial extensions to the given SQLite connection.
	/// </summary>
	void EnableSpatialExtensions()
	{
		CreateEWKBFunction(Handle, "ST_Area", ST.ST_Area);
		CreateEWKBFunction(Handle, "ST_AsBinary", ST.ST_AsBinary);
		CreateEWKBFunction(Handle, "ST_AsEWKT", ST.ST_AsEWKT);
		CreateEWKBFunction(Handle, "ST_AsText", ST.ST_AsText);
		CreateEWKBFunction(Handle, "ST_Boundary", ST.ST_Boundary);
		CreateEWKBDoubleFunction(Handle, "ST_Buffer", ST.ST_Buffer);
		CreateEWKBFunction(Handle, "ST_Centroid", ST.ST_Centroid);
		CreateEWKBEWKBFunction(Handle, "ST_Contains", ST.ST_Contains);
		CreateEWKBFunction(Handle, "ST_ConvexHull", ST.ST_ConvexHull);
		CreateEWKBEWKBFunction(Handle, "ST_CoveredBy", ST.ST_CoveredBy);
		CreateEWKBEWKBFunction(Handle, "ST_Covers", ST.ST_Covers);
		CreateEWKBEWKBFunction(Handle, "ST_Crosses", ST.ST_Crosses);
		CreateEWKBFunction(Handle, "ST_Dimension", ST.ST_Dimension);
		CreateEWKBEWKBFunction(Handle, "ST_Difference", ST.ST_Difference);
		CreateEWKBEWKBFunction(Handle, "ST_Disjoint", ST.ST_Disjoint);
		CreateEWKBEWKBFunction(Handle, "ST_Distance", ST.ST_Distance);
		CreateEWKBEWKBDoubleFunction(Handle, "ST_DWithin", ST.ST_DWithin);
		CreateEWKBEWKBFunction(Handle, "ST_Equals", ST.ST_Equals);
		CreateEWKBFunction(Handle, "ST_EndPoint", ST.ST_EndPoint);
		CreateEWKBFunction(Handle, "ST_Envelope", ST.ST_Envelope);
		CreateTextFunction(Handle, "ST_GeomFromText", text => ST.ST_GeomFromText(text));
		CreateTextIntegerFunction(Handle, "ST_GeomFromText", ST.ST_GeomFromText);
		CreateEWKBIntegerFunction(Handle, "ST_GeometryN", ST.ST_GeometryN);
		CreateEWKBFunction(Handle, "ST_GeometryType", ST.ST_GeometryType);
		CreateEWKBFunction(Handle, "ST_Height", ST.ST_Height);
		CreateEWKBFunction(Handle, "ST_InteriorPoint", ST.ST_InteriorPoint);
		CreateEWKBEWKBFunction(Handle, "ST_Intersection", ST.ST_Intersection);
		CreateEWKBEWKBFunction(Handle, "ST_Intersects", ST.ST_Intersects);
		CreateEWKBFunction(Handle, "ST_IsClosed", ST.ST_IsClosed);
		CreateEWKBFunction(Handle, "ST_IsEmpty", ST.ST_IsEmpty);
		CreateEWKBFunction(Handle, "ST_IsRectangle", ST.ST_IsRectangle);
		CreateEWKBFunction(Handle, "ST_IsRing", ST.ST_IsRing);
		CreateEWKBFunction(Handle, "ST_IsSimple", ST.ST_IsSimple);
		CreateEWKBFunction(Handle, "ST_IsValid", ST.ST_IsValid);
		CreateEWKBFunction(Handle, "ST_Length", ST.ST_Length);
		CreateEWKBFunction(Handle, "ST_MaxX", ST.ST_MaxX);
		CreateEWKBFunction(Handle, "ST_MaxY", ST.ST_MaxY);
		CreateEWKBFunction(Handle, "ST_MinX", ST.ST_MinX);
		CreateEWKBFunction(Handle, "ST_MinY", ST.ST_MinY);
		CreateEWKBFunction(Handle, "ST_NumGeometries", ST.ST_NumGeometries);
		CreateEWKBFunction(Handle, "ST_NumInteriorRings", ST.ST_NumInteriorRings);
		CreateEWKBFunction(Handle, "ST_NumPoints", ST.ST_NumPoints);
		CreateEWKBEWKBFunction(Handle, "ST_Overlaps", ST.ST_Overlaps);
		CreateEWKBFunction(Handle, "ST_Perimeter", ST.ST_Perimeter);
		CreateDoubleDoubleFunction(Handle, "ST_Point", (x, y) => ST.ST_Point(x, y));
		CreateDoubleDoubleIntegerFunction(Handle, "ST_Point", ST.ST_Point);
		CreateEWKBIntegerFunction(Handle, "ST_PointN", ST.ST_PointN);
		CreateEWKBFunction(Handle, "ST_PointOnSurface", ST.ST_PointOnSurface);
		CreateEWKBEWKBTextFunction(Handle, "ST_Relate", ST.ST_Relate);
		CreateEWKBFunction(Handle, "ST_Reverse", ST.ST_Reverse);
		CreateEWKBIntegerFunction(Handle, "ST_SetSRID", ST.ST_SetSRID);
		CreateEWKBFunction(Handle, "ST_SRID", ST.ST_SRID);
		CreateEWKBFunction(Handle, "ST_StartPoint", ST.ST_StartPoint);
		CreateEWKBEWKBFunction(Handle, "ST_SymDifference", ST.ST_SymDifference);
		CreateEWKBEWKBFunction(Handle, "ST_SymmetricDifference", ST.ST_SymmetricDifference);
		CreateEWKBEWKBFunction(Handle, "ST_Touches", ST.ST_Touches);
		CreateEWKBEWKBFunction(Handle, "ST_Union", ST.ST_Union);
		CreateEWKBFunction(Handle, "ST_Width", ST.ST_Width);
		CreateEWKBEWKBFunction(Handle, "ST_Within", ST.ST_Within);
		CreateEWKBFunction(Handle, "ST_X", ST.ST_X);
		CreateEWKBFunction(Handle, "ST_Y", ST.ST_Y);
	}

	static void CreateDoubleDoubleFunction<T>(sqlite3 handle, string name, Func<double, double, T> function) =>
		CreateFunction(handle, name, 2, args => function(raw.sqlite3_value_double(args[0]), raw.sqlite3_value_double(args[1])));

	static void CreateDoubleDoubleIntegerFunction<T>(sqlite3 handle, string name, Func<double, double, int, T> function) =>
		CreateFunction(handle, name, 3, args => function(raw.sqlite3_value_double(args[0]), raw.sqlite3_value_double(args[1]), raw.sqlite3_value_int(args[2])));

	static void CreateEWKBFunction<T>(sqlite3 handle, string name, Func<byte[], T> function) =>
		CreateFunction(handle, name, 1, args => function(GetEWKB(args[0])));

	static void CreateEWKBDoubleFunction<T>(sqlite3 handle, string name, Func<byte[], double, T> function) =>
		CreateFunction(handle, name, 2, args => function(GetEWKB(args[0]), raw.sqlite3_value_double(args[1])));

	static void CreateEWKBEWKBFunction<T>(sqlite3 handle, string name, Func<byte[], byte[], T> function) =>
		CreateFunction(handle, name, 2, args => function(GetEWKB(args[0]), GetEWKB(args[1])));

	static void CreateEWKBEWKBDoubleFunction<T>(sqlite3 handle, string name, Func<byte[], byte[], double, T> function) =>
		CreateFunction(handle, name, 3, args => function(GetEWKB(args[0]), GetEWKB(args[1]), raw.sqlite3_value_double(args[2])));

	static void CreateEWKBEWKBTextFunction<T>(sqlite3 handle, string name, Func<byte[], byte[], string, T> function) =>
		CreateFunction(handle, name, 3, args => function(GetEWKB(args[0]), GetEWKB(args[1]), GetText(args[2])));

	static void CreateEWKBIntegerFunction<T>(sqlite3 handle, string name, Func<byte[], int, T> function) =>
		CreateFunction(handle, name, 2, args => function(GetEWKB(args[0]), raw.sqlite3_value_int(args[1])));

	static void CreateTextFunction<T>(sqlite3 handle, string name, Func<string, T> function) =>
		CreateFunction(handle, name, 1, args => function(GetText(args[0])));

	static void CreateTextIntegerFunction<T>(sqlite3 handle, string name, Func<string, int, T> function) =>
		CreateFunction(handle, name, 2, args => function(GetText(args[0]), raw.sqlite3_value_int(args[1])));

	static void CreateFunction<T>(sqlite3 handle, string name, int argumentCount, Func<sqlite3_value[], T> function)
	{
		raw.sqlite3_create_function(handle, name, argumentCount, raw.SQLITE_UTF8 | raw.SQLITE_DETERMINISTIC, null, (sqlite3_context context, object userData, sqlite3_value[] args) =>
		{
			try
			{
				SetResult(context, function(args));
			}
			catch (Exception exception)
			{
				SetResultError(context, exception);
			}
		});
	}

	static byte[] GetEWKB(sqlite3_value value) => raw.sqlite3_value_blob(value).ToArray();

	static string GetText(sqlite3_value value) => raw.sqlite3_value_text(value).utf8_to_string();

	static void SetResult(sqlite3_context context, object? result)
	{
		switch (result)
		{
			case null:
				raw.sqlite3_result_null(context);
				break;
			case byte[] bytes:
				raw.sqlite3_result_blob(context, bytes);
				break;
			case string text:
				raw.sqlite3_result_text(context, utf8z.FromString(text));
				break;
			case double number:
				raw.sqlite3_result_double(context, number);
				break;
			case float number:
				raw.sqlite3_result_double(context, number);
				break;
			case int number:
				raw.sqlite3_result_int(context, number);
				break;
			case long number:
				raw.sqlite3_result_int64(context, number);
				break;
			case bool value:
				raw.sqlite3_result_int(context, value ? 1 : 0);
				break;
			default:
				throw new InvalidOperationException($"SQLite result type '{result.GetType().FullName}' is not supported.");
		}
	}

	static void SetResultError(sqlite3_context context, Exception exception) =>
		raw.sqlite3_result_error(context, utf8z.FromString(exception.Message));
}
