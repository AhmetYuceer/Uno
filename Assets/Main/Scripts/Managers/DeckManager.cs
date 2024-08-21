using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [SerializeField] public Transform DeckTransform;
    public Transform DroppedCardsTranform;
    private Stack<Card> _deck;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DeckTransform = transform;
    }

    public void SetDeck(List<Card> cards)
    {
        Shuffle(cards);
        _deck = new Stack<Card>(cards);
    }
    
    public Card GetCard()
    {
        Card card;

        if (_deck.Count <= 0)
        {
            card = ShuffleCardsAgain();
            card.gameObject.SetActive(true);
            return card;
        }

        card = _deck.Pop();
        card.gameObject.SetActive(true);
        return card;
    }

    private Card ShuffleCardsAgain()
    {
        List<Card> cards = new List<Card>();
        int count = DiscardPile.Instance.GetAllDiscardedCards().Count;

        while (count > 0)
        {
            Card card1 = DiscardPile.Instance.GetAllDiscardedCards().Pop();
            card1.transform.SetParent(DeckTransform);
            card1.IsDiscarded = false;
            card1.transform.localPosition = Vector3.zero;
            card1.transform.rotation = Quaternion.identity;
            card1.TurnBack();
            card1.gameObject.SetActive(false);
            cards.Add(card1);
            count--;
        }
        SetDeck(cards);

        Card card = _deck.Pop();
        card.gameObject.SetActive(true);
        return card;
    }

    public void PutCardBackOfDeck(Card card)
    {
        card.gameObject.SetActive(false);
        Stack<Card> temp = new Stack<Card>(_deck);
        temp.Reverse();
        temp.Push(card);
        temp.Reverse();
        _deck = new Stack<Card>(temp);
    }

    public void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int j = rng.Next(i, n);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}