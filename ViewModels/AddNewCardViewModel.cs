using FlashLearn.Models;
using FlashLearn.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashLearn.ViewModels
{
    public class AddNewCardViewModel : ViewModelBase
    {
        private string _cardFrontText;
        public string CardFrontText
        {
            get => _cardFrontText;
            set
            {
                _cardFrontText = value;

                if (_cardBackText != null && _cardFrontText != null) IsAddButtonEnabled = true;
                else IsAddButtonEnabled = false;

                OnPropertyChanged(nameof(CardFrontText));
            }
        }

        private string _cardBackText;
        public string CardBackText
        {
            get => _cardBackText;
            set
            {
                _cardBackText = value;

                if (_cardBackText != null && _cardFrontText != null) IsAddButtonEnabled = true;
                else IsAddButtonEnabled = false;

                OnPropertyChanged(nameof(CardBackText));
            }
        }

        private bool _isAddButtonEnabled;
        public bool IsAddButtonEnabled
        {
            get => _isAddButtonEnabled;
            set
            {
                _isAddButtonEnabled = value;
                OnPropertyChanged(nameof(IsAddButtonEnabled));
            }
        }

        public RelayCommand CancelCommand => new RelayCommand(execute => Cancel());
        public RelayCommand AddCardCommand => new RelayCommand(execute => AddCard());

        private EditDeckViewModel _editDeckViewModel;

        public AddNewCardViewModel(EditDeckViewModel editDeckViewModel)
        {
            _editDeckViewModel = editDeckViewModel;

            _isAddButtonEnabled = false;
        }

        private void AddCard()
        {
            CardModel cardModel = new CardModel(CardFrontText,CardBackText);
            _editDeckViewModel.Deck.Cards.Add(cardModel);
            _editDeckViewModel.ActiveCardIndex = _editDeckViewModel.Deck.Cards.Count - 1;

            Shell.Current.Navigation.PopAsync();
        }

        private void Cancel()
        {
            Shell.Current.Navigation.PopAsync();
        }
    }
}
