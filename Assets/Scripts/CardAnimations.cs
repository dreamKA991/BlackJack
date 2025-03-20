using DG.Tweening;
using UnityEngine;

public class CardAnimations
{
    private Transform _dealerCardsTransform, _playerCardsTransform, _deckTransform;
    public CardAnimations(UIDataProvider uiDataProvider)
    {
        _dealerCardsTransform = uiDataProvider.DealerCardsTransform;
        _playerCardsTransform = uiDataProvider.PlayerCardsTransform;
        _deckTransform = uiDataProvider.DeckCardsTransform;
    }
    
    public void DoCardMoveToPlayer(Transform ObjectTransform, float AnimationTime) =>
        DoAnimationCardToPoint(ObjectTransform, _playerCardsTransform, AnimationTime);

    public void DoCardMoveToDealer(Transform ObjectTransform, float AnimationTime) =>
        DoAnimationCardToPoint(ObjectTransform, _dealerCardsTransform, AnimationTime);
    
    private void DoAnimationCardToPoint(Transform ObjectTransform, Transform Destination, float AnimationTime)
    {
        ObjectTransform.position = _deckTransform.position;
        ObjectTransform.DOMove(Destination.position, AnimationTime);
    }
}