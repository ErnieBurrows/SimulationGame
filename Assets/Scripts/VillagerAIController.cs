using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class VillagerAIController : MonoBehaviour
{
    // Todo: Find the closest needed resource that has resources in it.
    // Todo: Figure out how running low/ out of player needs will affect the player.
    [SerializeField] VillagerNeedsController needsController;

    private List<GameObject> waterObjects = new List<GameObject>();

    private NavMeshAgent agent;


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
        Debug.Log("Damn I am Thirsty");

        agent.SetDestination(waterObjects.First().transform.position);
    }

    private void HandleOnHungry()
    {
        Debug.Log("Shit I am Hungry");
    }

      private void HandleOnFull()
    {
        Debug.Log("Yummy I am now a full boy :)");
    }

    private void HandleOnHydrated()
    {
        Debug.Log("I am now Hydrated!");
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
