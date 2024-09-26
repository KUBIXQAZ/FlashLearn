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
            }
        }

        public RelayCommand NextStepCommand => new RelayCommand(execute => NextStep());

        private void NextStep()
        {
            DeckModel deck = new DeckModel(_deckName);

            CreateNewCardPage createNewCardPage = new CreateNewCardPage();
            createNewCardPage.BindingContext = new CreateNewCardViewModel(deck);
            Shell.Current.Navigation.PushAsync(new CreateNewCardPage());
        }
    }
}