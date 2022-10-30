using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_CanAttack : Node
{
    private Transform m_transform;
    private Transform m_lastEnemy;

    private float m_attkResetTime = .25f;
    private float m_attkCounter = 1.0f;

    public L_CanAttack(Transform transform_)
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


        m_attkCounter += Time.deltaTime;

        // if so far
        if (dSqrToTarget < 100.0f && m_attkResetTime < m_attkCounter)
            m_state = NodeState.SUCCESS;
        else // When far, can rush
            m_state = NodeState.FAILURE;

        return m_state;
    }
}
