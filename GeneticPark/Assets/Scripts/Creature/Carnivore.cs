using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carnivore : Creature
{
    public override void isDead()
    {
        // now, this is meat
        gameObject.layer = 9; // meat

        m_doDestroy = true;

        GetComponent<BT_Carnivore>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        // Set color gray
        m_renderer.GetPropertyBlock(m_propBlock);
        m_propBlock.SetColor("_Color", Color.gray);
        m_renderer.SetPropertyBlock(m_propBlock);
        // remove from child array
        m_worldLevelSetting.m_children.Remove(gameObject);
    }
    public override void Eat(GameObject food_)
    {
        float dt = Time.deltaTime;
        if (m_foodAmount < m_foodCapacity)
            m_foodAmount += dt *
                m_body.m_totalEnergyConsume * 0.1f; //magic number. please set properly

        food_.GetComponent<Creature>().m_deadTimeCount += dt;
    }
}
