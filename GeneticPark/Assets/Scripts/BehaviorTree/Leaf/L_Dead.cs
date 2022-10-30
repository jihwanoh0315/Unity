using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Dead : Node
{
    private Transform m_transform;

    public L_Dead(Transform transform_)
    {
        m_transform = transform_;
    }

    public override NodeState Evaluate()
    {
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
