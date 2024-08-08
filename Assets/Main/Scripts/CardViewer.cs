using UnityEngine;

public static class CardViewer
{
    private static Card _lastHitCard;
    private static LayerMask _cardLayer = LayerMask.GetMask("Card");
    private static float _distance = 100f;

    public static void ViewCard()
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

    public static void StopShowingCard()
    {
        if (_lastHitCard != null)
        {
            _lastHitCard.StopLookingCard();
            _lastHitCard = null;
        }
    }
}