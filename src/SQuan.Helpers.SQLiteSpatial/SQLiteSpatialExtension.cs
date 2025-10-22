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
		CreateGeometryFunction<Geometry?>(db.Handle, "ST_Boundary", (g) => g?.Boundary);
		CreateGeometryDoubleFunction<Geometry?>(db.Handle, "ST_Buffer", (g, d) => g?.Buffer(d));
		CreateGeometryFunction<Geometry?>(db.Handle, "ST_Centroid", (g) => g?.Centroid);
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_Contains", (g, g2) => g?.Contains(g2) ?? false);
		CreateGeometryFunction<Geometry?>(db.Handle, "ST_ConvexHull", (g) => g?.ConvexHull());
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_CoveredBy", (g, g2) => g?.CoveredBy(g2) ?? false);
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_Covers", (g, g2) => g?.Covers(g2) ?? false);
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_Crosses", (g, g2) => g?.Crosses(g2) ?? false);
		CreateGeometryGeometryFunction<Geometry?>(db.Handle, "ST_Difference", (g, g2) => g?.Difference(g2));
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_Disjoint", (g, g2) => g?.Disjoint(g2) ?? false);
		CreateGeometryGeometryFunction<double?>(db.Handle, "ST_Distance", (g, g2) => g?.Distance(g2));
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_Equals", (g, g2) => g?.Equals(g2) ?? false);
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_EqualsExact", (g, g2) => g?.EqualsExact(g2) ?? false);
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_EqualsNormalized", (g, g2) => g?.EqualsNormalized(g2) ?? false);
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_EqualsTopologically", (g, g2) => g?.EqualsTopologically(g2) ?? false);
		CreateGeometryFunction<Geometry?>(db.Handle, "ST_Envelope", (g) => g?.Envelope);
		CreateGeometryFunction<string?>(db.Handle, "ST_GeometryType", (g) => g?.GeometryType);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_Height", (e) => e?.Height);
		CreateGeometryFunction<Geometry?>(db.Handle, "ST_IsRectangle", (g) => g?.InteriorPoint);
		CreateGeometryGeometryFunction<Geometry?>(db.Handle, "ST_Intersection", (g, g2) => g?.Intersection(g2));
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_Intersects", (g, g2) => g?.Intersects(g2) ?? false);
		CreateGeometryFunction<bool>(db.Handle, "ST_IsEmpty", (g) => g?.IsEmpty ?? false);
		SQLitePCL.raw.sqlite3_create_function(db.Handle, "ST_IsGeometry", 1, SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC, ST_IsGeometry);
		CreateGeometryFunction<bool>(db.Handle, "ST_IsRectangle", (g) => g?.IsRectangle ?? false);
		CreateGeometryFunction<bool>(db.Handle, "ST_IsSimple", (g) => g?.IsSimple ?? false);
		CreateGeometryFunction<bool>(db.Handle, "ST_IsValid", (g) => g?.IsValid ?? false);
		CreateGeometryFunction<double?>(db.Handle, "ST_Length", (g) => g?.Length);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_MaxX", (e) => e?.MaxX);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_MaxY", (e) => e?.MaxY);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_MinX", (e) => e?.MinX);
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_MinY", (e) => e?.MinY);
		CreateDoubleDoubleFunction<Geometry>(db.Handle, "ST_Point", (x, y) => new Point(x, y));
		CreateGeometryFunction<Geometry?>(db.Handle, "ST_Reverse", (g) => g?.Reverse());
		CreateGeometryIntegerFunction<Geometry?>(db.Handle, "ST_SetSRID", (g, i) => { g?.SRID = i; return g; });
		CreateGeometryFunction<int?>(db.Handle, "ST_SRID", (g) => g?.SRID);
		CreateGeometryGeometryFunction<Geometry?>(db.Handle, "ST_SymmetricDifference", (g, g2) => g?.SymmetricDifference(g2));
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_Touches", (g, g2) => g?.Touches(g2) ?? false);
		CreateGeometryGeometryFunction<Geometry?>(db.Handle, "ST_Union", (g, g2) => g?.Union(g2));
		CreateSpatialIndexFunction<double?>(db.Handle, "ST_Width", (e) => e?.Width);
		CreateGeometryGeometryFunction<bool>(db.Handle, "ST_Within", (g, g2) => g?.Within(g2) ?? false);
		CreateGeometryFunction<double?>(db.Handle, "ST_X", (g) => g?.Centroid.X);
		CreateGeometryFunction<double?>(db.Handle, "ST_Y", (g) => g?.Centroid.Y);
	}

	/// <summary>
	/// Determines if the provided argument is a valid geometry object.
	/// </summary>
	/// <remarks>This method sets the result to <see langword="1"/> if the argument is a valid geometry object;
	/// otherwise, it sets the result to <see langword="0"/>.</remarks>
	/// <param name="ctx">The SQLite context in which the function is executed.</param>
	/// <param name="user_data">User data associated with the function, not used in this implementation.</param>
	/// <param name="args">An array of SQLite values, where the first element is expected to be a geometry object.</param>
	static void ST_IsGeometry(sqlite3_context ctx, object user_data, sqlite3_value[] args)
	{
		try
		{
			var wkt = raw.sqlite3_value_text(args[0]).utf8_to_string();
			if (!string.IsNullOrEmpty(wkt))
			{
				var geometry = ToGeometry(wkt);
				if (geometry is not null)
				{
					SetResult(ctx, 1);
					return;
				}
			}
		}
		catch (Exception)
		{
		}
		SetResult(ctx, 0);
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
	static void CreateGeometryFunction<T>(sqlite3 handle, string name, Func<Geometry?, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
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
	static void CreateGeometryDoubleFunction<T>(sqlite3 handle, string name, Func<Geometry?, double, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
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
	static void CreateGeometryGeometryFunction<T>(sqlite3 handle, string name, Func<Geometry?, Geometry?, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
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
	static void CreateGeometryIntegerFunction<T>(sqlite3 handle, string name, Func<Geometry?, int, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
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
	static void CreateSpatialIndexFunction<T>(sqlite3 handle, string name, Func<Envelope?, T> func, int flags = SQLitePCL.raw.SQLITE_UTF8 | SQLitePCL.raw.SQLITE_DETERMINISTIC)
	{
		SQLitePCL.raw.sqlite3_create_function(handle, name, 1, flags, null, (sqlite3_context ctx, object user_data, sqlite3_value[] args) =>
		{
			try
			{
				var geometry = ToGeometry(raw.sqlite3_value_text(args[0]).utf8_to_string());
				if (geometry?.EnvelopeInternal is Envelope e && !e.IsNull)
				{
					SetResult(ctx, func(e));
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
			case float f:
				raw.sqlite3_result_double(ctx, f);
				break;
			case int i:
				raw.sqlite3_result_int(ctx, i);
				break;
			case long l:
				raw.sqlite3_result_int64(ctx, l);
				break;
			case bool b:
				raw.sqlite3_result_int(ctx, b ? 1 : 0);
				break;
			case Geometry g:
				SetResult(ctx, g.AsText());
				break;
			default:
				if (result?.ToString() is string str && !string.IsNullOrEmpty(str))
				{
					raw.sqlite3_result_text(ctx, utf8z.FromString(str));
				}
				else
				{
					raw.sqlite3_result_null(ctx);
				}
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
	}
}
