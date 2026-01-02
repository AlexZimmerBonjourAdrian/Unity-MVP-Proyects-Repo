using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParamet_", menuName = "PEG/SimpleRnadomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int interactions = 10, walkLength = 10;
    public bool startRandomlyEachInteration = true;

        
}
