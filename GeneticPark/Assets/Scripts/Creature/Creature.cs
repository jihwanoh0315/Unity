using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// See the BodyPart.cs
public class Exon // part of gene with info
{
    public Organ m_organID = Organ.UNKNOWN;
    public List<bool> m_info;
    public Exon()
    {
        m_info = new List<bool>();
    }
    public Exon(List<bool> info_)
    {
        m_info = new List<bool>(info_);
    }
    public Exon(Organ organ_, List<bool> info_)
    {
        m_organID = organ_;
        m_info = new List<bool>(info_);
    }
    public void SetGene(List<bool> param_)
    {
        m_info = param_;
    }
    public int Count()
    {
        return m_info.Count;
    }
}

public class Creature : MonoBehaviour
{
    public enum Status
    {
        RUNNING,
        OTHER
    }

    public enum Type
    {
        CARNIVORE,
        HERBIVORE,
        OMNIVORE
    }

    public Body m_body;
    public List<Exon> m_gene = new List<Exon>();
    public List<Vector3> m_foodLocation = new List<Vector3>();
    public Status m_currStat;
    public Type m_type;

    // variables for rendering - color
    private Renderer m_renderer;
    private MaterialPropertyBlock m_propBlock;
    private NavMeshAgent m_navAgent;
    public LevelSetting m_worldLevelSetting;

    // Values for Behavior Tree
    // Speed
    public float m_maxSpeed;
    public float m_usualSpeedRate; // ~1.0f
    public float m_sneakSpeedRate; // ~1.0f

    // Hunger
    public float m_maxHunger;
    public float m_hungryRate; // ~1.0f
    public float m_hunger;

    // Force
    public float m_accel;
    public float m_force;

    // Health
    public float m_health;

    // Time untill Dead body gone
    public float m_deadTime; 
    private float m_deadTimeCount = 0.0f; // +dt;
    
    public bool m_doDestroy;

    /****************************************************
     * 
     * Inheritance
     * 
     * ***************************************************/
    List<Exon> GetGamete()
    {
        List<Exon> gamete = new List<Exon>();
        foreach (Exon gene in m_gene)
        {
            int digit = 0;
            List<bool> gameteBlock = new List<bool>();
            while (gene.Count() > digit)
            {
                //Debug.Log("Curr Gene : " + System.Convert.ToString(geneCopy % 4 + 4, 2) );

                int pick = Random.Range(0, 10);
                if (pick < 5)
                {
                    gameteBlock.Add(gene.m_info[digit]);
                }
                else
                {
                    gameteBlock.Add(gene.m_info[digit + 1]);
                }
                digit += 2;
            }
            gamete.Add(new Exon(gene.m_organID, gameteBlock));
        }

        //Debug.Log(digit);
        return gamete;
    }

    public List<Exon> Reproduction(Creature mate_, int mutantRate_)
    {
        List<Exon> ownGene = GetGamete();
        List<Exon> mateGene = mate_.GetGamete();

        //Debug.Log("own Gene : " + System.Convert.ToString(ownGene, 2) + "\n" +
        //    "mate Gene : " + System.Convert.ToString(mateGene, 2));

        List<Exon> childGene = new List<Exon>();
        if (ownGene.Count != mateGene.Count)
            return childGene;

        for (int i = 0; i < ownGene.Count; ++i)
        {
            List<bool> childGeneBlock = new List<bool>();
            int mutantPossible = Random.Range(0, 100);
            if (mutantPossible < mutantRate_)
            {
                int changeDigit = Random.Range(0, ownGene[i].m_info.Count);
                ownGene[i].m_info[changeDigit] = !ownGene[i].m_info[changeDigit];
            }
            mutantPossible = Random.Range(0, 100);
            if (mutantPossible < mutantRate_)
            {
                int changeDigit = Random.Range(0, mateGene[i].m_info.Count);
                mateGene[i].m_info[changeDigit] = !mateGene[i].m_info[changeDigit];
            }

            for (int j = 0; j < ownGene[i].Count(); ++j)
            {
                childGeneBlock.Add(ownGene[i].m_info[j]);
                childGeneBlock.Add(mateGene[i].m_info[j]);
            }

            childGene.Add(new Exon(ownGene[i].m_organID, childGeneBlock));
        }

        return childGene;
    }
    public List<bool> SetGeneRandomList(int size_ = 10, int ratio_ = 5)
    {
        List<bool> newList = new List<bool>();

        for (int i = 0; i < size_; ++i)
        {
            newList.Add(Random.Range(0, 10) < ratio_ ? true : false);
        }
        return newList;
    }

