using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    private Stack<Card> _deck;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetDeck(List<Card> cards)
    {
        Shuffle(cards);
        _deck = new Stack<Card>(cards);
    }

    public Card GetCard()
    {
        Card card = _deck.Pop();
        return card;
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