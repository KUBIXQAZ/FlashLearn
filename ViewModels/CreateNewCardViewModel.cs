using FlashLearn.Models;
using FlashLearn.MVVM;
using FlashLearn.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlashLearn.ViewModels
{
    public class CreateNewCardViewModel : ViewModelBase
    {
        private DeckModel _deck;

        public RelayCommand CreateNextCardCommand => new RelayCommand(execute => CreateNextCard());
        public RelayCommand FinishCreatingDeckCommand => new RelayCommand(execute => FinishCreatingDeck());

        private string _cardFrontText;
        public string CardFrontText
        {
            get => _cardFrontText;
            set
            {
                _cardFrontText = value;
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
                OnPropertyChanged(nameof(CardBackText));
            }
        }

        public CreateNewCardViewModel(DeckModel deck)
        {
            _deck = deck;
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

            CreateNewCardPage createNewCardPage = new CreateNewCardPage();
            CreateNewCardViewModel createNewCardViewModel = new CreateNewCardViewModel(_deck);
            createNewCardPage.BindingContext = createNewCardViewModel;
            Shell.Current.Navigation.PushAsync(createNewCardPage);
        }
    }
}