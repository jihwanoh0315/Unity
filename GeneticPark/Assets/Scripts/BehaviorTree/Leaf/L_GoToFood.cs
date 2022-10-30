using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_GoToFood : Node
{
    private Transform m_transform;
    private Transform m_lastEnemy;
    private NavMeshAgent m_navAgent;

    public L_GoToFood(Transform transform_)
    {
        m_transform = transform_;
        m_navAgent = transform_.gameObject.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        GameObject food = (GameObject)GetData("food");
        Transform target = food.transform;
        if (target != m_lastEnemy)
        {
            m_lastEnemy = target;
        }

        Vector3 nextPos = m_lastEnemy.position;
        Vector3 directionToTarget = nextPos - m_transform.position;
        float dSqrToTarget = directionToTarget.sqrMagnitude;

        m_navAgent.SetDestination(nextPos);

        // if so far
        if (dSqrToTarget > 100.0f)
            m_state = NodeState.RUNNING;
        else // When close, can eat
            m_state = NodeState.SUCCESS;

        return m_state;
    }
}
