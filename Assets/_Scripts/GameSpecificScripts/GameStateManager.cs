using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    public List<GameState> gameStates;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InvokeLevelStart()
    {

        foreach (var gameState in gameStates)
        {
            gameState.OnLevelStart?.Invoke();
        }
    }

    public void InvokeLevelEnd()
    {
        foreach (var gameState in gameStates)
        {
            gameState.OnLevelEnd?.Invoke();
        }
    }
}
