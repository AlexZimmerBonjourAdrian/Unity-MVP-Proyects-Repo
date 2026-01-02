using UnityEngine;

namespace HorrorEngine
{
    public class UnlockDoorCommand : ICommand
    {
        private readonly CGameManager gameManager;

        public UnlockDoorCommand(CGameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void Execute()
        {
            Debug.Log("UnlockDoorCommand: Executing unlock door logic.");
            gameManager.StopBackgroundMusic();
        }
    }
}
