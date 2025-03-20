using UnityEngine;
using UnityEngine.Events;

public class SignalBus : MonoBehaviour
{
    public static SignalBus Instance { get; private set; }

    public UnityEvent OnPlayerTakedCard;
    public UnityEvent OnDealerTakedCard;
    public UnityEvent OnRestart;
    public UnityEvent<int> OnPlayerActionDid;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayerTookCard()
    {
        OnPlayerTakedCard?.Invoke();
    }

    public void DealerTookCard()
    {
        OnDealerTakedCard?.Invoke();
    }

    public void PlayerActionDid(int choice)
    {
        OnPlayerActionDid?.Invoke(choice);
    }

    public void Restart()
    {
        OnRestart?.Invoke();
    }
}