using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_Staring : Node
{
    private Transform m_transform;
    private Transform m_lastEnemy;
    private Creature m_enemy;
    public NavMeshAgent m_navAgent;

    public Vector3 destination;
    public float m_moveScale;


    public L_Staring(Transform transform_)
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

        m_transform.LookAt(m_lastEnemy.transform);

        m_state = NodeState.RUNNING;
        return m_state;
    }
}
