using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_CheckEnemyInRange : Node
{
    private static int m_enemyLayerMask = 1 << 6;
    private Transform m_transform;
    private float m_scanRange;

    public L_CheckEnemyInRange(Transform transform_)
    {
        m_transform = transform_;
        Brain currBody = (Brain)transform_.GetComponent<Creature>().m_body.m_bodyParts[Organ.BRAIN];
        m_scanRange = currBody.m_runSight;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        Collider[] colliders = Physics.OverlapSphere(
            m_transform.position, BT_Creature.m_scanRange, m_enemyLayerMask);
        if(colliders.Length > 0)
        {
            m_parent.m_parent.SetData("target", colliders[0].transform);

            m_transform.GetComponent<Creature>().m_currStat = Creature.Status.RUNNING;
            m_state = NodeState.SUCCESS;
            //Debug.Log("There is New Enemy");
            return m_state;
        }
            
        m_transform.GetComponent<Creature>().m_currStat = Creature.Status.OTHER;
        m_state = NodeState.FAILURE;
        return m_state;
    }
}
