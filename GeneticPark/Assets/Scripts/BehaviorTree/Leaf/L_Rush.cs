using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_Rush : Node
{
    private Creature m_creature;
    public NavMeshAgent m_navAgent;

    public L_Rush(Transform transform_)
    {
        m_navAgent = transform_.gameObject.GetComponent<NavMeshAgent>();
        m_creature = transform_.gameObject.GetComponent<Creature>();
    }

    public override NodeState Evaluate()
    {
        m_navAgent.speed = m_creature.m_maxSpeed * m_creature.m_maxSpeed;

        m_state = NodeState.RUNNING;
        return m_state;
    }
}
