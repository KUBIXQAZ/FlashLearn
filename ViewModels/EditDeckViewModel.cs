using FlashLearn.Models;
using FlashLearn.MVVM;
using FlashLearn.Views;
using Newtonsoft.Json;

namespace FlashLearn.ViewModels
{
    public class EditDeckViewModel : ViewModelBase
    {
        public DeckModel Deck { get; set; }

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
                return $"{_activeCardIndex + 1}/{Deck.Cards.Count}";
            }
            set
            {
                _cardNumber = value;
                OnPropertyChanged(nameof(CardNumber));
            }
        }

        private string _deckTitle;
        public string DeckTitle
        {
            get => _deckTitle;
            set
            {
                _deckTitle = value;
                OnPropertyChanged(nameof(DeckTitle));
                UpdateDeckName();
            }
        }

        private string _deleteButtonText = DeleteButtonTextEnum.Delete.ToString();
        public string DeleteButtonText
        {
            get => _deleteButtonText;
            set
            {
                _deleteButtonText = value;
                OnPropertyChanged(nameof(DeleteButtonText));
            }
        }

        private Brush _deleteButtonColor;
        public Brush DeleteButtonColor
        {
            get => _deleteButtonColor;
            set
            {
                _deleteButtonColor = value;
                OnPropertyChanged(nameof(DeleteButtonColor));
            }
        }
        private Brush DefaultDeleteButtonColor;

        enum DeleteButtonTextEnum
        {
            Delete,
            Confirm
        }

        private bool _isNoCardsAlertVisible;
        public bool IsNoCardsAlertVisible
        {
            get => _isNoCardsAlertVisible;
            set
            {
                _isNoCardsAlertVisible = value;
                OnPropertyChanged(nameof(IsNoCardsAlertVisible));
            }
        }

        private bool _displayCards;
        public bool DisplayCards
        {
            get => _displayCards;
            set
            {
                _displayCards = value;
                OnPropertyChanged(nameof(DisplayCards));
            }
        }

        public RelayCommand SwipeLeftCommand => new RelayCommand(execute => SwipeLeft());
        public RelayCommand SwipeRightCommand => new RelayCommand(execute => SwipeRight());
        public RelayCommand SaveDeckCommand => new RelayCommand(execute => SaveDeck());
        public RelayCommand DeleteCardCommand => new RelayCommand(execute => DeleteCard());
        public RelayCommand ReturnToMenuCommand => new RelayCommand(execute => ReturnToMenu());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteDeck());
        public RelayCommand AddCardCommand => new RelayCommand(execute => AddCard());

        public EditDeckViewModel(DeckModel deck)
        {
            Deck = deck;
            DeckTitle = Deck.Name;

            DefaultDeleteButtonColor = new Button().Background;
            DeleteButtonColor = DefaultDeleteButtonColor;

            if(CheckForCards())
            {
                CardChanged();
            }
        }

        private bool CheckForCards()
        {
            if (Deck.Cards.Count == 0)
            {
                IsNoCardsAlertVisible = true;
                DisplayCards = false;
                return false;
            } 
            else
            {
                IsNoCardsAlertVisible = false;
                DisplayCards = true;
                return true;
            }
        }

        private void UpdateCard()
        {
            Deck.Cards[_activeCardIndex].FrontString = _cardFrontText;
            Deck.Cards[_activeCardIndex].BackString = _cardBackText;
        }

        private void UpdateDeckName()
        {
            Deck.Name = _deckTitle;
        }

        private void SwipeLeft()
        {
            if (ActiveCardIndex == Deck.Cards.Count - 1)
            {
                ActiveCardIndex = 0;
                return;
            }
            ActiveCardIndex++;
        }

        private void SwipeRight()
        {
            UpdateCard();

            if (ActiveCardIndex == 0)
            {
                ActiveCardIndex = Deck.Cards.Count - 1;
                return;
            }
            ActiveCardIndex--;
        }

        private void CardChanged()
        {
            if(CheckForCards())
            {
                string front = Deck.Cards[_activeCardIndex].FrontString;
                string back = Deck.Cards[ActiveCardIndex].BackString;

                CardFrontText = front;
                CardBackText = back;
            }
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

                decks[Deck.Id] = Deck;
            }

            string new_json = JsonConvert.SerializeObject(decks);
            File.WriteAllText(path, new_json);
        }

        private void DeleteCard()
        {
            Deck.Cards.RemoveAt(ActiveCardIndex);
            ActiveCardIndex = 0;
        }

        private void ReturnToMenu()
        {
            Shell.Current.Navigation.PopToRootAsync();
        }

        private void DeleteDeck()
        {
            if (DeleteButtonText == DeleteButtonTextEnum.Confirm.ToString())
            {
                string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\FlashLearn";
                string path = $"{folder}\\decks.json";

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    List<DeckModel> decks = JsonConvert.DeserializeObject<List<DeckModel>>(json);

                    decks.RemoveAt(Deck.Id);

                    string save_json = JsonConvert.SerializeObject(decks);
                    File.WriteAllText(path, save_json);

                    Shell.Current.Navigation.PopToRootAsync();
                }
            }

            DeleteButtonText = DeleteButtonTextEnum.Confirm.ToString();
            DeleteButtonColor = Brush.Red;

            IDispatcherTimer timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, e) =>
            {
                DeleteButtonText = DeleteButtonTextEnum.Delete.ToString();
                DeleteButtonColor = DefaultDeleteButtonColor;
                timer.Stop();
            };
            timer.Start();
        }

        private void AddCard()
        {
            AddNewCardPage page = new AddNewCardPage();
            AddNewCardViewModel viewModel = new AddNewCardViewModel(this);
            page.BindingContext = viewModel;
            Shell.Current.Navigation.PushAsync(page);
        }
    }
}