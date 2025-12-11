using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum WorkEthic
{
    Extreme,
    Average,
    Bad
}

[RequireComponent(typeof(VillagerNeedsController))]
public class VillagerAIController : SimulatableBehaviour
{
    private readonly List<Resource> waterSources = new List<Resource>();
    private Resource currentTarget = null;
    private bool isGoingToWater = false;

    // References
    private VillagerNeedsController needsController;
    private NavMeshAgent agent;

    private void Awake()
    {
        needsController = GetComponent<VillagerNeedsController>();
        agent = GetComponent<NavMeshAgent>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        if (needsController != null)
        {
            needsController.OnHungry += HandleOnHungry;
            needsController.OnThirsty += HandleOnThirsty;
            needsController.OnHydrated += HandleOnHydrated;
            needsController.OnFull += HandleOnFull;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (needsController != null)
        {
            needsController.OnHungry -= HandleOnHungry;
            needsController.OnThirsty -= HandleOnThirsty;
            needsController.OnHydrated -= HandleOnHydrated;
            needsController.OnFull -= HandleOnFull;
        }
    }

    private void Start()
    {
        foreach (var res in FindObjectsByType<Resource>(FindObjectsSortMode.None))
        {
            waterSources.Add(res);
        }        
    }

    public override void Simulate(float dt)
    {
        HandleWaterArrival();
    }

    private void HandleOnThirsty() 
    { 
        ChooseBestWaterSource(); 
    }

    private void HandleOnHungry()
    {
        ChooseBestWaterSource();
    }
    private void HandleOnFull()
    {
        WanderRandomly(); 
    }
    private void HandleOnHydrated() 
    {
        WanderRandomly(); 
    }

    private void ChooseBestWaterSource()
    {
        if (agent == null || waterSources.Count == 0)
            return;

        Resource best = null;
        float bestScore = float.MinValue;

        float need = needsController.Needs.maxThirst - needsController.Needs.thirst;

        foreach (var res in waterSources)
        {
            if (res == null || res.currentAmount <= 0) continue;

            float dist = Vector3.Distance(transform.position, res.transform.position);
            float score = res.currentAmount - dist * 0.1f;

            if (score > bestScore)
            {
                bestScore = score;
                best = res;
            }
        }

        if (best != null)
        {
            agent.SetDestination(best.transform.position);
        }
    }


    private void HandleWaterArrival()
    {
        if (!isGoingToWater || currentTarget == null)
            return;

        // Wait until navmesh reaches the target
        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // DRINKING HAPPENS HERE
            DrinkFromResource();

            // Reset state
            isGoingToWater = false;
            currentTarget = null;

            // Wander afterward
            WanderRandomly();
        }
    }

    private void DrinkFromResource()
    {
        float maxDrink = needsController.Needs.maxThirst - needsController.Needs.thirst;
        if (maxDrink <= 0) return;

        float amount = currentTarget.Withdraw(maxDrink);
        needsController.AddWater(amount); // THIS TRIGGERS UI UPDATE!
    }

    private void WanderRandomly()
    {
        if (agent == null) return;

        Vector3 destination;

        // Try up to N times to find a non-water location
        for (int i = 0; i < 10; i++)
        {
            destination = RandomNavmeshLocation(15f);

            if (!IsPointInWater(destination))
            {
                agent.SetDestination(destination);
                return;
            }
        }

        // Fallback: last resort, just move a little bit away from current position
        destination = transform.position + (transform.forward * 3f);
        agent.SetDestination(destination);
    }


    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomPos = Random.insideUnitSphere * radius + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
            return hit.position;
        return transform.position;
    }


    // HELPER SCRIPTS
   private bool IsPointInWater(Vector3 point)
    {
        Collider[] hits = Physics.OverlapSphere(point, 0.25f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Water"))
                return true;
        }

        return false;
    }

}
