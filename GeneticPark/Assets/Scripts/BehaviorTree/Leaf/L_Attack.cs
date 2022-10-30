/*******************************************************************************
filename : L_Attack.cs
team name : Brute Force
author : Jihwan Oh
email : jihwan.oh@digipen.edu, jihwanoh315@gmail.com
date : 2022.10.29

description:
  This file have Leaf node of Attack behavior

*******************************************************************************/
using UnityEngine;

/****************************************************************************
\Class Name : L_Attack
\Role :
 This class have attack leaf behavior

*****************************************************************************/
public class L_Attack : Node
{
    private Creature m_creature; //!< the owner
    private Creature m_prey;
    private GameObject m_lastPrey; //!< to block repeating

    public L_Attack(Transform transform_)
    {
        m_creature = transform_.gameObject.GetComponent<Creature>();
    }

    public override NodeState Evaluate()
    {
        // Get prey
        GameObject prey = (GameObject)GetData("prey");
        if (prey != m_lastPrey)
        {
            // block repeating
            m_prey = prey.GetComponent<Creature>();
            m_lastPrey = prey;
        }

        // When attack, damage the prey
        m_prey.m_health -= m_creature.m_force;

        // Running because not need go next
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
