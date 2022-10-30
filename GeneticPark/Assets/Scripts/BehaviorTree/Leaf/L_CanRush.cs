using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_CanRush : Node
{
    private Transform m_transform;
    private Transform m_lastEnemy;

    public L_CanRush(Transform transform_)
    {
        m_transform = transform_;
    }

    public override NodeState Evaluate()
    {
        GameObject prey = (GameObject)GetData("prey");
        Transform target = prey.transform;
        if (target != m_lastEnemy)
        {
            m_lastEnemy = target;
        }

        Vector3 nextPos = m_lastEnemy.position;
        Vector3 directionToTarget = nextPos - m_transform.position;
        float dSqrToTarget = directionToTarget.sqrMagnitude;
        
        // When Close, can rush
        if (dSqrToTarget < 1600.0f)
            m_state = NodeState.SUCCESS;
        else  // if so far
            m_state = NodeState.FAILURE;

        return m_state;
    }
}
