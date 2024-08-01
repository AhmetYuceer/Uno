using UnityEngine; 
using System.Collections;

public static class CardViewer
{
    private static Card _lastHitCard;
    private static LayerMask _cardLayer = LayerMask.GetMask("Card");
 
    public static void ViewCard()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rayOrigin = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Camera.main.transform.forward, 100f, _cardLayer);

        if (hits.Length > 0)
        {
            RaycastHit2D highestOrderHit = hits[0];
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out Card card))
                {
                    SpriteRenderer renderer = hit.collider.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    if (renderer != null && renderer.sortingOrder > highestOrderHit.collider.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder)
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
            _lastHitCard.StopLookingCard();
            _lastHitCard = null;
        }
    }
}
