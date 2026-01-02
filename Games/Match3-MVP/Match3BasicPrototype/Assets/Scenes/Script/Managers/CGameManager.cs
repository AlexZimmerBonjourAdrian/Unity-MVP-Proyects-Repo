using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Codice.Client.Common.GameUI;
public class CGameManager : MonoBehaviour
{


    #region Debug Variable
    


    #endregion

    public TextMeshProUGUI textUiTest;
    
   public enum GameState
   {
    none,
    GameStart,
    GamePlay,
    GameOver,
    FinishGame
   }

   
 public static CGameManager Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("GameManager");
                return obj.AddComponent<CGameManager>();
            }
            return _inst;

        }
    }
    private static CGameManager _inst;

  private GameState actualState = GameState.GameStart;
 


 public void Awake()
    {
    if(_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _inst = this;
        //InitializeFlags();
    }

    public void chagedText(string changeText)
    {
        textUiTest = FindObjectOfType<TextMeshProUGUI>();
        textUiTest.text = changeText; 
    }
   public void SetState(GameState state)
   {
     actualState = state;
   }
    
   public GameState GetState()
   {
    return actualState;
   }


    private void GameStartState()
    {
        chagedText("State: "+"Game Start");
    }

    private void GamePlayState()
    {
          chagedText("State: "+"Game Play");
        
    }

    private void GameOverState()
    {
        chagedText("State: "+"Game GameOver");
    }

    private void FinishGameState()
    {
        chagedText("State: "+"Game FinishGame");
    }

    //Invoke is a call function with name used a coldown for Execute.

 

   void Update()
   {
    if(actualState == GameState.GameStart)
    {
      Invoke("GameStartState",1f);
    }
    else if(actualState == GameState.GamePlay)
    {
       Invoke("GamePlayState",1f);
    }
    else if(actualState == GameState.GameOver)
    {
         Invoke("GameOverState",1f);
    }
    else if(actualState == GameState.FinishGame)
    {
         Invoke("FinishGameState",1f);
    }
   
   }
}
