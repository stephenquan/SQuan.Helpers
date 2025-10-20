// SQLiteSpatialExtension.cs

using System.Runtime.Caching;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using SQLite;
using SQLitePCL;

namespace SQuan.Helpers.SQLiteSpatial;

/// <summary>
/// Extension methods for enabling and using spatial functions in SQLite connections.
/// Includes helpers for registering NetTopologySuite-based spatial functions and caching WKT geometries for performance.
/// </summary>
public static class SQLiteSpatialExtensions
{
	static readonly MemoryCache wktGeometryCache = new MemoryCache("WKTGeometryCache");
	static readonly MemoryCache wktFrequencyCache = new MemoryCache("WKTFrequencyCache");
	static readonly WKTReader wktReader = new WKTReader();

	/// <summary>
	/// Gets or sets the threshold for promoting WKT strings from frequency cache to geometry cache.
	/// </summary>
	public static int WKTFrequencyPromotionThreshold { get; set; } = 5;

	/// <summary>
	/// Gets or sets the expiration time for entries in the WKT frequency cache.
	/// </summary>
	public static int WKTFrequencyCacheExpirationSeconds { get; set; } = 5;

	/// <summary>
	/// Gets or sets the expiration time for entries in the WKT geometry cache.
	/// </summary>
	public static int WKTGeometryCacheExpirationSeconds { get; set; } = 60;

	/// <summary>
	/// Obtains a Geometry object from its WKT representation, utilizing caching for performance.
	/// </summary>
	/// <param name="wkt">The WKT representation of the geometry.</param>
	/// <returns>The Geometry object or null.</returns>
	public static Geometry? ToGeometry(this string wkt)
	{
		if (string.IsNullOrWhiteSpace(wkt))
		{
			return null;
		}

		// Check long-term cache first
		if (wktGeometryCache.Contains(wkt))
		{
			return (Geometry)wktGeometryCache.Get(wkt);
		}

		// Count frequency in short-term cache
		int count = (wktFrequencyCache.Get(wkt) as int?) ?? 0;
		count++;
		wktFrequencyCache.Set(wkt, count, DateTimeOffset.Now.AddSeconds(WKTFrequencyCacheExpirationSeconds));

		// Promote if threshold reached
		if (count >= WKTFrequencyPromotionThreshold)
		{
			var geometry = wktReader.Read(wkt);
			wktGeometryCache.Set(wkt, geometry, DateTimeOffset.Now.AddSeconds(WKTGeometryCacheExpirationSeconds));
			return geometry;
		}

		// Otherwise, deserialize without caching
		return wktReader.Read(wkt);
	}

