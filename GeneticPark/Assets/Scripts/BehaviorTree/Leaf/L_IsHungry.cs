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
        // When It feel the hungry
        if(m_creature.m_hunger < m_creature.m_hungryRate)
        {
            // Then do next step
            m_state = NodeState.SUCCESS;
            return m_state;
        }

        m_state = NodeState.FAILURE;
        return m_state;
    }
}
