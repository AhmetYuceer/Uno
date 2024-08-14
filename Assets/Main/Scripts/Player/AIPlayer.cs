using UnityEngine;
using System.Collections;

public class AIPlayer : Player
{
    private const float OFFSET = 1f;
    private float _moveDelay = 1.5f;

    public override bool MyTurn
    {
        set
        {
            base.MyTurn = value;

            if (MyTurn)
            {
                StartCoroutine(PlayTurn());
            }
        }
    }

    public IEnumerator PlayTurn()
    {
        yield return new WaitForSeconds(_moveDelay);
        PlayerAction();
    }

    public void PlayerAction()
    {
        SetSelectableCards();

        if (SelectableCards.Count > 0)
        {
            int rndIndex = Random.Range(0, SelectableCards.Count);
            Card card = SelectableCards[rndIndex];
            DiscardCard(card);
            IsDraw = false;
        }
        else if (!IsDraw)
        {
            IsDraw = true;
            DrawCard(1, CardTypeEnum.DRAW);
        }
        else
        {
            IsDraw = false;
            TurnManager.NextTurn(this);
        }
    }

    public override void AddCard(Card card)
    {
        Offset = OFFSET;
        base.AddCard(card);
    }

    public override void DrawCard(int cardCount, CardTypeEnum cardType)
    {
        StartCoroutine(AnimationDrawCard(cardCount));
    }

    private IEnumerator AnimationDrawCard(int cardCount)
    {
        for (int i = 0; i < cardCount; i++)
        {
            Card card = GameManager.Instance.DeckManager.GetCard();
            AddCard(card);
            card.SetMaxOrder();
            yield return new WaitForSeconds(0.5f);
            card.SetDefauldOrder();
            StartCoroutine(ArrangeTheCards());
        }

        yield return new WaitForSeconds(0.2f);
        PlayerAction();
    }
}