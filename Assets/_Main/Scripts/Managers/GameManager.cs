using UnityEngine; 
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Stack<Card> DroppedCards = new Stack<Card>();
    public List<Player> Players = new List<Player>();

    public CardManager CardManager { get; private set; }
    public DeckManager DeckManager { get; private set; }
    public TurnManager TurnManager { get; private set; }

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
        TurnManager = TurnManager.Instance;
     
        CardManager.SetupUnoClassic();
        DeckManager.SetDeck(CardManager.Cards);

        StartGame();
    }

    private void StartGame()
    {
        foreach (var player in Players)
            TurnManager.AddPlayer(player);

        foreach (var player in Players)
        {
            for (int i = 0; i < 7; i++)
            {
                Card card = DeckManager.GetCard();
                player.AddCard(card);
            }
        }

        TurnManager.StartTurn();
    }
}