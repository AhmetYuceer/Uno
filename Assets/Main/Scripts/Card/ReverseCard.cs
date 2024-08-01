public class ReverseCard : Card
{
    public override void PlayCard()
    {
        GameManager.Instance.TurnManager.ReverseTurn();
    }
}