using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public List<Card> Cards { get; private set; } = new List<Card>();

    private const string BLUE_PATH = "Cards/Blue";
    private const string GREEN_PATH = "Cards/Green";
    private const string RED_PATH = "Cards/Red";
    private const string YELLOW_PATH = "Cards/Yellow";
    private const string WILD_PATH = "Cards/Wild";
    private const string BACK_SPRITE = "Cards/Deck/Deck";

    private Sprite _cardBackSprite;

    [Header("Card Settings")]
    [SerializeField] private GameObject _cardPrebaf;
    [SerializeField] private Transform _deckParentTransform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _cardBackSprite = Resources.Load<Sprite>(BACK_SPRITE);
    }

    public void SetupUnoClassic()
    {
        CreateYellowCards();
        //CreateBlueCards();
        //CreateRedCards();
        //CreateGreenCards();
        CreateWildCards();
    }

    #region CardSetupUnoClassic
    private void CreateWildCards()
    {
        CreateSpecialCard(CardTypeEnum.WILD_DRAW, CardColorEnum.WILD, 4);
        CreateSpecialCard(CardTypeEnum.WILD, CardColorEnum.WILD, 50);
    }

    private void CreateYellowCards()
    {
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.ZERO, 1);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.ONE, 2);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.TWO, 2);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.THREE, 2);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.FOUR, 2);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.FIVE, 2);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.SIX, 2);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.SEVEN, 2);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.EIGHT, 2);
        CreateNormalCard(CardColorEnum.YELLOW, CardFaceValueEnum.NINE, 2);

        CreateSpecialCard(CardTypeEnum.DRAW, CardColorEnum.YELLOW, 2);
        CreateSpecialCard(CardTypeEnum.REVERSE, CardColorEnum.YELLOW, 2);
        CreateSpecialCard(CardTypeEnum.SKIP, CardColorEnum.YELLOW, 2);
    }
    
    private void CreateGreenCards()
    {
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.ZERO, 1);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.ONE, 2);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.TWO, 2);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.THREE, 2);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.FOUR, 2);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.FIVE, 2);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.SIX, 2);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.SEVEN, 2);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.EIGHT, 2);
        CreateNormalCard(CardColorEnum.GREEN, CardFaceValueEnum.NINE, 2);

        CreateSpecialCard(CardTypeEnum.DRAW, CardColorEnum.GREEN, 2);
        CreateSpecialCard(CardTypeEnum.REVERSE, CardColorEnum.GREEN, 2);
        CreateSpecialCard(CardTypeEnum.SKIP, CardColorEnum.GREEN, 2);
    }

    private void CreateBlueCards()
    {
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.ZERO, 1);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.ONE, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.TWO, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.THREE, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.FOUR, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.FIVE, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.SIX, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.SEVEN, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.EIGHT, 2);
        CreateNormalCard(CardColorEnum.BLUE, CardFaceValueEnum.NINE, 2);

        CreateSpecialCard(CardTypeEnum.DRAW, CardColorEnum.BLUE, 2);
        CreateSpecialCard(CardTypeEnum.REVERSE, CardColorEnum.BLUE, 2);
        CreateSpecialCard(CardTypeEnum.SKIP, CardColorEnum.BLUE, 2);
    }

    private void CreateRedCards()
    {
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.ZERO, 1);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.ONE, 2);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.TWO, 2);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.THREE, 2);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.FOUR, 2);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.FIVE, 2);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.SIX, 2);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.SEVEN, 2);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.EIGHT, 2);
        CreateNormalCard(CardColorEnum.RED, CardFaceValueEnum.NINE, 2);

        CreateSpecialCard(CardTypeEnum.DRAW, CardColorEnum.RED, 2);
        CreateSpecialCard(CardTypeEnum.REVERSE, CardColorEnum.RED, 2);
        CreateSpecialCard(CardTypeEnum.SKIP, CardColorEnum.RED, 2);
    }
    #endregion

    private void CreateNormalCard(CardColorEnum cardColor, CardFaceValueEnum cardFaceValue, int cardCount)
    {
        if (cardColor == CardColorEnum.WILD)
        {
            Debug.LogError("Error: Normal cards cannot have the WILD color.");
            return;
        }

        string path = GetPathForColor(cardColor);
        string value = ((int)cardFaceValue).ToString();
        string fileName = path.Split('/')[1];
        string cardName = $"{fileName}_{value}";
        
        path += $"/{cardName}";
        
        Sprite cardSprite = Resources.Load<Sprite>(path);
        GameObject[] spawnedCards = InstantiateCardObjects(cardCount);

        SetNormalCards(spawnedCards, cardSprite, cardName, cardFaceValue, cardColor);
    }

    private void CreateSpecialCard(CardTypeEnum cardType, CardColorEnum cardColor , int cardCount)
    {
        if (cardType == CardTypeEnum.NORMAL)
        {
            Debug.LogWarning("Error: To create the normal card, use the method called 'CreateNormalCard'");
            return;
        }
        if (cardColor == CardColorEnum.WILD && (cardType != CardTypeEnum.WILD && cardType != CardTypeEnum.WILD_DRAW))
        {
            Debug.LogWarning($"Error: Only WILD or WILD_DRAW card types can have the WILD color.\n CardType: {cardType}, CardColor: {cardColor} \n");
            return;
        }
        if(cardColor != CardColorEnum.WILD && (cardType == CardTypeEnum.WILD || cardType == CardTypeEnum.WILD_DRAW))
        {
            Debug.LogWarning($"Error: WILD or WILD_DRAW card types must have the WILD color. \n CardType: {cardType}, CardColor: {cardColor} \n");
            return;
        }

        string path = GetPathForSpecialCardSprite(cardType, cardColor);
        string cardName = path.Split('/')[2];

        if (path.Length > 0)
        {
            Sprite cardSprite = Resources.Load<Sprite>(path);
            GameObject[] spawnedCards = InstantiateCardObjects(cardCount);
            SetCardAttribute(cardType, spawnedCards, cardColor, cardSprite, cardName);
        }
    } 

    private GameObject[] InstantiateCardObjects(int cardCount)
    {
        GameObject[] spawnedCards = new GameObject[cardCount];

        for (int i = 0; i < cardCount; i++)
        {
            GameObject spawnedCard = Instantiate(_cardPrebaf, _deckParentTransform);
            spawnedCard.SetActive(false);
            spawnedCards[i] = spawnedCard;
        }
        return spawnedCards;
    }

    #region CardSetupMethods
    private void SetCardAttribute(CardTypeEnum cardType, GameObject[] cardObjects, CardColorEnum cardColor, Sprite cardSprite, string cardName)
    {
        switch (cardType)
        {
            case CardTypeEnum.DRAW:
                SetDrawCard(cardObjects, cardColor, cardSprite, cardName);
                break;
            case CardTypeEnum.REVERSE:
                SetReverseCard(cardObjects, cardColor, cardSprite, cardName);
                break;
            case CardTypeEnum.SKIP:
                SetSkipCard(cardObjects, cardColor, cardSprite, cardName);
                break;
            case CardTypeEnum.WILD:
                SetWildCard(cardObjects, cardColor, cardSprite, cardName);
                break;
            case CardTypeEnum.WILD_DRAW:
                SetWildDrawCard(cardObjects, cardColor, cardSprite, cardName);
                break;
        }
    }

    private void SetNormalCards(GameObject[] spawnedCards, Sprite cardFrontSprite, string cardName, CardFaceValueEnum cardFaceValue, CardColorEnum cardColor)
    {
        foreach (var spawnedCard in spawnedCards)
        {
            NormalCard normalCard = spawnedCard.AddComponent<NormalCard>().GetComponent<NormalCard>();
            normalCard.name = cardName;
            normalCard.FaceValue = cardFaceValue;
            normalCard.CardColor = cardColor;
            normalCard.CardTypeEnum = CardTypeEnum.NORMAL;
            normalCard.SetSprite(cardFrontSprite, _cardBackSprite);
            Cards.Add(normalCard);
        }
    }
    private void SetDrawCard(GameObject[] cardObjects, CardColorEnum cardColor, Sprite cardFrontSprite, string cardName)
    {
        foreach (var card in cardObjects)
        {
            DrawCard drawCard = card.AddComponent<DrawCard>().GetComponent<DrawCard>();
            drawCard.name = cardName;
            drawCard.CardColor = cardColor;
            drawCard.CardTypeEnum = CardTypeEnum.DRAW;
            drawCard.SetSprite(cardFrontSprite, _cardBackSprite);
            Cards.Add(drawCard);
        }
    }
    private void SetReverseCard(GameObject[] cardObjects, CardColorEnum cardColor, Sprite cardFrontSprite, string cardName)
    {
        foreach (var card in cardObjects)
        {
            ReverseCard reverseCard = card.AddComponent<ReverseCard>().GetComponent<ReverseCard>();
            reverseCard.CardColor = cardColor;
            reverseCard.name = cardName;
            reverseCard.CardTypeEnum = CardTypeEnum.REVERSE;
            reverseCard.SetSprite(cardFrontSprite, _cardBackSprite);
            Cards.Add(reverseCard);
        }
    }
    private void SetSkipCard(GameObject[] cardObjects, CardColorEnum cardColor, Sprite cardFrontSprite, string cardName)
    {
        foreach (var card in cardObjects)
        {
            SkipCard skipCard = card.AddComponent<SkipCard>().GetComponent<SkipCard>();
            skipCard.CardColor = cardColor;
            skipCard.name = cardName;
            skipCard.CardTypeEnum = CardTypeEnum.SKIP;
            skipCard.SetSprite(cardFrontSprite, _cardBackSprite);
            Cards.Add(skipCard);
        }
    }
    private void SetWildCard(GameObject[] cardObjects, CardColorEnum cardColor, Sprite cardFrontSprite, string cardName)
    {
        foreach (var card in cardObjects)
        {
            WildCard wildCard = card.AddComponent<WildCard>().GetComponent<WildCard>();
            wildCard.CardColor = cardColor;
            wildCard.name = cardName;
            wildCard.CardTypeEnum = CardTypeEnum.WILD;
            wildCard.SetSprite(cardFrontSprite, _cardBackSprite);
            Cards.Add(wildCard);
        }
    }
    private void SetWildDrawCard(GameObject[] cardObjects, CardColorEnum cardColor, Sprite cardFrontSprite, string cardName)
    {
        foreach (var card in cardObjects)
        {
            WildDrawCard wildDrawCard = card.AddComponent<WildDrawCard>().GetComponent<WildDrawCard>();
            wildDrawCard.CardColor = cardColor;
            wildDrawCard.name = cardName;
            wildDrawCard.CardTypeEnum = CardTypeEnum.WILD_DRAW;
            wildDrawCard.SetSprite(cardFrontSprite, _cardBackSprite);
            Cards.Add(wildDrawCard);
        }
    }
    #endregion

    #region Helpers
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

    private string GetPathForColor(CardColorEnum cardColor)
    {
        string path = "";

        switch (cardColor)
        {
            case CardColorEnum.BLUE:
                path = BLUE_PATH;
                break;
            case CardColorEnum.RED:
                path = RED_PATH;
                break;
            case CardColorEnum.YELLOW:
                path = YELLOW_PATH;
                break;
            case CardColorEnum.GREEN:
                path = GREEN_PATH;
                break;
            case CardColorEnum.WILD:
                path = WILD_PATH;
                break;
            default:
                path = "";
                break;
        }
        return path;
    }
    #endregion
}