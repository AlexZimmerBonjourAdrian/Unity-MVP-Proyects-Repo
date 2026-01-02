using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public abstract class CEnemyCondition : Conditional
{
    [SerializeField]
    protected float _LIfeEnemy;

    [SerializeField]
    protected float _MaxLife;

    [SerializeField]
    protected float _MinLife;
    
    [SerializeField]
    protected GameObject _Player;
    // Start is called before the first frame update

}
