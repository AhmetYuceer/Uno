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
        SortDiscardedCardList();
        card.DiscardedCard();

        yield return new WaitForSeconds(0.2f);

        DiscardAnimation(card, player);

        yield return new WaitForSeconds(0.5f);
        
        if (player != null)
            StartCoroutine(player.ArrangeTheCards());
    }

    private void DiscardAnimation(Card card, Player player)
    {
        card.transform.SetParent(_discardedCardsTranform);
        card.transform.localRotation = Quaternion.identity;
        card.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        card.transform.DOLocalMove(Vector3.zero, 0.5f)
        .OnComplete(() =>
        {
            card.StopLookingCard();

            if (player != null)
                card.ApplyAction(player);
        });
    }

    private void SortDiscardedCardList()
    {
        List<Card> temp = new List<Card>(_discardedCards);
        temp.Reverse();

        for (int i = 0; i < temp.Count; i++)
        {
            if (i != 0)
                temp[i].GetCardSpriteRenderer().sortingOrder = temp[i - 1].GetCardSpriteRenderer().sortingOrder + 2;
            else
                temp[i].GetCardSpriteRenderer().sortingOrder = 2;

            temp[i].GetCardBorderSpriteRenderer().sortingOrder = temp[i].GetCardSpriteRenderer().sortingOrder - 1;
        }
    }

    public Card GetLastDiscardedCard()
    {
        return _discardedCards.Peek();
    }
}
