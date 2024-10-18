using System.Collections.ObjectModel;
using FlashLearn.Models;
using FlashLearn.MVVM;
using FlashLearn.Views;
using Newtonsoft.Json;

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

        internal override void OnAppearing()
        {
            base.OnAppearing();

            LoadDecks();
        }

        private void LoadDecks()
        {
            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\FlashLearn";
            string path = $"{folder}\\decks.json";

            if(!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            else
            {
                if(File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    List<DeckModel> decks = JsonConvert.DeserializeObject<List<DeckModel>>(json);
                    Decks = new ObservableCollection<DeckModel>(decks);

                    int i = 0;
                    foreach(DeckModel deck in Decks)
                    {
                        deck.Id = i;
                        i++;
                    }
                }
            }
        }

        private void CreateNewDeck()
        {
            Shell.Current.Navigation.PushAsync(new CreateNewDeckPage());
        }
    }
}