using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_Sneaking : Node
{
    private Transform m_transform;
    private Transform m_lastEnemy;
    private Creature m_creature;
    public NavMeshAgent m_navAgent;

    public Vector3 destination;
    public float m_moveScale;
    private float m_runResetTime = 0.5f;
    private float m_runCounter = 1.0f;

    public L_Sneaking(Transform transform_)
    {
        m_transform = transform_;
        m_navAgent = transform_.gameObject.GetComponent<NavMeshAgent>();
        m_creature = transform_.gameObject.GetComponent<Creature>();
        m_moveScale = transform_.localScale.x;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != m_lastEnemy)
        {
            m_lastEnemy = target;
        }

        Vector3 nextPos = m_lastEnemy.position;
        Vector3 directionToTarget = nextPos - m_transform.position;
        float dSqrToTarget = directionToTarget.sqrMagnitude;

        //Check distance
        if (dSqrToTarget > 400.0f)
        {
            m_navAgent.speed = m_creature.m_maxSpeed * m_creature.m_sneakSpeedRate;
            m_navAgent.SetDestination(nextPos);

            m_state = NodeState.RUNNING;
            return m_state;
        }
        // If very close, Attack!
        else
        {
            m_state = NodeState.SUCCESS;
            return m_state;
        }
    }
}
