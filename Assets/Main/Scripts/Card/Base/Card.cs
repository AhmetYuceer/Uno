using UnityEngine;
public abstract class Card : MonoBehaviour
{
    public bool IsSelectable;

    public CardColorEnum CardColor;
    public CardTypeEnum CardTypeEnum;

    private SpriteRenderer _cardSpriteRenderer;
    SpriteRenderer _cardBorderSpriteRenderer;
    private Sprite _frontSprite;
    private Sprite _backSprite;

    [Header("Card Select Settings")]
    private bool _isRayHitting;
    private Vector3 _defaultScale = Vector3.one;
    private int _defaultOrderValue;
    private int _defaultChildOrderValue;
    private int _maxOrderValue = 10000;

    public virtual void PlayCard() {}

    public void LookAtCard()
    {
        if (!_isRayHitting)
        {
            _isRayHitting = true;
            _defaultScale = transform.GetChild(0).localScale;
            
            _defaultOrderValue = _cardSpriteRenderer.sortingOrder;
            _defaultChildOrderValue = _cardBorderSpriteRenderer.sortingOrder;
            _cardSpriteRenderer.sortingOrder = _maxOrderValue;
            _cardBorderSpriteRenderer.sortingOrder = _maxOrderValue - 1;

            transform.GetChild(0).localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    public void StopLookingCard()
    {
        if (_isRayHitting)
        {
            _isRayHitting = false;

            _cardSpriteRenderer.sortingOrder = _defaultOrderValue;
            _cardBorderSpriteRenderer.sortingOrder = _defaultChildOrderValue;
            
            transform.GetChild(0).localScale = _defaultScale;
        }
    }

    public void SetSprite(Sprite frontSprite, Sprite backSprite)
    {
        _frontSprite = frontSprite;
        _backSprite = backSprite;

        if (_cardSpriteRenderer == null)
            _cardSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (_cardBorderSpriteRenderer == null)
            _cardBorderSpriteRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();

        TurnBack();
    }

    public SpriteRenderer GetCardSpriteRenderer()
    {
        return _cardSpriteRenderer;
    }

    public SpriteRenderer GetCardBorderSpriteRenderer()
    {
        return _cardBorderSpriteRenderer;
    }

    public void TurnFront()
    {
        _cardSpriteRenderer.sprite = _frontSprite;
    }

    public void TurnBack()
    {
        _cardSpriteRenderer.sprite = _backSprite;
    }
}