using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Organ
{
    UNKNOWN,
    EYES,
    ARMS,
    LEGS,
    EARS,
    CHEST,
    SKIN,
    BRAIN,
    BONE
}

public class Body : MonoBehaviour
{
    public void Awake()
    {
        m_bodyParts = new Dictionary<Organ, BodyPart>();
        m_tall = 10.0f;
        m_totalEnergyConsume = 10.0f;
        m_weight = 10.0f;
        m_mass = 1.0f;
    }

    public void SetValues()
    {

        // Set weight
        m_mass = 0.0f;
        Legs leg = ((Legs)m_bodyParts[Organ.LEGS]);
        m_mass += leg.m_length * leg.m_width;
        Chest chest = ((Chest)m_bodyParts[Organ.CHEST]);
        m_mass += chest.m_length * chest.m_width;
        m_weight = m_mass * Physics.gravity.y * -1.0f;

        // Set energy Consume
        m_totalEnergyConsume = 0.0f;
        foreach (var bp in m_bodyParts)
        {
            m_totalEnergyConsume += bp.Value.EnergyConsume();
        }

        // Set accel
        m_accel = m_mass /*/ m_totalEnergyConsume*/;

        // Set force
        m_force = m_accel * m_mass;

        Vector3 originObjSize = gameObject.GetComponentInChildren<Renderer>().bounds.size;
        // Set body parts Length
        float legLength = leg.m_length;
        foreach (Transform legtrans in m_legTrans)
        {
            legtrans.localScale *= legLength;
        }

        // Set size and reset the renderer pos
        Vector3 objSize = gameObject.GetComponentInChildren<Renderer>().bounds.size;
        m_tall = objSize.y / originObjSize.y;
        m_boneTrans.localPosition =
            new Vector3(m_boneTrans.localPosition.x,/*  (m_tall/ 2.0f)*/0.5f, m_boneTrans.localPosition.z);

        Brain brain = ((Brain)m_bodyParts[Organ.BRAIN]);
        m_hunger = m_mass * m_totalEnergyConsume;
        m_hungryRatio = brain.m_hungryRatio;
        m_usualSpeedRate = brain.m_usualSpeedRate;
        m_sneakSpeedRate = brain.m_sneakSpeedRate;
    }
    public Dictionary<Organ, BodyPart> m_bodyParts;
    public float m_totalEnergyConsume;

    //public List<Transform> m_frontLegTrans;
    //public List<Transform> m_backLegTrans;
    public Transform m_boneTrans;
    public List<Transform> m_legTrans;
    public float m_force;
    public float m_mass;
    public float m_weight;
    public float m_accel;
    public float m_tall;
    public float m_hunger;
    public float m_hungryRatio;
    public float m_usualSpeedRate;
    public float m_sneakSpeedRate;
}
public class BodyPart
{
    public virtual void Initialize(List<bool> gene_) { }
    public float EnergyConsume()
    {
        return m_energyConsume;
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
    
    public float Translation_CountF(List<bool> gene_)
    {
        float digit = 0;

        foreach (bool gene in gene_)
        {
            digit += gene ? 1 : 0;
        }

        return digit;
    }

    public float m_energyConsume = 0.0f;
}

public class Eyes : BodyPart
{
    public override void Initialize(List<bool> gene_)
    {
        float digit = Translation_CountF(gene_);
        m_maxSight = digit * 30.0f;

        m_energyConsume = m_maxSight;
    }

    public float m_maxSight;
    public float m_angle;
}
public class Arms : BodyPart
{
    public override void Initialize(List<bool> gene_)
    {

    }

    public float m_length;
    public float m_width;
}


public class Legs : BodyPart
{
    public override void Initialize(List<bool> gene_)
    {
        float totalLength = gene_.Count;
        float digit = Translation_CountF(gene_);

        m_length = 1.0f + (digit * 2.0f  / totalLength);
    }

    public float m_length = 1.0f;
    public float m_width = 1.0f;
}

public class Chest : BodyPart
{
    public override void Initialize(List<bool> gene_)
    {
        float totalLength = gene_.Count;
        float digit = Translation_CountF(gene_);

        m_length = 0.5f + digit / totalLength;
        m_width = 1.0f;
    }

    public float m_length;
    public float m_width;
}

public class Ears : BodyPart
{
    public override void Initialize(List<bool> gene_)
    {

    }

    public float m_size;
    public float m_range;
    public float m_angle;
}

public class Skin : BodyPart
{
    public override void Initialize(List<bool> gene_)
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

        m_color = geneColor;
    }

    public float m_thick;
    public Color m_color;
}

public class Brain : BodyPart
{
    public override void Initialize(List<bool> gene_)
    {
        float digit = Translation_CountF(gene_);
        m_runSight = digit / (float)gene_.Count;

        m_energyConsume = m_runSight;
        // magic numbers. just do this again from gene
        m_hungryRatio = m_runSight / (float)gene_.Count;
        m_usualSpeedRate = m_runSight / (float)gene_.Count;
        m_sneakSpeedRate = m_runSight / (float)gene_.Count / 2.0f;
    }

    public float m_runSight;
    public float m_runLimit;
    public float m_hungryRatio;
    public float m_usualSpeedRate;
    public float m_sneakSpeedRate;
}

public class Bone : BodyPart
{
    public override void Initialize(List<bool> gene_)
    {

    }

    public float m_density;
}

