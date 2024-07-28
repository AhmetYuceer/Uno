using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine.Rendering;

public class CardManager : MonoBehaviour
{
    public List<Card> Deck { get; private set; } = new List<Card>();

    [Header("Card Settings")]
    [SerializeField] private GameObject _cardPrebaf;
    [SerializeField] private Transform _deckParentTransform;

    private void Start()
    {
        InitilalizeCards();
    }

    private void InitilalizeCards()
    {
        CreateSpecialCard(CardTypeEnum.NORMAL, CardColorEnum.BLUE, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.NINE, 2);
    }

    private void CreateNormalCard(CardColorEnum cardColor, CardFaceValueEnum cardFaceValue, int cardCount)
    {
        string path = GetPathForColor(cardColor);
        string value = ((int)cardFaceValue).ToString();
        string fileName = path.Split('/')[1];
        string cardName = $"{fileName}_{value}";
        
        path += $"/{cardName}";
        
        Sprite sprite = Resources.Load<Sprite>(path);
        
        for (int i = 0; i < cardCount; i++)
        {
            GameObject spawnedCard = Instantiate(_cardPrebaf, _deckParentTransform);
            NormalCard normalCard = spawnedCard.AddComponent<NormalCard>().GetComponent<NormalCard>();
            normalCard.GetComponent<SpriteRenderer>().sprite = sprite;
            normalCard.name = cardName;
            normalCard.FaceValue = cardFaceValue;
            normalCard.CardColor = cardColor;
            normalCard.CardTypeEnum = CardTypeEnum.NORMAL;
        }
    }

    private void CreateSpecialCard(CardTypeEnum cardType, CardColorEnum cardColor , int cardCount)
    {
        if (cardType == CardTypeEnum.NORMAL)
        {
            Debug.LogWarning("To create the normal card, use the method called 'CreateNormalCard'");
            return;
        }

        string path = GetPathForSpecialCardSprite(cardType, cardColor);
        string cardName = path.Split('/')[2];

        if (path.Length > 0)
        {
            GameObject[] spawnedCards = new GameObject[cardCount];
            Sprite cardSprite = Resources.Load<Sprite>(path);
            for (int i = 0; i < cardCount; i++)
            {
                GameObject spawnedCard = Instantiate(_cardPrebaf, _deckParentTransform);
                spawnedCard.name = cardName;
                spawnedCard.GetComponent<SpriteRenderer>().sprite = cardSprite;
                spawnedCards[i] = spawnedCard;
            }
            SetCardAttribute(cardType, spawnedCards, cardColor);
        }
    } 

    private string GetPathForColor(CardColorEnum cardColor)
    {
        string path = "";

        switch (cardColor)
        {
            case CardColorEnum.BLUE:
                path = "Cards/Blue";
                break;
            case CardColorEnum.RED:
                path = "Cards/Red";
                break;
            case CardColorEnum.YELLOW:
                path = "Cards/Yellow";
                break;
            case CardColorEnum.GREEN:
                path = "Cards/Green";
                break;
            case CardColorEnum.WILD:
                path = "Cards/Wild";
                break;
            default:
                path = "";
                break;
        }
        return path;
    }

    private void SetCardAttribute(CardTypeEnum cardType, GameObject[] cardObjects, CardColorEnum cardColor)
    {
        switch (cardType)
        {
            case CardTypeEnum.DRAW:
                foreach (var card in cardObjects)
                {
                    DrawCard drawCard = card.AddComponent<DrawCard>().GetComponent<DrawCard>();
                    drawCard.CardColor = cardColor;
                    drawCard.CardTypeEnum = cardType;
                }
                break;
            case CardTypeEnum.REVERSE:
                foreach (var card in cardObjects)
                {
                    ReverseCard reverseCard = card.AddComponent<ReverseCard>().GetComponent<ReverseCard>();
                    reverseCard.CardColor = cardColor;
                    reverseCard.CardTypeEnum = cardType;
                }
                break;
            case CardTypeEnum.SKIP:
                foreach (var card in cardObjects)
                {
                    SkipCard skipCard = card.AddComponent<SkipCard>().GetComponent<SkipCard>();
                    skipCard.CardColor = cardColor;
                    skipCard.CardTypeEnum = cardType;
                }
                break;
            case CardTypeEnum.WILD:
                foreach (var card in cardObjects)
                {
                    WildCard wildCard = card.AddComponent<WildCard>().GetComponent<WildCard>();
                    wildCard.CardColor = cardColor;
                    wildCard.CardTypeEnum = cardType;
                }
                break;
            case CardTypeEnum.WILD_DRAW:
                foreach (var card in cardObjects)
                {
                    WildDrawCard wildDrawCard = card.AddComponent<WildDrawCard>().GetComponent<WildDrawCard>();
                    wildDrawCard.CardColor = cardColor;
                    wildDrawCard.CardTypeEnum = cardType;
                }
                break;
        }
    }

    private string GetPathForSpecialCardSprite(CardTypeEnum cardType, CardColorEnum cardColor)
    {
        string path = GetPathForColor(cardColor);
        if (path.Length > 0)
        {
            string fileName = path.Split('/')[1];
            switch (cardType)
            {
                case CardTypeEnum.DRAW:
                    path += $"/{fileName}_Draw";
                    break;
                case CardTypeEnum.REVERSE:
                    path += $"/{fileName}_Reverse";
                    break;
                case CardTypeEnum.SKIP:
                    path += $"/{fileName}_Skip";
                    break;
                case CardTypeEnum.WILD:
                    path += $"/{fileName}";
                    break;
                case CardTypeEnum.WILD_DRAW:
                    path += $"/{fileName}_Draw";
                    break;
                default:
                    path = "";
                    break;
            }
        }
        return path;
    } 
}