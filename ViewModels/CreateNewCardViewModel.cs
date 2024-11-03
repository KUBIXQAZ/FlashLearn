using FlashLearn.Models;
using FlashLearn.MVVM;
using Newtonsoft.Json;

namespace FlashLearn.ViewModels
{
    public class CreateNewCardViewModel : ViewModelBase
    {
        private DeckModel _deck;

        public RelayCommand CreateNextCardCommand => new RelayCommand(execute => CreateNextCard());
        public RelayCommand FinishCreatingDeckCommand => new RelayCommand(execute => FinishCreatingDeck());
        public RelayCommand CancelCommand => new RelayCommand(execute => Cancel());

        private string _cardFrontText;
        public string CardFrontText
        {
            get => _cardFrontText;
            set
            {
                _cardFrontText = value;
                OnPropertyChanged(nameof(CardFrontText));

                if(_cardFrontText != null && _cardBackText != null)
                {
                    if(_cardFrontText.Length > 0 && _cardBackText.Length > 0)
                    {
                        IsProgressionButtonEnabled = true;
                    } else
                    {
                        IsProgressionButtonEnabled = false;
                    }
                }
            }
        }

        private string _cardBackText;
        public string CardBackText
        {
            get => _cardBackText;
            set
            {
                _cardBackText = value;
                OnPropertyChanged(nameof(CardBackText));

                if (_cardBackText != null && _cardFrontText != null)
                {
                    if (_cardBackText.Length > 0 && _cardFrontText.Length > 0)
                    {
                        IsProgressionButtonEnabled = true;
                    }
                    else
                    {
                        IsProgressionButtonEnabled = false;
                    }
                }
            }
        }

        private bool _isProgressionButtonEnabled;
        public bool IsProgressionButtonEnabled
        {
            get => _isProgressionButtonEnabled;
            set
            {
                _isProgressionButtonEnabled = value;
                OnPropertyChanged(nameof(IsProgressionButtonEnabled));
            }
        }

        public CreateNewCardViewModel(DeckModel deck)
        {
            _deck = deck;

            IsProgressionButtonEnabled = false;
        }

        private void FinishCreatingDeck()
        {
            CardModel card = new CardModel(CardFrontText, CardBackText);
            _deck.Cards.Add(card);

            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\FlashLearn";
            string path = $"{folder}\\decks.json";

            if(!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            List<DeckModel> decks = new List<DeckModel>();

            if(File.Exists(path))
            {
                string json = File.ReadAllText(path);
                decks = JsonConvert.DeserializeObject<List<DeckModel>>(json);
            }
            
            decks.Add(_deck);
            
            string new_json = JsonConvert.SerializeObject(decks);
            File.WriteAllText(path, new_json);

            Shell.Current.Navigation.PopToRootAsync();
        }

        private void CreateNextCard()
        {
            CardModel card = new CardModel(CardFrontText,CardBackText);
            _deck.Cards.Add(card);

            CardFrontText = string.Empty;
            CardBackText = string.Empty;
        }

        private void Cancel()
        {
            Shell.Current.Navigation.PopToRootAsync();
        }
    }
}