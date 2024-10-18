using FlashLearn.MVVM;
using FlashLearn.Views;
using FlashLearn.ViewModels;

namespace FlashLearn.Models
{
    public class DeckModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CardModel> Cards { get; set; }

        public DeckModel(string name, List<CardModel> cards = null)
        {
            Id = -1;
            Name = name;
            Cards = cards == null ? new List<CardModel>() : cards;
        }

        public RelayCommand ViewDeckCommand => new RelayCommand(execute => ViewDeck());

        private void ViewDeck()
        {
            DeckViewPage deckViewPage = new DeckViewPage();
            DeckViewViewModel viewModel = new DeckViewViewModel(this);
            deckViewPage.BindingContext = viewModel;
            Shell.Current.Navigation.PushAsync(deckViewPage);
        }
    }
}