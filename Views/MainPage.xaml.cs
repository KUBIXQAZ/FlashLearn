using FlashLearn.ViewModels;

namespace FlashLearn
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = new MainViewModel();

            InitializeComponent();
        }
    }
}