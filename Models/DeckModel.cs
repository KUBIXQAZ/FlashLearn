namespace FlashLearn.Models
{
    public class DeckModel
    {
        public string Name { get; set; }
        public List<CardModel> Cards { get; set; }

        public DeckModel(string name, List<CardModel> cards = null)
        {
            Name = name;
            Cards = cards;
        }
    }
}