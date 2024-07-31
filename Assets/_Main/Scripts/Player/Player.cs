using UnityEngine;
using System.Collections.Generic;

public abstract class Player : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();
    public Transform CardsParentTransform;
    private SpriteRenderer _spriteRenderer;
    private bool _isMyTurn = false;

    public virtual bool MyTurn
    {
        get
        {
            return  _isMyTurn;
        }
        set
        {
            _isMyTurn = value;

            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_isMyTurn)
            {
                _spriteRenderer.color = Color.green;
            }
            else
            {
                _spriteRenderer.color = Color.white;
            }
        }
    }
    
    public virtual void Move(Card card) { }

    public void AddCard(Card card)
    {
        card.gameObject.transform.SetParent(CardsParentTransform);
        Cards.Add(card);
        ArrangeTheCards();
    }

    public void DropCard(Card card)
    {
        Cards.Remove(card);
        ArrangeTheCards();
    }

    private void SetSelectableCards()
    {
        //Seçilebilir kartlarý belirle
    }
    
    private void ArrangeTheCards()
    {
        foreach (var card in Cards)
        {
            card.GetComponent<Transform>().rotation = new Quaternion(0,0,0,0); 
            card.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (Cards.Count > 7)
        {
            float scaleFactor = (float)7 / Cards.Count;
            CardsParentTransform.transform.localScale = new Vector3(scaleFactor, scaleFactor, CardsParentTransform.transform.localScale.z);
        }

        float totalWidth = 0;
        foreach (var card in Cards)
        {
            totalWidth += card.GetComponent<Transform>().localScale.x;
        }

        float startX = CardsParentTransform.localPosition.x - totalWidth / 2;
        float currentX = startX;

        for (int i = 0; i < Cards.Count; i++)
        {
            Vector3 cardSize = Cards[i].GetComponent<Transform>().localScale;
            Cards[i].transform.localPosition = new Vector3((currentX + cardSize.x / 2) + (CardsParentTransform.transform.localPosition.x * -1), 0, 0);
            currentX += cardSize.x;
            
            if (i!= 0)
                Cards[i].GetComponent<SpriteRenderer>().sortingOrder = Cards[i-1].GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }
} 