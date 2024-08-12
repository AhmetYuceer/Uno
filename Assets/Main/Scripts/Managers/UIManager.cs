using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    private Card _card;
    private RealPlayer _realPlayer;

    [Header("Choose Color For Wild Card ")]
    [SerializeField] private GameObject _chooseColorPanel;
    [SerializeField] private Button _redButton, _blueButton, _yellowButton, _greenButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _redButton.onClick.AddListener(() =>
        {
            int colorIndex = 0;
            SetColor(_card, colorIndex);
        });

        _blueButton.onClick.AddListener(() =>
        {
            int colorIndex = 1; 
            SetColor(_card, colorIndex);
        });

        _yellowButton.onClick.AddListener(() =>
        {
            int colorIndex = 2;
            SetColor(_card, colorIndex);
        });

        _greenButton.onClick.AddListener(() =>
        {
            int colorIndex = 3;
            SetColor(_card, colorIndex);
        });
    }

    public void OpenChooseColorPanel(RealPlayer player, Card card)
    {
        if (card != null || player != null)
        {
            if (card.CardTypeEnum == CardTypeEnum.WILD || card.CardTypeEnum == CardTypeEnum.WILD_DRAW)
            {
                _realPlayer = player;
                _chooseColorPanel.SetActive(true);
                _card = card;
            }
        }
    }
 
    private void SetColor(Card card , int colorIndex)
    {
        if (card.CardTypeEnum == CardTypeEnum.WILD)
        {
            WildCard wildCard = (WildCard)card;
            wildCard.ChangeColor(_realPlayer, colorIndex);
        }
        else if(card.CardTypeEnum == CardTypeEnum.WILD_DRAW)
        {
            WildDrawCard wildDrawCard = (WildDrawCard)card;
            wildDrawCard.ChangeColor(_realPlayer, colorIndex);
        }

        _card = null;
        _chooseColorPanel.SetActive(false);
    }
}