namespace FlashLearn.Models
{
    public class CardModel
    {
        public string FrontString { get; set; }
        public string BackString { get; set; }

        public CardModel(string frontString, string backString)
        {
            FrontString = frontString;
            BackString = backString;
        }
    }
}