    public void SetBasicGeneRandom()
    {
        // EYES
        List<bool> eyesList = SetGeneRandomList();
        m_gene.Add(new Exon(Organ.EYES, eyesList));

        Eyes newEyes = new Eyes();
        newEyes.Initialize(eyesList);
        m_body.m_bodyParts.Add(Organ.EYES, newEyes);

        // BRAIN
        List<bool> brainList = SetGeneRandomList();
        m_gene.Add(new Exon(Organ.BRAIN, brainList));

        Brain newBrain = new Brain();
        newBrain.Initialize(brainList);
        m_body.m_bodyParts.Add(Organ.BRAIN, newBrain);

        // SKIN
        List<bool> skinList = SetGeneRandomList(24);
        m_gene.Add(new Exon(Organ.SKIN, skinList));

        Skin newSkin = new Skin();
        newSkin.Initialize(skinList);
        m_body.m_bodyParts.Add(Organ.SKIN, newSkin);

        // LEGS
        List<bool> legList = SetGeneRandomList();
        m_gene.Add(new Exon(Organ.LEGS, legList));

        Legs newLegs = new Legs();
        newLegs.Initialize(legList);
        m_body.m_bodyParts.Add(Organ.LEGS, newLegs);

        // CHEST
        List<bool> chestList = SetGeneRandomList();
        m_gene.Add(new Exon(Organ.CHEST, chestList));

        Chest newChest = new Chest();
        newChest.Initialize(chestList);
        m_body.m_bodyParts.Add(Organ.CHEST, newChest);

        // DIGESTIVEORGAN
        List<bool> digList = SetGeneRandomList(32);
        m_gene.Add(new Exon(Organ.DIGESTIVEORGAN, digList));

        DigestiveOrgan newDigest = new DigestiveOrgan();
        newDigest.Initialize(digList);
        m_body.m_bodyParts.Add(Organ.DIGESTIVEORGAN, newDigest);
    }

    public void SetGeneRandom(int size_)
    {
        int geneLength;
        if (size_ < 3)
        {
            geneLength = 3;
        }
        else
            geneLength = size_;

        for (int i = 0; i < geneLength; ++i)
        {
            List<bool> geneBlock = new List<bool>();
            if (i == 1)
            {//Color
                for (int j = 0; j < 8; ++j)
                {
                    geneBlock.Add(Random.Range(0, 10)
                        < 5 ? true : false);
                }
                for (int j = 0; j < 16; ++j)
                {
                    geneBlock.Add(false);
                }
            }
            else
            {
                for (int j = 0; j < 16; ++j)
                {
                    geneBlock.Add(Random.Range(0, 10)
                        < 5 ? true : false);
                }
            }

            Exon toAdd = new Exon();
            toAdd.SetGene(geneBlock);
            m_gene.Add(toAdd);
        }
    }
    public void SetGene(List<Exon> gene_)
    {
        m_gene.Clear();
        m_gene = new List<Exon>(gene_);

        foreach (Exon currEx in m_gene)
        {
            switch (currEx.m_organID)
            {
                case Organ.UNKNOWN:
                    break;
                case Organ.EYES:
                    Eyes newEyes = new Eyes();
                    newEyes.Initialize(currEx.m_info);
                    m_body.m_bodyParts.Add(Organ.EYES, newEyes);
                    break;
                case Organ.ARMS:
                    break;
                case Organ.LEGS:
                    Legs newLegs = new Legs();
                    newLegs.Initialize(currEx.m_info);
                    m_body.m_bodyParts.Add(Organ.LEGS, newLegs);
                    break;
                case Organ.EARS:
                    break;
                case Organ.CHEST:
                    Chest newChest = new Chest();
                    newChest.Initialize(currEx.m_info);
                    m_body.m_bodyParts.Add(Organ.CHEST, newChest);
                    break;
                case Organ.SKIN:
                    Skin newSkin = new Skin();
                    newSkin.Initialize(currEx.m_info);
                    m_body.m_bodyParts.Add(Organ.SKIN, newSkin);
                    break;
                case Organ.BRAIN:
                    Brain newBrain = new Brain();
                    newBrain.Initialize(currEx.m_info);
                    m_body.m_bodyParts.Add(Organ.BRAIN, newBrain);
                    break;
                case Organ.DIGESTIVEORGAN:
                    DigestiveOrgan newDigest = new DigestiveOrgan();
                    newDigest.Initialize(currEx.m_info);
                    m_body.m_bodyParts.Add(Organ.DIGESTIVEORGAN, newDigest);
                    break;
                default:
                    break;
            }
        }
    }



