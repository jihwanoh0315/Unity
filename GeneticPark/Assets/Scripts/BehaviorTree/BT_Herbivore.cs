using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BT_Herbivore : BHTree
{
    public static float m_speed = 2.0f;
    public static float m_scanRange = 50.0f;
    protected void Start()
    {
        m_owner = gameObject;
        m_foodLocation = m_owner.GetComponent<Creature>().m_foodLocation;
        m_root = SetupTree();
    }

    protected override Node SetupTree()
    {
        Node root = new C_Selector(new List<Node>
        {
            new C_Sequencer(new List<Node>
            {
                new L_CheckEnemy(transform),
                new C_Selector(new List<Node>
                {
                    new C_Sequencer(new List<Node>
                    {
                        new L_CheckEnemyInRange(transform),
                        new L_Runaway(transform),
                    }),
                    new L_Staring(transform),
                }),
            }),
            new L_FindingFood(transform, m_foodLocation),
        });
        return root;
    }
}
