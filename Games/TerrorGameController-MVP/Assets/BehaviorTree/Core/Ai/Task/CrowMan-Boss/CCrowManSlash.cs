using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
namespace Core.AI
{
    public class CCrowManSlash : CBossAction
    {
        [SerializeField]
        private Transform target;

        public override void OnStart()
        {
            target = playerTransform;
            var random = Random.Range(0, 1);
            if (random == 0)
            {
                DataBoss.getAnimator().SetTrigger("Attack_1");
            }
            if (random == 1)
            {
                DataBoss.getAnimator().SetTrigger("Attack_2");
            }
        }

        public override TaskStatus OnUpdate()
        {
      

           
            

            float distance = Vector3.Distance(target.position, transform.position);
            Debug.Log("Crow Man Slash");
            RaycastHit hit;
            Vector3 direction = DataBoss.getTrget().position - transform.position;
            Vector3 currentDirection = Vector3.SmoothDamp(transform.forward, direction, ref currentVelocity, 0.5f);
            Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            Debug.Log("Entra en la caja de coliciones");
            // var PlayerLife = DataBoss.getHitBox().gameObject.GetComponentInChildren<CPlayerLife>();
            if (Physics.Raycast(DataBoss.GetSpwnBullet().position, DataBoss.GetSpwnBullet().forward, out hit, 3f, DataBoss.GetMask()))
            {
               
                AnimatorStateInfo currentState = DataBoss.Anim.GetCurrentAnimatorStateInfo(0);

                float TimeFinish = currentState.normalizedTime;
                Debug.DrawRay(DataBoss.GetSpwnBullet().position, DataBoss.GetSpwnBullet().TransformDirection(Vector3.forward) * 3f, Color.yellow);
                Debug.Log(hit.collider.name);
                //DataBoss.GetMuzzleFlash().SetActive(true);
                //hit.collider.GetComponentInChildren<CPlayerLife>().TakeDamage(40);

                if(TimeFinish > 0.98)
                {
                    return TaskStatus.Success;
                }  
                
            }
            else
            {
                return TaskStatus.Failure;
            }
            
            
            return TaskStatus.Failure;
           
          
        }
       


    }
}
