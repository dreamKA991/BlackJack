using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour, IRestartable
{
    [SerializeField] private AudioClip _cardSound;
    [SerializeField] private AudioClip _resultSound;
    private AudioSource _audioSource;
    private bool isGameEnded = false;

    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();
        SignalBus.Instance.OnRestart.AddListener(Restart);
    } 

    public void PlayCardSound()
    {
        if (_cardSound != null)
            _audioSource.PlayOneShot(_cardSound);
        else
            Debug.LogWarning("cardSound не назначен!", this);
    }
    public void PlayResultSound()
    {
        if(isGameEnded) return;
        isGameEnded = true;
        if (_resultSound != null)
            _audioSource.PlayOneShot(_resultSound);
        else
            Debug.LogWarning("resultSound не назначен!", this);
    }

    public void Restart()
    {
        isGameEnded = false;
    }
}