using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    private List<Player> _players = new List<Player>();
    public TurnDirectionEnum TurnDirection { get; private set; } = TurnDirectionEnum.RIGHT;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    public void StartTurn(Card firstCard)
    {
        switch (firstCard.CardTypeEnum)
        {
            case CardTypeEnum.NORMAL:
                _players[0].MyTurn = true;
                break;
            case CardTypeEnum.DRAW:
                firstCard.ApplyAction(_players[0]);
                break;
            case CardTypeEnum.REVERSE:
                firstCard.ApplyAction(_players[0]);
                break;
            case CardTypeEnum.SKIP:
                firstCard.ApplyAction(_players[_players.Count - 1]);
                break;
        }
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }
 
    public void ReverseDirection()
    {
        switch (TurnDirection)
        {
            case TurnDirectionEnum.LEFT:
                TurnDirection = TurnDirectionEnum.RIGHT;
                break;
            case TurnDirectionEnum.RIGHT:
                TurnDirection = TurnDirectionEnum.LEFT;
                break;
        }
    }

    public void NextTurn(Player currentPlayer)
    {
        if (GameManager.Instance.IsPlay)
        {
            Player nextPlayer = GetNextPlayerIndex(currentPlayer);
            currentPlayer.MyTurn = false;
            nextPlayer.MyTurn = true;
        }
    }

    public Player GetNextPlayerIndex(Player currentPlayer)
    {
        int playerIndex = _players.IndexOf(currentPlayer);

        switch (TurnDirection)
        {
            case TurnDirectionEnum.RIGHT:
                playerIndex++;
                break;
            case TurnDirectionEnum.LEFT:
                playerIndex--;
                break;
        }

        if (playerIndex > _players.Count - 1)
            playerIndex = 0;
        else if (playerIndex <= -1)
            playerIndex = _players.Count - 1;
        
        return _players[playerIndex];
    }
}