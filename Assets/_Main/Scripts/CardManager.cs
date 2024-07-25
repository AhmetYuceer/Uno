using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CardManager : MonoBehaviour
{
    public List<Card> Deck { get; private set; } = new List<Card>();

    [Header("Card Settings")]
    [SerializeField] private GameObject _cardPrebaf;
    [SerializeField] private Transform _deckParentTransform;

    private Array _faceValues;

    private void Start()
    {
        InitilalizeRedCards();
    }

    private void InitilalizeRedCards()
    {
        GameObject spawnedCard = Instantiate(_cardPrebaf, _deckParentTransform);
        spawnedCard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Cards/Red/Red_0");
        spawnedCard.name = "Red_0";
        AddNormalCard(spawnedCard, CardColorEnum.RED, 0);

        for (int i = 1; i < 10; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                spawnedCard = Instantiate(_cardPrebaf, _deckParentTransform);
                spawnedCard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Cards/Red/Red_{i}");
                spawnedCard.name = $"Red_{i}_{j+1}";
                AddNormalCard(spawnedCard, CardColorEnum.RED, i);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            spawnedCard = Instantiate(_cardPrebaf, _deckParentTransform);
            spawnedCard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Cards/Red/Red_Draw");
            spawnedCard.name = $"Red_Draw";
            spawnedCard.AddComponent<DrawCard>();
            AddSpecialCard(spawnedCard, CardColorEnum.RED, CardTypeEnum.DRAW);

             spawnedCard = Instantiate(_cardPrebaf, _deckParentTransform);
            spawnedCard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Cards/Red/Red_Reverse");
            spawnedCard.name = $"Red_Reverse";
            spawnedCard.AddComponent<ReverseCard>();
            AddSpecialCard(spawnedCard, CardColorEnum.RED, CardTypeEnum.REVERSE);

            spawnedCard = Instantiate(_cardPrebaf, _deckParentTransform);
            spawnedCard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Cards/Red/Red_Skip");
            spawnedCard.name = $"Red_Skip";
            spawnedCard.AddComponent<SkipCard>();
            AddSpecialCard(spawnedCard, CardColorEnum.RED, CardTypeEnum.SKIP);
        }
    }

    private void AddSpecialCard(GameObject cardObject, CardColorEnum colorEnum , CardTypeEnum cardTypeEnum)
    {
        switch (cardTypeEnum)
        {
            case CardTypeEnum.DRAW:
                AddDrawCard(cardObject, colorEnum);
                break;
            case CardTypeEnum.SKIP:
                AddSkipCard(cardObject, colorEnum);
                break;
            case CardTypeEnum.REVERSE:
                AddReverseCard(cardObject, colorEnum);
                break;
            case CardTypeEnum.WILD:
                break;
            case CardTypeEnum.WILD_DRAW:
                break;
        }
    }

    private void AddNormalCard(GameObject cardObject, CardColorEnum colorEnum, int index)
    {
        NormalCard normalCard = cardObject.AddComponent<NormalCard>();
        normalCard.CardColor = colorEnum;
        normalCard.CardTypeEnum = CardTypeEnum.NORMAL;

        if (_faceValues == null)
            _faceValues = Enum.GetValues(typeof(CardFaceValueEnum));

        normalCard.FaceValue = (CardFaceValueEnum)_faceValues.GetValue(index);
    }

    private void AddDrawCard(GameObject cardObject, CardColorEnum colorEnum)
    {
        DrawCard drawCard = cardObject.AddComponent<DrawCard>();
        drawCard.CardColor = colorEnum;
        drawCard.CardTypeEnum= CardTypeEnum.DRAW;
    }

    private void AddReverseCard(GameObject cardObject, CardColorEnum colorEnum)
    {
        ReverseCard reverseCard = cardObject.AddComponent<ReverseCard>();
        reverseCard.CardColor = colorEnum;
        reverseCard.CardTypeEnum = CardTypeEnum.REVERSE;
    }

    private void AddSkipCard(GameObject cardObject, CardColorEnum colorEnum)
    {
        SkipCard skipCard = cardObject.AddComponent<SkipCard>();
        skipCard.CardTypeEnum = CardTypeEnum.SKIP;
        skipCard.CardColor = colorEnum;
    }
}