using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_Runaway : Node
{
    private Transform m_transform;
    private Transform m_lastEnemy;
    private Creature m_enemy;
    public NavMeshAgent m_navAgent;

    public Vector3 destination;
    public float m_moveScale;
    private float m_runResetTime = 0.5f;
    private float m_runCounter = 1.0f;

    public L_Runaway(Transform transform_)
    {
        m_transform = transform_;
        m_navAgent = transform_.gameObject.GetComponent<NavMeshAgent>();
        m_moveScale = transform_.localScale.x;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != m_lastEnemy)
        {
            m_enemy = target.GetComponent<Creature>();
            m_lastEnemy = target;
        }

        m_runCounter += Time.deltaTime;
        if (m_runCounter >= m_runResetTime)
        {
            Vector3 nextPos = (m_transform.position - m_lastEnemy.position);
            nextPos = new Vector3(nextPos.x, 0.0f, nextPos.z).normalized;
            //nextPos = new Vector3(nextPos.x, nextPos.y, 0.0f).normalized;


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
                m_navAgent.SetDestination(m_lastEnemy.transform.position);
            else
             m_runCounter = 0.0f;
            //m_navAgent.SetDestination(new Vector3(0.0f, 0.0f, 0.0f));
        }
        //if (m_navAgent.destination.x == m_transform.position.x
        //    && m_navAgent.destination.z == m_transform.position.z)
        //    m_navAgent.SetDestination(m_enemy.transform.position);


        m_state = NodeState.RUNNING;

        return m_state;
    }
}
