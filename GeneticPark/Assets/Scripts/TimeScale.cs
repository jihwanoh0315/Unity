using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TimeScale : MonoBehaviour
{
    public GameObject levelSetting;
    public float m_timeScale = 1.0f;
    public int m_mutantProb = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        Time.timeScale = m_timeScale;

        //List<GameObject> children = levelSetting.GetComponent<LevelSetting>().m_children;
        //foreach (GameObject child in children)
        //{
        //    child.GetComponent<NavMeshAgent>().
        //}
        
    }
    public void MutantProb()
    {
        levelSetting.GetComponent<LevelSetting>().m_mutantProb = m_mutantProb;
    }
}
