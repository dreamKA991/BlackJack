using System.Collections;
using UnityEngine;
using BlackJack.Infrastructure;

namespace BlackJack.Gameplay
{
    [RequireComponent(typeof(PopupShower))]
    public class GamePlayLogic : MonoBehaviour, IRestartable
    {
        private const float CARDANIMATIONTIME = 0.15f;
        private int _dealerPoints, _playerPoints;
        private CardFabric _cardFabric;
        private ObjectPoolGenerator _objectPoolGenerator;
        private DeckOperations _deckOperations;
        private CardAnimations _cardAnimations;
        private UIDataProvider _uiDataProvider;
        private DecksData _decksData;
        private SoundPlayer _soundPlayer;
        private PlayerActions _playerActions;
        private PopupShower _popupShower;
        private bool isPlayerDidChoice = false;
        public void Init(ScriptableObject config, UIDataProvider uiDataProvider, SoundPlayer soundPlayer)
        {
            _cardFabric = new CardFabric(config);
            _uiDataProvider = uiDataProvider;
            _decksData = new DecksData();
            _objectPoolGenerator = new ObjectPoolGenerator(_cardFabric, _uiDataProvider);
            _cardAnimations = new CardAnimations(_uiDataProvider);
            _deckOperations = GameObject.FindObjectOfType<DeckOperations>().GetComponent<DeckOperations>();
            _deckOperations.Init(_uiDataProvider, _decksData, _cardAnimations, CARDANIMATIONTIME);
            _soundPlayer = soundPlayer;
            _playerActions = GameObject.FindObjectOfType<PlayerActions>().GetComponent<PlayerActions>();
            _playerActions.Init(_uiDataProvider, _deckOperations, CARDANIMATIONTIME);
            _popupShower = GetComponent<PopupShower>();
            _popupShower.Init(_uiDataProvider);
            SignalBus.Instance.OnPlayerTakedCard.AddListener(GivePlayerPoints);
            SignalBus.Instance.OnDealerTakedCard.AddListener(GiveDealerPoints);
            SignalBus.Instance.OnRestart.AddListener(Restart);
        }
        
        public IEnumerator StartGame()
        {
            _dealerPoints = 0;
            _playerPoints = 0;
            _decksData.AllDeck = _objectPoolGenerator.GenerateCardsPool();
            yield return StartCoroutine(GiveFirstCards());
            yield return StartCoroutine(WaitForPlayerAction());
            SignalBus.Instance.OnPlayerActionDid.AddListener(DoActions);
        }

        private void DoActions(int choice)
        {
            if (choice == 1) // STOP BUTTON
            {
                Debug.Log("Player choosed:" + choice);
                if (isPlayerDidChoice) return;
                isPlayerDidChoice = true;
                StartCoroutine(MainLogic());
            }
            else if (choice == 2) // DOUBLE BUTTON
            {
                Debug.Log("Player choosed:" + choice);
                if (isPlayerDidChoice) return;
                isPlayerDidChoice = true;
                _deckOperations.StartCoroutine(_deckOperations.GiveCardForPlayer());
                StartCoroutine(MainLogic());
            }
        }

        private IEnumerator MainLogic()
        {
            Debug.Log("Main Logic, _playerpoints:" + _playerPoints + ", dealerpoints:" + _dealerPoints);
            yield return new WaitForSeconds(CARDANIMATIONTIME);
            _dealerPoints = _deckOperations.CountPointsFromDeck(_decksData.DealerDeck);
            while (_dealerPoints < 17)
            {
                yield return _deckOperations.StartCoroutine(_deckOperations.GiveCardForDealer());
                yield return StartCoroutine(WaitAndCountPoints());
                CheckResult(false, true);
            }

            if (_playerPoints > _dealerPoints )
            {
                yield return _deckOperations.StartCoroutine(_deckOperations.GiveCardForDealer());
                yield return StartCoroutine(WaitAndCountPoints());
            }
            CheckResult(false, true);
        }
        private IEnumerator DoubleCardAndContinue()
        {
            yield return _deckOperations.StartCoroutine(_deckOperations.GiveCardForPlayer());
            StartCoroutine(WaitAndCountPoints());
            CheckResult(true,false);
            Debug.Log("Double Card and Continue, _playerpoints:" + _playerPoints);
            if (_playerPoints <= 21) StartCoroutine(MainLogic());
        }

        private IEnumerator WaitAndCountPoints()
        {
            yield return new WaitForSeconds(CARDANIMATIONTIME);
            _dealerPoints = _deckOperations.CountPointsFromDeck(_decksData.DealerDeck);
            _playerPoints = _deckOperations.CountPointsFromDeck(_decksData.PlayerDeck);
        }
        
        private IEnumerator GiveFirstCards()
        {
            yield return _deckOperations.StartCoroutine(_deckOperations.StartDealCards());
        }

        private IEnumerator WaitForPlayerAction()
        { 
            yield return _playerActions.StartCoroutine(_playerActions.WaitForPlayerAction());
        }
        private void GivePlayerPoints()
        {
            _soundPlayer.PlayCardSound();
            _playerPoints = _deckOperations.CountPointsFromDeck(_decksData.PlayerDeck);
            _uiDataProvider.UpdatePlayerPointsText(_playerPoints);
            CheckResult(true, false);
        }
        
        private void GiveDealerPoints()
        {
            _soundPlayer.PlayCardSound();
            _dealerPoints = _deckOperations.CountPointsFromDeck(_decksData.DealerDeck);
            _uiDataProvider.UpdateDealerPointsText(_dealerPoints);
            CheckResult(false, false);
        }

        private void CheckResult(bool isItPlayer, bool isEndGame)
        {
            if (isEndGame)
            {
                if (_dealerPoints > _playerPoints) 
                {
                    Debug.Log("Dealer Wins!");
                    StartCoroutine(GameLose());
                }
                else if (_dealerPoints < _playerPoints)
                {
                    Debug.Log("Player Wins!");
                    StartCoroutine(GameWin());
                }
                else if (_dealerPoints == _playerPoints)
                {
                    Debug.Log("Push (Tie)");
                    StartCoroutine(GameDraw());
                }
            }
            // if player bust
            if (isItPlayer && _playerPoints > 21)
            {
                Debug.Log("You Lose!");
                StartCoroutine(GameLose());
                return;
            }

            // if dealer bust
            if (!isItPlayer && _dealerPoints > 21)
            {
                Debug.Log("You Win!");
                StartCoroutine(GameWin());
                return;
            }
        }


        private IEnumerator GameWin()
        {
            yield return new WaitForSeconds(CARDANIMATIONTIME);
            _soundPlayer.PlayResultSound();
            _popupShower.GameWin();
        }

        private IEnumerator GameLose()
        {
            yield return new WaitForSeconds(CARDANIMATIONTIME);
            _soundPlayer.PlayResultSound();
            _popupShower.GameLose();
        }
        private IEnumerator GameDraw()
        {
            yield return new WaitForSeconds(CARDANIMATIONTIME);
            _soundPlayer.PlayResultSound();
            _popupShower.GameDraw();
        }
        public void Restart()
        {
            Debug.Log("Restarting game");
            isPlayerDidChoice = false;
            _decksData.Restart();
            StartCoroutine(StartGame());
        }
    }
}