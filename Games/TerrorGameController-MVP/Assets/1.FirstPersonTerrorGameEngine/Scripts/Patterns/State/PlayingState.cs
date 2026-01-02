using UnityEngine;

namespace HorrorEngine
{
    public class PlayingState : IGameState
    {
        // private readonly CGameManager gameManager;

        // public PlayingState(CGameManager gameManager)
        // {
        //     this.gameManager = gameManager;
        // }

        // public void Enter()
        // {
        //     UnityEngine.Time.timeScale = 1.0f;
        //     Cursor.lockState = CursorLockMode.Locked;
        //     Cursor.visible = false;
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
