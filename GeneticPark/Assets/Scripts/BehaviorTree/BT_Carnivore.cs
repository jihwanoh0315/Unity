using System.Collections.Generic;

public class BT_Carnivore : BHTree
{
    public static float m_speed = 2.0f;
    public static float m_scanRange = 50.0f;
    // Start is called before the first frame update
    protected void Start()
    {
        m_owner = gameObject;
        m_foodLocation = m_owner.GetComponent<Creature>().m_foodLocation;
        m_root = SetupTree();
    }

    protected override Node SetupTree()
    {
        Node root = new C_Selector(new List<Node>
        {
            new C_Sequencer(new List<Node>
            {
                new L_IsHungry(transform),
                new C_Selector(new List<Node>
                {
                    // Find food
                    new C_Sequencer(new List<Node>
                    {
                        new L_FindingFood(transform, m_foodLocation),
                        new L_GoToFood(transform),
                        new L_EatFood(transform),
                    }),
                    // If no food, hunting!
                    new C_Sequencer(new List<Node>
                    {
                        new L_FindingPrey(transform),
                        // chase prey
                        new C_Selector(new List<Node> 
                        {
                            // Attack
                            new C_Sequencer(new List<Node>
                            {
                                new L_CanAttack(transform),
                                new L_Attack(transform),
                            }),
                            // if can't, Rush to prey
                            new C_Sequencer(new List<Node>
                            {
                                new L_CanRush(transform),
                                new L_Rush(transform),
                            }),
                            // if can't, sneak to the prey
                            new L_Sneaking(transform),
                        }),// chase end
                    }),//hunting end
                }),
            }),
            // If not hungry or prey, move around!
            new L_Wandering(transform),
            // put Sleep with random consequencer
        });
        return root;
    }
}
