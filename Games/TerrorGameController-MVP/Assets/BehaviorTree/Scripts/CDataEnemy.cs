using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CDataEnemy : MonoBehaviour
{
    public int id;
    [SerializeField]
    private float Health = 100;
    [SerializeField]
    private bool IsAlert = false;
    [SerializeField]
    public List<Cover> ListCover;
    [SerializeField]
    private List<GameObject> ListPatrollingPoints;
    private NavMeshAgent navMesh;
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.red;
    public LayerMask layer;
    public Animator Anim;
    public Transform target;
    public const int Damage_Score = 40;
    public const int Kill_Score = 200;
    Collider[] colliders = new Collider[50];
    Mesh mesh;
    [SerializeField]
    private LayerMask MaskObstacule;
    // [SerializeField]
    // public FieldOfView FienldOfWiew;

   // public CRagdollController ragdollController;
    public bool PlayingOneAnimation;
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
    private SphereCollider CollitionDetection;

    private bool IsCover = false;

    public bool IsDead;

    [SerializeField]public AudioSource audioSource;
    [SerializeField] private AudioClip SFXPaso1;
    [SerializeField] private AudioClip SFXPaso2;
    [SerializeField] private AudioClip SFXShootNormal;
    [SerializeField] private AudioClip SFXShootShootgun;
    [SerializeField] private AudioClip SFXDeathEnemy;
    
    public Transform SpawnBullet;
    public event System.Action OnDeath;

    public void Start()
    {
        Anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
       
        target = GameObject.Find("PlayerObj").transform;
       // ragdollController = GetComponent<CRagdollController>();
       
        //CGameEvents.current.OnDetectionShoot += OnDetectionShootEnemy;

        //OnDeath += Die ;
         ListCover = new List<Cover>();
        Cover[] objectsInScene = FindObjectsOfType<Cover>();
        foreach (Cover obj in objectsInScene)
        {
            ListCover.Add(obj);
        }
        navMesh.enabled = true;
    }
    
    public bool GetPlayingOneAnimation()
    {
        return PlayingOneAnimation;
    }
    public void SetPlayingOneAnimation(bool PlayingAnimation)
    {
        this.PlayingOneAnimation = PlayingAnimation;
    }
    public List<GameObject> getListPatrollingPoints()
    {
        return this.ListPatrollingPoints;
    }
    private void Update()
    {
        CanSee();
        //Death();
    }
    public Animator getAnimator()
    {
        return Anim;
    }
    public enum StateIdle
    {
        Reset = 0,
        sit = 1,
        Idle = 2


    };

    public StateIdle stateIdle;

    public Transform getTrget()
    {
        return target;
    }

    public LayerMask GetMask()
    {
        return Mask;
    }

    public enum StateOr
    {
        Idle = 0,
        Patrolling = 1
    };

    public StateOr StateOrientation = 0;

    public StateOr GetStateOr()
    {
        return this.StateOrientation;
    }
    public NavMeshAgent getNavMeshAgent()
    {
        return navMesh;
    }
    public StateIdle getStateIdle()
    {
        return this.stateIdle;
    }
    public float GetHealth()
    {
        return Health;
    }
    public void SetHealth(int Health)
    {
        this.Health = Health;
    }

    public void TakeDamage(float Damage)
    {
        setAlert(true);
        getAnimator().Play("hit");
       
            Health -= Damage;
            if(IsDead == false)
            { 
                //CGameEvents.current.ScoreAdd(Damage_Score);  
            }

        if (Health <= 0 && !IsDead)
        {
            //CGameEvents.current.ScoreAdd(Damage_Score* 10);
            audioSource.clip = SFXDeathEnemy;
            audioSource.Play();
            Health = 0;
            Die();

           
           
        }
       
    }
    //public void Death()
    //{
    //    if(IsDead == false)
    //    { 
    //        IsDead = true;
    //        CGameEvents.current.DeathEnemy();
    //        CGameEvents.current.ScoreAdd(Kill_Score);
    //        Die();
    //    }
    //}

    protected void Die()
    {
        
        IsDead = true;
        //CGameEvents.current.DeathEnemy();
        //CGameEvents.current.ScoreAdd(Kill_Score);
        //ragdollController.SetEnabled(true);
        if (OnDeath != null)
        {
            OnDeath();
        }
       // GameObject.Destroy(gameObject);
    }

    public bool getAlert()
    {
        return this.IsAlert;
    }

    public Mesh getMesh()
    {
        return this.mesh;
    }
    public bool getIsDead()
    {
        return IsDead;
    }
    public void setAlert(bool IsAlert)
    {
        this.IsAlert = IsAlert;
    }
    public LayerMask GetObstaculeMask()
    {
      return MaskObstacule;
    }
    public float getFireRate()
    {
        return fireRate;
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
        // if(FienldOfWiew.getCanSeePlayer() == true)
        // { 
        //     this.IsAlert = FienldOfWiew.getCanSeePlayer();
        // }

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
    public void ShootNormalSound()
    {
        audioSource.clip = SFXShootNormal;
        audioSource.Play();
    }
    public void ShootShootGunSound()
    {
        audioSource.clip = SFXShootShootgun;
        audioSource.Play();
    }
    public GameObject GetMuzzleFlash()
    {
        return MuzzleFlash;
    }
  
    public bool GetIsCover()
    {
        return IsCover;
    }
    public void SetIsCover(bool isCover)
    {
        this.IsCover = isCover;
    }

    public Transform GetSpwnBullet()
    {
        return SpawnBullet;
    }
    
    public SphereCollider GetColliderDetection()
    {
        return CollitionDetection;
    }
    
   private void OnDetectionShootEnemy()
    {
     
        IsAlert = true;
    }

    
    //public void ColliderEnemyAlert()
    //{
    //    if (IsAlert == true)
    //    {
    //        if(CollitionDetection.tag == "Enemy" && CollitionDetection != gameObject)
    //        {
    //            CollitionDetection.gameObject.GetComponent<CDataEnemy>().setAlert(true);
    //        }
    //    }
    //}

}
