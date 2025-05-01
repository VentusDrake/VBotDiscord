namespace VBotDiscord.other {
    public class CardSystem {
        private int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        private string[] cardSuits = { "Clubs", "Spades", "Diamonds", "Hearts" };

        public int SelectedNumber { get; private set; }
        public string SelectedCard { get; private set; }

        public CardSystem() {
            DrawCard();   
        }

        private void DrawCard() {
            var random = new Random();
            int numberIndex = random.Next(0, cardNumbers.Length);
            int suitIndex = random.Next(0, cardSuits.Length);

            SelectedNumber = cardNumbers[numberIndex];
            SelectedCard = $"{SelectedNumber} of {cardSuits[suitIndex]}";
        }
    }
}
