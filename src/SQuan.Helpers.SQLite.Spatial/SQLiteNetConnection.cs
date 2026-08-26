// SQLiteNetConnection.cs

using SQLite;

namespace SQuan.Helpers.SQLite.Spatial;

/// <summary>
/// Represents a SQLite connection with spatial extensions enabled.
/// </summary>
public partial class SQLiteNetConnection : SQLiteConnection
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SQLiteNetConnection"/> class with the specified database path and an optional flag to store DateTime values as ticks.
	/// </summary>
	/// <param name="databasePath">The path to the SQLite database file.</param>
	/// <param name="openFlags">The flags to use when opening the SQLite database.</param>
	/// <param name="storeDateTimeAsTicks">A value indicating whether to store DateTime values as ticks.</param>
	public SQLiteNetConnection(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = true)
		: base(databasePath, openFlags, storeDateTimeAsTicks)
	{
	}
}
