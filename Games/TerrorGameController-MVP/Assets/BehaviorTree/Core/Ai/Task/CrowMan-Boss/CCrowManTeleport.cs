using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
namespace Core.AI
{
    public class CCrowManTeleport : CBossAction
    {
        int i=0;
        bool isSelected = false;
        
        
        
        public override void OnStart()
        {
            DataBoss.getAnimator().Play("Teleport");
            //DataBoss.getNavMeshAgent().isStopped = true;
         
        }
        public override TaskStatus OnUpdate()
        {
            Debug.Log("CrowManTeleport");
            AnimatorStateInfo currentState = DataBoss.Anim.GetCurrentAnimatorStateInfo(0);
            float TimeFinish = currentState.normalizedTime;
            if (TimeFinish > .5f)
            {
                DataBoss.getParticleSmoke().GetComponent<ParticleSystem>().Play();
            }
            if (TimeFinish > .9f)
            { 
                for (int i = 0; i <= 4; i++)
                {
                    TeleportBucle();
                    
                }
            }
            if (isSelected == true)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;

        }
       public int TeleportSelecter()
        {
            var ListAllTeleport = DataBoss.GetTeletransportPoints();
           var random = Random.Range(0, DataBoss.GetTeletransportPoints().Count-1);
            //Debug.Log(random + "Nodo de telepor seleccionado");
            return random;
        }

      
        public void TeleportBucle()
        {

            if (i <= DataBoss.GetTeletransportPoints().Count - 1)
            {
                if (i != TeleportSelecter())
                {
                   // Debug.Log(i + "Teleport Node Recorer");
                    StartCoroutine(WaitCorutine());
                    transform.position = DataBoss.GetTeletransportPoints()[i].transform.position;
                    i++;
                }
                else
                {
                    isSelected = true;
                }
            }
          
            else
            {
                i = 0;
                isSelected = true;
            }
            
        }
        IEnumerator WaitCorutine()
        {
            Debug.Log("Entra en WaitCorutine");
            yield return new WaitForSeconds(5f);
            TeleportBucle();
        }
    }
}
