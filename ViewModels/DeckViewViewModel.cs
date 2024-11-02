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

        private bool _errorVisibility = false;
        public bool ErrorVisibility
        {
            get => _errorVisibility;
            set
            {
                _errorVisibility = value;
                OnPropertyChanged(nameof(ErrorVisibility));
            }
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;

                if (_errorText != null) ErrorVisibility = true;
                else ErrorVisibility = false;

                OnPropertyChanged(nameof(ErrorText));
            }
        }

        public RelayCommand PlayCommand => new RelayCommand(execute => Play());
        public RelayCommand EditCommand => new RelayCommand(execute => EditDeck());
        public RelayCommand ReturnCommand => new RelayCommand(execute => Return());

        private void Return()
        {
            Shell.Current.Navigation.PopAsync();
        }

        public DeckViewViewModel(DeckModel deck)
        {
            _deck = deck;

            DeckName = _deck.Name;

            if(!CheckForCards())
            {
                ErrorText = "You can't play, there is no cards in this deck!";
            }
        }

        private bool CheckForCards()
        {
            if(_deck.Cards.Count == 0)
            {
                return false;
            } 
            else
            {
                return true;
            }
        }

        private void Play()
        {
            if(CheckForCards())
            {
                GamePage gamePage = new GamePage();
                GameViewModel gameViewModel = new GameViewModel(_deck);
                gamePage.BindingContext = gameViewModel;
                Shell.Current.Navigation.PushAsync(gamePage);
            }
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
