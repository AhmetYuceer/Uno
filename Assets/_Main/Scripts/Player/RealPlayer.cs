using UnityEngine;

public class RealPlayer : Player
{
    public override void Move(Card card)
    {
        if (!MyTurn)
            return;

        MyTurn = false;
        StartCoroutine(GameManager.Instance.TurnManager.NextPlayer());
    }
}