// LocalizationStringResourceInfo.cs

using Microsoft.Extensions.Localization;

namespace SQuan.Helpers.Maui.Localization;

class LocalizationStringResourceInfo
{
	public IStringLocalizer? Localizer { get; internal set; } = null;
	public bool IsInitialized { get; internal set; } = false;
}

