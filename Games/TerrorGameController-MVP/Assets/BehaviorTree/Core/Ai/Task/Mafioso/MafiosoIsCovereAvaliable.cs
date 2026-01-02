using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core.AI
{
        public class MafiosoIsCovereAvaliable : CEnemyAction
        {
        [SerializeField]
        protected Cover[] avaliableCovers;
        [SerializeField]
        //protected NavMeshAgent navMeshAgent;
       protected Transform bestCoverSpot;

        public override void OnStart()
        {
            avaliableCovers = DataEnemy.ListCover.ToArray();
        }
        public override TaskStatus OnUpdate()
        {
            
            Transform bestSpot = FindBestCoverSpot();
            SetBestCoverSpot(bestSpot);
            Debug.Log(bestSpot);
            return bestSpot != null ? TaskStatus.Success : TaskStatus.Failure;
        }
        private Transform FindBestCoverSpot()
        {
            if (GetBestCoverSpot() != null)
            {
                if (CheckIfSpotIsValid(GetBestCoverSpot()))
                {
                    return GetBestCoverSpot();
                }
            }
            //float minAngle = 90;
            Transform bestSpot = null;
            for (int i = 0; i < avaliableCovers.Length; i++) // Corrección: usar <= en lugar de -1
            {
                Transform bestSpotInCover = FindBestSpotInCover(avaliableCovers[i]);
                if (bestSpotInCover != null)
                {
                    bestSpot = bestSpotInCover;
                }
            }
            return bestSpot;
        }

        private Transform FindBestSpotInCover(Cover cover)
        {
            Transform[] avaliableSpots = cover.GetCoverSpots();
            Transform bestSpot = null;
            float minAngle = 90;

            for (int i = 0; i < avaliableSpots.Length; i++)
            {
                Vector3 direction = DataEnemy.getTrget().position - avaliableSpots[i].position;

                if (CheckIfSpotIsValid(avaliableSpots[i]))
                {
                    float angle = Vector3.Angle(avaliableSpots[i].forward, direction);

                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        bestSpot = avaliableSpots[i];
                    }
                }
            }

            return bestSpot;
        }

        private bool CheckIfSpotIsValid(Transform spot)
        {
            RaycastHit hit;
            Vector3 direction = DataEnemy.getTrget().position - spot.position;
            if (Physics.Raycast(spot.position, direction, out hit))
            {
                if (hit.collider.transform != DataEnemy.getTrget())
                {
                    return true;
                }
            }
            return false;
        }

        public void SetBestCoverSpot(Transform bestCoverSpot)
        {
            this.bestCoverSpot = bestCoverSpot;
        }

        public Transform GetBestCoverSpot()
        {
            return bestCoverSpot;
        }
    }

 



}
