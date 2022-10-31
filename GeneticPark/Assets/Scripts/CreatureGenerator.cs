using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureGenerator : MonoBehaviour
{
    public int m_mutantProb = 1;

    //Start is called before the first frame update
    public GameObject m_carnivorePrefab;
    public GameObject m_harbivorePrefab;
    public GameObject m_omnivorePrefab;
    public GameObject m_selectedCreature;

    public Dictionary<int, GameObject> m_carnivoreParents = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> m_carnivores = new Dictionary<int, GameObject>();

    public Dictionary<int, GameObject> m_harbivoreParents = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> m_harbivores = new Dictionary<int, GameObject>();

    public Dictionary<int, GameObject> m_omnivoreParents = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> m_omnivores = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /**********************************************
     * 
     *  Carnivore
     * 
     * ***********************************************/

    // Set Parent
    public void GenerateCarnivoreParent()
    {
        // clear parent list
        if (m_carnivoreParents.Count > 0)
        {
            foreach (var item in m_carnivoreParents)
            {
                Destroy(item.Value);
            }
            m_carnivoreParents.Clear();
        }


        for (int i = 1; i < 2; ++i)
        {
            GameObject currParent = CreateCreature(m_carnivorePrefab);
            currParent.name = "CarnivoreParent " + (i + 1).ToString();
            currParent.layer = 0;
            currParent.GetComponent<NavMeshAgent>().enabled = false;
            currParent.GetComponent<BT_Carnivore>().enabled = false;
            currParent.GetComponent<Creature>().m_health = 100.0f;
            currParent.GetComponent<Creature>().m_hunger = 100.0f;
            currParent.GetComponent<Creature>().m_body.m_totalEnergyConsume = 0.0f;
            //currParent.transform.position = new Vector3(-10.0f + xPosRatio * i, currParent.transform.localScale.y * 0.5f, -65);

            m_carnivoreParents.Add(i, currParent);
        }
    }


    public void GenerateChild(GameObject mother_, GameObject father_, GameObject childObj_, int genNumber_)
    {
        // SetParent
        Creature parent1 = mother_.GetComponent<Creature>();
        Creature parent2 = father_.GetComponent<Creature>();

        //CreateCreature();
        for (int i = 1; i <= genNumber_; ++i)
        {
            GameObject creature = Instantiate(childObj_, new Vector3(0, 0, 0), Quaternion.identity);
            creature.GetComponent<Creature>().InitializeBodyWithGene(parent1.Reproduction(parent2, m_mutantProb));
            creature.GetComponent<Creature>().InitializeCreatureWithBody();
            creature.name = "Child " + i.ToString();
            // TODO : Set position with area;
            creature.transform.position = new Vector3();
        }
    }

    public void GenerateHerbivores()
    {

    }

    public void GenerateOmnivores()
    {

    }

    GameObject CreateCreature(GameObject objToGen_)
    {
        GameObject creature = Instantiate(objToGen_, new Vector3(0, 0, 0), Quaternion.identity);

        return creature;
    }
}
