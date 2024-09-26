using FlashLearn.ViewModels;

namespace FlashLearn.Views;

public partial class CreateNewDeckPage : ContentPage
{
	public CreateNewDeckPage()
	{
		BindingContext = new CreateNewDeckViewModel();

		InitializeComponent();
	}
}