using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{
    public class CMafiosoHeañth : CEnemyAction
    {
        [SerializeField]
        private float threshold;

        public CDataEnemy Data;
        public override void OnStart()
        {
            Data = GetComponent<CDataEnemy>();
            threshold = Data.GetHealth();
        }
        public override TaskStatus OnUpdate()
        {
            return currentHealth <= threshold ? TaskStatus.Success : TaskStatus.Failure;
        }

       
    }




}
