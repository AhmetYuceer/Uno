using UnityEngine; 
using System.Collections;

public class RealPlayer : Player
{
    private const float OFFSET = 0.7f;

    public bool IsDraw;
    private int _drawCardCount = 0;

    [SerializeField] private LayerMask _deckLayer;
    private float _distance = 100f;

    public override bool MyTurn 
    { 
        get => base.MyTurn;
        set 
        {
            base.MyTurn = value;

            if (MyTurn && !IsDraw)
            {
                SetSelectableCards();
                StartCoroutine(ShowSelectableCards());
            }
        }
    }

    private RaycastHit2D CastRay(LayerMask layerMask)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rayOrigin = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Camera.main.transform.forward, _distance, layerMask);
        return hit;
    }

    public override void DrawCard(int cardCount)
    {
        IsDraw = true;
        _drawCardCount = cardCount;
    }

    private IEnumerator ShowSelectableCards()
    {
        yield return null;
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
}