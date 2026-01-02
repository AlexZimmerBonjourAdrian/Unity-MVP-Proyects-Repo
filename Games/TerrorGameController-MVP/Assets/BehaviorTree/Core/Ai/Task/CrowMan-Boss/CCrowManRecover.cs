using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
namespace Core.AI
{ 
    public class CCrowManRecover : CBossAction
    {
        public bool IsContinue= false;
        
        public override void OnStart()
        {
            
            

        }
       IEnumerator WaitChange()
        {
            yield return new WaitForSeconds(2f);
            DataBoss.SetInvulnerable(false);
            DataBoss.setIsHurth(false);
        }

        public override TaskStatus OnUpdate()
        {
            if (IsContinue == false)
            {
                DataBoss.getAnimator().SetTrigger("Hit");
                AnimatorStateInfo currentState = DataBoss.Anim.GetCurrentAnimatorStateInfo(0);
            float TimeFinish = currentState.normalizedTime;
                IsContinue = true;
            if (TimeFinish > .7f)
            {

                    DataBoss.SetInvulnerable(false);
                    DataBoss.setIsHurth(false);
                    //StartCoroutine(WaitChange());
                   
            }
              
            }
            return DataBoss.GetIsInvulnerable() == false ? TaskStatus.Success : TaskStatus.Running;
        }

    }
}
