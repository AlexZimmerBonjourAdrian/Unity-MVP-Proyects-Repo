using UnityEngine;
using System.Collections.Generic;

namespace TriNodo.Core
{
    public class Node : MonoBehaviour
    {
        public int Id { get; set; }
        public List<Node> Neighbors { get; private set; } = new List<Node>();

        public void Connect(Node other)
        {
            if (!Neighbors.Contains(other))
            {
                Neighbors.Add(other);
                other.Neighbors.Add(this);
            }
        }
    }
}
