using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Infrastructure
{
    public class ObjectPoolGenerator
    {
        CardFabric _cardFabric;
        private Transform _canvasTransform;
        public ObjectPoolGenerator(CardFabric cardFabric, UIDataProvider uiDataProvider)
        {
            _cardFabric = cardFabric;
            _canvasTransform = uiDataProvider.CanvasTransform;
        }
        
        public List<Card> GenerateCardsPool()
        {
            List<Card> Pool = new List<Card>();
            foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
            {
                for (int i = 1; i <= 13; i++)
                {
                    GameObject newCardObject = _cardFabric.CreateNewCard(suit, i);
                    Card _card = newCardObject.GetComponent<Card>();
                    _card.Initialize(i);
                    newCardObject.transform.SetParent(_canvasTransform, false);
                    newCardObject.SetActive(false);
                    Pool.Add(_card);
                }
            }
            return Pool;
        }
    }
}