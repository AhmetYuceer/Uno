using UnityEngine;

public class WildCard : Card
{
    public override void ApplyAction(Player player)
    {
        if (player.GetType() == typeof(AIPlayer))
        {
            CardColorEnum[] cardColors = { CardColorEnum.YELLOW , CardColorEnum.BLUE, CardColorEnum.RED, CardColorEnum.GREEN};
            int colorIndex = Random.Range(0, cardColors.Length);
            this.CardColor = cardColors[colorIndex];
            TurnManager.NextTurn(player);
        }
        else
        {
            //Kart rengi seçme menüsünü aç       
        }
    }
}