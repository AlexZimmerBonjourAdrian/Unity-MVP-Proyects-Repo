using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime.Tasks;
namespace Core.AI
{ 
public class CMafiosoShootShotgun : CEnemyAction
{
        // Start is called before the first frame update
        public float MaxDistanceToShoot = 20f;
        public int magSize_bullet = 30;
        public float max_distance_Shoot;
        public int billet_in_Mag;
        public bool IsCrouch;
        public bool HasShoot = false;
        //public float range;
        private string animationTriggerName;
        // Start is called before the first frame update

        public BehaviorDesigner.Runtime.SharedGameObject targetGameObject;

        public BehaviorDesigner.Runtime.SharedVector3 offset;

        // cache the navmeshagent component

        private GameObject prevGameObject;
        [SerializeField]
        private NavMeshAgent navMeshAgent;




        public override TaskStatus OnUpdate()
        {

            //float distance = Vector3.Distance(playerTransform.position, transform.position);
            DataEnemy.getNavMeshAgent().isStopped = true;
            //ai.SetColor(Color.green);
            //CManagerBullet.Inst.Spawn(SpawnBullet.position, ai.transform.forward * BulletSpeed, 0);
            RaycastHit hit;
            Vector3 direction = DataEnemy.getTrget().position - transform.position;
            Vector3 currentDirection = Vector3.SmoothDamp(transform.forward, direction, ref currentVelocity, 0.005f);
            Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            DataEnemy.getAnimator().SetTrigger("Aim");
            var isWall = Physics.Raycast(DataEnemy.GetSpwnBullet().position, DataEnemy.GetSpwnBullet().forward, out hit, 4, DataEnemy.GetObstaculeMask());
            if (!isWall)
            {
               // Debug.Log(hit.collider.name);
                if (Physics.Raycast(DataEnemy.GetSpwnBullet().position, DataEnemy.GetSpwnBullet().forward, out hit, 50, DataEnemy.GetMask()))
            {

                if (timeSinceLastShot >= 2f)
                {
                    Debug.DrawRay(DataEnemy.GetSpwnBullet().position, DataEnemy.GetSpwnBullet().TransformDirection(Vector3.forward) * 50f, Color.yellow);

                    Debug.Log(hit.collider.name);
                        //TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);

                        //StartCoroutine(SpawnTrail(trail, playerCamera.transform.forward * 1000, Vector3.zero, false));
                        //DataEnemy.GetMuzzleFlash().SetActive(true);
                        //LastShootTime = Time.time;
                        //DataEnemy.GetMuzzleFlash().SetActive(true);
                        //hit.collider.GetComponent<CPlayerLife>().TakeDamage(20);
                        // CManagerBullet.Inst.Spawn(DataEnemy.GetSpwnBullet().position , DataEnemy.GetSpwnBullet().forward * 20f);
                        // CManagerBullet.Inst.Spawn(DataEnemy.GetSpwnBullet().position, DataEnemy.GetSpwnBullet().forward * 20f);
                        // CManagerBullet.Inst.Spawn(DataEnemy.GetSpwnBullet().position, DataEnemy.GetSpwnBullet().forward * 20f);
                        // CManagerBullet.Inst.Spawn(DataEnemy.GetSpwnBullet().position, DataEnemy.GetSpwnBullet().forward * 20f);

                        Debug.Log("Entra");
                    timeSinceLastShot = 0f;
                        DataEnemy.getAnimator().Play("FiringRiffle");
                    }
                DataEnemy.GetMuzzleFlash().SetActive(false);
                DataEnemy.getAnimator().SetTrigger("Aim");
                timeSinceLastShot += Time.deltaTime;





                    //Inantiate bullet hole
                    //Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                    //Vector3 direction = hitInfo.point - transform.position;
                    //transform.rotation = Quaternion.LookRotation(direction);

                    return TaskStatus.Failure;

                }
               
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
            return TaskStatus.Running;
          

        }

        //public override TaskStatus OnUpdate()
        //{
        //    //ai.SetColor(Color.yellow);
        //    //float distance = Vector3.Distance(playerTransform.position, navMeshAgent.transform.position);
        //    //if (distance > 0.2f)
        //    //{
        //    //    navMeshAgent.isStopped = false;
        //    //    navMeshAgent.SetDestination(target.position);
        //    //    return TaskStatus.Running;
        //    //}
        //    //else
        //    //{
        //    //    navMeshAgent.isStopped = true;
        //    //    return TaskStatus.Success;
        //    //}
        //    return FollowPositionStyle2();


        //    //return HasShoot ? TaskStatus.Success : TaskStatus.Running;
        //}
        private void SpawnTrail(Vector3 startPos, Vector3 HitPoint, float BulletSpeed)
        {
            //CManagerBulletTrail.Inst.SpawnBulletTrail(startPos, HitPoint, BulletSpeed);
        }

        public override void OnEnd()
        {
            HasShoot = false;
        }

        public TaskStatus FollowPositionStyle2()
        {
            if (navMeshAgent == null)
            {
                Debug.LogWarning("NavMeshAgent is null");
                //return TaskStatus.Failure;
                navMeshAgent = DataEnemy.getNavMeshAgent();
            }

            float distance = Vector3.Distance(DataEnemy.getTrget().position, navMeshAgent.transform.position);
            if (distance > 0.2f)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(DataEnemy.getTrget().position);
                return TaskStatus.Running;
            }
            else
            {
                navMeshAgent.isStopped = true;
                return TaskStatus.Success;
            }
        }
        public TaskStatus FollowPositionStyle1()
        {
            if (navMeshAgent == null)
            {
                Debug.LogWarning("NavMeshAgent is null");
                //return TaskStatus.Failure;
                navMeshAgent = DataEnemy.getNavMeshAgent();
            }

            navMeshAgent.SetDestination(playerTransform.position);

            return TaskStatus.Running;
        }
    }
}

