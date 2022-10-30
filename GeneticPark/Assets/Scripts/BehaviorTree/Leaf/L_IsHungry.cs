using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_IsHungry : Node
{
    public Creature m_creature;

    public L_IsHungry(Transform transform_)
    {
        m_creature = transform_.gameObject.GetComponent<Creature>();
    }

    public override NodeState Evaluate()
    {
        if(m_creature.m_hunger < m_creature.m_hungryRate)
        {
            m_state = NodeState.RUNNING;
            return m_state;
        }

        m_state = NodeState.FAILURE;
        return m_state;
    }
}
