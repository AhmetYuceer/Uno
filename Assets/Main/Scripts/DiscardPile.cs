using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscardPile : MonoBehaviour
{
    public static DiscardPile Instance;

    private Transform _droppedCardsTranform;
    private Stack<Card> _droppedCards;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
 
    public void SetManager(Transform droppedCardsTranform , int droppedCardCapacity)
    {
        _droppedCardsTranform = droppedCardsTranform;
        _droppedCards = new Stack<Card>(droppedCardCapacity);
    }

    public IEnumerator DropCard(Card card)
    {
        card.transform.rotation = Quaternion.identity;
        _droppedCards.Push(card);

        List<Card> temp = new List<Card>(_droppedCards);
        temp.Reverse();

        for (int i = 0; i < temp.Count; i++)
        {
            if (i != 0)
                temp[i].GetCardSpriteRenderer().sortingOrder = temp[i - 1].GetCardSpriteRenderer().sortingOrder + 2;
            else
                temp[i].GetCardSpriteRenderer().sortingOrder = 2;

            temp[i].GetCardBorderSpriteRenderer().sortingOrder = temp[i].GetCardSpriteRenderer().sortingOrder - 1;
        }

        card.TurnFront();
        card.transform.SetParent(_droppedCardsTranform);
        card.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        card.transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(1f);
    }

    public Card GetLastDiscardedCard()
    {
        return _droppedCards.Peek();
    }
}
