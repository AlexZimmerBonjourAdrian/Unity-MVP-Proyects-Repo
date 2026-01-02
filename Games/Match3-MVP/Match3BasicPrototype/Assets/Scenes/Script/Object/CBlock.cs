using UnityEngine;

public class CBlock : MonoBehaviour, Iinteract
{

   public enum BlockType { Red, Blue, Green } 
    public BlockType blockType;

    public void Oninteract()
    {
      Debug.Log("Nombre del objeto: " + this.name.ToString());
    }
}
