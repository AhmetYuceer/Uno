﻿using UnityEngine; 
using System.Collections;

public class RealPlayer : Player
{
    private const float OFFSET = 0.7f;
    
    [SerializeField] private LayerMask _cardLayer;

    public Card LastHitCard;
    private float _distance = 100f;

    public bool IsDraw;
    private int _drawCardCount = 0; 

    public override bool MyTurn 
    { 
        get => base.MyTurn;
        set 
        {
            base.MyTurn = value;

            if (MyTurn && !IsDraw)
            {
                SetSelectableCards();
                ShowSelectableCards();

                if (SelectableCards.Count < 1)
                {
                    IsDraw = true;
                    _drawCardCount = 1;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            CastRay();
    }

    private void CastRay()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rayOrigin = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Camera.main.transform.forward, 100f);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Deck") && IsDraw && MyTurn)
            {
                Debug.Log("Deck");
                StartCoroutine(AnimationDrawCard(_drawCardCount));
            }
            else if(hit.collider.CompareTag("DiscardArea") && MyTurn && LastHitCard != null)
            {
                Debug.Log("Drop");
                if (LastHitCard.IsSelectable)
                    DiscardCard(LastHitCard);
            }
        }
        ViewCard();
    }

    public override void DrawCard(int cardCount)
    {
        IsDraw = true;
       _drawCardCount = cardCount;
    }

    private IEnumerator AnimationDrawCard(int cardCount)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < cardCount; i++)
        {
            Card card = GameManager.Instance.DeckManager.GetCard();
            card.LookAtCard();
            AddCard(card);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ArrangeTheCards());
        }
        yield return new WaitForSeconds(0.2f);
        
        IsDraw = false;
        SetSelectableCards();

        if (SelectableCards.Count > 0)
            ShowSelectableCards();
        else
            TurnManager.NextTurn(this);
    }

    private void ShowSelectableCards()
    {
        foreach (var card in Cards)
        {
            if (card.IsSelectable)
                card.transform.localPosition = new Vector3(card.transform.localPosition.x, card.transform.localPosition.y + 1, card.transform.localPosition.z);
        }
    }

    public override void AddCard(Card card)
    {
        Offset = OFFSET;
        card.IsShowable = true;
        base.AddCard(card);
    }

    private void ViewCard()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rayOrigin = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Camera.main.transform.forward, _distance, _cardLayer);

        if (hits.Length > 0)
        {
            RaycastHit2D highestOrderHit = hits[0];

            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out Card card))
                {
                    Card highestCard = highestOrderHit.collider.GetComponent<Card>();

                    if (card.GetCardSpriteRenderer().sortingOrder > highestCard.GetCardSpriteRenderer().sortingOrder)
                    {
                        highestOrderHit = hit;
                    }
                }
            }

            if (highestOrderHit.collider != null && highestOrderHit.collider.gameObject.TryGetComponent(out Card highestOrderCard))
            {
                if (LastHitCard == null)
                {
                    LastHitCard = highestOrderCard;
                    LastHitCard.LookAtCard();
                }
                else if (LastHitCard == highestOrderCard)
                {
                    LastHitCard.LookAtCard();
                }
                else
                {
                    LastHitCard.StopLookingCard();
                    LastHitCard = highestOrderCard;
                    LastHitCard.LookAtCard();
                }
            }
        }
        else if (LastHitCard != null)
        {
            StopShowingCard();
        }
    }

    private void StopShowingCard()
    {
        if (LastHitCard != null)
        {
            LastHitCard.StopLookingCard();
            LastHitCard = null;
        }
    }
}