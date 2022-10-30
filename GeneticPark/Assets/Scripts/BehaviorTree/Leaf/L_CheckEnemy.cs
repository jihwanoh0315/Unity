using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_CheckEnemy : Node
{
    private static int m_enemyLayerMask = 1 << 6;
    private Transform m_transform;
    private float m_scanRange;
    public L_CheckEnemy(Transform transform_)
    {
        m_transform = transform_;
        Eyes currBody = (Eyes)transform_.GetComponent<Creature>().m_body.m_bodyParts[Organ.EYES];
        m_scanRange = currBody.m_maxSight;
    }

    public override NodeState Evaluate()
    {
        Collider[] colliders = Physics.OverlapSphere(
            m_transform.position, m_scanRange, m_enemyLayerMask);
        if(colliders.Length > 0)
        {
            m_parent.m_parent.SetData("target", colliders[0].transform);

            m_state = NodeState.SUCCESS;
            return m_state;
        }
            
        m_state = NodeState.FAILURE;
        return m_state;
    }
}
