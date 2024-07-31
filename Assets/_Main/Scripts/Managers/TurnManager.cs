using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    private List<Player> _players = new List<Player>();
    private TurnDirectionEnum _turnDirection;
    private int _nextPlayerIndex;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartTurn()
    {
        _nextPlayerIndex = 0;
        _turnDirection = TurnDirectionEnum.RIGHT;
        _players[_nextPlayerIndex].MyTurn = true;
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }

    public void ReverseTurn()
    {
        switch (_turnDirection)
        {
            case TurnDirectionEnum.RIGHT:
                _turnDirection = TurnDirectionEnum.LEFT;
                break;
            case TurnDirectionEnum.LEFT:
                _turnDirection = TurnDirectionEnum.RIGHT;
                break;
        }

        _players[_nextPlayerIndex].MyTurn = false;
        StartCoroutine(NextPlayer());
    }

    public IEnumerator NextPlayer()
    {
        yield return null;

        switch (_turnDirection)
        {
            case TurnDirectionEnum.RIGHT:
                _nextPlayerIndex++;
                break;
            case TurnDirectionEnum.LEFT:
                _nextPlayerIndex--;
                break;
        }

        if (_nextPlayerIndex > _players.Count -1)
        {
            _nextPlayerIndex = 0;
        }
        else if (_nextPlayerIndex <= -1)
        {
            _nextPlayerIndex = _players.Count - 1;
        }

        _players[_nextPlayerIndex].MyTurn = true;
    }
}
