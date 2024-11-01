using FlashLearn.Models;
using FlashLearn.MVVM;
using FlashLearn.Views;
using Newtonsoft.Json;

namespace FlashLearn.ViewModels
{
    public class DeckViewViewModel : ViewModelBase
    {
        private DeckModel _deck;

        private string _deckName;
        public string DeckName
        {
            get => _deckName;
            set
            {
                _deckName = value;
                OnPropertyChanged(nameof(DeckName));
            }
        }

        public RelayCommand PlayCommand => new RelayCommand(execute => Play());
        public RelayCommand EditCommand => new RelayCommand(execute => EditDeck());

        public DeckViewViewModel(DeckModel deck)
        {
            _deck = deck;

            DeckName = _deck.Name;
        }

        private void Play()
        {
            GamePage gamePage = new GamePage();
            GameViewModel gameViewModel = new GameViewModel(_deck);
            gamePage.BindingContext = gameViewModel;
            Shell.Current.Navigation.PushAsync(gamePage);
        }

        private void EditDeck()
        {
            EditDeckPage page = new EditDeckPage();
            EditDeckViewModel viewModel = new EditDeckViewModel(_deck);
            page.BindingContext = viewModel;
            Shell.Current.Navigation.PushAsync(page);
        }
    }
}
