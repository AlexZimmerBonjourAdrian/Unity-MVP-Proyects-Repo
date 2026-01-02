using UnityEngine;

namespace HorrorEngine
{
    public class PausedState : IGameState
    {
        // private readonly CGameManager gameManager;

        // public PausedState(CGameManager gameManager)
        // {
        //     this.gameManager = gameManager;
        // }

        // public void Enter()
        // {
        //     UnityEngine.Time.timeScale = 0.0f;
        //     Cursor.lockState = CursorLockMode.None;
        //     Cursor.visible = true;
        // }

        // public void Exit()
        // {
        //     // Cleanup logic if needed
        // }
        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}
