using UnityEngine;
using System.Collections;

public class AIPlayer : Player
{
    private bool _isDrawn;
    private const float OFFSET = 1f;
    private float _moveDelay = 2f;

    public override bool MyTurn 
    { 
        set
        {
            base.MyTurn = value;
            
            if (MyTurn)
            {
                if (IsDraw)
                {
                    IsDraw = false;
                }
                else if (IsSkip)
                {
                    IsSkip = false;
                }
                else
                {
                    StartCoroutine(PlayTurn());
                }
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
            DrawCard();
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

    public override void DrawCard()
    {
        Card card = GameManager.Instance.DeckManager.GetCard();
        AddCard(card);
        PlayerAction();
    }
}