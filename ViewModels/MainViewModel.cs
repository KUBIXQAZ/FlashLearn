using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashLearn.Models;
using FlashLearn.MVVM;
using FlashLearn.ViewModels;
using FlashLearn.Views;

namespace FlashLearn.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<DeckModel> _decks = new ObservableCollection<DeckModel>();
        public ObservableCollection<DeckModel> Decks
        {
            get
            {
                return _decks;
            }
            set
            {
                _decks = value;
                OnPropertyChanged(nameof(Decks));
            }
        }

        public RelayCommand CreateNewDeckCommand => new RelayCommand(execute => CreateNewDeck());

        public MainViewModel()
        {
            Decks.Add(new DeckModel("niemiecki v1"));
        }

        private void CreateNewDeck()
        {
            Shell.Current.Navigation.PushAsync(new CreateNewDeckPage());
        }
    }
}