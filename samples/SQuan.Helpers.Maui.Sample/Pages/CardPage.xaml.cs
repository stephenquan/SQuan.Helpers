using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Mvvm.Input;
using ObservablePropertyAttribute = SQuan.Helpers.Maui.Mvvm.ObservablePropertyAttribute;
namespace SQuan.Helpers.Maui.Sample;

public partial class CardPage : ContentPage
{
	public ObservableCollection<CardInfo> Cards { get; } =
	[
		new CardInfo
		{
			CardTitle = "Slavko Vlasic",
			CardDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla elit dolor, convallis non interdum.",
		},
		new CardInfo
		{
			CardTitle = "Carolina Pena",
			CardDescription = "Phasellus eu convallis mi. In tempus augue eu dignissim fermentum. Morbi ut lacus vitae eros lacinia.",
		},
		new CardInfo
		{
			CardTitle = "Wade Banks",
			CardDescription = "Aliquam sagittis, odio lacinia fermentum cictum, mi erat sccelerisque erat; quis alliquet arou.",
		},
		new CardInfo
		{
			CardTitle = "Colette Quint",
			CardDescription = "In pellenteqsuq odio ecet augue elemntum lobortis. Sed augue masso, rhoocus eu nisi vitae, egestas.",
		}
	];

	// Implementation of the selected card will be done the source generator.
	[ObservableProperty] public partial CardInfo? SelectedCard { get; set; } = null;

	// This partial function will be called by the source generator.
	partial void OnSelectedCardChanged(CardInfo? oldValue, CardInfo? newValue)
	{
		if (oldValue is not null)
		{
			oldValue.IsSelected = false;
		}
		if (newValue is not null)
		{
			newValue.IsSelected = true;
		}
	}

	public CardPage()
	{
		BindingContext = this;
		InitializeComponent();

		ClearSelectionBtn
			.Bind(
				Button.IsEnabledProperty,
				static (CardPage ctx) => ctx.SelectedCard,
				convert: (CardInfo? selectedCard) => selectedCard is not null);
	}

	[RelayCommand]
	void ClearSelection()
	{
		SelectedCard = null;
	}
}
