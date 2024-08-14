using System.Collections;
using UnityEngine;

public class DrawCard : Card
{
    private const int DRAW_VALUE = 2;

    public override void ApplyAction(Player player)
    {
        Player nextPlayer = TurnManager.GetNextPlayerIndex(player);
        player.MyTurn = false;

        if (nextPlayer.GetType() == typeof(AIPlayer))
        {
            StartCoroutine(AnimationDrawCard(nextPlayer, DRAW_VALUE));
        }
        else
        {
            RealPlayer realPlayer = (RealPlayer)nextPlayer;
            realPlayer.DrawCard(DRAW_VALUE, this.CardTypeEnum);
            TurnManager.NextTurn(player);
        }
    }

    private IEnumerator AnimationDrawCard(Player player, int cardCount)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < cardCount; i++)
        {
            Card card = GameManager.Instance.DeckManager.GetCard();
            player.AddCard(card);
            card.SetMaxOrder();
            yield return new WaitForSeconds(0.5f);
            card.SetDefauldOrder();
            StartCoroutine(player.ArrangeTheCards());
        }
        yield return null;

        TurnManager.NextTurn(player);
    }
}