	/// <summary>
	/// Applies spatial extensions to the given SQLite connection.
	/// </summary>
	/// <param name="db">A SQLite connection.</param>
	public static void EnableSpatialExtensions(this SQLiteConnection db)
	{
		CreateGeometryFunction<double?>(db.Handle, "ST_Area", (g) => g?.Area);
		CreateGeometryFunction<string?>(db.Handle, "ST_Boundary", (g) => g?.Boundary?.AsText());
		CreateGeometryDoubleFunction<string?>(db.Handle, "ST_Buffer", (g, d) => g?.Buffer(d)?.AsText());
		CreateGeometryFunction<string?>(db.Handle, "ST_Centroid", (g) => g?.Centroid?.AsText());
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_Contains", (g, g2) => g?.Contains(g2) ?? false ? 1 : 0);
		CreateGeometryFunction<string?>(db.Handle, "ST_ConvexHull", (g) => g?.ConvexHull()?.AsText());
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_CoveredBy", (g, g2) => g?.CoveredBy(g2) ?? false ? 1 : 0);
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_Covers", (g, g2) => g?.Covers(g2) ?? false ? 1 : 0);
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_Crosses", (g, g2) => g?.Crosses(g2) ?? false ? 1 : 0);
		CreateGeometryGeometryFunction<string?>(db.Handle, "ST_Difference", (g, g2) => g?.Difference(g2)?.ToText());
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_Disjoint", (g, g2) => g?.Disjoint(g2) ?? false ? 1 : 0);
		CreateGeometryGeometryFunction<double?>(db.Handle, "ST_Distance", (g, g2) => g?.Distance(g2));
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_Equals", (g, g2) => g?.Equals(g2) ?? false ? 1 : 0);
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_EqualsExact", (g, g2) => g?.EqualsExact(g2) ?? false ? 1 : 0);
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_EqualsNormalized", (g, g2) => g?.EqualsNormalized(g2) ?? false ? 1 : 0);
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_EqualsTopologically", (g, g2) => g?.EqualsTopologically(g2) ?? false ? 1 : 0);
		CreateGeometryFunction<string?>(db.Handle, "ST_Envelope", (g) => g?.Envelope?.AsText());
		CreateGeometryFunction<string?>(db.Handle, "ST_GeometryType", (g) => g?.GeometryType);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_Height", (s) => s?.Height);
		CreateGeometryFunction<string?>(db.Handle, "ST_IsRectangle", (g) => g?.InteriorPoint?.AsText());
		CreateGeometryGeometryFunction<string?>(db.Handle, "ST_Intersection", (g, g2) => g?.Intersection(g2)?.AsText());
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_Intersects", (g, g2) => g?.Intersects(g2) ?? false ? 1 : 0);
		CreateGeometryFunction<int>(db.Handle, "ST_IsEmpty", (g) => g?.IsEmpty ?? false ? 1 : 0);
		CreateGeometryFunction<int>(db.Handle, "ST_IsRectangle", (g) => g?.IsRectangle ?? false ? 1 : 0);
		CreateGeometryFunction<int>(db.Handle, "ST_IsSimple", (g) => g?.IsSimple ?? false ? 1 : 0);
		CreateGeometryFunction<int>(db.Handle, "ST_IsValid", (g) => g?.IsValid ?? false ? 1 : 0);
		CreateGeometryFunction<double?>(db.Handle, "ST_Length", (g) => g?.Length);
		CreateDoubleDoubleFunction<string>(db.Handle, "ST_Point", (x, y) => new NetTopologySuite.Geometries.Point(x, y).AsText());
		CreateGeometryFunction<string?>(db.Handle, "ST_SRID", (g) => g?.Reverse()?.ToText());
		CreateGeometryIntegerFunction<string?>(db.Handle, "ST_SetSRID", (g, i) => { g?.SRID = i; return g?.ToText(); });
		CreateGeometryFunction<int?>(db.Handle, "ST_SRID", (g) => g?.SRID);
		CreateGeometryGeometryFunction<string?>(db.Handle, "ST_SymmetricDifference", (g, g2) => g?.SymmetricDifference(g2)?.ToText());
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_Touches", (g, g2) => g?.Touches(g2) ?? false ? 1 : 0);
		CreateGeometryGeometryFunction<string?>(db.Handle, "ST_Union", (g, g2) => g?.Union(g2)?.ToText());
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_Width", (s) => s?.Width);
		CreateGeometryGeometryFunction<int>(db.Handle, "ST_Within", (g, g2) => g?.Within(g2) ?? false ? 1 : 0);
		CreateGeometryFunction<double?>(db.Handle, "ST_X", (g) => g?.Centroid.X);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_XMax", (s) => s?.X2);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_XMin", (s) => s?.X1);
		CreateGeometryFunction<double?>(db.Handle, "ST_Y", (g) => g?.Centroid.Y);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_YMax", (s) => s?.Y2);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_YMin", (s) => s?.Y1);
	}

	/// <summary>
	/// Creates a custom SQLite function that accepts two double parameters and returns a value of type <typeparamref name="T"/>.
	/// This is useful for registering mathematical or spatial functions (e.g., point creation) with SQLite.
	/// The function is registered with the specified name and flags, and the provided delegate is invoked with the double arguments.
	/// </summary>
	/// <typeparam name="T">The return type of the function.</typeparam>
	/// <param name="handle">A SQLite3 database handle.</param>
	/// <param name="name">The name of the SQLite function to register.</param>
	/// <param name="func">A delegate that takes two double arguments and returns a value of type <typeparamref name="T"/>.</param>
	/// <param name="flags">SQLite function flags (default: SQLITE_UTF8 | SQLITE_DETERMINISTIC).</param>
	static void CreateDoubleDoubleFunction<T>(sqlite3 handle, string name, Func<double, double, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
	{
		SQLitePCL.raw.sqlite3_create_function(handle, name, 2, flags, null, (sqlite3_context ctx, object user_data, sqlite3_value[] args) =>
		{
			try
			{
				double d = raw.sqlite3_value_double(args[0]);
				double d2 = raw.sqlite3_value_double(args[1]);
				SetResult(ctx, func(d, d2));
			}
			catch (Exception ex)
			{
				SetResultError(ctx, ex);
			}
		});
	}

	/// <summary>
	/// Creates a custom SQLite function that accepts a single geometry argument and returns a value of type <typeparamref name="T"/>.
	/// This is useful for registering spatial functions (e.g., area, centroid, envelope) with SQLite.
	/// The function is registered with the specified name and flags, and the provided delegate is invoked with the geometry argument.
	/// </summary>
	/// <typeparam name="T">The return type of the function.</typeparam>
	/// <param name="handle">A SQLite3 database handle.</param>
	/// <param name="name">The name of the SQLite function to register.</param>
	/// <param name="func">A delegate that takes a geometry argument and returns a value of type <typeparamref name="T"/>.</param>
	/// <param name="flags">SQLite function flags (default: SQLITE_UTF8 | SQLITE_DETERMINISTIC).</param>
	static void CreateGeometryFunction<T>(sqlite3 handle, string name, Func<NetTopologySuite.Geometries.Geometry?, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
	{
		SQLitePCL.raw.sqlite3_create_function(handle, name, 1, flags, null, (sqlite3_context ctx, object user_data, sqlite3_value[] args) =>
		{
			try
			{
				var geometry = ToGeometry(raw.sqlite3_value_text(args[0]).utf8_to_string());
				SetResult(ctx, func(geometry));
			}
			catch (Exception ex)
			{
				SetResultError(ctx, ex);
			}
		});
	}

	/// <summary>
	/// Creates a custom SQLite function that accepts a geometry argument and a double argument, and returns a value of type <typeparamref name="T"/>.
	/// This is useful for registering spatial functions that require both a geometry and a double parameter (e.g., buffer operations) with SQLite.
	/// The function is registered with the specified name and flags, and the provided delegate is invoked with the geometry and double arguments.
	/// </summary>
	/// <typeparam name="T">The return type of the function.</typeparam>
	/// <param name="handle">A SQLite3 database handle.</param>
	/// <param name="name">The name of the SQLite function to register.</param>
	/// <param name="func">A delegate that takes a geometry argument and a double argument, and returns a value of type <typeparamref name="T"/>.</param>
	/// <param name="flags">SQLite function flags (default: SQLITE_UTF8 | SQLITE_DETERMINISTIC).</param>
	static void CreateGeometryDoubleFunction<T>(sqlite3 handle, string name, Func<NetTopologySuite.Geometries.Geometry?, double, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
	{
		SQLitePCL.raw.sqlite3_create_function(handle, name, 2, flags, null, (sqlite3_context ctx, object user_data, sqlite3_value[] args) =>
		{
			try
			{
				var geometry = ToGeometry(raw.sqlite3_value_text(args[0]).utf8_to_string());
				double d = raw.sqlite3_value_double(args[1]);
				SetResult(ctx, func(geometry, d));
			}
			catch (Exception ex)
			{
				SetResultError(ctx, ex);
			}
		});
	}

	/// <summary>
	/// Creates a custom SQLite function that accepts two geometry arguments and returns a value of type <typeparamref name="T"/>.
	/// This is useful for registering spatial functions that operate on two geometries (e.g., intersection, union, contains) with SQLite.
	/// The function is registered with the specified name and flags, and the provided delegate is invoked with the geometry arguments.
	/// </summary>
	/// <typeparam name="T">The return type of the function.</typeparam>
	/// <param name="handle">A SQLite3 database handle.</param>
	/// <param name="name">The name of the SQLite function to register.</param>
	/// <param name="func">A delegate that takes two geometry arguments and returns a value of type <typeparamref name="T"/>.</param>
	/// <param name="flags">SQLite function flags (default: SQLITE_UTF8 | SQLITE_DETERMINISTIC).</param>
	static void CreateGeometryGeometryFunction<T>(sqlite3 handle, string name, Func<NetTopologySuite.Geometries.Geometry?, NetTopologySuite.Geometries.Geometry?, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
	{
		SQLitePCL.raw.sqlite3_create_function(handle, name, 2, flags, null, (sqlite3_context ctx, object user_data, sqlite3_value[] args) =>
		{
			try
			{
				var geometry = ToGeometry(raw.sqlite3_value_text(args[0]).utf8_to_string());
				var geometry2 = ToGeometry(raw.sqlite3_value_text(args[1]).utf8_to_string());
				SetResult(ctx, func(geometry, geometry2));
			}
			catch (Exception ex)
			{
				SetResultError(ctx, ex);
			}
		});
	}

	/// <summary>
	/// Creates a custom SQLite function that accepts a geometry argument and an integer argument, and returns a value of type <typeparamref name="T"/>.
	/// This is useful for registering spatial functions that require both a geometry and an integer parameter (e.g., setting SRID) with SQLite.
	/// The function is registered with the specified name and flags, and the provided delegate is invoked with the geometry and integer arguments.
	/// </summary>
	/// <typeparam name="T">The return type of the function.</typeparam>
	/// <param name="handle">A SQLite3 database handle.</param>
	/// <param name="name">The name of the SQLite function to register.</param>
	/// <param name="func">A delegate that takes a geometry argument and an integer argument, and returns a value of type <typeparamref name="T"/>.</param>
	/// <param name="flags">SQLite function flags (default: SQLITE_UTF8 | SQLITE_DETERMINISTIC).</param>
	static void CreateGeometryIntegerFunction<T>(sqlite3 handle, string name, Func<NetTopologySuite.Geometries.Geometry?, int, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
	{
		SQLitePCL.raw.sqlite3_create_function(handle, name, 2, flags, null, (sqlite3_context ctx, object user_data, sqlite3_value[] args) =>
		{
			try
			{
				var geometry = ToGeometry(raw.sqlite3_value_text(args[0]).utf8_to_string());
				int i = raw.sqlite3_value_int(args[1]);
				SetResult(ctx, func(geometry, i));
			}
			catch (Exception ex)
			{
				SetResultError(ctx, ex);
			}
		});
	}

	/// <summary>
	/// Creates a custom SQLite function that accepts a spatial index argument and returns a value of type <typeparamref name="T"/>.
	/// This is useful for registering spatial index functions (e.g., bounding box calculations) with SQLite.
	/// The function is registered with the specified name and flags, and the provided delegate is invoked with the spatial index argument.
	/// </summary>
	/// <typeparam name="T">The return type of the function.</typeparam>
	/// <param name="handle">A SQLite3 database handle.</param>
	/// <param name="name">The name of the SQLite function to register.</param>
	/// <param name="func">A delegate that takes a spatial index argument and returns a value of type <typeparamref name="T"/>.</param>
	/// <param name="flags">SQLite function flags (default: SQLITE_UTF8 | SQLITE_DETERMINISTIC).</param>
	static void CreateSpatialIndexFunction<T>(sqlite3 handle, string name, Func<SpatialIndex?, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
	{
		SQLitePCL.raw.sqlite3_create_function(handle, name, 1, flags, null, (sqlite3_context ctx, object user_data, sqlite3_value[] args) =>
		{
			try
			{
				var geometry = ToGeometry(raw.sqlite3_value_text(args[0]).utf8_to_string());
				if (geometry is NetTopologySuite.Geometries.Point p)
				{
					SetResult(ctx, func(new SpatialIndex(p.X, p.Y, p.X, p.Y)));
					return;
				}
				if (geometry?.Envelope is NetTopologySuite.Geometries.Geometry e && e.Coordinates.Length == 5)
				{
					SetResult(ctx, func(new SpatialIndex(e.Coordinates[0].X, e.Coordinates[0].Y, e.Coordinates[2].X, e.Coordinates[2].Y)));
					return;
				}
				SetResult(ctx, func(null));
			}
			catch (Exception ex)
			{
				SetResultError(ctx, ex);
			}
		});
	}


	/// <summary>
	/// Sets the result of a SQLite function call based on the type of the provided result object.
	/// </summary>
	/// <param name="ctx">The SQLite function context.</param>
	/// <param name="result">The result object to be returned to SQLite. Supported types are <c>null</c>, <see cref="string"/>, <see cref="double"/>, and <see cref="int"/>.</param>
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
	/// Sets an error result for a SQLite function call.
	/// This helper method is used within custom SQLite function delegates to report errors back to SQLite.
	/// It sets the error message and error code in the SQLite context, ensuring that the calling SQL statement
	/// receives an appropriate error response.
	/// </summary>
	/// <param name="ctx">The SQLite function context to set the error result for.</param>
	/// <param name="ex">The exception containing the error details to report.</param>
	static void SetResultError(sqlite3_context ctx, Exception ex)
	{
		raw.sqlite3_result_error(ctx, utf8z.FromString(ex.Message));
		raw.sqlite3_result_error_code(ctx, raw.SQLITE_ERROR);
	}

	/// <summary>
	/// Represents a spatial index defined by a bounding box with minimum and maximum X and Y coordinates.
	/// Provides properties for width, height, and center coordinates of the bounding box.
	/// Used internally for spatial calculations and indexing.
	/// </summary>
	class SpatialIndex
	{
		/// <summary>
		/// Gets or sets the minimum X coordinate of the bounding box.
		/// </summary>
		public double X1 { get; set; }

		/// <summary>
		/// Gets or sets the minimum Y coordinate of the bounding box.
		/// </summary>
		public double Y1 { get; set; }

		/// <summary>
		/// Gets or sets the maximum X coordinate of the bounding box.
		/// </summary>
		public double X2 { get; set; }

		/// <summary>
		/// Gets or sets the maximum Y coordinate of the bounding box.
		/// </summary>
		public double Y2 { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SpatialIndex"/> class with the specified bounding box coordinates.
		/// </summary>
		/// <param name="x1">The minimum X coordinate.</param>
		/// <param name="y1">The minimum Y coordinate.</param>
		/// <param name="x2">The maximum X coordinate.</param>
		/// <param name="y2">The maximum Y coordinate.</param>
		public SpatialIndex(double x1, double y1, double x2, double y2)
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}

		/// <summary>
		/// Gets the width of the bounding box (X2 - X1).
		/// </summary>
		public double Width => X2 - X1;

		/// <summary>
		/// Gets the height of the bounding box (Y2 - Y1).
		/// </summary>
		public double Height => Y2 - Y1;

		/// <summary>
		/// Gets the center X coordinate of the bounding box ((X1 + X2) / 2).
		/// </summary>
		public double CenterX => (X1 + X2) / 2;

		/// <summary>
		/// Gets the center Y coordinate of the bounding box ((Y1 + Y2) / 2).
		/// </summary>
		public double CenterY => (Y1 + Y2) / 2;
	}
}
