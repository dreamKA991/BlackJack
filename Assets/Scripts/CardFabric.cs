using UnityEngine;
using UnityEngine.UI;

namespace BlackJack.Infrastructure
{
    public class CardFabric
    {
        private CardConfig _config;

        public CardFabric(ScriptableObject config)
        {
            _config = config as CardConfig;
            if (_config == null)
            {
                Debug.LogError("Передан неправильный ScriptableObject! Ожидался CardConfig.");
            }
        }

        public GameObject CreateNewCard(CardSuit suit, int cardRank)
        {
            GameObject card = GameObject.Instantiate(Resources.Load("CardPrefab", typeof(GameObject))) as GameObject;
            card.GetComponent<Image>().sprite = GetCardImage(suit, cardRank);
            card.GetComponent<Card>().HiddenSprite = _config.BackSide;
            return card;
        }

        private Sprite GetCardImage(CardSuit suit, int cardRank)
        {
            if (_config == null)
            {
                Debug.LogError("CardConfig не задан!");
                return null;
            }

            switch (cardRank)
            {
                case 1: return GetSuitSprite(suit, _config.Ace_Trefa, _config.Ace_Bybna, _config.Ace_Chervi, _config.Ace_Pika);
                case 2: return GetSuitSprite(suit, _config.Two_Trefa, _config.Two_Bybna, _config.Two_Chervi, _config.Two_Pika);
                case 3: return GetSuitSprite(suit, _config.Three_Trefa, _config.Three_Bybna, _config.Three_Chervi, _config.Three_Pika);
                case 4: return GetSuitSprite(suit, _config.Four_Trefa, _config.Four_Bybna, _config.Four_Chervi, _config.Four_Pika);
                case 5: return GetSuitSprite(suit, _config.Five_Trefa, _config.Five_Bybna, _config.Five_Chervi, _config.Five_Pika);
                case 6: return GetSuitSprite(suit, _config.Six_Trefa, _config.Six_Bybna, _config.Six_Chervi, _config.Six_Pika);
                case 7: return GetSuitSprite(suit, _config.Seven_Trefa, _config.Seven_Bybna, _config.Seven_Chervi, _config.Seven_Pika);
                case 8: return GetSuitSprite(suit, _config.Eight_Trefa, _config.Eight_Bybna, _config.Eight_Chervi, _config.Eight_Pika);
                case 9: return GetSuitSprite(suit, _config.Nine_Trefa, _config.Nine_Bybna, _config.Nine_Chervi, _config.Nine_Pika);
                case 10: return GetSuitSprite(suit, _config.Ten_Trefa, _config.Ten_Bybna, _config.Ten_Chervi, _config.Ten_Pika);
                case 11: return GetSuitSprite(suit, _config.Joker_Trefa, _config.Joker_Bybna, _config.Joker_Chervi, _config.Joker_Pika);
                case 12: return GetSuitSprite(suit, _config.Queen_Trefa, _config.Queen_Bybna, _config.Queen_Chervi, _config.Queen_Pika);
                case 13: return GetSuitSprite(suit, _config.King_Trefa, _config.King_Bybna, _config.King_Chervi, _config.King_Pika);
                default:
                    Debug.LogError("Некорректный ранг карты! Дано:" + cardRank);
                    return null;
            }
        }

        private Sprite GetSuitSprite(CardSuit suit, Sprite trefa, Sprite bybna, Sprite chirva, Sprite pika)
        {
            switch (suit)
            {
                case CardSuit.Trefa: return trefa;
                case CardSuit.Bybna: return bybna;
                case CardSuit.Chirva: return chirva;
                case CardSuit.Pika: return pika;
                default:
                    Debug.LogError("Некорректная масть!");
                    return null;
            }
        }
    }
}
