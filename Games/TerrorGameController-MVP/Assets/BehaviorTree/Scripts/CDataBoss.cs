using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class CDataBoss : MonoBehaviour
{
    public int id;
    [SerializeField]
    private float Health = 100;
    public Image fillLine;
    //[SerializeField]
    //private float distance = 400f;
    //private float angle = 30;
    public float height = 1.0f;
    public Color MeshColor = Color.blue;
    public HumanBodyBones Hips;
    public LayerMask Layer;
    public float SmoothTransition;
    public Animator Anim;
    public Transform target;
    public const int Damage_Score = 40;
    public const int kill_Score = 200;
    Collider[] colliders = new Collider[50];
    Mesh mesh;
    // [SerializeField]
    // public FieldOfView FieldOfView;
    public Rigidbody rb;
    [SerializeField]
    private NavMeshAgent navMesh;
    [SerializeField]
    public GameObject MuzzleFlash;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private Vector3 offsetShoot;
    [SerializeField]
    private float CooldownRotation = 0.5f;
    [SerializeField]
    private LayerMask Mask;



    [SerializeField]
    private float MaxDistanceFollow = 2;
    [SerializeField]
    private bool IsDead;

    public Transform SpawnBullet;

    [SerializeField]
    private BoxCollider HixBox;
    [SerializeField]
    private bool IsAttack;

    [SerializeField]
    private float DistanceAttack;

    [SerializeField]
    private bool IsHurt = false;

    [SerializeField]
    private bool IsInvulnerable;

    [SerializeField]
    private GameObject ParticleSmoke;
    [SerializeField]
    public List<GameObject> ListTeleport = new List<GameObject>();

    [SerializeField]
    public GameObject DeathPosition;

    [SerializeField]
    private GameObject LaserTargetSniper;

    public AudioSource audioSource;

    [SerializeField] private AudioClip SFXPaso1;
    [SerializeField] private AudioClip SFXPaso2;
    [SerializeField] private AudioClip SFXFireShoot;
    [SerializeField] private AudioClip SFXDeathBoss;
    [SerializeField] private AudioClip SFXSlash;
   // [SerializeField] private AudioClip SFX
   
    public float DamageCounter = 0;
    //public enum StateIdle
    //{
    //    Set = 0,
    //    sit = 1,
    //    Idle = 2


    //};
    //public StateIdle stateIdle;
    //public enum StateOr
    //{
    //    Idle = 0,
    //    Patrolling = 1
    //};

    //public StateOr StateOrientation = 0;
    //public StateOr GetStateOr()
    //{
    //    return this.StateOrientation;
    //}
    void Start()
    {
        Anim = GetComponent<Animator>();
        //navMesh = GetComponent<NavMeshAgent>();
        target = GameObject.Find("PlayerObj").transform;
        //ragdollController = GetComponent<CRagdollController>();
        rb = GetComponent<Rigidbody>();
        navMesh.enabled = true;
        LaserTargetSniper.SetActive(false);
        GameObject[] objectsInScene = GameObject.FindGameObjectsWithTag("Teleport");
        //for (int i = 0; i <= 4; i++)
        //{
        //    TeleportBucle();
        //}


        //OnDeath += Die ;

        foreach (GameObject obj in objectsInScene)
        {
            ListTeleport.Add(obj);
        }
    }
    void Update()
    {
        //Debug.Log("Entra en TakeDamage" + getIsHurth());
        target = GameObject.Find("PlayerObj").transform;
        Debug.Log("IsInvulnerabiliti" + IsInvulnerable);
        UpdateHPInFill();
    }

    public Rigidbody GetRB()
    {
        return rb;
    }
    public GameObject getDeathPosition()
    {
        return DeathPosition;
    }
    public float getMaxDistanceFollow()
    {
        return this.MaxDistanceFollow;
    }

    public bool getIsHurth()
    {
        return IsHurt;
    }
    public void setIsHurth(bool isHurt)
    {
        this.IsHurt = isHurt;
    }

    public Vector3 getPostition()
    {
        return transform.position;
    }
    public Transform getTrget()
    {
        return target;
    }
    public LayerMask GetMask()
    {
        return Mask;
    }
    public NavMeshAgent getNavMeshAgent()
    {
        return navMesh;
    }
    //public StateIdle getStateIdle()
    //{
    //    return this.stateIdle;
    //}
    public float GetHealth()
    {
        return Health;
    }

    public BoxCollider getHitBox()
    {
        return this.HixBox;
    }
    public void SetHealth(int Health)
    {
        this.Health = Health;
    }

    public GameObject GetLaserTargetSniper()
    {
        return LaserTargetSniper;
    }
    public event System.Action OnDeath;

    public bool GetIsInvulnerable()
    {
        return IsInvulnerable;
    }
    public void TakeDamage(float Damage)
    {
        
        Debug.Log(Health + "Vida Restante del jefe");

        //if(IsInvulnerable == false)
        //{ 
        Health -= Damage;
        if (IsDead == false)
            {
               
                //CGameEvents.current.ScoreAdd(Damage_Score);
                DamageCounter += Damage;
                Debug.Log(DamageCounter + "Damage Counter");
               
            }
        //}
        if (DamageCounter >= 60)
        {
            Debug.Log("Entra en el Damage Counter Checker");
            DamageCounter = 0;
        }

        if (Health <= 0 && !IsDead)
        {
            //CGameEvents.current.ScoreAdd(Damage_Score * 10);
            Health = 0;
            Die();
        }

    }
    public void SetInvulnerable(bool Invulnerable)
    {
        IsInvulnerable = Invulnerable;
    }
    protected void Die()
    {

       // IsDead = true;
        //CGameEvents.current.DeathEnemy();
        //CGameEvents.current.ScoreAdd(Damage_Score);
        if (OnDeath != null)
        {
            OnDeath();
        }
        audioSource.clip = SFXDeathBoss;
        audioSource.Play();
         //CGameEvents.current.DeathBoss();
         GameObject.Destroy(gameObject);
    }
    public void Death()
    {

        if (Health <= 0)
        {
            if (IsDead == false)
            {

               // CGameEvents.current.DeathEnemy();
               // CGameEvents.current.ScoreAdd(kill_Score);
                //CGameEvents.current.DeathBoss();
                IsDead = true;
                Destroy(gameObject);
               

            }
        }
    }
    // Start is called before the first frame update

    public Animator getAnimator()
    {
        return Anim;
    }
    public Mesh getMesh()
    {
        return this.mesh;
    }
    public bool getIsDead()
    {
        return IsDead;
    }
    public float getFireRate()
    {
        return fireRate;
    }
    public Quaternion getRotation()
    {
        return transform.rotation;
    }

    public void setRotation(Quaternion rotation)
    {
        transform.rotation = rotation;  
    }
    public float getCooldown()

    {
        return CooldownRotation;
    }
    public Vector3 getoffsetShoot()
    {
        return offsetShoot;
    }
    public void CanSee()
    {
        // if (FieldOfView.getCanSeePlayer() == true)
        // {
        //    // this.IsAlert = FienldOfWiew.getCanSeePlayer();
        // }

    }

    public List<GameObject> GetTeletransportPoints()
    {
        return this.ListTeleport;
    }
    public GameObject GetMuzzleFlash()
    {
        return MuzzleFlash;
    }
    public Transform GetSpwnBullet()
    {
        return SpawnBullet;
    }

    public GameObject getParticleSmoke()
    {
        return ParticleSmoke;
    }

    public void FireSound()
    {
        audioSource.clip = SFXFireShoot;
        audioSource.Play();
    }

    public void Paso1Sound()
    {
        audioSource.clip = SFXPaso1;
        audioSource.Play();
    }

    public void Paso2Sound()
    {
        audioSource.clip = SFXPaso2;
        audioSource.Play();
    }

    //public SphereCollider GetColliderDetection()
    //{
    //    return CollitionDetection;
    //}
    // Update is called once per frame
    public void UpdateHPInFill()
    {
        float normalizedHealth = Mathf.Clamp01(Health / 1000f);
        fillLine.fillAmount = normalizedHealth;
    }
}
