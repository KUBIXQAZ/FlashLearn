using FlashLearn.MVVM;
using FlashLearn.Views;
using FlashLearn.ViewModels;

namespace FlashLearn.Models
{
    public class DeckModel
    {
        public string Name { get; set; }
        public List<CardModel> Cards { get; set; }

        public DeckModel(string name, List<CardModel> cards = null)
        {
            Name = name;
            Cards = cards == null ? new List<CardModel>() : cards;
        }

        public RelayCommand PlayCommand => new RelayCommand(execute => Play());

        private void Play()
        {
            GamePage gamePage = new GamePage();
            GameViewModel gameViewModel = new GameViewModel(this);
            gamePage.BindingContext = gameViewModel;
            Shell.Current.Navigation.PushAsync(gamePage);
        }
    }
}