using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Player : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();
    public List<Card> SelectableCards = new List<Card>();

    public Transform CardsParentTransform;
    public float Offset;

    private SpriteRenderer _spriteRenderer;
    private bool _isMyTurn = false;

    public virtual bool MyTurn
    {
        get
        {
            return  _isMyTurn;
        }
        set
        {
            _isMyTurn = value;

            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_isMyTurn)
            {
                _spriteRenderer.color = Color.green;
                SetSelectableCards();
            }
            else
            {
                _spriteRenderer.color = Color.white;
            }
        }
    }
    
    public virtual void Move(Card card) { }

    public virtual void AddCard(Card card)
    {
        card.gameObject.transform.SetParent(CardsParentTransform);
        Cards.Add(card);
        ArrangeTheCards();
    }

    public void PlayCard(Card card)
    {
        StartCoroutine(GameManager.Instance.DiscardPile.DropCard(card));
        Cards.Remove(card);
        ArrangeTheCards();
    }
    
    public void SetSelectableCards()
    {
        Card lastDiscardedCard = GameManager.Instance.DiscardPile.GetLastDiscardedCard();
        List<WildDrawCard> wildDrawCards = new List<WildDrawCard>();

        foreach (var card in Cards)
        {
            if (lastDiscardedCard.CardColor == card.CardColor)
            {
                card.IsSelectable = true;
                SelectableCards.Add(card);
            }
            else
            {
                switch (card.CardTypeEnum)
                {
                    case CardTypeEnum.NORMAL:
                        if (lastDiscardedCard.CardTypeEnum == card.CardTypeEnum)
                        {
                            NormalCard normalCard = (NormalCard)card;
                            NormalCard discaredNormalCard = (NormalCard)lastDiscardedCard;

                            if (discaredNormalCard.FaceValue == normalCard.FaceValue)
                            {
                                card.IsSelectable = true;
                                SelectableCards.Add(card);
                            }
                        }
                        break;
                    case CardTypeEnum.DRAW:
                        if (lastDiscardedCard.CardTypeEnum == card.CardTypeEnum)
                        {
                            card.IsSelectable = true;
                            SelectableCards.Add(card);
                        }
                        break;
                    case CardTypeEnum.REVERSE:
                        if (lastDiscardedCard.CardTypeEnum == card.CardTypeEnum)
                        {
                            card.IsSelectable = true;
                            SelectableCards.Add(card);
                        }
                        break;
                    case CardTypeEnum.SKIP:
                        if (lastDiscardedCard.CardTypeEnum == card.CardTypeEnum)
                        {
                            card.IsSelectable = true;
                            SelectableCards.Add(card);
                        }
                        break;
                    case CardTypeEnum.WILD:
                        card.IsSelectable = true;
                        SelectableCards.Add(card);
                        break;
                    case CardTypeEnum.WILD_DRAW:
                        wildDrawCards.Add((WildDrawCard)card);
                        break;
                }
            }
        }

        if (wildDrawCards.Count > 0 && SelectableCards.Count == 0)
        {
            foreach (var wildDrawCard in wildDrawCards)
            {
                wildDrawCard.IsSelectable = true;
                SelectableCards.Add(wildDrawCard);
            }
        }

        foreach (var card in Cards)
        {
            if (card.IsSelectable)
            {
                card.transform.localPosition = new Vector3(0, 2,0);
                card.TurnFront();
            }
        }
    }

    private void ArrangeTheCards()
    {
        foreach (var card in Cards)
        {
            card.GetComponent<Transform>().localRotation = Quaternion.identity;
            card.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (Cards.Count > 7)
        {
            float scaleFactor = (float)7 / Cards.Count;
            CardsParentTransform.transform.localScale = new Vector3(scaleFactor, scaleFactor, CardsParentTransform.transform.localScale.z);
        }

        float totalWidth = 0;
        foreach (var card in Cards)
        {
            totalWidth += card.GetComponent<Transform>().localScale.x;
        }

        float startX = CardsParentTransform.localPosition.x - totalWidth / 2;
        float currentX = startX;

        for (int i = 0; i < Cards.Count; i++)
        {
            Vector3 cardSize = Cards[i].GetComponent<Transform>().localScale;
            Cards[i].transform.localPosition = new Vector3(((currentX + cardSize.x) / Offset) + (CardsParentTransform.transform.localPosition.x * -1), 0, 0);
            currentX += cardSize.x;

            if (i!= 0)
                Cards[i].GetCardSpriteRenderer().sortingOrder = Cards[i-1].GetCardSpriteRenderer().sortingOrder + 2;
            else
                Cards[i].GetCardSpriteRenderer().sortingOrder = 2;
            
            Cards[i].GetCardBorderSpriteRenderer().sortingOrder = Cards[i].GetCardSpriteRenderer().sortingOrder - 1;
        }
    }
} 