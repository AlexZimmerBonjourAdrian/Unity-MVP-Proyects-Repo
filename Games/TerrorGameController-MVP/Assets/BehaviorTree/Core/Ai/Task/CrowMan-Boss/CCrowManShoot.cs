using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Core.AI;

namespace Core.CrowMan.AI
{
    public class CCrowManShoot : CBossAction
    {
        public float MaxDistanceToShoot { get; set; } = 20f;
        public int MagSizeBullet { get; set; } = 30;

        private bool shoot = false;

        private const int DamageAmount = 40; // Valor de da�o del disparo

        public override void OnStart()
        {
            DataBoss.GetLaserTargetSniper().SetActive(true);
            //DataBoss.getAnimator().Play("Teleport");
            DataBoss.getNavMeshAgent().enabled = false; // Desactivar el componente NavMeshAgent
            //DataBoss.GetRB().constraints = RigidbodyConstraints.freezer; // Congelar la posici�n y rotaci�n del jefe
        }

        public override TaskStatus OnUpdate()
        {
            Debug.Log("CrowMan Shoot");

           // RaycastHit hit;
            Vector3 direction = DataBoss.getTrget().position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = rotation; // Establecer la rotaci�n directamente

            //DataBoss.getAnimator().SetTrigger("Crouch");

            //if (timeSinceLastShot > 3f)
            //{
                //if (Physics.Raycast(DataBoss.GetSpwnBullet().position, DataBoss.GetSpwnBullet().forward, out hit, MaxDistanceToShoot, DataBoss.GetMask()))
                //{
                    //ShootTarget(hit.collider);
                    //if (timeSinceLastShot >= 1f)
                    //{
                    //    Debug.DrawRay(DataBoss.GetSpwnBullet().position, DataBoss.GetSpwnBullet().TransformDirection(Vector3.forward) * 50f, Color.yellow);

                    //    Debug.Log(hit.collider.name);

                    //    //TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);
                    //    CManagerBullet.Inst.Spawn(DataBoss.GetSpwnBullet().position, DataBoss.GetSpwnBullet().forward * 20f);
                    //    //StartCoroutine(SpawnTrail(trail, playerCamera.transform.forward * 1000, Vector3.zero, false));

                    //    //LastShootTime = Time.time;
                    //    DataBoss.GetMuzzleFlash().SetActive(true);
                    //    //hit.collider.GetComponent<CPlayerLife>().TakeDamage(20);
                    //    DataBoss.GetRB().constraints = RigidbodyConstraints.FreezeRotationX;
                    //    DataBoss.GetRB().constraints = RigidbodyConstraints.FreezeRotationZ;

                    //    Debug.Log("Entra");
                    //    timeSinceLastShot = 0f;
                    //    DataBoss.getAnimator().SetTrigger("Shoot");
                    //}
                    //DataBoss.GetMuzzleFlash().SetActive(false);
                    //DataBoss.getAnimator().SetTrigger("Aim");
                    //timeSinceLastShot += Time.deltaTime;
                   // if (timeSinceLastShot >= 3f)
                  //  {
                        //Debug.DrawRay(DataBoss.GetSpwnBullet().position, DataBoss.GetSpwnBullet().TransformDirection(Vector3.forward) * 50f, Color.yellow);

                       // Debug.Log(hit.collider.name);
                        //TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);
                        //CManagerBullet.Inst.Spawn(DataBoss.GetSpwnBullet().position, DataBoss.GetSpwnBullet().forward * 15f);
                        
                        //StartCoroutine(SpawnTrail(trail, playerCamera.transform.forward * 1000, Vector3.zero, false));

                        //LastShootTime = Time.time;
                        //DataBoss.GetMuzzleFlash().SetActive(true);
                        //hit.collider.GetComponent<CPlayerLife>().TakeDamage(20);
                        //DataBoss.GetRB().constraints = RigidbodyConstraints.FreezeRotationX;
                        //DataBoss.GetRB().constraints = RigidbodyConstraints.FreezeRotationZ;

                        Debug.Log("Entra");
                        timeSinceLastShot = 0f;
                        DataBoss.getAnimator().SetTrigger("Shoot");
                         return TaskStatus.Success;
        }
        //DataBoss.GetMuzzleFlash().SetActive(false);
        // DataBoss.getAnimator().SetTrigger("Aim");
        //timeSinceLastShot += Time.deltaTime;
        
               
        private void ShootTarget(Collider targetCollider)
        {
            //DataBoss.getAnimator().Play("SniperShoot");
           // targetCollider.GetComponentInChildren<CPlayerLife>().TakeDamage(DamageAmount);
            DataBoss.GetLaserTargetSniper().SetActive(false);
            timeSinceLastShot = 0f;
            shoot = true;
        }

    }
            
            //else
            //{
            //    timeSinceLastShot += Time.deltaTime;
                
            //}

            
        //}

       
    }


