using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public bool IsPlay { get; private set; }
    public static GameManager Instance;
    public List<Player> Players = new List<Player>();

    public TurnManager TurnManager { get; private set; }
    public CardManager CardManager { get; private set; }
    public DeckManager DeckManager { get; private set; }
    public DiscardPile DiscardPile { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CardManager = CardManager.Instance;
        DeckManager = DeckManager.Instance;
        DiscardPile = DiscardPile.Instance;
        TurnManager = TurnManager.Instance;

        CardManager.SetupUnoClassic();
        DeckManager.SetDeck(CardManager.Cards);
        DiscardPile.SetManager(DeckManager.DroppedCardsTranform, CardManager.Cards.Count);

        StartGame();
    }

    private void StartGame()
    {
        IsPlay = true;
        SetPlayers();
        StartCoroutine(DealCards());
    }
    
    public void EndGame(Player wonPlayer)
    {
        IsPlay = false;
        UIManager.Instance.EndGame(wonPlayer);
    }

    #region Game States
    private void SetPlayers()
    {
        foreach (var player in Players)
            TurnManager.AddPlayer(player);
    }

    private IEnumerator DealCards()
    {
        foreach (var player in Players)
        {
            for (int i = 0; i < 7; i++)
            {
                Card card = DeckManager.GetCard();
                player.AddCard(card);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(player.ArrangeTheCards());
        }

        yield return new WaitForSeconds(0.2f);
        
        StartCoroutine(StartTurn());
    }

    private IEnumerator StartTurn()
    {
        Card card = DeckManager.GetCard();

        while (card.CardTypeEnum == CardTypeEnum.WILD || card.CardTypeEnum == CardTypeEnum.WILD_DRAW)
        {
            DeckManager.PutCardBackOfDeck(card);
            card = DeckManager.GetCard();
        }

        StartCoroutine(DiscardPile.DiscardCard(card, null));

        yield return new WaitForSeconds(1f);
        TurnManager.StartTurn(card);
    }
    #endregion
}
