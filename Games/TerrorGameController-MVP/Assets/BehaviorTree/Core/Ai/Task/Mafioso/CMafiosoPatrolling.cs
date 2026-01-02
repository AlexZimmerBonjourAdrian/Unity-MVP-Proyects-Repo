using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
namespace Core.AI
 {
   
        [TaskDescription("Patrol around the specified waypoints using the Unity NavMesh.")]
        [TaskCategory("Movement")]
       // [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
        [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]
        public class CMafiosoPatrolling : CEnemyAction
        {
            //[Tooltip("Should the agent patrol the waypoints randomly?")]
            public bool randomPatrol = false;
           // [Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
            public float waypointPauseDuration= 5f;
           // [Tooltip("The waypoints to move to")]
            public List<GameObject> waypoints;

            // The current index that we are heading towards within the waypoints array
            private int waypointIndex;
            private float waypointReachedTime= 5f;

        //private float Speed = 2f;
            public override void OnStart()
            {
                base.OnStart();
                 waypoints = DataEnemy.getListPatrollingPoints();

                // initially move towards the closest waypoint
                float distance = Mathf.Infinity;
                float localDistance;
                for (int i = 0; i < waypoints.Count; ++i)
                {
                    if ((localDistance = Vector3.Magnitude(transform.position - waypoints[i].transform.position)) < distance)
                    {
                        distance = localDistance;
                        waypointIndex = i;
                    }
                }
                waypointReachedTime = -1;
            DataEnemy.getNavMeshAgent().enabled = true;
            DataEnemy.getNavMeshAgent().SetDestination(Target());
            DataEnemy.getAnimator().SetTrigger("WalkPatrolling");
            }

            // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
            public override TaskStatus OnUpdate()
            {
         
            if (waypoints.Count == 0 )
            {
                    return TaskStatus.Failure;
            }
            float distance = Vector3.Distance(transform.position, DataEnemy.getNavMeshAgent().destination);
            if (distance < 0.5f)
                {
                    if (waypointReachedTime == -1)
                    {
                        waypointReachedTime = Time.time;
                    }
                    // wait the required duration before switching waypoints.
                    if (waypointReachedTime + waypointPauseDuration <=  Time.time)
                    {
                        if (randomPatrol)
                        {
                            if (waypoints.Count == 1)
                            {
                                waypointIndex = 0;
                            }
                            else
                            {
                                // prevent the same waypoint from being selected
                                var newWaypointIndex = waypointIndex;
                            DataEnemy.getAnimator().SetTrigger("WalkPatrolling");
                            while (newWaypointIndex == waypointIndex)
                                {
                                    newWaypointIndex = Random.Range(0, waypoints.Count);
                                }
                                waypointIndex = newWaypointIndex;
                            }
                        }
                        else
                        {
                            waypointIndex = (waypointIndex + 1) % waypoints.Count;
                        }
                        DataEnemy.getNavMeshAgent().SetDestination(Target());
                        waypointReachedTime = -1;
                    }
                }
                if (DataEnemy.getAlert() == true)
                {
                     TerminatedAnimation();
                     return TaskStatus.Failure;
                }
                return TaskStatus.Running;
            }

            // Return the current waypoint index position
            private Vector3 Target()
            {
                if (waypointIndex >= waypoints.Count)
                {
                    return transform.position;
                }
                return waypoints[waypointIndex].transform.position;
            }
                
        public void TerminatedAnimation()
        {
            DataEnemy.getAnimator().SetTrigger("Idle");
        }
            // Reset the public variables
            public override void OnReset()
            {
                base.OnReset();
      
            randomPatrol = false;
                waypointPauseDuration = 0;
                waypoints = null;
            }

            // Draw a gizmo indicating a patrol 
            public override void OnDrawGizmos()
            {
#if UNITY_EDITOR
                if (waypoints == null || waypoints == null)
                {
                    return;
                }
                var oldColor = UnityEditor.Handles.color;
                UnityEditor.Handles.color = Color.yellow;
                for (int i = 0; i < waypoints.Count; ++i)
                {
                    if (waypoints[i] != null)
                    {
                        UnityEditor.Handles.SphereHandleCap(0, waypoints[i].transform.position, waypoints[i].transform.rotation, 1, EventType.Repaint);
                    }
                }
                UnityEditor.Handles.color = oldColor;
#endif
            }
        }
    }

