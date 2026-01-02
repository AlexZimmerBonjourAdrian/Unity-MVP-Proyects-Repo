using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CMafiosoAI : MonoBehaviour
{

    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;


    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] avaliableCovers;



    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;


    [SerializeField]
    private Transform WeaponTransform;
    [SerializeField]
    private float BulletSpeed;

    private float _currentHealth;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    public GameObject MuzzleToon;
 [SerializeField]
    private Animator Anim;
    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        if(GameObject.Find("PlayerObj") != null)
        {
            playerTransform = GameObject.Find("PlayerObj").transform;
        }
    }
    void Start()
    {
        _currentHealth = startingHealth;

        ConstructBehahaviourTree();
    }
    private void ConstructBehahaviourTree()
    {
        #region Start Node
        MafiosoIsCovereAvaliableNode coverAvaliableNode = new MafiosoIsCovereAvaliableNode(avaliableCovers, playerTransform, this);
        MafiosoGoToCoverNode goToCoverNode = new MafiosoGoToCoverNode(agent, this);
        MafiosoHearthNode healthNode = new MafiosoHearthNode(this, lowHealthThreshold);
        MafiosoIsCoveredNode isCoveredNode = new MafiosoIsCoveredNode(playerTransform, transform);

        MafiosoShootNode ShootNode = new MafiosoShootNode( agent,this, playerTransform, WeaponTransform, BulletSpeed, MuzzleToon, layerMask,Anim);

        MafiosoChaseNode ChaseNode = new MafiosoChaseNode(playerTransform, agent, this);
        MafiosoFollowPlayer mafiosoFollowPlayer = new MafiosoFollowPlayer(agent, this, playerTransform, Anim);
        RangeNode ChasinRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode ShootRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        //MafiosoShootNode ShootNode = new MafiosoShootNode(agent,this, playerTransform)
       
        #endregion

        Sequence chaseSequence = new Sequence(new List<Node> { ChasinRangeNode, ChaseNode });
        #region Tree section;
        Sequence shootSequence = new Sequence(new List<Node> { ShootRangeNode, ShootNode });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode });
        //Sequence chaseSequence = new Sequence(new List<Node> {ChasinRangeNode,});
        //TODO: Idle Random, DeathNode, Selector Cover o Shoot, Patrolling;
        #endregion 

        #region TopRoot
        topNode = new Selector(new List<Node> {mainCoverSequence, shootSequence, chaseSequence });
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        topNode.Evaluate();
        if (topNode.nodeState == NodeState.FAILURE)
        {
            //SetColor(Color.red);
            agent.isStopped = true;
        }
        currentHealth += Time.deltaTime * healthRestoreRate;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }
    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public void Shoot()
    {
        RaycastHit hit_1;

        if (Physics.Raycast(transform.position, transform.forward, out hit_1, Mathf.Infinity))
        {
            Debug.Log("hit an player");
            //hit_1.collider.GetComponent<CpL>
        }
    }
}
