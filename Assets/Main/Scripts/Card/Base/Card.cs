using UnityEngine;
using DG.Tweening;

public abstract class Card : MonoBehaviour
{
    public bool IsSelectable;
    public bool IsShowable;

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
    private Vector3 _lookedScaleSize = new Vector3(1.5f, 1.5f, 1.5f);

    [Header("Drag And Drop")]
    private Vector3 _previosPosition;

    public virtual void ApplyAction(Player player) { }

    private void OnMouseDown()
    {
        _previosPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (_isRayHitting && IsSelectable && IsShowable)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(pos.x, pos.y);
        }
    }

    private void OnMouseUp()
    {
        transform.position = _previosPosition;
        StopLookingCard();
    }
  
    public void LookAtCard()
    {
        if (!_isRayHitting && IsShowable)
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