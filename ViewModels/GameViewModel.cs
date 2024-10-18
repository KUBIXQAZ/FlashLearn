using FlashLearn.Models;
using FlashLearn.Utilities;
using FlashLearn.MVVM;

namespace FlashLearn.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private DeckModel _deck;

        private bool _isGoToNextButtonEnabled;
        public bool IsGoToNextButtonEnabled
        {
            get => _isGoToNextButtonEnabled;
            set
            {
                _isGoToNextButtonEnabled = value;
                OnPropertyChanged(nameof(IsGoToNextButtonEnabled));
            }
        }

        private CardModel _card;
        public CardModel Card
        {
            get => _card;
            set
            {
                _card = value;
                OnPropertyChanged(nameof(Card));

                if(_deck != null)
                {
                    if (_deck.Cards.Count == 1)
                    {
                        ProgressionButtonText = ProgressionStatus.Finish.ToString();
                    }
                }
            }
        } 

        private bool _isBackSideVisible;
        public bool IsBackSideVisible
        {
            get => _isBackSideVisible;
            set
            {
                _isBackSideVisible = value;
                OnPropertyChanged(nameof(IsBackSideVisible));
            }
        }

        private string _progressionButtonText = ProgressionStatus.Next.ToString();
        public string ProgressionButtonText
        {
            get => _progressionButtonText;
            set
            {
                _progressionButtonText = value;
                OnPropertyChanged(nameof(ProgressionButtonText));
            }
        }

        enum ProgressionStatus
        {
            Next,
            Finish
        }

        public RelayCommand FlipCardCommand => new RelayCommand(execute => FlipCard());
        public RelayCommand ProgressionCommand => new RelayCommand(execute => Progression());

        public GameViewModel(DeckModel deck)
        {
            _deck = deck;
            _deck.Cards = ShuffleUtility.Shuffle(_deck.Cards);

            LoadGame();
        }

        private void LoadGame()
        {
            IsGoToNextButtonEnabled = false;
            IsBackSideVisible = false;

            LoadCard();
        }

        private void LoadCard()
        {
            Card = _deck.Cards[0];
            _deck.Cards.Remove(Card);
        }

        private void FlipCard()
        {
            IsBackSideVisible = true;

            IsGoToNextButtonEnabled = true;
        }

        private void Progression()
        {
            if(_deck.Cards.Count > 0)
            {
                LoadGame();
                return;
            }

            Shell.Current.Navigation.PopToRootAsync();
        }
    }
}