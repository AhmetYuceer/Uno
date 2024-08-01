using UnityEngine;
using System.Collections;

public class AIPlayer : Player
{
    private const float OFFSET = 1f;
    private float _moveDelay = 1f;

    public override bool MyTurn 
    { 
        get => base.MyTurn;
        set
        {
            base.MyTurn = value;

            if (base.MyTurn)
                StartCoroutine(PlayTurn());
        }    
    }

    public override void AddCard(Card card)
    {
        Offset = OFFSET;
        base.AddCard(card);
    }

    public IEnumerator PlayTurn()
    {
        yield return new WaitForSeconds(_moveDelay);
        MyTurn = false;

        if (SelectableCards.Count > 0)
        {
            PlayCard(SelectableCards[0]);
            StartCoroutine(GameManager.Instance.TurnManager.NextPlayer());
        }
    }

    public override void Move(Card card)
    {
        if (!MyTurn)
            return;

        MyTurn = false;
        StartCoroutine(GameManager.Instance.TurnManager.NextPlayer());
    }
}
