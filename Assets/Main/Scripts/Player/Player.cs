using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
public abstract class Player : MonoBehaviour
{
    public bool IsDraw;
    public bool IsDrawCard;
    public bool IsWildDraw;

    public List<Card> Cards = new List<Card>();
    public List<Card> SelectableCards = new List<Card>();

    public Transform CardsParentTransform;
    [HideInInspector] public float Offset;

    public SpriteRenderer SpriteRenderer;
    private bool _isMyTurn = false;

    public virtual bool MyTurn
    {
        get
        {
            return _isMyTurn;
        }
        set
        {
            _isMyTurn = value;

            if (SpriteRenderer == null)
                SpriteRenderer = GetComponent<SpriteRenderer>();

            if (_isMyTurn)
            {
                SpriteRenderer.color = Color.green;
            }
            else
            {
                SpriteRenderer.color = Color.white;
            }
        }
    }

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void DrawCard(int cardCount, CardTypeEnum cardType) { }

    public void DiscardCard(Card card) 
    {
        StartCoroutine(GameManager.Instance.DiscardPile.DiscardCard(card, this));
        ClearSelectableCards();
    }
    
    public virtual void AddCard(Card card)
    {
        Cards.Add(card);
        card.transform.SetParent(CardsParentTransform);
        card.transform.localRotation = Quaternion.identity;
        AnimationAddCard(card);
    }

    public void AnimationAddCard(Card card)
    {
        card.SetMaxOrder();
        card.transform.DOLocalMove(Vector3.zero, 0.5f)
        .OnComplete(() =>
        {
            card.SetDefauldOrder();
        });
    }

    private void ClearSelectableCards()
    {
        foreach (var item in SelectableCards)
            item.IsSelectable = false;

        SelectableCards.Clear();
    }

    public void SetSelectableCards()
    {
        ClearSelectableCards();

        Card lastDiscardedCard = GameManager.Instance.DiscardPile.LastDiscardedCard;
        List<WildDrawCard> wildDrawCards = new List<WildDrawCard>();

        foreach (var card in Cards)
        {
            if (lastDiscardedCard.CardColor == card.CardColor)
            {
                SetSelectableCard(card);
            }
            else
            {
                if (card.CardTypeEnum == CardTypeEnum.WILD || card.CardTypeEnum == CardTypeEnum.WILD_DRAW)
                {
                    switch (card.CardTypeEnum)
                    {
                        case CardTypeEnum.WILD:
                            SetSelectableCard(card);
                            break;
                        case CardTypeEnum.WILD_DRAW:
                            wildDrawCards.Add((WildDrawCard)card);
                            break;
                    }
                }
                else if (lastDiscardedCard.CardTypeEnum == card.CardTypeEnum)
                {
                    switch (card.CardTypeEnum)
                    {
                        case CardTypeEnum.NORMAL:
                            NormalCard normalCard = (NormalCard)card;
                            NormalCard discaredNormalCard = (NormalCard)lastDiscardedCard;
                            if (discaredNormalCard.FaceValue == normalCard.FaceValue)
                                SetSelectableCard(card);
                            break;
                        case CardTypeEnum.DRAW:
                            SetSelectableCard(card);
                            break;
                        case CardTypeEnum.REVERSE:
                            SetSelectableCard(card);
                            break;
                        case CardTypeEnum.SKIP:
                            SetSelectableCard(card);
                            break;
                    }
                }
            }
        }

        if (wildDrawCards.Count > 0 && SelectableCards.Count == 0)
        {
            foreach (var wildDrawCard in wildDrawCards)
                SetSelectableCard(wildDrawCard);
        }
    }

    private void SetSelectableCard(Card card)
    {
        card.IsSelectable = true;
        SelectableCards.Add(card);
    }

    public IEnumerator ArrangeTheCards()
    {
        yield return null;

        foreach (var card in Cards)
        {
            card.GetComponent<Transform>().localRotation = Quaternion.identity;
            card.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (Cards.Count >= 10)
        {
            float scaleFactor = (float)10 / Cards.Count;

            scaleFactor = Mathf.Clamp(scaleFactor, 0.5f, 0.7f);
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

            if (i != 0)
                Cards[i].GetCardSpriteRenderer().sortingOrder = Cards[i - 1].GetCardSpriteRenderer().sortingOrder + 2;
            else
                Cards[i].GetCardSpriteRenderer().sortingOrder = 2;

            Cards[i].GetCardBorderSpriteRenderer().sortingOrder = Cards[i].GetCardSpriteRenderer().sortingOrder - 1;
        }

        if (GetType() == typeof(RealPlayer))
        {
            foreach (var card in Cards)
            {
                card.TurnFront();
            }
        }

        yield return null;
    }
}