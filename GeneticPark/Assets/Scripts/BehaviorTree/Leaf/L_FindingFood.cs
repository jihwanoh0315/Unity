using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L_FindingFood : Node
{
    private static int m_fruitRange = 1 << 8;
    private static int m_meatRange = 1 << 9;
    private Transform m_transform;

    public bool FindFruit()
    {
        Collider[] colliders = Physics.OverlapSphere(
            m_transform.position, BT_Creature.m_scanRange, m_fruitRange);

        if (colliders.Length > 0)
        {
            m_parent.m_parent.SetData("food", colliders[0].gameObject);
            return true;
        }

        return false;
    }

    public bool FindMeat()
    {
        Collider[] colliders = Physics.OverlapSphere(
            m_transform.position, BT_Creature.m_scanRange, m_meatRange);

        if (colliders.Length > 0)
        {
            m_parent.m_parent.SetData("food", colliders[0].gameObject);
            return true;
        }

        return false;
    }

    public L_FindingFood(Transform transform_, List<Vector3> foodLocation_)
    {
        m_transform = transform_;
    }

    public override NodeState Evaluate()
    {
        // Carnivore
        if(m_transform.GetComponent<Creature>().m_type == Creature.Type.CARNIVORE)
        {
            if (FindMeat())
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }
        // Herbivore
        else if (m_transform.GetComponent<Creature>().m_type == Creature.Type.HERBIVORE)
        {
            if(FindFruit())
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }
        else // Omnivore
        {
            if(FindMeat() || FindFruit())
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }

        m_state = NodeState.FAILURE;
        return m_state;
    }
}
