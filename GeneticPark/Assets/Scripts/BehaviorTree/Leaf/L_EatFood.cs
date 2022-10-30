using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_EatFood : Node
{
    private Creature m_creature;
    private GameObject m_lastFood;

    public L_EatFood(Transform transform_)
    {
        m_creature = transform_.GetComponent<Creature>();
    }

    public override NodeState Evaluate()
    {
        GameObject food = (GameObject)GetData("food");
        if (m_lastFood != food)
        {
            m_lastFood = food;
        }

        m_creature.Eat(food);

        m_state = NodeState.RUNNING;
        return m_state;
    }
}
