using FlashLearn.Models;
using FlashLearn.MVVM;
using Newtonsoft.Json;

namespace FlashLearn.ViewModels
{
    public class EditDeckViewModel : ViewModelBase
    {
        private DeckModel _deck;

        private int _activeCardIndex;
        public int ActiveCardIndex
        {
            get
            {
                return _activeCardIndex;
            }
            set
            {
                _activeCardIndex = value;
                OnPropertyChanged(nameof(ActiveCardIndex));
                OnPropertyChanged(nameof(CardNumber));
                CardChanged();
            }
        }

        private string _cardFrontText;
        public string CardFrontText
        {
            get
            {
                return _cardFrontText;
            }
            set
            {
                _cardFrontText = value;  
                OnPropertyChanged(nameof(CardFrontText));
                UpdateCard();
            }
        }

        private string _cardBackText;
        public string CardBackText
        {
            get
            {
                return _cardBackText;
            }
            set
            {
                _cardBackText = value;
                OnPropertyChanged(nameof(CardBackText));
                UpdateCard();
            }
        }

        private string _cardNumber;
        public string CardNumber
        {
            get
            {
                return $"{_activeCardIndex + 1}/{_deck.Cards.Count}";
            }
            set
            {
                _cardNumber = value;
                OnPropertyChanged(nameof(CardNumber));
            }
        }

        public RelayCommand SwipeLeftCommand => new RelayCommand(execute => SwipeLeft());
        public RelayCommand SwipeRightCommand => new RelayCommand(execute => SwipeRight());
        public RelayCommand SaveDeckCommand => new RelayCommand(execute => SaveDeck());
        public RelayCommand DeleteCardCommand => new RelayCommand(execute => DeleteCard());
        public RelayCommand ReturnToMenuCommand => new RelayCommand(execute => ReturnToMenu());

        public EditDeckViewModel(DeckModel deck)
        {
            _deck = deck;

            CardChanged();
        }

        private void UpdateCard()
        {
            _deck.Cards[_activeCardIndex].FrontString = _cardFrontText;
            _deck.Cards[_activeCardIndex].BackString = _cardBackText;
        }

        private void SwipeLeft()
        {
            if (ActiveCardIndex == _deck.Cards.Count - 1) return;
            ActiveCardIndex++;
        }

        private void SwipeRight()
        {
            UpdateCard();

            if (ActiveCardIndex == 0) return;
            ActiveCardIndex--;
        }

        private void CardChanged()
        {
            string front = _deck.Cards[_activeCardIndex].FrontString;
            string back = _deck.Cards[ActiveCardIndex].BackString;

            CardFrontText = front;
            CardBackText = back;
        }

        private void SaveDeck()
        {
            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\FlashLearn";
            string path = $"{folder}\\decks.json";

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            List<DeckModel> decks = new List<DeckModel>();

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                decks = JsonConvert.DeserializeObject<List<DeckModel>>(json);

                decks[_deck.Id] = _deck;
            }

            string new_json = JsonConvert.SerializeObject(decks);
            File.WriteAllText(path, new_json);
        }

        private void DeleteCard()
        {
            _deck.Cards.RemoveAt(ActiveCardIndex);
            ActiveCardIndex = 0;
        }

        private void ReturnToMenu()
        {
            Shell.Current.Navigation.PopAsync();
        }
    }
}