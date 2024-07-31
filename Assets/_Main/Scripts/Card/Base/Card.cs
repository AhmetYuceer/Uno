using UnityEngine;
public abstract class Card : MonoBehaviour
{
    public bool IsSelectable;

    public CardColorEnum CardColor;
    public CardTypeEnum CardTypeEnum;

    private Sprite _frontSprite;
    private Sprite _backSprite;
    private SpriteRenderer _spriteRenderer;

    public virtual void PlayCard() {}

    public void SetSprite(Sprite frontSprite, Sprite backSprite)
    {
        _frontSprite = frontSprite;
        _backSprite = backSprite;

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        TurnBack();
    }

    public void TurnFront()
    {
        _spriteRenderer.sprite = _frontSprite;
    }

    public void TurnBack()
    {
        _spriteRenderer.sprite = _backSprite;
    }
}