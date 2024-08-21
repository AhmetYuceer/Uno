using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class DiscardPile : MonoBehaviour
{
    public static DiscardPile Instance;
    public Card LastDiscardedCard { get; private set; }
    private Transform _discardedCardsTranform;
    private Stack<Card> _discardedCards;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Stack<Card> GetAllDiscardedCards()
    {
        return _discardedCards;
    }

    public void SetManager(Transform droppedCardsTranform, int droppedCardCapacity)
    {
        _discardedCardsTranform = droppedCardsTranform;
        _discardedCards = new Stack<Card>(droppedCardCapacity);
    }

    public IEnumerator DiscardCard(Card card, Player player)
    {
        _discardedCards.Push(card);
        LastDiscardedCard = card;
        card.IsDiscarded = true;
        card.IsShowable = false;
        card.IsSelectable = false;
     
        yield return new WaitForSeconds(0.1f);
        
        StartCoroutine(DiscardAnimation(card, player));
    }

    private IEnumerator DiscardAnimation(Card card, Player player)
    {
        card.SetMaxOrder();
        card.TurnFront();

        card.transform.SetParent(_discardedCardsTranform);
        card.transform.localRotation = Quaternion.identity;

        if (_discardedCards.Count % 2 == 0)
            card.transform.localRotation = Quaternion.Euler(0, 0, 15);
        else
            card.transform.localRotation = Quaternion.Euler(0, 0, -15);

        card.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        card.transform.DOLocalMove(Vector3.zero, 0.5f)
        .OnComplete(() =>
        {
            if (player != null && GameManager.Instance.IsPlay)
            {
                card.SetDefauldOrder();

                player.Cards.Remove(card);

                if (player.Cards.Count < 1)
                    GameManager.Instance.EndGame(player);
                
                card.ApplyAction(player);
                StartCoroutine(player.ArrangeTheCards());
            }
        });
        
        yield return new WaitForSeconds(0.2f);

        int sortingOrder = _discardedCards.Count * 2;
        card.SetOrder(sortingOrder);
        card.StopLookingCard();
    }
}
