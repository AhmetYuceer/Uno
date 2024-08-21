using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
 
    [Header("Text Effect")]
    [SerializeField] private Image _textEffectPanel;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("End Panel")]
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TextMeshProUGUI _wonPlayerName;
    [SerializeField] private TextMeshProUGUI _wonOrLoss;
    [SerializeField] private Button _restartButton, _exitButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _endPanel.SetActive(false);
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

        _restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

        _exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void EndGame(Player wonPlayer)
    {
        if (wonPlayer.GetType() == typeof(RealPlayer))
        {
            _wonOrLoss.text = "Won!";
            _wonPlayerName.text = "";
        }
        else
        {
            _wonOrLoss.text = "Loss!";
            _wonPlayerName.text = $"Winner : {wonPlayer.name}";
        }

        _endPanel.SetActive(true);
    }

    public void SetEffectText(Card card)
    {
        string text = SetText(card);
        Color color = SetColor(card);
        _text.text = text;
        _textEffectPanel.color = color;
    }

    private string SetText(Card card)
    {
        string text = $"{card.CardName}";

        switch (card.CardTypeEnum)
        { 
            case CardTypeEnum.DRAW:
                DrawCard drawCard = (DrawCard)card;
                text += $" (+{drawCard.DrawValue})";
                break;
            case CardTypeEnum.WILD_DRAW:
                WildDrawCard wildCard = (WildDrawCard)card;
                text += $" (+{wildCard.DrawValue})";
                break;
        }
        return text;
    }

    private Color SetColor(Card card)
    {
        Color color = Color.black;

        switch (card.CardColor)
        {
            case CardColorEnum.BLUE:
                color = Color.blue;
                break;
            case CardColorEnum.RED:
                color = Color.red;
                break;
            case CardColorEnum.YELLOW:
                color = Color.yellow;
                break;
            case CardColorEnum.GREEN:
                color = Color.green;
                break;
            case CardColorEnum.WILD:
                color = Color.cyan;
                break;
        }
        return color;
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
            StartCoroutine(wildCard.ChangeColor(_realPlayer, buttonIndex));
        }
        else if(card.CardTypeEnum == CardTypeEnum.WILD_DRAW)
        {
            WildDrawCard wildDrawCard = (WildDrawCard)card;
            StartCoroutine(wildDrawCard.ChangeColor(_realPlayer, buttonIndex));
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