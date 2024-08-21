using System.Collections;
using UnityEngine;

public class NormalCard : Card
{
    public CardFaceValueEnum FaceValue;

    public override void ApplyAction(Player player)
    {
        base.ApplyAction(player);
    }

    public override IEnumerator Action(Player player)
    {
        UIManager.Instance.SetEffectText(this);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.TurnManager.NextTurn(player);
    }
}