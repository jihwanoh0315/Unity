using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_Wandering : Node
{
    private Transform m_transform;
    private Creature m_enemy;
    public NavMeshAgent m_navAgent;

    public Vector3 m_prevPos;
    public float m_moveScale;
    private float m_runResetTime = 0.5f;
    private float m_runCounter = 1.0f;

    public L_Wandering(Transform transform_)
    {
        m_transform = transform_;
        m_navAgent = transform_.gameObject.GetComponent<NavMeshAgent>();
        m_moveScale = transform_.localScale.x;
        m_prevPos = transform_.position;
    }

    public override NodeState Evaluate()
    {
        Vector3 nextPos;
        m_runCounter += Time.deltaTime;
        if (m_runCounter >= m_runResetTime)
        {
            nextPos = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;

            float turnAngle = 10;
            int i = 0;
            for (i = 0; i < 360/turnAngle; ++i)
            {
                m_navAgent.SetDestination(m_transform.position + nextPos / m_moveScale * 100.0f);
                nextPos = Quaternion.Euler(0, turnAngle, 0) * nextPos;
                if (m_navAgent.destination.x != m_transform.position.x
                && m_navAgent.destination.z != m_transform.position.z)
                    break;
            }
            if(i == 360/turnAngle)
                m_navAgent.SetDestination(m_prevPos);
            else
                m_runCounter = 0.0f;
            //m_navAgent.SetDestination(new Vector3(0.0f, 0.0f, 0.0f));
            m_prevPos = nextPos;
        }
        //if (m_navAgent.destination.x == m_transform.position.x
        //    && m_navAgent.destination.z == m_transform.position.z)
        //    m_navAgent.SetDestination(m_enemy.transform.position);

        m_state = NodeState.RUNNING;

        return m_state;
    }
}
