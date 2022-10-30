using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BHTree : MonoBehaviour
{
    protected Node m_root = null;
    public GameObject m_owner;
    public List<Vector3> m_foodLocation = new List<Vector3>();

    //protected void Start()
    //{
    //    m_owner = gameObject;
    //    m_foodLocation = m_owner.GetComponent<Creature>().m_foodLocation;
    //    m_root = SetupTree();
    //}

    // Update is called once per frame
    void Update()
    {
        if(m_root != null)
        {
            m_root.Evaluate();
        }
    }
    protected abstract Node SetupTree();
}
