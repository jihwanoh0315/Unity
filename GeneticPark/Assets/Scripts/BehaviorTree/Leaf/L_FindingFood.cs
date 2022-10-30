using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_FindingFood : Node
{
    private static int m_fruitRange = 1 << 8;
    private static int m_meatRange = 1 << 9;
    private NavMeshAgent m_navAgent;
    private Transform m_transform;
    public List<Vector3> m_foodLocation;


    public bool FindFruit()
    {
        Collider[] colliders = Physics.OverlapSphere(
            m_transform.position, BT_Creature.m_scanRange, m_fruitRange);

        if (colliders.Length > 0)
        {
            m_parent.m_parent.SetData("food", colliders[0].gameObject);
            return true;
        }

        return false;
    }

    public bool FindMeat()
    {
        Collider[] colliders = Physics.OverlapSphere(
            m_transform.position, BT_Creature.m_scanRange, m_meatRange);

        if (colliders.Length > 0)
        {
            m_parent.m_parent.SetData("food", colliders[0].gameObject);
            return true;
        }

        return false;
    }

    public L_FindingFood(Transform transform_, List<Vector3> foodLocation_)
    {
        m_transform = transform_;
        m_navAgent = transform_.gameObject.GetComponent<NavMeshAgent>();
        m_foodLocation = foodLocation_;
    }

    public override NodeState Evaluate()
    {
        if (m_navAgent.enabled)
        {
            if (m_foodLocation.Count > 0)
            {
                Vector3 bestTarget = new Vector3(0.0f, 0.0f, 0.0f);
                float closestDistanceSqr = Mathf.Infinity;
                Vector3 currentPosition = m_transform.position;
                foreach (Vector3 food in m_foodLocation)
                {
                    Vector3 potentialTarget = food;
                    Vector3 directionToTarget = potentialTarget - currentPosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = potentialTarget;
                    }
                }
                m_navAgent.SetDestination(bestTarget);

                if(closestDistanceSqr < 100.0f)
                {
                    m_transform.GetComponent<Creature>().m_hunger += 5.0f * Time.deltaTime;
                }

            }
        }

        //Debug.Log("FindFood");
        m_state = NodeState.RUNNING;
        //return m_state; 
        
        // Carnivore
        if(m_transform.GetComponent<Creature>().m_type == Creature.Type.CARNIVORE)
        {
            if(FindFruit())
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }
        // Herbivore
        else if (m_transform.GetComponent<Creature>().m_type == Creature.Type.HERBIVORE)
        {
            if (FindMeat())
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }
        else // Omnivore
        {
            if(FindMeat() || FindFruit())
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }

        m_state = NodeState.FAILURE;
        return m_state;
    }
}
