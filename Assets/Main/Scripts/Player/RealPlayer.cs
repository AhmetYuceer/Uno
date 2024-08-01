using UnityEngine; 

public class RealPlayer : Player
{
    private const float OFFSET = 0.7f;

    public override bool MyTurn 
    { 
        get => base.MyTurn;
        set 
        {
            base.MyTurn = value;

        }
    }

    public override void AddCard(Card card)
    {
        Offset = OFFSET;
        card.TurnFront();
        base.AddCard(card);
    }

    public override void Move(Card card)
    {
        if (!MyTurn)
            return;

        MyTurn = false;
        StartCoroutine(GameManager.Instance.TurnManager.NextPlayer());
    }
}