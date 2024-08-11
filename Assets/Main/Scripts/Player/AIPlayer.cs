using UnityEngine;
using System.Collections;

public class AIPlayer : Player
{
    private bool _isDrawn = false;
    private const float OFFSET = 1f;
    private float _moveDelay = 1f;

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
        CardViewer.StopShowingCard();
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
            Cards.Remove(card);
            DiscardCard(card);
        }
        else if (!_isDrawn)
        {
            _isDrawn = true;
            DrawCard(1);
        }
        else
        {
            _isDrawn = false;
            TurnManager.NextTurn(this);
        }
    }

    public override void AddCard(Card card)
    {
        Offset = OFFSET;
        base.AddCard(card);
    }

    public override void DiscardCard(Card card)
    {
        StartCoroutine(GameManager.Instance.DiscardPile.DiscardCard(card, this));
    }

    public override void DrawCard(int cardCount)
    {
        StartCoroutine(AnimationDrawCard(cardCount));
    }

    private IEnumerator AnimationDrawCard(int cardCount)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < cardCount; i++)
        {
            Card card = GameManager.Instance.DeckManager.GetCard();
            card.LookAtCard();
            AddCard(card);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ArrangeTheCards());
        }

        yield return new WaitForSeconds(0.2f);
        PlayerAction();
    }
}