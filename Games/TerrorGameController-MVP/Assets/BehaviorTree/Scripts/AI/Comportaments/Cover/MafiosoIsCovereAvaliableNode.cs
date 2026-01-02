using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MafiosoIsCovereAvaliableNode : Node
{
    private Cover[] avalibleCovers;
    private Transform target;
    private CMafiosoAI AI;

    public MafiosoIsCovereAvaliableNode(Cover[] avalibleCovers, Transform target, CMafiosoAI AI)
    {
        this.avalibleCovers = avalibleCovers;
        this.target = target;
        this.AI = AI;
    }

    public override NodeState Evaluate()
    {
        Transform bestSpot = FindBestCoverSpot();
        AI.SetBestCoverSpot(bestSpot);
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    // Start is called before the first frame update

    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}


    private Transform FindBestCoverSpot()
    {
        if (AI.GetBestCoverSpot() != null)
        {
            if (CheckIfSpotIsValid(AI.GetBestCoverSpot()))
            {
                return AI.GetBestCoverSpot();
            }
        }
        float minAngle = 90;
        Transform bestSpot = null;
        for (int i = 0; i < avalibleCovers.Length; i++)
        {
            Transform bestSpotInCover = FindBestSpotInCover(avalibleCovers[i], ref minAngle);
            if (bestSpotInCover != null)
            {
                bestSpot = bestSpotInCover;
            }
        }
        return bestSpot;
    }
    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for (int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = target.position - avaliableSpots[i].position;
            if (CheckIfSpotIsValid(avaliableSpots[i]))
            {
                float angle = Vector3.Angle(avaliableSpots[i].forward, direction);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = avaliableSpots[i];
                }
            }
        }
        return bestSpot;
    }
    private bool CheckIfSpotIsValid(Transform spot)
    {
        RaycastHit hit;
        Vector3 direction = target.position - spot.position;
        if (Physics.Raycast(spot.position, direction, out hit))
        {
            if (hit.collider.transform != target)
            {
                return true;
            }
        }
        return false;
    }
}
