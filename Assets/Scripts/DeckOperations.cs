using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace BlackJack.Infrastructure
{
    public class DeckOperations : MonoBehaviour, IRestartable// ИМЕЕТСЯ ПОВТОРЕНИЕ КОДА, НЕ SLIWKOM КРИТИЧНО
    {
        private UIDataProvider _uIDataProvider;
        private DecksData _decksData;
        private CardAnimations _cardAnimations;
        private float _animationTime;
        public void Init(UIDataProvider uiDataProvider, DecksData decksData, CardAnimations cardAnimations, float animationTime)
        {
            _uIDataProvider = uiDataProvider;
            _decksData = decksData;
            _cardAnimations = cardAnimations;
            _animationTime = animationTime;
            SignalBus.Instance.OnRestart.AddListener(Restart);
        }
        public Card DrawRandomCard(List<Card> deckOfCards)
        {
            if (deckOfCards.Count <= 0)
            {
                Debug.Log("Колода пуста!");
                return null; 
            }

            int randomIndex = Random.Range(0, deckOfCards.Count); 
            Card drawnCard = deckOfCards[randomIndex]; 
            deckOfCards.RemoveAt(randomIndex); 
            return drawnCard;
        }

        public int CountPointsFromDeck(List<Card> deckOfCards)
        {

            int points = 0;
            int aceCount = 0;

            foreach (Card card in deckOfCards)
            {
                if (card.GetCardRank() == 1)
                    aceCount++;
                else
                    points += card.GetCardValue();
            }
            
            for (int i = 0; i < aceCount; i++)
            {
                points += (points + 11 > 21) ? 1 : 11;
            }

            Debug.Log("DeckOperations class, points: " + points);
            return points;
        }

        
        public IEnumerator GiveCardForPlayer()
        {
            Card drawenCard = DrawRandomCard(_decksData.AllDeck);
            if (_decksData.PlayerDeck == null) _decksData.PlayerDeck = new List<Card>();
            _decksData.PlayerDeck.Add(drawenCard);
            drawenCard.gameObject.SetActive(true);
            _cardAnimations.DoCardMoveToPlayer(drawenCard.transform, _animationTime);
            SignalBus.Instance.PlayerTookCard();
            yield return new WaitForSeconds(_animationTime);
            drawenCard.transform.SetParent(_uIDataProvider.PlayerCardsTransform , false);
        }

        public IEnumerator GiveCardForDealer()
        {
            Card drawenCard = DrawRandomCard(_decksData.AllDeck);
            if (_decksData.DealerDeck == null) _decksData.DealerDeck = new List<Card>();
            _decksData.DealerDeck.Add(drawenCard);
            drawenCard.gameObject.SetActive(true);
            _cardAnimations.DoCardMoveToDealer(drawenCard.transform, _animationTime); 
            SignalBus.Instance.DealerTookCard(); 
            yield return new WaitForSeconds(_animationTime);
            drawenCard.transform.SetParent(_uIDataProvider.DealerCardsTransform, false); 
        }
        
        
        public IEnumerator GiveHidenCardForDealer()
        {
            Card drawenCard = DrawRandomCard(_decksData.AllDeck);
            if (_decksData.DealerDeck == null) _decksData.DealerDeck = new List<Card>();
            _decksData.DealerDeck.Add(drawenCard);
            drawenCard.gameObject.SetActive(true);
            drawenCard.HideCard();
            _cardAnimations.DoCardMoveToDealer(drawenCard.transform, _animationTime); 
            yield return new WaitForSeconds(_animationTime);
            drawenCard.transform.SetParent(_uIDataProvider.DealerCardsTransform, false); 
            int newPoints = drawenCard.GetComponent<Card>().GetCardValue();
        }

        public IEnumerator StartDealCards()
        {
            yield return StartCoroutine(GiveCardForPlayer());
            yield return new WaitForSeconds(_animationTime);
            yield return StartCoroutine(GiveCardForDealer());
            yield return new WaitForSeconds(_animationTime);
            yield return StartCoroutine(GiveCardForPlayer());
            yield return new WaitForSeconds(_animationTime);
            yield return StartCoroutine(GiveHidenCardForDealer());
            yield return new WaitForSeconds(_animationTime);
        }

        public void Restart()
        {
            DestroyChildFromTransform(_uIDataProvider.PlayerCardsTransform);
            DestroyChildFromTransform(_uIDataProvider.DealerCardsTransform);
        }

        private void DestroyChildFromTransform(Transform parentTransform)
        {
            foreach (Transform child in parentTransform)
                Destroy(child.gameObject);
        }

    }
}