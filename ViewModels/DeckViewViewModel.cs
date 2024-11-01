using FlashLearn.Models;
using FlashLearn.MVVM;
using FlashLearn.Views;
using Newtonsoft.Json;

namespace FlashLearn.ViewModels
{
    public class DeckViewViewModel : ViewModelBase
    {
        private DeckModel _deck;

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

        public RelayCommand PlayCommand => new RelayCommand(execute => Play());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteDeck());
        public RelayCommand EditCommand => new RelayCommand(execute => EditDeck());

        enum DeleteButtonTextEnum
        {
            Delete,
            Confirm
        }

        public DeckViewViewModel(DeckModel deck)
        {
            _deck = deck;

            DeckName = _deck.Name;

            DefaultDeleteButtonColor = new Button().Background;
            DeleteButtonColor = DefaultDeleteButtonColor;
        }

        private void Play()
        {
            GamePage gamePage = new GamePage();
            GameViewModel gameViewModel = new GameViewModel(_deck);
            gamePage.BindingContext = gameViewModel;
            Shell.Current.Navigation.PushAsync(gamePage);
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

                    decks.RemoveAt(_deck.Id);

                    string save_json = JsonConvert.SerializeObject(decks);
                    File.WriteAllText(path, save_json);

                    Shell.Current.Navigation.PopToRootAsync();
                }
            }

            DeleteButtonText = DeleteButtonTextEnum.Confirm.ToString();
            DeleteButtonColor = Brush.Red;

            IDispatcherTimer timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s,e) =>
            {
                DeleteButtonText = DeleteButtonTextEnum.Delete.ToString();
                DeleteButtonColor = DefaultDeleteButtonColor;
                timer.Stop();
            };
            timer.Start();
        }

        private void EditDeck()
        {
            EditDeckPage page = new EditDeckPage();
            EditDeckViewModel viewModel = new EditDeckViewModel(_deck);
            page.BindingContext = viewModel;
            Shell.Current.Navigation.PushAsync(page);
        }
    }
}
