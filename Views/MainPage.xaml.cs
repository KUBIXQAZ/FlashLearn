using FlashLearn.ViewModels;

namespace FlashLearn.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as ViewModelBase)?.OnAppearing();
        }
    }
}