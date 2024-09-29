using FlashLearn.Models;
using FlashLearn.MVVM;
using FlashLearn.Views;

namespace FlashLearn.ViewModels
{
    public class CreateNewDeckViewModel : ViewModelBase
    {
        private string _deckName;
        public string DeckName
        {
            get => _deckName;
            set
            {
                _deckName = value;
                OnPropertyChanged(nameof(DeckName));

                if(_deckName != null)
                {
                    if (_deckName.Length > 0) IsNextButtonEnabled = true;
                    else IsNextButtonEnabled = false;
                }
            }
        }

        private bool _isNextButtonEnabled;
        public bool IsNextButtonEnabled
        {
            get => _isNextButtonEnabled;
            set
            {
                _isNextButtonEnabled = value;
                OnPropertyChanged(nameof(IsNextButtonEnabled));
            }
        }

        public RelayCommand NextStepCommand => new RelayCommand(execute => NextStep());

        public CreateNewDeckViewModel()
        {
            IsNextButtonEnabled = false;
        }

        private void NextStep()
        {
            DeckModel deck = new DeckModel(_deckName);

            CreateNewCardPage createNewCardPage = new CreateNewCardPage();
            createNewCardPage.BindingContext = new CreateNewCardViewModel(deck);
            Shell.Current.Navigation.PushAsync(createNewCardPage);
        }
    }
}