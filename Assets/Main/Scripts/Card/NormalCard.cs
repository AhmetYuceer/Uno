public class NormalCard : Card
{
    public CardFaceValueEnum FaceValue;

    public override void ApplyAction(Player player)
    {
        TurnManager.NextTurn(player);
    }
}