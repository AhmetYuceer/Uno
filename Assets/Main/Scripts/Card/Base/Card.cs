using UnityEngine;
using DG.Tweening;

public abstract class Card : MonoBehaviour
{
    public bool IsSelectable;
    public bool IsShowable;
    public bool IsDiscarded;

    public CardColorEnum CardColor;
    public CardTypeEnum CardTypeEnum;

    private SpriteRenderer _cardSpriteRenderer;
    private SpriteRenderer _cardBorderSpriteRenderer;
    private Sprite _frontSprite;
    private Sprite _backSprite;

    [Header("Card Look at Settings")]
    private bool _isRayHitting;
    private int _defaultOrderValue;
    private int _defaultChildOrderValue;
    private int _maxOrderValue = 10000;
    private Vector3 _defaultScale = Vector3.one;
    private Vector3 _lookedScaleSize = new Vector3(1.3f, 1.3f, 1.3f);

    public virtual void ApplyAction(Player player) { }

    public void SetOrder(int cardOrder)
    {
        _defaultOrderValue = cardOrder;
        _defaultChildOrderValue = cardOrder - 1;
        _cardSpriteRenderer.sortingOrder = _defaultOrderValue;
        _cardBorderSpriteRenderer.sortingOrder = _defaultChildOrderValue;
    }

    public void LookAtCard()
    {
        if (!_isRayHitting && IsShowable && !IsDiscarded)
        {
            _isRayHitting = true;
            _defaultScale = transform.GetChild(0).localScale;

            _defaultOrderValue = _cardSpriteRenderer.sortingOrder;
            _defaultChildOrderValue = _cardBorderSpriteRenderer.sortingOrder;

            _cardSpriteRenderer.sortingOrder = _maxOrderValue;
            _cardBorderSpriteRenderer.sortingOrder = _maxOrderValue - 1;

            LookAnimation(transform.GetChild(0).transform, _lookedScaleSize);
        }
    }

    public void StopLookingCard()
    {
        if (_isRayHitting)
        {
            _isRayHitting = false;

            _cardSpriteRenderer.sortingOrder = _defaultOrderValue;
            _cardBorderSpriteRenderer.sortingOrder = _defaultChildOrderValue;

            LookAnimation(transform.GetChild(0).transform, _defaultScale);
        }
    }

    private void LookAnimation(Transform childTransform, Vector3 size)
    {
        childTransform.DOScale(size, 0.5f);
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