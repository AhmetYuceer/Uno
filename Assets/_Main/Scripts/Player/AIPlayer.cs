using System.Collections;
using UnityEngine;

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
        //AI kart seçer ve Oynar
        yield return new WaitForSeconds(_moveDelay);
    }

    public override void Move(Card card)
    {
        if (!MyTurn)
            return;

        MyTurn = false;
        StartCoroutine(GameManager.Instance.TurnManager.NextPlayer());
    }
}
