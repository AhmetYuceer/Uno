using System.Collections;
using UnityEngine;

public class WildDrawCard : Card
{
    public int DrawValue = 4;
    private CardColorEnum[] _cardColors = { CardColorEnum.RED, CardColorEnum.BLUE, CardColorEnum.YELLOW, CardColorEnum.GREEN };

    public override void ApplyAction(Player player)
    {
        base.ApplyAction(player);   
    }

    public override IEnumerator Action(Player player)
    {
        if (player.GetType() == typeof(AIPlayer))
        {
            int colorIndex = Random.Range(0, _cardColors.Length);
            StartCoroutine(ChangeColor(player, colorIndex));
        }
        else
        {
            RealPlayer realPlayer = (RealPlayer)player;
            UIManager.Instance.OpenChooseColorPanel(realPlayer, this);
        }

        yield return null;
    }


    public IEnumerator ChangeColor(Player player, int colorIndex)
    {
        this.CardColor = _cardColors[colorIndex];

        Player nextPlayer = GameManager.Instance.TurnManager.GetNextPlayerIndex(player);
        player.MyTurn = false;

        if (nextPlayer.GetType() == typeof(AIPlayer))
        {
            UIManager.Instance.SetEffectText(this);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AnimationDrawCard(nextPlayer, DrawValue));
        }
        else
        {
            RealPlayer realPlayer = (RealPlayer)nextPlayer;
            realPlayer.DrawCard(DrawValue, this.CardTypeEnum);

            UIManager.Instance.SetEffectText(this);
            yield return new WaitForSeconds(0.5f);

            GameManager.Instance.TurnManager.NextTurn(player);
        }
    }

    private IEnumerator AnimationDrawCard(Player player, int cardCount)
    {
        for (int i = 0; i < cardCount; i++)
        {
            Card card = GameManager.Instance.DeckManager.GetCard();
            player.AddCard(card);
            card.SetMaxOrder();
            yield return new WaitForSeconds(0.5f);
            card.SetDefauldOrder();
            StartCoroutine(player.ArrangeTheCards());
        }
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.TurnManager.NextTurn(player);
    }
}