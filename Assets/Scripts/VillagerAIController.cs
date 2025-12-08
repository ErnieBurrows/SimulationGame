using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum WorkEthic
{
    Extreme,
    Average,
    Bad
}

public class VillagerAIController : MonoBehaviour
{
    [SerializeField] VillagerNeedsController needsController;
    private List<GameObject> waterObjects = new List<GameObject>();
    private NavMeshAgent agent;

    public WorkEthic workEthic = WorkEthic.Average;


    private void OnEnable()
    {
        if(needsController != null)
        {
            needsController.OnHungry += HandleOnHungry;
            needsController.OnThirsty += HandleOnThirsty;
            needsController.OnHydrated += HandleOnHydrated;
            needsController.OnFull += HandleOnFull;
        }
    }

    private void OnDisable()
    {
        if(needsController != null)
        {
            needsController.OnHungry -= HandleOnHungry;
            needsController.OnThirsty -= HandleOnThirsty;
            needsController.OnHydrated -= HandleOnHydrated;
            needsController.OnFull -= HandleOnFull;
        }
    }

    private void Start()
    {
        foreach(Resource resource in FindObjectsByType<Resource>(FindObjectsSortMode.None))
        {
            waterObjects.Add(resource.gameObject);
        }

        agent = GetComponent<NavMeshAgent>();
    }

    private void HandleOnThirsty()
    {
        // Todo: Implement different type of decision making here.

        // Assign values based on both closeness as well as resource amount available
        // Make sure the building has enough water to fill us.
        // Else -> Lets try to go to the lake or something.

        Resource desiredResource = waterObjects.First().GetComponent<Resource>();

        Vector3 destination = waterObjects.First().transform.position;
        float originalDistance = Vector3.Distance(transform.position, destination);

        foreach (GameObject water in waterObjects)
        {
            Resource currentResource = water.GetComponent<Resource>();

            float newDistance = Vector3.Distance(transform.position, water.transform.position);

            // Does the selected resource have enough to satiate our character
            if (currentResource.currentAmount > (needsController.Needs.maxThirst - needsController.Needs.thirst))
            {
                desiredResource = water.GetComponent<Resource>();
            }

            if (newDistance < originalDistance)
            {
                destination = water.transform.position;
            }
        }


        agent.SetDestination(destination);
    }

    private void HandleOnHungry()
    {
       
    }

    private void HandleOnFull()
    {
       
    }

    private void HandleOnHydrated()
    {
        
        agent.SetDestination(RandomNavmeshLocation(15.0f));
    }

    public Vector3 RandomNavmeshLocation(float radius) 
    {
        Vector3 randomPosition = Random.insideUnitSphere * radius;
        randomPosition += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1)) 
            finalPosition = hit.position;            
        
        return finalPosition;
    }
   
}
