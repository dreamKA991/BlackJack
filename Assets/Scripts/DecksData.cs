using System.Collections.Generic;

namespace BlackJack.Infrastructure
{
    public class DecksData : IRestartable
    {
        public List<Card> AllDeck { get; set; }
        public List<Card> PlayerDeck { get; set; }
        public List<Card> DealerDeck { get; set; }

        public void Restart()
        {
            AllDeck = new List<Card>();
            PlayerDeck = new List<Card>();
            DealerDeck = new List<Card>();
        }

    }
}