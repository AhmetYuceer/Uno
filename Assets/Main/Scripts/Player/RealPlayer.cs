using System.Collections;
using UnityEngine; 

public class RealPlayer : Player
{
    private const float OFFSET = 0.7f;

    public override bool MyTurn 
    { 
        get => base.MyTurn;
        set 
        {
            base.MyTurn = value;

            if (MyTurn)
            {
                SetSelectableCards();
                StartCoroutine(ShowSelectableCards());
            }
        }
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