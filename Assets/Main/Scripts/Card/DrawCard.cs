using System.Collections;
using UnityEngine;

public class DrawCard : Card
{
    public int DrawValue = 2;

    public override void ApplyAction(Player player)
    {
        base.ApplyAction(player);
    }

    public override IEnumerator Action(Player player)
    {
        UIManager.Instance.SetEffectText(this);
        yield return new WaitForSeconds(0.5f);

        Player nextPlayer = GameManager.Instance.TurnManager.GetNextPlayerIndex(player);
        player.MyTurn = false;

        if (nextPlayer.GetType() == typeof(AIPlayer))
        {
            StartCoroutine(AnimationDrawCard(nextPlayer, DrawValue));
        }
        else
        {
            RealPlayer realPlayer = (RealPlayer)nextPlayer;
            realPlayer.DrawCard(DrawValue, this.CardTypeEnum);
            GameManager.Instance.TurnManager.NextTurn(player);
        }

        yield return null;
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

        GameManager.Instance.TurnManager.NextTurn(player);
    }
}