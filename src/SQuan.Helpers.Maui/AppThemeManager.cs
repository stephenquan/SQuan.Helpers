using System.ComponentModel;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides a singleton manager for handling application themes in a .NET MAUI application.
/// </summary>
public partial class AppThemeManager : INotifyPropertyChanged
{
	static AppThemeManager? instance;

	/// <summary>
	/// Gets the singleton instance of the <see cref="AppThemeManager"/> class.
	/// </summary>
	public static AppThemeManager Current
	{
		get
		{
			if (instance is null)
			{
				instance = new AppThemeManager();
			}
			return instance;
		}
	}

	/// <summary>
	/// Gets the theme currently requested by the application.
	/// </summary>
	public AppTheme? RequestedTheme => (Application.Current is Application app) ? app.RequestedTheme : AppTheme.Light;

	/// <summary>
	/// Gets or sets the user-defined application theme.
	/// </summary>
	/// <remarks>Setting this property updates the application's theme and raises property change notifications for
	/// both <see cref="UserAppTheme"/> and <see cref="RequestedTheme"/>.</remarks>
	public AppTheme UserAppTheme
	{
		get => (Application.Current is Application app) ? app.UserAppTheme : AppTheme.Light;
		set
		{
			if (Application.Current is Application app)
			{
				app.UserAppTheme = value;
				OnPropertyChanged(nameof(UserAppTheme));
				OnPropertyChanged(nameof(RequestedTheme));
			}
		}
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event to notify subscribers that a property value has changed.
	/// </summary>
	/// <remarks>This method is typically called within a property setter to notify bound controls or other
	/// listeners of property changes.</remarks>
	/// <param name="propertyName">The name of the property that changed. This value can be <see langword="null"/> or empty to indicate that all
	/// properties have changed.</param>
	public void OnPropertyChanged(string? propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AppThemeManager"/> class.
	/// </summary>
	/// <remarks>This constructor subscribes to the <see cref="Application.RequestedThemeChanged"/> event if the 
	/// current application is available. When the theme changes, the <see cref="RequestedTheme"/> property  is updated by
	/// raising the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.</remarks>
	public AppThemeManager()
	{
		if (Application.Current is Application app)
		{
			app.RequestedThemeChanged += (s, e) => { OnPropertyChanged(nameof(RequestedTheme)); };
		}
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	/// <remarks>This event is typically used to notify subscribers that a property has been updated. It is commonly
	/// implemented in classes that support data binding or observable patterns.</remarks>
	public event PropertyChangedEventHandler? PropertyChanged;
}
