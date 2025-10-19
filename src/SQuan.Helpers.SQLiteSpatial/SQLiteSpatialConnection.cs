// SQLiteSpatialConnection.cs

using SQLite;

namespace SQuan.Helpers.SQLiteSpatial;

/// <summary>
/// Represents a specialized <see cref="SQLiteConnection"/> that enables spatial capabilities for SQLite databases.
/// This class automatically enables spatial extensions upon initialization, allowing spatial queries and operations.
/// </summary>
public partial class SQLiteSpatialConnection : SQLiteConnection
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SQLiteSpatialConnection"/> class.
	/// Enables spatial extensions for the underlying SQLite database connection.
	/// </summary>
	/// <param name="databasePath">The file path to the SQLite database.</param>
	/// <param name="openFlags">
	/// The flags controlling how the database is opened. Defaults to <see cref="SQLiteOpenFlags.ReadWrite"/>, 
	/// <see cref="SQLiteOpenFlags.Create"/>, and <see cref="SQLiteOpenFlags.FullMutex"/>.
	/// </param>
	public SQLiteSpatialConnection(string databasePath, SQLiteOpenFlags openFlags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex)
		: base(databasePath, openFlags)
	{
		// Enable spatial extensions for this connection.
		SQLiteSpatialExtensions.EnableSpatialExtensions(this);
	}
}
