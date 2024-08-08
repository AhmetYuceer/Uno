public class ReverseCard : Card
{
    public override void ApplyAction(Player player)
    {
        TurnManager.ReverseDirection();
        TurnManager.NextTurn(player);
    }
}