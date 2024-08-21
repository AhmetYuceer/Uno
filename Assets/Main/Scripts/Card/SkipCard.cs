using System.Collections;
using UnityEngine;

public class SkipCard : Card
{
    public override void ApplyAction(Player player)
    {
        base.ApplyAction(player);
    }

    public override IEnumerator Action(Player player)
    {
        UIManager.Instance.SetEffectText(this);
        yield return new WaitForSeconds(0.5f);

        player.MyTurn = false;
        Player nextPlayer = GameManager.Instance.TurnManager.GetNextPlayerIndex(player);
        GameManager.Instance.TurnManager.NextTurn(nextPlayer);
    }
}