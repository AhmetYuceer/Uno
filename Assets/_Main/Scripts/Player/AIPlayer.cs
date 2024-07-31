using UnityEngine;
using System.Collections;

public class AIPlayer : Player
{
    private float _moveDelay = 1f;

    public override bool MyTurn 
    { 
        get => base.MyTurn;
        set
        {
            base.MyTurn = value;

            if (base.MyTurn)
            {
                StartCoroutine(PlayTurn());
            } 
        }    
    }

    public IEnumerator PlayTurn()
    {
        yield return new WaitForSeconds(_moveDelay);
        //AI kart seçer ve Oynar
    }

    public override void Move(Card card)
    {
        if (!MyTurn)
            return;

        MyTurn = false;
        StartCoroutine(GameManager.Instance.TurnManager.NextPlayer());
    }
}
