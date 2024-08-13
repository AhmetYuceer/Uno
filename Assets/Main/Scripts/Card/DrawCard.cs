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
            realPlayer.IsDrawCard = true;
            realPlayer.DrawCard(DRAW_VALUE);
            TurnManager.NextTurn(player);
        }
    }

    private IEnumerator AnimationDrawCard(Player player, int cardCount)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < cardCount; i++)
        {
            Card card = GameManager.Instance.DeckManager.GetCard();
            card.LookAtCard();
            player.AddCard(card);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(player.ArrangeTheCards());
        }
        yield return null;

        TurnManager.NextTurn(player);
    }
}