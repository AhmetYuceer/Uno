using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private LayerMask _cardLayer;
    [SerializeField] private LayerMask _cardDropAreaLayer;
    [SerializeField] private LayerMask _deckLayer;

    private float _distance = 100f;
    private RealPlayer _player;
    private Card _lastHitCard;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);   
    }

    private void Start()
    {
        _player = GetComponent<RealPlayer>();
    }

    public void CastRay()
    {
        if (_player.IsDraw)
        {
            DrawCard();
        }

        if (DiscardCard())
            return;

        ViewCard();
    }

    private bool DrawCard()
    {
        RaycastHit2D hit = CastRay(_deckLayer);

        if (hit.collider != null)
        {
            
        }

        return false;
    }

    private RaycastHit2D CastRay(LayerMask layerMask)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rayOrigin = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Camera.main.transform.forward, _distance, layerMask);
        return hit;
    }

    private bool DiscardCard()
    {
        if (_player.MyTurn)
        {
            RaycastHit2D hit = CastRay(_cardDropAreaLayer);
            if (hit.collider != null && _lastHitCard != null && _lastHitCard.IsSelectable)
            {
                _player.DiscardCard(_lastHitCard);
                return true;
            }
        }
        return false;
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

    public void StopShowingCard()
    {
        if (_lastHitCard != null)
        {
            _lastHitCard.StopLookingCard();
            _lastHitCard = null;
        }
    }
}