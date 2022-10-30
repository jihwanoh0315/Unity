using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_FindingPrey : Node
{
    //private static int m_enemyLayerMask = 1 << 6;
    private static int m_creatureLayerMask = 1 << 7;
    private Transform m_transform;
    private Creature m_creature;
    private float m_scanRange;

    public L_FindingPrey(Transform transform_)
    {
        m_transform = transform_;
        m_creature = transform_.GetComponent<Creature>();
        Eyes currBody = (Eyes)m_creature.m_body.m_bodyParts[Organ.EYES];
        m_scanRange = currBody.m_maxSight;
    }

    private int PreyPriority(Creature creature_)
    {
        return 0;
    }


    public override NodeState Evaluate()
    {
        object t = GetData("prey");
        Collider[] colliders = Physics.OverlapSphere(
            m_transform.position, m_scanRange, m_creatureLayerMask);
        if(colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                //if(it is prey, not friend) continue;

                // if not that much hungry, only find Carnivore
                // find only prey. not harv.
                if (collider.GetComponent<Creature>().m_type == Creature.Type.CARNIVORE)
                {
                    // Check hash
                    // if(PreyPriority(currCreature) < PreyPriority(newCreature) 
                    m_parent.m_parent.SetData("prey", collider.gameObject);
                }
            }
            // check starving ratio
            if (m_creature.m_hunger <= 0.0f)
            {
                // if starving, find anything
                foreach (Collider collider in colliders)
                {
                    // Check hash
                    // if(PreyPriority(currCreature) < PreyPriority(newCreature) 
                    // if not same
                    m_parent.m_parent.SetData("prey", collider.gameObject);
                }
            }
            m_state = NodeState.SUCCESS;
            return m_state;
        }
            
        m_state = NodeState.FAILURE;
        return m_state;
    }
}
