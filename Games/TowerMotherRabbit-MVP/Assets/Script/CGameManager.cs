using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour
{
    enum GameStates
    {
        None,
        Start,
        Play,
        GameOver,

    }
    [HideInInspector]private GameStates _gameState;
    [HideInInspector]private GameStates _prevGameState;


    public static CGameManager Instance { get; private set; } // Singleton instance
    private void Awake()
    {
        // Singleton initialization
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        
    }
    private void SetState(GameStates state)
    {
        _prevGameState = _gameState;
        _gameState = state;
    }

    private void SetprevState()
    {
        _gameState = _prevGameState;
    }

}
    
