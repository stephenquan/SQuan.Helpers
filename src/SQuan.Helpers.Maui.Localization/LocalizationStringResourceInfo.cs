using Microsoft.Extensions.Localization;

namespace SQuan.Helpers.Maui.Localization;

class LocalizationStringResourceInfo
{
	public IStringLocalizer? Stringlocalizer { get; internal set; } = null;
	public bool IsInitialized { get; internal set; } = false;
}