    /***************************************
     * 
     * This count the number of 1 in the gene
     * 
     ***************************************/
    public int Translation_Count(List<bool> gene_)
    {
        int digit = 0;

        foreach (bool gene in gene_)
        {
            digit += gene ? 1 : 0;
        }

        return digit;
    }

    public Color Translation_Color(List<bool> gene_)
    {
        int digit = 0;
        Color geneColor = Color.black;

        float colorValue = 255.0f;
        for (int i = 0; i < 24; ++i)
        {
            if (gene_[i])
                colorValue -= 1 << digit;

            ++digit;

            if (i == 7)
            {
                geneColor.r = colorValue / 255.0f;
                digit = 0;
                colorValue = 255.0f;
            }
            else if (i == 15)
            {
                geneColor.g = colorValue / 255.0f;
                digit = 0;
                colorValue = 255.0f;
            }
            else if (i == 23)
            {
                geneColor.b = colorValue / 255.0f;
            }
        }

        return geneColor;
    }

    public void SetScaleAndColor()
    {
        // Scale
        float scaleCount = (float)(Translation_Count(m_gene[0].m_info));
        float objSize = 1.0f + scaleCount;
        GetComponent<Transform>().localScale = new Vector3(objSize, objSize, objSize);
        m_navAgent.speed = 5.0f + 50.0f * (m_gene[0].m_info.Count - scaleCount) / m_gene[0].m_info.Count;

        // Usual Speed
        m_maxSpeed = 5.0f + 50.0f * (m_gene[0].m_info.Count - scaleCount) / m_gene[0].m_info.Count;
        m_usualSpeedRate = m_body.m_usualSpeedRate;
        m_navAgent.speed = m_maxSpeed * m_usualSpeedRate;

        // Sneak Speed
        m_sneakSpeedRate = m_body.m_sneakSpeedRate;

        // Color
        Color objColor = ((Skin)m_body.m_bodyParts[Organ.SKIN]).m_color;
        m_renderer.GetPropertyBlock(m_propBlock);
        m_propBlock.SetColor("_Color", objColor);
        m_renderer.SetPropertyBlock(m_propBlock);

        m_body.SetValues();

        m_accel = m_body.m_accel;
        m_force = m_body.m_force;

        m_maxHunger = m_body.m_hunger * 60.0f;
        m_hungryRate = m_body.m_hungryRatio * m_maxHunger;
        m_hunger = m_maxHunger;
        //m_navAgent.acceleration = m_accel * 2;
    }



    void Awake()
    {
        m_propBlock = new MaterialPropertyBlock();
        m_renderer = GetComponentInChildren<Renderer>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_worldLevelSetting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Vegitable");
        foreach (GameObject obj in objs)
        {
            m_foodLocation.Add(obj.transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_health = 100.0f;
        m_deadTime = m_health;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        if (m_doDestroy)
        {
            DoDestroy();
            return;
        }
        // When hungry
        if (m_currStat == Status.RUNNING)
        {
            m_hunger -= (dt * m_body.m_totalEnergyConsume) * 1.7f;
        }
        else
            m_hunger -= (dt * m_body.m_totalEnergyConsume);

        if (m_health < 0.0f)
            isDead();

        if (m_hunger < 0.0f)
        {
            m_health -= dt;
        }
    }

    public void isDead()
    {
        // now, this is meat
        int m_meatRange = 1 << 9;
        gameObject.layer = m_meatRange;

        m_doDestroy = true;

        GetComponent<BT_Creature>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        // Set color gray
        m_renderer.GetPropertyBlock(m_propBlock);
        m_propBlock.SetColor("_Color", Color.gray);
        m_renderer.SetPropertyBlock(m_propBlock);
        // remove from child array
        m_worldLevelSetting.m_children.Remove(gameObject);
    }
    void DoDestroy()
    {
        m_deadTimeCount += Time.deltaTime;
        if (m_deadTime < m_deadTimeCount)
            Destroy(gameObject);
    }

    public string GeneInfoString()
    {
        string toReturn = name + " Gene\n".ToString();
        for (int i = 0; i < m_gene.Count; ++i)
        {
            switch (i)
            {
                case 0:
                    toReturn += "Size : ";
                    break;
                case 1:
                    toReturn += "Color : ";
                    break;
                default:
                    toReturn += "??? : ";
                    break;
            }

            foreach (var gVal in m_gene[i].m_info)
            {
                toReturn += gVal ? "1" : "0";
            }
            toReturn += "\n".ToString();
        }
        toReturn += "\n".ToString();

        return toReturn;
    }

    public virtual void Eat(GameObject food_) { }
    public virtual void Digest() { }
}
