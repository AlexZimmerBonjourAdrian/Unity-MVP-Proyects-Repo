using UnityEngine;

namespace HorrorEngine
{
    public class UnlockDoorCommand : ICommand
    {
        public UnlockDoorCommand()
        {
        }

        public void Execute()
        {
            Debug.Log("UnlockDoorCommand: Executing unlock door logic.");
            // CGameManager eliminado - funcionalidad removida
        }
    }
}
