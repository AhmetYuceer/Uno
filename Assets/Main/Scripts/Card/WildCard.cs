using UnityEngine;

public class WildCard : Card
{
    private CardColorEnum[] _cardColors = { CardColorEnum.RED, CardColorEnum.BLUE, CardColorEnum.YELLOW, CardColorEnum.GREEN };

    public override void ApplyAction(Player player)
    {
        if (player.GetType() == typeof(AIPlayer))
        {
            int colorIndex = Random.Range(0, _cardColors.Length);
            ChangeColor(player, colorIndex);
        }
        else
        {
            RealPlayer realPlayer = (RealPlayer)player;
            UIManager.Instance.OpenChooseColorPanel(realPlayer, this);
        }
    }

    public void ChangeColor(Player player , int colorIndex)
    {
        this.CardColor = _cardColors[colorIndex];
        TurnManager.NextTurn(player);
    }
}