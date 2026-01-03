using UnityEngine;

namespace HorrorEngine
{
    public class BaseCharacter : MonoBehaviour, ICharacter, Iinteract
    {
        public virtual void Inicilizate()
        {
            throw new System.NotImplementedException();
        }

        public  virtual void Oninteract()
        {
            throw new System.NotImplementedException();
        }
    }
}
