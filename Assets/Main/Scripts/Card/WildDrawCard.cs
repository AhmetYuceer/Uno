public class WildDrawCard : Card
{
    private const int DRAW_VALUE = 4;
    
    public override void ApplyAction(Player player)
    {
        Player nextPlayer = TurnManager.GetNextPlayerIndex(player);
        player.MyTurn = false;

        if (nextPlayer.GetType() == typeof(AIPlayer))
        {
            for (int i = 0; i < DRAW_VALUE; i++)
            {
                Card card = GameManager.Instance.DeckManager.GetCard();
                nextPlayer.AddCard(card);
            }
            TurnManager.NextTurn(nextPlayer);
        }
        else
        {
            // UI arayüzünü aç
        }
    }

}