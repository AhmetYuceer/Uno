using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public List<Card> Deck { get; private set; } = new List<Card>();

    [Header("Card Settings")]
    [SerializeField] private GameObject _cardPrebaf;
 
    // 1 tane 0 say�s�
    // 2 �er tane di�er say�lar 

    private void InitilalizeRedCards()
    {
        //GameObject spawnedCard = 
    }
}