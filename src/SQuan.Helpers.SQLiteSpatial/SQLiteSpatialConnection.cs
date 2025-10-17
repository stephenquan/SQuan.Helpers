using SQLite;

namespace SQuan.Helpers.SQLiteSpatial;

/// <summary>
/// Adds spatial capabilities to SQLiteConnection.
/// </summary>
public partial class SQLiteSpatialConnection : SQLiteConnection
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SQLiteSpatialConnection"/> class.
	/// </summary>
	/// <param name="databasePath"></param>
	/// <param name="openFlags"></param>
	public SQLiteSpatialConnection(string databasePath, SQLiteOpenFlags openFlags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex)
		: base(databasePath, openFlags)
	{
		SQLiteSpatialExtensions.EnableSpatialExtensions(this);
	}
}
