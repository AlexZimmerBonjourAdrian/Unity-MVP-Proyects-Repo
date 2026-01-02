using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core.AI
{
   public class CMafiosoIsRutine : CEnemyConditional
    {
        // Start is called before the first frame update
        public int IsRutine;

        public override TaskStatus OnUpdate()
        {
            return (int)DataEnemy.GetStateOr() == IsRutine ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
