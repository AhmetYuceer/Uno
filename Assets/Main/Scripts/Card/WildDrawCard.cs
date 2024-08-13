using System.Collections;
using UnityEngine;

public class WildDrawCard : Card
{
    private const int DRAW_VALUE = 4;
    private CardColorEnum[] _cardColors = { CardColorEnum.RED, CardColorEnum.BLUE, CardColorEnum.YELLOW, CardColorEnum.GREEN };

    public override void ApplyAction(Player player)
    {
        if (player.GetType() == typeof(AIPlayer))
        {
            int colorIndex = Random.Range(0, _cardColors.Length);
            ChangeColor(player, colorIndex);
        }
        else
        {
            RealPlayer realPlayer = (RealPlayer)player;
            UIManager.Instance.OpenChooseColorPanel(realPlayer, this);
        }
    }

    public void ChangeColor(Player player, int colorIndex)
    {
        this.CardColor = _cardColors[colorIndex];

        Player nextPlayer = TurnManager.GetNextPlayerIndex(player);
        player.MyTurn = false;

        if (nextPlayer.GetType() == typeof(AIPlayer))
        {
            StartCoroutine(AnimationDrawCard(nextPlayer, DRAW_VALUE));
        }
        else
        {
            RealPlayer realPlayer = (RealPlayer)nextPlayer;
            realPlayer.IsWildDraw = true;
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