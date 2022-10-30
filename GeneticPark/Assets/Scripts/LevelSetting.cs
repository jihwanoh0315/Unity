using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class LevelSetting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_creaturePrefab;
    public GameObject m_selectedCreature;
    public List<GameObject> m_children = new List<GameObject>();
    public List<GameObject> m_parent = new List<GameObject>();
    public List<GameObject> m_wolves = new List<GameObject>();

    public TextMeshProUGUI m_parentText;
    public TextMeshProUGUI m_childText;
    public TextMeshProUGUI m_genChildCount;

    public Camera m_camera;

    public int m_generation = 1;
    public int m_mutantProb = 1;
    public bool m_autoGenMode = false;
    public bool m_parentGenClicked = false;
    public bool m_childGenClicked = false;

    // Variables for the outline
    Material m_outlineShader;
    //Material m_creatureShader;
    Renderer m_renderers;
    List<Material> m_materialList = new List<Material>();

    void Start()
    {
        m_outlineShader = new Material(Shader.Find("Custom/Outline"));
        m_generation = 1;
        //m_creatureShader = new Material(Shader.Find("Custom/CreatureSurface"));
    }

    // Update is called once per frame
    void Update()
    {
        SelectCreature();

        m_genChildCount.text =
            "Generation : " + System.Convert.ToString(m_generation + "\n")
            + "ChildNum : " + System.Convert.ToString(m_children.Count);

        if (!m_selectedCreature)
            m_childText.text = "Selected Creature";

        if(m_autoGenMode && m_childGenClicked)
        {
            if(m_children.Count >= 2 &&
                m_children.Count < 5)
            {
                GenNextGeneration(true);
            }
            if(m_children.Count < 2)
                GenNextGeneration(false);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void SetParentText()
    {
        m_parentText.text = "".ToString();
        foreach (GameObject par in m_parent)
        {
            m_parentText.text += par.GetComponent<Creature>().GeneInfoString();
        }
    }

    public void SelectCreature()
    {
        // Check Mouse left click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Creature")
                {
                    if (m_selectedCreature)
                    {
                        Renderer renderer = m_selectedCreature.GetComponentInChildren<Renderer>();

                        m_materialList.Clear();
                        m_materialList.AddRange(renderer.sharedMaterials);
                        m_materialList.Remove(m_outlineShader);

                        renderer.materials = m_materialList.ToArray();
                    }

                    m_selectedCreature = hit.collider.gameObject;

                    m_renderers = m_selectedCreature.GetComponentInChildren<Renderer>();

                    m_materialList.Clear();
                    m_materialList.AddRange(m_renderers.sharedMaterials);
                    m_materialList.Add(m_outlineShader);

                    m_renderers.materials = m_materialList.ToArray();

                    m_childText.text = m_selectedCreature.GetComponent<Creature>().GeneInfoString();
                }
            }
        }
    } // SelectCreature() end

    /****************************************************************************
        \Function Name : CreateCreature()
        \Output : 
          GameObject - creature created
        \Role : 
          Create the creature in the game - just generate. not with any position

    *****************************************************************************/
    GameObject CreateCreature()
    {
        GameObject creature = Instantiate(m_creaturePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //creature.GetComponent<Creature>().SetGeneRandom(3);
        creature.GetComponent<Creature>().SetBasicGeneRandom();
        creature.GetComponent<Creature>().m_body.m_totalEnergyConsume = 0.0f;
        creature.GetComponent<Creature>().SetScaleAndColor();

        return creature;
    }
    /****************************************************************************
    \Function Name : SetParent()

    \Role : 
      Set parent from child exist

    *****************************************************************************/
    public void SetParent()
    {
        // clear parent list
        if (m_parent.Count > 0)
        {
            foreach (GameObject par in m_parent)
            {
                Destroy(par);
            }
            m_parent.Clear();
        }

        float xPosRatio = 40.0f / (m_children.Count - 1);
        for (int i = 0; i < m_children.Count; ++i)
        {
            GameObject currParent = m_children[i];
            currParent.name = "Parent " + (i + 1).ToString();
            currParent.layer = 0;
            currParent.GetComponent<NavMeshAgent>().enabled = false;
            currParent.GetComponent<BT_Creature>().enabled = false;
            currParent.GetComponent<Creature>().m_hunger = 10.0f;
            currParent.GetComponent<Creature>().m_body.m_totalEnergyConsume = 0.0f;

            m_children[i].transform.position = new Vector3(-20.0f + xPosRatio * i, m_children[i].transform.localScale.y * 0.5f, -65);
            m_children[i].transform.rotation = Quaternion.identity;
            m_parent.Add(currParent);
        }
        m_children.Clear();
        SetParentText();
    }
    /****************************************************************************
    \Function Name : SetRandomParent()
    \Input : 
        parentCount_ - number of parent to create
    \Role : 
      Set random parent with count

    *****************************************************************************/
    public void SetRandomParent(int parentCount_)
    {
        // clear parent list
        if (m_parent.Count > 0)
        {
            foreach (GameObject par in m_parent)
            {
                Destroy(par);
            }
            m_parent.Clear();
        }

        float xPosRatio = 20.0f / (parentCount_ - 1);
        for (int i = 0; i < parentCount_; ++i)
        {
            GameObject currParent = CreateCreature();
            currParent.name = "Parent " + (i + 1).ToString();
            currParent.layer = 0;
            currParent.GetComponent<NavMeshAgent>().enabled = false;
            currParent.GetComponent<BT_Creature>().enabled = false;
            currParent.GetComponent<Creature>().m_hunger = 10.0f;
            currParent.GetComponent<Creature>().m_body.m_totalEnergyConsume = 0.0f;
            currParent.transform.position = new Vector3(-10.0f + xPosRatio * i, currParent.transform.localScale.y * 0.5f, -65);
            m_parent.Add(currParent);
        }
        SetParentText();
    }


    /****************************************************************************
    \Function Name : CreateChild()
    \Input : 
        parentCount_ - number of parent to create
    \Role : 
      Set random parent with count

    *****************************************************************************/
    public void CreateChild()
    {
        m_wolves[0].transform.position = new Vector3(-100, 2, Random.Range(-10, 45));
        m_wolves[1].transform.position = new Vector3(-100, 2, Random.Range(50, 90));
        m_wolves[2].transform.position = new Vector3(130, 2, Random.Range(-10, 90));

        // can do this when parent more than 2
        if (m_parent.Count < 2)
        {
            return;
        }

        // clear the current child
        if(m_children.Count > 0)
        {
            for (int i = 0; i < m_children.Count; ++i)
            {
                Destroy(m_children[i]);
            }
            m_children.Clear();
        }
        // Create child
        int childCount = 1;
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                int parent1Num = Random.Range(0, m_parent.Count);
                int parent2Num = 0;

                while (parent1Num == parent2Num)
                    parent2Num = Random.Range(0, m_parent.Count);

                Creature parent1 = m_parent[parent1Num].GetComponent<Creature>();
                Creature parent2 = m_parent[parent2Num].GetComponent<Creature>();

                GameObject creature = Instantiate(m_creaturePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                creature.GetComponent<Creature>().SetGene(parent1.Reproduction(parent2, m_mutantProb));
                creature.GetComponent<Creature>().SetScaleAndColor();
                creature.name = "Child " + childCount.ToString();
                ++childCount;

                creature.transform.position = new Vector3(-40.0f + 20.0f * i, creature.transform.localScale.y * 0.5f, -40.0f + 20.0f * j);
                m_children.Add(creature);
            }// loop j end
        } // loop i end
    }

    public void GenNextGeneration(bool newGen_)
    {
        if(newGen_)
        {
            ++m_generation;
            SetParent();
        }
        CreateChild();
    }
        
}
