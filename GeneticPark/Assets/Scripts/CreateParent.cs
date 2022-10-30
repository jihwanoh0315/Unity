using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateParent : MonoBehaviour
{
    public GameObject obj;
    public GameObject levelSetting;

    public void OnClicked()
    {
        levelSetting.GetComponent<LevelSetting>().SetRandomParent(2);
        levelSetting.GetComponent<LevelSetting>().m_parentGenClicked = true;
        levelSetting.GetComponent<LevelSetting>().m_childGenClicked = false;
    }
    public void OnApplicationPause(bool pause)
    {
        
    }
    public void Pause()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }
}
