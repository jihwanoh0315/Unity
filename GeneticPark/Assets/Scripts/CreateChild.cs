using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChild : MonoBehaviour
{
    public GameObject levelSetting;

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
        if(levelSetting.GetComponent<LevelSetting>().m_parentGenClicked)
        {
            levelSetting.GetComponent<LevelSetting>().CreateChild();
            levelSetting.GetComponent<LevelSetting>().m_childGenClicked = true;
            levelSetting.GetComponent<LevelSetting>().m_generation = 1;
        }
    }
}
