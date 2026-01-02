using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MafiosoShootNode : Node
{
    // Start is called before the first frame update

    private NavMeshAgent agent;
    RaycastHit hit;
    private CMafiosoAI ai;
    private Vector3 currentVelocity;
    private float smoothDamp;
    private Transform SpawnBullet;
    private float BulletSpeed;
    private Transform target;
    //private int Damage = 20;
    [SerializeField]
    private GameObject bulletTrailPreb;
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
    private Animator anim;
    public MafiosoShootNode(NavMeshAgent agent, CMafiosoAI ai, Transform target, Transform SpawnBullet, float BulletSpeed, GameObject muzzleToon, LayerMask mask, Animator anim)
    {
        this.ai = ai;
        this.target = target;
        smoothDamp = 1f;
        this.SpawnBullet = SpawnBullet;
        this.BulletSpeed = BulletSpeed;
        this.MuzzleToon = muzzleToon;
        this.Mask = mask;
        this.agent = agent;
        this.anim = anim;
    }


    public override NodeState Evaluate()
    {
        RaycastHit hit;
        agent.isStopped = true;
        //ai.SetColor(Color.green);
        Vector3 direction = target.position - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        //CManagerBullet.Inst.Spawn(SpawnBullet.position, ai.transform.forward * BulletSpeed, 0);


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

                //hit.collider.GetComponent<CPlayerLife>().TakeDamage(20);

                Debug.Log("Entra");

                //_anim.Play("mp5k-shoot-automatic");

                timeSinceLastShot = 0f;
                //.RecoilFire();
                //ammo_in_mag--;
                SpawnTrail(SpawnBullet.position, SpawnBullet.forward * 100, BulletSpeed);
            }

            timeSinceLastShot += Time.deltaTime;

        }

        MuzzleToon.SetActive(false);

        ai.transform.rotation = rotation;
        return NodeState.RUNNING;
    }

    private void SpawnTrail(Vector3 startPos, Vector3 HitPoint, float BulletSpeed)
    {
        //CManagerBulletTrail.Inst.SpawnBulletTrail(startPos, HitPoint, BulletSpeed);
    }

}
