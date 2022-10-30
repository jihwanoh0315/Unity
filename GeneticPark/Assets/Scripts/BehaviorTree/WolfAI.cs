using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    private static int m_creatureLayerMask = 1 << 7;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, 1000.0f, m_creatureLayerMask);

        // Follow
        if (colliders.Length > 0)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            Collider toChase = null;
            foreach (Collider collider in colliders)
            {
                Transform potentialTarget = collider.transform;
                if (potentialTarget.gameObject.GetComponent<Creature>().m_doDestroy)
                    continue;
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                    toChase = collider;
                }
            }
            Vector3 tarPos = new Vector3(0.0f, 0.0f, 0.0f);
            if (bestTarget)
            {
                tarPos = bestTarget.position;
                GetComponent<NavMeshAgent>().SetDestination(tarPos);
            }

            if (toChase)
            {
                float targetScale = toChase.gameObject.transform.localScale.x;
                if (closestDistanceSqr < targetScale * targetScale * 2.25f)
                {
                    bestTarget.gameObject.GetComponent<Creature>().isDead();
                }
            }
        }
    }
}
