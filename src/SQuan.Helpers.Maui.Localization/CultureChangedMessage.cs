using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Message sent when some culture settings have changed.
/// </summary>
public class CultureChangedMessage : ValueChangedMessage<CultureFlags>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CultureChangedMessage"/> class indicating which cultures have changed.
	/// </summary>
	public CultureChangedMessage(CultureFlags value) : base(value)
	{
	}
}