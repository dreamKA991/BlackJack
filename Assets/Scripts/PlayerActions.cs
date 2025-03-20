using System.Collections;
using BlackJack.Infrastructure;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private UIDataProvider _uIDataProvider;
    private DeckOperations _deckOperations;
    private float _animationTime;
    private bool isFirstDeal = false;
    private int _playerChoice = -1;
    public void Init(UIDataProvider uIDataProvider, DeckOperations deckOperations, float animationTime)
    {
        _uIDataProvider = uIDataProvider;
        _deckOperations = deckOperations;
        _animationTime = animationTime;
        SetButtonsActive(false);
        SubscribeButtons();
    }

    private void SubscribeButtons()
    {
        _uIDataProvider.TakeCardsButton.onClick.AddListener(PlayerPressedTakeButton);
        _uIDataProvider.StopCardsButton.onClick.AddListener(PlayerPressedStopButton);
        _uIDataProvider.DoubleCardsButton.onClick.AddListener(PlayerPressedDoubleButton);
        _uIDataProvider.RestartButton.onClick.AddListener(SignalBus.Instance.Restart);
    }

    private void SetButtonsActive(bool isActive)
    {
        _uIDataProvider.TakeCardsButton.gameObject.SetActive(isActive);
        _uIDataProvider.StopCardsButton.gameObject.SetActive(isActive);
        _uIDataProvider.DoubleCardsButton.gameObject.SetActive(isActive);
    }

    private void PlayerPressedTakeButton()
    {
        _deckOperations.StartCoroutine(_deckOperations.GiveCardForPlayer());
    }

    private void PlayerPressedStopButton() => _playerChoice = 1;

    private void PlayerPressedDoubleButton() => _playerChoice = 2;

    private IEnumerator GivePlayerChoice()
    {
        SetButtonsActive(true);
        _playerChoice = -1; 
        
        yield return new WaitUntil(() => _playerChoice != -1);
        
        SetButtonsActive(false);
        SignalBus.Instance.PlayerActionDid(_playerChoice);
    }

    public IEnumerator WaitForPlayerAction()
    {
        yield return new WaitForSeconds(isFirstDeal? _animationTime : _animationTime * 4);
        yield return StartCoroutine(GivePlayerChoice());
    }
}
