using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavClickPosition : MonoBehaviour
{
    public Camera m_camera;
    public NavMeshAgent m_agent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check Mouse right click
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                m_agent.SetDestination(hit.point);
            }
        }
    }
}
