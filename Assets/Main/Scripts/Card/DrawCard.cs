public class DrawCard : Card
{
    private const int DRAW_VALUE = 2;

    public override void ApplyAction(Player player)
    {
        Player nextPlayer = TurnManager.GetNextPlayerIndex(player);
        nextPlayer.IsDraw = true;

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