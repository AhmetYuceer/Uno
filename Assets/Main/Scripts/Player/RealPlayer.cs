using UnityEngine; 
using System.Collections;

public class RealPlayer : Player
{
    private const float OFFSET = 0.7f;

    [SerializeField] private LayerMask _cardLayer;

    private Card _lastHitCard;
    private float _distance = 100f;

    private int _drawCardCount = 0; 

    public override bool MyTurn 
    { 
        get => base.MyTurn;
        set 
        {
            base.MyTurn = value;

            if (MyTurn && !IsDrawCard && !IsWildDraw)
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
            if (hit.collider.CompareTag("Deck") && MyTurn && (IsDraw || IsDrawCard || IsWildDraw))
            {
                StartCoroutine(AnimationDrawCard(_drawCardCount));
                return;
            }
            if(hit.collider.CompareTag("DiscardArea") && MyTurn && _lastHitCard != null)
            {
                if (_lastHitCard.IsSelectable)
                    DiscardCard(_lastHitCard);
                return;
            }
        }
        ViewCard();
    }

    public override void DrawCard(int cardCount , CardTypeEnum cardType)
    {
        _drawCardCount = cardCount;

        if (cardType == CardTypeEnum.DRAW)
            IsDrawCard = true;
        else if (cardType == CardTypeEnum.WILD_DRAW)
            IsWildDraw = true;
    }

    private IEnumerator AnimationDrawCard(int cardCount)
    {
        for (int i = 0; i < cardCount; i++)
        {
            Card card = GameManager.Instance.DeckManager.GetCard();
            AddCard(card);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ArrangeTheCards());
        }
        yield return new WaitForSeconds(0.2f);
         
        if (IsDrawCard || IsWildDraw)
        {
            IsDrawCard = false;
            IsWildDraw = false;
            GameManager.Instance.TurnManager.NextTurn(this);
        }
        else
        {
            IsDraw = false;
            SetSelectableCards();
            if (SelectableCards.Count > 0)
                ShowSelectableCards();
            else
                GameManager.Instance.TurnManager.NextTurn(this);
        }
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
                if (_lastHitCard == null)
                {
                    _lastHitCard = highestOrderCard;
                    _lastHitCard.LookAtCard();
                }
                else if (_lastHitCard == highestOrderCard)
                {
                    _lastHitCard.LookAtCard();
                }
                else
                {
                    _lastHitCard.StopLookingCard();
                    _lastHitCard = highestOrderCard;
                    _lastHitCard.LookAtCard();
                }
            }
        }
        else if (_lastHitCard != null)
        {
            StopShowingCard();
        }
    }

    private void StopShowingCard()
    {
        if (_lastHitCard != null)
        {
            _lastHitCard.StopLookingCard();
            _lastHitCard = null;
        }
    }
}