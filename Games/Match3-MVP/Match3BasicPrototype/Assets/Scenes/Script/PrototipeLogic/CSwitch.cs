using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CSwitch : MonoBehaviour
{
      private enum DebugStatesGame
    {
        None,
        GameStates,

        InterativeStates,
        
    }

    [SerializeField]
    DebugStatesGame DebugState = DebugStatesGame.None;
    public void Update()
    {
        switch((int)DebugState)
        {
            case (int)DebugStatesGame.GameStates:
                DebugLevelStates();
            break;
            case (int)DebugStatesGame.InterativeStates:
                DebugInterativeStates();
            break;
        }
    }

    private void DebugLevelStates()
    {
         if(CGameManager.Inst.GetState() != CGameManager.GameState.GameOver)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                CGameManager.Inst.SetState(CGameManager.GameState.GameStart);
            }

            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                    CGameManager.Inst.SetState(CGameManager.GameState.GamePlay);
            }

            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                CGameManager.Inst.SetState(CGameManager.GameState.GameOver);
            }

            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                CGameManager.Inst.SetState(CGameManager.GameState.FinishGame);
            }
        }
    
        else if(Input.GetKeyDown(KeyCode.R) && CGameManager.Inst.GetState() == CGameManager.GameState.GameOver)
        {
            CGameManager.Inst.SetState(CGameManager.GameState.GameStart);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

     private void DebugLevelButtonStates()
    {
         if(CGameManager.Inst.GetState() != CGameManager.GameState.GameOver)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                CGameManager.Inst.SetState(CGameManager.GameState.GameStart);
            }

            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                    CGameManager.Inst.SetState(CGameManager.GameState.GamePlay);
            }

            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                CGameManager.Inst.SetState(CGameManager.GameState.GameOver);
            }

            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                CGameManager.Inst.SetState(CGameManager.GameState.FinishGame);
            }
        }
    
        else if(Input.GetKeyDown(KeyCode.R) && CGameManager.Inst.GetState() == CGameManager.GameState.GameOver)
        {
            CGameManager.Inst.SetState(CGameManager.GameState.GameStart);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void DebugInterativeStates()
    {
        var obj = FindAnyObjectByType<CInteractObjects>();
        
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                obj.SetState(CInteractObjects.InteracteObject.None);
            }

            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                    obj.SetState(CInteractObjects.InteracteObject.Hove);
            }

            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                obj.SetState(CInteractObjects.InteracteObject.Iinteract);
            }
    }
}
