using FlashLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashLearn.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private DeckModel _deck;

        public GameViewModel(DeckModel deck)
        {
            _deck = deck;
        }
    }
}