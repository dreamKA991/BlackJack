using UnityEngine;
using UnityEngine.UI;
namespace BlackJack.Infrastructure
{
    public class Card : MonoBehaviour, IRestartable
    {
        [SerializeField] private int _cardValue, _cardRank;
        private Sprite _trueSprite;
        public Sprite HiddenSprite;
        private Image _image;
        private bool isHidden = false;
        
        public void Initialize(int cardRank)
        {
            _cardRank = cardRank;
            _cardValue = CalculateCardValue();
            _image = GetComponent<Image>();
            _trueSprite = _image.sprite;
            SignalBus.Instance.OnRestart.AddListener(Restart);
            SignalBus.Instance.OnPlayerActionDid.AddListener(ShowCard);
        }
        private int CalculateCardValue()
        {
            if (_cardRank == 1) return 11;  // Ace
            if (_cardRank >= 10 && _cardRank <= 13) return 10; // 10,11Joker,12Queen,13King
            if (_cardRank >= 2 && _cardRank <= 9) return _cardRank;

            Debug.LogError("Card class: Некорректный ранг карты! Дано: " + _cardRank);
            return 0;
        }

        public int GetCardValue() => _cardValue;
        
        public int GetCardRank() => _cardRank;

        public void HideCard()
        {
            isHidden = true;
            _image.sprite = HiddenSprite;
        }
        
        public void ShowCard(int plug)
        {
            if(!isHidden) return;
            _image.sprite = _trueSprite;
            SignalBus.Instance.DealerTookCard();
        }

        public void Restart()
        {
            Destroy(this.gameObject);
        }
    }
}

