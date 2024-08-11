using System.Collections.Generic; 

public static class TurnManager
{
    private static List<Player> _players = new List<Player>();
    public static TurnDirectionEnum TurnDirection { get; private set; } = TurnDirectionEnum.RIGHT;

    public static void StartTurn(Card firstCard)
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

    public static void AddPlayer(Player player)
    {
        _players.Add(player);
    }
 
    public static void ReverseDirection()
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

    public static void NextTurn(Player currentPlayer)
    {
        if (currentPlayer.Cards.Count == 0)
        {
            GameManager.Instance.EndGame(currentPlayer);
            return;
        }

        Player nextPlayer = GetNextPlayerIndex(currentPlayer);
        currentPlayer.MyTurn = false;
        nextPlayer.MyTurn = true;
    }

    public static Player GetNextPlayerIndex(Player currentPlayer)
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