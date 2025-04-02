using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIDataProvider : MonoBehaviour, IRestartable
{
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private Transform _dealerCardsTransform;
    [SerializeField] private Transform _playerCardsTransform;
    [SerializeField] private Transform _deckCardsTransform;
    [SerializeField] private Text _playerPointsText;
    [SerializeField] private Text _dealerPointsText;
    [SerializeField] private Button _takeCardsButton;
    [SerializeField] private Button _stopCardsButton;
    [SerializeField] private Button _doubleCardsButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private GameObject _gameEndPopup;
    [SerializeField] private TMP_Text _gameEndText;

    private void Start()
    {
        SignalBus.Instance.OnRestart.AddListener(Restart);
    } 
    public Transform CanvasTransform => _canvasTransform;
    public Transform DealerCardsTransform => _dealerCardsTransform;
    public Transform PlayerCardsTransform => _playerCardsTransform;
    public Transform DeckCardsTransform => _deckCardsTransform;
    public void UpdatePlayerPointsText(int value) => _playerPointsText.text = value.ToString();
    public void UpdateDealerPointsText(int value) => _dealerPointsText.text = value.ToString();
    
    public Button TakeCardsButton => _takeCardsButton;
    public Button StopCardsButton => _stopCardsButton;
    public Button DoubleCardsButton => _doubleCardsButton;
    public Button RestartButton => _restartButton;
    public GameObject GameEndPopup => _gameEndPopup;
    public TMP_Text GameEndText => _gameEndText;

    public void Restart()
    {
        _playerPointsText.text = "0";
        _dealerPointsText.text = "0";
        _gameEndPopup.SetActive(false);
    }
}