using System.Collections;
using UnityEngine;

public class WildCard : Card
{
    private CardColorEnum[] _cardColors = { CardColorEnum.RED, CardColorEnum.BLUE, CardColorEnum.YELLOW, CardColorEnum.GREEN };

    public override void ApplyAction(Player player)
    {
        base.ApplyAction(player);
    }

    public override IEnumerator Action(Player player)
    {
        yield return null;

        if (player.GetType() == typeof(AIPlayer))
        {
            int colorIndex = Random.Range(0, _cardColors.Length);
            StartCoroutine(ChangeColor(player, colorIndex));
        }
        else
        {
            RealPlayer realPlayer = (RealPlayer)player;
            UIManager.Instance.OpenChooseColorPanel(realPlayer, this);
        }
    }

    public IEnumerator ChangeColor(Player player , int colorIndex)
    {
        this.CardColor = _cardColors[colorIndex];

        UIManager.Instance.SetEffectText(this);
        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.TurnManager.NextTurn(player);
    }
}