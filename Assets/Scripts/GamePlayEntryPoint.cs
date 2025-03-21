using BlackJack.Gameplay;
using UnityEngine;
[RequireComponent(typeof(UIDataProvider),typeof(GamePlayLogic), typeof(PlayerActions))]
public class GamePlayEntryPoint : MonoBehaviour
{
    [SerializeField] private CardConfig _config;
    private UIDataProvider _uiDataProvider;
    private GamePlayLogic _gamePlayLogic;
    private SoundPlayer _soundPlayer;
    private void Start()
    {
        _uiDataProvider = GetComponent<UIDataProvider>();
        _soundPlayer = GetComponent<SoundPlayer>();
        _gamePlayLogic = GetComponent<GamePlayLogic>();
        _gamePlayLogic.Init(_config, _uiDataProvider, _soundPlayer);
        _gamePlayLogic.StartGame();
    }
}
