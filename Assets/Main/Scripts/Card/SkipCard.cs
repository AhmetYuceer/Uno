public class SkipCard : Card
{
    public override void ApplyAction(Player player)
    {
        player.MyTurn = false;
        Player nextPlayer = TurnManager.GetNextPlayerIndex(player);
        TurnManager.NextTurn(nextPlayer);
    }
}