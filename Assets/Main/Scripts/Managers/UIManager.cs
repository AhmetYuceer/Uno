using DG.Tweening;
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
    private Button[] buttons = new Button[4];


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        buttons[0] = _redButton;
        buttons[1] = _blueButton;
        buttons[2] = _yellowButton;
        buttons[3] = _greenButton;

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
                _card = card;
                ColorPanelAnimation(true);
            }
        }
    }

    private void ColorPanelAnimation(bool value)
    {
        if (value)
        {
            foreach (var button in buttons)
            {
                button.transform.localScale = Vector3.one;
                button.gameObject.SetActive(true);
            }

            _chooseColorPanel.transform.localScale = Vector3.zero;
            _chooseColorPanel.transform.rotation = Quaternion.Euler(0, 0, 360);
            _chooseColorPanel.SetActive(value);
            
            _chooseColorPanel.transform.DORotate(new Vector3(0, 0, 90), 0.5f);
            _chooseColorPanel.transform.DOScale(Vector3.one, 0.5f);
        }
        else
        {
            _chooseColorPanel.transform.localScale = Vector3.one;
            _chooseColorPanel.transform.rotation = Quaternion.Euler(0, 0, 90);

            _chooseColorPanel.transform.DORotate(new Vector3(0, 0, 360), 0.5f);
            _chooseColorPanel.transform.DOScale(Vector3.zero, 0.5f)
            .OnComplete(() =>
            {
                _chooseColorPanel.SetActive(value);

                foreach (var button in buttons)
                {
                    button.transform.localScale = Vector3.one;
                    button.gameObject.SetActive(true);
                }
            });
        }
    }

    private void SetColor(Card card, int buttonIndex)
    {
        if (card.CardTypeEnum == CardTypeEnum.WILD)
        {
            WildCard wildCard = (WildCard)card;
            wildCard.ChangeColor(_realPlayer, buttonIndex);
        }
        else if(card.CardTypeEnum == CardTypeEnum.WILD_DRAW)
        {
            WildDrawCard wildDrawCard = (WildDrawCard)card;
            wildDrawCard.ChangeColor(_realPlayer, buttonIndex);
        }
        _card = null;
        AnimationSetColor(buttonIndex);
    }

    private void AnimationSetColor(int buttonIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == buttonIndex)
                continue;
            
            buttons[i].gameObject.SetActive(false);
        }
        
        buttons[buttonIndex].transform.DOScale(Vector3.one * 2, 0.5f)
        .OnComplete(() =>
        {
            ColorPanelAnimation(false);
        });
    }
}