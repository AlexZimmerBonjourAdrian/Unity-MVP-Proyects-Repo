using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Core.AI;

namespace Core.CrowMan.AI
{
    public class CCrowManAimToAim : CBossAction
    {
        public float MaxDistanceToShoot { get; set; } = 20f;
        private bool shoot = false;

        private const int DamageAmount = 40;
        // Start is called before the first frame update
        public override void OnStart()
        {
            //DataBoss.GetLaserTargetSniper().SetActive(true);
           // DataBoss.getAnimator().Play("Teleport");
            DataBoss.getNavMeshAgent().enabled = true; // Desactivar el componente NavMeshAgent
            //DataBoss.GetRB().constraints = RigidbodyConstraints.FreezeRotation; 

        }

        // Update is called once per frame
        public override TaskStatus OnUpdate()
        {
            Debug.Log("CrowMan Shoot");

            RaycastHit hit;
            //Vector3 direction = playerTransform.position - transform.position;
            //Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
            Vector3 direction = DataBoss.getTrget().position - transform.position;
            Vector3 currentDirection = Vector3.SmoothDamp(transform.forward, direction, ref currentVelocity, 0.5f);
            Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            DataBoss.getAnimator().SetTrigger("Crouch");
            if (Physics.Raycast(DataBoss.GetSpwnBullet().position, DataBoss.GetSpwnBullet().forward, out hit, Mathf.Infinity, DataBoss.GetMask()))
            {
                return TaskStatus.Success;
            }

            //Vector3 direction = DataBoss.getTrget().position - DataBoss.getPostition();
            ////direction.y = 0f; // Opcional: Mantén la rotación solo en el plano horizontal
            //// Rota el enemigo hacia el jugador

            //Quaternion rotation = Quaternion.LookRotation(direction);
            //DataBoss.setRotation(Quaternion.Slerp(DataBoss.getRotation(), rotation, Time.deltaTime* 100f));
            
            //Vector3 relativePos = DataBoss.getTrget().position - transform.position;
            //Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.rotation;
            current.x = 0;
            current.z = 0;
            //current.y = 0;
            transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime
                * 2f);
            //transform.rotation = rotation;
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);
            return TaskStatus.Running;
        }
    }
}

