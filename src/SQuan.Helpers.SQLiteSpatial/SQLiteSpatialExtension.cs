using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using SQLite;
using SQLitePCL;

namespace SQuan.Helpers.SQLiteSpatial;

/// <summary>
/// Provides extension methods for enabling spatial functions in SQLite connections.
/// </summary>
class SQLiteSpatialExtensions
{
	/// <summary>
	/// Applies spatial extensions to the given SQLite connection.
	/// </summary>
	/// <param name="db"></param>
	internal static void EnableSpatialExtensions(SQLiteConnection db)
	{
		var dbHandle = db.Handle;
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Area", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Area);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Boundary", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Boundary);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Buffer", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Buffer);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Centroid", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Centroid);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Contains", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Contains);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_ConvexHull", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_ConvexHull);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_CoveredBy", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_CoveredBy);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Covers", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Covers);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Crosses", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Crosses);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Difference", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Difference);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Disjoint", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Disjoint);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Distance", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Distance);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Equals", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Equals);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_EqualsExact", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_EqualsExact);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_EqualsNormalized", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_EqualsNormalized);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_EqualsTopologically", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_EqualsTopologically);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Envelope", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Envelope);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_GeometryType", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_GeometryType);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_InteriorPoint", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_InteriorPoint);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Intersection", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Intersection);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Intersects", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Intersects);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_IsEmpty", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_IsEmpty);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_IsRectangle", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_IsRectangle);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_IsSimple", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_IsSimple);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_IsValid", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_IsValid);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Length", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Length);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Point", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Point);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Reverse", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Reverse);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_SetSRID", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_SetSRID);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_SRID", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_SRID);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_SymmetricDifference", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_SymmetricDifference);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Touches", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Touches);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Union", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Union);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Within", 2, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Within);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_X", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_X);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_Y", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, null, ST_Y);
	}

	/// <summary>
	/// Implements the ST_Area function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Area(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<double?>(ctx, user_data, args, (g) => g?.Area);

	/// <summary>
	/// Implements the ST_Boundary function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Boundary(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<string?>(ctx, user_data, args,
			(g) => g?.Boundary is NetTopologySuite.Geometries.Geometry result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_Buffer function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Buffer(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryDoubleFunction<string?>(ctx, user_data, args,
			(g, d) => g?.Buffer(d) is NetTopologySuite.Geometries.Geometry b ? b.AsText() : null);

	/// <summary>
	/// Implements the ST_Centroid function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Centroid(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<string?>(ctx, user_data, args,
			(g) => g?.Centroid is NetTopologySuite.Geometries.Point p ? p.AsText() : null);

	/// <summary>
	/// Implements the ST_Contains function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Contains(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.Contains(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_ConvexHull function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_ConvexHull(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<string?>(ctx, user_data, args,
			(g) => g?.ConvexHull() is NetTopologySuite.Geometries.Geometry result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_CoveredBy function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_CoveredBy(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.CoveredBy(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_Covers function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Covers(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.Covers(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_Crosses function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Crosses(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.Crosses(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_Difference function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Difference(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<string?>(ctx, user_data, args,
			(g, g2) => g?.Difference(g2) is NetTopologySuite.Geometries.Geometry result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_Disjoint function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Disjoint(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.Disjoint(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_Distance function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Distance(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<double?>(ctx, user_data, args, (g, g2) => g?.Distance(g2));

	/// <summary>
	/// Implements the ST_Envelope function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Envelope(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<string?>(ctx, user_data, args,
			(g) => g?.Envelope is NetTopologySuite.Geometries.Geometry result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_Equals function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Equals(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.Equals(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_EqualsExact function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_EqualsExact(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.EqualsExact(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_EqualsNormalized function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_EqualsNormalized(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.EqualsNormalized(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_EqualsTopologically function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_EqualsTopologically(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.EqualsTopologically(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_GeometryType function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_GeometryType(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<string?>(ctx, user_data, args, (g) => g?.GeometryType);

	/// <summary>
	/// Implements the ST_InteriorPoint function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_InteriorPoint(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<string?>(ctx, user_data, args,
			(g) => g?.InteriorPoint is NetTopologySuite.Geometries.Point result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_Intersection function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Intersection(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<string?>(ctx, user_data, args,
			(g, g2) => g?.Intersection(g2) is NetTopologySuite.Geometries.Geometry result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_Intersects function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Intersects(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.Intersects(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_IsEmpty function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_IsEmpty(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<int>(ctx, user_data, args, (g) => g?.IsEmpty ?? true ? 1 : 0);

	/// <summary>
	/// Implements the ST_IsRectangle function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_IsRectangle(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<int>(ctx, user_data, args, (g) => g?.IsRectangle ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_IsSimple function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_IsSimple(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<int>(ctx, user_data, args, (g) => g?.IsSimple ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_IsValid function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_IsValid(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<int>(ctx, user_data, args, (g) => g?.IsValid ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_Length function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Length(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<double?>(ctx, user_data, args, (g) => g?.Length);

	/// <summary>
	/// Implements the ST_Point function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Point(sqlite3_context ctx, object user_data, sqlite3_value[] args)
	{
		try
		{
			double x = raw.sqlite3_value_double(args[0]);
			double y = raw.sqlite3_value_double(args[1]);
			var geometry = new NetTopologySuite.Geometries.Point(x, y);
			SetResult(ctx, geometry.AsText());
		}
		catch (Exception ex)
		{
			SetResultError(ctx, ex);
		}
	}

	/// <summary>
	/// Implements the ST_Reverse function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Reverse(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<string?>(ctx, user_data, args,
			(g) => g?.Reverse() is NetTopologySuite.Geometries.Geometry result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_SymmetricDifference function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_SymmetricDifference(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<string?>(ctx, user_data, args,
			(g, g2) => g?.SymmetricDifference(g2) is NetTopologySuite.Geometries.Geometry result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_SetSRID function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_SetSRID(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryIntegerFunction<string?>(ctx, user_data, args,
			(g, i) => (g is not null && (g.SRID = i) == i) ? g.AsText() : null);

	/// <summary>
	/// Implements the ST_SRID function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_SRID(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<int?>(ctx, user_data, args, (g) => g?.SRID);

	/// <summary>
	/// Implements the ST_Touches function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Touches(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.Touches(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_Union function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Union(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<string?>(ctx, user_data, args,
			(g, g2) => g?.Union(g2) is NetTopologySuite.Geometries.Geometry result ? result.AsText() : null);

	/// <summary>
	/// Implements the ST_Within function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Within(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryGeometryFunction<int>(ctx, user_data, args, (g, g2) => g?.Within(g2) ?? false ? 1 : 0);

	/// <summary>
	/// Implements the ST_X function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_X(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<double?>(ctx, user_data, args, (g) => g is null ? null : g.Envelope.Centroid.X);

	/// <summary>
	/// Implements the ST_Y function for SQLite.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	static void ST_Y(sqlite3_context ctx, object user_data, sqlite3_value[] args)
		=> ST_GeometryFunction<double?>(ctx, user_data, args, (g) => g is null ? null : g.Envelope.Centroid.Y);

	/// <summary>
	/// Internal helper to handle geometry-based SQLite functions.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	/// <param name="func"></param>
	static void ST_GeometryFunction<T>(sqlite3_context ctx, object user_data, sqlite3_value[] args, Func<Geometry?, T> func)
	{
		try
		{
			string wkt = raw.sqlite3_value_text(args[0]).utf8_to_string();
			WKTReader reader = new();
			var geometry = reader.Read(wkt);
			SetResult(ctx, func(geometry));
		}
		catch (Exception ex)
		{
			SetResultError(ctx, ex);
		}
	}

	/// <summary>
	/// Internal helper to handle geometry and double-based SQLite functions.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	/// <param name="func"></param>
	static void ST_GeometryDoubleFunction<T>(sqlite3_context ctx, object user_data, sqlite3_value[] args, Func<Geometry?, double, T> func)
	{
		try
		{
			string wkt = raw.sqlite3_value_text(args[0]).utf8_to_string();
			double d = raw.sqlite3_value_double(args[1]);
			WKTReader reader = new();
			var geometry = reader.Read(wkt);
			SetResult(ctx, func(geometry, d));
		}
		catch (Exception ex)
		{
			SetResultError(ctx, ex);
		}
	}

	/// <summary>
	/// Internal helper to handle two-geometry-based SQLite functions.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	/// <param name="func"></param>
	static void ST_GeometryGeometryFunction<T>(sqlite3_context ctx, object user_data, sqlite3_value[] args, Func<Geometry?, Geometry?, T> func)
	{
		try
		{
			string wkt = raw.sqlite3_value_text(args[0]).utf8_to_string();
			string wkt2 = raw.sqlite3_value_text(args[1]).utf8_to_string();
			WKTReader reader = new();
			var geometry = reader.Read(wkt);
			var geometry2 = reader.Read(wkt2);
			SetResult(ctx, func(geometry, geometry2));
		}
		catch (Exception ex)
		{
			SetResultError(ctx, ex);
		}
	}

	/// <summary>
	/// Internal helper to handle geometry and double-based SQLite functions.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="user_data"></param>
	/// <param name="args"></param>
	/// <param name="func"></param>
	static void ST_GeometryIntegerFunction<T>(sqlite3_context ctx, object user_data, sqlite3_value[] args, Func<Geometry?, int, T> func)
	{
		try
		{
			string wkt = raw.sqlite3_value_text(args[0]).utf8_to_string();
			int i = raw.sqlite3_value_int(args[1]);
			WKTReader reader = new();
			var geometry = reader.Read(wkt);
			SetResult(ctx, func(geometry, i));
		}
		catch (Exception ex)
		{
			SetResultError(ctx, ex);
		}
	}


	/// <summary>
	/// Internal helper to set the result of a SQLite function call.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="result"></param>
	static void SetResult(sqlite3_context ctx, object? result)
	{
		switch (result)
		{
			case null:
				raw.sqlite3_result_null(ctx);
				break;
			case string s:
				raw.sqlite3_result_text(ctx, utf8z.FromString(s));
				break;
			case double d:
				raw.sqlite3_result_double(ctx, d);
				break;
			case int i:
				raw.sqlite3_result_int(ctx, i);
				break;
			default:
				raw.sqlite3_result_null(ctx);
				break;
		}
	}

	/// <summary>
	/// Internal helper to set an error result for a SQLite function call.
	/// </summary>
	/// <param name="ctx"></param>
	/// <param name="ex"></param>
	static void SetResultError(sqlite3_context ctx, Exception ex)
	{
		raw.sqlite3_result_error(ctx, utf8z.FromString(ex.Message));
		raw.sqlite3_result_error_code(ctx, raw.SQLITE_ERROR);
	}
}
