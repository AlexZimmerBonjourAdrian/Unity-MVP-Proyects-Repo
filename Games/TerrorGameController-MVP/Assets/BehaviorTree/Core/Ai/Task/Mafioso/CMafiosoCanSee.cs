using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Core.AI
{
    //[HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CMafiosoCanSee : CEnemyConditional
    {
        //[Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
        //[Tooltip("The object that we are searching for")]
        public GameObject targetObject;
        public List<GameObject> targetObjects;
        //[Tooltip("The tag of the object that we are searching for")]
        public string targetTag;
        //[Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        //[Tooltip("If using the object layer mask, specifies the maximum number of colliders that the physics cast can collide with")]
        public int maxCollisionCount = 200;
        //[Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask ignoreLayerMask;
        //[Tooltip("The field of view angle of the agent (in degrees)")]
        public float fieldOfViewAngle = 90;
        //[Tooltip("The distance that the agent can see")]
        public float viewDistance = 1000;
        //[Tooltip("The raycast offset relative to the pivot position")]
        public Vector3 offset;
        //[Tooltip("The target raycast offset relative to the pivot position")]
        public Vector3 targetOffset;
        //[Tooltip("The offset to apply to 2D angles")]
        public float angleOffset2D;
        //[Tooltip("Should the target bone be used?")]
        public bool useTargetBone;
        //[Tooltip("The target's bone if the target is a humanoid")]
        public HumanBodyBones targetBone;
        //[Tooltip("Should a debug look ray be drawn to the scene view?")]
        public bool drawDebugRay;
        //[Tooltip("Should the agent's layer be disabled before the Can See Object check is executed?")]
        public bool disableAgentColliderLayer;
        //[Tooltip("The object that is within sight")]
        public bool returnedObject;

        private GameObject[] agentColliderGameObjects;
        private int[] originalColliderLayer;
        private Collider[] overlapColliders;
        private Collider2D[] overlap2DColliders;

        private int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");



        public float distance = 10;
        public float angle = 30;
        public float height = 1.0f;
        public Color meshColor = Color.red;

        Mesh mesh;

        public override TaskStatus OnUpdate()
        {
            return DataEnemy.getAlert() == false ? TaskStatus.Success : TaskStatus.Failure;
        }


    }

    
   
        
}
