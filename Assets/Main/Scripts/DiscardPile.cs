using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class DiscardPile : MonoBehaviour
{
    public static DiscardPile Instance;

    private Transform _discardedCardsTranform;
    private Stack<Card> _discardedCards;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetManager(Transform droppedCardsTranform, int droppedCardCapacity)
    {
        _discardedCardsTranform = droppedCardsTranform;
        _discardedCards = new Stack<Card>(droppedCardCapacity);
    }

    public IEnumerator DiscardCard(Card card, Player player)
    {
        _discardedCards.Push(card);

        card.IsDiscarded = true;
        card.IsShowable = false;
        card.IsSelectable = false;
        card.TurnFront();
        card.LookAtCard();
     
        yield return new WaitForSeconds(0.6f);
        
        StartCoroutine(DiscardAnimation(card, player));
    }

    private IEnumerator DiscardAnimation(Card card, Player player)
    {
        card.transform.SetParent(_discardedCardsTranform);
        card.transform.localRotation = Quaternion.identity;
        card.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        card.transform.DOLocalMove(Vector3.zero, 0.5f)
        .OnComplete(() =>
        {
            if (player != null)
            {
                card.ApplyAction(player);
                StartCoroutine(player.ArrangeTheCards());
            }
        });
        
        yield return new WaitForSeconds(0.2f);

        int sortingOrder = _discardedCards.Count * 2;
        card.SetOrder(sortingOrder);
        card.StopLookingCard();
    }

    public Card GetLastDiscardedCard()
    {
        return _discardedCards.Peek();
    }
}
