using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;
    private Transform target;

    private Vector3 currentVelocity;
    private float smoothDamp;
    private Transform SpawnBullet;
    private float BulletSpeed;
    //private int Damage = 20;

    [SerializeField]
    private GameObject bulletTrailPreb;
    //[SerializeField]
    //private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    [SerializeField]
    private TrailRenderer BulletTrail;
    //[SerializeField]
    //private float ShootDelay = 0.5f;
    [SerializeField]
    protected LayerMask Mask;

    private GameObject MuzzleToon;
    float timeSinceLastShot = 0f;
    public ShootNode(NavMeshAgent agent, EnemyAI ai, Transform target,Transform SpawnBullet,float BulletSpeed, GameObject muzzleToon, LayerMask mask)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        smoothDamp = 1f;
        this.SpawnBullet = SpawnBullet;
        this.BulletSpeed= BulletSpeed;
        this.MuzzleToon = muzzleToon;
        this.Mask = mask;
    }

    public override NodeState Evaluate()
    {

        agent.isStopped = true;
        //ai.SetColor(Color.green);
        //CManagerBullet.Inst.Spawn(SpawnBullet.position, ai.transform.forward * BulletSpeed, 0);
        RaycastHit hit;
        Vector3 direction = target.position - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        


        MuzzleToon.SetActive(true);
        if (Physics.Raycast(SpawnBullet.position, SpawnBullet.forward, out hit, 50, Mask))
        {
           
            if (timeSinceLastShot >= 0.09f)
            {
                Debug.DrawRay(SpawnBullet.position, SpawnBullet.TransformDirection(Vector3.forward) * 50f, Color.yellow);

                Debug.Log(hit.collider.name);
                //TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);
               
                //StartCoroutine(SpawnTrail(trail, playerCamera.transform.forward * 1000, Vector3.zero, false));

                //LastShootTime = Time.time;

               // hit.collider.GetComponent<CPlayerLife>().TakeDamage(20);

                Debug.Log("Entra");

                //_anim.Play("mp5k-shoot-automatic");
            
                timeSinceLastShot = 0f;
                //.RecoilFire();
                //ammo_in_mag--;
                //SpawnTrail(SpawnBullet.position, SpawnBullet.forward * 100, BulletSpeed);

            }
         
            timeSinceLastShot += Time.deltaTime;


            


                //Inantiate bullet hole
                //Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                //Vector3 direction = hitInfo.point - transform.position;
                //transform.rotation = Quaternion.LookRotation(direction);
            

        }
        
        MuzzleToon.SetActive(false);


        ai.transform.rotation = rotation;
        return NodeState.RUNNING;
    }





    // private void SpawnTrail( Vector3 startPos,Vector3 HitPoint, float BulletSpeed)
    // {
    //     CManagerBulletTrail.Inst.SpawnBulletTrail(startPos,HitPoint, BulletSpeed);
    // }
    

  

}
