using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class PopupShower : MonoBehaviour, IRestartable
{ 
    private GameObject _popup;
    private Image _image;
    private TMP_Text _resultText;
    private const float ANIMATIONDURATION = 1f;
    private bool isGameOver = false;

    public void Init(UIDataProvider uiDataProvider)
    {
        _popup = uiDataProvider.GameEndPopup;
        _image = _popup.GetComponent<Image>();
        _resultText = uiDataProvider.GameEndText;
        SignalBus.Instance.OnRestart.AddListener(Restart);
    }
    public void GameWin() =>
        ShowResult("YOU WIN", Color.green);

    public void GameLose() =>
        ShowResult("YOU LOOSE", Color.red);
    
    public void GameDraw() =>
        ShowResult("DRAW", Color.yellow);

    private void ShowResult(string textResult, Color color)
    {
        if(isGameOver) return;
        isGameOver = true;
        _resultText.text = textResult;
        _resultText.color = color;
        FadeIn();
        _popup.SetActive(true);
    }
    
    private void FadeIn()
    {
        Color color = _image.color;
        color.a = 0; 
        _image.color = color;
        
        _image.DOFade(0.86f, ANIMATIONDURATION);
    }

    public void Restart() => isGameOver = false;
}
